namespace WikiDesk
{
    partial class ProgressForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.prgProgress = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblRemainingTimeName_ = new System.Windows.Forms.Label();
            this.lblRemainingTimeValue_ = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // txtInfo
            //
            this.txtInfo.Location = new System.Drawing.Point(12, 12);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.Size = new System.Drawing.Size(454, 46);
            this.txtInfo.TabIndex = 0;
            //
            // prgProgress
            //
            this.prgProgress.Location = new System.Drawing.Point(12, 70);
            this.prgProgress.Name = "prgProgress";
            this.prgProgress.Size = new System.Drawing.Size(454, 23);
            this.prgProgress.TabIndex = 1;
            //
            // btnCancel
            //
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(391, 108);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // lblRemainingTimeName_
            //
            this.lblRemainingTimeName_.AutoSize = true;
            this.lblRemainingTimeName_.Location = new System.Drawing.Point(9, 113);
            this.lblRemainingTimeName_.Name = "lblRemainingTimeName_";
            this.lblRemainingTimeName_.Size = new System.Drawing.Size(135, 13);
            this.lblRemainingTimeName_.TabIndex = 3;
            this.lblRemainingTimeName_.Text = "Estimated Remaining Time:";
            //
            // lblRemainingTimeValue_
            //
            this.lblRemainingTimeValue_.AutoSize = true;
            this.lblRemainingTimeValue_.Location = new System.Drawing.Point(150, 113);
            this.lblRemainingTimeValue_.Name = "lblRemainingTimeValue_";
            this.lblRemainingTimeValue_.Size = new System.Drawing.Size(83, 13);
            this.lblRemainingTimeValue_.TabIndex = 3;
            this.lblRemainingTimeValue_.Text = "Remaining Time";
            //
            // ProgressForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(478, 143);
            this.Controls.Add(this.lblRemainingTimeValue_);
            this.Controls.Add(this.lblRemainingTimeName_);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.prgProgress);
            this.Controls.Add(this.txtInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ProgressForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ProgressForm";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.TextBox txtInfo;
        public System.Windows.Forms.ProgressBar prgProgress;
        private System.Windows.Forms.Label lblRemainingTimeName_;
        public System.Windows.Forms.Label lblRemainingTimeValue_;
    }
}