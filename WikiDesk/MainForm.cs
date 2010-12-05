﻿namespace WikiDesk
{
    using System;
    using System.IO;
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
            this.Controls.Add(browser_);
        }

        private void LoadClick(object sender, EventArgs e)
        {
            string folder = "Z:\\"; //Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string dbPath = Path.Combine(folder, "wikidesk.db");
            using (Database db = new Database(dbPath))
            {
                db.Load("Z:\\simplewiki-20100401-pages-articles.xml");
            }
        }

        private void OpenDatabase()
        {
            string folder = "Z:\\"; //Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string dbPath = Path.Combine(folder, "wikidesk.db");
            db_ = new Database(dbPath);
        }

        private void OpenClick(object sender, EventArgs e)
        {
            OpenDatabase();

            foreach (Page page in db_.GetPages())
            {
                lstTitles.Items.Add(page.Title);
            }
        }

        private void lstTitles_SelectedIndexChanged(object sender, EventArgs e)
        {
            string title = lstTitles.SelectedItem as string;
            if (!string.IsNullOrEmpty(title))
            {
                Page page = db_.QueryPage(title);
                if (page != null)
                {
                    Revision rev = db_.QueryRevision(page.LastRevisionId);
                    if (rev != null)
                    {
                        browser_.DocumentText = Wiki.Wiki2Html(rev.Text);
                    }
                }
            }
        }

        private Database db_;
        private WebKitBrowser browser_ = new WebKitBrowser();
    }
}
