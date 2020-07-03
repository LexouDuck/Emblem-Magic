namespace EmblemMagic.Editors
{
    partial class HexEditor
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
            this.Tabs_Control = new System.Windows.Forms.TabControl();
            this.MainTabPage = new System.Windows.Forms.TabPage();
            this.MainHexBox = new EmblemMagic.Components.HexBox();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.Status_Position = new System.Windows.Forms.ToolStripStatusLabel();
            this.Status_FileSize = new System.Windows.Forms.ToolStripStatusLabel();
            this.Status_BitViewer = new System.Windows.Forms.ToolStripStatusLabel();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_File_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_File_RecentFiles = new EmblemMagic.Components.RecentFileMenu();
            this.Menu_Separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu_File_Apply = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_File_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_File_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu_File_Close = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Edit = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Edit_Cut = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Edit_Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Edit_Paste = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Separator3 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu_Edit_CopyHex = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Edit_PasteHex = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Separator4 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu_Edit_SelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tool = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tool_Find = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tool_FindNext = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tool_GoTo = new System.Windows.Forms.ToolStripMenuItem();
            this.MagicButton = new EmblemMagic.Components.MagicButton();
            this.Tabs_Control.SuspendLayout();
            this.MainTabPage.SuspendLayout();
            this.StatusStrip.SuspendLayout();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tabs_Control
            // 
            this.Tabs_Control.AllowDrop = true;
            this.Tabs_Control.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Tabs_Control.Controls.Add(this.MainTabPage);
            this.Tabs_Control.Location = new System.Drawing.Point(3, 27);
            this.Tabs_Control.Name = "Tabs_Control";
            this.Tabs_Control.SelectedIndex = 0;
            this.Tabs_Control.Size = new System.Drawing.Size(580, 410);
            this.Tabs_Control.TabIndex = 4;
            this.Tabs_Control.DragDrop += new System.Windows.Forms.DragEventHandler(this.Core_DragDrop);
            this.Tabs_Control.DragEnter += new System.Windows.Forms.DragEventHandler(this.Core_DragEnter);
            // 
            // MainTabPage
            // 
            this.MainTabPage.Controls.Add(this.MainHexBox);
            this.MainTabPage.Location = new System.Drawing.Point(4, 22);
            this.MainTabPage.Name = "MainTabPage";
            this.MainTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.MainTabPage.Size = new System.Drawing.Size(572, 384);
            this.MainTabPage.TabIndex = 0;
            this.MainTabPage.Text = "Loaded ROM";
            // 
            // MainHexBox
            // 
            this.MainHexBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainHexBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MainHexBox.ColumnInfoVisible = true;
            this.MainHexBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainHexBox.LineInfoVisible = true;
            this.MainHexBox.Location = new System.Drawing.Point(-2, 0);
            this.MainHexBox.Name = "MainHexBox";
            this.MainHexBox.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.MainHexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(135)))), ((int)(((byte)(206)))), ((int)(((byte)(235)))));
            this.MainHexBox.Size = new System.Drawing.Size(574, 385);
            this.MainHexBox.StringViewVisible = true;
            this.MainHexBox.TabIndex = 0;
            this.MainHexBox.UseFixedBytesPerLine = true;
            this.MainHexBox.VScrollBarVisible = true;
            this.MainHexBox.SelectionStartChanged += new System.EventHandler(this.HexBox_SelectionStartChanged);
            this.MainHexBox.SelectionLengthChanged += new System.EventHandler(this.HexBox_SelectionLengthChanged);
            this.MainHexBox.CurrentLineChanged += new System.EventHandler(this.HexBox_Position_Changed);
            this.MainHexBox.CurrentPositionInLineChanged += new System.EventHandler(this.HexBox_Position_Changed);
            this.MainHexBox.Copied += new System.EventHandler(this.HexBox_Copy);
            this.MainHexBox.CopiedHex += new System.EventHandler(this.HexBox_CopyHex);
            // 
            // StatusStrip
            // 
            this.StatusStrip.BackColor = System.Drawing.SystemColors.Control;
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Status_Position,
            this.Status_FileSize,
            this.Status_BitViewer});
            this.StatusStrip.Location = new System.Drawing.Point(0, 440);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.StatusStrip.Size = new System.Drawing.Size(584, 22);
            this.StatusStrip.SizingGrip = false;
            this.StatusStrip.TabIndex = 1;
            // 
            // Status_Position
            // 
            this.Status_Position.Margin = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.Status_Position.Name = "Status_Position";
            this.Status_Position.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.Status_Position.Size = new System.Drawing.Size(7, 17);
            // 
            // Status_FileSize
            // 
            this.Status_FileSize.Name = "Status_FileSize";
            this.Status_FileSize.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Status_FileSize.Size = new System.Drawing.Size(8, 17);
            // 
            // Status_BitViewer
            // 
            this.Status_BitViewer.Name = "Status_BitViewer";
            this.Status_BitViewer.Size = new System.Drawing.Size(0, 17);
            // 
            // MenuStrip
            // 
            this.MenuStrip.BackColor = System.Drawing.SystemColors.Control;
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File,
            this.Menu_Edit,
            this.Menu_Tool});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(584, 24);
            this.MenuStrip.TabIndex = 3;
            // 
            // Menu_File
            // 
            this.Menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File_Open,
            this.Menu_File_RecentFiles,
            this.Menu_Separator1,
            this.Menu_File_Apply,
            this.Menu_File_Save,
            this.Menu_File_SaveAs,
            this.Menu_Separator2,
            this.Menu_File_Close});
            this.Menu_File.Name = "Menu_File";
            this.Menu_File.Size = new System.Drawing.Size(37, 20);
            this.Menu_File.Text = "File";
            // 
            // Menu_File_Open
            // 
            this.Menu_File_Open.Name = "Menu_File_Open";
            this.Menu_File_Open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.Menu_File_Open.Size = new System.Drawing.Size(225, 22);
            this.Menu_File_Open.Text = "Open File...";
            this.Menu_File_Open.Click += new System.EventHandler(this.File_Open_Click);
            // 
            // Menu_File_RecentFiles
            // 
            this.Menu_File_RecentFiles.Enabled = false;
            this.Menu_File_RecentFiles.Name = "Menu_File_RecentFiles";
            this.Menu_File_RecentFiles.Size = new System.Drawing.Size(225, 22);
            this.Menu_File_RecentFiles.Text = "Recent Files";
            this.Menu_File_RecentFiles.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.File_Recent_Click);
            // 
            // Menu_Separator1
            // 
            this.Menu_Separator1.Name = "Menu_Separator1";
            this.Menu_Separator1.Size = new System.Drawing.Size(222, 6);
            // 
            // Menu_File_Apply
            // 
            this.Menu_File_Apply.Name = "Menu_File_Apply";
            this.Menu_File_Apply.Size = new System.Drawing.Size(225, 22);
            this.Menu_File_Apply.Text = "Apply ROM changes";
            this.Menu_File_Apply.Click += new System.EventHandler(this.File_Apply_Click);
            // 
            // Menu_File_Save
            // 
            this.Menu_File_Save.Name = "Menu_File_Save";
            this.Menu_File_Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.Menu_File_Save.Size = new System.Drawing.Size(225, 22);
            this.Menu_File_Save.Text = "Save ROM...";
            this.Menu_File_Save.Click += new System.EventHandler(this.File_Save_Click);
            // 
            // Menu_File_SaveAs
            // 
            this.Menu_File_SaveAs.Name = "Menu_File_SaveAs";
            this.Menu_File_SaveAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.Menu_File_SaveAs.Size = new System.Drawing.Size(225, 22);
            this.Menu_File_SaveAs.Text = "Save ROM As...";
            this.Menu_File_SaveAs.Click += new System.EventHandler(this.File_SaveAs_Click);
            // 
            // Menu_Separator2
            // 
            this.Menu_Separator2.Name = "Menu_Separator2";
            this.Menu_Separator2.Size = new System.Drawing.Size(222, 6);
            // 
            // Menu_File_Close
            // 
            this.Menu_File_Close.Enabled = false;
            this.Menu_File_Close.Name = "Menu_File_Close";
            this.Menu_File_Close.Size = new System.Drawing.Size(225, 22);
            this.Menu_File_Close.Text = "Close File";
            this.Menu_File_Close.Click += new System.EventHandler(this.File_Close_Click);
            // 
            // Menu_Edit
            // 
            this.Menu_Edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Edit_Cut,
            this.Menu_Edit_Copy,
            this.Menu_Edit_Paste,
            this.Menu_Separator3,
            this.Menu_Edit_CopyHex,
            this.Menu_Edit_PasteHex,
            this.Menu_Separator4,
            this.Menu_Edit_SelectAll});
            this.Menu_Edit.Name = "Menu_Edit";
            this.Menu_Edit.Size = new System.Drawing.Size(39, 20);
            this.Menu_Edit.Text = "Edit";
            // 
            // Menu_Edit_Cut
            // 
            this.Menu_Edit_Cut.Name = "Menu_Edit_Cut";
            this.Menu_Edit_Cut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.Menu_Edit_Cut.Size = new System.Drawing.Size(199, 22);
            this.Menu_Edit_Cut.Text = "Cut";
            this.Menu_Edit_Cut.Click += new System.EventHandler(this.Edit_Cut_Click);
            // 
            // Menu_Edit_Copy
            // 
            this.Menu_Edit_Copy.Name = "Menu_Edit_Copy";
            this.Menu_Edit_Copy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.Menu_Edit_Copy.Size = new System.Drawing.Size(199, 22);
            this.Menu_Edit_Copy.Text = "Copy";
            this.Menu_Edit_Copy.Click += new System.EventHandler(this.Edit_Copy_Click);
            // 
            // Menu_Edit_Paste
            // 
            this.Menu_Edit_Paste.Name = "Menu_Edit_Paste";
            this.Menu_Edit_Paste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.Menu_Edit_Paste.Size = new System.Drawing.Size(199, 22);
            this.Menu_Edit_Paste.Text = "Paste";
            this.Menu_Edit_Paste.Click += new System.EventHandler(this.Edit_Paste_Click);
            // 
            // Menu_Separator3
            // 
            this.Menu_Separator3.Name = "Menu_Separator3";
            this.Menu_Separator3.Size = new System.Drawing.Size(196, 6);
            // 
            // Menu_Edit_CopyHex
            // 
            this.Menu_Edit_CopyHex.Name = "Menu_Edit_CopyHex";
            this.Menu_Edit_CopyHex.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.Menu_Edit_CopyHex.Size = new System.Drawing.Size(199, 22);
            this.Menu_Edit_CopyHex.Text = "Copy Hex";
            this.Menu_Edit_CopyHex.Click += new System.EventHandler(this.Edit_CopyHex_Click);
            // 
            // Menu_Edit_PasteHex
            // 
            this.Menu_Edit_PasteHex.Name = "Menu_Edit_PasteHex";
            this.Menu_Edit_PasteHex.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.V)));
            this.Menu_Edit_PasteHex.Size = new System.Drawing.Size(199, 22);
            this.Menu_Edit_PasteHex.Text = "Paste Hex";
            this.Menu_Edit_PasteHex.Click += new System.EventHandler(this.Edit_PasteHex_Click);
            // 
            // Menu_Separator4
            // 
            this.Menu_Separator4.Name = "Menu_Separator4";
            this.Menu_Separator4.Size = new System.Drawing.Size(196, 6);
            // 
            // Menu_Edit_SelectAll
            // 
            this.Menu_Edit_SelectAll.Name = "Menu_Edit_SelectAll";
            this.Menu_Edit_SelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.Menu_Edit_SelectAll.Size = new System.Drawing.Size(199, 22);
            this.Menu_Edit_SelectAll.Text = "Select All";
            this.Menu_Edit_SelectAll.Click += new System.EventHandler(this.Edit_SelectAll_Click);
            // 
            // Menu_Tool
            // 
            this.Menu_Tool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Tool_Find,
            this.Menu_Tool_FindNext,
            this.Menu_Tool_GoTo});
            this.Menu_Tool.Name = "Menu_Tool";
            this.Menu_Tool.Size = new System.Drawing.Size(48, 20);
            this.Menu_Tool.Text = "Tools";
            // 
            // Menu_Tool_Find
            // 
            this.Menu_Tool_Find.Name = "Menu_Tool_Find";
            this.Menu_Tool_Find.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.Menu_Tool_Find.Size = new System.Drawing.Size(196, 22);
            this.Menu_Tool_Find.Text = "Find...";
            this.Menu_Tool_Find.Click += new System.EventHandler(this.Tool_Find_Click);
            // 
            // Menu_Tool_FindNext
            // 
            this.Menu_Tool_FindNext.Name = "Menu_Tool_FindNext";
            this.Menu_Tool_FindNext.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F)));
            this.Menu_Tool_FindNext.Size = new System.Drawing.Size(196, 22);
            this.Menu_Tool_FindNext.Text = "Find Next";
            this.Menu_Tool_FindNext.Click += new System.EventHandler(this.Tool_FindNext_Click);
            // 
            // Menu_Tool_GoTo
            // 
            this.Menu_Tool_GoTo.Name = "Menu_Tool_GoTo";
            this.Menu_Tool_GoTo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.Menu_Tool_GoTo.Size = new System.Drawing.Size(196, 22);
            this.Menu_Tool_GoTo.Text = "Goto...";
            this.Menu_Tool_GoTo.Click += new System.EventHandler(this.Tool_GoTo_Click);
            // 
            // MagicButton
            // 
            this.MagicButton.Location = new System.Drawing.Point(555, 0);
            this.MagicButton.MaximumSize = new System.Drawing.Size(24, 24);
            this.MagicButton.MinimumSize = new System.Drawing.Size(24, 24);
            this.MagicButton.Name = "MagicButton";
            this.MagicButton.Size = new System.Drawing.Size(24, 24);
            this.MagicButton.TabIndex = 1;
            this.Help_ToolTip.SetToolTip(this.MagicButton, "This is a shortcut button to open the Basic ROM Editor at the current address.");
            this.MagicButton.UseVisualStyleBackColor = true;
            this.MagicButton.Click += new System.EventHandler(this.MagicButton_Click);
            // 
            // HexEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(584, 462);
            this.Controls.Add(this.MagicButton);
            this.Controls.Add(this.Tabs_Control);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.MenuStrip);
            this.MainMenuStrip = this.MenuStrip;
            this.MinimumSize = new System.Drawing.Size(600, 250);
            this.Name = "HexEditor";
            this.Text = "Hex Editor";
            this.Tabs_Control.ResumeLayout(false);
            this.MainTabPage.ResumeLayout(false);
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem Menu_File;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_Open;
        private EmblemMagic.Components.RecentFileMenu Menu_File_RecentFiles;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_Save;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_SaveAs;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_Close;
        private System.Windows.Forms.ToolStripMenuItem Menu_Edit;
        private System.Windows.Forms.ToolStripMenuItem Menu_Edit_Cut;
        private System.Windows.Forms.ToolStripMenuItem Menu_Edit_Copy;
        private System.Windows.Forms.ToolStripMenuItem Menu_Edit_Paste;
        private System.Windows.Forms.ToolStripMenuItem Menu_Edit_CopyHex;
        private System.Windows.Forms.ToolStripMenuItem Menu_Edit_PasteHex;
        private System.Windows.Forms.ToolStripMenuItem Menu_Edit_SelectAll;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tool;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tool_Find;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tool_FindNext;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tool_GoTo;

        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel Status_Position;
        private System.Windows.Forms.ToolStripStatusLabel Status_BitViewer;
        private System.Windows.Forms.ToolStripStatusLabel Status_FileSize;

        private System.Windows.Forms.ToolStripSeparator Menu_Separator1;
        private System.Windows.Forms.ToolStripSeparator Menu_Separator2;
        private System.Windows.Forms.ToolStripSeparator Menu_Separator3;
        private System.Windows.Forms.ToolStripSeparator Menu_Separator4;

        private System.Windows.Forms.TabControl Tabs_Control;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_Apply;
        private Components.MagicButton MagicButton;
    }
}