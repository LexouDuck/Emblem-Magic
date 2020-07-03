namespace EmblemMagic.Editors
{
    partial class TextEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextEditor));
            this.EntryNumBox = new EmblemMagic.Components.ShortBox();
            this.Text_PointerBox = new EmblemMagic.Components.PointerBox();
            this.EntryLabel = new System.Windows.Forms.Label();
            this.Text_Pointer_Label = new System.Windows.Forms.Label();
            this.Text_CodeBox = new EmblemMagic.Components.CodeBox();
            this.Font_GridBox = new EmblemMagic.Components.GridBox();
            this.Font_GroupBox = new System.Windows.Forms.GroupBox();
            this.Glyph_GroupBox = new System.Windows.Forms.GroupBox();
            this.Glyph_Pointer_PointerBox = new EmblemMagic.Components.PointerBox();
            this.Glyph_Pointer_Label = new System.Windows.Forms.Label();
            this.Glyph_Shift_ByteBox = new EmblemMagic.Components.ByteBox();
            this.Glyph_Unknown_Label = new System.Windows.Forms.Label();
            this.Glyph_Address_PointerBox = new EmblemMagic.Components.PointerBox();
            this.Glyph_Address_Label = new System.Windows.Forms.Label();
            this.Glyph_Width_ByteBox = new EmblemMagic.Components.ByteBox();
            this.Glyph_Width_Label = new System.Windows.Forms.Label();
            this.Font_ComboBox = new System.Windows.Forms.ComboBox();
            this.Font_InsertButton = new System.Windows.Forms.Button();
            this.Text_Apply_Button = new System.Windows.Forms.Button();
            this.Text_Cancel_Button = new System.Windows.Forms.Button();
            this.Editor_Menu = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.File_InsertEntry = new System.Windows.Forms.ToolStripMenuItem();
            this.File_InsertScript = new System.Windows.Forms.ToolStripMenuItem();
            this.File_InsertFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Separator = new System.Windows.Forms.ToolStripSeparator();
            this.File_SaveEntry = new System.Windows.Forms.ToolStripMenuItem();
            this.File_SaveScript = new System.Windows.Forms.ToolStripMenuItem();
            this.File_SaveFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tools = new System.Windows.Forms.ToolStripMenuItem();
            this.Tool_Find = new System.Windows.Forms.ToolStripMenuItem();
            this.Tool_Replace = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_View = new System.Windows.Forms.ToolStripMenuItem();
            this.View_Bytecodes = new System.Windows.Forms.ToolStripMenuItem();
            this.Text_Preview_ImageBox = new EmblemMagic.Components.ImageBox();
            this.Text_Line_NumBox = new EmblemMagic.Components.ByteBox();
            this.Text_Line_Label = new System.Windows.Forms.Label();
            this.Text_ASCII_CheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.EntryNumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Text_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Text_CodeBox)).BeginInit();
            this.Font_GroupBox.SuspendLayout();
            this.Glyph_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Glyph_Pointer_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Glyph_Shift_ByteBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Glyph_Address_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Glyph_Width_ByteBox)).BeginInit();
            this.Editor_Menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Text_Line_NumBox)).BeginInit();
            this.SuspendLayout();
            // 
            // EntryNumBox
            // 
            this.EntryNumBox.Hexadecimal = true;
            this.EntryNumBox.Location = new System.Drawing.Point(55, 27);
            this.EntryNumBox.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.EntryNumBox.Name = "EntryNumBox";
            this.EntryNumBox.Size = new System.Drawing.Size(56, 20);
            this.EntryNumBox.TabIndex = 0;
            this.Help_ToolTip.SetToolTip(this.EntryNumBox, "Select a text entry to view/edit. This entry index is an unsigned 16-bit number.");
            this.EntryNumBox.Value = ((ushort)(0));
            this.EntryNumBox.ValueChanged += new System.EventHandler(this.EntryNumBox_ValueChanged);
            // 
            // Text_PointerBox
            // 
            this.Text_PointerBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Text_PointerBox.Hexadecimal = true;
            this.Text_PointerBox.Location = new System.Drawing.Point(235, 27);
            this.Text_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Text_PointerBox.Name = "Text_PointerBox";
            this.Text_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Text_PointerBox.TabIndex = 1;
            this.Help_ToolTip.SetToolTip(this.Text_PointerBox, "Pointer to the text data for the current text entry.\r\nWill write to ROM if change" +
        "d.");
            // 
            // EntryLabel
            // 
            this.EntryLabel.AutoSize = true;
            this.EntryLabel.Location = new System.Drawing.Point(12, 29);
            this.EntryLabel.Name = "EntryLabel";
            this.EntryLabel.Size = new System.Drawing.Size(37, 13);
            this.EntryLabel.TabIndex = 2;
            this.EntryLabel.Text = "Entry :";
            // 
            // Text_Pointer_Label
            // 
            this.Text_Pointer_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Text_Pointer_Label.AutoSize = true;
            this.Text_Pointer_Label.Location = new System.Drawing.Point(183, 29);
            this.Text_Pointer_Label.Name = "Text_Pointer_Label";
            this.Text_Pointer_Label.Size = new System.Drawing.Size(46, 13);
            this.Text_Pointer_Label.TabIndex = 3;
            this.Text_Pointer_Label.Text = "Pointer :";
            // 
            // Text_CodeBox
            // 
            this.Text_CodeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Text_CodeBox.AutoCompleteBracketsList = new char[] {
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
            this.Text_CodeBox.AutoScrollMinSize = new System.Drawing.Size(23, 12);
            this.Text_CodeBox.BackBrush = null;
            this.Text_CodeBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Text_CodeBox.CharHeight = 12;
            this.Text_CodeBox.CharWidth = 6;
            this.Text_CodeBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Text_CodeBox.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Text_CodeBox.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.Text_CodeBox.IsReplaceMode = false;
            this.Text_CodeBox.Location = new System.Drawing.Point(12, 53);
            this.Text_CodeBox.Name = "Text_CodeBox";
            this.Text_CodeBox.Paddings = new System.Windows.Forms.Padding(0);
            this.Text_CodeBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.Text_CodeBox.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("Text_CodeBox.ServiceColors")));
            this.Text_CodeBox.Size = new System.Drawing.Size(293, 400);
            this.Text_CodeBox.TabIndex = 4;
            this.Text_CodeBox.Zoom = 100;
            // 
            // Font_GridBox
            // 
            this.Font_GridBox.Location = new System.Drawing.Point(12, 53);
            this.Font_GridBox.Name = "Font_GridBox";
            this.Font_GridBox.Selection = null;
            this.Font_GridBox.ShowGrid = false;
            this.Font_GridBox.Size = new System.Drawing.Size(256, 256);
            this.Font_GridBox.TabIndex = 5;
            this.Font_GridBox.TabStop = false;
            this.Font_GridBox.Text = "gridBox1";
            this.Font_GridBox.TileSize = 16;
            this.Help_ToolTip.SetToolTip(this.Font_GridBox, "This is the font display image. Click on any character in this image to select it" +
        ".\r\nYou can then modify the font properties for the selected character/glyph with" +
        " the controls below.");
            this.Font_GridBox.SelectionChanged += new System.EventHandler(this.Font_GridBox_SelectionChanged);
            // 
            // Font_GroupBox
            // 
            this.Font_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Font_GroupBox.Controls.Add(this.Glyph_GroupBox);
            this.Font_GroupBox.Controls.Add(this.Font_ComboBox);
            this.Font_GroupBox.Controls.Add(this.Font_InsertButton);
            this.Font_GroupBox.Controls.Add(this.Font_GridBox);
            this.Font_GroupBox.Location = new System.Drawing.Point(320, 106);
            this.Font_GroupBox.Name = "Font_GroupBox";
            this.Font_GroupBox.Size = new System.Drawing.Size(280, 390);
            this.Font_GroupBox.TabIndex = 6;
            this.Font_GroupBox.TabStop = false;
            this.Font_GroupBox.Text = "Font Editing";
            // 
            // Glyph_GroupBox
            // 
            this.Glyph_GroupBox.Controls.Add(this.Glyph_Pointer_PointerBox);
            this.Glyph_GroupBox.Controls.Add(this.Glyph_Pointer_Label);
            this.Glyph_GroupBox.Controls.Add(this.Glyph_Shift_ByteBox);
            this.Glyph_GroupBox.Controls.Add(this.Glyph_Unknown_Label);
            this.Glyph_GroupBox.Controls.Add(this.Glyph_Address_PointerBox);
            this.Glyph_GroupBox.Controls.Add(this.Glyph_Address_Label);
            this.Glyph_GroupBox.Controls.Add(this.Glyph_Width_ByteBox);
            this.Glyph_GroupBox.Controls.Add(this.Glyph_Width_Label);
            this.Glyph_GroupBox.Location = new System.Drawing.Point(12, 315);
            this.Glyph_GroupBox.Name = "Glyph_GroupBox";
            this.Glyph_GroupBox.Size = new System.Drawing.Size(256, 70);
            this.Glyph_GroupBox.TabIndex = 12;
            this.Glyph_GroupBox.TabStop = false;
            this.Glyph_GroupBox.Text = "Glyph Editing";
            // 
            // Glyph_Pointer_PointerBox
            // 
            this.Glyph_Pointer_PointerBox.Hexadecimal = true;
            this.Glyph_Pointer_PointerBox.Location = new System.Drawing.Point(68, 43);
            this.Glyph_Pointer_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Glyph_Pointer_PointerBox.Name = "Glyph_Pointer_PointerBox";
            this.Glyph_Pointer_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Glyph_Pointer_PointerBox.TabIndex = 13;
            this.Help_ToolTip.SetToolTip(this.Glyph_Pointer_PointerBox, "\r\nWill write to ROM if changed.");
            this.Glyph_Pointer_PointerBox.ValueChanged += new System.EventHandler(this.Glyph_Pointer_PointerBox_ValueChanged);
            // 
            // Glyph_Pointer_Label
            // 
            this.Glyph_Pointer_Label.AutoSize = true;
            this.Glyph_Pointer_Label.Location = new System.Drawing.Point(29, 45);
            this.Glyph_Pointer_Label.Name = "Glyph_Pointer_Label";
            this.Glyph_Pointer_Label.Size = new System.Drawing.Size(33, 13);
            this.Glyph_Pointer_Label.TabIndex = 12;
            this.Glyph_Pointer_Label.Text = "Link :";
            // 
            // Glyph_Shift_ByteBox
            // 
            this.Glyph_Shift_ByteBox.Hexadecimal = true;
            this.Glyph_Shift_ByteBox.Location = new System.Drawing.Point(210, 43);
            this.Glyph_Shift_ByteBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Glyph_Shift_ByteBox.Name = "Glyph_Shift_ByteBox";
            this.Glyph_Shift_ByteBox.Size = new System.Drawing.Size(40, 20);
            this.Glyph_Shift_ByteBox.TabIndex = 10;
            this.Help_ToolTip.SetToolTip(this.Glyph_Shift_ByteBox, "The character code for Shift-JIS encoding (for japanese fonts, which have many mo" +
        "re different glyphs)\r\nWill write to ROM if changed.");
            this.Glyph_Shift_ByteBox.Value = ((byte)(0));
            this.Glyph_Shift_ByteBox.ValueChanged += new System.EventHandler(this.Glyph_Shift_ByteBox_ValueChanged);
            // 
            // Glyph_Unknown_Label
            // 
            this.Glyph_Unknown_Label.AutoSize = true;
            this.Glyph_Unknown_Label.Location = new System.Drawing.Point(152, 45);
            this.Glyph_Unknown_Label.Name = "Glyph_Unknown_Label";
            this.Glyph_Unknown_Label.Size = new System.Drawing.Size(52, 13);
            this.Glyph_Unknown_Label.TabIndex = 11;
            this.Glyph_Unknown_Label.Text = "Shift-JIS :";
            // 
            // Glyph_Address_PointerBox
            // 
            this.Glyph_Address_PointerBox.Hexadecimal = true;
            this.Glyph_Address_PointerBox.Location = new System.Drawing.Point(68, 17);
            this.Glyph_Address_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Glyph_Address_PointerBox.Name = "Glyph_Address_PointerBox";
            this.Glyph_Address_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Glyph_Address_PointerBox.TabIndex = 9;
            this.Help_ToolTip.SetToolTip(this.Glyph_Address_PointerBox, "The address of this glyph.\r\nWill write to ROM if changed.");
            this.Glyph_Address_PointerBox.ValueChanged += new System.EventHandler(this.Glyph_Address_PointerBox_ValueChanged);
            // 
            // Glyph_Address_Label
            // 
            this.Glyph_Address_Label.AutoSize = true;
            this.Glyph_Address_Label.Location = new System.Drawing.Point(11, 19);
            this.Glyph_Address_Label.Name = "Glyph_Address_Label";
            this.Glyph_Address_Label.Size = new System.Drawing.Size(51, 13);
            this.Glyph_Address_Label.TabIndex = 8;
            this.Glyph_Address_Label.Text = "Address :";
            // 
            // Glyph_Width_ByteBox
            // 
            this.Glyph_Width_ByteBox.Hexadecimal = true;
            this.Glyph_Width_ByteBox.Location = new System.Drawing.Point(210, 17);
            this.Glyph_Width_ByteBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Glyph_Width_ByteBox.Name = "Glyph_Width_ByteBox";
            this.Glyph_Width_ByteBox.Size = new System.Drawing.Size(40, 20);
            this.Glyph_Width_ByteBox.TabIndex = 6;
            this.Help_ToolTip.SetToolTip(this.Glyph_Width_ByteBox, "The width of this glyph - i.e. the kerning/horizontal spacing for this glyph.\r\nWi" +
        "ll write to ROM if changed.");
            this.Glyph_Width_ByteBox.Value = ((byte)(0));
            this.Glyph_Width_ByteBox.ValueChanged += new System.EventHandler(this.Glyph_Width_ByteBox_ValueChanged);
            // 
            // Glyph_Width_Label
            // 
            this.Glyph_Width_Label.AutoSize = true;
            this.Glyph_Width_Label.Location = new System.Drawing.Point(163, 19);
            this.Glyph_Width_Label.Name = "Glyph_Width_Label";
            this.Glyph_Width_Label.Size = new System.Drawing.Size(41, 13);
            this.Glyph_Width_Label.TabIndex = 7;
            this.Glyph_Width_Label.Text = "Width :";
            // 
            // Font_ComboBox
            // 
            this.Font_ComboBox.FormattingEnabled = true;
            this.Font_ComboBox.Location = new System.Drawing.Point(12, 21);
            this.Font_ComboBox.Name = "Font_ComboBox";
            this.Font_ComboBox.Size = new System.Drawing.Size(138, 21);
            this.Font_ComboBox.TabIndex = 11;
            this.Help_ToolTip.SetToolTip(this.Font_ComboBox, "The font to view - changing this changes the preview display image above.");
            this.Font_ComboBox.SelectedIndexChanged += new System.EventHandler(this.Font_ComboBox_SelectedIndexChanged);
            // 
            // Font_InsertButton
            // 
            this.Font_InsertButton.Location = new System.Drawing.Point(169, 13);
            this.Font_InsertButton.Name = "Font_InsertButton";
            this.Font_InsertButton.Size = new System.Drawing.Size(99, 35);
            this.Font_InsertButton.TabIndex = 10;
            this.Font_InsertButton.Text = "Insert from file...";
            this.Help_ToolTip.SetToolTip(this.Font_InsertButton, "Click on this button to insert a new font/glyph charset to replace the currently " +
        "selected one.\r\nWill write to ROM if clicked, after prompting the user for an app" +
        "ropriate image file.");
            this.Font_InsertButton.UseVisualStyleBackColor = true;
            this.Font_InsertButton.Click += new System.EventHandler(this.Font_InsertButton_Click);
            // 
            // Text_Apply_Button
            // 
            this.Text_Apply_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Text_Apply_Button.Location = new System.Drawing.Point(179, 459);
            this.Text_Apply_Button.Name = "Text_Apply_Button";
            this.Text_Apply_Button.Size = new System.Drawing.Size(126, 37);
            this.Text_Apply_Button.TabIndex = 12;
            this.Text_Apply_Button.Text = "Apply Changes";
            this.Help_ToolTip.SetToolTip(this.Text_Apply_Button, "Click this button to apply the current changes written in the text area.\r\nWill wr" +
        "ite to ROM if clicked, and repoint the this text entry.");
            this.Text_Apply_Button.UseVisualStyleBackColor = true;
            this.Text_Apply_Button.Click += new System.EventHandler(this.Text_Apply_Button_Click);
            // 
            // Text_Cancel_Button
            // 
            this.Text_Cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Text_Cancel_Button.Location = new System.Drawing.Point(12, 459);
            this.Text_Cancel_Button.Name = "Text_Cancel_Button";
            this.Text_Cancel_Button.Size = new System.Drawing.Size(126, 37);
            this.Text_Cancel_Button.TabIndex = 13;
            this.Text_Cancel_Button.Text = "Reload Text";
            this.Help_ToolTip.SetToolTip(this.Text_Cancel_Button, "Restore the text area to its original state for this text entry.");
            this.Text_Cancel_Button.UseVisualStyleBackColor = true;
            this.Text_Cancel_Button.Click += new System.EventHandler(this.Text_Cancel_Button_Click);
            // 
            // Editor_Menu
            // 
            this.Editor_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File,
            this.Menu_Tools,
            this.Menu_View});
            this.Editor_Menu.Location = new System.Drawing.Point(0, 0);
            this.Editor_Menu.Name = "Editor_Menu";
            this.Editor_Menu.Size = new System.Drawing.Size(612, 24);
            this.Editor_Menu.TabIndex = 14;
            this.Editor_Menu.Text = "menuStrip1";
            // 
            // Menu_File
            // 
            this.Menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File_InsertEntry,
            this.File_InsertScript,
            this.File_InsertFolder,
            this.File_Separator,
            this.File_SaveEntry,
            this.File_SaveScript,
            this.File_SaveFolder});
            this.Menu_File.Name = "Menu_File";
            this.Menu_File.Size = new System.Drawing.Size(37, 20);
            this.Menu_File.Text = "File";
            // 
            // File_InsertEntry
            // 
            this.File_InsertEntry.Name = "File_InsertEntry";
            this.File_InsertEntry.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.File_InsertEntry.Size = new System.Drawing.Size(254, 22);
            this.File_InsertEntry.Text = "Insert text from file...";
            this.File_InsertEntry.Click += new System.EventHandler(this.File_InsertEntry_Click);
            // 
            // File_InsertScript
            // 
            this.File_InsertScript.Name = "File_InsertScript";
            this.File_InsertScript.Size = new System.Drawing.Size(254, 22);
            this.File_InsertScript.Text = "Insert script from txt file...";
            this.File_InsertScript.Click += new System.EventHandler(this.File_InsertScript_Click);
            // 
            // File_InsertFolder
            // 
            this.File_InsertFolder.Name = "File_InsertFolder";
            this.File_InsertFolder.Size = new System.Drawing.Size(254, 22);
            this.File_InsertFolder.Text = "Insert script from folder...";
            this.File_InsertFolder.Click += new System.EventHandler(this.File_InsertFolder_Click);
            // 
            // File_Separator
            // 
            this.File_Separator.Name = "File_Separator";
            this.File_Separator.Size = new System.Drawing.Size(251, 6);
            // 
            // File_SaveEntry
            // 
            this.File_SaveEntry.Name = "File_SaveEntry";
            this.File_SaveEntry.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.File_SaveEntry.Size = new System.Drawing.Size(254, 22);
            this.File_SaveEntry.Text = "Save current text entry...";
            this.File_SaveEntry.Click += new System.EventHandler(this.File_SaveEntry_Click);
            // 
            // File_SaveScript
            // 
            this.File_SaveScript.Name = "File_SaveScript";
            this.File_SaveScript.Size = new System.Drawing.Size(254, 22);
            this.File_SaveScript.Text = "Save entire game script to txt file...";
            this.File_SaveScript.Click += new System.EventHandler(this.File_SaveScript_Click);
            // 
            // File_SaveFolder
            // 
            this.File_SaveFolder.Name = "File_SaveFolder";
            this.File_SaveFolder.Size = new System.Drawing.Size(254, 22);
            this.File_SaveFolder.Text = "Save entire game script to folder...";
            this.File_SaveFolder.Click += new System.EventHandler(this.File_SaveFolder_Click);
            // 
            // Menu_Tools
            // 
            this.Menu_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Tool_Find,
            this.Tool_Replace});
            this.Menu_Tools.Name = "Menu_Tools";
            this.Menu_Tools.Size = new System.Drawing.Size(48, 20);
            this.Menu_Tools.Text = "Tools";
            // 
            // Tool_Find
            // 
            this.Tool_Find.Name = "Tool_Find";
            this.Tool_Find.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.Tool_Find.Size = new System.Drawing.Size(167, 22);
            this.Tool_Find.Text = "Find...";
            this.Tool_Find.Click += new System.EventHandler(this.Tool_Find_Click);
            // 
            // Tool_Replace
            // 
            this.Tool_Replace.Name = "Tool_Replace";
            this.Tool_Replace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.Tool_Replace.Size = new System.Drawing.Size(167, 22);
            this.Tool_Replace.Text = "Replace...";
            this.Tool_Replace.Click += new System.EventHandler(this.Tool_Replace_Click);
            // 
            // Menu_View
            // 
            this.Menu_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.View_Bytecodes});
            this.Menu_View.Name = "Menu_View";
            this.Menu_View.Size = new System.Drawing.Size(44, 20);
            this.Menu_View.Text = "View";
            // 
            // View_Bytecodes
            // 
            this.View_Bytecodes.CheckOnClick = true;
            this.View_Bytecodes.Name = "View_Bytecodes";
            this.View_Bytecodes.Size = new System.Drawing.Size(207, 22);
            this.View_Bytecodes.Text = "Commands as bytecodes";
            this.View_Bytecodes.Click += new System.EventHandler(this.View_Bytecodes_Click);
            // 
            // Text_Preview_ImageBox
            // 
            this.Text_Preview_ImageBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Text_Preview_ImageBox.BackColor = System.Drawing.SystemColors.Control;
            this.Text_Preview_ImageBox.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.Text_Preview_ImageBox.Location = new System.Drawing.Point(360, 36);
            this.Text_Preview_ImageBox.Name = "Text_Preview_ImageBox";
            this.Text_Preview_ImageBox.Size = new System.Drawing.Size(240, 64);
            this.Text_Preview_ImageBox.TabIndex = 15;
            this.Text_Preview_ImageBox.TabStop = false;
            this.Text_Preview_ImageBox.Text = "imageBox1";
            this.Help_ToolTip.SetToolTip(this.Text_Preview_ImageBox, "A preview of how the text will look like ingame.");
            // 
            // Text_Line_NumBox
            // 
            this.Text_Line_NumBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Text_Line_NumBox.Hexadecimal = true;
            this.Text_Line_NumBox.Location = new System.Drawing.Point(314, 80);
            this.Text_Line_NumBox.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.Text_Line_NumBox.Name = "Text_Line_NumBox";
            this.Text_Line_NumBox.Size = new System.Drawing.Size(40, 20);
            this.Text_Line_NumBox.TabIndex = 16;
            this.Help_ToolTip.SetToolTip(this.Text_Line_NumBox, "The line offset for the preview text bubble display.");
            this.Text_Line_NumBox.Value = ((byte)(0));
            this.Text_Line_NumBox.ValueChanged += new System.EventHandler(this.Text_Line_NumBox_ValueChanged);
            // 
            // Text_Line_Label
            // 
            this.Text_Line_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Text_Line_Label.AutoSize = true;
            this.Text_Line_Label.Location = new System.Drawing.Point(311, 64);
            this.Text_Line_Label.Name = "Text_Line_Label";
            this.Text_Line_Label.Size = new System.Drawing.Size(33, 13);
            this.Text_Line_Label.TabIndex = 17;
            this.Text_Line_Label.Text = "Line :";
            // 
            // Text_ASCII_CheckBox
            // 
            this.Text_ASCII_CheckBox.AutoSize = true;
            this.Text_ASCII_CheckBox.Location = new System.Drawing.Point(124, 28);
            this.Text_ASCII_CheckBox.Name = "Text_ASCII_CheckBox";
            this.Text_ASCII_CheckBox.Size = new System.Drawing.Size(53, 17);
            this.Text_ASCII_CheckBox.TabIndex = 18;
            this.Text_ASCII_CheckBox.Text = "ASCII";
            this.Help_ToolTip.SetToolTip(this.Text_ASCII_CheckBox, resources.GetString("Text_ASCII_CheckBox.ToolTip"));
            this.Text_ASCII_CheckBox.UseVisualStyleBackColor = true;
            this.Text_ASCII_CheckBox.CheckedChanged += new System.EventHandler(this.Text_ASCII_CheckBox_CheckedChanged);
            // 
            // TextEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 508);
            this.Controls.Add(this.Text_ASCII_CheckBox);
            this.Controls.Add(this.Text_Line_Label);
            this.Controls.Add(this.Text_Line_NumBox);
            this.Controls.Add(this.Text_Preview_ImageBox);
            this.Controls.Add(this.Text_Cancel_Button);
            this.Controls.Add(this.Text_Apply_Button);
            this.Controls.Add(this.Font_GroupBox);
            this.Controls.Add(this.Text_CodeBox);
            this.Controls.Add(this.Text_Pointer_Label);
            this.Controls.Add(this.EntryLabel);
            this.Controls.Add(this.Text_PointerBox);
            this.Controls.Add(this.EntryNumBox);
            this.Controls.Add(this.Editor_Menu);
            this.MainMenuStrip = this.Editor_Menu;
            this.MinimumSize = new System.Drawing.Size(628, 546);
            this.Name = "TextEditor";
            this.Text = "Text Editor";
            ((System.ComponentModel.ISupportInitialize)(this.EntryNumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Text_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Text_CodeBox)).EndInit();
            this.Font_GroupBox.ResumeLayout(false);
            this.Glyph_GroupBox.ResumeLayout(false);
            this.Glyph_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Glyph_Pointer_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Glyph_Shift_ByteBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Glyph_Address_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Glyph_Width_ByteBox)).EndInit();
            this.Editor_Menu.ResumeLayout(false);
            this.Editor_Menu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Text_Line_NumBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Components.ShortBox EntryNumBox;
        private Components.PointerBox Text_PointerBox;
        private System.Windows.Forms.Label EntryLabel;
        private System.Windows.Forms.Label Text_Pointer_Label;
        private Components.CodeBox Text_CodeBox;
        private Components.GridBox Font_GridBox;
        private System.Windows.Forms.GroupBox Font_GroupBox;
        private Components.ByteBox Glyph_Width_ByteBox;
        private Components.PointerBox Glyph_Address_PointerBox;
        private System.Windows.Forms.Label Glyph_Address_Label;
        private System.Windows.Forms.Label Glyph_Width_Label;
        private System.Windows.Forms.ComboBox Font_ComboBox;
        private System.Windows.Forms.Button Font_InsertButton;
        private System.Windows.Forms.Button Text_Apply_Button;
        private System.Windows.Forms.Button Text_Cancel_Button;
        private System.Windows.Forms.MenuStrip Editor_Menu;
        private System.Windows.Forms.ToolStripMenuItem Menu_File;
        private System.Windows.Forms.ToolStripMenuItem File_SaveEntry;
        private System.Windows.Forms.ToolStripMenuItem File_SaveFolder;
        private System.Windows.Forms.ToolStripSeparator File_Separator;
        private System.Windows.Forms.ToolStripMenuItem File_InsertEntry;
        private System.Windows.Forms.ToolStripMenuItem File_InsertFolder;
        private Components.ImageBox Text_Preview_ImageBox;
        private Components.ByteBox Text_Line_NumBox;
        private System.Windows.Forms.Label Text_Line_Label;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tools;
        private System.Windows.Forms.ToolStripMenuItem Tool_Find;
        private System.Windows.Forms.ToolStripMenuItem Tool_Replace;
        private System.Windows.Forms.ToolStripMenuItem Menu_View;
        private System.Windows.Forms.ToolStripMenuItem View_Bytecodes;
        private System.Windows.Forms.GroupBox Glyph_GroupBox;
        private Components.PointerBox Glyph_Pointer_PointerBox;
        private System.Windows.Forms.Label Glyph_Pointer_Label;
        private Components.ByteBox Glyph_Shift_ByteBox;
        private System.Windows.Forms.Label Glyph_Unknown_Label;
        private System.Windows.Forms.CheckBox Text_ASCII_CheckBox;
        private System.Windows.Forms.ToolStripMenuItem File_InsertScript;
        private System.Windows.Forms.ToolStripMenuItem File_SaveScript;
    }
}