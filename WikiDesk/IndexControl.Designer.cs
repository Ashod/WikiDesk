namespace WikiDesk
{
    partial class IndexControl
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cboLanguages_ = new System.Windows.Forms.ComboBox();
            this.txtTitle_ = new System.Windows.Forms.TextBox();
            this.lstTitles_ = new System.Windows.Forms.ListView();
            this.columnHeader = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            //
            // cboLanguages_
            //
            this.cboLanguages_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboLanguages_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLanguages_.FormattingEnabled = true;
            this.cboLanguages_.Location = new System.Drawing.Point(3, 3);
            this.cboLanguages_.MaxDropDownItems = 10;
            this.cboLanguages_.Name = "cboLanguages_";
            this.cboLanguages_.Size = new System.Drawing.Size(326, 21);
            this.cboLanguages_.TabIndex = 1;
            this.cboLanguages_.SelectedIndexChanged += new System.EventHandler(this.cboLanguages__SelectedIndexChanged);
            //
            // txtTitle_
            //
            this.txtTitle_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitle_.Location = new System.Drawing.Point(3, 30);
            this.txtTitle_.Name = "txtTitle_";
            this.txtTitle_.Size = new System.Drawing.Size(326, 20);
            this.txtTitle_.TabIndex = 2;
            this.txtTitle_.TextChanged += new System.EventHandler(this.txtTitle__TextChanged);
            //
            // lstTitles_
            //
            this.lstTitles_.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstTitles_.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader});
            this.lstTitles_.FullRowSelect = true;
            this.lstTitles_.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstTitles_.HideSelection = false;
            this.lstTitles_.Location = new System.Drawing.Point(3, 56);
            this.lstTitles_.MultiSelect = false;
            this.lstTitles_.Name = "lstTitles_";
            this.lstTitles_.ShowItemToolTips = true;
            this.lstTitles_.Size = new System.Drawing.Size(326, 339);
            this.lstTitles_.TabIndex = 3;
            this.lstTitles_.UseCompatibleStateImageBehavior = false;
            this.lstTitles_.View = System.Windows.Forms.View.Details;
            this.lstTitles_.VirtualMode = true;
            this.lstTitles_.Resize += new System.EventHandler(this.lstTitles__Resize);
            this.lstTitles_.SearchForVirtualItem += new System.Windows.Forms.SearchForVirtualItemEventHandler(this.lstTitles__SearchForVirtualItem);
            this.lstTitles_.DoubleClick += new System.EventHandler(this.lstTitles__DoubleClick);
            this.lstTitles_.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.lstTitles__RetrieveVirtualItem);
            //
            // columnHeader
            //
            this.columnHeader.Width = 200;
            //
            // IndexControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 397);
            this.Controls.Add(this.lstTitles_);
            this.Controls.Add(this.txtTitle_);
            this.Controls.Add(this.cboLanguages_);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "IndexControl";
            this.Text = "Index";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboLanguages_;
        private System.Windows.Forms.TextBox txtTitle_;
        private System.Windows.Forms.ListView lstTitles_;
        private System.Windows.Forms.ColumnHeader columnHeader;
    }
}
