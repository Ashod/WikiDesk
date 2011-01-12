namespace WikiDesk
{
    partial class SearchControl
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
            this.txtTitle_ = new System.Windows.Forms.TextBox();
            this.lstTitles_ = new System.Windows.Forms.ListView();
            this.columnHeader = new System.Windows.Forms.ColumnHeader();
            this.cboLanguages_ = new System.Windows.Forms.ComboBox();
            this.btnSearch_ = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // cboDomains_
            //
            this.cboDomains_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboDomains_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDomains_.FormattingEnabled = true;
            this.cboDomains_.Location = new System.Drawing.Point(3, 3);
            this.cboDomains_.MaxDropDownItems = 10;
            this.cboDomains_.Name = "cboDomains_";
            this.cboDomains_.Size = new System.Drawing.Size(324, 21);
            this.cboDomains_.TabIndex = 1;
            //
            // txtTitle_
            //
            this.txtTitle_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitle_.Location = new System.Drawing.Point(3, 57);
            this.txtTitle_.Name = "txtTitle_";
            this.txtTitle_.Size = new System.Drawing.Size(285, 20);
            this.txtTitle_.TabIndex = 2;
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
            this.lstTitles_.Location = new System.Drawing.Point(3, 83);
            this.lstTitles_.MultiSelect = false;
            this.lstTitles_.Name = "lstTitles_";
            this.lstTitles_.ShowItemToolTips = true;
            this.lstTitles_.Size = new System.Drawing.Size(324, 312);
            this.lstTitles_.TabIndex = 3;
            this.lstTitles_.UseCompatibleStateImageBehavior = false;
            this.lstTitles_.View = System.Windows.Forms.View.Details;
            //
            // columnHeader
            //
            this.columnHeader.Width = 200;
            //
            // cboLanguages_
            //
            this.cboLanguages_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboLanguages_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLanguages_.FormattingEnabled = true;
            this.cboLanguages_.Location = new System.Drawing.Point(3, 30);
            this.cboLanguages_.Name = "cboLanguages_";
            this.cboLanguages_.Size = new System.Drawing.Size(324, 21);
            this.cboLanguages_.TabIndex = 4;
            //
            // btnSearch_
            //
            this.btnSearch_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch_.Location = new System.Drawing.Point(294, 55);
            this.btnSearch_.Name = "btnSearch_";
            this.btnSearch_.Size = new System.Drawing.Size(33, 23);
            this.btnSearch_.TabIndex = 5;
            this.btnSearch_.Text = "Go";
            this.btnSearch_.UseVisualStyleBackColor = true;
            this.btnSearch_.Click += new System.EventHandler(this.btnSearch__Click);
            //
            // SearchControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 397);
            this.Controls.Add(this.btnSearch_);
            this.Controls.Add(this.cboLanguages_);
            this.Controls.Add(this.lstTitles_);
            this.Controls.Add(this.txtTitle_);
            this.Controls.Add(this.cboDomains_);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SearchControl";
            this.Text = "Search";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboDomains_;
        private System.Windows.Forms.TextBox txtTitle_;
        private System.Windows.Forms.ListView lstTitles_;
        private System.Windows.Forms.ColumnHeader columnHeader;
        private System.Windows.Forms.ComboBox cboLanguages_;
        private System.Windows.Forms.Button btnSearch_;
    }
}