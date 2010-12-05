namespace WikiDesk
{
    using System;
    using System.IO;
    using System.Windows.Forms;

    using WikiDesk.Data;

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
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

        private void OpenClick(object sender, EventArgs e)
        {
            string folder = "Z:\\"; //Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string dbPath = Path.Combine(folder, "wikidesk.db");
            using (Database db = new Database(dbPath))
            {
                //lstTitles.Items.AddRange(db.GetTitles());
            }
        }
    }
}
