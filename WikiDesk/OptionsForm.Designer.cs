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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tpCache_ = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClearCacheNow_ = new System.Windows.Forms.Button();
            this.chkClearCacheOnExit_ = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cacheSizeBar_ = new System.Windows.Forms.TrackBar();
            this.btnBrowse_ = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCacheFolder_ = new System.Windows.Forms.TextBox();
            this.chkEnableCaching_ = new System.Windows.Forms.CheckBox();
            this.btnSave_ = new System.Windows.Forms.Button();
            this.btnCancel_ = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabs_.SuspendLayout();
            this.tpCache_.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cacheSizeBar_)).BeginInit();
            this.SuspendLayout();
            //
            // tabs_
            //
            this.tabs_.Controls.Add(this.tabPage1);
            this.tabs_.Controls.Add(this.tpCache_);
            this.tabs_.Location = new System.Drawing.Point(0, 0);
            this.tabs_.Name = "tabs_";
            this.tabs_.SelectedIndex = 0;
            this.tabs_.Size = new System.Drawing.Size(527, 265);
            this.tabs_.TabIndex = 0;
            //
            // tabPage1
            //
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(519, 239);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            //
            // tpCache_
            //
            this.tpCache_.Controls.Add(this.label3);
            this.tpCache_.Controls.Add(this.btnClearCacheNow_);
            this.tpCache_.Controls.Add(this.chkClearCacheOnExit_);
            this.tpCache_.Controls.Add(this.label13);
            this.tpCache_.Controls.Add(this.label12);
            this.tpCache_.Controls.Add(this.label11);
            this.tpCache_.Controls.Add(this.label10);
            this.tpCache_.Controls.Add(this.label9);
            this.tpCache_.Controls.Add(this.label8);
            this.tpCache_.Controls.Add(this.label7);
            this.tpCache_.Controls.Add(this.label6);
            this.tpCache_.Controls.Add(this.label5);
            this.tpCache_.Controls.Add(this.label4);
            this.tpCache_.Controls.Add(this.label2);
            this.tpCache_.Controls.Add(this.cacheSizeBar_);
            this.tpCache_.Controls.Add(this.btnBrowse_);
            this.tpCache_.Controls.Add(this.label1);
            this.tpCache_.Controls.Add(this.txtCacheFolder_);
            this.tpCache_.Controls.Add(this.chkEnableCaching_);
            this.tpCache_.Location = new System.Drawing.Point(4, 22);
            this.tpCache_.Name = "tpCache_";
            this.tpCache_.Padding = new System.Windows.Forms.Padding(3);
            this.tpCache_.Size = new System.Drawing.Size(519, 239);
            this.tpCache_.TabIndex = 1;
            this.tpCache_.Text = "Cache";
            this.tpCache_.UseVisualStyleBackColor = true;
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(476, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "128 GB";
            //
            // btnClearCacheNow_
            //
            this.btnClearCacheNow_.Location = new System.Drawing.Point(198, 165);
            this.btnClearCacheNow_.Name = "btnClearCacheNow_";
            this.btnClearCacheNow_.Size = new System.Drawing.Size(110, 23);
            this.btnClearCacheNow_.TabIndex = 9;
            this.btnClearCacheNow_.Text = "Clear cache now";
            this.btnClearCacheNow_.UseVisualStyleBackColor = true;
            //
            // chkClearCacheOnExit_
            //
            this.chkClearCacheOnExit_.AutoSize = true;
            this.chkClearCacheOnExit_.Location = new System.Drawing.Point(32, 169);
            this.chkClearCacheOnExit_.Name = "chkClearCacheOnExit_";
            this.chkClearCacheOnExit_.Size = new System.Drawing.Size(123, 17);
            this.chkClearCacheOnExit_.TabIndex = 8;
            this.chkClearCacheOnExit_.Text = "Clear cache on exit?";
            this.chkClearCacheOnExit_.UseVisualStyleBackColor = true;
            //
            // label13
            //
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(442, 120);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(37, 13);
            this.label13.TabIndex = 7;
            this.label13.Text = "64 GB";
            //
            // label12
            //
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(402, 120);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(37, 13);
            this.label12.TabIndex = 7;
            this.label12.Text = "32 GB";
            //
            // label11
            //
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(362, 120);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(37, 13);
            this.label11.TabIndex = 7;
            this.label11.Text = "16 GB";
            //
            // label10
            //
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(329, 120);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(31, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "8 GB";
            //
            // label9
            //
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(292, 120);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "4 GB";
            //
            // label8
            //
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(255, 120);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "2 GB";
            //
            // label7
            //
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(217, 120);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "1024";
            //
            // label6
            //
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(181, 120);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "512";
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(144, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "256";
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(108, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "128";
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Cache Limit:";
            //
            // cacheSizeBar_
            //
            this.cacheSizeBar_.AutoSize = false;
            this.cacheSizeBar_.Location = new System.Drawing.Point(105, 88);
            this.cacheSizeBar_.Maximum = 11;
            this.cacheSizeBar_.Minimum = 1;
            this.cacheSizeBar_.Name = "cacheSizeBar_";
            this.cacheSizeBar_.Size = new System.Drawing.Size(404, 45);
            this.cacheSizeBar_.TabIndex = 4;
            this.cacheSizeBar_.Value = 1;
            //
            // btnBrowse_
            //
            this.btnBrowse_.Location = new System.Drawing.Point(479, 49);
            this.btnBrowse_.Name = "btnBrowse_";
            this.btnBrowse_.Size = new System.Drawing.Size(30, 23);
            this.btnBrowse_.TabIndex = 3;
            this.btnBrowse_.Text = "...";
            this.btnBrowse_.UseVisualStyleBackColor = true;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Cache Folder:";
            //
            // txtCacheFolder_
            //
            this.txtCacheFolder_.Location = new System.Drawing.Point(105, 51);
            this.txtCacheFolder_.Name = "txtCacheFolder_";
            this.txtCacheFolder_.Size = new System.Drawing.Size(368, 20);
            this.txtCacheFolder_.TabIndex = 1;
            //
            // chkEnableCaching_
            //
            this.chkEnableCaching_.AutoSize = true;
            this.chkEnableCaching_.Location = new System.Drawing.Point(8, 20);
            this.chkEnableCaching_.Name = "chkEnableCaching_";
            this.chkEnableCaching_.Size = new System.Drawing.Size(153, 17);
            this.chkEnableCaching_.TabIndex = 0;
            this.chkEnableCaching_.Text = "Enable media file caching?";
            this.chkEnableCaching_.UseVisualStyleBackColor = true;
            //
            // btnSave_
            //
            this.btnSave_.Location = new System.Drawing.Point(441, 271);
            this.btnSave_.Name = "btnSave_";
            this.btnSave_.Size = new System.Drawing.Size(75, 23);
            this.btnSave_.TabIndex = 1;
            this.btnSave_.Text = "Save";
            this.btnSave_.UseVisualStyleBackColor = true;
            //
            // btnCancel_
            //
            this.btnCancel_.Location = new System.Drawing.Point(333, 271);
            this.btnCancel_.Name = "btnCancel_";
            this.btnCancel_.Size = new System.Drawing.Size(75, 23);
            this.btnCancel_.TabIndex = 2;
            this.btnCancel_.Text = "Cancel";
            this.btnCancel_.UseVisualStyleBackColor = true;
            //
            // openFileDialog1
            //
            this.openFileDialog1.FileName = "openFileDialog_";
            //
            // OptionsForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 306);
            this.Controls.Add(this.btnCancel_);
            this.Controls.Add(this.btnSave_);
            this.Controls.Add(this.tabs_);
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.tabs_.ResumeLayout(false);
            this.tpCache_.ResumeLayout(false);
            this.tpCache_.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cacheSizeBar_)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabs_;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tpCache_;
        private System.Windows.Forms.Button btnSave_;
        private System.Windows.Forms.Button btnCancel_;
        private System.Windows.Forms.TextBox txtCacheFolder_;
        private System.Windows.Forms.CheckBox chkEnableCaching_;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar cacheSizeBar_;
        private System.Windows.Forms.Button btnBrowse_;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnClearCacheNow_;
        private System.Windows.Forms.CheckBox chkClearCacheOnExit_;
        private System.Windows.Forms.Label label3;
    }
}