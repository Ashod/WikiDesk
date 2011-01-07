namespace WikiDesk
{
    partial class ImportForm
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
            this.cboDomains_ = new System.Windows.Forms.ComboBox();
            this.cboLanguages_ = new System.Windows.Forms.ComboBox();
            this.txtFilepath_ = new System.Windows.Forms.TextBox();
            this.btnBrowse_ = new System.Windows.Forms.Button();
            this.btnImport_ = new System.Windows.Forms.Button();
            this.btnCancel_ = new System.Windows.Forms.Button();
            this.openFileDialog_ = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // cboDomains_
            // 
            this.cboDomains_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboDomains_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDomains_.FormattingEnabled = true;
            this.cboDomains_.Location = new System.Drawing.Point(12, 49);
            this.cboDomains_.Name = "cboDomains_";
            this.cboDomains_.Size = new System.Drawing.Size(352, 21);
            this.cboDomains_.TabIndex = 0;
            this.cboDomains_.SelectedIndexChanged += new System.EventHandler(this.cboDomains__SelectedIndexChanged);
            // 
            // cboLanguages_
            // 
            this.cboLanguages_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboLanguages_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLanguages_.FormattingEnabled = true;
            this.cboLanguages_.Location = new System.Drawing.Point(12, 76);
            this.cboLanguages_.Name = "cboLanguages_";
            this.cboLanguages_.Size = new System.Drawing.Size(352, 21);
            this.cboLanguages_.TabIndex = 1;
            // 
            // txtFilepath_
            // 
            this.txtFilepath_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilepath_.Enabled = false;
            this.txtFilepath_.Location = new System.Drawing.Point(12, 12);
            this.txtFilepath_.Name = "txtFilepath_";
            this.txtFilepath_.Size = new System.Drawing.Size(311, 20);
            this.txtFilepath_.TabIndex = 2;
            // 
            // btnBrowse_
            // 
            this.btnBrowse_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse_.Location = new System.Drawing.Point(329, 10);
            this.btnBrowse_.Name = "btnBrowse_";
            this.btnBrowse_.Size = new System.Drawing.Size(35, 23);
            this.btnBrowse_.TabIndex = 3;
            this.btnBrowse_.Text = "...";
            this.btnBrowse_.UseVisualStyleBackColor = true;
            this.btnBrowse_.Click += new System.EventHandler(this.btnBrowse__Click);
            // 
            // btnImport_
            // 
            this.btnImport_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport_.Location = new System.Drawing.Point(289, 118);
            this.btnImport_.Name = "btnImport_";
            this.btnImport_.Size = new System.Drawing.Size(75, 23);
            this.btnImport_.TabIndex = 4;
            this.btnImport_.Text = "Import";
            this.btnImport_.UseVisualStyleBackColor = true;
            this.btnImport_.Click += new System.EventHandler(this.btnImport__Click);
            // 
            // btnCancel_
            // 
            this.btnCancel_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel_.Location = new System.Drawing.Point(182, 118);
            this.btnCancel_.Name = "btnCancel_";
            this.btnCancel_.Size = new System.Drawing.Size(75, 23);
            this.btnCancel_.TabIndex = 4;
            this.btnCancel_.Text = "Cancel";
            this.btnCancel_.UseVisualStyleBackColor = true;
            this.btnCancel_.Click += new System.EventHandler(this.btnCancel__Click);
            // 
            // openFileDialog_
            // 
            this.openFileDialog_.ReadOnlyChecked = true;
            this.openFileDialog_.ShowReadOnly = true;
            // 
            // ImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 153);
            this.Controls.Add(this.btnCancel_);
            this.Controls.Add(this.btnImport_);
            this.Controls.Add(this.btnBrowse_);
            this.Controls.Add(this.txtFilepath_);
            this.Controls.Add(this.cboLanguages_);
            this.Controls.Add(this.cboDomains_);
            this.MaximizeBox = false;
            this.Name = "ImportForm";
            this.Text = "Import Wiki Dump";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboDomains_;
        private System.Windows.Forms.ComboBox cboLanguages_;
        private System.Windows.Forms.TextBox txtFilepath_;
        private System.Windows.Forms.Button btnBrowse_;
        private System.Windows.Forms.Button btnImport_;
        private System.Windows.Forms.Button btnCancel_;
        private System.Windows.Forms.OpenFileDialog openFileDialog_;
    }
}