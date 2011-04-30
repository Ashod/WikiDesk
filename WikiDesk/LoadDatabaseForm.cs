
using System.Windows.Forms;

namespace WikiDesk
{
    using System;

    public partial class LoadDatabaseForm : Form
    {
        public LoadDatabaseForm(
                ref long entries,
                long totalEntires)
        {
            entries_ = entries;
            totalEntires_ = totalEntires;

            InitializeComponent();

            prgProgress_.Maximum = (int)(totalEntires / 1024);

            timer_.Interval = 60;
            timer_.Tick += OnTimer;
            timer_.Start();
        }

        private void OnTimer(object sender, EventArgs e)
        {
            lblEntriesLoadedValue_.Text = string.Format("{0} / {1}", entries_, totalEntires_);
            prgProgress_.Value = (int)(entries_ / 1024);
        }

        #region representation

        private readonly long entries_;

        private readonly long totalEntires_;

        private readonly Timer timer_ = new Timer();

        #endregion // representation
    }
}
