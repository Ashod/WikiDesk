namespace WikiDesk
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Forms;

    using WeifenLuo.WinFormsUI.Docking;

    public partial class IndexControl : DockContent
    {
        public IndexControl(Dictionary<string,
                                Dictionary<string, PrefixMatchContainer<string>>> entriesMap)
        {
            InitializeComponent();

            lstTitles_.VirtualMode = true;
            entriesMap_ = entriesMap;

            UpdateListItems();
        }

        public void UpdateListItems()
        {
            foreach (KeyValuePair<string, Dictionary<string, PrefixMatchContainer<string>>> domainLangPair in entriesMap_)
            {
                cboDomains_.Items.Add(domainLangPair.Key);
            }

            cboDomains_.SelectedIndex = (cboDomains_.Items.Count > 0) ? 0 : -1;
        }

        #region events

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

            if (cboLanguages_.SelectedIndex < 0)
            {
                titles_ = null;
                lstTitles_.VirtualListSize = 0;
                lstTitles_.Items.Clear();
            }

            lstTitles_.Invalidate();
        }

        private void cboLanguages__SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.Assert(cboDomains_.SelectedIndex >= 0);

            txtTitle_.Text = string.Empty;

            if (cboLanguages_.SelectedIndex >= 0)
            {
                Dictionary<string, PrefixMatchContainer<string>> langEntries;
                if (entriesMap_.TryGetValue(cboDomains_.Text, out langEntries))
                {
                    if (langEntries.TryGetValue(cboLanguages_.Text, out titles_))
                    {
                        if (titles_ != null)
                        {
                            lstTitles_.VirtualListSize = titles_.Count;
                            lstTitles_.Invalidate();
                            return;
                        }
                    }
                }
            }

            titles_ = null;
            lstTitles_.VirtualListSize = 0;
            lstTitles_.Items.Clear();
            lstTitles_.Invalidate();
        }

        private void txtTitle__TextChanged(object sender, EventArgs e)
        {
            ListViewItem lvi = lstTitles_.FindItemWithText(txtTitle_.Text);

            // Select the item found and scroll it into view.
            if (lvi != null)
            {
                lstTitles_.SelectedIndices.Add(lvi.Index);
                lstTitles_.EnsureVisible(lvi.Index);
            }
        }

        private void lstTitles__RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (titles_ != null)
            {
                e.Item = new ListViewItem(titles_[e.ItemIndex]);
            }
        }

        private void lstTitles__SearchForVirtualItem(object sender, SearchForVirtualItemEventArgs e)
        {
            if (titles_ != null)
            {
                e.Index = titles_.Find(e.Text, true, false);
            }
        }

        private void lstTitles__DoubleClick(object sender, EventArgs e)
        {
            //TODO: Browse topic.
        }

        private void lstTitles__ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (titles_ != null && e.IsSelected)
            {
                if (e.ItemIndex >= 0)
                {
                    txtTitle_.Text = titles_[e.ItemIndex];
                    return;
                }
            }

            txtTitle_.Text = string.Empty;
        }

        private void lstTitles__Resize(object sender, EventArgs e)
        {
            lstTitles_.Columns[0].Width = lstTitles_.ClientSize.Width - 5;
        }

        #endregion // events

        #region representation

        /// <summary>
        /// All entries mapped as: Domain : Language : Title.
        /// </summary>
        private readonly Dictionary<string, Dictionary<string, PrefixMatchContainer<string>>> entriesMap_;

        /// <summary>
        /// The titles under the currently selected domain and language, if any.
        /// </summary>
        private PrefixMatchContainer<string> titles_;

        #endregion // representation
    }
}
