
using System.Windows.Forms;

namespace WikiDesk
{
    using System;
    using System.Collections.Generic;

    public partial class LoadDatabaseForm : Form
    {
        public LoadDatabaseForm(
                Dictionary<string, Dictionary<string, PrefixMatchContainer<string>>> entriesMap,
                long totalEntires)
        {
            entriesMap_ = entriesMap;
            totalEntires_ = totalEntires;

            InitializeComponent();

            prgProgress_.Maximum = (int)(totalEntires / 1024);

            timer_.Interval = 60;
            timer_.Tick += OnTimer;
            timer_.Start();
        }

        private void OnTimer(object sender, EventArgs e)
        {
            long total = 0;
            foreach (var pair in entriesMap_)
            {
                total += pair.Value.Count;
            }

            lblEntriesLoadedValue_.Text = string.Format("{0} / {1}", total, totalEntires_);
            prgProgress_.Value = (int)(total / 1024);
        }

        #region representation

        private readonly Dictionary<string, Dictionary<string, PrefixMatchContainer<string>>> entriesMap_;

        private readonly long totalEntires_;

        private readonly Timer timer_ = new Timer();

        #endregion // representation
    }
}
