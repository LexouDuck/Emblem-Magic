namespace EmblemMagic.Editors
{
    partial class SpellAnimEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpellAnimEditor));
            this.EntryArrayBox = new Magic.Components.ByteArrayBox();
            this.Palette_PointerBox = new Magic.Components.PointerBox();
            this.Tileset_PointerBox = new Magic.Components.PointerBox();
            this.TSA_PointerBox = new Magic.Components.PointerBox();
            this.Palette_Label = new System.Windows.Forms.Label();
            this.Tileset_Label = new System.Windows.Forms.Label();
            this.TSA_Label = new System.Windows.Forms.CheckBox();
            this.Spell_ImageBox = new Magic.Components.ImageBox();
            this.Refresh_Button = new System.Windows.Forms.Button();
            this.Palette_CheckBox = new System.Windows.Forms.CheckBox();
            this.Tileset_CheckBox = new System.Windows.Forms.CheckBox();
            this.TSA_CheckBox = new System.Windows.Forms.CheckBox();
            this.Spell_PaletteBox = new Magic.Components.PaletteBox();
            this.Name_TextBox = new System.Windows.Forms.TextBox();
            this.Looped_CheckBox = new System.Windows.Forms.CheckBox();
            this.Name_Label = new System.Windows.Forms.Label();
            this.MagicButton = new Magic.Components.MagicButton(App);
            this.Palette_Prev_Button = new System.Windows.Forms.Button();
            this.Palette_Next_Button = new System.Windows.Forms.Button();
            this.Tileset_Next_Button = new System.Windows.Forms.Button();
            this.Tileset_Prev_Button = new System.Windows.Forms.Button();
            this.TSA_Next_Button = new System.Windows.Forms.Button();
            this.TSA_Prev_Button = new System.Windows.Forms.Button();
            this.Height_NumberBox = new System.Windows.Forms.NumericUpDown();
            this.Width_NumberBox = new System.Windows.Forms.NumericUpDown();
            this.Height_Label = new System.Windows.Forms.Label();
            this.Width_Label = new System.Windows.Forms.Label();
            this.Next_Button = new System.Windows.Forms.Button();
            this.Prev_Button = new System.Windows.Forms.Button();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.File_OpenFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.File_OpenFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.File_SaveFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.File_SaveFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tools = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_View = new System.Windows.Forms.ToolStripMenuItem();
            this.View_PureASM = new System.Windows.Forms.ToolStripMenuItem();
            this.Anim_CodeBox = new Magic.Components.CodeBox();
            this.CopyASM_Button = new System.Windows.Forms.Button();
            this.AnimLoading_Label = new System.Windows.Forms.RadioButton();
            this.AnimLoading_PointerBox = new Magic.Components.PointerBox();
            this.LoopRoutine_Label = new System.Windows.Forms.RadioButton();
            this.Constructor_Label = new System.Windows.Forms.RadioButton();
            this.ASM_ListBox = new System.Windows.Forms.ListBox();
            this.LoopRoutine_PointerBox = new Magic.Components.PointerBox();
            this.Constructor_PointerBox = new Magic.Components.PointerBox();
            this.Functions_ListBox = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.Palette_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tileset_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TSA_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Height_NumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Width_NumberBox)).BeginInit();
            this.MenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Anim_CodeBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnimLoading_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoopRoutine_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Constructor_PointerBox)).BeginInit();
            this.SuspendLayout();
            // 
            // EntryArrayBox
            // 
            this.EntryArrayBox.Location = new System.Drawing.Point(12, 31);
            this.EntryArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.EntryArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.EntryArrayBox.Size = new System.Drawing.Size(243, 26);
            this.EntryArrayBox.TabIndex = 0;
            // 
            // Palette_PointerBox
            // 
            this.Palette_PointerBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Palette_PointerBox.Hexadecimal = true;
            this.Palette_PointerBox.Location = new System.Drawing.Point(482, 228);
            this.Palette_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Palette_PointerBox.Name = "Palette_PointerBox";
            this.Palette_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Palette_PointerBox.TabIndex = 2;
            // 
            // Tileset_PointerBox
            // 
            this.Tileset_PointerBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Tileset_PointerBox.Hexadecimal = true;
            this.Tileset_PointerBox.Location = new System.Drawing.Point(482, 254);
            this.Tileset_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Tileset_PointerBox.Name = "Tileset_PointerBox";
            this.Tileset_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Tileset_PointerBox.TabIndex = 3;
            // 
            // TSA_PointerBox
            // 
            this.TSA_PointerBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TSA_PointerBox.Hexadecimal = true;
            this.TSA_PointerBox.Location = new System.Drawing.Point(482, 281);
            this.TSA_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.TSA_PointerBox.Name = "TSA_PointerBox";
            this.TSA_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.TSA_PointerBox.TabIndex = 4;
            // 
            // Palette_Label
            // 
            this.Palette_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Palette_Label.AutoSize = true;
            this.Palette_Label.Location = new System.Drawing.Point(435, 230);
            this.Palette_Label.Name = "Palette_Label";
            this.Palette_Label.Size = new System.Drawing.Size(46, 13);
            this.Palette_Label.TabIndex = 6;
            this.Palette_Label.Text = "Palette :";
            // 
            // Tileset_Label
            // 
            this.Tileset_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Tileset_Label.AutoSize = true;
            this.Tileset_Label.Location = new System.Drawing.Point(437, 256);
            this.Tileset_Label.Name = "Tileset_Label";
            this.Tileset_Label.Size = new System.Drawing.Size(44, 13);
            this.Tileset_Label.TabIndex = 7;
            this.Tileset_Label.Text = "Tileset :";
            // 
            // TSA_Label
            // 
            this.TSA_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TSA_Label.AutoSize = true;
            this.TSA_Label.Location = new System.Drawing.Point(428, 284);
            this.TSA_Label.Name = "TSA_Label";
            this.TSA_Label.Size = new System.Drawing.Size(53, 17);
            this.TSA_Label.TabIndex = 8;
            this.TSA_Label.Text = "TSA :";
            this.TSA_Label.CheckedChanged += new System.EventHandler(this.TSA_Label_CheckedChanged);
            // 
            // Spell_ImageBox
            // 
            this.Spell_ImageBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Spell_ImageBox.BackColor = System.Drawing.SystemColors.Control;
            this.Spell_ImageBox.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.Spell_ImageBox.Location = new System.Drawing.Point(440, 31);
            this.Spell_ImageBox.Name = "Spell_ImageBox";
            this.Spell_ImageBox.Size = new System.Drawing.Size(256, 160);
            this.Spell_ImageBox.TabIndex = 10;
            this.Spell_ImageBox.TabStop = false;
            this.Spell_ImageBox.Text = "Spell_ImageBox";
            // 
            // Refresh_Button
            // 
            this.Refresh_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Refresh_Button.Location = new System.Drawing.Point(526, 316);
            this.Refresh_Button.Name = "Refresh_Button";
            this.Refresh_Button.Size = new System.Drawing.Size(52, 46);
            this.Refresh_Button.TabIndex = 13;
            this.Refresh_Button.Text = "Refresh Image";
            this.Refresh_Button.UseVisualStyleBackColor = true;
            this.Refresh_Button.Click += new System.EventHandler(this.Refresh_Button_Click);
            // 
            // Palette_CheckBox
            // 
            this.Palette_CheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Palette_CheckBox.AutoSize = true;
            this.Palette_CheckBox.Location = new System.Drawing.Point(558, 229);
            this.Palette_CheckBox.Name = "Palette_CheckBox";
            this.Palette_CheckBox.Size = new System.Drawing.Size(51, 17);
            this.Palette_CheckBox.TabIndex = 14;
            this.Palette_CheckBox.Text = "LZ77";
            this.Palette_CheckBox.UseVisualStyleBackColor = true;
            // 
            // Tileset_CheckBox
            // 
            this.Tileset_CheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Tileset_CheckBox.AutoSize = true;
            this.Tileset_CheckBox.Location = new System.Drawing.Point(558, 256);
            this.Tileset_CheckBox.Name = "Tileset_CheckBox";
            this.Tileset_CheckBox.Size = new System.Drawing.Size(51, 17);
            this.Tileset_CheckBox.TabIndex = 15;
            this.Tileset_CheckBox.Text = "LZ77";
            this.Tileset_CheckBox.UseVisualStyleBackColor = true;
            // 
            // TSA_CheckBox
            // 
            this.TSA_CheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TSA_CheckBox.AutoSize = true;
            this.TSA_CheckBox.Location = new System.Drawing.Point(558, 282);
            this.TSA_CheckBox.Name = "TSA_CheckBox";
            this.TSA_CheckBox.Size = new System.Drawing.Size(51, 17);
            this.TSA_CheckBox.TabIndex = 16;
            this.TSA_CheckBox.Text = "LZ77";
            this.TSA_CheckBox.UseVisualStyleBackColor = true;
            // 
            // Spell_PaletteBox
            // 
            this.Spell_PaletteBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Spell_PaletteBox.ColorsPerLine = 16;
            this.Spell_PaletteBox.Location = new System.Drawing.Point(592, 316);
            this.Spell_PaletteBox.Name = "Spell_PaletteBox";
            this.Spell_PaletteBox.Size = new System.Drawing.Size(128, 64);
            this.Spell_PaletteBox.TabIndex = 17;
            this.Spell_PaletteBox.TabStop = false;
            this.Spell_PaletteBox.Text = "paletteBox1";
            // 
            // Name_TextBox
            // 
            this.Name_TextBox.Location = new System.Drawing.Point(79, 63);
            this.Name_TextBox.Name = "Name_TextBox";
            this.Name_TextBox.Size = new System.Drawing.Size(108, 20);
            this.Name_TextBox.TabIndex = 30;
            // 
            // Looped_CheckBox
            // 
            this.Looped_CheckBox.AutoSize = true;
            this.Looped_CheckBox.Location = new System.Drawing.Point(193, 65);
            this.Looped_CheckBox.Name = "Looped_CheckBox";
            this.Looped_CheckBox.Size = new System.Drawing.Size(62, 17);
            this.Looped_CheckBox.TabIndex = 31;
            this.Looped_CheckBox.Text = "Looped";
            this.Looped_CheckBox.UseVisualStyleBackColor = true;
            // 
            // Name_Label
            // 
            this.Name_Label.AutoSize = true;
            this.Name_Label.Location = new System.Drawing.Point(32, 66);
            this.Name_Label.Name = "Name_Label";
            this.Name_Label.Size = new System.Drawing.Size(41, 13);
            this.Name_Label.TabIndex = 32;
            this.Name_Label.Text = "Name :";
            // 
            // MagicButton
            // 
            this.MagicButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.MagicButton.Location = new System.Drawing.Point(554, 368);
            this.MagicButton.MaximumSize = new System.Drawing.Size(24, 24);
            this.MagicButton.MinimumSize = new System.Drawing.Size(24, 24);
            this.MagicButton.Name = "MagicButton";
            this.MagicButton.Size = new System.Drawing.Size(24, 24);
            this.MagicButton.TabIndex = 34;
            this.MagicButton.UseVisualStyleBackColor = true;
            this.MagicButton.Click += new System.EventHandler(this.MagicButton_Click);
            // 
            // Palette_Prev_Button
            // 
            this.Palette_Prev_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Palette_Prev_Button.Location = new System.Drawing.Point(641, 229);
            this.Palette_Prev_Button.Name = "Palette_Prev_Button";
            this.Palette_Prev_Button.Size = new System.Drawing.Size(24, 20);
            this.Palette_Prev_Button.TabIndex = 35;
            this.Palette_Prev_Button.Text = "<-";
            this.Palette_Prev_Button.UseVisualStyleBackColor = true;
            this.Palette_Prev_Button.Click += new System.EventHandler(this.Palette_Prev_Button_Click);
            // 
            // Palette_Next_Button
            // 
            this.Palette_Next_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Palette_Next_Button.Location = new System.Drawing.Point(671, 229);
            this.Palette_Next_Button.Name = "Palette_Next_Button";
            this.Palette_Next_Button.Size = new System.Drawing.Size(24, 20);
            this.Palette_Next_Button.TabIndex = 36;
            this.Palette_Next_Button.Text = "->";
            this.Palette_Next_Button.UseVisualStyleBackColor = true;
            this.Palette_Next_Button.Click += new System.EventHandler(this.Palette_Next_Button_Click);
            // 
            // Tileset_Next_Button
            // 
            this.Tileset_Next_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Tileset_Next_Button.Location = new System.Drawing.Point(671, 255);
            this.Tileset_Next_Button.Name = "Tileset_Next_Button";
            this.Tileset_Next_Button.Size = new System.Drawing.Size(24, 20);
            this.Tileset_Next_Button.TabIndex = 38;
            this.Tileset_Next_Button.Text = "->";
            this.Tileset_Next_Button.UseVisualStyleBackColor = true;
            this.Tileset_Next_Button.Click += new System.EventHandler(this.Tileset_Next_Button_Click);
            // 
            // Tileset_Prev_Button
            // 
            this.Tileset_Prev_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Tileset_Prev_Button.Location = new System.Drawing.Point(641, 255);
            this.Tileset_Prev_Button.Name = "Tileset_Prev_Button";
            this.Tileset_Prev_Button.Size = new System.Drawing.Size(24, 20);
            this.Tileset_Prev_Button.TabIndex = 37;
            this.Tileset_Prev_Button.Text = "<-";
            this.Tileset_Prev_Button.UseVisualStyleBackColor = true;
            this.Tileset_Prev_Button.Click += new System.EventHandler(this.Tileset_Prev_Button_Click);
            // 
            // TSA_Next_Button
            // 
            this.TSA_Next_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TSA_Next_Button.Location = new System.Drawing.Point(671, 281);
            this.TSA_Next_Button.Name = "TSA_Next_Button";
            this.TSA_Next_Button.Size = new System.Drawing.Size(24, 20);
            this.TSA_Next_Button.TabIndex = 40;
            this.TSA_Next_Button.Text = "->";
            this.TSA_Next_Button.UseVisualStyleBackColor = true;
            this.TSA_Next_Button.Click += new System.EventHandler(this.TSA_Next_Button_Click);
            // 
            // TSA_Prev_Button
            // 
            this.TSA_Prev_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TSA_Prev_Button.Location = new System.Drawing.Point(641, 281);
            this.TSA_Prev_Button.Name = "TSA_Prev_Button";
            this.TSA_Prev_Button.Size = new System.Drawing.Size(24, 20);
            this.TSA_Prev_Button.TabIndex = 39;
            this.TSA_Prev_Button.Text = "<-";
            this.TSA_Prev_Button.UseVisualStyleBackColor = true;
            this.TSA_Prev_Button.Click += new System.EventHandler(this.TSA_Prev_Button_Click);
            // 
            // Height_NumberBox
            // 
            this.Height_NumberBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Height_NumberBox.Location = new System.Drawing.Point(474, 342);
            this.Height_NumberBox.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.Height_NumberBox.Name = "Height_NumberBox";
            this.Height_NumberBox.Size = new System.Drawing.Size(46, 20);
            this.Height_NumberBox.TabIndex = 41;
            this.Height_NumberBox.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.Height_NumberBox.ValueChanged += new System.EventHandler(this.Height_NumberBox_ValueChanged);
            // 
            // Width_NumberBox
            // 
            this.Width_NumberBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Width_NumberBox.Location = new System.Drawing.Point(474, 316);
            this.Width_NumberBox.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.Width_NumberBox.Name = "Width_NumberBox";
            this.Width_NumberBox.Size = new System.Drawing.Size(46, 20);
            this.Width_NumberBox.TabIndex = 42;
            this.Width_NumberBox.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.Width_NumberBox.ValueChanged += new System.EventHandler(this.Width_NumberBox_ValueChanged);
            // 
            // Height_Label
            // 
            this.Height_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Height_Label.AutoSize = true;
            this.Height_Label.Location = new System.Drawing.Point(424, 344);
            this.Height_Label.Name = "Height_Label";
            this.Height_Label.Size = new System.Drawing.Size(44, 13);
            this.Height_Label.TabIndex = 43;
            this.Height_Label.Text = "Height :";
            // 
            // Width_Label
            // 
            this.Width_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Width_Label.AutoSize = true;
            this.Width_Label.Location = new System.Drawing.Point(427, 318);
            this.Width_Label.Name = "Width_Label";
            this.Width_Label.Size = new System.Drawing.Size(41, 13);
            this.Width_Label.TabIndex = 44;
            this.Width_Label.Text = "Width :";
            // 
            // Next_Button
            // 
            this.Next_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Next_Button.Location = new System.Drawing.Point(701, 228);
            this.Next_Button.Name = "Next_Button";
            this.Next_Button.Size = new System.Drawing.Size(19, 73);
            this.Next_Button.TabIndex = 45;
            this.Next_Button.Text = ">";
            this.Next_Button.UseVisualStyleBackColor = true;
            this.Next_Button.Click += new System.EventHandler(this.Next_Button_Click);
            // 
            // Prev_Button
            // 
            this.Prev_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Prev_Button.Location = new System.Drawing.Point(616, 228);
            this.Prev_Button.Name = "Prev_Button";
            this.Prev_Button.Size = new System.Drawing.Size(19, 73);
            this.Prev_Button.TabIndex = 46;
            this.Prev_Button.Text = "<";
            this.Prev_Button.UseVisualStyleBackColor = true;
            this.Prev_Button.Click += new System.EventHandler(this.Prev_Button_Click);
            // 
            // Menu
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File,
            this.Menu_Tools,
            this.Menu_View});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "Menu";
            this.MenuStrip.Size = new System.Drawing.Size(732, 24);
            this.MenuStrip.TabIndex = 47;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // Menu_File
            // 
            this.Menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File_OpenFolder,
            this.File_OpenFiles,
            this.toolStripSeparator1,
            this.File_SaveFolder,
            this.File_SaveFiles});
            this.Menu_File.Name = "Menu_File";
            this.Menu_File.Size = new System.Drawing.Size(37, 20);
            this.Menu_File.Text = "File";
            // 
            // File_OpenFolder
            // 
            this.File_OpenFolder.Name = "File_OpenFolder";
            this.File_OpenFolder.Size = new System.Drawing.Size(276, 22);
            this.File_OpenFolder.Text = "Compile anim from folder and script...";
            // 
            // File_OpenFiles
            // 
            this.File_OpenFiles.Name = "File_OpenFiles";
            this.File_OpenFiles.Size = new System.Drawing.Size(276, 22);
            this.File_OpenFiles.Text = "Insert anim from compiled files...";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(273, 6);
            // 
            // File_SaveFolder
            // 
            this.File_SaveFolder.Name = "File_SaveFolder";
            this.File_SaveFolder.Size = new System.Drawing.Size(276, 22);
            this.File_SaveFolder.Text = "Save current anim to folder...";
            // 
            // File_SaveFiles
            // 
            this.File_SaveFiles.Name = "File_SaveFiles";
            this.File_SaveFiles.Size = new System.Drawing.Size(276, 22);
            this.File_SaveFiles.Text = "Save current anim as compiled files...";
            // 
            // Menu_Tools
            // 
            this.Menu_Tools.Name = "Menu_Tools";
            this.Menu_Tools.Size = new System.Drawing.Size(48, 20);
            this.Menu_Tools.Text = "Tools";
            // 
            // Menu_View
            // 
            this.Menu_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.View_PureASM});
            this.Menu_View.Name = "Menu_View";
            this.Menu_View.Size = new System.Drawing.Size(44, 20);
            this.Menu_View.Text = "View";
            // 
            // View_PureASM
            // 
            this.View_PureASM.CheckOnClick = true;
            this.View_PureASM.Name = "View_PureASM";
            this.View_PureASM.Size = new System.Drawing.Size(313, 22);
            this.View_PureASM.Text = "View ASM dissassembly instead of anim code";
            this.View_PureASM.Click += new System.EventHandler(this.View_PureASM_Click);
            // 
            // Anim_CodeBox
            // 
            this.Anim_CodeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Anim_CodeBox.AutoCompleteBracketsList = new char[] {
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
            this.Anim_CodeBox.AutoScrollMinSize = new System.Drawing.Size(23, 12);
            this.Anim_CodeBox.BackBrush = null;
            this.Anim_CodeBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Anim_CodeBox.CharHeight = 12;
            this.Anim_CodeBox.CharWidth = 6;
            this.Anim_CodeBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Anim_CodeBox.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Anim_CodeBox.Font = new System.Drawing.Font("Consolas", 8F);
            this.Anim_CodeBox.IsReplaceMode = false;
            this.Anim_CodeBox.Location = new System.Drawing.Point(12, 168);
            this.Anim_CodeBox.Name = "Anim_CodeBox";
            this.Anim_CodeBox.Paddings = new System.Windows.Forms.Padding(0);
            this.Anim_CodeBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.Anim_CodeBox.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("Anim_CodeBox.ServiceColors")));
            this.Anim_CodeBox.Size = new System.Drawing.Size(396, 212);
            this.Anim_CodeBox.TabIndex = 56;
            this.Anim_CodeBox.Zoom = 100;
            // 
            // CopyASM_Button
            // 
            this.CopyASM_Button.Location = new System.Drawing.Point(360, 126);
            this.CopyASM_Button.Name = "CopyASM_Button";
            this.CopyASM_Button.Size = new System.Drawing.Size(48, 36);
            this.CopyASM_Button.TabIndex = 55;
            this.CopyASM_Button.Text = "Copy ASM";
            this.CopyASM_Button.UseVisualStyleBackColor = true;
            // 
            // AnimLoading_Label
            // 
            this.AnimLoading_Label.AutoSize = true;
            this.AnimLoading_Label.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AnimLoading_Label.Location = new System.Drawing.Point(16, 141);
            this.AnimLoading_Label.Name = "AnimLoading_Label";
            this.AnimLoading_Label.Size = new System.Drawing.Size(95, 17);
            this.AnimLoading_Label.TabIndex = 54;
            this.AnimLoading_Label.Text = "Anim Loading :";
            // 
            // AnimLoading_PointerBox
            // 
            this.AnimLoading_PointerBox.Hexadecimal = true;
            this.AnimLoading_PointerBox.Location = new System.Drawing.Point(117, 141);
            this.AnimLoading_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.AnimLoading_PointerBox.Name = "AnimLoading_PointerBox";
            this.AnimLoading_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.AnimLoading_PointerBox.TabIndex = 53;
            // 
            // LoopRoutine_Label
            // 
            this.LoopRoutine_Label.AutoSize = true;
            this.LoopRoutine_Label.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LoopRoutine_Label.Location = new System.Drawing.Point(16, 115);
            this.LoopRoutine_Label.Name = "LoopRoutine_Label";
            this.LoopRoutine_Label.Size = new System.Drawing.Size(95, 17);
            this.LoopRoutine_Label.TabIndex = 52;
            this.LoopRoutine_Label.Text = "Loop Routine :";
            // 
            // Constructor_Label
            // 
            this.Constructor_Label.AutoSize = true;
            this.Constructor_Label.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Constructor_Label.Checked = true;
            this.Constructor_Label.Location = new System.Drawing.Point(26, 89);
            this.Constructor_Label.Name = "Constructor_Label";
            this.Constructor_Label.Size = new System.Drawing.Size(85, 17);
            this.Constructor_Label.TabIndex = 51;
            this.Constructor_Label.TabStop = true;
            this.Constructor_Label.Text = "Constructor :";
            // 
            // ASM_ListBox
            // 
            this.ASM_ListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ASM_ListBox.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.ASM_ListBox.FormattingEnabled = true;
            this.ASM_ListBox.Location = new System.Drawing.Point(12, 168);
            this.ASM_ListBox.Name = "ASM_ListBox";
            this.ASM_ListBox.Size = new System.Drawing.Size(395, 212);
            this.ASM_ListBox.TabIndex = 50;
            this.ASM_ListBox.Visible = false;
            // 
            // LoopRoutine_PointerBox
            // 
            this.LoopRoutine_PointerBox.Hexadecimal = true;
            this.LoopRoutine_PointerBox.Location = new System.Drawing.Point(117, 115);
            this.LoopRoutine_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.LoopRoutine_PointerBox.Name = "LoopRoutine_PointerBox";
            this.LoopRoutine_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.LoopRoutine_PointerBox.TabIndex = 49;
            // 
            // Constructor_PointerBox
            // 
            this.Constructor_PointerBox.Hexadecimal = true;
            this.Constructor_PointerBox.Location = new System.Drawing.Point(117, 89);
            this.Constructor_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Constructor_PointerBox.Name = "Constructor_PointerBox";
            this.Constructor_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Constructor_PointerBox.TabIndex = 48;
            // 
            // Functions_ListBox
            // 
            this.Functions_ListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Functions_ListBox.FormattingEnabled = true;
            this.Functions_ListBox.Location = new System.Drawing.Point(193, 93);
            this.Functions_ListBox.Name = "Functions_ListBox";
            this.Functions_ListBox.ScrollAlwaysVisible = true;
            this.Functions_ListBox.Size = new System.Drawing.Size(161, 69);
            this.Functions_ListBox.TabIndex = 57;
            // 
            // SpellAnimEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 393);
            this.Controls.Add(this.Functions_ListBox);
            this.Controls.Add(this.Anim_CodeBox);
            this.Controls.Add(this.CopyASM_Button);
            this.Controls.Add(this.AnimLoading_Label);
            this.Controls.Add(this.AnimLoading_PointerBox);
            this.Controls.Add(this.LoopRoutine_Label);
            this.Controls.Add(this.Constructor_Label);
            this.Controls.Add(this.ASM_ListBox);
            this.Controls.Add(this.LoopRoutine_PointerBox);
            this.Controls.Add(this.Constructor_PointerBox);
            this.Controls.Add(this.Prev_Button);
            this.Controls.Add(this.Next_Button);
            this.Controls.Add(this.Width_Label);
            this.Controls.Add(this.Height_Label);
            this.Controls.Add(this.Width_NumberBox);
            this.Controls.Add(this.Height_NumberBox);
            this.Controls.Add(this.TSA_Next_Button);
            this.Controls.Add(this.TSA_Prev_Button);
            this.Controls.Add(this.Tileset_Next_Button);
            this.Controls.Add(this.Tileset_Prev_Button);
            this.Controls.Add(this.Palette_Next_Button);
            this.Controls.Add(this.Palette_Prev_Button);
            this.Controls.Add(this.MagicButton);
            this.Controls.Add(this.Name_Label);
            this.Controls.Add(this.Looped_CheckBox);
            this.Controls.Add(this.Name_TextBox);
            this.Controls.Add(this.Spell_PaletteBox);
            this.Controls.Add(this.TSA_CheckBox);
            this.Controls.Add(this.Tileset_CheckBox);
            this.Controls.Add(this.Palette_CheckBox);
            this.Controls.Add(this.Refresh_Button);
            this.Controls.Add(this.Spell_ImageBox);
            this.Controls.Add(this.TSA_Label);
            this.Controls.Add(this.Tileset_Label);
            this.Controls.Add(this.Palette_Label);
            this.Controls.Add(this.TSA_PointerBox);
            this.Controls.Add(this.Tileset_PointerBox);
            this.Controls.Add(this.Palette_PointerBox);
            this.Controls.Add(this.EntryArrayBox);
            this.Controls.Add(this.MenuStrip);
            this.MainMenuStrip = this.MenuStrip;
            this.MinimumSize = new System.Drawing.Size(748, 431);
            this.Name = "SpellAnimEditor";
            this.Text = "SpellAnimEditor";
            ((System.ComponentModel.ISupportInitialize)(this.Palette_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tileset_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TSA_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Height_NumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Width_NumberBox)).EndInit();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Anim_CodeBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnimLoading_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoopRoutine_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Constructor_PointerBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Magic.Components.ByteArrayBox EntryArrayBox;
        private Magic.Components.PointerBox Palette_PointerBox;
        private Magic.Components.PointerBox Tileset_PointerBox;
        private Magic.Components.PointerBox TSA_PointerBox;
        private System.Windows.Forms.Label Palette_Label;
        private System.Windows.Forms.Label Tileset_Label;
        private System.Windows.Forms.CheckBox TSA_Label;
        private Magic.Components.ImageBox Spell_ImageBox;
        private System.Windows.Forms.Button Refresh_Button;
        private System.Windows.Forms.CheckBox Palette_CheckBox;
        private System.Windows.Forms.CheckBox Tileset_CheckBox;
        private System.Windows.Forms.CheckBox TSA_CheckBox;
        private Magic.Components.PaletteBox Spell_PaletteBox;
        private System.Windows.Forms.TextBox Name_TextBox;
        private System.Windows.Forms.CheckBox Looped_CheckBox;
        private System.Windows.Forms.Label Name_Label;
        private Magic.Components.MagicButton MagicButton;
        private System.Windows.Forms.Button Palette_Prev_Button;
        private System.Windows.Forms.Button Palette_Next_Button;
        private System.Windows.Forms.Button Tileset_Next_Button;
        private System.Windows.Forms.Button Tileset_Prev_Button;
        private System.Windows.Forms.Button TSA_Next_Button;
        private System.Windows.Forms.Button TSA_Prev_Button;
        private System.Windows.Forms.NumericUpDown Height_NumberBox;
        private System.Windows.Forms.NumericUpDown Width_NumberBox;
        private System.Windows.Forms.Label Height_Label;
        private System.Windows.Forms.Label Width_Label;
        private System.Windows.Forms.Button Next_Button;
        private System.Windows.Forms.Button Prev_Button;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private Magic.Components.CodeBox Anim_CodeBox;
        private System.Windows.Forms.Button CopyASM_Button;
        private System.Windows.Forms.RadioButton AnimLoading_Label;
        private Magic.Components.PointerBox AnimLoading_PointerBox;
        private System.Windows.Forms.RadioButton LoopRoutine_Label;
        private System.Windows.Forms.RadioButton Constructor_Label;
        private System.Windows.Forms.ListBox ASM_ListBox;
        private Magic.Components.PointerBox LoopRoutine_PointerBox;
        private Magic.Components.PointerBox Constructor_PointerBox;
        private System.Windows.Forms.ListBox Functions_ListBox;
        private System.Windows.Forms.ToolStripMenuItem Menu_File;
        private System.Windows.Forms.ToolStripMenuItem File_OpenFolder;
        private System.Windows.Forms.ToolStripMenuItem File_OpenFiles;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem File_SaveFolder;
        private System.Windows.Forms.ToolStripMenuItem File_SaveFiles;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tools;
        private System.Windows.Forms.ToolStripMenuItem Menu_View;
        private System.Windows.Forms.ToolStripMenuItem View_PureASM;
    }
}