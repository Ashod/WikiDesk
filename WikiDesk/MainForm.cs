namespace WikiDesk
{
    using System;
    using System.Drawing;
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
            Controls.Add(browser_);
            browser_.BringToFront();
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

        private void Navigation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (int)Keys.Enter)
            {
                Titles_SelectedIndexChanged(sender, e);
            }
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            cboNavigate.Size = new Size(Width - 200, cboNavigate.Height);
        }

        private void ExitClick(object sender, EventArgs e)
        {
            Close();
        }

        private Database db_;
        private readonly WebKitBrowser browser_ = new WebKitBrowser();
        private readonly AutoCompleteStringCollection titles_ = new AutoCompleteStringCollection();
    }
}
