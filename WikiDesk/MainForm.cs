namespace WikiDesk
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    using WebKit;

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
            Controls.Add(browser_);
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

            btnBack.Enabled = false;
            btnForward.Enabled = false;
            btnStop.Enabled = false;
            btnGo.Enabled = false;
            cboLanguage.Enabled = false;

            try
            {
                settings_ = Settings.Deserialize(CONFIG_FILENAME);
            }
            catch (Exception)
            {
                settings_ = new Settings();
                settings_.Serialize(CONFIG_FILENAME);
            }

            OpenDatabase(settings_.DefaultDatabaseFilename);

            languages_ = LanguageCodes.Deserialize(settings_.LanguageCodesFilename);

            StoreLanguageCodes(languages_);
        }

        private void StoreLanguageCodes(LanguageCodes langCodes)
        {
            langCodes.Serialize(settings_.LanguageCodesFilename);

            foreach (Language language in langCodes.Languages)
            {
                Data.Language lang = new Data.Language();
                lang.Code = language.Code;
                lang.Name = language.Name;
                db_.Update(lang);
            }
        }

        #region Browser Events

        private void browser__Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            Text = string.Format("{0} - {1}", APPLICATION_NAME, browser_.DocumentTitle ?? string.Empty);
            cboNavigate.Text = browser_.Url != null ? browser_.Url.ToString() : string.Empty;
        }

        private void browser__Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            cboNavigate.Text = browser_.Url.ToString();
        }

        private void browser__DocumentTitleChanged(object sender, EventArgs e)
        {
            Text = string.Format("{0} - {1}", APPLICATION_NAME, browser_.DocumentTitle);
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
            cboNavigate.Text = browser_.Url.ToString();

            btnBack.Enabled = browser_.CanGoBack;
            btnForward.Enabled = browser_.CanGoForward;
        }

        #endregion // Browser Events

        #region Browser Controls

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
                db.Load("Z:\\simplewiki-20100401-pages-articles.xml", "en", true);
            }
        }

        private void OpenDatabase(string dbPath)
        {
            db_ = new Database(dbPath);

            titles_.Clear();
            foreach (Page page in db_.GetPages())
            {
                titles_.Add(page.Title);
            }

            cboNavigate.AutoCompleteCustomSource = titles_;
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

        private void NavigateTo(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return;
            }

            if (title.ToLowerInvariant().StartsWith("http://") ||
                title.ToLowerInvariant().StartsWith("https://"))
            {
                browser_.Url = new Uri(title);
            }
            else
            {
                BrowseWikiArticle(settings_.CurrentLanguageCode, title);
            }
        }

        private void BrowseWikiArticle(string languageCode, string title)
        {
            Page page = db_.QueryPage(title);
            if (page != null)
            {
                Revision rev = db_.QueryRevision(page.LastRevisionId);
                if (rev != null)
                {
                    string text = Encoding.UTF8.GetString(rev.Text);
                    //browser_.Url = null;

                    ShowWikiPage(title, text);
                    return;
                }
            }

            // Download from the web...
            string url = string.Concat("http://", languageCode, settings_.ExportUrl, title);
            string pageXml = Download.DownloadPage(url);
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(pageXml)))
            {
                db_.ImportFromXml(ms, false, languageCode);
            }

            Page page2 = db_.QueryPage(title);
            if (page2 != null)
            {
                Revision rev = db_.QueryRevision(page2.LastRevisionId);
                if (rev != null)
                {
                    string text = Encoding.UTF8.GetString(rev.Text);
                    //browser_.Url = null;

                    ShowWikiPage(title, text);
                    return;
                }
            }
        }

        private void ShowWikiPage(string title, string text)
        {
            cboLanguage.Items.Clear();
            foreach (KeyValuePair<string, string> pair in Wiki2Html.ExtractLanguages(ref text))
            {
                WikiArticleName name = new WikiArticleName();
                name.Name = pair.Value;
                name.LanguageCode = pair.Key;
                Language language = languages_.Languages.Find(lang => name.LanguageCode == lang.Code);
                if (language != null)
                {
                    name.LanguageName = language.Name;
                }
                else
                {
                    name.LanguageName = "Unknown Language";
                }

                cboLanguage.Items.Add(name);
            }

            cboLanguage.Enabled = (cboLanguage.Items.Count > 0);

//             browser_.DocumentText = Wiki2Html.Convert(text);
            Wiki2Html wiki2Html = new Wiki2Html();
            string html = wiki2Html.ConvertX(text);
            browser_.DocumentText = WrapInHtmlBody(title, html);
            Text = string.Format("{0} - {1}", APPLICATION_NAME, title);
        }

        private string WrapInHtmlBody(string title, string html)
        {
            string header = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">" +
            "<html xmlns=\"http://www.w3.org/1999/xhtml\" lang=\"en\" dir=\"ltr\">" +
            "<head><title>" + title + "</title>";
            string body = "<body class=\"mediawiki ltr ns-0 ns-subject page-Brazil skin-vector\">" +
                          "<div id=\"mw-page-base\" class=\"noprint\"></div>" +
                          "<div id=\"mw-head-base\" class=\"noprint\"></div>" +
                          "<div id=\"content\">";
            string footer = "</body></html>";

            return header + body + html + footer;
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
                BrowseWikiArticle(name.LanguageCode, name.Name);
            }
        }

        private void ExitClick(object sender, EventArgs e)
        {
            Close();
        }

        private Database db_;

        private readonly Settings settings_;

        private readonly LanguageCodes languages_;

        private readonly WebKitBrowser browser_ = new WebKitBrowser();
        private readonly AutoCompleteStringCollection titles_ = new AutoCompleteStringCollection();

        private const string APPLICATION_NAME = "WikiDesk";
        private const string CONFIG_FILENAME = "WikiDesk.xml";
    }
}
