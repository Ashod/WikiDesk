namespace WikiDesk
{
    partial class WikiDomainsForm
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
            this.lvDomains_ = new System.Windows.Forms.ListView();
            this.colName_ = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDomain_ = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFullPath_ = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFriendlyPath_ = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRemove_ = new System.Windows.Forms.Button();
            this.btnAddUpdate_ = new System.Windows.Forms.Button();
            this.gbDomainProperties_ = new System.Windows.Forms.GroupBox();
            this.txtFriendlyPath_ = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFullPath_ = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDomain_ = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName_ = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel_ = new System.Windows.Forms.Button();
            this.btnSave_ = new System.Windows.Forms.Button();
            this.gbDomains_ = new System.Windows.Forms.GroupBox();
            this.gbDomainProperties_.SuspendLayout();
            this.gbDomains_.SuspendLayout();
            this.SuspendLayout();
            //
            // lvDomains_
            //
            this.lvDomains_.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvDomains_.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName_,
            this.colDomain_,
            this.colFullPath_,
            this.colFriendlyPath_});
            this.lvDomains_.FullRowSelect = true;
            this.lvDomains_.Location = new System.Drawing.Point(6, 19);
            this.lvDomains_.MultiSelect = false;
            this.lvDomains_.Name = "lvDomains_";
            this.lvDomains_.Size = new System.Drawing.Size(518, 154);
            this.lvDomains_.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvDomains_.TabIndex = 0;
            this.lvDomains_.UseCompatibleStateImageBehavior = false;
            this.lvDomains_.View = System.Windows.Forms.View.Details;
            this.lvDomains_.SelectedIndexChanged += new System.EventHandler(this.lvDomains__SelectedIndexChanged);
            //
            // colName_
            //
            this.colName_.Text = "Name";
            this.colName_.Width = 110;
            //
            // colDomain_
            //
            this.colDomain_.Text = "Domain";
            this.colDomain_.Width = 110;
            //
            // colFullPath_
            //
            this.colFullPath_.Text = "Full Path";
            this.colFullPath_.Width = 160;
            //
            // colFriendlyPath_
            //
            this.colFriendlyPath_.Text = "Friendly Path";
            this.colFriendlyPath_.Width = 130;
            //
            // btnRemove_
            //
            this.btnRemove_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove_.Location = new System.Drawing.Point(449, 179);
            this.btnRemove_.Name = "btnRemove_";
            this.btnRemove_.Size = new System.Drawing.Size(75, 23);
            this.btnRemove_.TabIndex = 3;
            this.btnRemove_.Text = "Remove";
            this.btnRemove_.UseVisualStyleBackColor = true;
            this.btnRemove_.Click += new System.EventHandler(this.btnRemove__Click);
            //
            // btnAddUpdate_
            //
            this.btnAddUpdate_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddUpdate_.Location = new System.Drawing.Point(335, 179);
            this.btnAddUpdate_.Name = "btnAddUpdate_";
            this.btnAddUpdate_.Size = new System.Drawing.Size(75, 23);
            this.btnAddUpdate_.TabIndex = 4;
            this.btnAddUpdate_.Text = "Add";
            this.btnAddUpdate_.UseVisualStyleBackColor = true;
            this.btnAddUpdate_.Click += new System.EventHandler(this.btnAddUpdate__Click);
            //
            // gbDomainProperties_
            //
            this.gbDomainProperties_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDomainProperties_.Controls.Add(this.txtFriendlyPath_);
            this.gbDomainProperties_.Controls.Add(this.label4);
            this.gbDomainProperties_.Controls.Add(this.txtFullPath_);
            this.gbDomainProperties_.Controls.Add(this.label3);
            this.gbDomainProperties_.Controls.Add(this.txtDomain_);
            this.gbDomainProperties_.Controls.Add(this.label2);
            this.gbDomainProperties_.Controls.Add(this.txtName_);
            this.gbDomainProperties_.Controls.Add(this.label1);
            this.gbDomainProperties_.Location = new System.Drawing.Point(12, 226);
            this.gbDomainProperties_.Name = "gbDomainProperties_";
            this.gbDomainProperties_.Size = new System.Drawing.Size(530, 92);
            this.gbDomainProperties_.TabIndex = 5;
            this.gbDomainProperties_.TabStop = false;
            this.gbDomainProperties_.Text = "Domain Properties";
            //
            // txtFriendlyPath_
            //
            this.txtFriendlyPath_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFriendlyPath_.Location = new System.Drawing.Point(336, 56);
            this.txtFriendlyPath_.Name = "txtFriendlyPath_";
            this.txtFriendlyPath_.Size = new System.Drawing.Size(180, 20);
            this.txtFriendlyPath_.TabIndex = 26;
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(259, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "Friendly Path:";
            //
            // txtFullPath_
            //
            this.txtFullPath_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFullPath_.Location = new System.Drawing.Point(336, 26);
            this.txtFullPath_.Name = "txtFullPath_";
            this.txtFullPath_.Size = new System.Drawing.Size(180, 20);
            this.txtFullPath_.TabIndex = 24;
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(279, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Full Path:";
            //
            // txtDomain_
            //
            this.txtDomain_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDomain_.Location = new System.Drawing.Point(67, 52);
            this.txtDomain_.Name = "txtDomain_";
            this.txtDomain_.Size = new System.Drawing.Size(180, 20);
            this.txtDomain_.TabIndex = 22;
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Domain:";
            //
            // txtName_
            //
            this.txtName_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName_.Location = new System.Drawing.Point(67, 26);
            this.txtName_.Name = "txtName_";
            this.txtName_.Size = new System.Drawing.Size(180, 20);
            this.txtName_.TabIndex = 20;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Name:";
            //
            // btnCancel_
            //
            this.btnCancel_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel_.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel_.Location = new System.Drawing.Point(467, 332);
            this.btnCancel_.Name = "btnCancel_";
            this.btnCancel_.Size = new System.Drawing.Size(75, 25);
            this.btnCancel_.TabIndex = 6;
            this.btnCancel_.Text = "Cancel";
            this.btnCancel_.UseVisualStyleBackColor = true;
            //
            // btnSave_
            //
            this.btnSave_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave_.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave_.Location = new System.Drawing.Point(347, 332);
            this.btnSave_.Name = "btnSave_";
            this.btnSave_.Size = new System.Drawing.Size(75, 25);
            this.btnSave_.TabIndex = 7;
            this.btnSave_.Text = "OK";
            this.btnSave_.UseVisualStyleBackColor = true;
            this.btnSave_.Click += new System.EventHandler(this.btnSave__Click);
            //
            // gbDomains_
            //
            this.gbDomains_.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDomains_.Controls.Add(this.lvDomains_);
            this.gbDomains_.Controls.Add(this.btnRemove_);
            this.gbDomains_.Controls.Add(this.btnAddUpdate_);
            this.gbDomains_.Location = new System.Drawing.Point(12, 12);
            this.gbDomains_.Name = "gbDomains_";
            this.gbDomains_.Size = new System.Drawing.Size(530, 208);
            this.gbDomains_.TabIndex = 8;
            this.gbDomains_.TabStop = false;
            this.gbDomains_.Text = "Domains";
            //
            // WikiDomainsForm
            //
            this.AcceptButton = this.btnSave_;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel_;
            this.ClientSize = new System.Drawing.Size(554, 369);
            this.Controls.Add(this.gbDomains_);
            this.Controls.Add(this.btnSave_);
            this.Controls.Add(this.btnCancel_);
            this.Controls.Add(this.gbDomainProperties_);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(560, 800);
            this.MinimumSize = new System.Drawing.Size(560, 400);
            this.Name = "WikiDomainsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Wiki Domains Editor";
            this.Load += new System.EventHandler(this.WikiDomainsForm_Load);
            this.gbDomainProperties_.ResumeLayout(false);
            this.gbDomainProperties_.PerformLayout();
            this.gbDomains_.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvDomains_;
        private System.Windows.Forms.ColumnHeader colName_;
        private System.Windows.Forms.ColumnHeader colDomain_;
        private System.Windows.Forms.ColumnHeader colFullPath_;
        private System.Windows.Forms.ColumnHeader colFriendlyPath_;
        private System.Windows.Forms.Button btnRemove_;
        private System.Windows.Forms.Button btnAddUpdate_;
        private System.Windows.Forms.GroupBox gbDomainProperties_;
        private System.Windows.Forms.Button btnCancel_;
        private System.Windows.Forms.Button btnSave_;
        private System.Windows.Forms.GroupBox gbDomains_;
        private System.Windows.Forms.TextBox txtFriendlyPath_;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFullPath_;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName_;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDomain_;
    }
}