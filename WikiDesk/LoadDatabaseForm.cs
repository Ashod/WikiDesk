
using System.Windows.Forms;

namespace WikiDesk
{
    using System;

    public partial class LoadDatabaseForm : Form
    {
        public LoadDatabaseForm(
                string databasePath,
                SharedReference<long> entries,
                long totalEntires)
        {
            entries_ = entries;
            totalEntires_ = totalEntires;

            InitializeComponent();

            prgProgress_.Maximum = (int)(totalEntires / 1024);
            lblDatabasePathValue_.Text = databasePath;

            timer_.Interval = 60;
            timer_.Tick += OnTimer;
            timer_.Start();
        }

        public bool Cancel
        {
            get { return cancel_; }
        }

        #region implementation

        private void OnTimer(object sender, EventArgs e)
        {
            lblEntriesLoadedValue_.Text = string.Format("{0} / {1}", (long)entries_, totalEntires_);
            prgProgress_.Value = (int)(entries_ / 1024);
        }

        private void btnCancel__Click(object sender, EventArgs e)
        {
            cancel_ = true;
        }

        #endregion // implementation

        #region representation

        private readonly SharedReference<long> entries_;
        private readonly long totalEntires_;
        private readonly Timer timer_ = new Timer();

        private bool cancel_;

        #endregion // representation
    }

    public sealed class SharedReference<T>
    {
        public T Reference { get; set; }

        public static implicit operator T(SharedReference<T> rhs)
        {
            return rhs.Reference;
        }
    }
}
