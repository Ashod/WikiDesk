namespace WikiDesk
{
    using WeifenLuo.WinFormsUI.Docking;

    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin4 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
            WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin4 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient10 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient22 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin4 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient4 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient23 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient11 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient24 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient4 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient25 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient26 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient12 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient27 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient28 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.status_ = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexMenuItem_ = new System.Windows.Forms.ToolStripMenuItem();
            this.searchMenuItem_ = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontMenuItem_ = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.donateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stripNavigation = new System.Windows.Forms.ToolStrip();
            this.btnBack = new System.Windows.Forms.ToolStripButton();
            this.btnForward = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnStop = new System.Windows.Forms.ToolStripButton();
            this.lblNavigate = new System.Windows.Forms.ToolStripLabel();
            this.cboNavigate = new System.Windows.Forms.ToolStripComboBox();
            this.btnGo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cboLanguage = new System.Windows.Forms.ToolStripComboBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.dockPanel_ = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.dockContent_ = new WeifenLuo.WinFormsUI.Docking.DockContent();
            this.wikiDomainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.stripNavigation.SuspendLayout();
            this.SuspendLayout();
            //
            // statusStrip1
            //
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.status_});
            this.statusStrip1.Location = new System.Drawing.Point(0, 495);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(792, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            //
            // lblStatus
            //
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            //
            // status_
            //
            this.status_.Name = "status_";
            this.status_.Size = new System.Drawing.Size(0, 17);
            //
            // menuStrip
            //
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(792, 24);
            this.menuStrip.TabIndex = 3;
            this.menuStrip.Text = "menuStrip1";
            //
            // fileToolStripMenuItem
            //
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDatabaseToolStripMenuItem,
            this.importToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.fileToolStripMenuItem.Text = "&File";
            //
            // openDatabaseToolStripMenuItem
            //
            this.openDatabaseToolStripMenuItem.Name = "openDatabaseToolStripMenuItem";
            this.openDatabaseToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.openDatabaseToolStripMenuItem.Text = "&Open Database...";
            this.openDatabaseToolStripMenuItem.Click += new System.EventHandler(this.OpenClick);
            //
            // importToolStripMenuItem
            //
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.importToolStripMenuItem.Text = "&Import...";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            //
            // toolStripMenuItem1
            //
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(172, 6);
            //
            // exitToolStripMenuItem
            //
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitClick);
            //
            // viewToolStripMenuItem
            //
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.indexMenuItem_,
            this.searchMenuItem_});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.viewToolStripMenuItem.Text = "&View";
            this.viewToolStripMenuItem.DropDownOpening += new System.EventHandler(this.viewMenuItem_DropDownOpening);
            //
            // indexMenuItem_
            //
            this.indexMenuItem_.CheckOnClick = true;
            this.indexMenuItem_.Name = "indexMenuItem_";
            this.indexMenuItem_.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.indexMenuItem_.Size = new System.Drawing.Size(147, 22);
            this.indexMenuItem_.Text = "&Index";
            this.indexMenuItem_.Click += new System.EventHandler(this.indexMenuItem__Click);
            //
            // searchMenuItem_
            //
            this.searchMenuItem_.Name = "searchMenuItem_";
            this.searchMenuItem_.Size = new System.Drawing.Size(147, 22);
            this.searchMenuItem_.Text = "&Search";
            this.searchMenuItem_.Click += new System.EventHandler(this.searchToolStripMenuItem_Click);
            //
            // optionsToolStripMenuItem
            //
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fontMenuItem_,
            this.settingsToolStripMenuItem,
            this.wikiDomainToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            //
            // fontMenuItem_
            //
            this.fontMenuItem_.Name = "fontMenuItem_";
            this.fontMenuItem_.Size = new System.Drawing.Size(165, 22);
            this.fontMenuItem_.Text = "&Font...";
            this.fontMenuItem_.Click += new System.EventHandler(this.fontMenuItem__Click);
            //
            // settingsToolStripMenuItem
            //
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.settingsToolStripMenuItem.Text = "&Settings...";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            //
            // helpToolStripMenuItem
            //
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.donateToolStripMenuItem,
            this.toolStripMenuItem2,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            //
            // donateToolStripMenuItem
            //
            this.donateToolStripMenuItem.Name = "donateToolStripMenuItem";
            this.donateToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.donateToolStripMenuItem.Text = "&Donate";
            //
            // toolStripMenuItem2
            //
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(113, 6);
            //
            // aboutToolStripMenuItem
            //
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            //
            // stripNavigation
            //
            this.stripNavigation.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.stripNavigation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnBack,
            this.btnForward,
            this.toolStripSeparator1,
            this.btnStop,
            this.lblNavigate,
            this.cboNavigate,
            this.btnGo,
            this.toolStripSeparator2,
            this.toolStripLabel1,
            this.cboLanguage});
            this.stripNavigation.Location = new System.Drawing.Point(0, 24);
            this.stripNavigation.Name = "stripNavigation";
            this.stripNavigation.Size = new System.Drawing.Size(792, 25);
            this.stripNavigation.TabIndex = 4;
            this.stripNavigation.Resize += new System.EventHandler(this.stripNavigation_Resize);
            //
            // btnBack
            //
            this.btnBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnBack.Image = ((System.Drawing.Image)(resources.GetObject("btnBack.Image")));
            this.btnBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(23, 22);
            this.btnBack.Text = "toolStripButton1";
            this.btnBack.ToolTipText = "Back";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            //
            // btnForward
            //
            this.btnForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnForward.Image = ((System.Drawing.Image)(resources.GetObject("btnForward.Image")));
            this.btnForward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(23, 22);
            this.btnForward.Text = "toolStripButton1";
            this.btnForward.ToolTipText = "Forward";
            this.btnForward.Click += new System.EventHandler(this.btnForward_Click);
            //
            // toolStripSeparator1
            //
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            //
            // btnStop
            //
            this.btnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(23, 22);
            this.btnStop.Text = "toolStripButton1";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            //
            // lblNavigate
            //
            this.lblNavigate.Name = "lblNavigate";
            this.lblNavigate.Size = new System.Drawing.Size(66, 22);
            this.lblNavigate.Text = "Navigate: ";
            //
            // cboNavigate
            //
            this.cboNavigate.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboNavigate.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cboNavigate.AutoToolTip = true;
            this.cboNavigate.DropDownHeight = 200;
            this.cboNavigate.DropDownWidth = 400;
            this.cboNavigate.IntegralHeight = false;
            this.cboNavigate.MaxDropDownItems = 16;
            this.cboNavigate.Name = "cboNavigate";
            this.cboNavigate.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.cboNavigate.Size = new System.Drawing.Size(300, 25);
            this.cboNavigate.Sorted = true;
            this.cboNavigate.DropDownClosed += new System.EventHandler(this.Titles_SelectedIndexChanged);
            this.cboNavigate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Navigation_KeyDown);
            //
            // btnGo
            //
            this.btnGo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGo.Image = ((System.Drawing.Image)(resources.GetObject("btnGo.Image")));
            this.btnGo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(23, 22);
            this.btnGo.Text = "Go";
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            //
            // toolStripSeparator2
            //
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            //
            // toolStripLabel1
            //
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(68, 22);
            this.toolStripLabel1.Text = "Language:";
            //
            // cboLanguage
            //
            this.cboLanguage.DropDownHeight = 200;
            this.cboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLanguage.IntegralHeight = false;
            this.cboLanguage.MaxDropDownItems = 16;
            this.cboLanguage.Name = "cboLanguage";
            this.cboLanguage.Size = new System.Drawing.Size(200, 25);
            this.cboLanguage.SelectedIndexChanged += new System.EventHandler(this.cboLanguage_SelectedIndexChanged);
            //
            // dockPanel_
            //
            this.dockPanel_.ActiveAutoHideContent = null;
            this.dockPanel_.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel_.DockBackColor = System.Drawing.SystemColors.Control;
            this.dockPanel_.DockBottomPortion = 150D;
            this.dockPanel_.DockLeftPortion = 200D;
            this.dockPanel_.DockRightPortion = 200D;
            this.dockPanel_.DockTopPortion = 150D;
            this.dockPanel_.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, ((byte)(0)));
            this.dockPanel_.Location = new System.Drawing.Point(0, 49);
            this.dockPanel_.Name = "dockPanel_";
            this.dockPanel_.RightToLeftLayout = true;
            this.dockPanel_.Size = new System.Drawing.Size(792, 446);
            dockPanelGradient10.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient10.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin4.DockStripGradient = dockPanelGradient10;
            tabGradient22.EndColor = System.Drawing.SystemColors.Control;
            tabGradient22.StartColor = System.Drawing.SystemColors.Control;
            tabGradient22.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin4.TabGradient = tabGradient22;
            autoHideStripSkin4.TextFont = new System.Drawing.Font("Tahoma", 9.75F);
            dockPanelSkin4.AutoHideStripSkin = autoHideStripSkin4;
            tabGradient23.EndColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient23.StartColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient23.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient4.ActiveTabGradient = tabGradient23;
            dockPanelGradient11.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient11.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient4.DockStripGradient = dockPanelGradient11;
            tabGradient24.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient24.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient24.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient4.InactiveTabGradient = tabGradient24;
            dockPaneStripSkin4.DocumentGradient = dockPaneStripGradient4;
            dockPaneStripSkin4.TextFont = new System.Drawing.Font("Tahoma", 9.75F);
            tabGradient25.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient25.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient25.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient25.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient4.ActiveCaptionGradient = tabGradient25;
            tabGradient26.EndColor = System.Drawing.SystemColors.Control;
            tabGradient26.StartColor = System.Drawing.SystemColors.Control;
            tabGradient26.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient4.ActiveTabGradient = tabGradient26;
            dockPanelGradient12.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient12.StartColor = System.Drawing.SystemColors.ControlLight;
            dockPaneStripToolWindowGradient4.DockStripGradient = dockPanelGradient12;
            tabGradient27.EndColor = System.Drawing.SystemColors.InactiveCaption;
            tabGradient27.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient27.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient27.TextColor = System.Drawing.SystemColors.InactiveCaptionText;
            dockPaneStripToolWindowGradient4.InactiveCaptionGradient = tabGradient27;
            tabGradient28.EndColor = System.Drawing.Color.Transparent;
            tabGradient28.StartColor = System.Drawing.Color.Transparent;
            tabGradient28.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient4.InactiveTabGradient = tabGradient28;
            dockPaneStripSkin4.ToolWindowGradient = dockPaneStripToolWindowGradient4;
            dockPanelSkin4.DockPaneStripSkin = dockPaneStripSkin4;
            this.dockPanel_.Skin = dockPanelSkin4;
            this.dockPanel_.TabIndex = 0;
            //
            // dockContent_
            //
            this.dockContent_.ClientSize = new System.Drawing.Size(0, 0);
            this.dockContent_.DockPanel = null;
            this.dockContent_.DockState = WeifenLuo.WinFormsUI.Docking.DockState.Unknown;
            this.dockContent_.FloatPane = null;
            this.dockContent_.IsFloat = false;
            this.dockContent_.IsHidden = true;
            this.dockContent_.Location = new System.Drawing.Point(-32000, -32000);
            this.dockContent_.Name = "dockContent_";
            this.dockContent_.Pane = null;
            this.dockContent_.PanelPane = null;
            this.dockContent_.Visible = false;
            this.dockContent_.VisibleState = WeifenLuo.WinFormsUI.Docking.DockState.Unknown;
            this.dockContent_.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            //
            // wikiDomainToolStripMenuItem
            //
            this.wikiDomainToolStripMenuItem.Name = "wikiDomainToolStripMenuItem";
            this.wikiDomainToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.wikiDomainToolStripMenuItem.Text = "Wiki &Domains...";
            this.wikiDomainToolStripMenuItem.Click += new System.EventHandler(this.wikiDomainToolStripMenuItem_Click);
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 517);
            this.Controls.Add(this.dockPanel_);
            this.Controls.Add(this.stripNavigation);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.stripNavigation.ResumeLayout(false);
            this.stripNavigation.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStrip stripNavigation;
        private System.Windows.Forms.ToolStripButton btnStop;
        private System.Windows.Forms.ToolStripLabel lblNavigate;
        private System.Windows.Forms.ToolStripComboBox cboNavigate;
        private System.Windows.Forms.ToolStripButton btnGo;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripButton btnBack;
        private System.Windows.Forms.ToolStripButton btnForward;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cboLanguage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripStatusLabel status_;
        private DockPanel dockPanel_;
        private DockContent dockContent_;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indexMenuItem_;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem donateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchMenuItem_;
        private System.Windows.Forms.ToolStripMenuItem fontMenuItem_;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wikiDomainToolStripMenuItem;
    }
}

