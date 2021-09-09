namespace EmblemMagic.Editors
{
    partial class MapEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapEditor));
            this.Tileset_GridBox = new Magic.Components.GridBox();
            this.EntryArrayBox = new Magic.Components.ByteArrayBox();
            this.Palette_ArrayBox = new Magic.Components.ByteArrayBox();
            this.Palette_Label = new System.Windows.Forms.Label();
            this.Palette_PointerBox = new Magic.Components.PointerBox();
            this.Tileset1_PointerBox = new Magic.Components.PointerBox();
            this.Tilesets_Label = new System.Windows.Forms.Label();
            this.Tileset1_ArrayBox = new Magic.Components.ByteArrayBox();
            this.Tileset2_PointerBox = new Magic.Components.PointerBox();
            this.Tileset2_ArrayBox = new Magic.Components.ByteArrayBox();
            this.TilesetTSA_ArrayBox = new Magic.Components.ByteArrayBox();
            this.TilesetTSA_PointerBox = new Magic.Components.PointerBox();
            this.TileTSA_Label = new System.Windows.Forms.Label();
            this.Palette_PaletteBox = new Magic.Components.PaletteBox();
            this.Map_GridBox = new Magic.Components.MapBox();
            this.MapData_PointerBox = new Magic.Components.PointerBox();
            this.MapData_ArrayBox = new Magic.Components.ByteArrayBox();
            this.Map_Panel = new System.Windows.Forms.Panel();
            this.Chapter_Label = new System.Windows.Forms.Label();
            this.Map_Size_Label = new System.Windows.Forms.Label();
            this.TileAnim1_ArrayBox = new Magic.Components.ByteArrayBox();
            this.TileAnim2_ArrayBox = new Magic.Components.ByteArrayBox();
            this.TileAnim_Label = new System.Windows.Forms.Label();
            this.TileAnim1_PointerBox = new Magic.Components.PointerBox();
            this.TileAnim2_PointerBox = new Magic.Components.PointerBox();
            this.Changes_PointerBox = new Magic.Components.PointerBox();
            this.Changes_Label = new System.Windows.Forms.Label();
            this.Changes_ArrayBox = new Magic.Components.ByteArrayBox();
            this.Map_Height_NumBox = new System.Windows.Forms.NumericUpDown();
            this.Map_Width_NumBox = new System.Windows.Forms.NumericUpDown();
            this.Map_X_Label = new System.Windows.Forms.Label();
            this.Tool_Tile_Button = new System.Windows.Forms.RadioButton();
            this.Tool_Fill_Button = new System.Windows.Forms.RadioButton();
            this.Map_GroupBox = new System.Windows.Forms.GroupBox();
            this.Chapter_MagicButton = new Magic.Components.MagicButton(App);
            this.Changes_Total_NumBox = new Magic.Components.ByteBox();
            this.Changes_Total_Label = new System.Windows.Forms.Label();
            this.Changes_CheckBox = new System.Windows.Forms.CheckBox();
            this.Changes_NumBox = new Magic.Components.ByteBox();
            this.Array_GroupBox = new System.Windows.Forms.GroupBox();
            this.MapTileset_MagicButton = new Magic.Components.MagicButton(App);
            this.Tool_Pick_Button = new System.Windows.Forms.RadioButton();
            this.Tool_Erase_Button = new System.Windows.Forms.RadioButton();
            this.Editor_Menu = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Insert = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tool = new System.Windows.Forms.ToolStripMenuItem();
            this.Tool_OpenPaletteEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_View = new System.Windows.Forms.ToolStripMenuItem();
            this.View_AltPalette = new System.Windows.Forms.ToolStripMenuItem();
            this.Clear_Button = new System.Windows.Forms.Button();
            this.Map_MouseTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.Palette_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tileset1_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tileset2_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TilesetTSA_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapData_PointerBox)).BeginInit();
            this.Map_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TileAnim1_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TileAnim2_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Changes_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Map_Height_NumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Map_Width_NumBox)).BeginInit();
            this.Map_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Changes_Total_NumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Changes_NumBox)).BeginInit();
            this.Array_GroupBox.SuspendLayout();
            this.Editor_Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tileset_GridBox
            // 
            this.Tileset_GridBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Tileset_GridBox.Location = new System.Drawing.Point(277, 6);
            this.Tileset_GridBox.Name = "Tileset_GridBox";
            this.Tileset_GridBox.Selection = null;
            this.Tileset_GridBox.ShowGrid = true;
            this.Tileset_GridBox.Size = new System.Drawing.Size(512, 512);
            this.Tileset_GridBox.TabIndex = 0;
            this.Tileset_GridBox.TabStop = false;
            this.Tileset_GridBox.Text = "gridBox1";
            this.Tileset_GridBox.TileSize = 16;
            // 
            // EntryArrayBox
            // 
            this.EntryArrayBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EntryArrayBox.Location = new System.Drawing.Point(58, 27);
            this.EntryArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.EntryArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.EntryArrayBox.Size = new System.Drawing.Size(213, 26);
            this.EntryArrayBox.TabIndex = 1;
            this.Help_ToolTip.SetToolTip(this.EntryArrayBox, "Select the chapter whose map should be edited.");
            this.EntryArrayBox.ValueChanged += new System.EventHandler(this.EntryArrayBox_ValueChanged);
            // 
            // Palette_ArrayBox
            // 
            this.Palette_ArrayBox.Location = new System.Drawing.Point(9, 32);
            this.Palette_ArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.Palette_ArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.Palette_ArrayBox.Size = new System.Drawing.Size(216, 26);
            this.Palette_ArrayBox.TabIndex = 2;
            this.Help_ToolTip.SetToolTip(this.Palette_ArrayBox, "Which entry in the map array stores the palette data for this chapter\'s map tiles" +
        "et. Entry name should start with [PAL].\r\nWill write to ROM if changed.");
            this.Palette_ArrayBox.ValueChanged += new System.EventHandler(this.Palette_ArrayBox_ValueChanged);
            // 
            // Palette_Label
            // 
            this.Palette_Label.AutoSize = true;
            this.Palette_Label.Location = new System.Drawing.Point(6, 16);
            this.Palette_Label.Name = "Palette_Label";
            this.Palette_Label.Size = new System.Drawing.Size(85, 13);
            this.Palette_Label.TabIndex = 3;
            this.Palette_Label.Text = "Tileset Palettes :";
            // 
            // Palette_PointerBox
            // 
            this.Palette_PointerBox.Hexadecimal = true;
            this.Palette_PointerBox.Location = new System.Drawing.Point(231, 34);
            this.Palette_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Palette_PointerBox.Name = "Palette_PointerBox";
            this.Palette_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Palette_PointerBox.TabIndex = 4;
            this.Help_ToolTip.SetToolTip(this.Palette_PointerBox, resources.GetString("Palette_PointerBox.ToolTip"));
            this.Palette_PointerBox.ValueChanged += new System.EventHandler(this.Palette_PointerBox_ValueChanged);
            // 
            // Tileset1_PointerBox
            // 
            this.Tileset1_PointerBox.Hexadecimal = true;
            this.Tileset1_PointerBox.Location = new System.Drawing.Point(231, 80);
            this.Tileset1_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Tileset1_PointerBox.Name = "Tileset1_PointerBox";
            this.Tileset1_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Tileset1_PointerBox.TabIndex = 7;
            this.Help_ToolTip.SetToolTip(this.Tileset1_PointerBox, resources.GetString("Tileset1_PointerBox.ToolTip"));
            this.Tileset1_PointerBox.ValueChanged += new System.EventHandler(this.Tileset1_PointerBox_ValueChanged);
            // 
            // Tilesets_Label
            // 
            this.Tilesets_Label.AutoSize = true;
            this.Tilesets_Label.Location = new System.Drawing.Point(6, 61);
            this.Tilesets_Label.Name = "Tilesets_Label";
            this.Tilesets_Label.Size = new System.Drawing.Size(89, 13);
            this.Tilesets_Label.TabIndex = 6;
            this.Tilesets_Label.Text = "Tileset Graphics :";
            // 
            // Tileset1_ArrayBox
            // 
            this.Tileset1_ArrayBox.Location = new System.Drawing.Point(9, 77);
            this.Tileset1_ArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.Tileset1_ArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.Tileset1_ArrayBox.Size = new System.Drawing.Size(216, 26);
            this.Tileset1_ArrayBox.TabIndex = 5;
            this.Help_ToolTip.SetToolTip(this.Tileset1_ArrayBox, "Which entry in the map array stores the 1st set of pixel data for this chapter\'s " +
        "map tileset. Entry name should start with [IMG].\r\nWill write to ROM if changed.\r" +
        "\n");
            this.Tileset1_ArrayBox.ValueChanged += new System.EventHandler(this.Tileset1_ArrayBox_ValueChanged);
            // 
            // Tileset2_PointerBox
            // 
            this.Tileset2_PointerBox.Hexadecimal = true;
            this.Tileset2_PointerBox.Location = new System.Drawing.Point(231, 106);
            this.Tileset2_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Tileset2_PointerBox.Name = "Tileset2_PointerBox";
            this.Tileset2_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Tileset2_PointerBox.TabIndex = 7;
            this.Help_ToolTip.SetToolTip(this.Tileset2_PointerBox, resources.GetString("Tileset2_PointerBox.ToolTip"));
            this.Tileset2_PointerBox.ValueChanged += new System.EventHandler(this.Tileset2_PointerBox_ValueChanged);
            // 
            // Tileset2_ArrayBox
            // 
            this.Tileset2_ArrayBox.Location = new System.Drawing.Point(9, 104);
            this.Tileset2_ArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.Tileset2_ArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.Tileset2_ArrayBox.Size = new System.Drawing.Size(216, 26);
            this.Tileset2_ArrayBox.TabIndex = 6;
            this.Help_ToolTip.SetToolTip(this.Tileset2_ArrayBox, "Which entry in the map array stores the 2nd set of pixel data for this chapter\'s " +
        "map tileset. Entry name should start with [IMG].\r\nWill write to ROM if changed.\r" +
        "\n");
            this.Tileset2_ArrayBox.ValueChanged += new System.EventHandler(this.Tileset2_ArrayBox_ValueChanged);
            // 
            // TilesetTSA_ArrayBox
            // 
            this.TilesetTSA_ArrayBox.Location = new System.Drawing.Point(307, 32);
            this.TilesetTSA_ArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.TilesetTSA_ArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.TilesetTSA_ArrayBox.Size = new System.Drawing.Size(216, 26);
            this.TilesetTSA_ArrayBox.TabIndex = 8;
            this.Help_ToolTip.SetToolTip(this.TilesetTSA_ArrayBox, "Which entry in the map array stores the TSA data for this chapter\'s map tileset. " +
        "Entry name should start with [TSA].\r\nWill write to ROM if changed.\r\n");
            this.TilesetTSA_ArrayBox.ValueChanged += new System.EventHandler(this.TilesetTSA_ArrayBox_ValueChanged);
            // 
            // TilesetTSA_PointerBox
            // 
            this.TilesetTSA_PointerBox.Hexadecimal = true;
            this.TilesetTSA_PointerBox.Location = new System.Drawing.Point(531, 34);
            this.TilesetTSA_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.TilesetTSA_PointerBox.Name = "TilesetTSA_PointerBox";
            this.TilesetTSA_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.TilesetTSA_PointerBox.TabIndex = 10;
            this.Help_ToolTip.SetToolTip(this.TilesetTSA_PointerBox, resources.GetString("TilesetTSA_PointerBox.ToolTip"));
            this.TilesetTSA_PointerBox.ValueChanged += new System.EventHandler(this.TilesetTSA_PointerBox_ValueChanged);
            // 
            // TileTSA_Label
            // 
            this.TileTSA_Label.AutoSize = true;
            this.TileTSA_Label.Location = new System.Drawing.Point(309, 16);
            this.TileTSA_Label.Name = "TileTSA_Label";
            this.TileTSA_Label.Size = new System.Drawing.Size(125, 13);
            this.TileTSA_Label.TabIndex = 9;
            this.TileTSA_Label.Text = "Tileset TSA and Terrain :";
            // 
            // Palette_PaletteBox
            // 
            this.Palette_PaletteBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Palette_PaletteBox.ColorsPerLine = 16;
            this.Palette_PaletteBox.Location = new System.Drawing.Point(15, 524);
            this.Palette_PaletteBox.Name = "Palette_PaletteBox";
            this.Palette_PaletteBox.Size = new System.Drawing.Size(160, 100);
            this.Palette_PaletteBox.TabIndex = 11;
            this.Palette_PaletteBox.TabStop = false;
            this.Palette_PaletteBox.Text = "paletteBox1";
            this.Help_ToolTip.SetToolTip(this.Palette_PaletteBox, "The palette for this map (or rather, for the map tileset that this map uses).\r\nCl" +
        "ick on this to open a PaletteEditor, to modify this palette.");
            this.Palette_PaletteBox.Click += new System.EventHandler(this.Tool_OpenPaletteEditor_Click);
            // 
            // Map_GridBox
            // 
            this.Map_GridBox.Hover = null;
            this.Map_GridBox.Hovered = new System.Drawing.Point(0, 0);
            this.Map_GridBox.Location = new System.Drawing.Point(3, 3);
            this.Map_GridBox.Name = "Map_GridBox";
            this.Map_GridBox.ShowGrid = false;
            this.Map_GridBox.Size = new System.Drawing.Size(240, 160);
            this.Map_GridBox.TabIndex = 12;
            this.Map_GridBox.TabStop = false;
            this.Map_GridBox.Text = "gridBox2";
            this.Map_GridBox.TileSize = 16;
            this.Map_GridBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Map_GridBox_MouseDown);
            this.Map_GridBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Map_GridBox_MouseMove);
            this.Map_GridBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Map_GridBox_MouseUp);
            // 
            // MapData_PointerBox
            // 
            this.MapData_PointerBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MapData_PointerBox.Hexadecimal = true;
            this.MapData_PointerBox.Location = new System.Drawing.Point(183, 18);
            this.MapData_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.MapData_PointerBox.Name = "MapData_PointerBox";
            this.MapData_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.MapData_PointerBox.TabIndex = 15;
            this.Help_ToolTip.SetToolTip(this.MapData_PointerBox, resources.GetString("MapData_PointerBox.ToolTip"));
            this.MapData_PointerBox.ValueChanged += new System.EventHandler(this.MapData_PointerBox_ValueChanged);
            // 
            // MapData_ArrayBox
            // 
            this.MapData_ArrayBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MapData_ArrayBox.Location = new System.Drawing.Point(6, 16);
            this.MapData_ArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.MapData_ArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.MapData_ArrayBox.Size = new System.Drawing.Size(171, 26);
            this.MapData_ArrayBox.TabIndex = 13;
            this.Help_ToolTip.SetToolTip(this.MapData_ArrayBox, "Which entry of the map array holds map data for this chapter. The entry name shou" +
        "ld start with [MAP].\r\nWill write to ROM if changed.");
            this.MapData_ArrayBox.ValueChanged += new System.EventHandler(this.MapData_ArrayBox_ValueChanged);
            // 
            // Map_Panel
            // 
            this.Map_Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Map_Panel.AutoScroll = true;
            this.Map_Panel.Controls.Add(this.Map_GridBox);
            this.Map_Panel.Location = new System.Drawing.Point(12, 82);
            this.Map_Panel.Name = "Map_Panel";
            this.Map_Panel.Size = new System.Drawing.Size(259, 294);
            this.Map_Panel.TabIndex = 16;
            // 
            // Chapter_Label
            // 
            this.Chapter_Label.AutoSize = true;
            this.Chapter_Label.Location = new System.Drawing.Point(6, 33);
            this.Chapter_Label.Name = "Chapter_Label";
            this.Chapter_Label.Size = new System.Drawing.Size(50, 13);
            this.Chapter_Label.TabIndex = 17;
            this.Chapter_Label.Text = "Chapter :";
            // 
            // Map_Size_Label
            // 
            this.Map_Size_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Map_Size_Label.AutoSize = true;
            this.Map_Size_Label.Location = new System.Drawing.Point(95, 46);
            this.Map_Size_Label.Name = "Map_Size_Label";
            this.Map_Size_Label.Size = new System.Drawing.Size(57, 13);
            this.Map_Size_Label.TabIndex = 18;
            this.Map_Size_Label.Text = "Map Size :";
            // 
            // TileAnim1_ArrayBox
            // 
            this.TileAnim1_ArrayBox.Location = new System.Drawing.Point(307, 77);
            this.TileAnim1_ArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.TileAnim1_ArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.TileAnim1_ArrayBox.Size = new System.Drawing.Size(216, 26);
            this.TileAnim1_ArrayBox.TabIndex = 14;
            this.Help_ToolTip.SetToolTip(this.TileAnim1_ArrayBox, "Which entry in the map array stores the 1st set of animation data for this chapte" +
        "r\'s map tileset. Entry name should start with [ANM].\r\nWill write to ROM if chang" +
        "ed.");
            this.TileAnim1_ArrayBox.ValueChanged += new System.EventHandler(this.TileAnim1_ArrayBox_ValueChanged);
            // 
            // TileAnim2_ArrayBox
            // 
            this.TileAnim2_ArrayBox.Location = new System.Drawing.Point(307, 104);
            this.TileAnim2_ArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.TileAnim2_ArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.TileAnim2_ArrayBox.Size = new System.Drawing.Size(216, 26);
            this.TileAnim2_ArrayBox.TabIndex = 14;
            this.Help_ToolTip.SetToolTip(this.TileAnim2_ArrayBox, "Which entry in the map array stores the map 2nd set of animation data for this ch" +
        "apter\'s map tileset. Entry name should start with [ANM].\r\nWill write to ROM if c" +
        "hanged.");
            this.TileAnim2_ArrayBox.ValueChanged += new System.EventHandler(this.TileAnim2_ArrayBox_ValueChanged);
            // 
            // TileAnim_Label
            // 
            this.TileAnim_Label.AutoSize = true;
            this.TileAnim_Label.Location = new System.Drawing.Point(309, 61);
            this.TileAnim_Label.Name = "TileAnim_Label";
            this.TileAnim_Label.Size = new System.Drawing.Size(98, 13);
            this.TileAnim_Label.TabIndex = 19;
            this.TileAnim_Label.Text = "Tileset Animations :";
            // 
            // TileAnim1_PointerBox
            // 
            this.TileAnim1_PointerBox.Hexadecimal = true;
            this.TileAnim1_PointerBox.Location = new System.Drawing.Point(531, 80);
            this.TileAnim1_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.TileAnim1_PointerBox.Name = "TileAnim1_PointerBox";
            this.TileAnim1_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.TileAnim1_PointerBox.TabIndex = 20;
            this.Help_ToolTip.SetToolTip(this.TileAnim1_PointerBox, resources.GetString("TileAnim1_PointerBox.ToolTip"));
            this.TileAnim1_PointerBox.ValueChanged += new System.EventHandler(this.TileAnim1_PointerBox_ValueChanged);
            // 
            // TileAnim2_PointerBox
            // 
            this.TileAnim2_PointerBox.Hexadecimal = true;
            this.TileAnim2_PointerBox.Location = new System.Drawing.Point(531, 106);
            this.TileAnim2_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.TileAnim2_PointerBox.Name = "TileAnim2_PointerBox";
            this.TileAnim2_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.TileAnim2_PointerBox.TabIndex = 21;
            this.Help_ToolTip.SetToolTip(this.TileAnim2_PointerBox, resources.GetString("TileAnim2_PointerBox.ToolTip"));
            this.TileAnim2_PointerBox.ValueChanged += new System.EventHandler(this.TileAnim2_PointerBox_ValueChanged);
            // 
            // Changes_PointerBox
            // 
            this.Changes_PointerBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Changes_PointerBox.Hexadecimal = true;
            this.Changes_PointerBox.Location = new System.Drawing.Point(182, 81);
            this.Changes_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Changes_PointerBox.Name = "Changes_PointerBox";
            this.Changes_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Changes_PointerBox.TabIndex = 24;
            this.Help_ToolTip.SetToolTip(this.Changes_PointerBox, resources.GetString("Changes_PointerBox.ToolTip"));
            this.Changes_PointerBox.ValueChanged += new System.EventHandler(this.Changes_PointerBox_ValueChanged);
            // 
            // Changes_Label
            // 
            this.Changes_Label.AutoSize = true;
            this.Changes_Label.Location = new System.Drawing.Point(2, 63);
            this.Changes_Label.Name = "Changes_Label";
            this.Changes_Label.Size = new System.Drawing.Size(135, 13);
            this.Changes_Label.TabIndex = 23;
            this.Changes_Label.Text = "Triggerable Map Changes :";
            // 
            // Changes_ArrayBox
            // 
            this.Changes_ArrayBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Changes_ArrayBox.Location = new System.Drawing.Point(6, 79);
            this.Changes_ArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.Changes_ArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.Changes_ArrayBox.Size = new System.Drawing.Size(170, 26);
            this.Changes_ArrayBox.TabIndex = 22;
            this.Help_ToolTip.SetToolTip(this.Changes_ArrayBox, "Which entry of the map array holds the triggerable map tile changes data for this" +
        " chapter. The entry name should start with [CHG].\r\nWill write to ROM if changed." +
        "");
            this.Changes_ArrayBox.ValueChanged += new System.EventHandler(this.Changes_ArrayBox_ValueChanged);
            // 
            // Map_Height_NumBox
            // 
            this.Map_Height_NumBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Map_Height_NumBox.Location = new System.Drawing.Point(212, 44);
            this.Map_Height_NumBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Map_Height_NumBox.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.Map_Height_NumBox.Name = "Map_Height_NumBox";
            this.Map_Height_NumBox.Size = new System.Drawing.Size(40, 20);
            this.Map_Height_NumBox.TabIndex = 25;
            this.Map_Height_NumBox.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.Map_Height_NumBox.ValueChanged += new System.EventHandler(this.Map_HeightNumBox_ValueChanged);
            // 
            // Map_Width_NumBox
            // 
            this.Map_Width_NumBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Map_Width_NumBox.Location = new System.Drawing.Point(158, 44);
            this.Map_Width_NumBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Map_Width_NumBox.Minimum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.Map_Width_NumBox.Name = "Map_Width_NumBox";
            this.Map_Width_NumBox.Size = new System.Drawing.Size(40, 20);
            this.Map_Width_NumBox.TabIndex = 26;
            this.Map_Width_NumBox.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.Map_Width_NumBox.ValueChanged += new System.EventHandler(this.Map_WidthNumBox_ValueChanged);
            // 
            // Map_X_Label
            // 
            this.Map_X_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Map_X_Label.AutoSize = true;
            this.Map_X_Label.Location = new System.Drawing.Point(199, 46);
            this.Map_X_Label.Name = "Map_X_Label";
            this.Map_X_Label.Size = new System.Drawing.Size(12, 13);
            this.Map_X_Label.TabIndex = 27;
            this.Map_X_Label.Text = "x";
            // 
            // Tool_Tile_Button
            // 
            this.Tool_Tile_Button.AutoSize = true;
            this.Tool_Tile_Button.Checked = true;
            this.Tool_Tile_Button.Location = new System.Drawing.Point(12, 59);
            this.Tool_Tile_Button.Name = "Tool_Tile_Button";
            this.Tool_Tile_Button.Size = new System.Drawing.Size(66, 17);
            this.Tool_Tile_Button.TabIndex = 29;
            this.Tool_Tile_Button.TabStop = true;
            this.Tool_Tile_Button.Text = "Tile Tool";
            this.Tool_Tile_Button.UseVisualStyleBackColor = true;
            // 
            // Tool_Fill_Button
            // 
            this.Tool_Fill_Button.AutoSize = true;
            this.Tool_Fill_Button.Location = new System.Drawing.Point(84, 59);
            this.Tool_Fill_Button.Name = "Tool_Fill_Button";
            this.Tool_Fill_Button.Size = new System.Drawing.Size(61, 17);
            this.Tool_Fill_Button.TabIndex = 30;
            this.Tool_Fill_Button.Text = "Fill Tool";
            this.Tool_Fill_Button.UseVisualStyleBackColor = true;
            // 
            // Map_GroupBox
            // 
            this.Map_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Map_GroupBox.Controls.Add(this.Chapter_MagicButton);
            this.Map_GroupBox.Controls.Add(this.Changes_Total_NumBox);
            this.Map_GroupBox.Controls.Add(this.Changes_Total_Label);
            this.Map_GroupBox.Controls.Add(this.Changes_CheckBox);
            this.Map_GroupBox.Controls.Add(this.Changes_NumBox);
            this.Map_GroupBox.Controls.Add(this.MapData_ArrayBox);
            this.Map_GroupBox.Controls.Add(this.MapData_PointerBox);
            this.Map_GroupBox.Controls.Add(this.Map_Height_NumBox);
            this.Map_GroupBox.Controls.Add(this.Changes_Label);
            this.Map_GroupBox.Controls.Add(this.Changes_PointerBox);
            this.Map_GroupBox.Controls.Add(this.Changes_ArrayBox);
            this.Map_GroupBox.Controls.Add(this.Map_Size_Label);
            this.Map_GroupBox.Controls.Add(this.Map_Width_NumBox);
            this.Map_GroupBox.Controls.Add(this.Map_X_Label);
            this.Map_GroupBox.Location = new System.Drawing.Point(12, 382);
            this.Map_GroupBox.Name = "Map_GroupBox";
            this.Map_GroupBox.Size = new System.Drawing.Size(259, 136);
            this.Map_GroupBox.TabIndex = 31;
            this.Map_GroupBox.TabStop = false;
            this.Map_GroupBox.Text = "Edit Chapter Map";
            // 
            // Chapter_MagicButton
            // 
            this.Chapter_MagicButton.Location = new System.Drawing.Point(228, 107);
            this.Chapter_MagicButton.MaximumSize = new System.Drawing.Size(24, 24);
            this.Chapter_MagicButton.MinimumSize = new System.Drawing.Size(24, 24);
            this.Chapter_MagicButton.Name = "Chapter_MagicButton";
            this.Chapter_MagicButton.Size = new System.Drawing.Size(24, 24);
            this.Chapter_MagicButton.TabIndex = 32;
            this.Help_ToolTip.SetToolTip(this.Chapter_MagicButton, "Clicking this button opens a Chapter Editor module for the current selected entry" +
        ".");
            this.Chapter_MagicButton.UseVisualStyleBackColor = true;
            // 
            // Changes_Total_NumBox
            // 
            this.Changes_Total_NumBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Changes_Total_NumBox.Enabled = false;
            this.Changes_Total_NumBox.Hexadecimal = true;
            this.Changes_Total_NumBox.Location = new System.Drawing.Point(183, 109);
            this.Changes_Total_NumBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Changes_Total_NumBox.Name = "Changes_Total_NumBox";
            this.Changes_Total_NumBox.Size = new System.Drawing.Size(40, 20);
            this.Changes_Total_NumBox.TabIndex = 31;
            this.Help_ToolTip.SetToolTip(this.Changes_Total_NumBox, "The length of the array of map-tile-change data.");
            this.Changes_Total_NumBox.Value = ((byte)(0));
            // 
            // Changes_Total_Label
            // 
            this.Changes_Total_Label.AutoSize = true;
            this.Changes_Total_Label.Location = new System.Drawing.Point(147, 111);
            this.Changes_Total_Label.Name = "Changes_Total_Label";
            this.Changes_Total_Label.Size = new System.Drawing.Size(33, 13);
            this.Changes_Total_Label.TabIndex = 30;
            this.Changes_Total_Label.Text = "Max :";
            // 
            // Changes_CheckBox
            // 
            this.Changes_CheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Changes_CheckBox.AutoSize = true;
            this.Changes_CheckBox.Location = new System.Drawing.Point(9, 110);
            this.Changes_CheckBox.Name = "Changes_CheckBox";
            this.Changes_CheckBox.Size = new System.Drawing.Size(90, 17);
            this.Changes_CheckBox.TabIndex = 29;
            this.Changes_CheckBox.Text = "Edit Change :";
            this.Help_ToolTip.SetToolTip(this.Changes_CheckBox, "If checked, then changes to the tile map will alter the currently selected map-ti" +
        "le-changes data.");
            this.Changes_CheckBox.UseVisualStyleBackColor = true;
            this.Changes_CheckBox.CheckedChanged += new System.EventHandler(this.Changes_CheckBox_CheckedChanged);
            // 
            // Changes_NumBox
            // 
            this.Changes_NumBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Changes_NumBox.Enabled = false;
            this.Changes_NumBox.Hexadecimal = true;
            this.Changes_NumBox.Location = new System.Drawing.Point(101, 109);
            this.Changes_NumBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Changes_NumBox.Name = "Changes_NumBox";
            this.Changes_NumBox.Size = new System.Drawing.Size(40, 20);
            this.Changes_NumBox.TabIndex = 28;
            this.Help_ToolTip.SetToolTip(this.Changes_NumBox, "Select the index of the map-tile-change data to alter.");
            this.Changes_NumBox.Value = ((byte)(0));
            this.Changes_NumBox.ValueChanged += new System.EventHandler(this.Changes_NumBox_ValueChanged);
            // 
            // Array_GroupBox
            // 
            this.Array_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Array_GroupBox.Controls.Add(this.MapTileset_MagicButton);
            this.Array_GroupBox.Controls.Add(this.Palette_ArrayBox);
            this.Array_GroupBox.Controls.Add(this.Palette_Label);
            this.Array_GroupBox.Controls.Add(this.Palette_PointerBox);
            this.Array_GroupBox.Controls.Add(this.TileAnim2_PointerBox);
            this.Array_GroupBox.Controls.Add(this.Tileset1_ArrayBox);
            this.Array_GroupBox.Controls.Add(this.TileAnim1_PointerBox);
            this.Array_GroupBox.Controls.Add(this.Tilesets_Label);
            this.Array_GroupBox.Controls.Add(this.TileAnim_Label);
            this.Array_GroupBox.Controls.Add(this.Tileset1_PointerBox);
            this.Array_GroupBox.Controls.Add(this.TileAnim2_ArrayBox);
            this.Array_GroupBox.Controls.Add(this.Tileset2_PointerBox);
            this.Array_GroupBox.Controls.Add(this.TileAnim1_ArrayBox);
            this.Array_GroupBox.Controls.Add(this.Tileset2_ArrayBox);
            this.Array_GroupBox.Controls.Add(this.TilesetTSA_ArrayBox);
            this.Array_GroupBox.Controls.Add(this.TileTSA_Label);
            this.Array_GroupBox.Controls.Add(this.TilesetTSA_PointerBox);
            this.Array_GroupBox.Location = new System.Drawing.Point(182, 524);
            this.Array_GroupBox.Name = "Array_GroupBox";
            this.Array_GroupBox.Size = new System.Drawing.Size(607, 136);
            this.Array_GroupBox.TabIndex = 32;
            this.Array_GroupBox.TabStop = false;
            this.Array_GroupBox.Text = "Edit Map Tileset values for this Chapter";
            // 
            // MapTileset_MagicButton
            // 
            this.MapTileset_MagicButton.Location = new System.Drawing.Point(577, 10);
            this.MapTileset_MagicButton.MaximumSize = new System.Drawing.Size(24, 24);
            this.MapTileset_MagicButton.MinimumSize = new System.Drawing.Size(24, 24);
            this.MapTileset_MagicButton.Name = "MapTileset_MagicButton";
            this.MapTileset_MagicButton.Size = new System.Drawing.Size(24, 24);
            this.MapTileset_MagicButton.TabIndex = 33;
            this.Help_ToolTip.SetToolTip(this.MapTileset_MagicButton, "Clicking this button opens a Map Tileset Editor module for the current selected m" +
        "ap tileset values.");
            this.MapTileset_MagicButton.UseVisualStyleBackColor = true;
            this.MapTileset_MagicButton.Click += new System.EventHandler(this.MapTileset_MagicButton_Click);
            // 
            // Tool_Pick_Button
            // 
            this.Tool_Pick_Button.AutoSize = true;
            this.Tool_Pick_Button.Location = new System.Drawing.Point(206, 59);
            this.Tool_Pick_Button.Name = "Tool_Pick_Button";
            this.Tool_Pick_Button.Size = new System.Drawing.Size(71, 17);
            this.Tool_Pick_Button.TabIndex = 33;
            this.Tool_Pick_Button.TabStop = true;
            this.Tool_Pick_Button.Text = "Tile finder";
            this.Tool_Pick_Button.UseVisualStyleBackColor = true;
            // 
            // Tool_Erase_Button
            // 
            this.Tool_Erase_Button.AutoSize = true;
            this.Tool_Erase_Button.Location = new System.Drawing.Point(151, 59);
            this.Tool_Erase_Button.Name = "Tool_Erase_Button";
            this.Tool_Erase_Button.Size = new System.Drawing.Size(52, 17);
            this.Tool_Erase_Button.TabIndex = 34;
            this.Tool_Erase_Button.TabStop = true;
            this.Tool_Erase_Button.Text = "Erase";
            this.Tool_Erase_Button.UseVisualStyleBackColor = true;
            // 
            // Editor_Menu
            // 
            this.Editor_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File,
            this.Menu_Tool,
            this.Menu_View});
            this.Editor_Menu.Location = new System.Drawing.Point(0, 0);
            this.Editor_Menu.Name = "Editor_Menu";
            this.Editor_Menu.Size = new System.Drawing.Size(804, 24);
            this.Editor_Menu.TabIndex = 36;
            this.Editor_Menu.Text = "menuStrip1";
            // 
            // Menu_File
            // 
            this.Menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File_Insert,
            this.File_Save});
            this.Menu_File.Name = "Menu_File";
            this.Menu_File.Size = new System.Drawing.Size(37, 20);
            this.Menu_File.Text = "File";
            // 
            // File_Insert
            // 
            this.File_Insert.Name = "File_Insert";
            this.File_Insert.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.File_Insert.Size = new System.Drawing.Size(176, 22);
            this.File_Insert.Text = "Insert map...";
            this.File_Insert.Click += new System.EventHandler(this.File_Insert_Click);
            // 
            // File_Save
            // 
            this.File_Save.Name = "File_Save";
            this.File_Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.File_Save.Size = new System.Drawing.Size(176, 22);
            this.File_Save.Text = "Save map...";
            this.File_Save.Click += new System.EventHandler(this.File_Save_Click);
            // 
            // Menu_Tool
            // 
            this.Menu_Tool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Tool_OpenPaletteEditor});
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
            // Menu_View
            // 
            this.Menu_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.View_AltPalette});
            this.Menu_View.Name = "Menu_View";
            this.Menu_View.Size = new System.Drawing.Size(44, 20);
            this.Menu_View.Text = "View";
            // 
            // View_AltPalette
            // 
            this.View_AltPalette.CheckOnClick = true;
            this.View_AltPalette.Name = "View_AltPalette";
            this.View_AltPalette.Size = new System.Drawing.Size(213, 22);
            this.View_AltPalette.Text = "View with alternate palette";
            this.View_AltPalette.Click += new System.EventHandler(this.View_AltPalette_Click);
            // 
            // Clear_Button
            // 
            this.Clear_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Clear_Button.Location = new System.Drawing.Point(15, 630);
            this.Clear_Button.Name = "Clear_Button";
            this.Clear_Button.Size = new System.Drawing.Size(160, 30);
            this.Clear_Button.TabIndex = 37;
            this.Clear_Button.Text = "Clear the entire Map";
            this.Help_ToolTip.SetToolTip(this.Clear_Button, "Sets every tile in the map to index 0.");
            this.Clear_Button.UseVisualStyleBackColor = true;
            this.Clear_Button.Click += new System.EventHandler(this.Clear_Button_Click);
            // 
            // Map_MouseTimer
            // 
            this.Map_MouseTimer.Tick += new System.EventHandler(this.Map_MouseTimer_Tick);
            // 
            // MapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 672);
            this.Controls.Add(this.Clear_Button);
            this.Controls.Add(this.Tool_Erase_Button);
            this.Controls.Add(this.Tool_Pick_Button);
            this.Controls.Add(this.Palette_PaletteBox);
            this.Controls.Add(this.Tool_Fill_Button);
            this.Controls.Add(this.Array_GroupBox);
            this.Controls.Add(this.Map_GroupBox);
            this.Controls.Add(this.Tool_Tile_Button);
            this.Controls.Add(this.Chapter_Label);
            this.Controls.Add(this.Map_Panel);
            this.Controls.Add(this.EntryArrayBox);
            this.Controls.Add(this.Tileset_GridBox);
            this.Controls.Add(this.Editor_Menu);
            this.MainMenuStrip = this.Editor_Menu;
            this.MinimumSize = new System.Drawing.Size(820, 710);
            this.Name = "MapEditor";
            this.Text = "Map Editor";
            ((System.ComponentModel.ISupportInitialize)(this.Palette_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tileset1_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tileset2_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TilesetTSA_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapData_PointerBox)).EndInit();
            this.Map_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TileAnim1_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TileAnim2_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Changes_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Map_Height_NumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Map_Width_NumBox)).EndInit();
            this.Map_GroupBox.ResumeLayout(false);
            this.Map_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Changes_Total_NumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Changes_NumBox)).EndInit();
            this.Array_GroupBox.ResumeLayout(false);
            this.Array_GroupBox.PerformLayout();
            this.Editor_Menu.ResumeLayout(false);
            this.Editor_Menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Chapter_Label;
        private Magic.Components.ByteArrayBox EntryArrayBox;
        private Magic.Components.MapBox Map_GridBox;
        private System.Windows.Forms.Panel Map_Panel;
        private Magic.Components.GridBox Tileset_GridBox;
        private System.Windows.Forms.GroupBox Map_GroupBox;
        private Magic.Components.PointerBox MapData_PointerBox;
        private Magic.Components.ByteArrayBox MapData_ArrayBox;
        private System.Windows.Forms.NumericUpDown Map_Height_NumBox;
        private System.Windows.Forms.Label Map_X_Label;
        private System.Windows.Forms.NumericUpDown Map_Width_NumBox;
        private System.Windows.Forms.Label Changes_Label;
        private Magic.Components.PointerBox Changes_PointerBox;
        private Magic.Components.ByteArrayBox Changes_ArrayBox;
        private Magic.Components.PaletteBox Palette_PaletteBox;
        private System.Windows.Forms.GroupBox Array_GroupBox;
        private System.Windows.Forms.Label Palette_Label;
        private System.Windows.Forms.Label Tilesets_Label;
        private System.Windows.Forms.Label TileTSA_Label;
        private System.Windows.Forms.Label TileAnim_Label;
        private System.Windows.Forms.Label Map_Size_Label;
        private Magic.Components.PointerBox Palette_PointerBox;
        private Magic.Components.PointerBox Tileset1_PointerBox;
        private Magic.Components.PointerBox Tileset2_PointerBox;
        private Magic.Components.PointerBox TilesetTSA_PointerBox;
        private Magic.Components.PointerBox TileAnim1_PointerBox;
        private Magic.Components.PointerBox TileAnim2_PointerBox;
        private Magic.Components.ByteArrayBox Palette_ArrayBox;
        private Magic.Components.ByteArrayBox Tileset1_ArrayBox;
        private Magic.Components.ByteArrayBox Tileset2_ArrayBox;
        private Magic.Components.ByteArrayBox TilesetTSA_ArrayBox;
        private Magic.Components.ByteArrayBox TileAnim1_ArrayBox;
        private Magic.Components.ByteArrayBox TileAnim2_ArrayBox;
        private System.Windows.Forms.RadioButton Tool_Tile_Button;
        private System.Windows.Forms.RadioButton Tool_Fill_Button;
        private System.Windows.Forms.RadioButton Tool_Pick_Button;
        private System.Windows.Forms.CheckBox Changes_CheckBox;
        private Magic.Components.ByteBox Changes_NumBox;
        private Magic.Components.ByteBox Changes_Total_NumBox;
        private System.Windows.Forms.Label Changes_Total_Label;
        private System.Windows.Forms.RadioButton Tool_Erase_Button;
        private System.Windows.Forms.MenuStrip Editor_Menu;
        private System.Windows.Forms.ToolStripMenuItem Menu_File;
        private System.Windows.Forms.ToolStripMenuItem Menu_View;
        private System.Windows.Forms.ToolStripMenuItem View_AltPalette;
        private System.Windows.Forms.Button Clear_Button;
        private System.Windows.Forms.ToolStripMenuItem File_Insert;
        private System.Windows.Forms.ToolStripMenuItem File_Save;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tool;
        private System.Windows.Forms.ToolStripMenuItem Tool_OpenPaletteEditor;
        private System.Windows.Forms.Timer Map_MouseTimer;
        private Magic.Components.MagicButton Chapter_MagicButton;
        private Magic.Components.MagicButton MapTileset_MagicButton;
    }
}