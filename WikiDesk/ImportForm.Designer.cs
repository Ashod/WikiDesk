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
            this.dateTimePicker_ = new System.Windows.Forms.DateTimePicker();
            this.lblDumpFile_ = new System.Windows.Forms.Label();
            this.lblDomain_ = new System.Windows.Forms.Label();
            this.lblLanguage_ = new System.Windows.Forms.Label();
            this.lblDumpDate_ = new System.Windows.Forms.Label();
            this.chkIndexOnly_ = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            //
            // cboDomains_
            //
            this.cboDomains_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboDomains_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDomains_.FormattingEnabled = true;
            this.cboDomains_.Location = new System.Drawing.Point(12, 89);
            this.cboDomains_.Name = "cboDomains_";
            this.cboDomains_.Size = new System.Drawing.Size(488, 21);
            this.cboDomains_.TabIndex = 0;
            this.cboDomains_.SelectedIndexChanged += new System.EventHandler(this.cboDomains__SelectedIndexChanged);
            //
            // cboLanguages_
            //
            this.cboLanguages_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboLanguages_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLanguages_.FormattingEnabled = true;
            this.cboLanguages_.Location = new System.Drawing.Point(12, 133);
            this.cboLanguages_.Name = "cboLanguages_";
            this.cboLanguages_.Size = new System.Drawing.Size(488, 21);
            this.cboLanguages_.TabIndex = 1;
            this.cboLanguages_.SelectedIndexChanged += new System.EventHandler(this.cboLanguages__SelectedIndexChanged);
            //
            // txtFilepath_
            //
            this.txtFilepath_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilepath_.Enabled = false;
            this.txtFilepath_.Location = new System.Drawing.Point(12, 34);
            this.txtFilepath_.Name = "txtFilepath_";
            this.txtFilepath_.Size = new System.Drawing.Size(447, 20);
            this.txtFilepath_.TabIndex = 2;
            //
            // btnBrowse_
            //
            this.btnBrowse_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse_.Location = new System.Drawing.Point(465, 32);
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
            this.btnImport_.Location = new System.Drawing.Point(425, 249);
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
            this.btnCancel_.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel_.Location = new System.Drawing.Point(318, 249);
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
            // dateTimePicker_
            //
            this.dateTimePicker_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker_.Location = new System.Drawing.Point(12, 177);
            this.dateTimePicker_.Name = "dateTimePicker_";
            this.dateTimePicker_.Size = new System.Drawing.Size(488, 20);
            this.dateTimePicker_.TabIndex = 5;
            this.dateTimePicker_.ValueChanged += new System.EventHandler(this.dateTimePicker__ValueChanged);
            //
            // lblDumpFile_
            //
            this.lblDumpFile_.AutoSize = true;
            this.lblDumpFile_.Location = new System.Drawing.Point(9, 18);
            this.lblDumpFile_.Name = "lblDumpFile_";
            this.lblDumpFile_.Size = new System.Drawing.Size(57, 13);
            this.lblDumpFile_.TabIndex = 6;
            this.lblDumpFile_.Text = "Dump File:";
            //
            // lblDomain_
            //
            this.lblDomain_.AutoSize = true;
            this.lblDomain_.Location = new System.Drawing.Point(9, 73);
            this.lblDomain_.Name = "lblDomain_";
            this.lblDomain_.Size = new System.Drawing.Size(46, 13);
            this.lblDomain_.TabIndex = 7;
            this.lblDomain_.Text = "Domain:";
            //
            // lblLanguage_
            //
            this.lblLanguage_.AutoSize = true;
            this.lblLanguage_.Location = new System.Drawing.Point(9, 117);
            this.lblLanguage_.Name = "lblLanguage_";
            this.lblLanguage_.Size = new System.Drawing.Size(58, 13);
            this.lblLanguage_.TabIndex = 8;
            this.lblLanguage_.Text = "Language:";
            //
            // lblDumpDate_
            //
            this.lblDumpDate_.AutoSize = true;
            this.lblDumpDate_.Location = new System.Drawing.Point(9, 161);
            this.lblDumpDate_.Name = "lblDumpDate_";
            this.lblDumpDate_.Size = new System.Drawing.Size(64, 13);
            this.lblDumpDate_.TabIndex = 9;
            this.lblDumpDate_.Text = "Dump Date:";
            //
            // chkIndexOnly_
            //
            this.chkIndexOnly_.AutoSize = true;
            this.chkIndexOnly_.Location = new System.Drawing.Point(12, 217);
            this.chkIndexOnly_.Name = "chkIndexOnly_";
            this.chkIndexOnly_.Size = new System.Drawing.Size(181, 17);
            this.chkIndexOnly_.TabIndex = 10;
            this.chkIndexOnly_.Text = "Index only, don\'t import contents.";
            this.chkIndexOnly_.UseVisualStyleBackColor = true;
            //
            // ImportForm
            //
            this.AcceptButton = this.btnImport_;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel_;
            this.ClientSize = new System.Drawing.Size(512, 284);
            this.Controls.Add(this.chkIndexOnly_);
            this.Controls.Add(this.lblDumpDate_);
            this.Controls.Add(this.lblLanguage_);
            this.Controls.Add(this.lblDomain_);
            this.Controls.Add(this.lblDumpFile_);
            this.Controls.Add(this.dateTimePicker_);
            this.Controls.Add(this.btnCancel_);
            this.Controls.Add(this.btnImport_);
            this.Controls.Add(this.btnBrowse_);
            this.Controls.Add(this.txtFilepath_);
            this.Controls.Add(this.cboLanguages_);
            this.Controls.Add(this.cboDomains_);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ImportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
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
        private System.Windows.Forms.DateTimePicker dateTimePicker_;
        private System.Windows.Forms.Label lblDumpFile_;
        private System.Windows.Forms.Label lblDomain_;
        private System.Windows.Forms.Label lblLanguage_;
        private System.Windows.Forms.Label lblDumpDate_;
        private System.Windows.Forms.CheckBox chkIndexOnly_;
    }
}