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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportForm));
            this.btnImport_ = new System.Windows.Forms.Button();
            this.btnCancel_ = new System.Windows.Forms.Button();
            this.openFileDialog_ = new System.Windows.Forms.OpenFileDialog();
            this.chkIndexOnly_ = new System.Windows.Forms.CheckBox();
            this.btnBrowse_ = new System.Windows.Forms.Button();
            this.txtFileDumpPath_ = new System.Windows.Forms.TextBox();
            this.rdFileDump_ = new System.Windows.Forms.RadioButton();
            this.rdWebDump_ = new System.Windows.Forms.RadioButton();
            this.txtWebDumpUrl_ = new System.Windows.Forms.TextBox();
            this.gbSourceType_ = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.gbDumpInfo_ = new System.Windows.Forms.GroupBox();
            this.lblDumpDate_ = new System.Windows.Forms.Label();
            this.lblLanguage_ = new System.Windows.Forms.Label();
            this.lblDomain_ = new System.Windows.Forms.Label();
            this.dateTimePicker_ = new System.Windows.Forms.DateTimePicker();
            this.cboLanguages_ = new System.Windows.Forms.ComboBox();
            this.cboDomains_ = new System.Windows.Forms.ComboBox();
            this.gbImportOptions_ = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.numResumePosKbytes_ = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.gbSourceType_.SuspendLayout();
            this.gbDumpInfo_.SuspendLayout();
            this.gbImportOptions_.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numResumePosKbytes_)).BeginInit();
            this.SuspendLayout();
            //
            // btnImport_
            //            this.btnImport_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport_.Location = new System.Drawing.Point(377, 455);
            this.btnImport_.Name = "btnImport_";
            this.btnImport_.Size = new System.Drawing.Size(75, 23);
            this.btnImport_.TabIndex = 4;
            this.btnImport_.Text = "Import";
            this.btnImport_.UseVisualStyleBackColor = true;
            this.btnImport_.Click += new System.EventHandler(this.btnImport__Click);
            //
            // btnCancel_
            //
            this.btnCancel_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel_.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel_.Location = new System.Drawing.Point(497, 455);
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
            // chkIndexOnly_
            //
            this.chkIndexOnly_.AutoSize = true;
            this.chkIndexOnly_.Location = new System.Drawing.Point(9, 49);
            this.chkIndexOnly_.Name = "chkIndexOnly_";
            this.chkIndexOnly_.Size = new System.Drawing.Size(181, 17);
            this.chkIndexOnly_.TabIndex = 10;
            this.chkIndexOnly_.Text = "Index only, don\'t import contents.";
            this.chkIndexOnly_.UseVisualStyleBackColor = true;
            //
            // btnBrowse_
            //
            this.btnBrowse_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse_.Location = new System.Drawing.Point(528, 39);
            this.btnBrowse_.Name = "btnBrowse_";
            this.btnBrowse_.Size = new System.Drawing.Size(25, 23);
            this.btnBrowse_.TabIndex = 3;
            this.btnBrowse_.Text = "...";
            this.btnBrowse_.UseVisualStyleBackColor = true;
            this.btnBrowse_.Click += new System.EventHandler(this.btnBrowse__Click);
            //
            // txtFileDumpPath_
            //
            this.txtFileDumpPath_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileDumpPath_.Enabled = false;
            this.txtFileDumpPath_.Location = new System.Drawing.Point(19, 41);
            this.txtFileDumpPath_.Name = "txtFileDumpPath_";
            this.txtFileDumpPath_.Size = new System.Drawing.Size(503, 20);
            this.txtFileDumpPath_.TabIndex = 2;
            //
            // rdFileDump_
            //
            this.rdFileDump_.AutoSize = true;
            this.rdFileDump_.Checked = true;
            this.rdFileDump_.Location = new System.Drawing.Point(9, 19);
            this.rdFileDump_.Name = "rdFileDump_";
            this.rdFileDump_.Size = new System.Drawing.Size(365, 17);
            this.rdFileDump_.TabIndex = 4;
            this.rdFileDump_.TabStop = true;
            this.rdFileDump_.Text = "File Dump: (e.g. C:\\Downloads\\enwiki-20110405-pages-articles.xml.bz2)";
            this.rdFileDump_.UseVisualStyleBackColor = true;
            this.rdFileDump_.CheckedChanged += new System.EventHandler(this.rdFileDump_CheckedChanged);
            //
            // rdWebDump_
            //
            this.rdWebDump_.AutoSize = true;
            this.rdWebDump_.Location = new System.Drawing.Point(9, 67);
            this.rdWebDump_.Name = "rdWebDump_";
            this.rdWebDump_.Size = new System.Drawing.Size(536, 43);
            this.rdWebDump_.TabIndex = 5;
            this.rdWebDump_.Text = resources.GetString("rdWebDump_.Text");
            this.rdWebDump_.UseVisualStyleBackColor = true;
            this.rdWebDump_.CheckedChanged += new System.EventHandler(this.rdWebDump_CheckedChanged);
            //
            // txtWebDumpUrl_
            //
            this.txtWebDumpUrl_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWebDumpUrl_.Enabled = false;
            this.txtWebDumpUrl_.Location = new System.Drawing.Point(31, 128);
            this.txtWebDumpUrl_.Name = "txtWebDumpUrl_";
            this.txtWebDumpUrl_.Size = new System.Drawing.Size(534, 20);
            this.txtWebDumpUrl_.TabIndex = 6;
            this.txtWebDumpUrl_.Text = "http://download.wikimedia.org/";
            this.txtWebDumpUrl_.TextChanged += new System.EventHandler(this.txtWebDumpUrl__TextChanged);
            //
            // gbSourceType_
            //
            this.gbSourceType_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSourceType_.Controls.Add(this.label6);
            this.gbSourceType_.Controls.Add(this.rdWebDump_);
            this.gbSourceType_.Controls.Add(this.rdFileDump_);
            this.gbSourceType_.Controls.Add(this.label5);
            this.gbSourceType_.Controls.Add(this.txtFileDumpPath_);
            this.gbSourceType_.Controls.Add(this.btnBrowse_);
            this.gbSourceType_.Controls.Add(this.label4);
            this.gbSourceType_.Controls.Add(this.comboBox3);
            this.gbSourceType_.Controls.Add(this.comboBox4);
            this.gbSourceType_.Controls.Add(this.dateTimePicker2);
            this.gbSourceType_.Location = new System.Drawing.Point(12, 12);
            this.gbSourceType_.Name = "gbSourceType_";
            this.gbSourceType_.Size = new System.Drawing.Size(560, 148);
            this.gbSourceType_.TabIndex = 11;
            this.gbSourceType_.TabStop = false;
            this.gbSourceType_.Text = "Source";
            //
            // label6
            //
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(-45, -103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Dump Date:";
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(-45, -147);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Language:";
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-45, -191);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Domain:";
            //
            // comboBox3
            //
            this.comboBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(-42, -175);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(901, 21);
            this.comboBox3.TabIndex = 0;
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.cboDomains__SelectedIndexChanged);
            //
            // comboBox4
            //
            this.comboBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(-42, -131);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(901, 21);
            this.comboBox4.TabIndex = 1;
            this.comboBox4.SelectedIndexChanged += new System.EventHandler(this.cboLanguages__SelectedIndexChanged);
            //
            // dateTimePicker2
            //
            this.dateTimePicker2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker2.Location = new System.Drawing.Point(-42, -87);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(901, 20);
            this.dateTimePicker2.TabIndex = 5;
            this.dateTimePicker2.ValueChanged += new System.EventHandler(this.dateTimePicker__ValueChanged);
            //
            // gbDumpInfo_
            //
            this.gbDumpInfo_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDumpInfo_.Controls.Add(this.lblDumpDate_);
            this.gbDumpInfo_.Controls.Add(this.lblLanguage_);
            this.gbDumpInfo_.Controls.Add(this.lblDomain_);
            this.gbDumpInfo_.Controls.Add(this.dateTimePicker_);
            this.gbDumpInfo_.Controls.Add(this.cboLanguages_);
            this.gbDumpInfo_.Controls.Add(this.cboDomains_);
            this.gbDumpInfo_.Location = new System.Drawing.Point(12, 166);
            this.gbDumpInfo_.Name = "gbDumpInfo_";
            this.gbDumpInfo_.Size = new System.Drawing.Size(560, 161);
            this.gbDumpInfo_.TabIndex = 12;
            this.gbDumpInfo_.TabStop = false;
            this.gbDumpInfo_.Text = "Dump Info";
            //
            // lblDumpDate_
            //
            this.lblDumpDate_.AutoSize = true;
            this.lblDumpDate_.Location = new System.Drawing.Point(6, 109);
            this.lblDumpDate_.Name = "lblDumpDate_";
            this.lblDumpDate_.Size = new System.Drawing.Size(179, 13);
            this.lblDumpDate_.TabIndex = 21;
            this.lblDumpDate_.Text = "Dump Date: (used for auto-updating)";
            //
            // lblLanguage_
            //
            this.lblLanguage_.AutoSize = true;
            this.lblLanguage_.Location = new System.Drawing.Point(6, 65);
            this.lblLanguage_.Name = "lblLanguage_";
            this.lblLanguage_.Size = new System.Drawing.Size(58, 13);
            this.lblLanguage_.TabIndex = 19;
            this.lblLanguage_.Text = "Language:";
            //
            // lblDomain_
            //
            this.lblDomain_.AutoSize = true;
            this.lblDomain_.Location = new System.Drawing.Point(6, 21);
            this.lblDomain_.Name = "lblDomain_";
            this.lblDomain_.Size = new System.Drawing.Size(46, 13);
            this.lblDomain_.TabIndex = 17;
            this.lblDomain_.Text = "Domain:";
            //
            // dateTimePicker_
            //
            this.dateTimePicker_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker_.Location = new System.Drawing.Point(9, 125);
            this.dateTimePicker_.Name = "dateTimePicker_";
            this.dateTimePicker_.Size = new System.Drawing.Size(535, 20);
            this.dateTimePicker_.TabIndex = 14;
            //
            // cboLanguages_
            //
            this.cboLanguages_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboLanguages_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLanguages_.FormattingEnabled = true;
            this.cboLanguages_.Location = new System.Drawing.Point(9, 81);
            this.cboLanguages_.Name = "cboLanguages_";
            this.cboLanguages_.Size = new System.Drawing.Size(535, 21);
            this.cboLanguages_.TabIndex = 13;
            //
            // cboDomains_
            //
            this.cboDomains_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboDomains_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDomains_.FormattingEnabled = true;
            this.cboDomains_.Location = new System.Drawing.Point(9, 37);
            this.cboDomains_.Name = "cboDomains_";
            this.cboDomains_.Size = new System.Drawing.Size(535, 21);
            this.cboDomains_.TabIndex = 11;
            //
            // gbImportOptions_
            //
            this.gbImportOptions_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbImportOptions_.Controls.Add(this.checkBox1);
            this.gbImportOptions_.Controls.Add(this.numResumePosKbytes_);
            this.gbImportOptions_.Controls.Add(this.label8);
            this.gbImportOptions_.Controls.Add(this.label7);
            this.gbImportOptions_.Controls.Add(this.chkIndexOnly_);
            this.gbImportOptions_.Location = new System.Drawing.Point(12, 337);
            this.gbImportOptions_.Name = "gbImportOptions_";
            this.gbImportOptions_.Size = new System.Drawing.Size(560, 105);
            this.gbImportOptions_.TabIndex = 13;
            this.gbImportOptions_.TabStop = false;
            this.gbImportOptions_.Text = "Import Options";
            //
            // checkBox1
            //
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(9, 74);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(240, 17);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "Import latest revision (ignoring revision hisotry)";
            this.checkBox1.UseVisualStyleBackColor = true;
            //
            // numResumePosKbytes_
            //
            this.numResumePosKbytes_.Location = new System.Drawing.Point(105, 23);
            this.numResumePosKbytes_.Maximum = new decimal(new int[] {
            512000000,
            0,
            0,
            0});
            this.numResumePosKbytes_.Name = "numResumePosKbytes_";
            this.numResumePosKbytes_.Size = new System.Drawing.Size(120, 20);
            this.numResumePosKbytes_.TabIndex = 12;
            this.numResumePosKbytes_.ThousandsSeparator = true;
            //
            // label8
            //
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(231, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "KBytes";
            //
            // label7
            //
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Resume at offset: ";
            //
            // ImportForm
            //
            this.AcceptButton = this.btnImport_;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel_;
            this.ClientSize = new System.Drawing.Size(584, 490);
            this.Controls.Add(this.txtWebDumpUrl_);
            this.Controls.Add(this.gbImportOptions_);
            this.Controls.Add(this.gbDumpInfo_);
            this.Controls.Add(this.gbSourceType_);
            this.Controls.Add(this.btnCancel_);
            this.Controls.Add(this.btnImport_);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ImportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import Wiki Dump";
            this.gbSourceType_.ResumeLayout(false);
            this.gbSourceType_.PerformLayout();
            this.gbDumpInfo_.ResumeLayout(false);
            this.gbDumpInfo_.PerformLayout();
            this.gbImportOptions_.ResumeLayout(false);
            this.gbImportOptions_.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numResumePosKbytes_)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnImport_;
        private System.Windows.Forms.Button btnCancel_;
        private System.Windows.Forms.OpenFileDialog openFileDialog_;
        private System.Windows.Forms.CheckBox chkIndexOnly_;
        private System.Windows.Forms.Button btnBrowse_;
        private System.Windows.Forms.TextBox txtFileDumpPath_;
        private System.Windows.Forms.RadioButton rdFileDump_;
        private System.Windows.Forms.RadioButton rdWebDump_;
        private System.Windows.Forms.TextBox txtWebDumpUrl_;
        private System.Windows.Forms.GroupBox gbSourceType_;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.GroupBox gbDumpInfo_;
        private System.Windows.Forms.Label lblDumpDate_;
        private System.Windows.Forms.Label lblLanguage_;
        private System.Windows.Forms.Label lblDomain_;
        private System.Windows.Forms.DateTimePicker dateTimePicker_;
        private System.Windows.Forms.ComboBox cboLanguages_;
        private System.Windows.Forms.ComboBox cboDomains_;
        private System.Windows.Forms.GroupBox gbImportOptions_;
        private System.Windows.Forms.NumericUpDown numResumePosKbytes_;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}