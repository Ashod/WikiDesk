namespace WikiDesk
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    using WeifenLuo.WinFormsUI.Docking;

    public partial class IndexControl : DockContent
    {
        public IndexControl(PrefixMatchContainer<string> titlesMap)
        {
            InitializeComponent();

            titlesMap_ = titlesMap;
            titlesMap_.OnCollectionChanged += titlesMap__OnCollectionChanged;

            lstTitles_.VirtualMode = true;
            lstTitles_.VirtualListSize = titlesMap_.Count;
        }

        public void UpdateListItems()
        {

        }

        #region events

        private void titlesMap__OnCollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            lstTitles_.VirtualListSize = titlesMap_.Count;
        }

        private void cboLanguages__SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO: Change the current search language.
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
            e.Item = new ListViewItem(titlesMap_[e.ItemIndex]);
        }

        private void lstTitles__SearchForVirtualItem(object sender, SearchForVirtualItemEventArgs e)
        {
            e.Index = titlesMap_.Find(e.Text, true, false);
        }

        private void lstTitles__DoubleClick(object sender, System.EventArgs e)
        {
            //TODO: Browse topic.
        }

        private void lstTitles__Resize(object sender, EventArgs e)
        {
            lstTitles_.Columns[0].Width = lstTitles_.ClientSize.Width - 5;
        }

        #endregion // events

        #region representation

        private readonly PrefixMatchContainer<string> titlesMap_;

        #endregion // representation
    }
}
