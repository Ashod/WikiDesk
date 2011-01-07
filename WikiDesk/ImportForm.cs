using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WikiDesk
{
    using System.IO;

    using WikiDesk.Data;

    public partial class ImportForm : Form
    {
        public ImportForm(Database db, Dictionary<string, Dictionary<string, PrefixMatchContainer<string>>> entriesMap)
        {
            InitializeComponent();

            db_ = db;
            entriesMap_ = entriesMap;

            foreach (KeyValuePair<string, Dictionary<string, PrefixMatchContainer<string>>> domainLangPair in entriesMap_)
            {
                cboDomains_.Items.Add(domainLangPair.Key);
            }

            cboDomains_.SelectedIndex = (cboDomains_.Items.Count > 0) ? 0 : -1;
        }

        private void cboDomains__SelectedIndexChanged(object sender, EventArgs e)
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

        private void btnBrowse__Click(object sender, EventArgs e)
        {
            openFileDialog_.CheckFileExists = true;
            openFileDialog_.DefaultExt = "bz2";
            openFileDialog_.Filter = "Compressed XML files (*.xml.bz2)|*.xml.bz2|XML files (*.xml)|*.xml|All files (*.*)|*.*";
            if (openFileDialog_.ShowDialog(this) == DialogResult.OK)
            {
                txtFilepath_.Text = openFileDialog_.FileName;
            }
        }

        private void btnImport__Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtFilepath_.Text))
            {
                MessageBox.Show(
                        "Please select a valid dump file to import from.",
                        "Import Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(cboDomains_.Text))
            {
                MessageBox.Show(
                        "Please select a valid domain.",
                        "Import Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(cboLanguages_.Text))
            {
                MessageBox.Show(
                        "Please select a valid language.",
                        "Import Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel__Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        #region representation

        private readonly Database db_;

        private readonly Dictionary<string, Dictionary<string, PrefixMatchContainer<string>>> entriesMap_;

        #endregion // representation
    }
}
