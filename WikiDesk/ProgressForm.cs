
namespace WikiDesk
{
    using System;
    using System.Windows.Forms;

    public partial class ProgressForm : Form
    {
        public ProgressForm()
        {
            InitializeComponent();
        }

        public bool Cancelled { get; private set; }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = false;
            Cancelled = true;
        }
    }
}
