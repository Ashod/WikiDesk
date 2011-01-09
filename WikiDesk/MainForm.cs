namespace WikiDesk
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    using WebKit;

    using WeifenLuo.WinFormsUI.Docking;

    using WikiDesk.Core;
    using WikiDesk.Data;

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            browser_.Visible = true;
            browser_.Dock = DockStyle.Fill;
            browser_.Name = "browser";
            //browser.IsWebBrowserContextMenuEnabled = false;
            //browser.IsScriptingEnabled = false;
            browser_.BringToFront();
            browser_.ApplicationName = APPLICATION_NAME;
            browser_.DocumentTitleChanged += browser__DocumentTitleChanged;
            browser_.DocumentCompleted += browser__DocumentCompleted;
            browser_.Error += browser__Error;
            browser_.DownloadBegin += browser__DownloadBegin;
            browser_.NewWindowRequest += browser__NewWindowRequest;
            browser_.NewWindowCreated += browser__NewWindowCreated;
            browser_.Navigating += browser__Navigating;
            browser_.Navigated += browser__Navigated;
            browser_.DecideNavigationAction += browser__DecideNavigationAction;
            dockPanel_.Controls.Add(browser_);

            entriesMap_ = new Dictionary<string, Dictionary<string, PrefixMatchContainer<string>>>();

            btnBack.Enabled = false;
            btnForward.Enabled = false;
            btnStop.Enabled = false;
            btnGo.Enabled = false;
            cboLanguage.Enabled = false;

            tempFilename_ = Path.GetTempFileName().Replace(".tmp", ".html");
            tempFileUrl_ = "file:///" + tempFilename_.Replace('\\', '/');

            settings_ = Settings.Deserialize(CONFIG_FILENAME);

            fileCache_ = new FileCache(settings_.FileCacheFolder);

            OpenDatabase(settings_.DefaultDatabaseFilename);

            languages_ = LanguageCodes.Deserialize(settings_.LanguagesFilename);
            StoreWikiLanguages(languages_);

            domains_ = WikiDomains.Deserialize(settings_.DomainsFilename);
            StoreWikiDomains(domains_);

            currentDomain_ = domains_.FindByName(settings_.CurrentDomainName);
            if (currentDomain_ == null)
            {
                currentDomain_ = domains_.FindByName(settings_.DefaultDomainName);
                if (currentDomain_ != null)
                {
                    settings_.CurrentDomainName = settings_.DefaultDomainName;
                }
            }

            ShowAllLanguages();

            indexControl_ = new IndexControl(entriesMap_, BrowseWikiArticle);
            indexControl_.Show(dockPanel_, DockState.DockLeft);
            indexControl_.IsHidden = true;
        }

        private void ShowAllLanguages()
        {
            cboLanguage.BeginUpdate();
            int currentLanguageIndex = -1;
            try
            {
                cboLanguage.Items.Clear();
                foreach (WikiLanguage wikiLanguage in languages_.Languages)
                {
                    WikiArticleName name = new WikiArticleName(string.Empty, wikiLanguage);
                    cboLanguage.Items.Add(name);
                    if (settings_.CurrentLanguageCode == name.LanguageCode)
                    {
                        currentLanguageIndex = cboLanguage.Items.Count - 1;
                    }
                }
            }
            finally
            {
                cboLanguage.SelectedIndex = currentLanguageIndex;
                cboLanguage.Enabled = cboLanguage.Items.Count > 0;
                cboLanguage.EndUpdate();
            }
        }

        private void ShowArticleLanguages(string title, Dictionary<string, string> articleLanguageNames)
        {
            cboLanguage.BeginUpdate();
            try
            {
                cboLanguage.Items.Clear();

                // Add the current language first.
                WikiLanguage curWikiLanguage = languages_.Languages.Find(lang => settings_.CurrentLanguageCode == lang.Code);
                if (curWikiLanguage != null)
                {
                    WikiArticleName curName = new WikiArticleName(title, curWikiLanguage);
                    cboLanguage.Items.Add(curName);
                }

                foreach (KeyValuePair<string, string> pair in articleLanguageNames)
                {
                    string langCode = pair.Key;
                    WikiLanguage wikiLanguage = languages_.Languages.Find(lang => langCode == lang.Code);
                    if (wikiLanguage != null)
                    {
                        WikiArticleName name = new WikiArticleName(pair.Value, wikiLanguage);
                        cboLanguage.Items.Add(name);
                    }
                }

                cboLanguage.SelectedIndex = 0;
            }
            finally
            {
                cboLanguage.Enabled = cboLanguage.Items.Count > 0;
                cboLanguage.EndUpdate();
            }
        }

        private void StoreWikiDomains(WikiDomains domains)
        {
            domains.Serialize(settings_.DomainsFilename);

            foreach (WikiDomain wikiDomain in domains.Domains)
            {
                Domain domain = new Domain { Name = wikiDomain.Name };

                db_.UpdateInsertDomain(domain);
            }
        }

        private void StoreWikiLanguages(LanguageCodes langCodes)
        {
            langCodes.Serialize(settings_.LanguagesFilename);

            foreach (WikiLanguage language in langCodes.Languages)
            {
                Language lang = new Language
                    {
                        Code = language.Code,
                        Name = !string.IsNullOrEmpty(language.Name) ? language.Name : language.LocalName
                    };

                db_.UpdateInsertLanguage(lang);
            }
        }

        static string EncodeNonAsciiCharacters(IEnumerable<char> value)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in value)
            {
                if (c > 127)
                {
                    // This character is too big for ASCII
                    string encodedValue = "\\u" + ((int)c).ToString("x4");
                    sb.Append(encodedValue);
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        static string DecodeEncodedNonAsciiCharacters(string value)
        {
            return Regex.Replace(
                value,
                @"\\u([a-zA-Z0-9]{4})",
                m => ((char)int.Parse(m.Groups[1].Value, NumberStyles.HexNumber)).ToString());
        }

        #region Browser Events

        private bool browser__DecideNavigationAction(string url, string mainurl)
        {
            if (url.StartsWith(WIKI_PROTOCOL_STRING))
            {
                string title = url.Substring(WIKI_PROTOCOL_STRING.Length);
                title = title.Replace('/', '\\');
                title = DecodeEncodedNonAsciiCharacters(title);

                //TODO: What about pages with underscore?
                title = title.Replace('_', ' ');
                BrowseWikiArticle(currentDomain_, settings_.CurrentLanguageCode, title);

                // Handled, don't navigate.
                return false;
            }

            return true;
        }

        private void browser__Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            ChangePageTitle(browser_.Url, browser_.DocumentTitle ?? string.Empty);
        }

        private void browser__Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if ((browser_.Url != null) && (browser_.Url.ToString() != tempFileUrl_))
            {
                currentWikiPageName_ = null;
            }

            ChangePageTitle(browser_.Url, browser_.DocumentTitle ?? string.Empty);
        }

        private void browser__DocumentTitleChanged(object sender, EventArgs e)
        {
            ChangePageTitle(browser_.Url, browser_.DocumentTitle ?? string.Empty);
        }

        private void browser__NewWindowCreated(object sender, NewWindowCreatedEventArgs e)
        {
//            tabControl.TabPages.Add(new WebBrowserTabPage(args.WebKitBrowser, false));
        }

        private void browser__NewWindowRequest(object sender, NewWindowRequestEventArgs e)
        {
            //args.Cancel = (MessageBox.Show(args.Url, "Open new window?", MessageBoxButtons.YesNo) == DialogResult.No);
        }

        private void browser__DownloadBegin(object sender, FileDownloadBeginEventArgs e)
        {
            //DownloadForm frm = new DownloadForm(args.Download);
        }

        private void browser__Error(object sender, WebKitBrowserErrorEventArgs e)
        {
            browser_.DocumentText = "<html><head><title>Error</title></head><center><p>" + e.Description + "</p></center></html>";
        }

        private void browser__DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            ChangePageTitle(browser_.Url, browser_.DocumentTitle ?? string.Empty);

            btnBack.Enabled = browser_.CanGoBack;
            btnForward.Enabled = browser_.CanGoForward;
        }

        #endregion // Browser Events

        #region Browser Controls

        /// <summary>
        /// Updates the Navigation list and the window title.
        /// </summary>
        /// <param name="uri">The URI where we are.</param>
        /// <param name="name">The name of the page loaded.</param>
        private void ChangePageTitle(Uri uri, string name)
        {
            string url = currentWikiPageName_ ?? string.Empty;
            if (currentWikiPageName_ == null)
            {
                url = uri != null ? uri.ToString() : string.Empty;
            }

            // TODO: Save history.
            cboNavigate.Text = url;

            Text = string.Format("{0} - {1}", APPLICATION_NAME, name ?? string.Empty);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            browser_.GoBack();
            ActivateBrowser();
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            browser_.GoForward();
            ActivateBrowser();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            browser_.Reload();
            ActivateBrowser();
            //             browser_.Stop();
            //             ActivateBrowser();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            browser_.Navigate(cboNavigate.Text);
            ActivateBrowser();
        }

        #endregion // Browser Controls

        private void ActivateBrowser()
        {
            if (browser_.CanFocus)
            {
                browser_.Focus();
            }
        }

        private void LoadClick(object sender, EventArgs e)
        {
            string folder = "Z:\\"; //Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string dbPath = Path.Combine(folder, "wikidesk.db");
            using (Database db = new Database(dbPath))
            {
                Domain domain = db.GetDomain("Wikipedia");
                db.Load("Z:\\simplewiki-20100401-pages-articles.xml", domain.Id, "en", false);
            }
        }

        private void OpenDatabase(string dbPath)
        {
            db_ = new Database(dbPath);

            entriesMap_.Clear();
            foreach (Domain domain in db_.GetDomains())
            {
                Dictionary<string, PrefixMatchContainer<string>> langTitlesMap = new Dictionary<string, PrefixMatchContainer<string>>(8);

                foreach (Language language in db_.GetLanguages())
                {
                    IList<Page> pages = db_.GetPages(domain.Id, language.Id);
                    if (pages != null && pages.Count > 0)
                    {
                        PrefixMatchContainer<string> titles = new PrefixMatchContainer<string>();

                        foreach (Page page in pages)
                        {
                            titles.Add(page.Title, page.Title);
                        }

                        langTitlesMap.Add(language.Name, titles);
                    }
                }

                entriesMap_.Add(domain.Name, langTitlesMap);
            }

            //TODO: How should auto-complete work? Should we add a domain selector?
//            cboNavigate.AutoCompleteCustomSource = titlesMap_.AutoCompleteStringCollection;
        }

        private void OpenClick(object sender, EventArgs e)
        {
//             openFileDialog.CheckFileExists = true;
//             openFileDialog.ReadOnlyChecked = true;
//             openFileDialog.ShowReadOnly = false;
//             openFileDialog.Multiselect = false;
//             openFileDialog.DefaultExt = "db";
//             openFileDialog.Filter = "Sqlite database files (*.db)|*.db|All files (*.*)|*.*";
//             if (openFileDialog.ShowDialog(this) == DialogResult.OK)
//             {
//                 OpenDatabase(openFileDialog.FileName);
//             }

            string folder = "Z:\\"; //Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string dbPath = Path.Combine(folder, "wikidesk.db");
            OpenDatabase(dbPath);
        }

        private void Titles_SelectedIndexChanged(object sender, EventArgs e)
        {
            string title = cboNavigate.Text;
            NavigateTo(title);
        }

        private void NavigateTo(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            string urlUpper = url.ToUpperInvariant();
            if (urlUpper.StartsWith("HTTP://") ||
                urlUpper.StartsWith("HTTPS://") ||
                urlUpper.StartsWith("FILE://"))
            {
                if (url != tempFileUrl_)
                {
                    currentWikiPageName_ = null;
                }

                browser_.Navigate(url);
            }
            else
            {
                if (url.StartsWith(WIKI_PROTOCOL_STRING))
                {
                    url = url.Substring(WIKI_PROTOCOL_STRING.Length);
                }

                BrowseWikiArticle(currentDomain_, settings_.CurrentLanguageCode, url);
            }
        }

        /// <summary>
        /// Browse to a title given the domain name, language name and title.
        /// Typically called from a shortcut control, such as IndexControl or FavoritsControl.
        /// </summary>
        /// <param name="domainName">The domain name.</param>
        /// <param name="languageName">The language name.</param>
        /// <param name="title">The title of the entry.</param>
        private void BrowseWikiArticle(string domainName, string languageName, string title)
        {
            WikiDomain domain = domains_.FindByName(domainName);
            Language language = db_.GetLanguageByName(languageName);
            if (domain != null && language != null)
            {
                BrowseWikiArticle(domain, language.Code, title);
            }
        }

        private void BrowseWikiArticle(WikiDomain domain, string languageCode, string title)
        {
            currentWikiPageName_ = title;
            currentDomain_ = domain;
            settings_.CurrentDomainName = domain.Name;
            settings_.CurrentLanguageCode = languageCode;

            long domainId = db_.GetDomain(domain.Name).Id;
            Page page = db_.QueryPage(title, domainId, languageCode);
            if (page != null)
            {
                Revision rev = db_.QueryRevision(page.LastRevisionId);
                if (rev != null)
                {
                    ShowWikiPage(title, rev.Text);
                    return;
                }
            }

            if (settings_.AutoRetrieveMissing)
            {
                // Download and import from the web...
                page = ImportLivePage(title, domain, domainId, languageCode);
                if (page != null)
                {
                    Revision rev = db_.QueryRevision(page.LastRevisionId);
                    if (rev != null)
                    {
                        ShowWikiPage(title, rev.Text);
                    }
                }
            }
        }

        private Page ImportLivePage(string title, WikiDomain domain, long domainId, string languageCode)
        {
            Page page;
            title = title.Replace(' ', '_');
            string url = string.Concat("http://", languageCode, domain.ExportUrl, title);
            string pageXml = Download.DownloadPage(url);
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(pageXml)))
            {
                db_.ImportFromXml(ms, false, domainId, languageCode);
            }

            Language language = db_.GetLanguageByCode(languageCode);
            page = db_.QueryPage(title, domainId, language.Id);
            if (page != null)
            {
                Dictionary<string, PrefixMatchContainer<string>> langEntries;
                if (!entriesMap_.TryGetValue(domain.Name, out langEntries))
                {
                    langEntries = new Dictionary<string, PrefixMatchContainer<string>>();
                    entriesMap_.Add(domain.Name, langEntries);
                }

                PrefixMatchContainer<string> titles;
                if (!langEntries.TryGetValue(language.Name, out titles))
                {
                    titles = new PrefixMatchContainer<string>();
                    langEntries.Add(language.Name, titles);
                }

                titles.Add(page.Title, page.Title);
                indexControl_.UpdateListItems();
            }

            return page;
        }

        private void ShowWikiPage(string title, string text)
        {
            ShowArticleLanguages(title, Wiki2Html.ExtractLanguages(ref text));

            Wiki2Html wiki2Html = new Wiki2Html(new Configuration(), OnResolveWikiLinks, fileCache_);
            string html = wiki2Html.ConvertX(text);
            html = WrapInHtmlBody(title, html);

            using (FileStream fs = new FileStream(tempFilename_, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(html);
                fs.Write(bytes, 0, bytes.Length);
                fs.SetLength(bytes.Length);
            }

            NavigateTo(tempFileUrl_);
        }

        private static string OnResolveWikiLinks(string title, string languageCode)
        {
            //TODO: take the language code into consideration.
            title = title.Replace(' ', '_');
            return WIKI_PROTOCOL_STRING + EncodeNonAsciiCharacters(title);
        }

        private string WrapInHtmlBody(string title, string html)
        {
            StringBuilder sb = new StringBuilder(html.Length * 2);
            sb.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" ");
            sb.Append("\"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            //TODO: lang should be generated dynamically.
            sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\" lang=\"en\" dir=\"ltr\">");
            sb.Append("<head><title>").Append(title).Append("</title>");
            sb.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />");
            sb.Append("<style type=\"text/css\">");

            if (!string.IsNullOrEmpty(settings_.CssFilename))
            {
                sb.Append(File.ReadAllText(settings_.CssFilename));
            }

            sb.Append("</style></head>");
            sb.Append("<body class=\"mediawiki ltr ns-0 ns-subject page-");
            sb.Append(title.Replace(' ', '_'));
            sb.Append(" skin-vector\">");
            sb.Append("<div id=\"mw-page-base\" class=\"noprint\"></div>");
            sb.Append("<div id=\"mw-head-base\" class=\"noprint\"></div>");
            sb.Append("<div id=\"content\">");

            sb.Append("<h1 id=\"firstHeading\" class=\"firstHeading\">");
            sb.Append(title);
            sb.Append("</h1>");
    		sb.Append("<div id=\"bodyContent\">");
            sb.Append("<div id=\"siteSub\">From Wikipedia, the free encyclopedia</div>");

            sb.Append(html);

            sb.Append("</div></div>");
            sb.Append("</body></html>");

            return sb.ToString();
        }

        private void Navigation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Titles_SelectedIndexChanged(sender, e);
            }
        }

        private void stripNavigation_Resize(object sender, EventArgs e)
        {
            cboNavigate.Size = new Size(Width - 470, cboNavigate.Height);
        }

        private void cboLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLanguage.SelectedIndex >= 0)
            {
                WikiArticleName name = (WikiArticleName)cboLanguage.Items[cboLanguage.SelectedIndex];
                if (!string.IsNullOrEmpty(name.Name))
                {
                    if (name.LanguageCode != settings_.CurrentLanguageCode)
                    {
                        settings_.CurrentLanguageCode = name.LanguageCode;
                        BrowseWikiArticle(currentDomain_, name.LanguageCode, name.Name);
                    }
                }

                // Always change anyway.
                settings_.CurrentLanguageCode = name.LanguageCode;
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmImport_ == null)
            {
                frmImport_ = new ImportForm(db_, entriesMap_);
            }

            frmImport_.Show(this);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OptionsForm frmOptions = new OptionsForm())
            {
                frmOptions.ShowDialog(this);
            }
        }

        private void indexMenuItem__Click(object sender, EventArgs e)
        {
            indexControl_.IsHidden = !indexMenuItem_.Checked;
        }

        private void ExitClick(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            settings_.Serialize(CONFIG_FILENAME);
        }

        #region representation

        private Database db_;

        private readonly Settings settings_;

        private ImportForm frmImport_;

        /// <summary>
        /// The current wiki-page name/title.
        /// Valid only if we are on an internal wiki page.
        /// </summary>
        private string currentWikiPageName_;

        private readonly LanguageCodes languages_;
        private readonly WikiDomains domains_;
        private WikiDomain currentDomain_;

        private readonly WebKitBrowser browser_ = new WebKitBrowser();

        private readonly IndexControl indexControl_;

        /// <summary>
        /// All entries mapped as: Domain : Language : Title.
        /// </summary>
        private readonly Dictionary<string, Dictionary<string, PrefixMatchContainer<string>>> entriesMap_;

        private readonly IFileCache fileCache_;

        private readonly string tempFilename_;
        private readonly string tempFileUrl_;

        #endregion // representation

        #region constants

        private const string APPLICATION_NAME = "WikiDesk";
        private const string CONFIG_FILENAME = "WikiDesk.xml";

        private const string WIKI_PROTOCOL_STRING = "wiki://";

        #endregion // constants
    }
}
