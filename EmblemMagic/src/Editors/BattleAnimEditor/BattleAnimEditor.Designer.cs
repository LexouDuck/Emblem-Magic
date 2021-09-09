namespace EmblemMagic.Editors
{
    partial class BattleAnimEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BattleAnimEditor));
            this.AnimCode_Apply_Button = new System.Windows.Forms.Button();
            this.Palette_GroupBox = new System.Windows.Forms.GroupBox();
            this.Palette_Character_Current_ArrayBox = new Magic.Components.ByteArrayBox();
            this.Palette_Character_PointerBox = new Magic.Components.PointerBox();
            this.Palette_Default_Button = new System.Windows.Forms.RadioButton();
            this.Palette_PaletteBox = new Magic.Components.PaletteBox();
            this.Palette_Default_PointerBox = new Magic.Components.PointerBox();
            this.Palette_Character_ArrayBox = new Magic.Components.ByteArrayBox();
            this.Palette_Character_TextBox = new System.Windows.Forms.TextBox();
            this.Palette_Character_Button = new System.Windows.Forms.RadioButton();
            this.Palette_Default_ArrayBox = new Magic.Components.ByteArrayBox();
            this.Anim_Frame_Label = new System.Windows.Forms.Label();
            this.Frame_ByteBox = new Magic.Components.ByteBox();
            this.OAM_L_Button = new System.Windows.Forms.RadioButton();
            this.OAM_R_Button = new System.Windows.Forms.RadioButton();
            this.AnimCodeBox = new Magic.Components.CodeBox();
            this.Anim_Name_Label = new System.Windows.Forms.Label();
            this.AnimData_Label = new System.Windows.Forms.Label();
            this.Sections_Label = new System.Windows.Forms.Label();
            this.Anim_Mode_ListBox = new System.Windows.Forms.ListBox();
            this.Anim_Name_TextBox = new System.Windows.Forms.TextBox();
            this.OAM_L_PointerBox = new Magic.Components.PointerBox();
            this.OAM_R_PointerBox = new Magic.Components.PointerBox();
            this.AnimData_PointerBox = new Magic.Components.PointerBox();
            this.Sections_PointerBox = new Magic.Components.PointerBox();
            this.Entry_ArrayBox = new Magic.Components.ByteArrayBox();
            this.Anim_ImageBox = new Magic.Components.ImageBox();
            this.AnimCode_Reset_Button = new System.Windows.Forms.Button();
            this.Anim_Play_Button = new System.Windows.Forms.Button();
            this.PlayAnimTimer = new System.Windows.Forms.Timer(this.components);
            this.MenuBar = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Create = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Insert = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.File_SaveFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.File_SaveFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.File_SaveAllGIF = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tool = new System.Windows.Forms.ToolStripMenuItem();
            this.Tool_OpenOAMEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.Tool_OpenPaletteEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_View = new System.Windows.Forms.ToolStripMenuItem();
            this.View_AllFrames = new System.Windows.Forms.ToolStripMenuItem();
            this.View_Layered = new System.Windows.Forms.ToolStripMenuItem();
            this.View_AllAnimCode = new System.Windows.Forms.ToolStripMenuItem();
            this.Item_GroupBox = new System.Windows.Forms.GroupBox();
            this.Item_LayoutPanel = new System.Windows.Forms.Panel();
            this.Item_PointerArrayBox = new Magic.Components.PointerArrayBox();
            this.Palette_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Palette_Character_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Palette_Default_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Frame_ByteBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnimCodeBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OAM_L_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OAM_R_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnimData_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Sections_PointerBox)).BeginInit();
            this.MenuBar.SuspendLayout();
            this.Item_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // AnimCode_Apply_Button
            // 
            this.AnimCode_Apply_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AnimCode_Apply_Button.Location = new System.Drawing.Point(282, 425);
            this.AnimCode_Apply_Button.Name = "AnimCode_Apply_Button";
            this.AnimCode_Apply_Button.Size = new System.Drawing.Size(121, 28);
            this.AnimCode_Apply_Button.TabIndex = 23;
            this.AnimCode_Apply_Button.Text = "Apply code changes";
            this.Help_ToolTip.SetToolTip(this.AnimCode_Apply_Button, "Apply whichever changes were written in the anim code text area.\r\nWill write to R" +
        "OM if changed.");
            this.AnimCode_Apply_Button.UseVisualStyleBackColor = true;
            this.AnimCode_Apply_Button.Click += new System.EventHandler(this.AnimCode_Apply_Button_Click);
            // 
            // Palette_GroupBox
            // 
            this.Palette_GroupBox.Controls.Add(this.Palette_Character_Current_ArrayBox);
            this.Palette_GroupBox.Controls.Add(this.Palette_Character_PointerBox);
            this.Palette_GroupBox.Controls.Add(this.Palette_Default_Button);
            this.Palette_GroupBox.Controls.Add(this.Palette_PaletteBox);
            this.Palette_GroupBox.Controls.Add(this.Palette_Default_PointerBox);
            this.Palette_GroupBox.Controls.Add(this.Palette_Character_ArrayBox);
            this.Palette_GroupBox.Controls.Add(this.Palette_Character_TextBox);
            this.Palette_GroupBox.Controls.Add(this.Palette_Character_Button);
            this.Palette_GroupBox.Controls.Add(this.Palette_Default_ArrayBox);
            this.Palette_GroupBox.Location = new System.Drawing.Point(12, 276);
            this.Palette_GroupBox.Name = "Palette_GroupBox";
            this.Palette_GroupBox.Size = new System.Drawing.Size(164, 174);
            this.Palette_GroupBox.TabIndex = 20;
            this.Palette_GroupBox.TabStop = false;
            this.Palette_GroupBox.Text = "Palettes";
            // 
            // Palette_Character_Current_ArrayBox
            // 
            this.Palette_Character_Current_ArrayBox.Enabled = false;
            this.Palette_Character_Current_ArrayBox.Location = new System.Drawing.Point(6, 145);
            this.Palette_Character_Current_ArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.Palette_Character_Current_ArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.Palette_Character_Current_ArrayBox.Size = new System.Drawing.Size(152, 26);
            this.Palette_Character_Current_ArrayBox.TabIndex = 23;
            this.Help_ToolTip.SetToolTip(this.Palette_Character_Current_ArrayBox, "Select which sub-palette of this character palette to view (ally, enemy, NPC, etc" +
        ")");
            this.Palette_Character_Current_ArrayBox.ValueChanged += new System.EventHandler(this.Palette_Character_Current_ArrayBox_ValueChanged);
            // 
            // Palette_Character_PointerBox
            // 
            this.Palette_Character_PointerBox.Enabled = false;
            this.Palette_Character_PointerBox.Hexadecimal = true;
            this.Palette_Character_PointerBox.Location = new System.Drawing.Point(88, 122);
            this.Palette_Character_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Palette_Character_PointerBox.Name = "Palette_Character_PointerBox";
            this.Palette_Character_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Palette_Character_PointerBox.TabIndex = 22;
            this.Help_ToolTip.SetToolTip(this.Palette_Character_PointerBox, "Pointer to the currently selected character-specific battle animation palette.\r\nW" +
        "ill write to ROM if changed. Palette is LZ77 compressed.");
            this.Palette_Character_PointerBox.ValueChanged += new System.EventHandler(this.Palette_Character_PointerBox_ValueChanged);
            // 
            // Palette_Default_Button
            // 
            this.Palette_Default_Button.AutoSize = true;
            this.Palette_Default_Button.Checked = true;
            this.Palette_Default_Button.Location = new System.Drawing.Point(6, 15);
            this.Palette_Default_Button.Name = "Palette_Default_Button";
            this.Palette_Default_Button.Size = new System.Drawing.Size(65, 17);
            this.Palette_Default_Button.TabIndex = 10;
            this.Palette_Default_Button.TabStop = true;
            this.Palette_Default_Button.Text = "Default :";
            this.Help_ToolTip.SetToolTip(this.Palette_Default_Button, "Radio button to view battle animations with the default class palettes.");
            this.Palette_Default_Button.UseVisualStyleBackColor = true;
            this.Palette_Default_Button.CheckedChanged += new System.EventHandler(this.Palette_CheckedChanged);
            // 
            // Palette_PaletteBox
            // 
            this.Palette_PaletteBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Palette_PaletteBox.ColorsPerLine = 8;
            this.Palette_PaletteBox.ForeColor = System.Drawing.SystemColors.Control;
            this.Palette_PaletteBox.Location = new System.Drawing.Point(6, 70);
            this.Palette_PaletteBox.Name = "Palette_PaletteBox";
            this.Palette_PaletteBox.Size = new System.Drawing.Size(80, 20);
            this.Palette_PaletteBox.TabIndex = 8;
            this.Palette_PaletteBox.TabStop = false;
            this.Palette_PaletteBox.Text = "GBAPaletteBox";
            this.Help_ToolTip.SetToolTip(this.Palette_PaletteBox, "The current palette selected to display the current battle animation.\r\nClicking o" +
        "n this will open a \"Palette Editor\" window, to edit the palette.");
            this.Palette_PaletteBox.Click += new System.EventHandler(this.Tool_OpenPaletteEditor_Click);
            // 
            // Palette_Default_PointerBox
            // 
            this.Palette_Default_PointerBox.Hexadecimal = true;
            this.Palette_Default_PointerBox.Location = new System.Drawing.Point(88, 15);
            this.Palette_Default_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Palette_Default_PointerBox.Name = "Palette_Default_PointerBox";
            this.Palette_Default_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Palette_Default_PointerBox.TabIndex = 6;
            this.Help_ToolTip.SetToolTip(this.Palette_Default_PointerBox, resources.GetString("Palette_Default_PointerBox.ToolTip"));
            this.Palette_Default_PointerBox.ValueChanged += new System.EventHandler(this.Palette_Default_PointerBox_ValueChanged);
            // 
            // Palette_Character_ArrayBox
            // 
            this.Palette_Character_ArrayBox.Enabled = false;
            this.Palette_Character_ArrayBox.Location = new System.Drawing.Point(6, 96);
            this.Palette_Character_ArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.Palette_Character_ArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.Palette_Character_ArrayBox.Size = new System.Drawing.Size(152, 26);
            this.Palette_Character_ArrayBox.TabIndex = 12;
            this.Help_ToolTip.SetToolTip(this.Palette_Character_ArrayBox, "Select which character battle animation palette to view.");
            this.Palette_Character_ArrayBox.ValueChanged += new System.EventHandler(this.Palette_Character_ArrayBox_ValueChanged);
            // 
            // Palette_Character_TextBox
            // 
            this.Palette_Character_TextBox.Enabled = false;
            this.Palette_Character_TextBox.Font = new System.Drawing.Font("Consolas", 8F);
            this.Palette_Character_TextBox.Location = new System.Drawing.Point(94, 70);
            this.Palette_Character_TextBox.Name = "Palette_Character_TextBox";
            this.Palette_Character_TextBox.Size = new System.Drawing.Size(64, 20);
            this.Palette_Character_TextBox.TabIndex = 21;
            this.Help_ToolTip.SetToolTip(this.Palette_Character_TextBox, "The name of the currently selected character battle anim palette. Is useless.\r\nWi" +
        "ll write to ROM if changed. Max length is 8 characters of text.");
            this.Palette_Character_TextBox.TextChanged += new System.EventHandler(this.Palette_Character_TextBox_TextChanged);
            // 
            // Palette_Character_Button
            // 
            this.Palette_Character_Button.AutoSize = true;
            this.Palette_Character_Button.Location = new System.Drawing.Point(6, 122);
            this.Palette_Character_Button.Name = "Palette_Character_Button";
            this.Palette_Character_Button.Size = new System.Drawing.Size(77, 17);
            this.Palette_Character_Button.TabIndex = 11;
            this.Palette_Character_Button.TabStop = true;
            this.Palette_Character_Button.Text = "Character :";
            this.Help_ToolTip.SetToolTip(this.Palette_Character_Button, "Radio button to view battle animations with a character-specific palette.");
            this.Palette_Character_Button.UseVisualStyleBackColor = true;
            // 
            // Palette_Default_ArrayBox
            // 
            this.Palette_Default_ArrayBox.Location = new System.Drawing.Point(6, 38);
            this.Palette_Default_ArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.Palette_Default_ArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.Palette_Default_ArrayBox.Size = new System.Drawing.Size(152, 26);
            this.Palette_Default_ArrayBox.TabIndex = 9;
            this.Help_ToolTip.SetToolTip(this.Palette_Default_ArrayBox, "Select which of the different default palettes to view (ally, enemy, NPC, etc).");
            this.Palette_Default_ArrayBox.ValueChanged += new System.EventHandler(this.Palette_Default_ArrayBox_ValueChanged);
            // 
            // Anim_Frame_Label
            // 
            this.Anim_Frame_Label.AutoSize = true;
            this.Anim_Frame_Label.Location = new System.Drawing.Point(164, 60);
            this.Anim_Frame_Label.Name = "Anim_Frame_Label";
            this.Anim_Frame_Label.Size = new System.Drawing.Size(42, 13);
            this.Anim_Frame_Label.TabIndex = 19;
            this.Anim_Frame_Label.Text = "Frame :";
            // 
            // Frame_ByteBox
            // 
            this.Frame_ByteBox.Hexadecimal = true;
            this.Frame_ByteBox.Location = new System.Drawing.Point(212, 58);
            this.Frame_ByteBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Frame_ByteBox.Name = "Frame_ByteBox";
            this.Frame_ByteBox.Size = new System.Drawing.Size(40, 20);
            this.Frame_ByteBox.TabIndex = 18;
            this.Help_ToolTip.SetToolTip(this.Frame_ByteBox, resources.GetString("Frame_ByteBox.ToolTip"));
            this.Frame_ByteBox.Value = ((byte)(0));
            this.Frame_ByteBox.ValueChanged += new System.EventHandler(this.FrameByteBox_ValueChanged);
            // 
            // OAM_L_Button
            // 
            this.OAM_L_Button.AutoSize = true;
            this.OAM_L_Button.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.OAM_L_Button.Location = new System.Drawing.Point(6, 250);
            this.OAM_L_Button.Name = "OAM_L_Button";
            this.OAM_L_Button.Size = new System.Drawing.Size(43, 17);
            this.OAM_L_Button.TabIndex = 17;
            this.OAM_L_Button.TabStop = true;
            this.OAM_L_Button.Text = "Left";
            this.Help_ToolTip.SetToolTip(this.OAM_L_Button, "If checked, view the right-facing-left OAM for this battle animation.");
            this.OAM_L_Button.UseVisualStyleBackColor = true;
            this.OAM_L_Button.CheckedChanged += new System.EventHandler(this.OAM_CheckBox_CheckedChanged);
            // 
            // OAM_R_Button
            // 
            this.OAM_R_Button.AutoSize = true;
            this.OAM_R_Button.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.OAM_R_Button.Checked = true;
            this.OAM_R_Button.Location = new System.Drawing.Point(126, 250);
            this.OAM_R_Button.Name = "OAM_R_Button";
            this.OAM_R_Button.Size = new System.Drawing.Size(50, 17);
            this.OAM_R_Button.TabIndex = 16;
            this.OAM_R_Button.TabStop = true;
            this.OAM_R_Button.Text = "Right";
            this.Help_ToolTip.SetToolTip(this.OAM_R_Button, "If checked, view the right-facing-left OAM for this battle animation.");
            this.OAM_R_Button.UseVisualStyleBackColor = true;
            // 
            // AnimCodeBox
            // 
            this.AnimCodeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AnimCodeBox.AutoCompleteBracketsList = new char[] {
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
            this.AnimCodeBox.AutoScrollMinSize = new System.Drawing.Size(23, 12);
            this.AnimCodeBox.BackBrush = null;
            this.AnimCodeBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AnimCodeBox.CharHeight = 12;
            this.AnimCodeBox.CharWidth = 6;
            this.AnimCodeBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.AnimCodeBox.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.AnimCodeBox.Font = new System.Drawing.Font("Consolas", 8F);
            this.AnimCodeBox.IsReplaceMode = false;
            this.AnimCodeBox.Location = new System.Drawing.Point(182, 280);
            this.AnimCodeBox.MinimumSize = new System.Drawing.Size(220, 140);
            this.AnimCodeBox.Name = "AnimCodeBox";
            this.AnimCodeBox.Paddings = new System.Windows.Forms.Padding(0);
            this.AnimCodeBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.AnimCodeBox.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("AnimCodeBox.ServiceColors")));
            this.AnimCodeBox.Size = new System.Drawing.Size(220, 140);
            this.AnimCodeBox.TabIndex = 14;
            this.Help_ToolTip.SetToolTip(this.AnimCodeBox, "This text area is used to view the anim code, and/or modify it on the fly.\r\nThe s" +
        "yntax is a bit different when calling frame IDs, rather than files (\"fXX\" rather" +
        " than \"f[file.png]\").");
            this.AnimCodeBox.Zoom = 100;
            // 
            // Anim_Name_Label
            // 
            this.Anim_Name_Label.AutoSize = true;
            this.Anim_Name_Label.Location = new System.Drawing.Point(12, 60);
            this.Anim_Name_Label.Name = "Anim_Name_Label";
            this.Anim_Name_Label.Size = new System.Drawing.Size(41, 13);
            this.Anim_Name_Label.TabIndex = 13;
            this.Anim_Name_Label.Text = "Name :";
            // 
            // AnimData_Label
            // 
            this.AnimData_Label.AutoSize = true;
            this.AnimData_Label.Location = new System.Drawing.Point(347, 234);
            this.AnimData_Label.Name = "AnimData_Label";
            this.AnimData_Label.Size = new System.Drawing.Size(56, 13);
            this.AnimData_Label.TabIndex = 12;
            this.AnimData_Label.Text = "Anim Data";
            // 
            // Sections_Label
            // 
            this.Sections_Label.AutoSize = true;
            this.Sections_Label.Location = new System.Drawing.Point(280, 234);
            this.Sections_Label.Name = "Sections_Label";
            this.Sections_Label.Size = new System.Drawing.Size(48, 13);
            this.Sections_Label.TabIndex = 11;
            this.Sections_Label.Text = "Sections";
            // 
            // Anim_Mode_ListBox
            // 
            this.Anim_Mode_ListBox.FormattingEnabled = true;
            this.Anim_Mode_ListBox.Location = new System.Drawing.Point(258, 58);
            this.Anim_Mode_ListBox.Name = "Anim_Mode_ListBox";
            this.Anim_Mode_ListBox.Size = new System.Drawing.Size(145, 173);
            this.Anim_Mode_ListBox.TabIndex = 10;
            this.Help_ToolTip.SetToolTip(this.Anim_Mode_ListBox, resources.GetString("Anim_Mode_ListBox.ToolTip"));
            // 
            // Anim_Name_TextBox
            // 
            this.Anim_Name_TextBox.Font = new System.Drawing.Font("Consolas", 8F);
            this.Anim_Name_TextBox.Location = new System.Drawing.Point(59, 58);
            this.Anim_Name_TextBox.Name = "Anim_Name_TextBox";
            this.Anim_Name_TextBox.Size = new System.Drawing.Size(70, 20);
            this.Anim_Name_TextBox.TabIndex = 7;
            this.Help_ToolTip.SetToolTip(this.Anim_Name_TextBox, "The name written in the ROM for this battle animation. (is essentially useless)\r\n" +
        "Will write to ROM if changed. Max length is 8 characters/bytes.");
            this.Anim_Name_TextBox.TextChanged += new System.EventHandler(this.Anim_NameTextBox_TextChanged);
            // 
            // OAM_L_PointerBox
            // 
            this.OAM_L_PointerBox.Hexadecimal = true;
            this.OAM_L_PointerBox.Location = new System.Drawing.Point(55, 250);
            this.OAM_L_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.OAM_L_PointerBox.Name = "OAM_L_PointerBox";
            this.OAM_L_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.OAM_L_PointerBox.TabIndex = 5;
            this.Help_ToolTip.SetToolTip(this.OAM_L_PointerBox, resources.GetString("OAM_L_PointerBox.ToolTip"));
            this.OAM_L_PointerBox.ValueChanged += new System.EventHandler(this.OAM_L_PointerBox_ValueChanged);
            // 
            // OAM_R_PointerBox
            // 
            this.OAM_R_PointerBox.Hexadecimal = true;
            this.OAM_R_PointerBox.Location = new System.Drawing.Point(182, 250);
            this.OAM_R_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.OAM_R_PointerBox.Name = "OAM_R_PointerBox";
            this.OAM_R_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.OAM_R_PointerBox.TabIndex = 4;
            this.Help_ToolTip.SetToolTip(this.OAM_R_PointerBox, resources.GetString("OAM_R_PointerBox.ToolTip"));
            this.OAM_R_PointerBox.ValueChanged += new System.EventHandler(this.OAM_R_PointerBox_ValueChanged);
            // 
            // AnimData_PointerBox
            // 
            this.AnimData_PointerBox.Hexadecimal = true;
            this.AnimData_PointerBox.Location = new System.Drawing.Point(333, 250);
            this.AnimData_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.AnimData_PointerBox.Name = "AnimData_PointerBox";
            this.AnimData_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.AnimData_PointerBox.TabIndex = 3;
            this.Help_ToolTip.SetToolTip(this.AnimData_PointerBox, resources.GetString("AnimData_PointerBox.ToolTip"));
            this.AnimData_PointerBox.ValueChanged += new System.EventHandler(this.AnimDataPointerBox_ValueChanged);
            // 
            // Sections_PointerBox
            // 
            this.Sections_PointerBox.Hexadecimal = true;
            this.Sections_PointerBox.Location = new System.Drawing.Point(258, 250);
            this.Sections_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Sections_PointerBox.Name = "Sections_PointerBox";
            this.Sections_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Sections_PointerBox.TabIndex = 2;
            this.Help_ToolTip.SetToolTip(this.Sections_PointerBox, resources.GetString("Sections_PointerBox.ToolTip"));
            this.Sections_PointerBox.ValueChanged += new System.EventHandler(this.SectionsPointerBox_ValueChanged);
            // 
            // Entry_ArrayBox
            // 
            this.Entry_ArrayBox.Location = new System.Drawing.Point(12, 27);
            this.Entry_ArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.Entry_ArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.Entry_ArrayBox.Size = new System.Drawing.Size(240, 26);
            this.Entry_ArrayBox.TabIndex = 1;
            this.Help_ToolTip.SetToolTip(this.Entry_ArrayBox, "Entry Selector - select which battle animation to view/edit");
            this.Entry_ArrayBox.ValueChanged += new System.EventHandler(this.EntryArrayBox_ValueChanged);
            // 
            // Anim_ImageBox
            // 
            this.Anim_ImageBox.BackColor = System.Drawing.SystemColors.Control;
            this.Anim_ImageBox.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.Anim_ImageBox.Location = new System.Drawing.Point(12, 84);
            this.Anim_ImageBox.Name = "Anim_ImageBox";
            this.Anim_ImageBox.Size = new System.Drawing.Size(240, 160);
            this.Anim_ImageBox.TabIndex = 0;
            this.Anim_ImageBox.TabStop = false;
            this.Anim_ImageBox.Text = "GBAImageBox";
            // 
            // AnimCode_Reset_Button
            // 
            this.AnimCode_Reset_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AnimCode_Reset_Button.Location = new System.Drawing.Point(181, 425);
            this.AnimCode_Reset_Button.Name = "AnimCode_Reset_Button";
            this.AnimCode_Reset_Button.Size = new System.Drawing.Size(95, 28);
            this.AnimCode_Reset_Button.TabIndex = 25;
            this.AnimCode_Reset_Button.Text = "Reload code";
            this.Help_ToolTip.SetToolTip(this.AnimCode_Reset_Button, "Revert the anim code back to its ROM state, removing and modifications written in" +
        " the anim code text area that haven\'t been aplied.");
            this.AnimCode_Reset_Button.UseVisualStyleBackColor = true;
            this.AnimCode_Reset_Button.Click += new System.EventHandler(this.AnimCode_Reset_Button_Click);
            // 
            // Anim_Play_Button
            // 
            this.Anim_Play_Button.Location = new System.Drawing.Point(258, 25);
            this.Anim_Play_Button.Name = "Anim_Play_Button";
            this.Anim_Play_Button.Size = new System.Drawing.Size(145, 27);
            this.Anim_Play_Button.TabIndex = 28;
            this.Anim_Play_Button.Text = "Play Animation";
            this.Help_ToolTip.SetToolTip(this.Anim_Play_Button, "Click on this button to view the currently selected battle animation mode.\r\nThe a" +
        "nimation might play slower than it actually should, especially if there are affi" +
        "ne sprite-rotation/scaling effects.");
            this.Anim_Play_Button.UseVisualStyleBackColor = true;
            this.Anim_Play_Button.Click += new System.EventHandler(this.PlayAnimButton_Click);
            // 
            // PlayAnimTimer
            // 
            this.PlayAnimTimer.Tick += new System.EventHandler(this.PlayAnimTimer_Tick);
            // 
            // MenuBar
            // 
            this.MenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File,
            this.Menu_Tool,
            this.Menu_View});
            this.MenuBar.Location = new System.Drawing.Point(0, 0);
            this.MenuBar.Name = "MenuBar";
            this.MenuBar.Size = new System.Drawing.Size(684, 24);
            this.MenuBar.TabIndex = 29;
            this.MenuBar.Text = "menuStrip1";
            // 
            // Menu_File
            // 
            this.Menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File_Create,
            this.File_Insert,
            this.toolStripSeparator1,
            this.File_SaveFolder,
            this.File_SaveFiles,
            this.toolStripMenuItem1,
            this.File_SaveAllGIF});
            this.Menu_File.Name = "Menu_File";
            this.Menu_File.Size = new System.Drawing.Size(37, 20);
            this.Menu_File.Text = "File";
            // 
            // File_Create
            // 
            this.File_Create.Name = "File_Create";
            this.File_Create.Size = new System.Drawing.Size(265, 22);
            this.File_Create.Text = "Create animation...";
            this.File_Create.Click += new System.EventHandler(this.File_Create_Click);
            // 
            // File_Insert
            // 
            this.File_Insert.Name = "File_Insert";
            this.File_Insert.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.File_Insert.Size = new System.Drawing.Size(265, 22);
            this.File_Insert.Text = "Insert anim from files...";
            this.File_Insert.Click += new System.EventHandler(this.File_Insert_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(262, 6);
            // 
            // File_SaveFolder
            // 
            this.File_SaveFolder.Name = "File_SaveFolder";
            this.File_SaveFolder.Size = new System.Drawing.Size(265, 22);
            this.File_SaveFolder.Text = "Save anim to folder...";
            this.File_SaveFolder.Click += new System.EventHandler(this.File_SaveFolder_Click);
            // 
            // File_SaveFiles
            // 
            this.File_SaveFiles.Name = "File_SaveFiles";
            this.File_SaveFiles.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.File_SaveFiles.Size = new System.Drawing.Size(265, 22);
            this.File_SaveFiles.Text = "Save anim to files...";
            this.File_SaveFiles.Click += new System.EventHandler(this.File_SaveFiles_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.toolStripMenuItem1.Size = new System.Drawing.Size(265, 22);
            this.toolStripMenuItem1.Text = "Save this anim to GIF...";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.File_SaveGIF_Click);
            // 
            // File_SaveAllGIF
            // 
            this.File_SaveAllGIF.Name = "File_SaveAllGIF";
            this.File_SaveAllGIF.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.G)));
            this.File_SaveAllGIF.Size = new System.Drawing.Size(265, 22);
            this.File_SaveAllGIF.Text = "Save all anims to GIF...";
            this.File_SaveAllGIF.Click += new System.EventHandler(this.File_SaveAllGIF_Click);
            // 
            // Menu_Tool
            // 
            this.Menu_Tool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Tool_OpenOAMEditor,
            this.Tool_OpenPaletteEditor});
            this.Menu_Tool.Name = "Menu_Tool";
            this.Menu_Tool.Size = new System.Drawing.Size(48, 20);
            this.Menu_Tool.Text = "Tools";
            // 
            // Tool_OpenOAMEditor
            // 
            this.Tool_OpenOAMEditor.Name = "Tool_OpenOAMEditor";
            this.Tool_OpenOAMEditor.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.Tool_OpenOAMEditor.Size = new System.Drawing.Size(252, 22);
            this.Tool_OpenOAMEditor.Text = "Open frame OAM Editor...";
            this.Tool_OpenOAMEditor.Click += new System.EventHandler(this.Tool_OpenOAMEditor_Click);
            // 
            // Tool_OpenPaletteEditor
            // 
            this.Tool_OpenPaletteEditor.Name = "Tool_OpenPaletteEditor";
            this.Tool_OpenPaletteEditor.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.Tool_OpenPaletteEditor.Size = new System.Drawing.Size(252, 22);
            this.Tool_OpenPaletteEditor.Text = "Open Palette Editor...";
            this.Tool_OpenPaletteEditor.Click += new System.EventHandler(this.Tool_OpenPaletteEditor_Click);
            // 
            // Menu_View
            // 
            this.Menu_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.View_AllFrames,
            this.View_Layered,
            this.View_AllAnimCode});
            this.Menu_View.Name = "Menu_View";
            this.Menu_View.Size = new System.Drawing.Size(44, 20);
            this.Menu_View.Text = "View";
            // 
            // View_AllFrames
            // 
            this.View_AllFrames.CheckOnClick = true;
            this.View_AllFrames.Name = "View_AllFrames";
            this.View_AllFrames.Size = new System.Drawing.Size(228, 22);
            this.View_AllFrames.Text = "View by frame number";
            this.View_AllFrames.Click += new System.EventHandler(this.View_AllFrames_Click);
            // 
            // View_Layered
            // 
            this.View_Layered.CheckOnClick = true;
            this.View_Layered.Name = "View_Layered";
            this.View_Layered.Size = new System.Drawing.Size(228, 22);
            this.View_Layered.Text = "View two-layer modes as one";
            this.View_Layered.Click += new System.EventHandler(this.View_Layered_Click);
            // 
            // View_AllAnimCode
            // 
            this.View_AllAnimCode.CheckOnClick = true;
            this.View_AllAnimCode.Name = "View_AllAnimCode";
            this.View_AllAnimCode.Size = new System.Drawing.Size(228, 22);
            this.View_AllAnimCode.Text = "View entire anim code";
            this.View_AllAnimCode.Click += new System.EventHandler(this.View_AllAnimCode_Click);
            // 
            // Item_GroupBox
            // 
            this.Item_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Item_GroupBox.Controls.Add(this.Item_LayoutPanel);
            this.Item_GroupBox.Controls.Add(this.Item_PointerArrayBox);
            this.Item_GroupBox.Location = new System.Drawing.Point(409, 30);
            this.Item_GroupBox.Name = "Item_GroupBox";
            this.Item_GroupBox.Size = new System.Drawing.Size(262, 423);
            this.Item_GroupBox.TabIndex = 30;
            this.Item_GroupBox.TabStop = false;
            this.Item_GroupBox.Text = "Battle Animation Class/Item Association";
            // 
            // Item_LayoutPanel
            // 
            this.Item_LayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Item_LayoutPanel.AutoScroll = true;
            this.Item_LayoutPanel.Location = new System.Drawing.Point(6, 51);
            this.Item_LayoutPanel.Name = "Item_LayoutPanel";
            this.Item_LayoutPanel.Size = new System.Drawing.Size(254, 366);
            this.Item_LayoutPanel.TabIndex = 1;
            this.Help_ToolTip.SetToolTip(this.Item_LayoutPanel, resources.GetString("Item_LayoutPanel.ToolTip"));
            // 
            // Item_PointerArrayBox
            // 
            this.Item_PointerArrayBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Item_PointerArrayBox.Location = new System.Drawing.Point(7, 19);
            this.Item_PointerArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.Item_PointerArrayBox.MinimumSize = new System.Drawing.Size(128, 26);
            this.Item_PointerArrayBox.Size = new System.Drawing.Size(249, 26);
            this.Item_PointerArrayBox.TabIndex = 0;
            this.Help_ToolTip.SetToolTip(this.Item_PointerArrayBox, resources.GetString("Item_PointerArrayBox.ToolTip"));
            this.Item_PointerArrayBox.ValueChanged += new System.EventHandler(this.Item_PointerArrayBox_ValueChanged);
            // 
            // BattleAnimEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 462);
            this.Controls.Add(this.Item_GroupBox);
            this.Controls.Add(this.Anim_Play_Button);
            this.Controls.Add(this.AnimCode_Reset_Button);
            this.Controls.Add(this.AnimCode_Apply_Button);
            this.Controls.Add(this.Palette_GroupBox);
            this.Controls.Add(this.Anim_Frame_Label);
            this.Controls.Add(this.Frame_ByteBox);
            this.Controls.Add(this.OAM_L_Button);
            this.Controls.Add(this.OAM_R_Button);
            this.Controls.Add(this.AnimCodeBox);
            this.Controls.Add(this.Anim_Name_Label);
            this.Controls.Add(this.AnimData_Label);
            this.Controls.Add(this.Sections_Label);
            this.Controls.Add(this.Anim_Mode_ListBox);
            this.Controls.Add(this.Anim_Name_TextBox);
            this.Controls.Add(this.OAM_L_PointerBox);
            this.Controls.Add(this.OAM_R_PointerBox);
            this.Controls.Add(this.AnimData_PointerBox);
            this.Controls.Add(this.Sections_PointerBox);
            this.Controls.Add(this.Entry_ArrayBox);
            this.Controls.Add(this.Anim_ImageBox);
            this.Controls.Add(this.MenuBar);
            this.MainMenuStrip = this.MenuBar;
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "BattleAnimEditor";
            this.Text = "Battle Animation Editor";
            this.Palette_GroupBox.ResumeLayout(false);
            this.Palette_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Palette_Character_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Palette_Default_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Frame_ByteBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnimCodeBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OAM_L_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OAM_R_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnimData_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Sections_PointerBox)).EndInit();
            this.MenuBar.ResumeLayout(false);
            this.MenuBar.PerformLayout();
            this.Item_GroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Magic.Components.ImageBox Anim_ImageBox;
        private Magic.Components.ByteArrayBox Entry_ArrayBox;
        private Magic.Components.PointerBox Sections_PointerBox;
        private Magic.Components.PointerBox AnimData_PointerBox;
        private Magic.Components.PointerBox OAM_R_PointerBox;
        private Magic.Components.PointerBox OAM_L_PointerBox;
        private Magic.Components.PointerBox Palette_Default_PointerBox;
        private System.Windows.Forms.TextBox Anim_Name_TextBox;
        private Magic.Components.ByteArrayBox Palette_Default_ArrayBox;
        private Magic.Components.PaletteBox Palette_PaletteBox;
        private System.Windows.Forms.ListBox Anim_Mode_ListBox;
        private System.Windows.Forms.Label Sections_Label;
        private System.Windows.Forms.Label AnimData_Label;
        private System.Windows.Forms.Label Anim_Name_Label;
        private Magic.Components.CodeBox AnimCodeBox;
        private System.Windows.Forms.RadioButton OAM_R_Button;
        private System.Windows.Forms.RadioButton OAM_L_Button;
        private Magic.Components.ByteBox Frame_ByteBox;
        private System.Windows.Forms.Label Anim_Frame_Label;
        private System.Windows.Forms.GroupBox Palette_GroupBox;
        private Magic.Components.ByteArrayBox Palette_Character_ArrayBox;
        private System.Windows.Forms.RadioButton Palette_Character_Button;
        private System.Windows.Forms.RadioButton Palette_Default_Button;
        private System.Windows.Forms.TextBox Palette_Character_TextBox;
        private Magic.Components.PointerBox Palette_Character_PointerBox;
        private System.Windows.Forms.Button AnimCode_Apply_Button;
        private System.Windows.Forms.Button AnimCode_Reset_Button;
        private System.Windows.Forms.Button Anim_Play_Button;
        private System.Windows.Forms.Timer PlayAnimTimer;
        private System.Windows.Forms.MenuStrip MenuBar;
        private System.Windows.Forms.ToolStripMenuItem Menu_File;
        private System.Windows.Forms.ToolStripMenuItem File_Create;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem File_SaveFiles;
        private System.Windows.Forms.ToolStripMenuItem File_Insert;
        private System.Windows.Forms.ToolStripMenuItem Menu_View;
        private System.Windows.Forms.ToolStripMenuItem View_AllFrames;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tool;
        private System.Windows.Forms.ToolStripMenuItem Tool_OpenOAMEditor;
        private System.Windows.Forms.ToolStripMenuItem Tool_OpenPaletteEditor;
        private System.Windows.Forms.ToolStripMenuItem View_AllAnimCode;
        private Magic.Components.ByteArrayBox Palette_Character_Current_ArrayBox;
        private System.Windows.Forms.ToolStripMenuItem File_SaveFolder;
        private System.Windows.Forms.ToolStripMenuItem File_SaveAllGIF;
        private System.Windows.Forms.ToolStripMenuItem View_Layered;
        private System.Windows.Forms.GroupBox Item_GroupBox;
        private Magic.Components.PointerArrayBox Item_PointerArrayBox;
        private System.Windows.Forms.Panel Item_LayoutPanel;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}