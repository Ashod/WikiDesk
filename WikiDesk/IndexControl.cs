﻿namespace WikiDesk
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Forms;

    using WeifenLuo.WinFormsUI.Docking;

    public partial class IndexControl : DockContent
    {
        public delegate void OnTitleNavigate(string domainName, string languageName, string title);

        public IndexControl(Dictionary<string,
                                Dictionary<string, PrefixMatchContainer<string>>> entriesMap,
                            OnTitleNavigate onTitleNavigate)
        {
            InitializeComponent();

            lstTitles_.VirtualMode = true;

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
                            txtTitle_.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                            txtTitle_.AutoCompleteSource = AutoCompleteSource.CustomSource;
                            txtTitle_.AutoCompleteCustomSource = titles_.AutoCompleteStringCollection;
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

            // Focus on the item found and scroll it into view.
            if (lvi != null)
            {
                lstTitles_.FocusedItem = lvi;
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
            string title = lstTitles_.FocusedItem != null ? lstTitles_.FocusedItem.Text : null;
            NavigateTo(title);
        }

        private void txtTitle__KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ListViewItem lvi = lstTitles_.FindItemWithText(txtTitle_.Text);
                if (lvi != null)
                {
                    lstTitles_.SelectedIndices.Add(lvi.Index);
                    NavigateTo(lvi.Text);
                }
            }
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

        private readonly OnTitleNavigate onTitleNavigate_;

        /// <summary>
        /// The titles under the currently selected domain and language, if any.
        /// </summary>
        private PrefixMatchContainer<string> titles_;

        #endregion // representation
    }
}