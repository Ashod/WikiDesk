namespace WikiDesk
{
    partial class LoadDatabaseForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCancel_ = new System.Windows.Forms.Button();
            this.lblDatabasePathValue_ = new System.Windows.Forms.Label();
            this.lblEntriesLoadedValue_ = new System.Windows.Forms.Label();
            this.lblEntriesLoadedName_ = new System.Windows.Forms.Label();
            this.prgProgress_ = new System.Windows.Forms.ProgressBar();
            this.lblDatabasePathName_ = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            //
            // groupBox1
            //
            this.groupBox1.Controls.Add(this.btnCancel_);
            this.groupBox1.Controls.Add(this.lblDatabasePathValue_);
            this.groupBox1.Controls.Add(this.lblEntriesLoadedValue_);
            this.groupBox1.Controls.Add(this.lblEntriesLoadedName_);
            this.groupBox1.Controls.Add(this.prgProgress_);
            this.groupBox1.Controls.Add(this.lblDatabasePathName_);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(445, 112);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Loading WikiDesk Database";
            //
            // btnCancel_
            //
            this.btnCancel_.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel_.Location = new System.Drawing.Point(350, 72);
            this.btnCancel_.Name = "btnCancel_";
            this.btnCancel_.Size = new System.Drawing.Size(75, 23);
            this.btnCancel_.TabIndex = 5;
            this.btnCancel_.Text = "Cancel";
            this.btnCancel_.UseVisualStyleBackColor = true;
            //
            // lblDatabasePathValue_
            //
            this.lblDatabasePathValue_.AutoSize = true;
            this.lblDatabasePathValue_.Location = new System.Drawing.Point(100, 25);
            this.lblDatabasePathValue_.Name = "lblDatabasePathValue_";
            this.lblDatabasePathValue_.Size = new System.Drawing.Size(12, 13);
            this.lblDatabasePathValue_.TabIndex = 4;
            this.lblDatabasePathValue_.Text = "\\";
            //
            // lblEntriesLoadedValue_
            //
            this.lblEntriesLoadedValue_.AutoSize = true;
            this.lblEntriesLoadedValue_.Location = new System.Drawing.Point(100, 48);
            this.lblEntriesLoadedValue_.Name = "lblEntriesLoadedValue_";
            this.lblEntriesLoadedValue_.Size = new System.Drawing.Size(13, 13);
            this.lblEntriesLoadedValue_.TabIndex = 3;
            this.lblEntriesLoadedValue_.Text = "0";
            //
            // lblEntriesLoadedName_
            //
            this.lblEntriesLoadedName_.AutoSize = true;
            this.lblEntriesLoadedName_.Location = new System.Drawing.Point(14, 48);
            this.lblEntriesLoadedName_.Name = "lblEntriesLoadedName_";
            this.lblEntriesLoadedName_.Size = new System.Drawing.Size(77, 13);
            this.lblEntriesLoadedName_.TabIndex = 2;
            this.lblEntriesLoadedName_.Text = "Entries loaded:";
            //
            // prgProgress_
            //
            this.prgProgress_.Location = new System.Drawing.Point(17, 72);
            this.prgProgress_.Name = "prgProgress_";
            this.prgProgress_.Size = new System.Drawing.Size(321, 23);
            this.prgProgress_.TabIndex = 1;
            //
            // lblDatabasePathName_
            //
            this.lblDatabasePathName_.AutoSize = true;
            this.lblDatabasePathName_.Location = new System.Drawing.Point(14, 25);
            this.lblDatabasePathName_.Name = "lblDatabasePathName_";
            this.lblDatabasePathName_.Size = new System.Drawing.Size(80, 13);
            this.lblDatabasePathName_.TabIndex = 0;
            this.lblDatabasePathName_.Text = "Database path:";
            //
            // LoadDatabaseForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel_;
            this.ClientSize = new System.Drawing.Size(469, 136);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "LoadDatabaseForm";
            this.Text = "Loading WikiDesk Database";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblDatabasePathValue_;
        private System.Windows.Forms.Label lblEntriesLoadedValue_;
        private System.Windows.Forms.Label lblEntriesLoadedName_;
        private System.Windows.Forms.ProgressBar prgProgress_;
        private System.Windows.Forms.Label lblDatabasePathName_;
        private System.Windows.Forms.Button btnCancel_;
    }
}