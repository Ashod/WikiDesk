namespace WikiDesk
{
    using System.Collections.Generic;
    using System.Windows.Forms;

    using WeifenLuo.WinFormsUI.Docking;

    using WikiDesk.Core;
    using WikiDesk.Data;

    public partial class SearchControl : DockContent
    {
        public delegate void OnTitleNavigate(string domainName, string languageName, string title);

        public SearchControl(
                    Database db,
                    Dictionary<string,
                                Dictionary<string, PrefixMatchContainer<string>>> entriesMap,
                    OnTitleNavigate onTitleNavigate)
        {
            InitializeComponent();

            db_ = db;
            entriesMap_ = entriesMap;
            onTitleNavigate_ = onTitleNavigate;

            UpdateListItems();
        }

        public void UpdateListItems()
        {
            cboDomains_.Items.Clear();
            foreach (KeyValuePair<string, Dictionary<string, PrefixMatchContainer<string>>> domainLangPair in entriesMap_)
            {
                cboDomains_.Items.Add(domainLangPair.Key);
            }

            if (cboDomains_.SelectedIndex >= 0)
            {
                cboDomains_.SelectedIndex = cboDomains_.SelectedIndex;
            }
            else
            {
                cboDomains_.SelectedIndex = (cboDomains_.Items.Count > 0) ? 0 : -1;
            }
        }

        private void cboDomains__SelectedIndexChanged(object sender, System.EventArgs e)
        {
            cboLanguages_.SelectedIndex = -1;

            if (cboDomains_.SelectedIndex >= 0)
            {
                Dictionary<string, PrefixMatchContainer<string>> langEntries;
                if (entriesMap_.TryGetValue(cboDomains_.Text, out langEntries))
                {
                    cboLanguages_.Items.Clear();

                    foreach (KeyValuePair<string, PrefixMatchContainer<string>> langTitlesPair in langEntries)
                    {
                        cboLanguages_.Items.Add(langTitlesPair.Key);
                    }

                    //TODO: Automatically select the default or current language.
                    cboLanguages_.SelectedIndex = (cboLanguages_.Items.Count > 0) ? 0 : -1;
                    return;
                }
            }
        }

        private void txtTitle__KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch__Click(sender, e);
            }
        }

        private void btnSearch__Click(object sender, System.EventArgs e)
        {
            lstTitles_.Items.Clear();

            if (!string.IsNullOrEmpty(txtTitle_.Text))
            {
                string query = txtTitle_.Text;
                query = '%' + query.Trim('%') + '%';

                int domainId = db_.GetDomain(cboDomains_.Text).Id;
                int languageId = db_.GetLanguageByName(cboLanguages_.Text).Id;
                foreach (string title in db_.SearchPages(domainId, languageId, query))
                {
                    string titleDenorm = Title.Denormalize(title);
                    lstTitles_.Items.Add(titleDenorm);
                }
            }
        }

        private void lstTitles__DoubleClick(object sender, System.EventArgs e)
        {
            string title = lstTitles_.FocusedItem != null ? lstTitles_.FocusedItem.Text : null;
            NavigateTo(title);
        }

        private void NavigateTo(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                txtTitle_.Text = title;
                if (onTitleNavigate_ != null)
                {
                    onTitleNavigate_(cboDomains_.Text, cboLanguages_.Text, title);
                }
            }
        }

        private void lstTitles__Resize(object sender, System.EventArgs e)
        {
            lstTitles_.Columns[0].Width = lstTitles_.ClientSize.Width - 5;
        }

        #region representation

        private readonly Database db_;

        private readonly OnTitleNavigate onTitleNavigate_;

        private readonly Dictionary<string, Dictionary<string, PrefixMatchContainer<string>>> entriesMap_;

        #endregion // representation
    }
}
