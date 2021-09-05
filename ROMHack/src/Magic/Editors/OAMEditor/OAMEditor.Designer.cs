namespace Magic.Editors
{
    partial class OAMEditor
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
            this.Shape_ComboBox = new System.Windows.Forms.ComboBox();
            this.Size_ComboBox = new System.Windows.Forms.ComboBox();
            this.Size_Label = new System.Windows.Forms.Label();
            this.Shape_Label = new System.Windows.Forms.Label();
            this.GFX_Mode_Label = new System.Windows.Forms.Label();
            this.OBJ_Mode_Label = new System.Windows.Forms.Label();
            this.OBJ_Mode_ComboBox = new System.Windows.Forms.ComboBox();
            this.GFX_Mode_ComboBox = new System.Windows.Forms.ComboBox();
            this.FullColors_CheckBox = new System.Windows.Forms.CheckBox();
            this.DrawMosaic_CheckBox = new System.Windows.Forms.CheckBox();
            this.FlipV_CheckBox = new System.Windows.Forms.CheckBox();
            this.FlipH_CheckBox = new System.Windows.Forms.CheckBox();
            this.ShapeSize_Label = new System.Windows.Forms.Label();
            this.ShapeSize_Info = new System.Windows.Forms.Label();
            this.ScreenX_NumBox = new System.Windows.Forms.NumericUpDown();
            this.ScreenY_NumBox = new System.Windows.Forms.NumericUpDown();
            this.ScreenX_Label = new System.Windows.Forms.Label();
            this.ScreenY_Label = new System.Windows.Forms.Label();
            this.SheetY_NumBox = new System.Windows.Forms.NumericUpDown();
            this.SheetX_NumBox = new System.Windows.Forms.NumericUpDown();
            this.SheetX_Label = new System.Windows.Forms.Label();
            this.SheetY_Label = new System.Windows.Forms.Label();
            this.Palette_NumBox = new System.Windows.Forms.NumericUpDown();
            this.Priority_NumBox = new System.Windows.Forms.NumericUpDown();
            this.Priority_Label = new System.Windows.Forms.Label();
            this.Palette_Label = new System.Windows.Forms.Label();
            this.OAM_ImageBox = new Magic.Components.ImageBox();
            this.Entry_NumBox = new Magic.Components.ByteBox();
            this.Tileset_ImageBox = new Magic.Components.ImageBox();
            this.SpriteSize_GroupBox = new System.Windows.Forms.GroupBox();
            this.RenderBlit_GroupBox = new System.Windows.Forms.GroupBox();
            this.ScreenBlit_GroupBox = new System.Windows.Forms.GroupBox();
            this.TileSheet_GroupBox = new System.Windows.Forms.GroupBox();
            this.MenuBar = new System.Windows.Forms.MenuStrip();
            this.Status = new System.Windows.Forms.ToolStripLabel();
            this.Affine_GroupBox = new System.Windows.Forms.GroupBox();
            this.Affine_Index_NumBox = new Magic.Components.ByteBox();
            this.Affine_Index_Label = new System.Windows.Forms.Label();
            this.Affine_Ux_Label = new System.Windows.Forms.Label();
            this.Affine_Uy_Label = new System.Windows.Forms.Label();
            this.Affine_Ux_NumBox = new System.Windows.Forms.NumericUpDown();
            this.Affine_Uy_NumBox = new System.Windows.Forms.NumericUpDown();
            this.Affine_Vx_Label = new System.Windows.Forms.Label();
            this.Affine_Vy_Label = new System.Windows.Forms.Label();
            this.Affine_Vx_NumBox = new System.Windows.Forms.NumericUpDown();
            this.Affine_Vy_NumBox = new System.Windows.Forms.NumericUpDown();
            this.Entry_DeleteButton = new System.Windows.Forms.Button();
            this.Entry_CreateButton = new System.Windows.Forms.Button();
            this.OAM_Entry_Label = new System.Windows.Forms.Label();
            this.OAM_Amount_Label = new System.Windows.Forms.Label();
            this.Affine_Amount_Label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenX_NumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenY_NumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SheetY_NumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SheetX_NumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Palette_NumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Priority_NumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Entry_NumBox)).BeginInit();
            this.SpriteSize_GroupBox.SuspendLayout();
            this.RenderBlit_GroupBox.SuspendLayout();
            this.ScreenBlit_GroupBox.SuspendLayout();
            this.TileSheet_GroupBox.SuspendLayout();
            this.MenuBar.SuspendLayout();
            this.Affine_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Affine_Index_NumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Affine_Ux_NumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Affine_Uy_NumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Affine_Vx_NumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Affine_Vy_NumBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Shape_ComboBox
            // 
            this.Shape_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Shape_ComboBox.FormattingEnabled = true;
            this.Shape_ComboBox.Location = new System.Drawing.Point(58, 17);
            this.Shape_ComboBox.Name = "Shape_ComboBox";
            this.Shape_ComboBox.Size = new System.Drawing.Size(103, 21);
            this.Shape_ComboBox.TabIndex = 0;
            this.Shape_ComboBox.SelectedIndexChanged += new System.EventHandler(this.Shape_ComboBox_SelectedIndexChanged);
            // 
            // Size_ComboBox
            // 
            this.Size_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Size_ComboBox.FormattingEnabled = true;
            this.Size_ComboBox.Location = new System.Drawing.Point(58, 44);
            this.Size_ComboBox.Name = "Size_ComboBox";
            this.Size_ComboBox.Size = new System.Drawing.Size(103, 21);
            this.Size_ComboBox.TabIndex = 1;
            this.Size_ComboBox.SelectedIndexChanged += new System.EventHandler(this.Size_ComboBox_SelectedIndexChanged);
            // 
            // Size_Label
            // 
            this.Size_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Size_Label.AutoSize = true;
            this.Size_Label.Location = new System.Drawing.Point(19, 47);
            this.Size_Label.Name = "Size_Label";
            this.Size_Label.Size = new System.Drawing.Size(33, 13);
            this.Size_Label.TabIndex = 2;
            this.Size_Label.Text = "Size :";
            // 
            // Shape_Label
            // 
            this.Shape_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Shape_Label.AutoSize = true;
            this.Shape_Label.Location = new System.Drawing.Point(8, 21);
            this.Shape_Label.Name = "Shape_Label";
            this.Shape_Label.Size = new System.Drawing.Size(44, 13);
            this.Shape_Label.TabIndex = 3;
            this.Shape_Label.Text = "Shape :";
            // 
            // GFX_Mode_Label
            // 
            this.GFX_Mode_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GFX_Mode_Label.AutoSize = true;
            this.GFX_Mode_Label.Location = new System.Drawing.Point(6, 22);
            this.GFX_Mode_Label.Name = "GFX_Mode_Label";
            this.GFX_Mode_Label.Size = new System.Drawing.Size(64, 13);
            this.GFX_Mode_Label.TabIndex = 7;
            this.GFX_Mode_Label.Text = "GFX Mode :";
            // 
            // OBJ_Mode_Label
            // 
            this.OBJ_Mode_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OBJ_Mode_Label.AutoSize = true;
            this.OBJ_Mode_Label.Location = new System.Drawing.Point(7, 49);
            this.OBJ_Mode_Label.Name = "OBJ_Mode_Label";
            this.OBJ_Mode_Label.Size = new System.Drawing.Size(63, 13);
            this.OBJ_Mode_Label.TabIndex = 6;
            this.OBJ_Mode_Label.Text = "OBJ Mode :";
            // 
            // OBJ_Mode_ComboBox
            // 
            this.OBJ_Mode_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OBJ_Mode_ComboBox.FormattingEnabled = true;
            this.OBJ_Mode_ComboBox.Location = new System.Drawing.Point(75, 46);
            this.OBJ_Mode_ComboBox.Name = "OBJ_Mode_ComboBox";
            this.OBJ_Mode_ComboBox.Size = new System.Drawing.Size(86, 21);
            this.OBJ_Mode_ComboBox.TabIndex = 5;
            this.OBJ_Mode_ComboBox.SelectedIndexChanged += new System.EventHandler(this.OBJ_Mode_ComboBox_SelectedIndexChanged);
            // 
            // GFX_Mode_ComboBox
            // 
            this.GFX_Mode_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GFX_Mode_ComboBox.FormattingEnabled = true;
            this.GFX_Mode_ComboBox.Location = new System.Drawing.Point(75, 19);
            this.GFX_Mode_ComboBox.Name = "GFX_Mode_ComboBox";
            this.GFX_Mode_ComboBox.Size = new System.Drawing.Size(86, 21);
            this.GFX_Mode_ComboBox.TabIndex = 4;
            this.GFX_Mode_ComboBox.SelectedIndexChanged += new System.EventHandler(this.GFX_Mode_ComboBox_SelectedIndexChanged);
            // 
            // FullColors_CheckBox
            // 
            this.FullColors_CheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FullColors_CheckBox.AutoSize = true;
            this.FullColors_CheckBox.Location = new System.Drawing.Point(6, 114);
            this.FullColors_CheckBox.Name = "FullColors_CheckBox";
            this.FullColors_CheckBox.Size = new System.Drawing.Size(156, 17);
            this.FullColors_CheckBox.TabIndex = 8;
            this.FullColors_CheckBox.Text = "Full 256-Color Palette Mode";
            this.FullColors_CheckBox.UseVisualStyleBackColor = true;
            this.FullColors_CheckBox.CheckedChanged += new System.EventHandler(this.FullColors_CheckBox_CheckedChanged);
            // 
            // DrawMosaic_CheckBox
            // 
            this.DrawMosaic_CheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DrawMosaic_CheckBox.AutoSize = true;
            this.DrawMosaic_CheckBox.Location = new System.Drawing.Point(6, 137);
            this.DrawMosaic_CheckBox.Name = "DrawMosaic_CheckBox";
            this.DrawMosaic_CheckBox.Size = new System.Drawing.Size(141, 17);
            this.DrawMosaic_CheckBox.TabIndex = 9;
            this.DrawMosaic_CheckBox.Text = "Draw Pixel Mosaic Lines";
            this.DrawMosaic_CheckBox.UseVisualStyleBackColor = true;
            this.DrawMosaic_CheckBox.CheckedChanged += new System.EventHandler(this.DrawMosaic_CheckBox_CheckedChanged);
            // 
            // FlipV_CheckBox
            // 
            this.FlipV_CheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FlipV_CheckBox.AutoSize = true;
            this.FlipV_CheckBox.Location = new System.Drawing.Point(68, 95);
            this.FlipV_CheckBox.Name = "FlipV_CheckBox";
            this.FlipV_CheckBox.Size = new System.Drawing.Size(80, 17);
            this.FlipV_CheckBox.TabIndex = 10;
            this.FlipV_CheckBox.Text = "Flip Vertical";
            this.FlipV_CheckBox.UseVisualStyleBackColor = true;
            this.FlipV_CheckBox.CheckedChanged += new System.EventHandler(this.FlipV_CheckBox_CheckedChanged);
            // 
            // FlipH_CheckBox
            // 
            this.FlipH_CheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FlipH_CheckBox.AutoSize = true;
            this.FlipH_CheckBox.Location = new System.Drawing.Point(69, 73);
            this.FlipH_CheckBox.Name = "FlipH_CheckBox";
            this.FlipH_CheckBox.Size = new System.Drawing.Size(92, 17);
            this.FlipH_CheckBox.TabIndex = 11;
            this.FlipH_CheckBox.Text = "Flip Horizontal";
            this.FlipH_CheckBox.UseVisualStyleBackColor = true;
            this.FlipH_CheckBox.CheckedChanged += new System.EventHandler(this.FlipH_CheckBox_CheckedChanged);
            // 
            // ShapeSize_Label
            // 
            this.ShapeSize_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ShapeSize_Label.AutoSize = true;
            this.ShapeSize_Label.Location = new System.Drawing.Point(148, 21);
            this.ShapeSize_Label.Name = "ShapeSize_Label";
            this.ShapeSize_Label.Size = new System.Drawing.Size(33, 13);
            this.ShapeSize_Label.TabIndex = 12;
            this.ShapeSize_Label.Text = "Size :";
            // 
            // ShapeSize_Info
            // 
            this.ShapeSize_Info.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ShapeSize_Info.AutoSize = true;
            this.ShapeSize_Info.Location = new System.Drawing.Point(187, 21);
            this.ShapeSize_Info.Name = "ShapeSize_Info";
            this.ShapeSize_Info.Size = new System.Drawing.Size(10, 13);
            this.ShapeSize_Info.TabIndex = 13;
            this.ShapeSize_Info.Text = "-";
            // 
            // ScreenX_NumBox
            // 
            this.ScreenX_NumBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ScreenX_NumBox.Location = new System.Drawing.Point(76, 19);
            this.ScreenX_NumBox.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.ScreenX_NumBox.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.ScreenX_NumBox.Name = "ScreenX_NumBox";
            this.ScreenX_NumBox.Size = new System.Drawing.Size(56, 20);
            this.ScreenX_NumBox.TabIndex = 14;
            this.ScreenX_NumBox.ValueChanged += new System.EventHandler(this.ScreenX_NumBox_ValueChanged);
            // 
            // ScreenY_NumBox
            // 
            this.ScreenY_NumBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ScreenY_NumBox.Location = new System.Drawing.Point(76, 45);
            this.ScreenY_NumBox.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.ScreenY_NumBox.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.ScreenY_NumBox.Name = "ScreenY_NumBox";
            this.ScreenY_NumBox.Size = new System.Drawing.Size(56, 20);
            this.ScreenY_NumBox.TabIndex = 15;
            this.ScreenY_NumBox.ValueChanged += new System.EventHandler(this.ScreenY_NumBox_ValueChanged);
            // 
            // ScreenX_Label
            // 
            this.ScreenX_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ScreenX_Label.AutoSize = true;
            this.ScreenX_Label.Location = new System.Drawing.Point(13, 21);
            this.ScreenX_Label.Name = "ScreenX_Label";
            this.ScreenX_Label.Size = new System.Drawing.Size(57, 13);
            this.ScreenX_Label.TabIndex = 16;
            this.ScreenX_Label.Text = "Screen X :";
            // 
            // ScreenY_Label
            // 
            this.ScreenY_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ScreenY_Label.AutoSize = true;
            this.ScreenY_Label.Location = new System.Drawing.Point(13, 47);
            this.ScreenY_Label.Name = "ScreenY_Label";
            this.ScreenY_Label.Size = new System.Drawing.Size(57, 13);
            this.ScreenY_Label.TabIndex = 17;
            this.ScreenY_Label.Text = "Screen Y :";
            // 
            // SheetY_NumBox
            // 
            this.SheetY_NumBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SheetY_NumBox.Location = new System.Drawing.Point(88, 38);
            this.SheetY_NumBox.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.SheetY_NumBox.Name = "SheetY_NumBox";
            this.SheetY_NumBox.Size = new System.Drawing.Size(56, 20);
            this.SheetY_NumBox.TabIndex = 18;
            this.SheetY_NumBox.ValueChanged += new System.EventHandler(this.SheetY_NumBox_ValueChanged);
            // 
            // SheetX_NumBox
            // 
            this.SheetX_NumBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SheetX_NumBox.Location = new System.Drawing.Point(88, 12);
            this.SheetX_NumBox.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.SheetX_NumBox.Name = "SheetX_NumBox";
            this.SheetX_NumBox.Size = new System.Drawing.Size(56, 20);
            this.SheetX_NumBox.TabIndex = 19;
            this.SheetX_NumBox.ValueChanged += new System.EventHandler(this.SheetX_NumBox_ValueChanged);
            // 
            // SheetX_Label
            // 
            this.SheetX_Label.AutoSize = true;
            this.SheetX_Label.Location = new System.Drawing.Point(11, 19);
            this.SheetX_Label.Name = "SheetX_Label";
            this.SheetX_Label.Size = new System.Drawing.Size(71, 13);
            this.SheetX_Label.TabIndex = 20;
            this.SheetX_Label.Text = "Tile Sheet X :";
            // 
            // SheetY_Label
            // 
            this.SheetY_Label.AutoSize = true;
            this.SheetY_Label.Location = new System.Drawing.Point(11, 40);
            this.SheetY_Label.Name = "SheetY_Label";
            this.SheetY_Label.Size = new System.Drawing.Size(71, 13);
            this.SheetY_Label.TabIndex = 21;
            this.SheetY_Label.Text = "Tile Sheet Y :";
            // 
            // Palette_NumBox
            // 
            this.Palette_NumBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Palette_NumBox.Hexadecimal = true;
            this.Palette_NumBox.Location = new System.Drawing.Point(12, 88);
            this.Palette_NumBox.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.Palette_NumBox.Name = "Palette_NumBox";
            this.Palette_NumBox.Size = new System.Drawing.Size(40, 20);
            this.Palette_NumBox.TabIndex = 22;
            this.Palette_NumBox.ValueChanged += new System.EventHandler(this.Palette_NumBox_ValueChanged);
            // 
            // Priority_NumBox
            // 
            this.Priority_NumBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Priority_NumBox.Location = new System.Drawing.Point(190, 45);
            this.Priority_NumBox.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.Priority_NumBox.Name = "Priority_NumBox";
            this.Priority_NumBox.Size = new System.Drawing.Size(42, 20);
            this.Priority_NumBox.TabIndex = 23;
            this.Priority_NumBox.ValueChanged += new System.EventHandler(this.Priority_NumBox_ValueChanged);
            // 
            // Priority_Label
            // 
            this.Priority_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Priority_Label.AutoSize = true;
            this.Priority_Label.Location = new System.Drawing.Point(148, 47);
            this.Priority_Label.Name = "Priority_Label";
            this.Priority_Label.Size = new System.Drawing.Size(39, 13);
            this.Priority_Label.TabIndex = 24;
            this.Priority_Label.Text = "Layer :";
            // 
            // Palette_Label
            // 
            this.Palette_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Palette_Label.AutoSize = true;
            this.Palette_Label.Location = new System.Drawing.Point(9, 74);
            this.Palette_Label.Name = "Palette_Label";
            this.Palette_Label.Size = new System.Drawing.Size(46, 13);
            this.Palette_Label.TabIndex = 25;
            this.Palette_Label.Text = "Palette :";
            // 
            // OAM_ImageBox
            // 
            this.OAM_ImageBox.BackColor = System.Drawing.SystemColors.Control;
            this.OAM_ImageBox.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.OAM_ImageBox.Location = new System.Drawing.Point(12, 56);
            this.OAM_ImageBox.Name = "OAM_ImageBox";
            this.OAM_ImageBox.Size = new System.Drawing.Size(240, 160);
            this.OAM_ImageBox.TabIndex = 26;
            this.OAM_ImageBox.TabStop = false;
            this.OAM_ImageBox.Text = "OAM_ImageBox";
            // 
            // Entry_NumBox
            // 
            this.Entry_NumBox.Hexadecimal = true;
            this.Entry_NumBox.Location = new System.Drawing.Point(86, 29);
            this.Entry_NumBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Entry_NumBox.Name = "Entry_NumBox";
            this.Entry_NumBox.Size = new System.Drawing.Size(58, 20);
            this.Entry_NumBox.TabIndex = 27;
            this.Entry_NumBox.Value = ((byte)(0));
            this.Entry_NumBox.ValueChanged += new System.EventHandler(this.Entry_NumBox_ValueChanged);
            // 
            // Tileset_ImageBox
            // 
            this.Tileset_ImageBox.BackColor = System.Drawing.SystemColors.Control;
            this.Tileset_ImageBox.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.Tileset_ImageBox.Location = new System.Drawing.Point(168, 302);
            this.Tileset_ImageBox.Name = "Tileset_ImageBox";
            this.Tileset_ImageBox.Size = new System.Drawing.Size(256, 64);
            this.Tileset_ImageBox.TabIndex = 29;
            this.Tileset_ImageBox.TabStop = false;
            this.Tileset_ImageBox.Text = "imageBox2";
            // 
            // SpriteSize_GroupBox
            // 
            this.SpriteSize_GroupBox.Controls.Add(this.Shape_ComboBox);
            this.SpriteSize_GroupBox.Controls.Add(this.Size_ComboBox);
            this.SpriteSize_GroupBox.Controls.Add(this.Size_Label);
            this.SpriteSize_GroupBox.Controls.Add(this.Shape_Label);
            this.SpriteSize_GroupBox.Location = new System.Drawing.Point(258, 222);
            this.SpriteSize_GroupBox.Name = "SpriteSize_GroupBox";
            this.SpriteSize_GroupBox.Size = new System.Drawing.Size(167, 74);
            this.SpriteSize_GroupBox.TabIndex = 32;
            this.SpriteSize_GroupBox.TabStop = false;
            this.SpriteSize_GroupBox.Text = "Sprite Size";
            // 
            // RenderBlit_GroupBox
            // 
            this.RenderBlit_GroupBox.Controls.Add(this.GFX_Mode_ComboBox);
            this.RenderBlit_GroupBox.Controls.Add(this.FlipV_CheckBox);
            this.RenderBlit_GroupBox.Controls.Add(this.OBJ_Mode_ComboBox);
            this.RenderBlit_GroupBox.Controls.Add(this.FlipH_CheckBox);
            this.RenderBlit_GroupBox.Controls.Add(this.Palette_Label);
            this.RenderBlit_GroupBox.Controls.Add(this.Palette_NumBox);
            this.RenderBlit_GroupBox.Controls.Add(this.OBJ_Mode_Label);
            this.RenderBlit_GroupBox.Controls.Add(this.GFX_Mode_Label);
            this.RenderBlit_GroupBox.Controls.Add(this.FullColors_CheckBox);
            this.RenderBlit_GroupBox.Controls.Add(this.DrawMosaic_CheckBox);
            this.RenderBlit_GroupBox.Location = new System.Drawing.Point(259, 56);
            this.RenderBlit_GroupBox.Name = "RenderBlit_GroupBox";
            this.RenderBlit_GroupBox.Size = new System.Drawing.Size(167, 160);
            this.RenderBlit_GroupBox.TabIndex = 33;
            this.RenderBlit_GroupBox.TabStop = false;
            this.RenderBlit_GroupBox.Text = "Rendering Options";
            // 
            // ScreenBlit_GroupBox
            // 
            this.ScreenBlit_GroupBox.Controls.Add(this.ScreenY_NumBox);
            this.ScreenBlit_GroupBox.Controls.Add(this.ScreenX_NumBox);
            this.ScreenBlit_GroupBox.Controls.Add(this.ScreenX_Label);
            this.ScreenBlit_GroupBox.Controls.Add(this.ScreenY_Label);
            this.ScreenBlit_GroupBox.Controls.Add(this.ShapeSize_Label);
            this.ScreenBlit_GroupBox.Controls.Add(this.ShapeSize_Info);
            this.ScreenBlit_GroupBox.Controls.Add(this.Priority_NumBox);
            this.ScreenBlit_GroupBox.Controls.Add(this.Priority_Label);
            this.ScreenBlit_GroupBox.Location = new System.Drawing.Point(12, 222);
            this.ScreenBlit_GroupBox.Name = "ScreenBlit_GroupBox";
            this.ScreenBlit_GroupBox.Size = new System.Drawing.Size(240, 74);
            this.ScreenBlit_GroupBox.TabIndex = 34;
            this.ScreenBlit_GroupBox.TabStop = false;
            this.ScreenBlit_GroupBox.Text = "Sprite Screen Positioning";
            // 
            // TileSheet_GroupBox
            // 
            this.TileSheet_GroupBox.Controls.Add(this.SheetY_NumBox);
            this.TileSheet_GroupBox.Controls.Add(this.SheetX_NumBox);
            this.TileSheet_GroupBox.Controls.Add(this.SheetX_Label);
            this.TileSheet_GroupBox.Controls.Add(this.SheetY_Label);
            this.TileSheet_GroupBox.Location = new System.Drawing.Point(12, 302);
            this.TileSheet_GroupBox.Name = "TileSheet_GroupBox";
            this.TileSheet_GroupBox.Size = new System.Drawing.Size(150, 64);
            this.TileSheet_GroupBox.TabIndex = 35;
            this.TileSheet_GroupBox.TabStop = false;
            this.TileSheet_GroupBox.Text = "Sprite Tiles";
            // 
            // MenuBar
            // 
            this.MenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Status});
            this.MenuBar.Location = new System.Drawing.Point(0, 0);
            this.MenuBar.Name = "MenuBar";
            this.MenuBar.Size = new System.Drawing.Size(438, 24);
            this.MenuBar.TabIndex = 36;
            this.MenuBar.Text = "menuStrip1";
            // 
            // Status
            // 
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(222, 17);
            this.Status.Text = "Entry Type 0x00 [Entry Name] - ADDRESS";
            // 
            // Affine_GroupBox
            // 
            this.Affine_GroupBox.Controls.Add(this.Affine_Amount_Label);
            this.Affine_GroupBox.Controls.Add(this.Affine_Index_NumBox);
            this.Affine_GroupBox.Controls.Add(this.Affine_Index_Label);
            this.Affine_GroupBox.Controls.Add(this.Affine_Ux_Label);
            this.Affine_GroupBox.Controls.Add(this.Affine_Uy_Label);
            this.Affine_GroupBox.Controls.Add(this.Affine_Ux_NumBox);
            this.Affine_GroupBox.Controls.Add(this.Affine_Uy_NumBox);
            this.Affine_GroupBox.Controls.Add(this.Affine_Vx_Label);
            this.Affine_GroupBox.Controls.Add(this.Affine_Vy_Label);
            this.Affine_GroupBox.Controls.Add(this.Affine_Vx_NumBox);
            this.Affine_GroupBox.Controls.Add(this.Affine_Vy_NumBox);
            this.Affine_GroupBox.Location = new System.Drawing.Point(12, 372);
            this.Affine_GroupBox.Name = "Affine_GroupBox";
            this.Affine_GroupBox.Size = new System.Drawing.Size(414, 64);
            this.Affine_GroupBox.TabIndex = 37;
            this.Affine_GroupBox.TabStop = false;
            this.Affine_GroupBox.Text = "Affine Sprite Transform";
            // 
            // Affine_Index_NumBox
            // 
            this.Affine_Index_NumBox.Hexadecimal = true;
            this.Affine_Index_NumBox.Location = new System.Drawing.Point(131, 17);
            this.Affine_Index_NumBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Affine_Index_NumBox.Name = "Affine_Index_NumBox";
            this.Affine_Index_NumBox.Size = new System.Drawing.Size(40, 20);
            this.Affine_Index_NumBox.TabIndex = 39;
            this.Affine_Index_NumBox.Value = ((byte)(0));
            this.Affine_Index_NumBox.ValueChanged += new System.EventHandler(this.Affine_Index_NumBox_ValueChanged);
            // 
            // Affine_Index_Label
            // 
            this.Affine_Index_Label.AutoSize = true;
            this.Affine_Index_Label.Location = new System.Drawing.Point(10, 19);
            this.Affine_Index_Label.Name = "Affine_Index_Label";
            this.Affine_Index_Label.Size = new System.Drawing.Size(115, 13);
            this.Affine_Index_Label.TabIndex = 38;
            this.Affine_Index_Label.Text = "Transform Data Index :";
            // 
            // Affine_Ux_Label
            // 
            this.Affine_Ux_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Affine_Ux_Label.AutoSize = true;
            this.Affine_Ux_Label.Location = new System.Drawing.Point(177, 19);
            this.Affine_Ux_Label.Name = "Affine_Ux_Label";
            this.Affine_Ux_Label.Size = new System.Drawing.Size(29, 13);
            this.Affine_Ux_Label.TabIndex = 29;
            this.Affine_Ux_Label.Text = "U x :";
            // 
            // Affine_Uy_Label
            // 
            this.Affine_Uy_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Affine_Uy_Label.AutoSize = true;
            this.Affine_Uy_Label.Location = new System.Drawing.Point(177, 40);
            this.Affine_Uy_Label.Name = "Affine_Uy_Label";
            this.Affine_Uy_Label.Size = new System.Drawing.Size(29, 13);
            this.Affine_Uy_Label.TabIndex = 30;
            this.Affine_Uy_Label.Text = "U y :";
            // 
            // Affine_Ux_NumBox
            // 
            this.Affine_Ux_NumBox.DecimalPlaces = 6;
            this.Affine_Ux_NumBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.Affine_Ux_NumBox.Location = new System.Drawing.Point(209, 12);
            this.Affine_Ux_NumBox.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.Affine_Ux_NumBox.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            -2147483648});
            this.Affine_Ux_NumBox.Name = "Affine_Ux_NumBox";
            this.Affine_Ux_NumBox.Size = new System.Drawing.Size(75, 20);
            this.Affine_Ux_NumBox.TabIndex = 28;
            this.Affine_Ux_NumBox.ValueChanged += new System.EventHandler(this.Affine_Ux_NumBox_ValueChanged);
            // 
            // Affine_Uy_NumBox
            // 
            this.Affine_Uy_NumBox.DecimalPlaces = 6;
            this.Affine_Uy_NumBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.Affine_Uy_NumBox.Location = new System.Drawing.Point(209, 38);
            this.Affine_Uy_NumBox.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.Affine_Uy_NumBox.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            -2147483648});
            this.Affine_Uy_NumBox.Name = "Affine_Uy_NumBox";
            this.Affine_Uy_NumBox.Size = new System.Drawing.Size(75, 20);
            this.Affine_Uy_NumBox.TabIndex = 27;
            this.Affine_Uy_NumBox.ValueChanged += new System.EventHandler(this.Affine_Uy_NumBox_ValueChanged);
            // 
            // Affine_Vx_Label
            // 
            this.Affine_Vx_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Affine_Vx_Label.AutoSize = true;
            this.Affine_Vx_Label.Location = new System.Drawing.Point(299, 19);
            this.Affine_Vx_Label.Name = "Affine_Vx_Label";
            this.Affine_Vx_Label.Size = new System.Drawing.Size(28, 13);
            this.Affine_Vx_Label.TabIndex = 25;
            this.Affine_Vx_Label.Text = "V x :";
            // 
            // Affine_Vy_Label
            // 
            this.Affine_Vy_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Affine_Vy_Label.AutoSize = true;
            this.Affine_Vy_Label.Location = new System.Drawing.Point(299, 40);
            this.Affine_Vy_Label.Name = "Affine_Vy_Label";
            this.Affine_Vy_Label.Size = new System.Drawing.Size(28, 13);
            this.Affine_Vy_Label.TabIndex = 26;
            this.Affine_Vy_Label.Text = "V y :";
            // 
            // Affine_Vx_NumBox
            // 
            this.Affine_Vx_NumBox.DecimalPlaces = 6;
            this.Affine_Vx_NumBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.Affine_Vx_NumBox.Location = new System.Drawing.Point(330, 12);
            this.Affine_Vx_NumBox.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.Affine_Vx_NumBox.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            -2147483648});
            this.Affine_Vx_NumBox.Name = "Affine_Vx_NumBox";
            this.Affine_Vx_NumBox.Size = new System.Drawing.Size(75, 20);
            this.Affine_Vx_NumBox.TabIndex = 1;
            this.Affine_Vx_NumBox.ValueChanged += new System.EventHandler(this.Affine_Vx_NumBox_ValueChanged);
            // 
            // Affine_Vy_NumBox
            // 
            this.Affine_Vy_NumBox.DecimalPlaces = 6;
            this.Affine_Vy_NumBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.Affine_Vy_NumBox.Location = new System.Drawing.Point(330, 38);
            this.Affine_Vy_NumBox.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.Affine_Vy_NumBox.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            -2147483648});
            this.Affine_Vy_NumBox.Name = "Affine_Vy_NumBox";
            this.Affine_Vy_NumBox.Size = new System.Drawing.Size(75, 20);
            this.Affine_Vy_NumBox.TabIndex = 0;
            this.Affine_Vy_NumBox.ValueChanged += new System.EventHandler(this.Affine_Vy_NumBox_ValueChanged);
            // 
            // Entry_DeleteButton
            // 
            this.Entry_DeleteButton.Location = new System.Drawing.Point(150, 27);
            this.Entry_DeleteButton.Name = "Entry_DeleteButton";
            this.Entry_DeleteButton.Size = new System.Drawing.Size(58, 23);
            this.Entry_DeleteButton.TabIndex = 38;
            this.Entry_DeleteButton.Text = "Delete";
            this.Entry_DeleteButton.UseVisualStyleBackColor = true;
            this.Entry_DeleteButton.Click += new System.EventHandler(this.Entry_DeleteButton_Click);
            // 
            // Entry_CreateButton
            // 
            this.Entry_CreateButton.Location = new System.Drawing.Point(214, 27);
            this.Entry_CreateButton.Name = "Entry_CreateButton";
            this.Entry_CreateButton.Size = new System.Drawing.Size(58, 23);
            this.Entry_CreateButton.TabIndex = 39;
            this.Entry_CreateButton.Text = "Create";
            this.Entry_CreateButton.UseVisualStyleBackColor = true;
            this.Entry_CreateButton.Click += new System.EventHandler(this.Entry_CreateButton_Click);
            // 
            // OAM_Entry_Label
            // 
            this.OAM_Entry_Label.AutoSize = true;
            this.OAM_Entry_Label.Location = new System.Drawing.Point(9, 32);
            this.OAM_Entry_Label.Name = "OAM_Entry_Label";
            this.OAM_Entry_Label.Size = new System.Drawing.Size(71, 13);
            this.OAM_Entry_Label.TabIndex = 40;
            this.OAM_Entry_Label.Text = "OAM Object :";
            // 
            // OAM_Amount_Label
            // 
            this.OAM_Amount_Label.AutoSize = true;
            this.OAM_Amount_Label.Location = new System.Drawing.Point(278, 32);
            this.OAM_Amount_Label.Name = "OAM_Amount_Label";
            this.OAM_Amount_Label.Size = new System.Drawing.Size(106, 13);
            this.OAM_Amount_Label.TabIndex = 41;
            this.OAM_Amount_Label.Text = "Amount of Objects : -";
            // 
            // Affine_Amount_Label
            // 
            this.Affine_Amount_Label.AutoSize = true;
            this.Affine_Amount_Label.Location = new System.Drawing.Point(10, 42);
            this.Affine_Amount_Label.Name = "Affine_Amount_Label";
            this.Affine_Amount_Label.Size = new System.Drawing.Size(105, 13);
            this.Affine_Amount_Label.TabIndex = 40;
            this.Affine_Amount_Label.Text = "Transform Amount : -";
            // 
            // OAMEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 448);
            this.Controls.Add(this.OAM_Amount_Label);
            this.Controls.Add(this.OAM_Entry_Label);
            this.Controls.Add(this.Entry_CreateButton);
            this.Controls.Add(this.Entry_DeleteButton);
            this.Controls.Add(this.Affine_GroupBox);
            this.Controls.Add(this.TileSheet_GroupBox);
            this.Controls.Add(this.ScreenBlit_GroupBox);
            this.Controls.Add(this.RenderBlit_GroupBox);
            this.Controls.Add(this.SpriteSize_GroupBox);
            this.Controls.Add(this.Tileset_ImageBox);
            this.Controls.Add(this.Entry_NumBox);
            this.Controls.Add(this.OAM_ImageBox);
            this.Controls.Add(this.MenuBar);
            this.MainMenuStrip = this.MenuBar;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(454, 486);
            this.Name = "OAMEditor";
            this.Text = "OAM Editor";
            ((System.ComponentModel.ISupportInitialize)(this.ScreenX_NumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenY_NumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SheetY_NumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SheetX_NumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Palette_NumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Priority_NumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Entry_NumBox)).EndInit();
            this.SpriteSize_GroupBox.ResumeLayout(false);
            this.SpriteSize_GroupBox.PerformLayout();
            this.RenderBlit_GroupBox.ResumeLayout(false);
            this.RenderBlit_GroupBox.PerformLayout();
            this.ScreenBlit_GroupBox.ResumeLayout(false);
            this.ScreenBlit_GroupBox.PerformLayout();
            this.TileSheet_GroupBox.ResumeLayout(false);
            this.TileSheet_GroupBox.PerformLayout();
            this.MenuBar.ResumeLayout(false);
            this.MenuBar.PerformLayout();
            this.Affine_GroupBox.ResumeLayout(false);
            this.Affine_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Affine_Index_NumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Affine_Ux_NumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Affine_Uy_NumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Affine_Vx_NumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Affine_Vy_NumBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox Shape_ComboBox;
        private System.Windows.Forms.ComboBox Size_ComboBox;
        private System.Windows.Forms.Label Size_Label;
        private System.Windows.Forms.Label Shape_Label;
        private System.Windows.Forms.Label GFX_Mode_Label;
        private System.Windows.Forms.Label OBJ_Mode_Label;
        private System.Windows.Forms.ComboBox OBJ_Mode_ComboBox;
        private System.Windows.Forms.ComboBox GFX_Mode_ComboBox;
        private System.Windows.Forms.CheckBox FullColors_CheckBox;
        private System.Windows.Forms.CheckBox DrawMosaic_CheckBox;
        private System.Windows.Forms.CheckBox FlipV_CheckBox;
        private System.Windows.Forms.CheckBox FlipH_CheckBox;
        private System.Windows.Forms.Label ShapeSize_Label;
        private System.Windows.Forms.Label ShapeSize_Info;
        private System.Windows.Forms.NumericUpDown ScreenX_NumBox;
        private System.Windows.Forms.NumericUpDown ScreenY_NumBox;
        private System.Windows.Forms.Label ScreenX_Label;
        private System.Windows.Forms.Label ScreenY_Label;
        private System.Windows.Forms.NumericUpDown SheetY_NumBox;
        private System.Windows.Forms.NumericUpDown SheetX_NumBox;
        private System.Windows.Forms.Label SheetX_Label;
        private System.Windows.Forms.Label SheetY_Label;
        private System.Windows.Forms.NumericUpDown Palette_NumBox;
        private System.Windows.Forms.NumericUpDown Priority_NumBox;
        private System.Windows.Forms.Label Priority_Label;
        private System.Windows.Forms.Label Palette_Label;
        private Components.ImageBox OAM_ImageBox;
        private Components.ByteBox Entry_NumBox;
        private Components.ImageBox Tileset_ImageBox;
        private System.Windows.Forms.GroupBox SpriteSize_GroupBox;
        private System.Windows.Forms.GroupBox RenderBlit_GroupBox;
        private System.Windows.Forms.GroupBox ScreenBlit_GroupBox;
        private System.Windows.Forms.GroupBox TileSheet_GroupBox;
        private System.Windows.Forms.MenuStrip MenuBar;
        private System.Windows.Forms.ToolStripLabel Status;
        private System.Windows.Forms.GroupBox Affine_GroupBox;
        private System.Windows.Forms.NumericUpDown Affine_Vy_NumBox;
        private System.Windows.Forms.Label Affine_Vx_Label;
        private System.Windows.Forms.Label Affine_Vy_Label;
        private System.Windows.Forms.NumericUpDown Affine_Vx_NumBox;
        private System.Windows.Forms.Label Affine_Ux_Label;
        private System.Windows.Forms.Label Affine_Uy_Label;
        private System.Windows.Forms.NumericUpDown Affine_Ux_NumBox;
        private System.Windows.Forms.NumericUpDown Affine_Uy_NumBox;
        private System.Windows.Forms.Label Affine_Index_Label;
        private Components.ByteBox Affine_Index_NumBox;
        private System.Windows.Forms.Button Entry_DeleteButton;
        private System.Windows.Forms.Button Entry_CreateButton;
        private System.Windows.Forms.Label OAM_Entry_Label;
        private System.Windows.Forms.Label OAM_Amount_Label;
        private System.Windows.Forms.Label Affine_Amount_Label;
    }
}