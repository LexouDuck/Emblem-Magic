namespace EmblemMagic.Editors
{
    partial class GraphicsEditor
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
            this.Palette_PaletteBox = new EmblemMagic.Components.PaletteBox();
            this.TSA_CheckBox = new System.Windows.Forms.CheckBox();
            this.Tileset_CheckBox = new System.Windows.Forms.CheckBox();
            this.Palette_CheckBox = new System.Windows.Forms.CheckBox();
            this.Image_ImageBox = new EmblemMagic.Components.ImageBox();
            this.Tileset_Label = new System.Windows.Forms.Label();
            this.Palette_Label = new System.Windows.Forms.Label();
            this.TSA_PointerBox = new EmblemMagic.Components.PointerBox();
            this.Tileset_PointerBox = new EmblemMagic.Components.PointerBox();
            this.Palette_PointerBox = new EmblemMagic.Components.PointerBox();
            this.Width_NumBox = new System.Windows.Forms.NumericUpDown();
            this.Height_NumBox = new System.Windows.Forms.NumericUpDown();
            this.X_Label = new System.Windows.Forms.Label();
            this.Palette_Index_Label = new System.Windows.Forms.Label();
            this.Palette_Index_NumBox = new System.Windows.Forms.NumericUpDown();
            this.Editor_Menu = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Insert = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.File_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tool = new System.Windows.Forms.ToolStripMenuItem();
            this.Tool_OpenPaletteEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.Tool_OpenTSAEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_View = new System.Windows.Forms.ToolStripMenuItem();
            this.View_GrayscalePalette = new System.Windows.Forms.ToolStripMenuItem();
            this.TSA_Label = new System.Windows.Forms.CheckBox();
            this.TSA_FlipRows_CheckBox = new System.Windows.Forms.CheckBox();
            this.Address_GroupBox = new System.Windows.Forms.GroupBox();
            this.Tileset_GroupBox = new System.Windows.Forms.GroupBox();
            this.Tileset_8bpp_RadioButton = new System.Windows.Forms.RadioButton();
            this.Tileset_4bpp_RadioButton = new System.Windows.Forms.RadioButton();
            this.Tileset_2bpp_RadioButton = new System.Windows.Forms.RadioButton();
            this.Palette_GroupBox = new System.Windows.Forms.GroupBox();
            this.Palette_Opaque_CheckBox = new System.Windows.Forms.CheckBox();
            this.TSA_GroupBox = new System.Windows.Forms.GroupBox();
            this.Size_GroupBox = new System.Windows.Forms.GroupBox();
            this.Prev_Button = new System.Windows.Forms.Button();
            this.Next_Button = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.TSA_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tileset_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Palette_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Width_NumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Height_NumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Palette_Index_NumBox)).BeginInit();
            this.Editor_Menu.SuspendLayout();
            this.Address_GroupBox.SuspendLayout();
            this.Tileset_GroupBox.SuspendLayout();
            this.Palette_GroupBox.SuspendLayout();
            this.TSA_GroupBox.SuspendLayout();
            this.Size_GroupBox.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Palette_PaletteBox
            // 
            this.Palette_PaletteBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Palette_PaletteBox.ColorsPerLine = 16;
            this.Palette_PaletteBox.Location = new System.Drawing.Point(436, 161);
            this.Palette_PaletteBox.Name = "Palette_PaletteBox";
            this.Palette_PaletteBox.Size = new System.Drawing.Size(128, 128);
            this.Palette_PaletteBox.TabIndex = 29;
            this.Palette_PaletteBox.TabStop = false;
            this.Palette_PaletteBox.Text = "PaletteBox";
            this.Palette_PaletteBox.Click += new System.EventHandler(this.Tool_OpenPaletteEditor_Click);
            // 
            // TSA_CheckBox
            // 
            this.TSA_CheckBox.AutoSize = true;
            this.TSA_CheckBox.Enabled = false;
            this.TSA_CheckBox.Location = new System.Drawing.Point(142, 73);
            this.TSA_CheckBox.Name = "TSA_CheckBox";
            this.TSA_CheckBox.Size = new System.Drawing.Size(51, 17);
            this.TSA_CheckBox.TabIndex = 28;
            this.TSA_CheckBox.Text = "LZ77";
            this.TSA_CheckBox.UseVisualStyleBackColor = true;
            this.TSA_CheckBox.CheckedChanged += new System.EventHandler(this.TSA_CheckBox_CheckedChanged);
            // 
            // Tileset_CheckBox
            // 
            this.Tileset_CheckBox.AutoSize = true;
            this.Tileset_CheckBox.Location = new System.Drawing.Point(142, 46);
            this.Tileset_CheckBox.Name = "Tileset_CheckBox";
            this.Tileset_CheckBox.Size = new System.Drawing.Size(51, 17);
            this.Tileset_CheckBox.TabIndex = 27;
            this.Tileset_CheckBox.Text = "LZ77";
            this.Tileset_CheckBox.UseVisualStyleBackColor = true;
            this.Tileset_CheckBox.CheckedChanged += new System.EventHandler(this.Tileset_CheckBox_CheckedChanged);
            // 
            // Palette_CheckBox
            // 
            this.Palette_CheckBox.AutoSize = true;
            this.Palette_CheckBox.Location = new System.Drawing.Point(142, 21);
            this.Palette_CheckBox.Name = "Palette_CheckBox";
            this.Palette_CheckBox.Size = new System.Drawing.Size(51, 17);
            this.Palette_CheckBox.TabIndex = 26;
            this.Palette_CheckBox.Text = "LZ77";
            this.Palette_CheckBox.UseVisualStyleBackColor = true;
            this.Palette_CheckBox.CheckedChanged += new System.EventHandler(this.Palette_CheckBox_CheckedChanged);
            // 
            // Image_ImageBox
            // 
            this.Image_ImageBox.BackColor = System.Drawing.SystemColors.Control;
            this.Image_ImageBox.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.Image_ImageBox.Location = new System.Drawing.Point(3, 3);
            this.Image_ImageBox.Name = "Image_ImageBox";
            this.Image_ImageBox.Size = new System.Drawing.Size(256, 256);
            this.Image_ImageBox.TabIndex = 24;
            this.Image_ImageBox.TabStop = false;
            this.Image_ImageBox.Text = "ImageBox";
            // 
            // Tileset_Label
            // 
            this.Tileset_Label.AutoSize = true;
            this.Tileset_Label.Location = new System.Drawing.Point(16, 48);
            this.Tileset_Label.Name = "Tileset_Label";
            this.Tileset_Label.Size = new System.Drawing.Size(44, 13);
            this.Tileset_Label.TabIndex = 22;
            this.Tileset_Label.Text = "Tileset :";
            // 
            // Palette_Label
            // 
            this.Palette_Label.AutoSize = true;
            this.Palette_Label.Location = new System.Drawing.Point(14, 21);
            this.Palette_Label.Name = "Palette_Label";
            this.Palette_Label.Size = new System.Drawing.Size(46, 13);
            this.Palette_Label.TabIndex = 21;
            this.Palette_Label.Text = "Palette :";
            // 
            // TSA_PointerBox
            // 
            this.TSA_PointerBox.Enabled = false;
            this.TSA_PointerBox.Hexadecimal = true;
            this.TSA_PointerBox.Location = new System.Drawing.Point(66, 72);
            this.TSA_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.TSA_PointerBox.Name = "TSA_PointerBox";
            this.TSA_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.TSA_PointerBox.TabIndex = 20;
            this.TSA_PointerBox.ValueChanged += new System.EventHandler(this.TSA_PointerBox_ValueChanged);
            // 
            // Tileset_PointerBox
            // 
            this.Tileset_PointerBox.Hexadecimal = true;
            this.Tileset_PointerBox.Location = new System.Drawing.Point(66, 45);
            this.Tileset_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Tileset_PointerBox.Name = "Tileset_PointerBox";
            this.Tileset_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Tileset_PointerBox.TabIndex = 19;
            this.Tileset_PointerBox.ValueChanged += new System.EventHandler(this.Tileset_PointerBox_ValueChanged);
            // 
            // Palette_PointerBox
            // 
            this.Palette_PointerBox.Hexadecimal = true;
            this.Palette_PointerBox.Location = new System.Drawing.Point(66, 19);
            this.Palette_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Palette_PointerBox.Name = "Palette_PointerBox";
            this.Palette_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Palette_PointerBox.TabIndex = 18;
            this.Palette_PointerBox.ValueChanged += new System.EventHandler(this.Palette_PointerBox_ValueChanged);
            // 
            // Width_NumBox
            // 
            this.Width_NumBox.Location = new System.Drawing.Point(14, 17);
            this.Width_NumBox.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.Width_NumBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Width_NumBox.Name = "Width_NumBox";
            this.Width_NumBox.Size = new System.Drawing.Size(49, 20);
            this.Width_NumBox.TabIndex = 30;
            this.Width_NumBox.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.Width_NumBox.ValueChanged += new System.EventHandler(this.Width_NumBox_ValueChanged);
            // 
            // Height_NumBox
            // 
            this.Height_NumBox.Location = new System.Drawing.Point(87, 17);
            this.Height_NumBox.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.Height_NumBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Height_NumBox.Name = "Height_NumBox";
            this.Height_NumBox.Size = new System.Drawing.Size(49, 20);
            this.Height_NumBox.TabIndex = 31;
            this.Height_NumBox.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.Height_NumBox.ValueChanged += new System.EventHandler(this.Height_NumBox_ValueChanged);
            // 
            // X_Label
            // 
            this.X_Label.AutoSize = true;
            this.X_Label.Location = new System.Drawing.Point(69, 19);
            this.X_Label.Name = "X_Label";
            this.X_Label.Size = new System.Drawing.Size(12, 13);
            this.X_Label.TabIndex = 33;
            this.X_Label.Text = "x";
            // 
            // Palette_Index_Label
            // 
            this.Palette_Index_Label.AutoSize = true;
            this.Palette_Index_Label.Location = new System.Drawing.Point(11, 21);
            this.Palette_Index_Label.Name = "Palette_Index_Label";
            this.Palette_Index_Label.Size = new System.Drawing.Size(75, 13);
            this.Palette_Index_Label.TabIndex = 36;
            this.Palette_Index_Label.Text = "Palette Index :";
            // 
            // Palette_Index_NumBox
            // 
            this.Palette_Index_NumBox.Location = new System.Drawing.Point(92, 19);
            this.Palette_Index_NumBox.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.Palette_Index_NumBox.Name = "Palette_Index_NumBox";
            this.Palette_Index_NumBox.Size = new System.Drawing.Size(44, 20);
            this.Palette_Index_NumBox.TabIndex = 34;
            this.Palette_Index_NumBox.ValueChanged += new System.EventHandler(this.Palette_Index_NumBox_ValueChanged);
            // 
            // Editor_Menu
            // 
            this.Editor_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File,
            this.Menu_Tool,
            this.Menu_View});
            this.Editor_Menu.Location = new System.Drawing.Point(0, 0);
            this.Editor_Menu.Name = "Editor_Menu";
            this.Editor_Menu.Size = new System.Drawing.Size(577, 24);
            this.Editor_Menu.TabIndex = 38;
            this.Editor_Menu.Text = "menuStrip1";
            // 
            // Menu_File
            // 
            this.Menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File_Insert,
            this.toolStripSeparator1,
            this.File_Save});
            this.Menu_File.Name = "Menu_File";
            this.Menu_File.Size = new System.Drawing.Size(37, 20);
            this.Menu_File.Text = "File";
            // 
            // File_Insert
            // 
            this.File_Insert.Name = "File_Insert";
            this.File_Insert.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.File_Insert.Size = new System.Drawing.Size(185, 22);
            this.File_Insert.Text = "Insert image...";
            this.File_Insert.Click += new System.EventHandler(this.File_Insert_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(182, 6);
            // 
            // File_Save
            // 
            this.File_Save.Name = "File_Save";
            this.File_Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.File_Save.Size = new System.Drawing.Size(185, 22);
            this.File_Save.Text = "Save image...";
            this.File_Save.Click += new System.EventHandler(this.File_Save_Click);
            // 
            // Menu_Tool
            // 
            this.Menu_Tool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Tool_OpenPaletteEditor,
            this.Tool_OpenTSAEditor});
            this.Menu_Tool.Name = "Menu_Tool";
            this.Menu_Tool.Size = new System.Drawing.Size(48, 20);
            this.Menu_Tool.Text = "Tools";
            // 
            // Tool_OpenPaletteEditor
            // 
            this.Tool_OpenPaletteEditor.Name = "Tool_OpenPaletteEditor";
            this.Tool_OpenPaletteEditor.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.Tool_OpenPaletteEditor.Size = new System.Drawing.Size(226, 22);
            this.Tool_OpenPaletteEditor.Text = "Open Palette Editor...";
            this.Tool_OpenPaletteEditor.Click += new System.EventHandler(this.Tool_OpenPaletteEditor_Click);
            // 
            // Tool_OpenTSAEditor
            // 
            this.Tool_OpenTSAEditor.Enabled = false;
            this.Tool_OpenTSAEditor.Name = "Tool_OpenTSAEditor";
            this.Tool_OpenTSAEditor.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.Tool_OpenTSAEditor.Size = new System.Drawing.Size(226, 22);
            this.Tool_OpenTSAEditor.Text = "Open TSA Editor...";
            this.Tool_OpenTSAEditor.Click += new System.EventHandler(this.Tool_OpenTSAEditor_Click);
            // 
            // Menu_View
            // 
            this.Menu_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.View_GrayscalePalette});
            this.Menu_View.Name = "Menu_View";
            this.Menu_View.Size = new System.Drawing.Size(44, 20);
            this.Menu_View.Text = "View";
            // 
            // View_GrayscalePalette
            // 
            this.View_GrayscalePalette.CheckOnClick = true;
            this.View_GrayscalePalette.Name = "View_GrayscalePalette";
            this.View_GrayscalePalette.Size = new System.Drawing.Size(184, 22);
            this.View_GrayscalePalette.Text = "Use grayscale palette";
            this.View_GrayscalePalette.Click += new System.EventHandler(this.View_GrayscalePalette_Click);
            // 
            // TSA_Label
            // 
            this.TSA_Label.AutoSize = true;
            this.TSA_Label.Location = new System.Drawing.Point(9, 73);
            this.TSA_Label.Name = "TSA_Label";
            this.TSA_Label.Size = new System.Drawing.Size(53, 17);
            this.TSA_Label.TabIndex = 39;
            this.TSA_Label.Text = "TSA :";
            this.TSA_Label.UseVisualStyleBackColor = true;
            this.TSA_Label.CheckedChanged += new System.EventHandler(this.TSA_Label_CheckedChanged);
            // 
            // TSA_FlipRows_CheckBox
            // 
            this.TSA_FlipRows_CheckBox.AutoSize = true;
            this.TSA_FlipRows_CheckBox.Location = new System.Drawing.Point(9, 19);
            this.TSA_FlipRows_CheckBox.Name = "TSA_FlipRows_CheckBox";
            this.TSA_FlipRows_CheckBox.Size = new System.Drawing.Size(135, 17);
            this.TSA_FlipRows_CheckBox.TabIndex = 40;
            this.TSA_FlipRows_CheckBox.Text = "Flip TSA rows vertically";
            this.TSA_FlipRows_CheckBox.UseVisualStyleBackColor = true;
            this.TSA_FlipRows_CheckBox.CheckedChanged += new System.EventHandler(this.TSA_FlipRows_CheckBox_CheckedChanged);
            // 
            // Address_GroupBox
            // 
            this.Address_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Address_GroupBox.Controls.Add(this.TSA_PointerBox);
            this.Address_GroupBox.Controls.Add(this.Palette_PointerBox);
            this.Address_GroupBox.Controls.Add(this.TSA_Label);
            this.Address_GroupBox.Controls.Add(this.Tileset_PointerBox);
            this.Address_GroupBox.Controls.Add(this.Palette_Label);
            this.Address_GroupBox.Controls.Add(this.Tileset_Label);
            this.Address_GroupBox.Controls.Add(this.Palette_CheckBox);
            this.Address_GroupBox.Controls.Add(this.Tileset_CheckBox);
            this.Address_GroupBox.Controls.Add(this.TSA_CheckBox);
            this.Address_GroupBox.Location = new System.Drawing.Point(282, 22);
            this.Address_GroupBox.Name = "Address_GroupBox";
            this.Address_GroupBox.Size = new System.Drawing.Size(197, 100);
            this.Address_GroupBox.TabIndex = 41;
            this.Address_GroupBox.TabStop = false;
            this.Address_GroupBox.Text = "Display Image Addresses";
            // 
            // Tileset_GroupBox
            // 
            this.Tileset_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Tileset_GroupBox.Controls.Add(this.Tileset_8bpp_RadioButton);
            this.Tileset_GroupBox.Controls.Add(this.Tileset_4bpp_RadioButton);
            this.Tileset_GroupBox.Controls.Add(this.Tileset_2bpp_RadioButton);
            this.Tileset_GroupBox.Location = new System.Drawing.Point(485, 22);
            this.Tileset_GroupBox.Name = "Tileset_GroupBox";
            this.Tileset_GroupBox.Size = new System.Drawing.Size(79, 100);
            this.Tileset_GroupBox.TabIndex = 43;
            this.Tileset_GroupBox.TabStop = false;
            this.Tileset_GroupBox.Text = "Graphics";
            // 
            // Tileset_8bpp_RadioButton
            // 
            this.Tileset_8bpp_RadioButton.AutoSize = true;
            this.Tileset_8bpp_RadioButton.Location = new System.Drawing.Point(6, 72);
            this.Tileset_8bpp_RadioButton.Name = "Tileset_8bpp_RadioButton";
            this.Tileset_8bpp_RadioButton.Size = new System.Drawing.Size(71, 17);
            this.Tileset_8bpp_RadioButton.TabIndex = 2;
            this.Tileset_8bpp_RadioButton.Text = "8 bit/pixel";
            this.Tileset_8bpp_RadioButton.UseVisualStyleBackColor = true;
            this.Tileset_8bpp_RadioButton.CheckedChanged += new System.EventHandler(this.Tileset_8bpp_RadioButton_CheckedChanged);
            // 
            // Tileset_4bpp_RadioButton
            // 
            this.Tileset_4bpp_RadioButton.AutoSize = true;
            this.Tileset_4bpp_RadioButton.Checked = true;
            this.Tileset_4bpp_RadioButton.Location = new System.Drawing.Point(6, 46);
            this.Tileset_4bpp_RadioButton.Name = "Tileset_4bpp_RadioButton";
            this.Tileset_4bpp_RadioButton.Size = new System.Drawing.Size(71, 17);
            this.Tileset_4bpp_RadioButton.TabIndex = 1;
            this.Tileset_4bpp_RadioButton.TabStop = true;
            this.Tileset_4bpp_RadioButton.Text = "4 bit/pixel";
            this.Tileset_4bpp_RadioButton.UseVisualStyleBackColor = true;
            this.Tileset_4bpp_RadioButton.CheckedChanged += new System.EventHandler(this.Tileset_4bpp_RadioButton_CheckedChanged);
            // 
            // Tileset_2bpp_RadioButton
            // 
            this.Tileset_2bpp_RadioButton.AutoSize = true;
            this.Tileset_2bpp_RadioButton.Location = new System.Drawing.Point(6, 20);
            this.Tileset_2bpp_RadioButton.Name = "Tileset_2bpp_RadioButton";
            this.Tileset_2bpp_RadioButton.Size = new System.Drawing.Size(71, 17);
            this.Tileset_2bpp_RadioButton.TabIndex = 0;
            this.Tileset_2bpp_RadioButton.Text = "2 bit/pixel";
            this.Tileset_2bpp_RadioButton.UseVisualStyleBackColor = true;
            this.Tileset_2bpp_RadioButton.CheckedChanged += new System.EventHandler(this.Tileset_2bpp_RadioButton_CheckedChanged);
            // 
            // Palette_GroupBox
            // 
            this.Palette_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Palette_GroupBox.Controls.Add(this.Palette_Opaque_CheckBox);
            this.Palette_GroupBox.Controls.Add(this.Palette_Index_NumBox);
            this.Palette_GroupBox.Controls.Add(this.Palette_Index_Label);
            this.Palette_GroupBox.Location = new System.Drawing.Point(282, 177);
            this.Palette_GroupBox.Name = "Palette_GroupBox";
            this.Palette_GroupBox.Size = new System.Drawing.Size(148, 65);
            this.Palette_GroupBox.TabIndex = 44;
            this.Palette_GroupBox.TabStop = false;
            this.Palette_GroupBox.Text = "Palette Options";
            // 
            // Palette_Opaque_CheckBox
            // 
            this.Palette_Opaque_CheckBox.AutoSize = true;
            this.Palette_Opaque_CheckBox.Location = new System.Drawing.Point(11, 42);
            this.Palette_Opaque_CheckBox.Name = "Palette_Opaque_CheckBox";
            this.Palette_Opaque_CheckBox.Size = new System.Drawing.Size(125, 17);
            this.Palette_Opaque_CheckBox.TabIndex = 45;
            this.Palette_Opaque_CheckBox.Text = "Force palette opacity";
            this.Palette_Opaque_CheckBox.UseVisualStyleBackColor = true;
            this.Palette_Opaque_CheckBox.CheckedChanged += new System.EventHandler(this.Palette_Opaque_CheckBox_CheckedChanged);
            // 
            // TSA_GroupBox
            // 
            this.TSA_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TSA_GroupBox.Controls.Add(this.TSA_FlipRows_CheckBox);
            this.TSA_GroupBox.Enabled = false;
            this.TSA_GroupBox.Location = new System.Drawing.Point(282, 248);
            this.TSA_GroupBox.Name = "TSA_GroupBox";
            this.TSA_GroupBox.Size = new System.Drawing.Size(148, 41);
            this.TSA_GroupBox.TabIndex = 45;
            this.TSA_GroupBox.TabStop = false;
            this.TSA_GroupBox.Text = "TSA Options";
            // 
            // Size_GroupBox
            // 
            this.Size_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Size_GroupBox.Controls.Add(this.Width_NumBox);
            this.Size_GroupBox.Controls.Add(this.X_Label);
            this.Size_GroupBox.Controls.Add(this.Height_NumBox);
            this.Size_GroupBox.Location = new System.Drawing.Point(282, 128);
            this.Size_GroupBox.Name = "Size_GroupBox";
            this.Size_GroupBox.Size = new System.Drawing.Size(148, 43);
            this.Size_GroupBox.TabIndex = 46;
            this.Size_GroupBox.TabStop = false;
            this.Size_GroupBox.Text = "Display Size (in tiles)";
            // 
            // Prev_Button
            // 
            this.Prev_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Prev_Button.Location = new System.Drawing.Point(436, 128);
            this.Prev_Button.Name = "Prev_Button";
            this.Prev_Button.Size = new System.Drawing.Size(64, 29);
            this.Prev_Button.TabIndex = 47;
            this.Prev_Button.Text = "Previous";
            this.Prev_Button.UseVisualStyleBackColor = true;
            this.Prev_Button.Click += new System.EventHandler(this.Prev_Button_Click);
            // 
            // Next_Button
            // 
            this.Next_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Next_Button.Location = new System.Drawing.Point(500, 128);
            this.Next_Button.Name = "Next_Button";
            this.Next_Button.Size = new System.Drawing.Size(64, 29);
            this.Next_Button.TabIndex = 48;
            this.Next_Button.Text = "Next";
            this.Next_Button.UseVisualStyleBackColor = true;
            this.Next_Button.Click += new System.EventHandler(this.Next_Button_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.Image_ImageBox);
            this.panel1.Location = new System.Drawing.Point(12, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(264, 262);
            this.panel1.TabIndex = 49;
            // 
            // GraphicsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 302);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Next_Button);
            this.Controls.Add(this.Prev_Button);
            this.Controls.Add(this.Size_GroupBox);
            this.Controls.Add(this.TSA_GroupBox);
            this.Controls.Add(this.Palette_GroupBox);
            this.Controls.Add(this.Tileset_GroupBox);
            this.Controls.Add(this.Address_GroupBox);
            this.Controls.Add(this.Palette_PaletteBox);
            this.Controls.Add(this.Editor_Menu);
            this.MainMenuStrip = this.Editor_Menu;
            this.MinimumSize = new System.Drawing.Size(593, 340);
            this.Name = "GraphicsEditor";
            this.Text = "Graphics Editor";
            ((System.ComponentModel.ISupportInitialize)(this.TSA_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tileset_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Palette_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Width_NumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Height_NumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Palette_Index_NumBox)).EndInit();
            this.Editor_Menu.ResumeLayout(false);
            this.Editor_Menu.PerformLayout();
            this.Address_GroupBox.ResumeLayout(false);
            this.Address_GroupBox.PerformLayout();
            this.Tileset_GroupBox.ResumeLayout(false);
            this.Tileset_GroupBox.PerformLayout();
            this.Palette_GroupBox.ResumeLayout(false);
            this.Palette_GroupBox.PerformLayout();
            this.TSA_GroupBox.ResumeLayout(false);
            this.TSA_GroupBox.PerformLayout();
            this.Size_GroupBox.ResumeLayout(false);
            this.Size_GroupBox.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Components.PaletteBox Palette_PaletteBox;
        private System.Windows.Forms.CheckBox TSA_CheckBox;
        private System.Windows.Forms.CheckBox Tileset_CheckBox;
        private System.Windows.Forms.CheckBox Palette_CheckBox;
        private Components.ImageBox Image_ImageBox;
        private System.Windows.Forms.Label Tileset_Label;
        private System.Windows.Forms.Label Palette_Label;
        private Components.PointerBox TSA_PointerBox;
        private Components.PointerBox Tileset_PointerBox;
        private Components.PointerBox Palette_PointerBox;
        private System.Windows.Forms.NumericUpDown Width_NumBox;
        private System.Windows.Forms.NumericUpDown Height_NumBox;
        private System.Windows.Forms.Label X_Label;
        private System.Windows.Forms.Label Palette_Index_Label;
        private System.Windows.Forms.NumericUpDown Palette_Index_NumBox;
        private System.Windows.Forms.MenuStrip Editor_Menu;
        private System.Windows.Forms.ToolStripMenuItem Menu_File;
        private System.Windows.Forms.ToolStripMenuItem File_Insert;
        private System.Windows.Forms.ToolStripMenuItem File_Save;
        private System.Windows.Forms.CheckBox TSA_Label;
        private System.Windows.Forms.CheckBox TSA_FlipRows_CheckBox;
        private System.Windows.Forms.GroupBox Address_GroupBox;
        private System.Windows.Forms.GroupBox Tileset_GroupBox;
        private System.Windows.Forms.RadioButton Tileset_8bpp_RadioButton;
        private System.Windows.Forms.RadioButton Tileset_4bpp_RadioButton;
        private System.Windows.Forms.RadioButton Tileset_2bpp_RadioButton;
        private System.Windows.Forms.GroupBox Palette_GroupBox;
        private System.Windows.Forms.CheckBox Palette_Opaque_CheckBox;
        private System.Windows.Forms.GroupBox TSA_GroupBox;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tool;
        private System.Windows.Forms.ToolStripMenuItem Tool_OpenPaletteEditor;
        private System.Windows.Forms.ToolStripMenuItem Tool_OpenTSAEditor;
        private System.Windows.Forms.GroupBox Size_GroupBox;
        private System.Windows.Forms.Button Prev_Button;
        private System.Windows.Forms.Button Next_Button;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem Menu_View;
        private System.Windows.Forms.ToolStripMenuItem View_GrayscalePalette;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}