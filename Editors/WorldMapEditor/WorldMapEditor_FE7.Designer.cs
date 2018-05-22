namespace EmblemMagic.Editors
{
    partial class WorldMapEditor_FE7
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
            this.LargeMap_PaletteBox = new EmblemMagic.Components.PaletteBox();
            this.LargeMap_ImageBox = new EmblemMagic.Components.ImageBox();
            this.SmallMap_InsertButton = new System.Windows.Forms.Button();
            this.LargeMap_Panel = new System.Windows.Forms.Panel();
            this.SmallMap_ImageBox = new EmblemMagic.Components.ImageBox();
            this.SmallMap_PaletteBox = new EmblemMagic.Components.PaletteBox();
            this.LargeMap_InsertButton = new System.Windows.Forms.Button();
            this.SmallMap_Tileset_PointerBox = new EmblemMagic.Components.PointerBox();
            this.SmallMap_TilesetLabel = new System.Windows.Forms.Label();
            this.SmallMap_PaletteLabel = new System.Windows.Forms.Label();
            this.SmallMap_Palette_PointerBox = new EmblemMagic.Components.PointerBox();
            this.SmallMap_TSALabel = new System.Windows.Forms.Label();
            this.SmallMap_TSA_PointerBox = new EmblemMagic.Components.PointerBox();
            this.SmallMap_GroupBox = new System.Windows.Forms.GroupBox();
            this.SmallMap_TSA_Button = new System.Windows.Forms.Button();
            this.LargeMap_GroupBox = new System.Windows.Forms.GroupBox();
            this.LargeMap_TSA_NumBox = new System.Windows.Forms.NumericUpDown();
            this.LargeMap_TSA_Button = new System.Windows.Forms.Button();
            this.LargeMap_TilesetLabel = new System.Windows.Forms.Label();
            this.LargeMap_TSALabel = new System.Windows.Forms.Label();
            this.LargeMap_Tileset_PointerBox = new EmblemMagic.Components.PointerBox();
            this.LargeMap_TSA_PointerBox = new EmblemMagic.Components.PointerBox();
            this.LargeMap_PaletteLabel = new System.Windows.Forms.Label();
            this.LargeMap_Palette_PointerBox = new EmblemMagic.Components.PointerBox();
            this.LargeMap_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SmallMap_Tileset_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmallMap_Palette_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmallMap_TSA_PointerBox)).BeginInit();
            this.SmallMap_GroupBox.SuspendLayout();
            this.LargeMap_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_TSA_NumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_Tileset_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_TSA_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_Palette_PointerBox)).BeginInit();
            this.SuspendLayout();
            // 
            // LargeMap_PaletteBox
            // 
            this.LargeMap_PaletteBox.ColorsPerLine = 16;
            this.LargeMap_PaletteBox.Location = new System.Drawing.Point(106, 19);
            this.LargeMap_PaletteBox.Name = "LargeMap_PaletteBox";
            this.LargeMap_PaletteBox.Size = new System.Drawing.Size(128, 32);
            this.LargeMap_PaletteBox.TabIndex = 0;
            this.LargeMap_PaletteBox.TabStop = false;
            this.LargeMap_PaletteBox.Text = "paletteBox1";
            this.LargeMap_PaletteBox.Click += new System.EventHandler(this.LargeMap_PaletteBox_Click);
            // 
            // LargeMap_ImageBox
            // 
            this.LargeMap_ImageBox.BackColor = System.Drawing.SystemColors.Control;
            this.LargeMap_ImageBox.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.LargeMap_ImageBox.Location = new System.Drawing.Point(3, 3);
            this.LargeMap_ImageBox.Name = "LargeMap_ImageBox";
            this.LargeMap_ImageBox.Size = new System.Drawing.Size(1024, 688);
            this.LargeMap_ImageBox.TabIndex = 1;
            this.LargeMap_ImageBox.TabStop = false;
            this.LargeMap_ImageBox.Text = "imageBox1";
            // 
            // SmallMap_InsertButton
            // 
            this.SmallMap_InsertButton.Location = new System.Drawing.Point(106, 57);
            this.SmallMap_InsertButton.Name = "SmallMap_InsertButton";
            this.SmallMap_InsertButton.Size = new System.Drawing.Size(128, 35);
            this.SmallMap_InsertButton.TabIndex = 2;
            this.SmallMap_InsertButton.Text = "Insert from file...";
            this.SmallMap_InsertButton.UseVisualStyleBackColor = true;
            this.SmallMap_InsertButton.Click += new System.EventHandler(this.SmallMap_InsertButton_Click);
            // 
            // LargeMap_Panel
            // 
            this.LargeMap_Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LargeMap_Panel.AutoScroll = true;
            this.LargeMap_Panel.Controls.Add(this.LargeMap_ImageBox);
            this.LargeMap_Panel.Location = new System.Drawing.Point(255, 12);
            this.LargeMap_Panel.Name = "LargeMap_Panel";
            this.LargeMap_Panel.Size = new System.Drawing.Size(487, 448);
            this.LargeMap_Panel.TabIndex = 3;
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
            // SmallMap_PaletteBox
            // 
            this.SmallMap_PaletteBox.ColorsPerLine = 16;
            this.SmallMap_PaletteBox.Location = new System.Drawing.Point(106, 19);
            this.SmallMap_PaletteBox.Name = "SmallMap_PaletteBox";
            this.SmallMap_PaletteBox.Size = new System.Drawing.Size(128, 32);
            this.SmallMap_PaletteBox.TabIndex = 4;
            this.SmallMap_PaletteBox.TabStop = false;
            this.SmallMap_PaletteBox.Text = "paletteBox2";
            this.SmallMap_PaletteBox.Click += new System.EventHandler(this.SmallMap_PaletteBox_Click);
            // 
            // LargeMap_InsertButton
            // 
            this.LargeMap_InsertButton.Location = new System.Drawing.Point(106, 57);
            this.LargeMap_InsertButton.Name = "LargeMap_InsertButton";
            this.LargeMap_InsertButton.Size = new System.Drawing.Size(128, 36);
            this.LargeMap_InsertButton.TabIndex = 5;
            this.LargeMap_InsertButton.Text = "Insert from file...";
            this.LargeMap_InsertButton.UseVisualStyleBackColor = true;
            this.LargeMap_InsertButton.Click += new System.EventHandler(this.LargeMap_InsertButton_Click);
            // 
            // SmallMap_Tileset_PointerBox
            // 
            this.SmallMap_Tileset_PointerBox.Hexadecimal = true;
            this.SmallMap_Tileset_PointerBox.Location = new System.Drawing.Point(20, 72);
            this.SmallMap_Tileset_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.SmallMap_Tileset_PointerBox.Name = "SmallMap_Tileset_PointerBox";
            this.SmallMap_Tileset_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.SmallMap_Tileset_PointerBox.TabIndex = 6;
            this.SmallMap_Tileset_PointerBox.ValueChanged += new System.EventHandler(this.SmallMap_TilesetPointerBox_ValueChanged);
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
            // SmallMap_PaletteLabel
            // 
            this.SmallMap_PaletteLabel.AutoSize = true;
            this.SmallMap_PaletteLabel.Location = new System.Drawing.Point(20, 15);
            this.SmallMap_PaletteLabel.Name = "SmallMap_PaletteLabel";
            this.SmallMap_PaletteLabel.Size = new System.Drawing.Size(46, 13);
            this.SmallMap_PaletteLabel.TabIndex = 9;
            this.SmallMap_PaletteLabel.Text = "Palette :";
            // 
            // SmallMap_Palette_PointerBox
            // 
            this.SmallMap_Palette_PointerBox.Hexadecimal = true;
            this.SmallMap_Palette_PointerBox.Location = new System.Drawing.Point(20, 31);
            this.SmallMap_Palette_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.SmallMap_Palette_PointerBox.Name = "SmallMap_Palette_PointerBox";
            this.SmallMap_Palette_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.SmallMap_Palette_PointerBox.TabIndex = 8;
            this.SmallMap_Palette_PointerBox.ValueChanged += new System.EventHandler(this.SmallMap_PalettePointerBox_ValueChanged);
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
            // SmallMap_TSA_PointerBox
            // 
            this.SmallMap_TSA_PointerBox.Hexadecimal = true;
            this.SmallMap_TSA_PointerBox.Location = new System.Drawing.Point(20, 111);
            this.SmallMap_TSA_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.SmallMap_TSA_PointerBox.Name = "SmallMap_TSA_PointerBox";
            this.SmallMap_TSA_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.SmallMap_TSA_PointerBox.TabIndex = 10;
            this.SmallMap_TSA_PointerBox.ValueChanged += new System.EventHandler(this.SmallMap_TSAPointerBox_ValueChanged);
            // 
            // SmallMap_GroupBox
            // 
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_TSA_Button);
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_InsertButton);
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_TilesetLabel);
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_TSALabel);
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_Tileset_PointerBox);
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_PaletteBox);
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_TSA_PointerBox);
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_Palette_PointerBox);
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_PaletteLabel);
            this.SmallMap_GroupBox.Location = new System.Drawing.Point(12, 181);
            this.SmallMap_GroupBox.Name = "SmallMap_GroupBox";
            this.SmallMap_GroupBox.Size = new System.Drawing.Size(240, 137);
            this.SmallMap_GroupBox.TabIndex = 12;
            this.SmallMap_GroupBox.TabStop = false;
            this.SmallMap_GroupBox.Text = "Small Map (240x160)";
            // 
            // SmallMap_TSA_Button
            // 
            this.SmallMap_TSA_Button.Location = new System.Drawing.Point(106, 98);
            this.SmallMap_TSA_Button.Name = "SmallMap_TSA_Button";
            this.SmallMap_TSA_Button.Size = new System.Drawing.Size(128, 33);
            this.SmallMap_TSA_Button.TabIndex = 12;
            this.SmallMap_TSA_Button.Text = "Open TSA Editor...";
            this.SmallMap_TSA_Button.UseVisualStyleBackColor = true;
            this.SmallMap_TSA_Button.Click += new System.EventHandler(this.SmallMap_TSA_Button_Click);
            // 
            // LargeMap_GroupBox
            // 
            this.LargeMap_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_TSA_NumBox);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_TSA_Button);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_TilesetLabel);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_InsertButton);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_TSALabel);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_PaletteBox);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_Tileset_PointerBox);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_TSA_PointerBox);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_PaletteLabel);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_Palette_PointerBox);
            this.LargeMap_GroupBox.Location = new System.Drawing.Point(12, 323);
            this.LargeMap_GroupBox.Name = "LargeMap_GroupBox";
            this.LargeMap_GroupBox.Size = new System.Drawing.Size(240, 137);
            this.LargeMap_GroupBox.TabIndex = 13;
            this.LargeMap_GroupBox.TabStop = false;
            this.LargeMap_GroupBox.Text = "Large Map (1024x688)";
            // 
            // LargeMap_TSA_NumBox
            // 
            this.LargeMap_TSA_NumBox.Location = new System.Drawing.Point(106, 105);
            this.LargeMap_TSA_NumBox.Maximum = new decimal(new int[] {
            11,
            0,
            0,
            0});
            this.LargeMap_TSA_NumBox.Name = "LargeMap_TSA_NumBox";
            this.LargeMap_TSA_NumBox.Size = new System.Drawing.Size(41, 20);
            this.LargeMap_TSA_NumBox.TabIndex = 18;
            this.LargeMap_TSA_NumBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // LargeMap_TSA_Button
            // 
            this.LargeMap_TSA_Button.Location = new System.Drawing.Point(153, 99);
            this.LargeMap_TSA_Button.Name = "LargeMap_TSA_Button";
            this.LargeMap_TSA_Button.Size = new System.Drawing.Size(81, 32);
            this.LargeMap_TSA_Button.TabIndex = 13;
            this.LargeMap_TSA_Button.Text = "Open TSA...";
            this.LargeMap_TSA_Button.UseVisualStyleBackColor = true;
            this.LargeMap_TSA_Button.Click += new System.EventHandler(this.LargeMap_TSA_Button_Click);
            // 
            // LargeMap_TilesetLabel
            // 
            this.LargeMap_TilesetLabel.AutoSize = true;
            this.LargeMap_TilesetLabel.Location = new System.Drawing.Point(20, 54);
            this.LargeMap_TilesetLabel.Name = "LargeMap_TilesetLabel";
            this.LargeMap_TilesetLabel.Size = new System.Drawing.Size(68, 13);
            this.LargeMap_TilesetLabel.TabIndex = 13;
            this.LargeMap_TilesetLabel.Text = "Tileset Array:";
            // 
            // LargeMap_TSALabel
            // 
            this.LargeMap_TSALabel.AutoSize = true;
            this.LargeMap_TSALabel.Location = new System.Drawing.Point(20, 95);
            this.LargeMap_TSALabel.Name = "LargeMap_TSALabel";
            this.LargeMap_TSALabel.Size = new System.Drawing.Size(58, 13);
            this.LargeMap_TSALabel.TabIndex = 17;
            this.LargeMap_TSALabel.Text = "TSA Array:";
            // 
            // LargeMap_Tileset_PointerBox
            // 
            this.LargeMap_Tileset_PointerBox.Hexadecimal = true;
            this.LargeMap_Tileset_PointerBox.Location = new System.Drawing.Point(20, 70);
            this.LargeMap_Tileset_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.LargeMap_Tileset_PointerBox.Name = "LargeMap_Tileset_PointerBox";
            this.LargeMap_Tileset_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.LargeMap_Tileset_PointerBox.TabIndex = 12;
            this.LargeMap_Tileset_PointerBox.ValueChanged += new System.EventHandler(this.LargeMap_Tileset_PointerBox_ValueChanged);
            // 
            // LargeMap_TSA_PointerBox
            // 
            this.LargeMap_TSA_PointerBox.Hexadecimal = true;
            this.LargeMap_TSA_PointerBox.Location = new System.Drawing.Point(20, 111);
            this.LargeMap_TSA_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.LargeMap_TSA_PointerBox.Name = "LargeMap_TSA_PointerBox";
            this.LargeMap_TSA_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.LargeMap_TSA_PointerBox.TabIndex = 16;
            this.LargeMap_TSA_PointerBox.ValueChanged += new System.EventHandler(this.LargeMap_TSA_PointerBox_ValueChanged);
            // 
            // LargeMap_PaletteLabel
            // 
            this.LargeMap_PaletteLabel.AutoSize = true;
            this.LargeMap_PaletteLabel.Location = new System.Drawing.Point(20, 15);
            this.LargeMap_PaletteLabel.Name = "LargeMap_PaletteLabel";
            this.LargeMap_PaletteLabel.Size = new System.Drawing.Size(46, 13);
            this.LargeMap_PaletteLabel.TabIndex = 15;
            this.LargeMap_PaletteLabel.Text = "Palette :";
            // 
            // LargeMap_Palette_PointerBox
            // 
            this.LargeMap_Palette_PointerBox.Hexadecimal = true;
            this.LargeMap_Palette_PointerBox.Location = new System.Drawing.Point(20, 31);
            this.LargeMap_Palette_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.LargeMap_Palette_PointerBox.Name = "LargeMap_Palette_PointerBox";
            this.LargeMap_Palette_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.LargeMap_Palette_PointerBox.TabIndex = 14;
            this.LargeMap_Palette_PointerBox.ValueChanged += new System.EventHandler(this.LargeMap_Palette_PointerBox_ValueChanged);
            // 
            // WorldMapEditor_FE7
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 472);
            this.Controls.Add(this.LargeMap_GroupBox);
            this.Controls.Add(this.SmallMap_GroupBox);
            this.Controls.Add(this.SmallMap_ImageBox);
            this.Controls.Add(this.LargeMap_Panel);
            this.MinimumSize = new System.Drawing.Size(770, 510);
            this.Name = "WorldMapEditor_FE7";
            this.Text = "World Map Editor";
            this.LargeMap_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SmallMap_Tileset_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmallMap_Palette_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmallMap_TSA_PointerBox)).EndInit();
            this.SmallMap_GroupBox.ResumeLayout(false);
            this.SmallMap_GroupBox.PerformLayout();
            this.LargeMap_GroupBox.ResumeLayout(false);
            this.LargeMap_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_TSA_NumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_Tileset_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_TSA_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_Palette_PointerBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Components.PaletteBox LargeMap_PaletteBox;
        private Components.ImageBox LargeMap_ImageBox;
        private System.Windows.Forms.Button SmallMap_InsertButton;
        private System.Windows.Forms.Panel LargeMap_Panel;
        private Components.ImageBox SmallMap_ImageBox;
        private Components.PaletteBox SmallMap_PaletteBox;
        private System.Windows.Forms.Button LargeMap_InsertButton;
        private Components.PointerBox SmallMap_Tileset_PointerBox;
        private System.Windows.Forms.Label SmallMap_TilesetLabel;
        private System.Windows.Forms.Label SmallMap_PaletteLabel;
        private Components.PointerBox SmallMap_Palette_PointerBox;
        private System.Windows.Forms.Label SmallMap_TSALabel;
        private Components.PointerBox SmallMap_TSA_PointerBox;
        private System.Windows.Forms.GroupBox SmallMap_GroupBox;
        private System.Windows.Forms.GroupBox LargeMap_GroupBox;
        private System.Windows.Forms.Label LargeMap_TilesetLabel;
        private System.Windows.Forms.Label LargeMap_TSALabel;
        private Components.PointerBox LargeMap_Tileset_PointerBox;
        private Components.PointerBox LargeMap_TSA_PointerBox;
        private System.Windows.Forms.Label LargeMap_PaletteLabel;
        private Components.PointerBox LargeMap_Palette_PointerBox;
        private System.Windows.Forms.Button SmallMap_TSA_Button;
        private System.Windows.Forms.Button LargeMap_TSA_Button;
        private System.Windows.Forms.NumericUpDown LargeMap_TSA_NumBox;
    }
}