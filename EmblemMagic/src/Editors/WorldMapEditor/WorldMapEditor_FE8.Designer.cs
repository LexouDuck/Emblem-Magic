namespace EmblemMagic.Editors
{
    partial class WorldMapEditor_FE8
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
            this.Mini_Map_GroupBox = new System.Windows.Forms.GroupBox();
            this.Mini_Map_InsertButton = new System.Windows.Forms.Button();
            this.Mini_Map_GraphicsLabel = new System.Windows.Forms.Label();
            this.Mini_Map_TilesetPointerBox = new Magic.Components.PointerBox();
            this.Mini_Map_PaletteBox = new Magic.Components.PaletteBox();
            this.Mini_Map_PalettePointerBox = new Magic.Components.PointerBox();
            this.Mini_Map_PaletteLabel = new System.Windows.Forms.Label();
            this.LargeMap_GroupBox = new System.Windows.Forms.GroupBox();
            this.LargeMap_TSA_Button = new System.Windows.Forms.Button();
            this.LargeMap_TilesetLabel = new System.Windows.Forms.Label();
            this.LargeMap_InsertButton = new System.Windows.Forms.Button();
            this.LargeMap_TSALabel = new System.Windows.Forms.Label();
            this.LargeMap_PaletteBox = new Magic.Components.PaletteBox();
            this.LargeMap_TilesetPointerBox = new Magic.Components.PointerBox();
            this.LargeMap_TSAPointerBox = new Magic.Components.PointerBox();
            this.LargeMap_PaletteLabel = new System.Windows.Forms.Label();
            this.LargeMap_PalettePointerBox = new Magic.Components.PointerBox();
            this.SmallMap_GroupBox = new System.Windows.Forms.GroupBox();
            this.SmallMap_TSA_Button = new System.Windows.Forms.Button();
            this.SmallMap_InsertButton = new System.Windows.Forms.Button();
            this.SmallMap_TilesetLabel = new System.Windows.Forms.Label();
            this.SmallMap_TSALabel = new System.Windows.Forms.Label();
            this.SmallMap_TilesetPointerBox = new Magic.Components.PointerBox();
            this.SmallMap_PaletteBox = new Magic.Components.PaletteBox();
            this.SmallMap_TSAPointerBox = new Magic.Components.PointerBox();
            this.SmallMap_PalettePointerBox = new Magic.Components.PointerBox();
            this.SmallMap_PaletteLabel = new System.Windows.Forms.Label();
            this.Mini_Map_ImageBox = new Magic.Components.ImageBox();
            this.LargeMap_ImageBox = new Magic.Components.ImageBox();
            this.SmallMap_ImageBox = new Magic.Components.ImageBox();
            this.Mini_Map_NumberBox = new System.Windows.Forms.NumericUpDown();
            this.Mini_Map_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Mini_Map_TilesetPointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Mini_Map_PalettePointerBox)).BeginInit();
            this.LargeMap_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_TilesetPointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_TSAPointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_PalettePointerBox)).BeginInit();
            this.SmallMap_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SmallMap_TilesetPointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmallMap_TSAPointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmallMap_PalettePointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Mini_Map_NumberBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Mini_Map_GroupBox
            // 
            this.Mini_Map_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Mini_Map_GroupBox.Controls.Add(this.Mini_Map_InsertButton);
            this.Mini_Map_GroupBox.Controls.Add(this.Mini_Map_GraphicsLabel);
            this.Mini_Map_GroupBox.Controls.Add(this.Mini_Map_TilesetPointerBox);
            this.Mini_Map_GroupBox.Controls.Add(this.Mini_Map_PaletteBox);
            this.Mini_Map_GroupBox.Controls.Add(this.Mini_Map_PalettePointerBox);
            this.Mini_Map_GroupBox.Controls.Add(this.Mini_Map_PaletteLabel);
            this.Mini_Map_GroupBox.Location = new System.Drawing.Point(12, 342);
            this.Mini_Map_GroupBox.Name = "Mini_Map_GroupBox";
            this.Mini_Map_GroupBox.Size = new System.Drawing.Size(240, 91);
            this.Mini_Map_GroupBox.TabIndex = 15;
            this.Mini_Map_GroupBox.TabStop = false;
            this.Mini_Map_GroupBox.Text = "Mini Map (64x64)";
            // 
            // Mini_Map_InsertButton
            // 
            this.Mini_Map_InsertButton.Location = new System.Drawing.Point(146, 49);
            this.Mini_Map_InsertButton.Name = "Mini_Map_InsertButton";
            this.Mini_Map_InsertButton.Size = new System.Drawing.Size(88, 36);
            this.Mini_Map_InsertButton.TabIndex = 2;
            this.Mini_Map_InsertButton.Text = "Insert image...";
            this.Help_ToolTip.SetToolTip(this.Mini_Map_InsertButton, "Click on this to insert a new image to replace the ingame world map (tiny minimap" +
        " version).\r\nWill perform several writes to ROM when clicked.");
            this.Mini_Map_InsertButton.UseVisualStyleBackColor = true;
            this.Mini_Map_InsertButton.Click += new System.EventHandler(this.Mini_Map_InsertButton_Click);
            // 
            // Mini_Map_GraphicsLabel
            // 
            this.Mini_Map_GraphicsLabel.AutoSize = true;
            this.Mini_Map_GraphicsLabel.Location = new System.Drawing.Point(9, 62);
            this.Mini_Map_GraphicsLabel.Name = "Mini_Map_GraphicsLabel";
            this.Mini_Map_GraphicsLabel.Size = new System.Drawing.Size(55, 13);
            this.Mini_Map_GraphicsLabel.TabIndex = 7;
            this.Mini_Map_GraphicsLabel.Text = "Graphics :";
            // 
            // Mini_Map_TilesetPointerBox
            // 
            this.Mini_Map_TilesetPointerBox.Hexadecimal = true;
            this.Mini_Map_TilesetPointerBox.Location = new System.Drawing.Point(70, 60);
            this.Mini_Map_TilesetPointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Mini_Map_TilesetPointerBox.Name = "Mini_Map_TilesetPointerBox";
            this.Mini_Map_TilesetPointerBox.Size = new System.Drawing.Size(70, 20);
            this.Mini_Map_TilesetPointerBox.TabIndex = 6;
            this.Help_ToolTip.SetToolTip(this.Mini_Map_TilesetPointerBox, "Pointer to the pixel data for the tiny mini-map world map image.\r\nWill write to R" +
        "OM if changed. Is repointed when inserting a new image.");
            this.Mini_Map_TilesetPointerBox.ValueChanged += new System.EventHandler(this.Mini_Map_TilesetPointerBox_ValueChanged);
            // 
            // Mini_Map_PaletteBox
            // 
            this.Mini_Map_PaletteBox.ColorsPerLine = 8;
            this.Mini_Map_PaletteBox.Location = new System.Drawing.Point(146, 20);
            this.Mini_Map_PaletteBox.Name = "Mini_Map_PaletteBox";
            this.Mini_Map_PaletteBox.Size = new System.Drawing.Size(88, 24);
            this.Mini_Map_PaletteBox.TabIndex = 4;
            this.Mini_Map_PaletteBox.TabStop = false;
            this.Mini_Map_PaletteBox.Text = "paletteBox2";
            this.Help_ToolTip.SetToolTip(this.Mini_Map_PaletteBox, "The palette used for the world map image (tiny mini-map version).\r\nClick on this " +
        "to open a PaletteEditor, to modify this palette.");
            this.Mini_Map_PaletteBox.Click += new System.EventHandler(this.Mini_Map_PaletteBox_Click);
            // 
            // Mini_Map_PalettePointerBox
            // 
            this.Mini_Map_PalettePointerBox.Hexadecimal = true;
            this.Mini_Map_PalettePointerBox.Location = new System.Drawing.Point(70, 24);
            this.Mini_Map_PalettePointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Mini_Map_PalettePointerBox.Name = "Mini_Map_PalettePointerBox";
            this.Mini_Map_PalettePointerBox.Size = new System.Drawing.Size(70, 20);
            this.Mini_Map_PalettePointerBox.TabIndex = 8;
            this.Help_ToolTip.SetToolTip(this.Mini_Map_PalettePointerBox, "Pointer to the palette data for the tiny mini-map world map image.\r\nWill write to" +
        " ROM if changed. Is repointed when inserting a new image.");
            this.Mini_Map_PalettePointerBox.ValueChanged += new System.EventHandler(this.Mini_Map_PalettePointerBox_ValueChanged);
            // 
            // Mini_Map_PaletteLabel
            // 
            this.Mini_Map_PaletteLabel.AutoSize = true;
            this.Mini_Map_PaletteLabel.Location = new System.Drawing.Point(18, 26);
            this.Mini_Map_PaletteLabel.Name = "Mini_Map_PaletteLabel";
            this.Mini_Map_PaletteLabel.Size = new System.Drawing.Size(46, 13);
            this.Mini_Map_PaletteLabel.TabIndex = 9;
            this.Mini_Map_PaletteLabel.Text = "Palette :";
            // 
            // LargeMap_GroupBox
            // 
            this.LargeMap_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_TSA_Button);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_TilesetLabel);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_InsertButton);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_TSALabel);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_PaletteBox);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_TilesetPointerBox);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_TSAPointerBox);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_PaletteLabel);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_PalettePointerBox);
            this.LargeMap_GroupBox.Location = new System.Drawing.Point(337, 342);
            this.LargeMap_GroupBox.Name = "LargeMap_GroupBox";
            this.LargeMap_GroupBox.Size = new System.Drawing.Size(405, 91);
            this.LargeMap_GroupBox.TabIndex = 15;
            this.LargeMap_GroupBox.TabStop = false;
            this.LargeMap_GroupBox.Text = "Large Map (480x320)";
            // 
            // LargeMap_TSA_Button
            // 
            this.LargeMap_TSA_Button.Enabled = false;
            this.LargeMap_TSA_Button.Location = new System.Drawing.Point(146, 49);
            this.LargeMap_TSA_Button.Name = "LargeMap_TSA_Button";
            this.LargeMap_TSA_Button.Size = new System.Drawing.Size(119, 36);
            this.LargeMap_TSA_Button.TabIndex = 13;
            this.LargeMap_TSA_Button.Text = "Open Palette Map...";
            this.Help_ToolTip.SetToolTip(this.LargeMap_TSA_Button, "Click on this to open a PaletteMapEditor for the large world map image - this is " +
        "kind of like a TSA editor, but not quite.");
            this.LargeMap_TSA_Button.UseVisualStyleBackColor = true;
            this.LargeMap_TSA_Button.Click += new System.EventHandler(this.LargeMap_TSA_Button_Click);
            // 
            // LargeMap_TilesetLabel
            // 
            this.LargeMap_TilesetLabel.AutoSize = true;
            this.LargeMap_TilesetLabel.Location = new System.Drawing.Point(6, 25);
            this.LargeMap_TilesetLabel.Name = "LargeMap_TilesetLabel";
            this.LargeMap_TilesetLabel.Size = new System.Drawing.Size(55, 13);
            this.LargeMap_TilesetLabel.TabIndex = 13;
            this.LargeMap_TilesetLabel.Text = "Graphics :";
            // 
            // LargeMap_InsertButton
            // 
            this.LargeMap_InsertButton.Location = new System.Drawing.Point(271, 49);
            this.LargeMap_InsertButton.Name = "LargeMap_InsertButton";
            this.LargeMap_InsertButton.Size = new System.Drawing.Size(128, 36);
            this.LargeMap_InsertButton.TabIndex = 5;
            this.LargeMap_InsertButton.Text = "Insert from file...";
            this.Help_ToolTip.SetToolTip(this.LargeMap_InsertButton, "Click on this to insert a new image to replace the ingame world map (large versio" +
        "n).\r\nWill perform several writes to ROM when clicked.");
            this.LargeMap_InsertButton.UseVisualStyleBackColor = true;
            this.LargeMap_InsertButton.Click += new System.EventHandler(this.LargeMap_InsertButton_Click);
            // 
            // LargeMap_TSALabel
            // 
            this.LargeMap_TSALabel.AutoSize = true;
            this.LargeMap_TSALabel.Location = new System.Drawing.Point(27, 61);
            this.LargeMap_TSALabel.Name = "LargeMap_TSALabel";
            this.LargeMap_TSALabel.Size = new System.Drawing.Size(34, 13);
            this.LargeMap_TSALabel.TabIndex = 17;
            this.LargeMap_TSALabel.Text = "TSA :";
            // 
            // LargeMap_PaletteBox
            // 
            this.LargeMap_PaletteBox.ColorsPerLine = 16;
            this.LargeMap_PaletteBox.Location = new System.Drawing.Point(271, 14);
            this.LargeMap_PaletteBox.Name = "LargeMap_PaletteBox";
            this.LargeMap_PaletteBox.Size = new System.Drawing.Size(128, 32);
            this.LargeMap_PaletteBox.TabIndex = 0;
            this.LargeMap_PaletteBox.TabStop = false;
            this.LargeMap_PaletteBox.Text = "paletteBox1";
            this.Help_ToolTip.SetToolTip(this.LargeMap_PaletteBox, "The palette used for the world map image (large version).\r\nClick on this to open " +
        "a PaletteEditor, to modify this palette.");
            this.LargeMap_PaletteBox.Click += new System.EventHandler(this.LargeMap_PaletteBox_Click);
            // 
            // LargeMap_TilesetPointerBox
            // 
            this.LargeMap_TilesetPointerBox.Hexadecimal = true;
            this.LargeMap_TilesetPointerBox.Location = new System.Drawing.Point(67, 23);
            this.LargeMap_TilesetPointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.LargeMap_TilesetPointerBox.Name = "LargeMap_TilesetPointerBox";
            this.LargeMap_TilesetPointerBox.Size = new System.Drawing.Size(70, 20);
            this.LargeMap_TilesetPointerBox.TabIndex = 12;
            this.Help_ToolTip.SetToolTip(this.LargeMap_TilesetPointerBox, "Pointer to the pixel data for the large world map image.\r\nWill write to ROM if ch" +
        "anged. Is repointed when inserting a new image.");
            this.LargeMap_TilesetPointerBox.ValueChanged += new System.EventHandler(this.LargeMap_TilesetPointerBox_ValueChanged);
            // 
            // LargeMap_TSAPointerBox
            // 
            this.LargeMap_TSAPointerBox.Hexadecimal = true;
            this.LargeMap_TSAPointerBox.Location = new System.Drawing.Point(67, 59);
            this.LargeMap_TSAPointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.LargeMap_TSAPointerBox.Name = "LargeMap_TSAPointerBox";
            this.LargeMap_TSAPointerBox.Size = new System.Drawing.Size(70, 20);
            this.LargeMap_TSAPointerBox.TabIndex = 16;
            this.Help_ToolTip.SetToolTip(this.LargeMap_TSAPointerBox, "Pointer to the TSA array data for this world map image.\r\nWill write to ROM if cha" +
        "nged. Is repointed when inserting a new image.");
            this.LargeMap_TSAPointerBox.ValueChanged += new System.EventHandler(this.LargeMap_TSAPointerBox_ValueChanged);
            // 
            // LargeMap_PaletteLabel
            // 
            this.LargeMap_PaletteLabel.AutoSize = true;
            this.LargeMap_PaletteLabel.Location = new System.Drawing.Point(143, 25);
            this.LargeMap_PaletteLabel.Name = "LargeMap_PaletteLabel";
            this.LargeMap_PaletteLabel.Size = new System.Drawing.Size(46, 13);
            this.LargeMap_PaletteLabel.TabIndex = 15;
            this.LargeMap_PaletteLabel.Text = "Palette :";
            // 
            // LargeMap_PalettePointerBox
            // 
            this.LargeMap_PalettePointerBox.Hexadecimal = true;
            this.LargeMap_PalettePointerBox.Location = new System.Drawing.Point(195, 23);
            this.LargeMap_PalettePointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.LargeMap_PalettePointerBox.Name = "LargeMap_PalettePointerBox";
            this.LargeMap_PalettePointerBox.Size = new System.Drawing.Size(70, 20);
            this.LargeMap_PalettePointerBox.TabIndex = 14;
            this.Help_ToolTip.SetToolTip(this.LargeMap_PalettePointerBox, "Pointer to the palette data for the large world map image.\r\nWill write to ROM if " +
        "changed. Is repointed when inserting a new image.");
            this.LargeMap_PalettePointerBox.ValueChanged += new System.EventHandler(this.LargeMap_PalettePointerBox_ValueChanged);
            // 
            // SmallMap_GroupBox
            // 
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_TSA_Button);
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_InsertButton);
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_TilesetLabel);
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_TSALabel);
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_TilesetPointerBox);
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_PaletteBox);
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_TSAPointerBox);
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_PalettePointerBox);
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_PaletteLabel);
            this.SmallMap_GroupBox.Location = new System.Drawing.Point(12, 181);
            this.SmallMap_GroupBox.Name = "SmallMap_GroupBox";
            this.SmallMap_GroupBox.Size = new System.Drawing.Size(240, 137);
            this.SmallMap_GroupBox.TabIndex = 14;
            this.SmallMap_GroupBox.TabStop = false;
            this.SmallMap_GroupBox.Text = "Small Map (240x160)";
            // 
            // SmallMap_TSA_Button
            // 
            this.SmallMap_TSA_Button.Location = new System.Drawing.Point(106, 95);
            this.SmallMap_TSA_Button.Name = "SmallMap_TSA_Button";
            this.SmallMap_TSA_Button.Size = new System.Drawing.Size(128, 36);
            this.SmallMap_TSA_Button.TabIndex = 12;
            this.SmallMap_TSA_Button.Text = "Open TSA Editor...";
            this.Help_ToolTip.SetToolTip(this.SmallMap_TSA_Button, "Click on this button to open a TSA Editor for the small world map image.");
            this.SmallMap_TSA_Button.UseVisualStyleBackColor = true;
            this.SmallMap_TSA_Button.Click += new System.EventHandler(this.SmallMap_TSA_Button_Click);
            // 
            // SmallMap_InsertButton
            // 
            this.SmallMap_InsertButton.Location = new System.Drawing.Point(106, 57);
            this.SmallMap_InsertButton.Name = "SmallMap_InsertButton";
            this.SmallMap_InsertButton.Size = new System.Drawing.Size(128, 35);
            this.SmallMap_InsertButton.TabIndex = 2;
            this.SmallMap_InsertButton.Text = "Insert from file...";
            this.Help_ToolTip.SetToolTip(this.SmallMap_InsertButton, "Click on this to insert a new image to replace the ingame world map (small versio" +
        "n).\r\nWill perform several writes to ROM when clicked.");
            this.SmallMap_InsertButton.UseVisualStyleBackColor = true;
            this.SmallMap_InsertButton.Click += new System.EventHandler(this.SmallMap_InsertButton_Click);
            // 
            // SmallMap_TilesetLabel
            // 
            this.SmallMap_TilesetLabel.AutoSize = true;
            this.SmallMap_TilesetLabel.Location = new System.Drawing.Point(20, 56);
            this.SmallMap_TilesetLabel.Name = "SmallMap_TilesetLabel";
            this.SmallMap_TilesetLabel.Size = new System.Drawing.Size(55, 13);
            this.SmallMap_TilesetLabel.TabIndex = 7;
            this.SmallMap_TilesetLabel.Text = "Graphics :";
            // 
            // SmallMap_TSALabel
            // 
            this.SmallMap_TSALabel.AutoSize = true;
            this.SmallMap_TSALabel.Location = new System.Drawing.Point(20, 95);
            this.SmallMap_TSALabel.Name = "SmallMap_TSALabel";
            this.SmallMap_TSALabel.Size = new System.Drawing.Size(34, 13);
            this.SmallMap_TSALabel.TabIndex = 11;
            this.SmallMap_TSALabel.Text = "TSA :";
            // 
            // SmallMap_TilesetPointerBox
            // 
            this.SmallMap_TilesetPointerBox.Hexadecimal = true;
            this.SmallMap_TilesetPointerBox.Location = new System.Drawing.Point(20, 72);
            this.SmallMap_TilesetPointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.SmallMap_TilesetPointerBox.Name = "SmallMap_TilesetPointerBox";
            this.SmallMap_TilesetPointerBox.Size = new System.Drawing.Size(70, 20);
            this.SmallMap_TilesetPointerBox.TabIndex = 6;
            this.Help_ToolTip.SetToolTip(this.SmallMap_TilesetPointerBox, "Pointer to the pixel data for the small world map image.\r\nWill write to ROM if ch" +
        "anged. Is repointed when inserting a new image.");
            this.SmallMap_TilesetPointerBox.ValueChanged += new System.EventHandler(this.SmallMap_TilesetPointerBox_ValueChanged);
            // 
            // SmallMap_PaletteBox
            // 
            this.SmallMap_PaletteBox.ColorsPerLine = 16;
            this.SmallMap_PaletteBox.Location = new System.Drawing.Point(106, 19);
            this.SmallMap_PaletteBox.Name = "SmallMap_PaletteBox";
            this.SmallMap_PaletteBox.Size = new System.Drawing.Size(128, 32);
            this.SmallMap_PaletteBox.TabIndex = 4;
            this.SmallMap_PaletteBox.TabStop = false;
            this.SmallMap_PaletteBox.Text = "paletteBox2";
            this.Help_ToolTip.SetToolTip(this.SmallMap_PaletteBox, "The palette used for the world map image (small version).\r\nClick on this to open " +
        "a PaletteEditor, to modify this palette.");
            this.SmallMap_PaletteBox.Click += new System.EventHandler(this.SmallMap_PaletteBox_Click);
            // 
            // SmallMap_TSAPointerBox
            // 
            this.SmallMap_TSAPointerBox.Hexadecimal = true;
            this.SmallMap_TSAPointerBox.Location = new System.Drawing.Point(20, 111);
            this.SmallMap_TSAPointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.SmallMap_TSAPointerBox.Name = "SmallMap_TSAPointerBox";
            this.SmallMap_TSAPointerBox.Size = new System.Drawing.Size(70, 20);
            this.SmallMap_TSAPointerBox.TabIndex = 10;
            this.Help_ToolTip.SetToolTip(this.SmallMap_TSAPointerBox, "Pointer to the TSA array data for this world map image.\r\nWill write to ROM if cha" +
        "nged. Is repointed when inserting a new image.");
            this.SmallMap_TSAPointerBox.ValueChanged += new System.EventHandler(this.SmallMap_TSAPointerBox_ValueChanged);
            // 
            // SmallMap_PalettePointerBox
            // 
            this.SmallMap_PalettePointerBox.Hexadecimal = true;
            this.SmallMap_PalettePointerBox.Location = new System.Drawing.Point(20, 31);
            this.SmallMap_PalettePointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.SmallMap_PalettePointerBox.Name = "SmallMap_PalettePointerBox";
            this.SmallMap_PalettePointerBox.Size = new System.Drawing.Size(70, 20);
            this.SmallMap_PalettePointerBox.TabIndex = 8;
            this.Help_ToolTip.SetToolTip(this.SmallMap_PalettePointerBox, "Pointer to the palette data for the small world map image.\r\nWill write to ROM if " +
        "changed. Is repointed when inserting a new image.");
            this.SmallMap_PalettePointerBox.ValueChanged += new System.EventHandler(this.SmallMap_PalettePointerBox_ValueChanged);
            // 
            // SmallMap_PaletteLabel
            // 
            this.SmallMap_PaletteLabel.AutoSize = true;
            this.SmallMap_PaletteLabel.Location = new System.Drawing.Point(20, 15);
            this.SmallMap_PaletteLabel.Name = "SmallMap_PaletteLabel";
            this.SmallMap_PaletteLabel.Size = new System.Drawing.Size(46, 13);
            this.SmallMap_PaletteLabel.TabIndex = 9;
            this.SmallMap_PaletteLabel.Text = "Palette :";
            // 
            // Mini_Map_ImageBox
            // 
            this.Mini_Map_ImageBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Mini_Map_ImageBox.BackColor = System.Drawing.SystemColors.Control;
            this.Mini_Map_ImageBox.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.Mini_Map_ImageBox.Location = new System.Drawing.Point(262, 369);
            this.Mini_Map_ImageBox.Name = "Mini_Map_ImageBox";
            this.Mini_Map_ImageBox.Size = new System.Drawing.Size(64, 64);
            this.Mini_Map_ImageBox.TabIndex = 16;
            this.Mini_Map_ImageBox.TabStop = false;
            this.Mini_Map_ImageBox.Text = "imageBox1";
            // 
            // LargeMap_ImageBox
            // 
            this.LargeMap_ImageBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LargeMap_ImageBox.BackColor = System.Drawing.SystemColors.Control;
            this.LargeMap_ImageBox.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.LargeMap_ImageBox.Location = new System.Drawing.Point(262, 15);
            this.LargeMap_ImageBox.Name = "LargeMap_ImageBox";
            this.LargeMap_ImageBox.Size = new System.Drawing.Size(480, 320);
            this.LargeMap_ImageBox.TabIndex = 1;
            this.LargeMap_ImageBox.TabStop = false;
            this.LargeMap_ImageBox.Text = "imageBox1";
            // 
            // SmallMap_ImageBox
            // 
            this.SmallMap_ImageBox.BackColor = System.Drawing.SystemColors.Control;
            this.SmallMap_ImageBox.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.SmallMap_ImageBox.Location = new System.Drawing.Point(12, 15);
            this.SmallMap_ImageBox.Name = "SmallMap_ImageBox";
            this.SmallMap_ImageBox.Size = new System.Drawing.Size(240, 160);
            this.SmallMap_ImageBox.TabIndex = 2;
            this.SmallMap_ImageBox.TabStop = false;
            this.SmallMap_ImageBox.Text = "imageBox2";
            // 
            // Mini_Map_NumberBox
            // 
            this.Mini_Map_NumberBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Mini_Map_NumberBox.Enabled = false;
            this.Mini_Map_NumberBox.Location = new System.Drawing.Point(273, 343);
            this.Mini_Map_NumberBox.Name = "Mini_Map_NumberBox";
            this.Mini_Map_NumberBox.Size = new System.Drawing.Size(42, 20);
            this.Mini_Map_NumberBox.TabIndex = 17;
            this.Mini_Map_NumberBox.ValueChanged += new System.EventHandler(this.Mini_Map_NumberBox_ValueChanged);
            // 
            // WorldMapEditor_FE8
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 442);
            this.Controls.Add(this.Mini_Map_NumberBox);
            this.Controls.Add(this.Mini_Map_GroupBox);
            this.Controls.Add(this.Mini_Map_ImageBox);
            this.Controls.Add(this.LargeMap_ImageBox);
            this.Controls.Add(this.LargeMap_GroupBox);
            this.Controls.Add(this.SmallMap_GroupBox);
            this.Controls.Add(this.SmallMap_ImageBox);
            this.MinimumSize = new System.Drawing.Size(770, 480);
            this.Name = "WorldMapEditor_FE8";
            this.Text = "World Map Editor";
            this.Mini_Map_GroupBox.ResumeLayout(false);
            this.Mini_Map_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Mini_Map_TilesetPointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Mini_Map_PalettePointerBox)).EndInit();
            this.LargeMap_GroupBox.ResumeLayout(false);
            this.LargeMap_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_TilesetPointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_TSAPointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_PalettePointerBox)).EndInit();
            this.SmallMap_GroupBox.ResumeLayout(false);
            this.SmallMap_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SmallMap_TilesetPointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmallMap_TSAPointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmallMap_PalettePointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Mini_Map_NumberBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Magic.Components.ImageBox LargeMap_ImageBox;
        private Magic.Components.ImageBox SmallMap_ImageBox;
        private System.Windows.Forms.GroupBox LargeMap_GroupBox;
        private System.Windows.Forms.Button LargeMap_TSA_Button;
        private System.Windows.Forms.Label LargeMap_TilesetLabel;
        private System.Windows.Forms.Button LargeMap_InsertButton;
        private System.Windows.Forms.Label LargeMap_TSALabel;
        private Magic.Components.PaletteBox LargeMap_PaletteBox;
        private Magic.Components.PointerBox LargeMap_TilesetPointerBox;
        private Magic.Components.PointerBox LargeMap_TSAPointerBox;
        private System.Windows.Forms.Label LargeMap_PaletteLabel;
        private Magic.Components.PointerBox LargeMap_PalettePointerBox;
        private System.Windows.Forms.GroupBox SmallMap_GroupBox;
        private System.Windows.Forms.Button SmallMap_TSA_Button;
        private System.Windows.Forms.Button SmallMap_InsertButton;
        private System.Windows.Forms.Label SmallMap_TilesetLabel;
        private System.Windows.Forms.Label SmallMap_TSALabel;
        private Magic.Components.PointerBox SmallMap_TilesetPointerBox;
        private Magic.Components.PaletteBox SmallMap_PaletteBox;
        private Magic.Components.PointerBox SmallMap_TSAPointerBox;
        private Magic.Components.PointerBox SmallMap_PalettePointerBox;
        private System.Windows.Forms.Label SmallMap_PaletteLabel;
        private Magic.Components.ImageBox Mini_Map_ImageBox;
        private System.Windows.Forms.GroupBox Mini_Map_GroupBox;
        private System.Windows.Forms.Button Mini_Map_InsertButton;
        private System.Windows.Forms.Label Mini_Map_GraphicsLabel;
        private Magic.Components.PointerBox Mini_Map_TilesetPointerBox;
        private Magic.Components.PaletteBox Mini_Map_PaletteBox;
        private Magic.Components.PointerBox Mini_Map_PalettePointerBox;
        private System.Windows.Forms.Label Mini_Map_PaletteLabel;
        private System.Windows.Forms.NumericUpDown Mini_Map_NumberBox;
    }
}