﻿namespace WikiDesk
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    using ICSharpCode.SharpZipLib.BZip2;

    using Tracy;

    using WebKit;

    using WeifenLuo.WinFormsUI.Docking;

    using WikiDesk.Core;
    using WikiDesk.Data;

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            logger_ = LogManager.CreateLoger(typeof(Wiki2Html).FullName);

            // For now the user's data are kept next to the executable.
            //TODO: Move to user-specific data folder.
            userDataFolderPath_ = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (string.IsNullOrEmpty(userDataFolderPath_))
            {
                throw new ApplicationException("Invalid or missing user-data folder.");
            }

            settings_ = Settings.Deserialize(Path.Combine(userDataFolderPath_, CONFIG_FILENAME));
            settings_.InstallationFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // WebKit.
            browser_.Visible = true;
            browser_.Dock = DockStyle.Fill;
            browser_.Name = "browser";
            //browser_.IsWebBrowserContextMenuEnabled = false;
            //browser_.IsScriptingEnabled = false;
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

            entriesMap_ = new Dictionary<string, Dictionary<string, PrefixMatchContainer<string>>>();

            btnBack.Enabled = false;
            btnForward.Enabled = false;
            btnStop.Enabled = false;
            btnGo.Enabled = false;
            cboLanguage.Enabled = false;

            tempFilename_ = Path.GetTempFileName().Replace(".tmp", ".html");
            tempFileUrl_ = "file:///" + tempFilename_.Replace('\\', '/');

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

            SetCurrentWikiSite(wikiDomain, wikiLanguage);

            ShowAllLanguages();

            searchControl_ = new SearchControl(db_, entriesMap_, BrowseWikiArticle);
            searchControl_.HideOnClose = true;

            indexControl_ = new IndexControl(entriesMap_, BrowseWikiArticle);
            indexControl_.HideOnClose = true;

            dockPanel_.DocumentStyle = DocumentStyle.DockingSdi;
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(settings_.Layout)))
            {
                dockPanel_.LoadFromXml(ms, GetDockContentPersistString);
            }

            dockContent_.DockPanel = dockPanel_;
            dockContent_.DockState = DockState.Document;
            dockContent_.Controls.Add(browser_);
            dockContent_.Show(dockPanel_);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            LoadDatabase(db_);
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
                string title = url.Substring(WIKI_PROTOCOL_STRING.Length).TrimEnd('/');
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
            string url = e.Url != null ? e.Url.ToString() : string.Empty;
            if (!browser__DecideNavigationAction(url, string.Empty))
            {
                e.Cancel = true;
                return;
            }

            if (url != tempFileUrl_)
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
            entriesMap_.Clear();
            db_ = new Database(dbPath);
        }

        private void LoadDatabase(Database db)
        {
            Enabled = false;
            LoadDatabaseForm loadDatabaseForm = null;
            try
            {
                SharedReference<long> entriesCount = new SharedReference<long>();
                loadDatabaseForm = new LoadDatabaseForm(db.DatabasePath, entriesCount, db.CountPages(0, 0));
                loadDatabaseForm.Show(this);
                var x = new EventHandler(delegate { LoadDatabaseEntries(loadDatabaseForm, db, entriesMap_, entriesCount); });
                IAsyncResult asyncResult = x.BeginInvoke(null, null, null, null);
                do
                {
                    Application.DoEvents();
                    Thread.Sleep(30);
                }
                while (!asyncResult.IsCompleted);

                x.EndInvoke(asyncResult);
            }
            finally
            {
                Enabled = true;
                if (loadDatabaseForm != null)
                {
                    loadDatabaseForm.Dispose();
                }
            }

            //TODO: How should auto-complete work? Should we add a domain selector?
            // cboNavigate.AutoCompleteCustomSource = titlesMap_.AutoCompleteStringCollection;

            if (indexControl_ != null)
            {
                indexControl_.UpdateListItems();
            }

            if (searchControl_ != null)
            {
                searchControl_.UpdateListItems();
            }
        }

        private static void LoadDatabaseEntries(
                        LoadDatabaseForm loadDatabaseForm,
                        Database db,
                        Dictionary<string, Dictionary<string, PrefixMatchContainer<string>>> entriesMap,
                        SharedReference<long> entriesCount)
        {
            entriesMap.Clear();
            foreach (Domain domain in db.GetDomains())
            {
                if (loadDatabaseForm.Cancel)
                {
                    break;
                }

                Dictionary<string, PrefixMatchContainer<string>> langTitlesMap = new Dictionary<string, PrefixMatchContainer<string>>(8);

                foreach (Language language in db.GetLanguages())
                {
                    if (loadDatabaseForm.Cancel)
                    {
                        break;
                    }

                    IEnumerator<string> pageTitles = db.SelectPageTitles(domain.Id, language.Id);
                    if (pageTitles != null && pageTitles.MoveNext())
                    {
                        PrefixMatchContainer<string> titles = new PrefixMatchContainer<string>();

                        do
                        {
                            string title = Title.Denormalize(pageTitles.Current);
                            titles.Add(title, title);

                            entriesCount.Reference++;
                        }
                        while (pageTitles.MoveNext() && !loadDatabaseForm.Cancel);

                        langTitlesMap.Add(language.Name, titles);
                    }
                }

                entriesMap.Add(domain.Name, langTitlesMap);
            }
        }

        private void OpenClick(object sender, EventArgs e)
        {
            openFileDialog.CheckFileExists = true;
            openFileDialog.ReadOnlyChecked = true;
            openFileDialog.ShowReadOnly = false;
            openFileDialog.Multiselect = false;
            openFileDialog.DefaultExt = "db";
            openFileDialog.Filter = "Sqlite database files (*.db)|*.db|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                OpenDatabase(openFileDialog.FileName);
                LoadDatabase(db_);
            }
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
                return SetCurrentWikiSite(currentSite_.Domain, wikiLanguage);
            }

            return false;
        }

        /// <summary>
        /// Sets the current site.
        /// </summary>
        /// <param name="wikiDomain">The domain to set.</param>
        /// <param name="wikiLanguage">The language to set.</param>
        private bool SetCurrentWikiSite(WikiDomain wikiDomain, WikiLanguage wikiLanguage)
        {
            if (wikiDomain != null && wikiLanguage != null)
            {
                // Don't create new WikiSite if nothing changed.
                if (currentSite_ != null &&
                    currentSite_.Domain == wikiDomain &&
                    currentSite_.Language == wikiLanguage)
                {
                    return true;
                }

                WikiSite wikiSite = new WikiSite(
                                            wikiDomain,
                                            wikiLanguage,
                                            settings_.InstallationFolder);
                currentSite_ = wikiSite;
                settings_.CurrentDomainName = wikiDomain.Name;
                settings_.CurrentLanguageCode = wikiLanguage.Code;
                return true;
            }

            return false;
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
                WikiDomain wikiDomain = domains_.FindByName(domainName);
                WikiLanguage wikiLanguage = languages_.Languages.Find(lang => language.Code == lang.Code);
                if (SetCurrentWikiSite(wikiDomain, wikiLanguage))
                {
                    BrowseWikiArticle(title);
                }
            }
        }

        private void BrowseWikiArticle(string title)
        {
            Dictionary<string, PrefixMatchContainer<string>> langEntries;
            if (!entriesMap_.TryGetValue(currentSite_.Domain.Name, out langEntries))
            {
                logger_.Log(Levels.Warn, "Failed to find Domain [{0}].", currentSite_.Domain.Name);
                return;
            }

            PrefixMatchContainer<string> titles;
            if (!langEntries.TryGetValue(currentSite_.Language.LocalName, out titles))
            {
                titles = new PrefixMatchContainer<string>();
                langEntries[currentSite_.Language.LocalName] = titles;
            }

            int languageEntriesCount = langEntries.Count;
            int titlesCount = titles.Count;
            try
            {
                currentWikiPageName_ = null;

                Page page = RetrievePage(title);
                if (page != null && !string.IsNullOrEmpty(page.Text))
                {
                    ShowWikiPage(ref title, page.Text, page.LastUpdateDateUtc);
                    return;
                }
            }
            finally
            {
                if (languageEntriesCount != langEntries.Count ||
                    titlesCount != titles.Count)
                {
                    indexControl_.UpdateListItems();
                }
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
            string url = currentSite_.GetExportUrl(title);
            string pageXml = Download.DownloadPage(url);
            if (pageXml.ToUpperInvariant().StartsWith("<!DOCTYPE HTML PUBLIC") ||
                pageXml.ToUpperInvariant().StartsWith("<HTML"))
            {
                // XML response expected. HTML probably means error.
                return null;
            }

            // Import the new data.
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(pageXml)))
            {
                string newTitle = DumpParser.ImportFromXml(ms, db_, DateTime.UtcNow, false, domainId, language.Id);
                if (newTitle.ToUpperInvariant() == title.ToUpperInvariant())
                {
                    title = newTitle;
                }
            }

            // Try to retrieve the target page.
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
            }
        }

        private void ShowWikiPage(ref string pageName, string wikiText, DateTime lastUpdateDateUtc)
        {
            Stopwatch sw = Stopwatch.StartNew();

            logger_.Log(Levels.Info, "Generating Html for {0}.", pageName);

            pageName = Title.Denormalize(pageName);
            currentWikiPageName_ = pageName;

            ShowArticleLanguages(pageName, Wiki2Html.ExtractLanguages(ref wikiText));

            Configuration config = new Configuration(currentSite_);
            config.SkinsPath = Path.Combine(userDataFolderPath_, "skins");

            Wiki2Html wiki2Html = new Wiki2Html(config, OnResolveWikiLinks, OnRetrievePage, fileCache_);

            string nameSpace;
            string pageTitle = Title.ParseFullPageName(pageName, out nameSpace);
            string html = wiki2Html.Convert(ref nameSpace, ref pageTitle, wikiText);

            pageName = Title.FullPageName(nameSpace, pageTitle);
            currentWikiPageName_ = pageName;

            html = WrapInHtmlBody(pageName, html, lastUpdateDateUtc, sw);

            logger_.Log(Levels.Info, "Generated Html for {0}:{1}{2}", pageName, Environment.NewLine, html);

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

        private string OnRetrievePage(string pageName, string lanugageCode)
        {
            pageName = Title.Normalize(pageName);
            Page page = RetrievePage(pageName);
            return page != null ? page.Text : null;
        }

        private string WrapInHtmlBody(string title, string html, DateTime lastUpdateDateUtc, Stopwatch sw)
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
            skinsFolderpath = skinsFolderpath.Replace('\\', '/');

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

            // User Skin.
            string skinPath = Path.Combine(skinsFolderpath, settings_.SkinName);
            sb.Append("<link rel=\"stylesheet\" href=\"file:///");
            sb.Append(Path.Combine(skinPath, "main.css"));
            sb.AppendLine("\" type=\"text/css\" media=\"screen\" />");

            // User Custom CSS.
            sb.Append("<style TYPE=\"text/css\">");
            sb.Append("<!--");
            sb.Append(settings_.CustomCss);
            sb.Append("-->");
            sb.AppendLine("</style>");

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
            sb.Append(Title.Denormalize(title));
            sb.Append("</h1>");
    		sb.Append("<div id=\"bodyContent\">");
            sb.Append("<div id=\"siteSub\">From Wikipedia, the free encyclopedia</div>");

            sb.Append(html);

            sb.Append("</div></div>");

            sb.Append(GetHtmlFooter(title, lastUpdateDateUtc, sw));
            sb.Append("</body></html>");

            return sb.ToString();
        }

        private string GetHtmlFooter(string title, DateTime lastUpdateDateUtc, Stopwatch sw)
        {
            StringBuilder sb = new StringBuilder(1024);
            sb.AppendLine("<div id=\"footer\">");

            sb.AppendLine("<ul id=\"footer-info\">");

            sb.Append("<li id=\"footer-info-generator\">");
            sb.Append("Generated by WikiDesk on ");
            sb.Append(DateTime.Now);
            sb.Append(" in ");
            sb.AppendFormat("{0:0.000}", sw.ElapsedMilliseconds / 1000.0);
            sb.Append(" seconds.");
            sb.Append(" <a href=\"");
            sb.Append(currentSite_.GetViewUrl(title));
            sb.Append("\" title=\"");
            sb.Append(title);
            sb.Append("\">Online version</a>.");
            sb.AppendLine("</li>");

            DateTime lastUpdateDateLocal = lastUpdateDateUtc.ToLocalTime();
            sb.Append("<li id=\"footer-info-lastmod\">");
            sb.Append("This page was last retrieved on ");
            sb.Append(lastUpdateDateLocal.ToLongDateString());
            sb.Append(", at ");
            sb.Append(lastUpdateDateLocal.ToLongTimeString());
            sb.AppendLine(".</li>");

            sb.Append("<li id=\"footer-info-copyright\">Text is available under the ");
            sb.Append("<a href=\"http://creativecommons.org/licenses/by-sa/3.0/\">");
            sb.Append("Creative Commons Attribution/Share-Alike License</a>; ");
            sb.Append("additional terms may apply. See ");
            sb.Append("<a href=\"http://wikimediafoundation.org/wiki/Terms_of_Use\">");
            sb.Append("Terms of Use</a> for details.");
            sb.AppendLine("</li>");
            sb.AppendLine("</ul>");

            sb.AppendLine("<ul id=\"footer-places\">");
            sb.Append("<li id=\"footer-places-privacy\">");
            sb.Append("<a href=\"http://wikimediafoundation.org/wiki/Privacy_policy\" title=\"wikimedia:Privacy policy\">Privacy policy</a>");
            sb.AppendLine("</li>");
            sb.Append("<li id=\"footer-places-about\">");
            sb.Append("<a href=\"");
            sb.Append(WIKI_PROTOCOL_STRING);
            sb.Append("Project:About\" title=\"Project:About\">About MediaWiki.org</a>");
            sb.AppendLine("</li>");
            sb.Append("<li id=\"footer-places-disclaimer\">");
            sb.Append("<a href=\"");
            sb.Append(WIKI_PROTOCOL_STRING);
            sb.Append("Project:General_disclaimer\" title=\"Project:General disclaimer\">Disclaimers</a>");
            sb.AppendLine("</li>");
            sb.AppendLine("</ul>");

            string skinsFolderpath = Path.Combine(userDataFolderPath_, "skins");
            string commonSkinsFolderPath = Path.Combine(skinsFolderpath, "common");
            commonSkinsFolderPath = commonSkinsFolderPath.Replace('\\', '/');
            commonSkinsFolderPath = "file:///" + commonSkinsFolderPath;

            sb.AppendLine("<ul id=\"footer-icons\" class=\"noprint\">");
            sb.Append("<li id=\"footer-copyrightico\">");
            sb.Append("<a href=\"http://wikimediafoundation.org/\">Wikimedia Foundation</a>");
            sb.AppendLine("</li>");
            sb.Append("<li id=\"footer-poweredbyico\">");
            sb.Append("<a href=\"http://www.mediawiki.org/\"><img src=\"");
            sb.Append(commonSkinsFolderPath);
            sb.Append("/images/poweredby_mediawiki_88x31.png\" alt=\"Powered by MediaWiki\" width=\"88\" height=\"31\"></a>");
            sb.AppendLine("</li>");
            sb.AppendLine("</ul>");
            sb.AppendLine("<div style=\"clear:both\"></div>");
            sb.AppendLine("</div>");

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

            using (ProgressForm progForm = new ProgressForm())
            {
                Enabled = false;
                try
                {
                    string dumpFilename = Path.GetFileName(frmImport_.DumpFileSource);

                    progForm.Text = "Import Progress";
                    progForm.txtInfo.Text = string.Format("Importing Wiki dump file {0}...", dumpFilename);
                    progForm.prgProgress.Value = 0;
                    progForm.Show(this);

                    Stream sourceStream;
                    if (frmImport_.LocalDump)
                    {
                        // Local Dump.
                        sourceStream = new FileStream(
                            frmImport_.DumpFileSource, FileMode.Open, FileAccess.Read, FileShare.Read, 64 * 1024);
                        sourceStream.Seek(frmImport_.ResumePosition, SeekOrigin.Begin);
                    }
                    else
                    {
                        // Web Download.
                        sourceStream = new WebStream(frmImport_.DumpFileSource, frmImport_.ResumePosition);
                    }

                    using (sourceStream)
                    {
                        bool bz2 = dumpFilename.ToUpperInvariant().EndsWith("BZ2");
                        using (Stream inputStream = bz2 ? new BZip2InputStream(sourceStream) : sourceStream)
                        {
                            Domain domain = db_.GetDomain(frmImport_.DomainName);
                            Language language = db_.GetLanguageByName(frmImport_.LanguageName);
                            ImportProgress(
                                progForm,
                                dumpFilename,
                                inputStream,
                                domain,
                                language);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        string.Format("Failed to import dump source:{0}{0}{1}", Environment.NewLine, ex.Message),
                        "Import Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                }
                finally
                {
                    progForm.txtInfo.Text = "Reloading database...";
                    Application.DoEvents();

                    LoadDatabase(db_);
                    Enabled = true;
                }
            }
        }

        delegate void exec();
        private void ImportProgress(
                        ProgressForm progForm,
                        string sourceName,
                        Stream inputStream,
                        Domain domain,
                        Language language)
        {
            DateTime startTime = DateTime.UtcNow;

            progForm.prgProgress.Minimum = 0;
            progForm.prgProgress.Maximum = (int)(inputStream.Length / 1024 / 1024);

            bool cancel = false;
            int entries = 0;
            exec d =
                () =>
                DumpParser.ImportFromXml(
                    inputStream,
                    db_,
                    frmImport_.Date,
                    frmImport_.IndexOnly,
                    domain.Id,
                    language.Id,
                    ref cancel,
                    ref entries);

            IAsyncResult asyncResult = d.BeginInvoke(null, null);

            try
            {
                int lastEntryCount = 0;
                double entryRate = 0.0;
                DateTime lastRefreshTime = startTime;
                while (!asyncResult.IsCompleted)
                {
                    try
                    {
                        progForm.prgProgress.Value = (int)(inputStream.Position / 1024 / 1024);
                    }
                    catch (Exception)
                    {
                        // When the user cancels, the stream is closed and Position throws.
                    }

                    progForm.txtInfo.Text = string.Format(
                            "Importing Wiki dump {0}...{1}Imported {2} entries. ({3} / {4} MBytes.){1}Rate: {5} entries / sec.",
                            sourceName,
                            Environment.NewLine,
                            entries,
                            progForm.prgProgress.Value,
                            progForm.prgProgress.Maximum,
                            (int)entryRate);

                    double pcnt = 100.0 * progForm.prgProgress.Value / progForm.prgProgress.Maximum;
                    if (pcnt > 0.02)
                    {
                        TimeSpan elapsed = DateTime.UtcNow - startTime;
                        double remSeconds = (100.0 - pcnt) * elapsed.TotalSeconds / pcnt;
                        TimeSpan remaining = TimeSpan.FromSeconds(remSeconds + 0.5);
                        progForm.lblRemainingTimeValue_.Text =
                            string.Format("{0,2}h : {1,2}m : {2,2}s", remaining.Hours, remaining.Minutes, remaining.Seconds);

                        double entryRateElapsedSecs = (DateTime.UtcNow - lastRefreshTime).TotalSeconds;
                        int newEntries = entries - lastEntryCount;
                        if (entryRate == 0 ||
                            (entryRateElapsedSecs >= 5 && newEntries > 0))
                        {
                            double newEntryRate = newEntries / entryRateElapsedSecs;
                            entryRate = (entryRate + 3 * newEntryRate) / 4;
                            lastEntryCount = entries;
                            lastRefreshTime = DateTime.UtcNow;
                        }
                    }
                    else
                    {
                        progForm.lblRemainingTimeValue_.Text = "Estimating...";
                    }

                    for (int i = 0; i < 9 && !cancel; ++i)
                    {
                        cancel = progForm.Cancelled;
                        Application.DoEvents();
                        Thread.Sleep(166);
                    }
                }
            }
            finally
            {
                progForm.txtInfo.Text = "Cancelling...";
                Application.DoEvents();

                cancel = true;
                d.EndInvoke(asyncResult);
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OptionsForm frmOptions = new OptionsForm(settings_))
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
            using (MemoryStream ms = new MemoryStream(1024))
            {
                dockPanel_.SaveAsXml(ms, Encoding.UTF8);
                ms.Flush();
                settings_.Layout = Encoding.UTF8.GetString(ms.ToArray());
            }

            settings_.Serialize(Path.Combine(userDataFolderPath_, CONFIG_FILENAME));
        }

        private IDockContent GetDockContentPersistString(string persistString)
        {
            if (persistString == typeof(IndexControl).ToString())
            {
                return indexControl_;
            }

            if (persistString == typeof(SearchControl).ToString())
            {
                return searchControl_;
            }

            return null;
        }

        #region representation

        private readonly Settings settings_;

        private readonly string userDataFolderPath_;

        private readonly LanguageCodes languages_;
        private readonly WikiDomains domains_;

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

        private readonly ILogger logger_;

        private Database db_;
        private ImportForm frmImport_;

        /// <summary>
        /// The current wiki-page name/title.
        /// Valid only if we are on an internal wiki page.
        /// </summary>
        private string currentWikiPageName_;
        private WikiSite currentSite_;

        #endregion // representation

        #region constants

        private const string APPLICATION_NAME = "WikiDesk";
        private const string CONFIG_FILENAME = "WikiDesk.xml";

        private const string WIKI_PROTOCOL_STRING = "wiki://";

        #endregion // constants
    }
}
