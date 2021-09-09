namespace EmblemMagic.Editors
{
    partial class EventEditor
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventEditor));
            this.Map_Panel = new System.Windows.Forms.Panel();
            this.MapViewBox = new Magic.Components.GridBox();
            this.Entry_Label = new System.Windows.Forms.Label();
            this.Entry_ArrayBox = new Magic.Components.ByteArrayBox();
            this.Event_CodeBox = new Magic.Components.CodeBox();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Assemble = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Assemble_CurrentText = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.File_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_View = new System.Windows.Forms.ToolStripMenuItem();
            this.View_Grid = new System.Windows.Forms.ToolStripMenuItem();
            this.View_Units = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.View_ArrayDefinitions = new System.Windows.Forms.ToolStripMenuItem();
            this.View_HelperMacros = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tools = new System.Windows.Forms.ToolStripMenuItem();
            this.Tools_MakeEAtxt = new System.Windows.Forms.ToolStripMenuItem();
            this.Tools_MakeEMtxt = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.Tools_ManageSpace = new System.Windows.Forms.ToolStripMenuItem();
            this.MapSelection_Label = new System.Windows.Forms.ToolStripLabel();
            this.Event_PointerBox = new Magic.Components.PointerBox();
            this.Event_ArrayBox = new Magic.Components.ByteArrayBox();
            this.Event_Label = new System.Windows.Forms.Label();
            this.UnitEvents_ListBox = new System.Windows.Forms.CheckedListBox();
            this.UnitEvents_Label = new System.Windows.Forms.Label();
            this.MapChanges_Label = new System.Windows.Forms.Label();
            this.MapChanges_ListBox = new System.Windows.Forms.CheckedListBox();
            this.Chapter_MagicButton = new Magic.Components.MagicButton(App);
            this.Map_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Event_CodeBox)).BeginInit();
            this.MenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Event_PointerBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Map_Panel
            // 
            this.Map_Panel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Map_Panel.AutoScroll = true;
            this.Map_Panel.Controls.Add(this.MapViewBox);
            this.Map_Panel.Location = new System.Drawing.Point(420, 27);
            this.Map_Panel.MaximumSize = new System.Drawing.Size(600, 600);
            this.Map_Panel.MinimumSize = new System.Drawing.Size(300, 170);
            this.Map_Panel.Name = "Map_Panel";
            this.Map_Panel.Size = new System.Drawing.Size(300, 170);
            this.Map_Panel.TabIndex = 1;
            // 
            // MapViewBox
            // 
            this.MapViewBox.Location = new System.Drawing.Point(4, 3);
            this.MapViewBox.Name = "MapViewBox";
            this.MapViewBox.Selection = null;
            this.MapViewBox.ShowGrid = false;
            this.MapViewBox.Size = new System.Drawing.Size(240, 160);
            this.MapViewBox.TabIndex = 0;
            this.MapViewBox.TabStop = false;
            this.MapViewBox.TileSize = 16;
            this.Help_ToolTip.SetToolTip(this.MapViewBox, resources.GetString("MapViewBox.ToolTip"));
            this.MapViewBox.SelectionChanged += new System.EventHandler(this.MapViewBox_SelectionChanged);
            // 
            // Entry_Label
            // 
            this.Entry_Label.AutoSize = true;
            this.Entry_Label.Location = new System.Drawing.Point(13, 33);
            this.Entry_Label.Name = "Entry_Label";
            this.Entry_Label.Size = new System.Drawing.Size(50, 13);
            this.Entry_Label.TabIndex = 19;
            this.Entry_Label.Text = "Chapter :";
            // 
            // Entry_ArrayBox
            // 
            this.Entry_ArrayBox.Location = new System.Drawing.Point(69, 27);
            this.Entry_ArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.Entry_ArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.Entry_ArrayBox.Size = new System.Drawing.Size(313, 26);
            this.Entry_ArrayBox.TabIndex = 18;
            this.Help_ToolTip.SetToolTip(this.Entry_ArrayBox, "Select the chapter to view/edit.");
            this.Entry_ArrayBox.ValueChanged += new System.EventHandler(this.EntryArrayBox_ValueChanged);
            // 
            // Event_CodeBox
            // 
            this.Event_CodeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Event_CodeBox.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.Event_CodeBox.AutoScrollMinSize = new System.Drawing.Size(23, 12);
            this.Event_CodeBox.BackBrush = null;
            this.Event_CodeBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Event_CodeBox.CharHeight = 12;
            this.Event_CodeBox.CharWidth = 6;
            this.Event_CodeBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Event_CodeBox.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Event_CodeBox.Font = new System.Drawing.Font("Consolas", 8F);
            this.Event_CodeBox.IsReplaceMode = false;
            this.Event_CodeBox.Location = new System.Drawing.Point(12, 85);
            this.Event_CodeBox.MinimumSize = new System.Drawing.Size(400, 365);
            this.Event_CodeBox.Name = "Event_CodeBox";
            this.Event_CodeBox.Paddings = new System.Windows.Forms.Padding(0);
            this.Event_CodeBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.Event_CodeBox.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("Event_CodeBox.ServiceColors")));
            this.Event_CodeBox.Size = new System.Drawing.Size(400, 365);
            this.Event_CodeBox.TabIndex = 20;
            this.Event_CodeBox.Zoom = 100;
            this.Event_CodeBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Event_CodeBox_MouseMove);
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File,
            this.Menu_View,
            this.Menu_Tools,
            this.MapSelection_Label});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(731, 24);
            this.MenuStrip.TabIndex = 21;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // Menu_File
            // 
            this.Menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File_Assemble,
            this.File_Assemble_CurrentText,
            this.toolStripSeparator2,
            this.File_Save});
            this.Menu_File.Name = "Menu_File";
            this.Menu_File.Size = new System.Drawing.Size(37, 20);
            this.Menu_File.Text = "File";
            // 
            // File_Assemble
            // 
            this.File_Assemble.Name = "File_Assemble";
            this.File_Assemble.Size = new System.Drawing.Size(232, 22);
            this.File_Assemble.Text = "Assemble event code...";
            this.File_Assemble.Click += new System.EventHandler(this.File_Assemble_Click);
            // 
            // File_Assemble_CurrentText
            // 
            this.File_Assemble_CurrentText.Name = "File_Assemble_CurrentText";
            this.File_Assemble_CurrentText.Size = new System.Drawing.Size(232, 22);
            this.File_Assemble_CurrentText.Text = "Assemble current code";
            this.File_Assemble_CurrentText.Click += new System.EventHandler(this.File_Assemble_CurrentText_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(229, 6);
            // 
            // File_Save
            // 
            this.File_Save.Name = "File_Save";
            this.File_Save.Size = new System.Drawing.Size(232, 22);
            this.File_Save.Text = "Save current code as text file...";
            this.File_Save.Click += new System.EventHandler(this.File_Save_Click);
            // 
            // Menu_View
            // 
            this.Menu_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.View_Grid,
            this.View_Units,
            this.toolStripSeparator1,
            this.View_ArrayDefinitions,
            this.View_HelperMacros});
            this.Menu_View.Name = "Menu_View";
            this.Menu_View.Size = new System.Drawing.Size(44, 20);
            this.Menu_View.Text = "View";
            // 
            // View_Grid
            // 
            this.View_Grid.CheckOnClick = true;
            this.View_Grid.Name = "View_Grid";
            this.View_Grid.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.View_Grid.Size = new System.Drawing.Size(242, 22);
            this.View_Grid.Text = "Show Grid";
            this.View_Grid.Click += new System.EventHandler(this.View_Grid_Click);
            // 
            // View_Units
            // 
            this.View_Units.Checked = true;
            this.View_Units.CheckOnClick = true;
            this.View_Units.CheckState = System.Windows.Forms.CheckState.Checked;
            this.View_Units.Name = "View_Units";
            this.View_Units.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.View_Units.Size = new System.Drawing.Size(242, 22);
            this.View_Units.Text = "Show Units";
            this.View_Units.Click += new System.EventHandler(this.View_Units_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(239, 6);
            // 
            // View_ArrayDefinitions
            // 
            this.View_ArrayDefinitions.Checked = true;
            this.View_ArrayDefinitions.CheckOnClick = true;
            this.View_ArrayDefinitions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.View_ArrayDefinitions.Name = "View_ArrayDefinitions";
            this.View_ArrayDefinitions.Size = new System.Drawing.Size(242, 22);
            this.View_ArrayDefinitions.Text = "View code with array definitions";
            this.View_ArrayDefinitions.CheckedChanged += new System.EventHandler(this.View_ArrayDefinitions_CheckedChanged);
            // 
            // View_HelperMacros
            // 
            this.View_HelperMacros.Checked = true;
            this.View_HelperMacros.CheckState = System.Windows.Forms.CheckState.Checked;
            this.View_HelperMacros.Name = "View_HelperMacros";
            this.View_HelperMacros.Size = new System.Drawing.Size(242, 22);
            this.View_HelperMacros.Text = "View code with helper macros";
            // 
            // Menu_Tools
            // 
            this.Menu_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Tools_MakeEAtxt,
            this.Tools_MakeEMtxt,
            this.toolStripSeparator3,
            this.Tools_ManageSpace});
            this.Menu_Tools.Name = "Menu_Tools";
            this.Menu_Tools.Size = new System.Drawing.Size(48, 20);
            this.Menu_Tools.Text = "Tools";
            // 
            // Tools_MakeEAtxt
            // 
            this.Tools_MakeEAtxt.Name = "Tools_MakeEAtxt";
            this.Tools_MakeEAtxt.Size = new System.Drawing.Size(277, 22);
            this.Tools_MakeEAtxt.Text = "Make EventAssembler definitions file...";
            this.Tools_MakeEAtxt.Click += new System.EventHandler(this.Tools_MakeEAtxt_Click);
            // 
            // Tools_MakeEMtxt
            // 
            this.Tools_MakeEMtxt.Name = "Tools_MakeEMtxt";
            this.Tools_MakeEMtxt.Size = new System.Drawing.Size(277, 22);
            this.Tools_MakeEMtxt.Text = "Make EmblemMagic array txt file...";
            this.Tools_MakeEMtxt.Click += new System.EventHandler(this.Tools_MakeEMtxt_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(274, 6);
            // 
            // Tools_ManageSpace
            // 
            this.Tools_ManageSpace.Checked = true;
            this.Tools_ManageSpace.CheckOnClick = true;
            this.Tools_ManageSpace.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Tools_ManageSpace.Name = "Tools_ManageSpace";
            this.Tools_ManageSpace.Size = new System.Drawing.Size(277, 22);
            this.Tools_ManageSpace.Text = "Manage ORG/ASSERTs automatically";
            this.Tools_ManageSpace.CheckedChanged += new System.EventHandler(this.Tools_ManageSpace_CheckedChanged);
            // 
            // MapSelection_Label
            // 
            this.MapSelection_Label.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.MapSelection_Label.Name = "MapSelection_Label";
            this.MapSelection_Label.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.MapSelection_Label.Size = new System.Drawing.Size(79, 17);
            this.MapSelection_Label.Text = "X: __, Y: __";
            // 
            // Event_PointerBox
            // 
            this.Event_PointerBox.Hexadecimal = true;
            this.Event_PointerBox.Location = new System.Drawing.Point(342, 58);
            this.Event_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Event_PointerBox.Name = "Event_PointerBox";
            this.Event_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Event_PointerBox.TabIndex = 22;
            this.Help_ToolTip.SetToolTip(this.Event_PointerBox, "Pointer to the event data within the chapter/map pointer list.\r\nWill write to ROM" +
        " if changed. Is repointed when inserting event code with \"File -> Assemble...\".");
            this.Event_PointerBox.ValueChanged += new System.EventHandler(this.Event_PointerBox_ValueChanged);
            // 
            // Event_ArrayBox
            // 
            this.Event_ArrayBox.Location = new System.Drawing.Point(69, 55);
            this.Event_ArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.Event_ArrayBox.MinimumSize = new System.Drawing.Size(128, 26);
            this.Event_ArrayBox.Size = new System.Drawing.Size(267, 26);
            this.Event_ArrayBox.TabIndex = 23;
            this.Help_ToolTip.SetToolTip(this.Event_ArrayBox, "The entry in the chapter/map list at which to store the events ([EVT]) for this c" +
        "hapter.\r\nWill write to ROM if changed.");
            this.Event_ArrayBox.ValueChanged += new System.EventHandler(this.Event_ArrayBox_ValueChanged);
            // 
            // Event_Label
            // 
            this.Event_Label.AutoSize = true;
            this.Event_Label.Location = new System.Drawing.Point(17, 60);
            this.Event_Label.Name = "Event_Label";
            this.Event_Label.Size = new System.Drawing.Size(46, 13);
            this.Event_Label.TabIndex = 24;
            this.Event_Label.Text = "Events :";
            // 
            // UnitEvents_ListBox
            // 
            this.UnitEvents_ListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UnitEvents_ListBox.CheckOnClick = true;
            this.UnitEvents_ListBox.FormattingEnabled = true;
            this.UnitEvents_ListBox.Location = new System.Drawing.Point(420, 221);
            this.UnitEvents_ListBox.Name = "UnitEvents_ListBox";
            this.UnitEvents_ListBox.ScrollAlwaysVisible = true;
            this.UnitEvents_ListBox.Size = new System.Drawing.Size(212, 229);
            this.UnitEvents_ListBox.Sorted = true;
            this.UnitEvents_ListBox.TabIndex = 25;
            this.Help_ToolTip.SetToolTip(this.UnitEvents_ListBox, "The list of unit groups for this chapter map. Uncheck a unit group to hide them f" +
        "rom the map display.");
            this.UnitEvents_ListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.UnitEvents_ListBox_ItemCheck);
            // 
            // UnitEvents_Label
            // 
            this.UnitEvents_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UnitEvents_Label.AutoSize = true;
            this.UnitEvents_Label.Location = new System.Drawing.Point(420, 205);
            this.UnitEvents_Label.Name = "UnitEvents_Label";
            this.UnitEvents_Label.Size = new System.Drawing.Size(116, 13);
            this.UnitEvents_Label.TabIndex = 26;
            this.UnitEvents_Label.Text = "Unit Groups to display :";
            // 
            // MapChanges_Label
            // 
            this.MapChanges_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.MapChanges_Label.AutoSize = true;
            this.MapChanges_Label.Location = new System.Drawing.Point(638, 205);
            this.MapChanges_Label.Name = "MapChanges_Label";
            this.MapChanges_Label.Size = new System.Drawing.Size(78, 13);
            this.MapChanges_Label.TabIndex = 28;
            this.MapChanges_Label.Text = "Map changes :";
            // 
            // MapChanges_ListBox
            // 
            this.MapChanges_ListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.MapChanges_ListBox.CheckOnClick = true;
            this.MapChanges_ListBox.FormattingEnabled = true;
            this.MapChanges_ListBox.Location = new System.Drawing.Point(638, 221);
            this.MapChanges_ListBox.Name = "MapChanges_ListBox";
            this.MapChanges_ListBox.ScrollAlwaysVisible = true;
            this.MapChanges_ListBox.Size = new System.Drawing.Size(82, 229);
            this.MapChanges_ListBox.Sorted = true;
            this.MapChanges_ListBox.TabIndex = 27;
            this.Help_ToolTip.SetToolTip(this.MapChanges_ListBox, "The list of triggerable map tile changes for this map. Check them to apply them t" +
        "o the map display.");
            this.MapChanges_ListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.MapChanges_ListBox_ItemCheck);
            // 
            // Chapter_MagicButton
            // 
            this.Chapter_MagicButton.Location = new System.Drawing.Point(388, 27);
            this.Chapter_MagicButton.MaximumSize = new System.Drawing.Size(24, 24);
            this.Chapter_MagicButton.MinimumSize = new System.Drawing.Size(24, 24);
            this.Chapter_MagicButton.Name = "Chapter_MagicButton";
            this.Chapter_MagicButton.Size = new System.Drawing.Size(24, 24);
            this.Chapter_MagicButton.TabIndex = 29;
            this.Help_ToolTip.SetToolTip(this.Chapter_MagicButton, "Click on this to open the Chapter Editor module, to modify chapter data (like mus" +
        "ic, etc)");
            this.Chapter_MagicButton.UseVisualStyleBackColor = true;
            // 
            // EventEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 462);
            this.Controls.Add(this.Chapter_MagicButton);
            this.Controls.Add(this.MapChanges_Label);
            this.Controls.Add(this.MapChanges_ListBox);
            this.Controls.Add(this.UnitEvents_Label);
            this.Controls.Add(this.UnitEvents_ListBox);
            this.Controls.Add(this.Event_Label);
            this.Controls.Add(this.Event_ArrayBox);
            this.Controls.Add(this.Event_PointerBox);
            this.Controls.Add(this.Event_CodeBox);
            this.Controls.Add(this.Entry_Label);
            this.Controls.Add(this.Map_Panel);
            this.Controls.Add(this.Entry_ArrayBox);
            this.Controls.Add(this.MenuStrip);
            this.MainMenuStrip = this.MenuStrip;
            this.MinimumSize = new System.Drawing.Size(747, 500);
            this.Name = "EventEditor";
            this.Text = "Event Editor";
            this.SizeChanged += new System.EventHandler(this.EventEditor_SizeChanged);
            this.Map_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Event_CodeBox)).EndInit();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Event_PointerBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel Map_Panel;
        private System.Windows.Forms.Label Entry_Label;
        private Magic.Components.ByteArrayBox Entry_ArrayBox;
        private Magic.Components.CodeBox Event_CodeBox;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem Menu_File;
        private System.Windows.Forms.ToolStripMenuItem File_Assemble;
        private System.Windows.Forms.ToolStripMenuItem Menu_View;
        private System.Windows.Forms.ToolStripMenuItem View_Grid;
        private System.Windows.Forms.ToolStripMenuItem View_Units;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private Magic.Components.PointerBox Event_PointerBox;
        private Magic.Components.ByteArrayBox Event_ArrayBox;
        private System.Windows.Forms.Label Event_Label;
        private System.Windows.Forms.ToolStripMenuItem View_ArrayDefinitions;
        private Magic.Components.GridBox MapViewBox;
        private System.Windows.Forms.CheckedListBox UnitEvents_ListBox;
        private System.Windows.Forms.Label UnitEvents_Label;
        private System.Windows.Forms.Label MapChanges_Label;
        private System.Windows.Forms.CheckedListBox MapChanges_ListBox;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tools;
        private System.Windows.Forms.ToolStripMenuItem Tools_MakeEAtxt;
        private System.Windows.Forms.ToolStripMenuItem Tools_MakeEMtxt;
        private System.Windows.Forms.ToolStripMenuItem File_Save;
        private System.Windows.Forms.ToolStripLabel MapSelection_Label;
        private System.Windows.Forms.ToolStripMenuItem File_Assemble_CurrentText;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem Tools_ManageSpace;
        private Magic.Components.MagicButton Chapter_MagicButton;
        private System.Windows.Forms.ToolStripMenuItem View_HelperMacros;
    }
}