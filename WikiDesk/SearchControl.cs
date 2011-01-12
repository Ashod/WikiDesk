namespace WikiDesk
{
    using WeifenLuo.WinFormsUI.Docking;

    using WikiDesk.Data;

    public partial class SearchControl : DockContent
    {
        public SearchControl(Database db)
        {
            InitializeComponent();

            db_ = db;
        }

        private void btnSearch__Click(object sender, System.EventArgs e)
        {
            lstTitles_.Items.Clear();
            foreach (string title in db_.SearchPages(txtTitle_.Text))
            {
                lstTitles_.Items.Add(title);
            }
        }

        private readonly Database db_;
    }
}
