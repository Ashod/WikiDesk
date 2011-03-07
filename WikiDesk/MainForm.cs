namespace WikiDesk
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    using ICSharpCode.SharpZipLib.BZip2;

    using WebKit;

    using WeifenLuo.WinFormsUI.Docking;

    using WikiDesk.Core;
    using WikiDesk.Data;

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // For now the user's data are kept next to the executable.
            //TODO: Move to user-specific data folder.
            userDataFolderPath_ = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (string.IsNullOrEmpty(userDataFolderPath_))
            {
                throw new ApplicationException("Invalid or missing user-data folder.");
            }

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

            settings_ = Settings.Deserialize(Path.Combine(userDataFolderPath_, CONFIG_FILENAME));

            fileCache_ = new FileCache(settings_.FileCacheFolder);

            OpenDatabase(settings_.DefaultDatabaseFilename);

            // Language.
            languages_ = LanguageCodes.Deserialize(Path.Combine(userDataFolderPath_, settings_.LanguagesFilename));
            StoreWikiLanguages(languages_);
            WikiLanguage wikiLanguage = languages_.Languages.Find(lang => settings_.CurrentLanguageCode == lang.Code) ??
                                        languages_.Languages.Find(lang => settings_.DefaultLanguageCode == lang.Code);

            if (wikiLanguage == null)
            {
                throw new ApplicationException("Can't load the current or default language!");
            }

            // Domain.
            domains_ = WikiDomains.Deserialize(Path.Combine(userDataFolderPath_, settings_.DomainsFilename));
            StoreWikiDomains(domains_);
            WikiDomain wikiDomain = domains_.FindByName(settings_.CurrentDomainName) ??
                                    domains_.FindByName(settings_.DefaultDomainName);

            if (wikiDomain == null)
            {
                throw new ApplicationException("Can't load the current or default domain!");
            }

            SetCurrentWikiSite(new WikiSite(wikiDomain, wikiLanguage));

            ShowAllLanguages();

            //TODO: Persist the location and settings.
            searchControl_ = new SearchControl(db_, entriesMap_, BrowseWikiArticle);
            searchControl_.HideOnClose = true;
            searchControl_.Show(dockPanel_, DockState.DockRightAutoHide);

            indexControl_ = new IndexControl(entriesMap_, BrowseWikiArticle);
            indexControl_.HideOnClose = true;
            indexControl_.Show(dockPanel_, DockState.DockRightAutoHide);
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
            domains.Serialize(Path.Combine(userDataFolderPath_, settings_.DomainsFilename));

            foreach (WikiDomain wikiDomain in domains.Domains)
            {
                Domain domain = new Domain { Name = wikiDomain.Name };

                db_.UpdateInsert(domain, db_.GetDomain(domain.Name));
            }
        }

        private void StoreWikiLanguages(LanguageCodes langCodes)
        {
            langCodes.Serialize(Path.Combine(userDataFolderPath_, settings_.LanguagesFilename));

            foreach (WikiLanguage language in langCodes.Languages)
            {
                if (language.Disabled)
                {
                    continue;
                }

                Language lang = new Language
                    {
                        Code = language.Code,
                        Name = !string.IsNullOrEmpty(language.Name) ? language.Name : language.LocalName
                    };

                db_.UpdateInsert(lang, db_.GetLanguageByCode(lang.Code));
            }
        }

        #region Browser Events

        private bool browser__DecideNavigationAction(string url, string mainurl)
        {
            if (url.StartsWith(WIKI_PROTOCOL_STRING))
            {
                string title = url.Substring(WIKI_PROTOCOL_STRING.Length);
                title = title.Replace('/', '\\');
                title = Title.DecodeEncodedNonAsciiCharacters(title);

                BrowseWikiArticle(title);

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
            string url;
            if (currentWikiPageName_ != null)
            {
                url = Title.Denormalize(currentWikiPageName_);
            }
            else
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

        private void OpenDatabase(string dbPath)
        {
            db_ = new Database(dbPath);

            entriesMap_.Clear();
            foreach (Domain domain in db_.GetDomains())
            {
                Dictionary<string, PrefixMatchContainer<string>> langTitlesMap = new Dictionary<string, PrefixMatchContainer<string>>(8);

                foreach (Language language in db_.GetLanguages())
                {
                    IList<string> pageTitles = db_.SelectPageTitles(domain.Id, language.Id);
                    if (pageTitles != null && pageTitles.Count > 0)
                    {
                        PrefixMatchContainer<string> titles = new PrefixMatchContainer<string>();

                        foreach (string pageTitle in pageTitles)
                        {
                            string title = Title.Denormalize(pageTitle);
                            titles.Add(title, title);
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

                BrowseWikiArticle(url);
            }
        }

        /// <summary>
        /// Changes the current language to the given one.
        /// </summary>
        /// <param name="languageCode">The code of the language to change to.</param>
        /// <returns>True if a change took place, otherwise False.</returns>
        private bool ChangeCurrentLanguage(string languageCode)
        {
            if (languageCode != settings_.CurrentLanguageCode)
            {
                WikiLanguage wikiLanguage = languages_.Languages.Find(lang => languageCode == lang.Code);
                if (wikiLanguage != null)
                {
                    SetCurrentWikiSite(new WikiSite(currentSite_.Domain, wikiLanguage));
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Changes the current domain to the given one.
        /// </summary>
        /// <param name="domainName">The name of the domain to change to.</param>
        /// <returns>True if a change took place, otherwise False.</returns>
        private bool ChangeCurrentDomain(string domainName)
        {
            if (domainName != settings_.CurrentDomainName)
            {
                WikiDomain wikiDomain = domains_.FindByName(domainName);
                if (wikiDomain != null)
                {
                    SetCurrentWikiSite(new WikiSite(wikiDomain, currentSite_.Language));
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Sets the current site.
        /// </summary>
        /// <param name="site">The current site to set.</param>
        private void SetCurrentWikiSite(WikiSite site)
        {
            currentSite_ = site;
            settings_.CurrentDomainName = site.Domain.Name;
            settings_.CurrentLanguageCode = site.Language.Code;
        }

        /// <summary>
        /// Sets the current site.
        /// </summary>
        /// <param name="domainName">The name of the domain to change to.</param>
        /// <param name="languageCode">The code of the language to change to.</param>
        private void SetCurrentWikiSite(string domainName, string languageCode)
        {
            WikiDomain wikiDomain = domains_.FindByName(domainName);
            WikiLanguage wikiLanguage = languages_.Languages.Find(lang => languageCode == lang.Code);
            if (wikiDomain != null && wikiLanguage != null)
            {
                SetCurrentWikiSite(new WikiSite(wikiDomain, wikiLanguage));
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
            Language language = db_.GetLanguageByName(languageName);
            if (language != null)
            {
                SetCurrentWikiSite(domainName, language.Code);
                BrowseWikiArticle(title);
            }
        }

        private void BrowseWikiArticle(string title)
        {
            currentWikiPageName_ = null;

            Page page = RetrievePage(title);
            if (page != null && !string.IsNullOrEmpty(page.Text))
            {
                ShowWikiPage(title, page.Text);
                return;
            }
        }

        /// <summary>
        /// Retrieves a page either from the DB or from the web.
        /// </summary>
        /// <returns></returns>
        private Page RetrievePage(string title)
        {
            title = Title.Normalize(title);

            int domainId = db_.GetDomain(currentSite_.Domain.Name).Id;
            Language language = db_.GetLanguageByCode(currentSite_.Language.Code);

            Page page = db_.SelectPage(domainId, language.Id, title);

            // If we got it and need no update, just return it.
            if (page != null && !string.IsNullOrEmpty(page.Text))
            {
                if (settings_.AutoUpdateDays == 0 ||
                    (DateTime.UtcNow - page.LastUpdateDateUtc).TotalDays < settings_.AutoUpdateDays)
                {
                    return page;
                }
            }
            else
            if (!settings_.AutoRetrieveMissing)
            {
                // Missing and auto-retrieve disabled.
                return null;
            }

            // Download and import from the web...
            page = ImportLivePage(title, currentSite_.Domain, domainId, language);

            return page;
        }

        private Page ImportLivePage(string title, WikiDomain domain, int domainId, Language language)
        {
            title = Title.Normalize(title);
            string url = string.Concat("http://", language.Code, domain.ExportUrl, title);
            string pageXml = Download.DownloadPage(url);
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(pageXml)))
            {
                DumpParser.ImportFromXml(ms, db_, DateTime.UtcNow, false, domainId, language.Id);
            }

            Page page = db_.SelectPage(domainId, language.Id, title);
            if (page != null)
            {
                UpdateGuiLists(language.Name, domain.Name, page.Title);
            }

            return page;
        }

        /// <summary>
        /// Updates the Entries list, languages and the index control with the new page.
        /// </summary>
        /// <param name="languageName">The page language name.</param>
        /// <param name="domainName">The page domain name.</param>
        /// <param name="title">The page title.</param>
        private void UpdateGuiLists(string languageName, string domainName, string title)
        {
            Dictionary<string, PrefixMatchContainer<string>> langEntries;
            if (!entriesMap_.TryGetValue(domainName, out langEntries))
            {
                langEntries = new Dictionary<string, PrefixMatchContainer<string>>();
                entriesMap_.Add(domainName, langEntries);
            }

            PrefixMatchContainer<string> titles;
            if (!langEntries.TryGetValue(languageName, out titles))
            {
                titles = new PrefixMatchContainer<string>();
                langEntries.Add(languageName, titles);
            }

            string titleDenorm = Title.Denormalize(title);
            if (titles.Find(titleDenorm, false, true) < 0)
            {
                titles.Add(titleDenorm, titleDenorm);
                indexControl_.UpdateListItems();
            }
        }

        private void ShowWikiPage(string title, string text)
        {
            title = Title.Denormalize(title);
            currentWikiPageName_ = title;

            ShowArticleLanguages(title, Wiki2Html.ExtractLanguages(ref text));

            Configuration config = new Configuration(
                                        currentSite_.Language.Code,
                                        currentSite_.BaseUrl,
                                        currentSite_.ExportUrl);
            config.SkinsPath = Path.Combine(userDataFolderPath_, "skins");

            Wiki2Html wiki2Html = new Wiki2Html(config, OnResolveWikiLinks, OnResolveTemplate, fileCache_);
            string html = wiki2Html.Convert(text);
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
            return WIKI_PROTOCOL_STRING + Title.EncodeNonAsciiCharacters(title);
        }

        private string OnResolveTemplate(string word, string lanugageCode)
        {
            string title = word;

            int colIndex = word.IndexOf(':');
            if (colIndex < 0 || !currentSite_.Namespaces.Contains(word.Substring(0, colIndex)))
            {
                // Missing or invalid namespace, assume "Template".
                string templateName = currentSite_.GetNamespace(WikiSite.Namespace.Tempalate);
                title = templateName + ':' + word;
            }

            Page page = RetrievePage(title);
            if (page != null)
            {
                string text = page.Text;
                while (true)
                {
                    int start = text.IndexOf("<noinclude>");
                    if (start < 0)
                    {
                        break;
                    }

                    int end = text.IndexOf("</noinclude>", start);
                    if (end < 0)
                    {
                        break;
                    }

                    text = text.Remove(start, end - start + "</noinclude>".Length);
                }

                return text;
            }

            return string.Empty;
        }

        private string WrapInHtmlBody(string title, string html)
        {
            bool right2Left = false;
            string language = string.Empty;
            WikiLanguage curWikiLanguage = languages_.Languages.Find(lang => settings_.CurrentLanguageCode == lang.Code);
            if (curWikiLanguage != null)
            {
                right2Left = curWikiLanguage.RightToLeft;

                if (!string.IsNullOrEmpty(curWikiLanguage.MimeCode))
                {
                    language = curWikiLanguage.MimeCode;
                }
                else
                {
                    language = curWikiLanguage.Code;
                }
            }

            StringBuilder sb = new StringBuilder(html.Length * 2);
            sb.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" ");
            sb.AppendLine("\"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\" ");
            if (!string.IsNullOrEmpty(language))
            {
                sb.Append("lang=\"");
                sb.Append(language);
                sb.Append("\" ");
            }

            string directionCode = right2Left ? "rtl" : "ltr";
            sb.Append("dir=\"");
            sb.Append(directionCode);
            sb.AppendLine("\">");
            sb.AppendLine("<head><title>").Append(title).Append("</title>");
            sb.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />");
            sb.AppendLine("<meta http-equiv=\"Content-Style-Type\" content=\"text/css\" />");
            //TODO: Add program version.
            sb.AppendLine("<meta name=\"generator\" content=\"WikiDesk\" />");
            sb.AppendLine("<link rel=\"copyright\" href=\"http://creativecommons.org/licenses/by-sa/3.0/\" />");

            string skinsFolderpath = Path.Combine(userDataFolderPath_, "skins");

            // Main.
            sb.Append("<link rel=\"stylesheet\" href=\"file:///");
            sb.Append(Path.Combine(skinsFolderpath, string.Format("vector/main-{0}.css", directionCode)));
            sb.AppendLine("\" type=\"text/css\" media=\"screen\" />");

            // Common Shared.
            sb.Append("<link rel=\"stylesheet\" href=\"file:///");
            sb.Append(Path.Combine(skinsFolderpath, "common/shared.css"));
            sb.AppendLine("\" type=\"text/css\" media=\"screen\" />");

            // Common Print.
            sb.Append("<link rel=\"stylesheet\" href=\"file:///");
            sb.Append(Path.Combine(skinsFolderpath, "common/commonPrint.css"));
            sb.AppendLine("\" type=\"text/css\" media=\"print\" />");

            // User.
            string skinPath = Path.Combine(skinsFolderpath, settings_.SkinName);
            sb.Append("<link rel=\"stylesheet\" href=\"file:///");
            sb.Append(Path.Combine(skinPath, "main.css"));
            sb.AppendLine("\" type=\"text/css\" media=\"screen\" />");

            if (right2Left)
            {
                sb.Append("<link rel=\"stylesheet\" href=\"file:///");
                sb.Append(Path.Combine(skinPath, "rtl.css"));
                sb.AppendLine("\" type=\"text/css\" media=\"screen\" />");
            }

            sb.Append("</head>");
            sb.Append("<body class=\"mediawiki ltr ns-0 ns-subject page-");
            sb.Append(Title.Normalize(title));
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
                WikiArticleName name = cboLanguage.Items[cboLanguage.SelectedIndex] as WikiArticleName;
                if (name != null && ChangeCurrentLanguage(name.LanguageCode))
                {
                    if (!string.IsNullOrEmpty(name.Name))
                    {
                        BrowseWikiArticle(name.Name);
                    }
                }
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmImport_ == null)
            {
                frmImport_ = new ImportForm(domains_, languages_);
            }

            if (frmImport_.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                Enabled = false;

                Domain domain = db_.GetDomain(frmImport_.DomainName);
                Language language = db_.GetLanguageByName(frmImport_.LanguageName);

                using (
                    FileStream stream = new FileStream(
                        frmImport_.DumpFilename, FileMode.Open, FileAccess.Read, FileShare.Read, 64 * 1024))
                {
                    string dumpFilename = frmImport_.DumpFilename.ToUpperInvariant();
                    if (dumpFilename.EndsWith("BZ2"))
                    {
                        using (BZip2InputStream bz2Stream = new BZip2InputStream(stream))
                        {
                            ImportProgress(stream, bz2Stream, domain, language);
                        }
                    }
                    else
                    {
                        ImportProgress(stream, stream, domain, language);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(
                        "Unexpected error while importing dump file:{0}{0}{1}", Environment.NewLine, ex.Message),
                    "Import Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
            finally
            {
                Enabled = true;
            }
        }

        delegate void exec();
        private void ImportProgress(FileStream fileStream, Stream inputStream, Domain domain, Language language)
        {
            bool cancel = false;
            int pages = 0;
            exec d =
                delegate
                    {
                        DumpParser.ImportFromXml(
                            inputStream,
                            db_,
                            frmImport_.Date,
                            frmImport_.IndexOnly,
                            domain.Id,
                            language.Id,
                            ref cancel,
                            ref pages);
                    };
            DateTime startTime = DateTime.UtcNow;
            IAsyncResult asyncResult = d.BeginInvoke(null, null);

            using (ProgressForm progForm = new ProgressForm())
            {
                progForm.Text = "Import Progress";
                progForm.txtInfo.Text = string.Format("Importing Wiki dump file {0}...", fileStream.Name);
                progForm.prgProgress.Minimum = 0;
                progForm.prgProgress.Maximum = (int)inputStream.Length / 1024 / 1024;
                Enabled = false;
                progForm.Show(this);
                while (!asyncResult.IsCompleted)
                {
                    cancel = progForm.DialogResult == DialogResult.Cancel;
                    try
                    {
                        progForm.txtInfo.Text = string.Format(
                                "Importing Wiki dump file {0}...{1}Imported {2} articles.",
                                fileStream.Name,
                                Environment.NewLine,
                                pages);
                        progForm.prgProgress.Value = (int)inputStream.Position / 1024 / 1024;
                    }
                    catch (Exception)
                    {
                        // When the user cancels, the stream is closed and Position throws.
                    }

                    double pcnt = 100.0 * progForm.prgProgress.Value / progForm.prgProgress.Maximum;
                    if (pcnt > 0.02)
                    {
                        TimeSpan elapsed = DateTime.UtcNow - startTime;
                        double remSeconds = (100.0 - pcnt) * elapsed.TotalSeconds / pcnt;
                        progForm.lblRemainingTimeValue_.Text = TimeSpan.FromSeconds(remSeconds + 0.5).ToString();
                    }
                    else
                    {
                        progForm.lblRemainingTimeValue_.Text = "Estimating...";
                    }

                    Application.DoEvents();
                    Thread.Sleep(266);
                }
            }

            d.EndInvoke(asyncResult);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OptionsForm frmOptions = new OptionsForm())
            {
                frmOptions.ShowDialog(this);
            }
        }

        private void viewToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            indexMenuItem_.Checked = indexControl_ != null &&
                                     indexControl_.VisibleState != DockState.Unknown &&
                                     indexControl_.VisibleState != DockState.Hidden &&
                                     !indexControl_.IsHidden;
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
            settings_.Serialize(Path.Combine(userDataFolderPath_, CONFIG_FILENAME));
        }

        #region representation

        private Database db_;

        private readonly Settings settings_;

        private readonly string userDataFolderPath_;

        private ImportForm frmImport_;

        /// <summary>
        /// The current wiki-page name/title.
        /// Valid only if we are on an internal wiki page.
        /// </summary>
        private string currentWikiPageName_;

        private readonly LanguageCodes languages_;
        private readonly WikiDomains domains_;

        private WikiSite currentSite_;

        private readonly WebKitBrowser browser_ = new WebKitBrowser();

        private readonly IndexControl indexControl_;
        private readonly SearchControl searchControl_;

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
