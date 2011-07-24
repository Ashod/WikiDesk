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
            this.columnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cboLanguages_ = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            //
            // cboDomains_
            //
            this.cboDomains_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboDomains_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDomains_.FormattingEnabled = true;
            this.cboDomains_.Location = new System.Drawing.Point(11, 11);
            this.cboDomains_.MaxDropDownItems = 10;
            this.cboDomains_.Name = "cboDomains_";
            this.cboDomains_.Size = new System.Drawing.Size(308, 21);
            this.cboDomains_.TabIndex = 1;
            this.cboDomains_.SelectedIndexChanged += new System.EventHandler(this.cboDomains__SelectedIndexChanged);
            //
            // txtTitle_
            //
            this.txtTitle_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitle_.Location = new System.Drawing.Point(11, 65);
            this.txtTitle_.Name = "txtTitle_";
            this.txtTitle_.Size = new System.Drawing.Size(308, 20);
            this.txtTitle_.TabIndex = 2;
            this.txtTitle_.TextChanged += new System.EventHandler(this.txtTitle__TextChanged);
            this.txtTitle_.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTitle__KeyDown);
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
            this.lstTitles_.Location = new System.Drawing.Point(11, 91);
            this.lstTitles_.MultiSelect = false;
            this.lstTitles_.Name = "lstTitles_";
            this.lstTitles_.ShowItemToolTips = true;
            this.lstTitles_.Size = new System.Drawing.Size(308, 296);
            this.lstTitles_.TabIndex = 3;
            this.lstTitles_.UseCompatibleStateImageBehavior = false;
            this.lstTitles_.View = System.Windows.Forms.View.Details;
            this.lstTitles_.VirtualMode = true;
            this.lstTitles_.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lstTitles__ItemSelectionChanged);
            this.lstTitles_.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.lstTitles__RetrieveVirtualItem);
            this.lstTitles_.SearchForVirtualItem += new System.Windows.Forms.SearchForVirtualItemEventHandler(this.lstTitles__SearchForVirtualItem);
            this.lstTitles_.DoubleClick += new System.EventHandler(this.lstTitles__DoubleClick);
            this.lstTitles_.Resize += new System.EventHandler(this.lstTitles__Resize);
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
            this.cboLanguages_.Location = new System.Drawing.Point(11, 38);
            this.cboLanguages_.Name = "cboLanguages_";
            this.cboLanguages_.Size = new System.Drawing.Size(308, 21);
            this.cboLanguages_.TabIndex = 4;
            this.cboLanguages_.SelectedIndexChanged += new System.EventHandler(this.cboLanguages__SelectedIndexChanged);
            //
            // IndexControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 397);
            this.Controls.Add(this.cboLanguages_);
            this.Controls.Add(this.lstTitles_);
            this.Controls.Add(this.txtTitle_);
            this.Controls.Add(this.cboDomains_);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "IndexControl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "Index";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox cboDomains_;
        private System.Windows.Forms.TextBox txtTitle_;
        private System.Windows.Forms.ListView lstTitles_;
        private System.Windows.Forms.ColumnHeader columnHeader;
        private System.Windows.Forms.ComboBox cboLanguages_;
    }
}
