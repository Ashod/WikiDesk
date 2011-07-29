namespace WikiDesk
{
    partial class OptionsForm
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
            this.tabs_ = new System.Windows.Forms.TabControl();
            this.tpGen_ = new System.Windows.Forms.TabPage();
            this.gbGeneralSettings_ = new System.Windows.Forms.GroupBox();
            this.btnBrowseDefaultDatabase_ = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.txtDefaultDatabase_ = new System.Windows.Forms.TextBox();
            this.chkTrackBrowseHistory_ = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.numAutoUpdateOld_ = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.chkAutoRetrieveMissing_ = new System.Windows.Forms.CheckBox();
            this.tpCache_ = new System.Windows.Forms.TabPage();
            this.chkEnableCaching_ = new System.Windows.Forms.CheckBox();
            this.gbCacheSettings_ = new System.Windows.Forms.GroupBox();
            this.btnClearCacheNow_ = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.chkClearCacheOnExit_ = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.barCacheSize_ = new System.Windows.Forms.TrackBar();
            this.btnBrowse_ = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCacheFolder_ = new System.Windows.Forms.TextBox();
            this.tpWiki_ = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtCustomCss_ = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.numThumbWidth_ = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.cbSkinName_ = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.btnSave_ = new System.Windows.Forms.Button();
            this.btnCancel_ = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabs_.SuspendLayout();
            this.tpGen_.SuspendLayout();
            this.gbGeneralSettings_.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoUpdateOld_)).BeginInit();
            this.tpCache_.SuspendLayout();
            this.gbCacheSettings_.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barCacheSize_)).BeginInit();
            this.tpWiki_.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numThumbWidth_)).BeginInit();
            this.SuspendLayout();
            // 
            // tabs_
            // 
            this.tabs_.Controls.Add(this.tpGen_);
            this.tabs_.Controls.Add(this.tpCache_);
            this.tabs_.Controls.Add(this.tpWiki_);
            this.tabs_.Location = new System.Drawing.Point(0, 0);
            this.tabs_.Name = "tabs_";
            this.tabs_.SelectedIndex = 0;
            this.tabs_.Size = new System.Drawing.Size(527, 283);
            this.tabs_.TabIndex = 0;
            // 
            // tpGen_
            // 
            this.tpGen_.Controls.Add(this.gbGeneralSettings_);
            this.tpGen_.Location = new System.Drawing.Point(4, 22);
            this.tpGen_.Name = "tpGen_";
            this.tpGen_.Padding = new System.Windows.Forms.Padding(3);
            this.tpGen_.Size = new System.Drawing.Size(519, 257);
            this.tpGen_.TabIndex = 0;
            this.tpGen_.Text = "General";
            this.tpGen_.UseVisualStyleBackColor = true;
            // 
            // gbGeneralSettings_
            // 
            this.gbGeneralSettings_.Controls.Add(this.btnBrowseDefaultDatabase_);
            this.gbGeneralSettings_.Controls.Add(this.label19);
            this.gbGeneralSettings_.Controls.Add(this.txtDefaultDatabase_);
            this.gbGeneralSettings_.Controls.Add(this.chkTrackBrowseHistory_);
            this.gbGeneralSettings_.Controls.Add(this.label15);
            this.gbGeneralSettings_.Controls.Add(this.numAutoUpdateOld_);
            this.gbGeneralSettings_.Controls.Add(this.label14);
            this.gbGeneralSettings_.Controls.Add(this.chkAutoRetrieveMissing_);
            this.gbGeneralSettings_.Location = new System.Drawing.Point(8, 15);
            this.gbGeneralSettings_.Name = "gbGeneralSettings_";
            this.gbGeneralSettings_.Size = new System.Drawing.Size(504, 236);
            this.gbGeneralSettings_.TabIndex = 17;
            this.gbGeneralSettings_.TabStop = false;
            this.gbGeneralSettings_.Text = "WikiDesk Settings";
            // 
            // btnBrowseDefaultDatabase_
            // 
            this.btnBrowseDefaultDatabase_.Location = new System.Drawing.Point(468, 73);
            this.btnBrowseDefaultDatabase_.Name = "btnBrowseDefaultDatabase_";
            this.btnBrowseDefaultDatabase_.Size = new System.Drawing.Size(30, 23);
            this.btnBrowseDefaultDatabase_.TabIndex = 24;
            this.btnBrowseDefaultDatabase_.Text = "...";
            this.btnBrowseDefaultDatabase_.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 78);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(91, 13);
            this.label19.TabIndex = 23;
            this.label19.Text = "Default database:";
            // 
            // txtDefaultDatabase_
            // 
            this.txtDefaultDatabase_.Location = new System.Drawing.Point(103, 75);
            this.txtDefaultDatabase_.Name = "txtDefaultDatabase_";
            this.txtDefaultDatabase_.Size = new System.Drawing.Size(358, 20);
            this.txtDefaultDatabase_.TabIndex = 22;
            // 
            // chkTrackBrowseHistory_
            // 
            this.chkTrackBrowseHistory_.AutoSize = true;
            this.chkTrackBrowseHistory_.Location = new System.Drawing.Point(9, 110);
            this.chkTrackBrowseHistory_.Name = "chkTrackBrowseHistory_";
            this.chkTrackBrowseHistory_.Size = new System.Drawing.Size(132, 17);
            this.chkTrackBrowseHistory_.TabIndex = 21;
            this.chkTrackBrowseHistory_.Text = "Track browsing history";
            this.chkTrackBrowseHistory_.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(219, 48);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(92, 13);
            this.label15.TabIndex = 20;
            this.label15.Text = "days (0 to disable)";
            // 
            // numAutoUpdateOld_
            // 
            this.numAutoUpdateOld_.Location = new System.Drawing.Point(143, 46);
            this.numAutoUpdateOld_.Name = "numAutoUpdateOld_";
            this.numAutoUpdateOld_.Size = new System.Drawing.Size(70, 20);
            this.numAutoUpdateOld_.TabIndex = 19;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 48);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(131, 13);
            this.label14.TabIndex = 18;
            this.label14.Text = "Auto-update entries every:";
            // 
            // chkAutoRetrieveMissing_
            // 
            this.chkAutoRetrieveMissing_.AutoSize = true;
            this.chkAutoRetrieveMissing_.Location = new System.Drawing.Point(9, 19);
            this.chkAutoRetrieveMissing_.Name = "chkAutoRetrieveMissing_";
            this.chkAutoRetrieveMissing_.Size = new System.Drawing.Size(221, 17);
            this.chkAutoRetrieveMissing_.TabIndex = 17;
            this.chkAutoRetrieveMissing_.Text = "Auto-retrieve missing entries from the web";
            this.chkAutoRetrieveMissing_.UseVisualStyleBackColor = true;
            // 
            // tpCache_
            // 
            this.tpCache_.Controls.Add(this.chkEnableCaching_);
            this.tpCache_.Controls.Add(this.gbCacheSettings_);
            this.tpCache_.Location = new System.Drawing.Point(4, 22);
            this.tpCache_.Name = "tpCache_";
            this.tpCache_.Padding = new System.Windows.Forms.Padding(3);
            this.tpCache_.Size = new System.Drawing.Size(519, 257);
            this.tpCache_.TabIndex = 1;
            this.tpCache_.Text = "Cache";
            this.tpCache_.UseVisualStyleBackColor = true;
            // 
            // chkEnableCaching_
            // 
            this.chkEnableCaching_.AutoSize = true;
            this.chkEnableCaching_.Location = new System.Drawing.Point(20, 15);
            this.chkEnableCaching_.Name = "chkEnableCaching_";
            this.chkEnableCaching_.Size = new System.Drawing.Size(153, 17);
            this.chkEnableCaching_.TabIndex = 0;
            this.chkEnableCaching_.Text = "Enable media-file caching?";
            this.chkEnableCaching_.UseVisualStyleBackColor = true;
            // 
            // gbCacheSettings_
            // 
            this.gbCacheSettings_.Controls.Add(this.btnClearCacheNow_);
            this.gbCacheSettings_.Controls.Add(this.label3);
            this.gbCacheSettings_.Controls.Add(this.label13);
            this.gbCacheSettings_.Controls.Add(this.label12);
            this.gbCacheSettings_.Controls.Add(this.chkClearCacheOnExit_);
            this.gbCacheSettings_.Controls.Add(this.label11);
            this.gbCacheSettings_.Controls.Add(this.label10);
            this.gbCacheSettings_.Controls.Add(this.label9);
            this.gbCacheSettings_.Controls.Add(this.label8);
            this.gbCacheSettings_.Controls.Add(this.label7);
            this.gbCacheSettings_.Controls.Add(this.label6);
            this.gbCacheSettings_.Controls.Add(this.label5);
            this.gbCacheSettings_.Controls.Add(this.label4);
            this.gbCacheSettings_.Controls.Add(this.label2);
            this.gbCacheSettings_.Controls.Add(this.barCacheSize_);
            this.gbCacheSettings_.Controls.Add(this.btnBrowse_);
            this.gbCacheSettings_.Controls.Add(this.label1);
            this.gbCacheSettings_.Controls.Add(this.txtCacheFolder_);
            this.gbCacheSettings_.Location = new System.Drawing.Point(8, 15);
            this.gbCacheSettings_.Name = "gbCacheSettings_";
            this.gbCacheSettings_.Size = new System.Drawing.Size(504, 236);
            this.gbCacheSettings_.TabIndex = 11;
            this.gbCacheSettings_.TabStop = false;
            // 
            // btnClearCacheNow_
            // 
            this.btnClearCacheNow_.Location = new System.Drawing.Point(154, 136);
            this.btnClearCacheNow_.Name = "btnClearCacheNow_";
            this.btnClearCacheNow_.Size = new System.Drawing.Size(110, 23);
            this.btnClearCacheNow_.TabIndex = 27;
            this.btnClearCacheNow_.Text = "Clear cache now";
            this.btnClearCacheNow_.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(454, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "128 GB";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(420, 102);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(37, 13);
            this.label13.TabIndex = 21;
            this.label13.Text = "64 GB";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(380, 102);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(37, 13);
            this.label12.TabIndex = 20;
            this.label12.Text = "32 GB";
            // 
            // chkClearCacheOnExit_
            // 
            this.chkClearCacheOnExit_.AutoSize = true;
            this.chkClearCacheOnExit_.Location = new System.Drawing.Point(15, 140);
            this.chkClearCacheOnExit_.Name = "chkClearCacheOnExit_";
            this.chkClearCacheOnExit_.Size = new System.Drawing.Size(123, 17);
            this.chkClearCacheOnExit_.TabIndex = 8;
            this.chkClearCacheOnExit_.Text = "Clear cache on exit?";
            this.chkClearCacheOnExit_.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(340, 102);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(37, 13);
            this.label11.TabIndex = 19;
            this.label11.Text = "16 GB";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(307, 102);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(31, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "8 GB";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(270, 102);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 13);
            this.label9.TabIndex = 25;
            this.label9.Text = "4 GB";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(233, 102);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "2 GB";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(195, 102);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "1 GB";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(159, 102);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = ".5 GB";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(122, 102);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = ".2 GB";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(86, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = ".1 GB";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Cache Limit:";
            // 
            // barCacheSize_
            // 
            this.barCacheSize_.AutoSize = false;
            this.barCacheSize_.LargeChange = 2;
            this.barCacheSize_.Location = new System.Drawing.Point(83, 70);
            this.barCacheSize_.Maximum = 11;
            this.barCacheSize_.Minimum = 1;
            this.barCacheSize_.Name = "barCacheSize_";
            this.barCacheSize_.Size = new System.Drawing.Size(404, 45);
            this.barCacheSize_.TabIndex = 14;
            this.barCacheSize_.Value = 1;
            // 
            // btnBrowse_
            // 
            this.btnBrowse_.Location = new System.Drawing.Point(457, 31);
            this.btnBrowse_.Name = "btnBrowse_";
            this.btnBrowse_.Size = new System.Drawing.Size(30, 23);
            this.btnBrowse_.TabIndex = 13;
            this.btnBrowse_.Text = "...";
            this.btnBrowse_.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Cache Folder:";
            // 
            // txtCacheFolder_
            // 
            this.txtCacheFolder_.Location = new System.Drawing.Point(83, 33);
            this.txtCacheFolder_.Name = "txtCacheFolder_";
            this.txtCacheFolder_.Size = new System.Drawing.Size(368, 20);
            this.txtCacheFolder_.TabIndex = 11;
            // 
            // tpWiki_
            // 
            this.tpWiki_.Controls.Add(this.groupBox1);
            this.tpWiki_.Location = new System.Drawing.Point(4, 22);
            this.tpWiki_.Name = "tpWiki_";
            this.tpWiki_.Size = new System.Drawing.Size(519, 257);
            this.tpWiki_.TabIndex = 2;
            this.tpWiki_.Text = "Wiki";
            this.tpWiki_.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.txtCustomCss_);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.numThumbWidth_);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.cbSkinName_);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Location = new System.Drawing.Point(8, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(504, 232);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Wiki Display Settings";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(6, 76);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(249, 13);
            this.label20.TabIndex = 8;
            this.label20.Text = "Custom CSS: (overrides skin, embedded into page.)";
            // 
            // txtCustomCss_
            // 
            this.txtCustomCss_.AcceptsReturn = true;
            this.txtCustomCss_.AcceptsTab = true;
            this.txtCustomCss_.Location = new System.Drawing.Point(6, 92);
            this.txtCustomCss_.MaxLength = 65000;
            this.txtCustomCss_.Multiline = true;
            this.txtCustomCss_.Name = "txtCustomCss_";
            this.txtCustomCss_.Size = new System.Drawing.Size(492, 134);
            this.txtCustomCss_.TabIndex = 7;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(175, 23);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(100, 13);
            this.label17.TabIndex = 6;
            this.label17.Text = "pixels (Default: 220)";
            // 
            // numThumbWidth_
            // 
            this.numThumbWidth_.Location = new System.Drawing.Point(99, 21);
            this.numThumbWidth_.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numThumbWidth_.Minimum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numThumbWidth_.Name = "numThumbWidth_";
            this.numThumbWidth_.Size = new System.Drawing.Size(70, 20);
            this.numThumbWidth_.TabIndex = 5;
            this.numThumbWidth_.Value = new decimal(new int[] {
            220,
            0,
            0,
            0});
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 23);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(87, 13);
            this.label18.TabIndex = 4;
            this.label18.Text = "Thumbnail width:";
            // 
            // cbSkinName_
            // 
            this.cbSkinName_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSkinName_.FormattingEnabled = true;
            this.cbSkinName_.Location = new System.Drawing.Point(47, 47);
            this.cbSkinName_.Name = "cbSkinName_";
            this.cbSkinName_.Size = new System.Drawing.Size(150, 21);
            this.cbSkinName_.TabIndex = 1;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(10, 50);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(31, 13);
            this.label16.TabIndex = 0;
            this.label16.Text = "Skin:";
            // 
            // btnSave_
            // 
            this.btnSave_.Location = new System.Drawing.Point(315, 289);
            this.btnSave_.Name = "btnSave_";
            this.btnSave_.Size = new System.Drawing.Size(75, 23);
            this.btnSave_.TabIndex = 1;
            this.btnSave_.Text = "Save";
            this.btnSave_.UseVisualStyleBackColor = true;
            // 
            // btnCancel_
            // 
            this.btnCancel_.Location = new System.Drawing.Point(441, 289);
            this.btnCancel_.Name = "btnCancel_";
            this.btnCancel_.Size = new System.Drawing.Size(75, 23);
            this.btnCancel_.TabIndex = 2;
            this.btnCancel_.Text = "Cancel";
            this.btnCancel_.UseVisualStyleBackColor = true;
            this.btnCancel_.Click += new System.EventHandler(this.btnCancel__Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog_";
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 324);
            this.Controls.Add(this.btnCancel_);
            this.Controls.Add(this.btnSave_);
            this.Controls.Add(this.tabs_);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.tabs_.ResumeLayout(false);
            this.tpGen_.ResumeLayout(false);
            this.gbGeneralSettings_.ResumeLayout(false);
            this.gbGeneralSettings_.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoUpdateOld_)).EndInit();
            this.tpCache_.ResumeLayout(false);
            this.tpCache_.PerformLayout();
            this.gbCacheSettings_.ResumeLayout(false);
            this.gbCacheSettings_.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barCacheSize_)).EndInit();
            this.tpWiki_.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numThumbWidth_)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabs_;
        private System.Windows.Forms.TabPage tpGen_;
        private System.Windows.Forms.TabPage tpCache_;
        private System.Windows.Forms.Button btnSave_;
        private System.Windows.Forms.Button btnCancel_;
        private System.Windows.Forms.CheckBox chkEnableCaching_;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox chkClearCacheOnExit_;
        private System.Windows.Forms.GroupBox gbCacheSettings_;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar barCacheSize_;
        private System.Windows.Forms.Button btnBrowse_;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCacheFolder_;
        private System.Windows.Forms.TabPage tpWiki_;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbSkinName_;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown numThumbWidth_;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtCustomCss_;
        private System.Windows.Forms.GroupBox gbGeneralSettings_;
        private System.Windows.Forms.Button btnBrowseDefaultDatabase_;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtDefaultDatabase_;
        private System.Windows.Forms.CheckBox chkTrackBrowseHistory_;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown numAutoUpdateOld_;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox chkAutoRetrieveMissing_;
        private System.Windows.Forms.Button btnClearCacheNow_;
    }
}