using KirbyMagic.Properties;

namespace KirbyMagic
{
    partial class App
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
            this.File_RecentFiles = new Magic.Components.RecentFileMenu();
            this.Edit_Undo = new System.Windows.Forms.ToolStripMenuItem();
            this.Edit_Redo = new System.Windows.Forms.ToolStripMenuItem();
            KirbyMagic.Properties.Settings settings1 = new KirbyMagic.Properties.Settings();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(App));
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Suite_Menu = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.File_OpenROM = new System.Windows.Forms.ToolStripMenuItem();
            this.File_OpenFEH = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.File_SaveROM = new System.Windows.Forms.ToolStripMenuItem();
            this.File_SaveAsROM = new System.Windows.Forms.ToolStripMenuItem();
            this.File_SaveFEH = new System.Windows.Forms.ToolStripMenuItem();
            this.File_SaveAsFEH = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.File_AutoSaveROM = new System.Windows.Forms.ToolStripMenuItem();
            this.File_AutoSaveFEH = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.File_Export = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.File_CloseROM = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Edit = new System.Windows.Forms.ToolStripMenuItem();
            this.Edit_Separator = new System.Windows.Forms.ToolStripSeparator();
            this.Edit_OpenProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.Edit_Options = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tools = new System.Windows.Forms.ToolStripMenuItem();
            this.Tool_ExpandROM = new System.Windows.Forms.ToolStripMenuItem();
            this.Tool_GetPointer = new System.Windows.Forms.ToolStripMenuItem();
            this.Tool_GetFreeSpace = new System.Windows.Forms.ToolStripMenuItem();
            this.knownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.Tool_GetLastWrite = new System.Windows.Forms.ToolStripMenuItem();
            this.Tool_MarkSpace = new System.Windows.Forms.ToolStripMenuItem();
            this.Tool_Separator = new System.Windows.Forms.ToolStripSeparator();
            this.Tool_OpenWriteEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.Tool_OpenPointEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.Tool_OpenSpaceEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Help = new System.Windows.Forms.ToolStripMenuItem();
            this.Help_Help = new System.Windows.Forms.ToolStripMenuItem();
            this.Help_Separator = new System.Windows.Forms.ToolStripSeparator();
            this.Help_About = new System.Windows.Forms.ToolStripMenuItem();
            this.Suite_Tabs = new System.Windows.Forms.TabControl();
            this.Tabs_Info = new System.Windows.Forms.TabPage();
            this.InfoLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Tabs_Info_ROMPathLabel = new System.Windows.Forms.Label();
            this.Tabs_Info_FEH_FileInfo = new System.Windows.Forms.Label();
            this.Tabs_Info_ROMInfoLabel = new System.Windows.Forms.Label();
            this.Tabs_Info_ROM_FileSize = new System.Windows.Forms.Label();
            this.Tabs_Info_FEH_FilePath = new System.Windows.Forms.Label();
            this.Tabs_Info_ROM_FilePath = new System.Windows.Forms.Label();
            this.Tabs_Info_FEHPathLabel = new System.Windows.Forms.Label();
            this.Tabs_Info_FEHInfoLabel = new System.Windows.Forms.Label();
            this.Tabs_Info_FEH_AuthorLabel = new System.Windows.Forms.Label();
            this.Tabs_Info_FEH_NameLabel = new System.Windows.Forms.Label();
            this.Tabs_Info_FEH_Author = new System.Windows.Forms.Label();
            this.Tabs_Info_FEH_Name = new System.Windows.Forms.Label();
            this.Tabs_Open = new System.Windows.Forms.TabPage();
            this.Open_MenuEditor = new System.Windows.Forms.Button();
            this.Open_ItemEditor = new System.Windows.Forms.Button();
            this.Open_TitleEditor = new System.Windows.Forms.Button();
            this.Open_MapTilesetEditor = new System.Windows.Forms.Button();
            this.Open_GraphicsEditor = new System.Windows.Forms.Button();
            this.Open_MapEditor = new System.Windows.Forms.Button();
            this.Open_MapSpriteEditor = new System.Windows.Forms.Button();
            this.Open_ASMEditor = new System.Windows.Forms.Button();
            this.Open_MusicEditor = new System.Windows.Forms.Button();
            this.Open_BasicEditor = new System.Windows.Forms.Button();
            this.Open_WorldMapEditor = new System.Windows.Forms.Button();
            this.Open_EventEditor = new System.Windows.Forms.Button();
            this.Open_BattleScreenEditor = new System.Windows.Forms.Button();
            this.Open_PatchEditor = new System.Windows.Forms.Button();
            this.Open_BackgroundEditor = new System.Windows.Forms.Button();
            this.Open_SpellAnimEditor = new System.Windows.Forms.Button();
            this.Open_BattleAnimEditor = new System.Windows.Forms.Button();
            this.Open_PortraitEditor = new System.Windows.Forms.Button();
            this.Open_TextEditor = new System.Windows.Forms.Button();
            this.Open_ModuleEditor = new System.Windows.Forms.Button();
            this.Open_HexEditor = new System.Windows.Forms.Button();
            this.StatusStrip.SuspendLayout();
            this.Suite_Menu.SuspendLayout();
            this.Suite_Tabs.SuspendLayout();
            this.Tabs_Info.SuspendLayout();
            this.InfoLayoutPanel.SuspendLayout();
            this.Tabs_Open.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.StatusStrip.Location = new System.Drawing.Point(0, 361);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.StatusStrip.Size = new System.Drawing.Size(476, 22);
            this.StatusStrip.TabIndex = 0;
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // Suite_Menu
            // 
            this.Suite_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File,
            this.Menu_Edit,
            this.Menu_Tools,
            this.Menu_Help});
            this.Suite_Menu.Location = new System.Drawing.Point(0, 0);
            this.Suite_Menu.Name = "Suite_Menu";
            this.Suite_Menu.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.Suite_Menu.Size = new System.Drawing.Size(476, 24);
            this.Suite_Menu.TabIndex = 1;
            this.Suite_Menu.Text = "hubMenuStrip";
            // 
            // Menu_File
            // 
            this.Menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File_OpenROM,
            this.File_OpenFEH,
            this.toolStripSeparator1,
            this.File_SaveROM,
            this.File_SaveAsROM,
            this.File_SaveFEH,
            this.File_SaveAsFEH,
            this.toolStripSeparator2,
            this.File_AutoSaveROM,
            this.File_AutoSaveFEH,
            this.toolStripSeparator3,
            this.File_Export,
            this.toolStripSeparator4,
            this.File_CloseROM,
            this.File_Exit});
            this.Menu_File.Name = "Menu_File";
            this.Menu_File.Size = new System.Drawing.Size(37, 20);
            this.Menu_File.Text = "File";
            // 
            // File_OpenROM
            // 
            this.File_OpenROM.Name = "File_OpenROM";
            this.File_OpenROM.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.File_OpenROM.Size = new System.Drawing.Size(242, 22);
            this.File_OpenROM.Text = "Open ROM...";
            this.File_OpenROM.Click += new System.EventHandler(this.File_OpenROM_Click);
            // 
            // File_OpenFEH
            // 
            this.File_OpenFEH.Name = "File_OpenFEH";
            this.File_OpenFEH.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.O)));
            this.File_OpenFEH.Size = new System.Drawing.Size(242, 22);
            this.File_OpenFEH.Text = "Open FEH...";
            this.File_OpenFEH.Click += new System.EventHandler(this.File_OpenFEH_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(239, 6);
            // 
            // File_SaveROM
            // 
            this.File_SaveROM.Enabled = false;
            this.File_SaveROM.Name = "File_SaveROM";
            this.File_SaveROM.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.File_SaveROM.Size = new System.Drawing.Size(242, 22);
            this.File_SaveROM.Text = "Save ROM";
            this.File_SaveROM.Click += new System.EventHandler(this.File_SaveROM_Click);
            // 
            // File_SaveAsROM
            // 
            this.File_SaveAsROM.Enabled = false;
            this.File_SaveAsROM.Name = "File_SaveAsROM";
            this.File_SaveAsROM.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.File_SaveAsROM.Size = new System.Drawing.Size(242, 22);
            this.File_SaveAsROM.Text = "Save ROM As...";
            this.File_SaveAsROM.Click += new System.EventHandler(this.File_SaveROMas_Click);
            // 
            // File_SaveFEH
            // 
            this.File_SaveFEH.Enabled = false;
            this.File_SaveFEH.Name = "File_SaveFEH";
            this.File_SaveFEH.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.File_SaveFEH.Size = new System.Drawing.Size(242, 22);
            this.File_SaveFEH.Text = "Save FEH";
            this.File_SaveFEH.Click += new System.EventHandler(this.File_SaveFEH_Click);
            // 
            // File_SaveAsFEH
            // 
            this.File_SaveAsFEH.Enabled = false;
            this.File_SaveAsFEH.Name = "File_SaveAsFEH";
            this.File_SaveAsFEH.ShortcutKeys = ((System.Windows.Forms.Keys)((((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.File_SaveAsFEH.Size = new System.Drawing.Size(242, 22);
            this.File_SaveAsFEH.Text = "Save FEH As...";
            this.File_SaveAsFEH.Click += new System.EventHandler(this.File_SaveFEHas_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(239, 6);
            // 
            // File_AutoSaveROM
            // 
            this.File_AutoSaveROM.CheckOnClick = true;
            this.File_AutoSaveROM.Name = "File_AutoSaveROM";
            this.File_AutoSaveROM.Size = new System.Drawing.Size(242, 22);
            this.File_AutoSaveROM.Text = "Auto-Patch to ROM";
            // 
            // File_AutoSaveFEH
            // 
            this.File_AutoSaveFEH.CheckOnClick = true;
            this.File_AutoSaveFEH.Name = "File_AutoSaveFEH";
            this.File_AutoSaveFEH.Size = new System.Drawing.Size(242, 22);
            this.File_AutoSaveFEH.Text = "Auto-Save Hack";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(239, 6);
            // 
            // File_Export
            // 
            this.File_Export.Enabled = false;
            this.File_Export.Name = "File_Export";
            this.File_Export.Size = new System.Drawing.Size(242, 22);
            this.File_Export.Text = "Export to UPS Patch";
            this.File_Export.Click += new System.EventHandler(this.File_ExportUPS_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(239, 6);
            // 
            // File_CloseROM
            // 
            this.File_CloseROM.Enabled = false;
            this.File_CloseROM.Name = "File_CloseROM";
            this.File_CloseROM.Size = new System.Drawing.Size(242, 22);
            this.File_CloseROM.Text = "Close ROM";
            this.File_CloseROM.Click += new System.EventHandler(this.File_CloseROM_Click);
            // 
            // File_Exit
            // 
            this.File_Exit.Name = "File_Exit";
            this.File_Exit.Size = new System.Drawing.Size(242, 22);
            this.File_Exit.Text = "Exit";
            // 
            // Menu_Edit
            // 
            this.Menu_Edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Edit_Separator,
            this.Edit_OpenProperties,
            this.Edit_Options});
            this.Menu_Edit.Name = "Menu_Edit";
            this.Menu_Edit.Size = new System.Drawing.Size(39, 20);
            this.Menu_Edit.Text = "Edit";
            // 
            // Edit_Separator
            // 
            this.Edit_Separator.Name = "Edit_Separator";
            this.Edit_Separator.Size = new System.Drawing.Size(163, 6);
            // 
            // Edit_OpenProperties
            // 
            this.Edit_OpenProperties.Enabled = false;
            this.Edit_OpenProperties.Name = "Edit_OpenProperties";
            this.Edit_OpenProperties.Size = new System.Drawing.Size(166, 22);
            this.Edit_OpenProperties.Text = "Hack Properties...";
            this.Edit_OpenProperties.Click += new System.EventHandler(this.Edit_OpenProperties_Click);
            // 
            // Edit_Options
            // 
            this.Edit_Options.Name = "Edit_Options";
            this.Edit_Options.Size = new System.Drawing.Size(166, 22);
            this.Edit_Options.Text = "Options...";
            this.Edit_Options.Click += new System.EventHandler(this.Edit_Options_Click);
            // 
            // Menu_Tools
            // 
            this.Menu_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Tool_ExpandROM,
            this.Tool_GetPointer,
            this.Tool_MarkSpace,
            this.Tool_Separator,
            this.Tool_OpenWriteEditor,
            this.Tool_OpenPointEditor,
            this.Tool_OpenSpaceEditor});
            this.Menu_Tools.Name = "Menu_Tools";
            this.Menu_Tools.Size = new System.Drawing.Size(48, 20);
            this.Menu_Tools.Text = "Tools";
            // 
            // Tool_ExpandROM
            // 
            this.Tool_ExpandROM.Name = "Tool_ExpandROM";
            this.Tool_ExpandROM.Size = new System.Drawing.Size(225, 22);
            this.Tool_ExpandROM.Text = "Expand ROM...";
            this.Tool_ExpandROM.Click += new System.EventHandler(this.Tool_ExpandROM_Click);
            // 
            // Tool_GetPointer
            // 
            this.Tool_GetPointer.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Tool_GetFreeSpace,
            this.knownToolStripMenuItem,
            this.toolStripSeparator8,
            this.Tool_GetLastWrite});
            this.Tool_GetPointer.Enabled = false;
            this.Tool_GetPointer.Name = "Tool_GetPointer";
            this.Tool_GetPointer.Size = new System.Drawing.Size(225, 22);
            this.Tool_GetPointer.Text = "Get Pointer";
            // 
            // Tool_GetFreeSpace
            // 
            this.Tool_GetFreeSpace.Name = "Tool_GetFreeSpace";
            this.Tool_GetFreeSpace.Size = new System.Drawing.Size(157, 22);
            this.Tool_GetFreeSpace.Text = "Free Space...";
            this.Tool_GetFreeSpace.Click += new System.EventHandler(this.Tool_GetFreeSpace_Click);
            // 
            // knownToolStripMenuItem
            // 
            this.knownToolStripMenuItem.Enabled = false;
            this.knownToolStripMenuItem.Name = "knownToolStripMenuItem";
            this.knownToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.knownToolStripMenuItem.Text = "Marked Space...";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(154, 6);
            // 
            // Tool_GetLastWrite
            // 
            this.Tool_GetLastWrite.Name = "Tool_GetLastWrite";
            this.Tool_GetLastWrite.Size = new System.Drawing.Size(157, 22);
            this.Tool_GetLastWrite.Text = "Last Write";
            this.Tool_GetLastWrite.Click += new System.EventHandler(this.Tool_GetLastWrite_Click);
            // 
            // Tool_MarkSpace
            // 
            this.Tool_MarkSpace.Enabled = false;
            this.Tool_MarkSpace.Name = "Tool_MarkSpace";
            this.Tool_MarkSpace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.Tool_MarkSpace.Size = new System.Drawing.Size(225, 22);
            this.Tool_MarkSpace.Text = "Mark Space...";
            this.Tool_MarkSpace.Click += new System.EventHandler(this.Tool_MarkSpace_Click);
            // 
            // Tool_Separator
            // 
            this.Tool_Separator.Name = "Tool_Separator";
            this.Tool_Separator.Size = new System.Drawing.Size(222, 6);
            // 
            // Tool_OpenWriteEditor
            // 
            this.Tool_OpenWriteEditor.Enabled = false;
            this.Tool_OpenWriteEditor.Name = "Tool_OpenWriteEditor";
            this.Tool_OpenWriteEditor.Size = new System.Drawing.Size(225, 22);
            this.Tool_OpenWriteEditor.Text = "Manage Write history...";
            this.Tool_OpenWriteEditor.Click += new System.EventHandler(this.Tool_OpenWrite_Click);
            // 
            // Tool_OpenPointEditor
            // 
            this.Tool_OpenPointEditor.Enabled = false;
            this.Tool_OpenPointEditor.Name = "Tool_OpenPointEditor";
            this.Tool_OpenPointEditor.Size = new System.Drawing.Size(225, 22);
            this.Tool_OpenPointEditor.Text = "Manage Pointers...";
            this.Tool_OpenPointEditor.Click += new System.EventHandler(this.Tool_OpenPoint_Click);
            // 
            // Tool_OpenSpaceEditor
            // 
            this.Tool_OpenSpaceEditor.Enabled = false;
            this.Tool_OpenSpaceEditor.Name = "Tool_OpenSpaceEditor";
            this.Tool_OpenSpaceEditor.Size = new System.Drawing.Size(225, 22);
            this.Tool_OpenSpaceEditor.Text = "Manage ROM Space listing...";
            this.Tool_OpenSpaceEditor.Click += new System.EventHandler(this.Tool_OpenSpace_Click);
            // 
            // Menu_Help
            // 
            this.Menu_Help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Help_Help,
            this.Help_Separator,
            this.Help_About});
            this.Menu_Help.Name = "Menu_Help";
            this.Menu_Help.Size = new System.Drawing.Size(44, 20);
            this.Menu_Help.Text = "Help";
            // 
            // Help_Help
            // 
            this.Help_Help.Name = "Help_Help";
            this.Help_Help.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.Help_Help.Size = new System.Drawing.Size(127, 22);
            this.Help_Help.Text = "Help...";
            this.Help_Help.Click += new System.EventHandler(this.Help_Help_Click);
            // 
            // Help_Separator
            // 
            this.Help_Separator.Name = "Help_Separator";
            this.Help_Separator.Size = new System.Drawing.Size(124, 6);
            // 
            // Help_About
            // 
            this.Help_About.Name = "Help_About";
            this.Help_About.Size = new System.Drawing.Size(127, 22);
            this.Help_About.Text = "About...";
            this.Help_About.Click += new System.EventHandler(this.Help_About_Click);
            // 
            // Suite_Tabs
            // 
            this.Suite_Tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Suite_Tabs.Controls.Add(this.Tabs_Info);
            this.Suite_Tabs.Controls.Add(this.Tabs_Open);
            this.Suite_Tabs.Location = new System.Drawing.Point(0, 31);
            this.Suite_Tabs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Suite_Tabs.Name = "Suite_Tabs";
            this.Suite_Tabs.SelectedIndex = 0;
            this.Suite_Tabs.Size = new System.Drawing.Size(478, 323);
            this.Suite_Tabs.TabIndex = 2;
            this.Suite_Tabs.Visible = false;
            // 
            // Tabs_Info
            // 
            this.Tabs_Info.Controls.Add(this.InfoLayoutPanel);
            this.Tabs_Info.Location = new System.Drawing.Point(4, 24);
            this.Tabs_Info.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Tabs_Info.Name = "Tabs_Info";
            this.Tabs_Info.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Tabs_Info.Size = new System.Drawing.Size(470, 295);
            this.Tabs_Info.TabIndex = 0;
            this.Tabs_Info.Text = "ROM Info";
            this.Tabs_Info.UseVisualStyleBackColor = true;
            // 
            // InfoLayoutPanel
            // 
            this.InfoLayoutPanel.ColumnCount = 2;
            this.InfoLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.InfoLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.InfoLayoutPanel.Controls.Add(this.Tabs_Info_ROMPathLabel, 0, 1);
            this.InfoLayoutPanel.Controls.Add(this.Tabs_Info_FEH_FileInfo, 1, 4);
            this.InfoLayoutPanel.Controls.Add(this.Tabs_Info_ROMInfoLabel, 0, 0);
            this.InfoLayoutPanel.Controls.Add(this.Tabs_Info_ROM_FileSize, 1, 0);
            this.InfoLayoutPanel.Controls.Add(this.Tabs_Info_FEH_FilePath, 1, 5);
            this.InfoLayoutPanel.Controls.Add(this.Tabs_Info_ROM_FilePath, 1, 1);
            this.InfoLayoutPanel.Controls.Add(this.Tabs_Info_FEHPathLabel, 0, 5);
            this.InfoLayoutPanel.Controls.Add(this.Tabs_Info_FEHInfoLabel, 0, 4);
            this.InfoLayoutPanel.Controls.Add(this.Tabs_Info_FEH_AuthorLabel, 0, 3);
            this.InfoLayoutPanel.Controls.Add(this.Tabs_Info_FEH_NameLabel, 0, 2);
            this.InfoLayoutPanel.Controls.Add(this.Tabs_Info_FEH_Author, 1, 3);
            this.InfoLayoutPanel.Controls.Add(this.Tabs_Info_FEH_Name, 1, 2);
            this.InfoLayoutPanel.Location = new System.Drawing.Point(0, 3);
            this.InfoLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.InfoLayoutPanel.Name = "InfoLayoutPanel";
            this.InfoLayoutPanel.RowCount = 6;
            this.InfoLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.InfoLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.InfoLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.InfoLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.InfoLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.InfoLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.InfoLayoutPanel.Size = new System.Drawing.Size(469, 286);
            this.InfoLayoutPanel.TabIndex = 8;
            // 
            // Tabs_Info_ROMPathLabel
            // 
            this.Tabs_Info_ROMPathLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Tabs_Info_ROMPathLabel.AutoSize = true;
            this.Tabs_Info_ROMPathLabel.Location = new System.Drawing.Point(28, 23);
            this.Tabs_Info_ROMPathLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Tabs_Info_ROMPathLabel.Name = "Tabs_Info_ROMPathLabel";
            this.Tabs_Info_ROMPathLabel.Size = new System.Drawing.Size(85, 15);
            this.Tabs_Info_ROMPathLabel.TabIndex = 4;
            this.Tabs_Info_ROMPathLabel.Text = "ROM File path:";
            this.Tabs_Info_ROMPathLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Tabs_Info_FEH_FileInfo
            // 
            this.Tabs_Info_FEH_FileInfo.AutoSize = true;
            this.Tabs_Info_FEH_FileInfo.Location = new System.Drawing.Point(121, 166);
            this.Tabs_Info_FEH_FileInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Tabs_Info_FEH_FileInfo.Name = "Tabs_Info_FEH_FileInfo";
            this.Tabs_Info_FEH_FileInfo.Size = new System.Drawing.Size(12, 15);
            this.Tabs_Info_FEH_FileInfo.TabIndex = 3;
            this.Tabs_Info_FEH_FileInfo.Text = "-";
            // 
            // Tabs_Info_ROMInfoLabel
            // 
            this.Tabs_Info_ROMInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Tabs_Info_ROMInfoLabel.AutoSize = true;
            this.Tabs_Info_ROMInfoLabel.Location = new System.Drawing.Point(30, 0);
            this.Tabs_Info_ROMInfoLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Tabs_Info_ROMInfoLabel.Name = "Tabs_Info_ROMInfoLabel";
            this.Tabs_Info_ROMInfoLabel.Size = new System.Drawing.Size(83, 15);
            this.Tabs_Info_ROMInfoLabel.TabIndex = 5;
            this.Tabs_Info_ROMInfoLabel.Text = "ROM File size: ";
            this.Tabs_Info_ROMInfoLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Tabs_Info_ROM_FileSize
            // 
            this.Tabs_Info_ROM_FileSize.AutoSize = true;
            this.Tabs_Info_ROM_FileSize.Location = new System.Drawing.Point(121, 0);
            this.Tabs_Info_ROM_FileSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Tabs_Info_ROM_FileSize.Name = "Tabs_Info_ROM_FileSize";
            this.Tabs_Info_ROM_FileSize.Size = new System.Drawing.Size(12, 15);
            this.Tabs_Info_ROM_FileSize.TabIndex = 1;
            this.Tabs_Info_ROM_FileSize.Text = "-";
            // 
            // Tabs_Info_FEH_FilePath
            // 
            this.Tabs_Info_FEH_FilePath.AutoSize = true;
            this.Tabs_Info_FEH_FilePath.Location = new System.Drawing.Point(121, 189);
            this.Tabs_Info_FEH_FilePath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Tabs_Info_FEH_FilePath.Name = "Tabs_Info_FEH_FilePath";
            this.Tabs_Info_FEH_FilePath.Size = new System.Drawing.Size(34, 15);
            this.Tabs_Info_FEH_FilePath.TabIndex = 2;
            this.Tabs_Info_FEH_FilePath.Text = "none";
            // 
            // Tabs_Info_ROM_FilePath
            // 
            this.Tabs_Info_ROM_FilePath.AutoSize = true;
            this.Tabs_Info_ROM_FilePath.Location = new System.Drawing.Point(121, 23);
            this.Tabs_Info_ROM_FilePath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Tabs_Info_ROM_FilePath.Name = "Tabs_Info_ROM_FilePath";
            this.Tabs_Info_ROM_FilePath.Size = new System.Drawing.Size(34, 15);
            this.Tabs_Info_ROM_FilePath.TabIndex = 0;
            this.Tabs_Info_ROM_FilePath.Text = "none";
            // 
            // Tabs_Info_FEHPathLabel
            // 
            this.Tabs_Info_FEHPathLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Tabs_Info_FEHPathLabel.AutoSize = true;
            this.Tabs_Info_FEHPathLabel.Location = new System.Drawing.Point(28, 189);
            this.Tabs_Info_FEHPathLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Tabs_Info_FEHPathLabel.Name = "Tabs_Info_FEHPathLabel";
            this.Tabs_Info_FEHPathLabel.Size = new System.Drawing.Size(85, 15);
            this.Tabs_Info_FEHPathLabel.TabIndex = 6;
            this.Tabs_Info_FEHPathLabel.Text = "Hack File path:";
            this.Tabs_Info_FEHPathLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Tabs_Info_FEHInfoLabel
            // 
            this.Tabs_Info_FEHInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Tabs_Info_FEHInfoLabel.AutoSize = true;
            this.Tabs_Info_FEHInfoLabel.Location = new System.Drawing.Point(31, 166);
            this.Tabs_Info_FEHInfoLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Tabs_Info_FEHInfoLabel.Name = "Tabs_Info_FEHInfoLabel";
            this.Tabs_Info_FEHInfoLabel.Size = new System.Drawing.Size(82, 15);
            this.Tabs_Info_FEHInfoLabel.TabIndex = 7;
            this.Tabs_Info_FEHInfoLabel.Text = "Hack File info:";
            this.Tabs_Info_FEHInfoLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Tabs_Info_FEH_AuthorLabel
            // 
            this.Tabs_Info_FEH_AuthorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Tabs_Info_FEH_AuthorLabel.AutoSize = true;
            this.Tabs_Info_FEH_AuthorLabel.Location = new System.Drawing.Point(20, 143);
            this.Tabs_Info_FEH_AuthorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Tabs_Info_FEH_AuthorLabel.Name = "Tabs_Info_FEH_AuthorLabel";
            this.Tabs_Info_FEH_AuthorLabel.Size = new System.Drawing.Size(93, 15);
            this.Tabs_Info_FEH_AuthorLabel.TabIndex = 8;
            this.Tabs_Info_FEH_AuthorLabel.Text = "Hack Author(s) :";
            // 
            // Tabs_Info_FEH_NameLabel
            // 
            this.Tabs_Info_FEH_NameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Tabs_Info_FEH_NameLabel.AutoSize = true;
            this.Tabs_Info_FEH_NameLabel.Location = new System.Drawing.Point(38, 120);
            this.Tabs_Info_FEH_NameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Tabs_Info_FEH_NameLabel.Name = "Tabs_Info_FEH_NameLabel";
            this.Tabs_Info_FEH_NameLabel.Size = new System.Drawing.Size(75, 15);
            this.Tabs_Info_FEH_NameLabel.TabIndex = 9;
            this.Tabs_Info_FEH_NameLabel.Text = "Hack Name :";
            // 
            // Tabs_Info_FEH_Author
            // 
            this.Tabs_Info_FEH_Author.AutoSize = true;
            this.Tabs_Info_FEH_Author.Location = new System.Drawing.Point(121, 143);
            this.Tabs_Info_FEH_Author.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Tabs_Info_FEH_Author.Name = "Tabs_Info_FEH_Author";
            this.Tabs_Info_FEH_Author.Size = new System.Drawing.Size(12, 15);
            this.Tabs_Info_FEH_Author.TabIndex = 10;
            this.Tabs_Info_FEH_Author.Text = "-";
            // 
            // Tabs_Info_FEH_Name
            // 
            this.Tabs_Info_FEH_Name.AutoSize = true;
            this.Tabs_Info_FEH_Name.Location = new System.Drawing.Point(121, 120);
            this.Tabs_Info_FEH_Name.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Tabs_Info_FEH_Name.Name = "Tabs_Info_FEH_Name";
            this.Tabs_Info_FEH_Name.Size = new System.Drawing.Size(12, 15);
            this.Tabs_Info_FEH_Name.TabIndex = 11;
            this.Tabs_Info_FEH_Name.Text = "-";
            // 
            // Tabs_Open
            // 
            this.Tabs_Open.Controls.Add(this.Open_MenuEditor);
            this.Tabs_Open.Controls.Add(this.Open_ItemEditor);
            this.Tabs_Open.Controls.Add(this.Open_TitleEditor);
            this.Tabs_Open.Controls.Add(this.Open_MapTilesetEditor);
            this.Tabs_Open.Controls.Add(this.Open_GraphicsEditor);
            this.Tabs_Open.Controls.Add(this.Open_MapEditor);
            this.Tabs_Open.Controls.Add(this.Open_MapSpriteEditor);
            this.Tabs_Open.Controls.Add(this.Open_ASMEditor);
            this.Tabs_Open.Controls.Add(this.Open_MusicEditor);
            this.Tabs_Open.Controls.Add(this.Open_BasicEditor);
            this.Tabs_Open.Controls.Add(this.Open_WorldMapEditor);
            this.Tabs_Open.Controls.Add(this.Open_EventEditor);
            this.Tabs_Open.Controls.Add(this.Open_BattleScreenEditor);
            this.Tabs_Open.Controls.Add(this.Open_PatchEditor);
            this.Tabs_Open.Controls.Add(this.Open_BackgroundEditor);
            this.Tabs_Open.Controls.Add(this.Open_SpellAnimEditor);
            this.Tabs_Open.Controls.Add(this.Open_BattleAnimEditor);
            this.Tabs_Open.Controls.Add(this.Open_PortraitEditor);
            this.Tabs_Open.Controls.Add(this.Open_TextEditor);
            this.Tabs_Open.Controls.Add(this.Open_ModuleEditor);
            this.Tabs_Open.Controls.Add(this.Open_HexEditor);
            this.Tabs_Open.Location = new System.Drawing.Point(4, 24);
            this.Tabs_Open.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Tabs_Open.Name = "Tabs_Open";
            this.Tabs_Open.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Tabs_Open.Size = new System.Drawing.Size(470, 295);
            this.Tabs_Open.TabIndex = 1;
            this.Tabs_Open.Text = "Open New Editors";
            this.Tabs_Open.UseVisualStyleBackColor = true;
            // 
            // Open_MenuEditor
            // 
            this.Open_MenuEditor.Location = new System.Drawing.Point(317, 256);
            this.Open_MenuEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Open_MenuEditor.Name = "Open_MenuEditor";
            this.Open_MenuEditor.Size = new System.Drawing.Size(152, 35);
            this.Open_MenuEditor.TabIndex = 21;
            this.Open_MenuEditor.Text = "Menu Editor";
            this.Open_MenuEditor.UseVisualStyleBackColor = true;
            this.Open_MenuEditor.Click += new System.EventHandler(this.Open_MenuEditor_Click);
            // 
            // Open_ItemEditor
            // 
            this.Open_ItemEditor.Location = new System.Drawing.Point(317, 132);
            this.Open_ItemEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Open_ItemEditor.Name = "Open_ItemEditor";
            this.Open_ItemEditor.Size = new System.Drawing.Size(152, 35);
            this.Open_ItemEditor.TabIndex = 20;
            this.Open_ItemEditor.Text = "Item Editor";
            this.Open_ItemEditor.UseVisualStyleBackColor = true;
            this.Open_ItemEditor.Click += new System.EventHandler(this.Open_ItemEditor_Click);
            // 
            // Open_TitleEditor
            // 
            this.Open_TitleEditor.Location = new System.Drawing.Point(0, 256);
            this.Open_TitleEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Open_TitleEditor.Name = "Open_TitleEditor";
            this.Open_TitleEditor.Size = new System.Drawing.Size(152, 35);
            this.Open_TitleEditor.TabIndex = 19;
            this.Open_TitleEditor.Text = "Title Screen Editor";
            this.Open_TitleEditor.UseVisualStyleBackColor = true;
            this.Open_TitleEditor.Click += new System.EventHandler(this.Open_TitleScreenEditor_Click);
            // 
            // Open_MapTilesetEditor
            // 
            this.Open_MapTilesetEditor.Location = new System.Drawing.Point(0, 215);
            this.Open_MapTilesetEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Open_MapTilesetEditor.Name = "Open_MapTilesetEditor";
            this.Open_MapTilesetEditor.Size = new System.Drawing.Size(152, 35);
            this.Open_MapTilesetEditor.TabIndex = 18;
            this.Open_MapTilesetEditor.Text = "Map Tileset Editor";
            this.Open_MapTilesetEditor.UseVisualStyleBackColor = true;
            this.Open_MapTilesetEditor.Click += new System.EventHandler(this.Open_MapTilesetEditor_Click);
            // 
            // Open_GraphicsEditor
            // 
            this.Open_GraphicsEditor.Location = new System.Drawing.Point(0, 90);
            this.Open_GraphicsEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Open_GraphicsEditor.Name = "Open_GraphicsEditor";
            this.Open_GraphicsEditor.Size = new System.Drawing.Size(152, 35);
            this.Open_GraphicsEditor.TabIndex = 17;
            this.Open_GraphicsEditor.Text = "Graphics Editor";
            this.Open_GraphicsEditor.UseVisualStyleBackColor = true;
            this.Open_GraphicsEditor.Click += new System.EventHandler(this.Open_GraphicsEditor_Click);
            // 
            // Open_MapEditor
            // 
            this.Open_MapEditor.Location = new System.Drawing.Point(159, 215);
            this.Open_MapEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Open_MapEditor.Name = "Open_MapEditor";
            this.Open_MapEditor.Size = new System.Drawing.Size(152, 35);
            this.Open_MapEditor.TabIndex = 14;
            this.Open_MapEditor.Text = "Map Editor";
            this.Open_MapEditor.UseVisualStyleBackColor = true;
            this.Open_MapEditor.Click += new System.EventHandler(this.Open_MapEditor_Click);
            // 
            // Open_MapSpriteEditor
            // 
            this.Open_MapSpriteEditor.Location = new System.Drawing.Point(317, 215);
            this.Open_MapSpriteEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Open_MapSpriteEditor.Name = "Open_MapSpriteEditor";
            this.Open_MapSpriteEditor.Size = new System.Drawing.Size(152, 35);
            this.Open_MapSpriteEditor.TabIndex = 10;
            this.Open_MapSpriteEditor.Text = "Map Sprite Editor";
            this.Open_MapSpriteEditor.UseVisualStyleBackColor = true;
            this.Open_MapSpriteEditor.Click += new System.EventHandler(this.Open_MapSpriteEditor_Click);
            // 
            // Open_ASMEditor
            // 
            this.Open_ASMEditor.Location = new System.Drawing.Point(0, 48);
            this.Open_ASMEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Open_ASMEditor.Name = "Open_ASMEditor";
            this.Open_ASMEditor.Size = new System.Drawing.Size(152, 35);
            this.Open_ASMEditor.TabIndex = 16;
            this.Open_ASMEditor.Text = "ASM Editor";
            this.Open_ASMEditor.UseVisualStyleBackColor = true;
            this.Open_ASMEditor.Click += new System.EventHandler(this.Open_ASMEditor_Click);
            // 
            // Open_MusicEditor
            // 
            this.Open_MusicEditor.Location = new System.Drawing.Point(317, 90);
            this.Open_MusicEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Open_MusicEditor.Name = "Open_MusicEditor";
            this.Open_MusicEditor.Size = new System.Drawing.Size(152, 35);
            this.Open_MusicEditor.TabIndex = 15;
            this.Open_MusicEditor.Text = "Music Editor";
            this.Open_MusicEditor.UseVisualStyleBackColor = true;
            this.Open_MusicEditor.Click += new System.EventHandler(this.Open_MusicEditor_Click);
            // 
            // Open_BasicEditor
            // 
            this.Open_BasicEditor.Location = new System.Drawing.Point(0, 7);
            this.Open_BasicEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Open_BasicEditor.Name = "Open_BasicEditor";
            this.Open_BasicEditor.Size = new System.Drawing.Size(152, 35);
            this.Open_BasicEditor.TabIndex = 13;
            this.Open_BasicEditor.Text = "Basic ROM Editor";
            this.Open_BasicEditor.UseVisualStyleBackColor = true;
            this.Open_BasicEditor.Click += new System.EventHandler(this.Open_BasicEditor_Click);
            // 
            // Open_WorldMapEditor
            // 
            this.Open_WorldMapEditor.Location = new System.Drawing.Point(159, 256);
            this.Open_WorldMapEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Open_WorldMapEditor.Name = "Open_WorldMapEditor";
            this.Open_WorldMapEditor.Size = new System.Drawing.Size(152, 35);
            this.Open_WorldMapEditor.TabIndex = 5;
            this.Open_WorldMapEditor.Text = "World Map Editor";
            this.Open_WorldMapEditor.UseVisualStyleBackColor = true;
            this.Open_WorldMapEditor.Click += new System.EventHandler(this.Open_WorldMapEditor_Click);
            // 
            // Open_EventEditor
            // 
            this.Open_EventEditor.Location = new System.Drawing.Point(159, 48);
            this.Open_EventEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Open_EventEditor.Name = "Open_EventEditor";
            this.Open_EventEditor.Size = new System.Drawing.Size(152, 35);
            this.Open_EventEditor.TabIndex = 12;
            this.Open_EventEditor.Text = "Event Editor";
            this.Open_EventEditor.UseVisualStyleBackColor = true;
            this.Open_EventEditor.Click += new System.EventHandler(this.Open_EventEditor_Click);
            // 
            // Open_BattleScreenEditor
            // 
            this.Open_BattleScreenEditor.Location = new System.Drawing.Point(0, 173);
            this.Open_BattleScreenEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Open_BattleScreenEditor.Name = "Open_BattleScreenEditor";
            this.Open_BattleScreenEditor.Size = new System.Drawing.Size(152, 35);
            this.Open_BattleScreenEditor.TabIndex = 10;
            this.Open_BattleScreenEditor.Text = "Battle Screen Editor";
            this.Open_BattleScreenEditor.UseVisualStyleBackColor = true;
            this.Open_BattleScreenEditor.Click += new System.EventHandler(this.Open_BattleScreenEditor_Click);
            // 
            // Open_PatchEditor
            // 
            this.Open_PatchEditor.Location = new System.Drawing.Point(317, 7);
            this.Open_PatchEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Open_PatchEditor.Name = "Open_PatchEditor";
            this.Open_PatchEditor.Size = new System.Drawing.Size(152, 35);
            this.Open_PatchEditor.TabIndex = 11;
            this.Open_PatchEditor.Text = "Patch Editor";
            this.Open_PatchEditor.UseVisualStyleBackColor = true;
            this.Open_PatchEditor.Click += new System.EventHandler(this.Open_PatchEditor_Click);
            // 
            // Open_BackgroundEditor
            // 
            this.Open_BackgroundEditor.Location = new System.Drawing.Point(159, 132);
            this.Open_BackgroundEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Open_BackgroundEditor.Name = "Open_BackgroundEditor";
            this.Open_BackgroundEditor.Size = new System.Drawing.Size(152, 35);
            this.Open_BackgroundEditor.TabIndex = 9;
            this.Open_BackgroundEditor.Text = "Backgrounds Editor";
            this.Open_BackgroundEditor.UseVisualStyleBackColor = true;
            this.Open_BackgroundEditor.Click += new System.EventHandler(this.Open_BackgroundEditor_Click);
            // 
            // Open_SpellAnimEditor
            // 
            this.Open_SpellAnimEditor.Location = new System.Drawing.Point(317, 173);
            this.Open_SpellAnimEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Open_SpellAnimEditor.Name = "Open_SpellAnimEditor";
            this.Open_SpellAnimEditor.Size = new System.Drawing.Size(152, 35);
            this.Open_SpellAnimEditor.TabIndex = 8;
            this.Open_SpellAnimEditor.Text = "Spell Animation Editor";
            this.Open_SpellAnimEditor.UseVisualStyleBackColor = true;
            this.Open_SpellAnimEditor.Click += new System.EventHandler(this.Open_SpellAnimEditor_Click);
            // 
            // Open_BattleAnimEditor
            // 
            this.Open_BattleAnimEditor.Location = new System.Drawing.Point(159, 173);
            this.Open_BattleAnimEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Open_BattleAnimEditor.Name = "Open_BattleAnimEditor";
            this.Open_BattleAnimEditor.Size = new System.Drawing.Size(152, 35);
            this.Open_BattleAnimEditor.TabIndex = 7;
            this.Open_BattleAnimEditor.Text = "Battle Animation Editor";
            this.Open_BattleAnimEditor.UseVisualStyleBackColor = true;
            this.Open_BattleAnimEditor.Click += new System.EventHandler(this.Open_BattleAnimEditor_Click);
            // 
            // Open_PortraitEditor
            // 
            this.Open_PortraitEditor.Location = new System.Drawing.Point(0, 132);
            this.Open_PortraitEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Open_PortraitEditor.Name = "Open_PortraitEditor";
            this.Open_PortraitEditor.Size = new System.Drawing.Size(152, 35);
            this.Open_PortraitEditor.TabIndex = 6;
            this.Open_PortraitEditor.Text = "Portrait Editor";
            this.Open_PortraitEditor.UseVisualStyleBackColor = true;
            this.Open_PortraitEditor.Click += new System.EventHandler(this.Open_PortraitEditor_Click);
            // 
            // Open_TextEditor
            // 
            this.Open_TextEditor.Location = new System.Drawing.Point(159, 90);
            this.Open_TextEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Open_TextEditor.Name = "Open_TextEditor";
            this.Open_TextEditor.Size = new System.Drawing.Size(152, 35);
            this.Open_TextEditor.TabIndex = 4;
            this.Open_TextEditor.Text = "Text Editor";
            this.Open_TextEditor.UseVisualStyleBackColor = true;
            this.Open_TextEditor.Click += new System.EventHandler(this.Open_TextEditor_Click);
            // 
            // Open_ModuleEditor
            // 
            this.Open_ModuleEditor.Location = new System.Drawing.Point(317, 48);
            this.Open_ModuleEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Open_ModuleEditor.Name = "Open_ModuleEditor";
            this.Open_ModuleEditor.Size = new System.Drawing.Size(152, 35);
            this.Open_ModuleEditor.TabIndex = 3;
            this.Open_ModuleEditor.Text = "Module Editor";
            this.Open_ModuleEditor.UseVisualStyleBackColor = true;
            this.Open_ModuleEditor.Click += new System.EventHandler(this.Open_ModuleEditor_Click);
            // 
            // Open_HexEditor
            // 
            this.Open_HexEditor.Location = new System.Drawing.Point(159, 7);
            this.Open_HexEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Open_HexEditor.Name = "Open_HexEditor";
            this.Open_HexEditor.Size = new System.Drawing.Size(152, 35);
            this.Open_HexEditor.TabIndex = 2;
            this.Open_HexEditor.Text = "Hex Editor";
            this.Open_HexEditor.UseVisualStyleBackColor = true;
            this.Open_HexEditor.Click += new System.EventHandler(this.Open_HexEditor_Click);
            // 
            // App
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 383);
            this.Controls.Add(this.Suite_Tabs);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.Suite_Menu);
            settings1.SettingsKey = "";
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", settings1, "WindowLocation", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(492, 421);
            this.MinimumSize = new System.Drawing.Size(492, 421);
            this.Name = "App";
            this.Text = "Kirby Magic";
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.Suite_Menu.ResumeLayout(false);
            this.Suite_Menu.PerformLayout();
            this.Suite_Tabs.ResumeLayout(false);
            this.Tabs_Info.ResumeLayout(false);
            this.InfoLayoutPanel.ResumeLayout(false);
            this.InfoLayoutPanel.PerformLayout();
            this.Tabs_Open.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.MenuStrip Suite_Menu;
        private System.Windows.Forms.ToolStripMenuItem Menu_File;
        private System.Windows.Forms.ToolStripMenuItem File_OpenROM;
        private System.Windows.Forms.ToolStripMenuItem File_OpenFEH;
        private System.Windows.Forms.ToolStripMenuItem File_SaveROM;
        private System.Windows.Forms.ToolStripMenuItem File_SaveAsROM;
        private System.Windows.Forms.ToolStripMenuItem File_SaveFEH;
        private System.Windows.Forms.ToolStripMenuItem File_SaveAsFEH;
        private System.Windows.Forms.ToolStripMenuItem File_Export;
        private System.Windows.Forms.ToolStripMenuItem File_AutoSaveROM;
        private System.Windows.Forms.ToolStripMenuItem File_AutoSaveFEH;
        private System.Windows.Forms.ToolStripMenuItem File_CloseROM;
        private System.Windows.Forms.ToolStripMenuItem File_Exit;

        private System.Windows.Forms.ToolStripMenuItem Menu_Edit;
        private System.Windows.Forms.ToolStripMenuItem Edit_Options;

        private System.Windows.Forms.ToolStripMenuItem Menu_Tools;
        private System.Windows.Forms.ToolStripMenuItem Tool_GetPointer;
        private System.Windows.Forms.ToolStripMenuItem Tool_GetFreeSpace;
        private System.Windows.Forms.ToolStripMenuItem Tool_GetLastWrite;
        private System.Windows.Forms.ToolStripMenuItem Tool_OpenWriteEditor;
        private System.Windows.Forms.ToolStripMenuItem Tool_OpenPointEditor;
        private System.Windows.Forms.ToolStripMenuItem Tool_OpenSpaceEditor;



        private System.Windows.Forms.TabControl Suite_Tabs;

        private System.Windows.Forms.TabPage Tabs_Info;
        private System.Windows.Forms.Label Tabs_Info_ROMPathLabel;
        private System.Windows.Forms.Label Tabs_Info_ROMInfoLabel;
        private System.Windows.Forms.Label Tabs_Info_FEHPathLabel;
        private System.Windows.Forms.Label Tabs_Info_FEHInfoLabel;
        private System.Windows.Forms.Label Tabs_Info_ROM_FilePath;
        private System.Windows.Forms.Label Tabs_Info_ROM_FileSize;
        private System.Windows.Forms.Label Tabs_Info_FEH_FilePath;
        private System.Windows.Forms.Label Tabs_Info_FEH_FileInfo;

        private System.Windows.Forms.TabPage Tabs_Open;
        private System.Windows.Forms.Button Open_BattleScreenEditor;
        private System.Windows.Forms.Button Open_MapSpriteEditor;
        private System.Windows.Forms.Button Open_BackgroundEditor;
        private System.Windows.Forms.Button Open_SpellAnimEditor;
        private System.Windows.Forms.Button Open_BattleAnimEditor;
        private System.Windows.Forms.Button Open_PortraitEditor;
        private System.Windows.Forms.Button Open_WorldMapEditor;
        private System.Windows.Forms.Button Open_TextEditor;
        private System.Windows.Forms.Button Open_ModuleEditor;
        private System.Windows.Forms.Button Open_HexEditor;

        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator Edit_Separator;
        private System.Windows.Forms.ToolStripSeparator Tool_Separator;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.ToolStripMenuItem knownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Tool_MarkSpace;
        private System.Windows.Forms.Button Open_MusicEditor;
        private System.Windows.Forms.Button Open_MapEditor;
        private System.Windows.Forms.Button Open_BasicEditor;
        private System.Windows.Forms.Button Open_EventEditor;
        private System.Windows.Forms.Button Open_PatchEditor;
        private System.Windows.Forms.Button Open_ASMEditor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem Menu_Help;
        private System.Windows.Forms.ToolStripMenuItem Help_Help;
        private System.Windows.Forms.ToolStripSeparator Help_Separator;
        private System.Windows.Forms.ToolStripMenuItem Help_About;
        private System.Windows.Forms.TableLayoutPanel InfoLayoutPanel;
        private System.Windows.Forms.Label Tabs_Info_FEH_AuthorLabel;
        private System.Windows.Forms.Label Tabs_Info_FEH_NameLabel;
        private System.Windows.Forms.Label Tabs_Info_FEH_Author;
        private System.Windows.Forms.Label Tabs_Info_FEH_Name;
        private System.Windows.Forms.Button Open_GraphicsEditor;
        private System.Windows.Forms.Button Open_MapTilesetEditor;
        private System.Windows.Forms.ToolStripMenuItem Edit_OpenProperties;
        private System.Windows.Forms.ToolStripMenuItem Tool_ExpandROM;
        private System.Windows.Forms.Button Open_MenuEditor;
        private System.Windows.Forms.Button Open_ItemEditor;
        private System.Windows.Forms.Button Open_TitleEditor;
    }
}