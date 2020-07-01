namespace EmblemMagic.Editors
{
    partial class WorldMapEditor_FE6
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
            this.SmallMap_InsertButton = new System.Windows.Forms.Button();
            this.SmallMap_ImageBox = new EmblemMagic.Components.ImageBox();
            this.SmallMap_PaletteBox = new EmblemMagic.Components.PaletteBox();
            this.LargeMap_ImageBox = new EmblemMagic.Components.ImageBox();
            this.LargeMap_PaletteBox = new EmblemMagic.Components.PaletteBox();
            this.SmallMap_GroupBox = new System.Windows.Forms.GroupBox();
            this.SmallMap_GraphicsPointerBox = new EmblemMagic.Components.PointerBox();
            this.SmallMap_GraphicsLabel = new System.Windows.Forms.Label();
            this.SmallMap_PalettePointerBox = new EmblemMagic.Components.PointerBox();
            this.SmallMap_PaletteLabel = new System.Windows.Forms.Label();
            this.LargeMap_GroupBox = new System.Windows.Forms.GroupBox();
            this.LargeMap_TL_GraphicsLabel = new System.Windows.Forms.Label();
            this.LargeMap_BL_GraphicsLabel = new System.Windows.Forms.Label();
            this.LargeMap_BR_GraphicsLabel = new System.Windows.Forms.Label();
            this.LargeMap_BL_GraphicsPointerBox = new EmblemMagic.Components.PointerBox();
            this.LargeMap_TR_GraphicsLabel = new System.Windows.Forms.Label();
            this.LargeMap_InsertButton = new System.Windows.Forms.Button();
            this.LargeMap_TR_GraphicsPointerBox = new EmblemMagic.Components.PointerBox();
            this.LargeMap_PalettePointerBox = new EmblemMagic.Components.PointerBox();
            this.LargeMap_BR_GraphicsPointerBox = new EmblemMagic.Components.PointerBox();
            this.LargeMap_TL_PaletteLabel = new System.Windows.Forms.Label();
            this.LargeMap_TL_GraphicsPointerBox = new EmblemMagic.Components.PointerBox();
            this.SmallMap_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SmallMap_GraphicsPointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmallMap_PalettePointerBox)).BeginInit();
            this.LargeMap_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_BL_GraphicsPointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_TR_GraphicsPointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_PalettePointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_BR_GraphicsPointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_TL_GraphicsPointerBox)).BeginInit();
            this.SuspendLayout();
            // 
            // SmallMap_InsertButton
            // 
            this.SmallMap_InsertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SmallMap_InsertButton.Location = new System.Drawing.Point(6, 19);
            this.SmallMap_InsertButton.Name = "SmallMap_InsertButton";
            this.SmallMap_InsertButton.Size = new System.Drawing.Size(94, 36);
            this.SmallMap_InsertButton.TabIndex = 2;
            this.SmallMap_InsertButton.Text = "Insert image...";
            this.SmallMap_InsertButton.UseVisualStyleBackColor = true;
            this.SmallMap_InsertButton.Click += new System.EventHandler(this.SmallMap_InsertButton_Click);
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
            this.SmallMap_PaletteBox.Size = new System.Drawing.Size(128, 128);
            this.SmallMap_PaletteBox.TabIndex = 4;
            this.SmallMap_PaletteBox.TabStop = false;
            this.SmallMap_PaletteBox.Text = "paletteBox2";
            this.SmallMap_PaletteBox.Click += new System.EventHandler(this.SmallMap_PaletteBox_Click);
            // 
            // LargeMap_ImageBox
            // 
            this.LargeMap_ImageBox.BackColor = System.Drawing.SystemColors.Control;
            this.LargeMap_ImageBox.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.LargeMap_ImageBox.Location = new System.Drawing.Point(262, 15);
            this.LargeMap_ImageBox.Name = "LargeMap_ImageBox";
            this.LargeMap_ImageBox.Size = new System.Drawing.Size(480, 320);
            this.LargeMap_ImageBox.TabIndex = 1;
            this.LargeMap_ImageBox.TabStop = false;
            this.LargeMap_ImageBox.Text = "imageBox1";
            // 
            // LargeMap_PaletteBox
            // 
            this.LargeMap_PaletteBox.ColorsPerLine = 32;
            this.LargeMap_PaletteBox.Location = new System.Drawing.Point(468, 11);
            this.LargeMap_PaletteBox.Name = "LargeMap_PaletteBox";
            this.LargeMap_PaletteBox.Size = new System.Drawing.Size(256, 64);
            this.LargeMap_PaletteBox.TabIndex = 7;
            this.LargeMap_PaletteBox.TabStop = false;
            this.LargeMap_PaletteBox.Text = "LargeMap_TL_PaletteBox";
            this.LargeMap_PaletteBox.Click += new System.EventHandler(this.LargeMap_PaletteBox_Click);
            // 
            // SmallMap_GroupBox
            // 
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_GraphicsPointerBox);
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_GraphicsLabel);
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_PalettePointerBox);
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_PaletteLabel);
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_PaletteBox);
            this.SmallMap_GroupBox.Controls.Add(this.SmallMap_InsertButton);
            this.SmallMap_GroupBox.Location = new System.Drawing.Point(12, 181);
            this.SmallMap_GroupBox.Name = "SmallMap_GroupBox";
            this.SmallMap_GroupBox.Size = new System.Drawing.Size(240, 154);
            this.SmallMap_GroupBox.TabIndex = 8;
            this.SmallMap_GroupBox.TabStop = false;
            this.SmallMap_GroupBox.Text = "Small Map (240x160)";
            // 
            // SmallMap_GraphicsPointerBox
            // 
            this.SmallMap_GraphicsPointerBox.Hexadecimal = true;
            this.SmallMap_GraphicsPointerBox.Location = new System.Drawing.Point(18, 80);
            this.SmallMap_GraphicsPointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.SmallMap_GraphicsPointerBox.Name = "SmallMap_GraphicsPointerBox";
            this.SmallMap_GraphicsPointerBox.Size = new System.Drawing.Size(70, 20);
            this.SmallMap_GraphicsPointerBox.TabIndex = 12;
            this.SmallMap_GraphicsPointerBox.ValueChanged += new System.EventHandler(this.SmallMap_GraphicsPointerBox_ValueChanged);
            // 
            // SmallMap_GraphicsLabel
            // 
            this.SmallMap_GraphicsLabel.AutoSize = true;
            this.SmallMap_GraphicsLabel.Location = new System.Drawing.Point(18, 64);
            this.SmallMap_GraphicsLabel.Name = "SmallMap_GraphicsLabel";
            this.SmallMap_GraphicsLabel.Size = new System.Drawing.Size(55, 13);
            this.SmallMap_GraphicsLabel.TabIndex = 13;
            this.SmallMap_GraphicsLabel.Text = "Graphics :";
            // 
            // SmallMap_PalettePointerBox
            // 
            this.SmallMap_PalettePointerBox.Hexadecimal = true;
            this.SmallMap_PalettePointerBox.Location = new System.Drawing.Point(18, 124);
            this.SmallMap_PalettePointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.SmallMap_PalettePointerBox.Name = "SmallMap_PalettePointerBox";
            this.SmallMap_PalettePointerBox.Size = new System.Drawing.Size(70, 20);
            this.SmallMap_PalettePointerBox.TabIndex = 10;
            this.SmallMap_PalettePointerBox.ValueChanged += new System.EventHandler(this.SmallMap_PalettePointerBox_ValueChanged);
            // 
            // SmallMap_PaletteLabel
            // 
            this.SmallMap_PaletteLabel.AutoSize = true;
            this.SmallMap_PaletteLabel.Location = new System.Drawing.Point(18, 108);
            this.SmallMap_PaletteLabel.Name = "SmallMap_PaletteLabel";
            this.SmallMap_PaletteLabel.Size = new System.Drawing.Size(46, 13);
            this.SmallMap_PaletteLabel.TabIndex = 11;
            this.SmallMap_PaletteLabel.Text = "Palette :";
            // 
            // LargeMap_GroupBox
            // 
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_TL_GraphicsLabel);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_BL_GraphicsLabel);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_BR_GraphicsLabel);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_BL_GraphicsPointerBox);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_TR_GraphicsLabel);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_InsertButton);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_TR_GraphicsPointerBox);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_PalettePointerBox);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_BR_GraphicsPointerBox);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_TL_PaletteLabel);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_TL_GraphicsPointerBox);
            this.LargeMap_GroupBox.Controls.Add(this.LargeMap_PaletteBox);
            this.LargeMap_GroupBox.Location = new System.Drawing.Point(12, 341);
            this.LargeMap_GroupBox.Name = "LargeMap_GroupBox";
            this.LargeMap_GroupBox.Size = new System.Drawing.Size(730, 82);
            this.LargeMap_GroupBox.TabIndex = 9;
            this.LargeMap_GroupBox.TabStop = false;
            this.LargeMap_GroupBox.Text = "Large Map (480x320)";
            // 
            // LargeMap_TL_GraphicsLabel
            // 
            this.LargeMap_TL_GraphicsLabel.AutoSize = true;
            this.LargeMap_TL_GraphicsLabel.Location = new System.Drawing.Point(12, 21);
            this.LargeMap_TL_GraphicsLabel.Name = "LargeMap_TL_GraphicsLabel";
            this.LargeMap_TL_GraphicsLabel.Size = new System.Drawing.Size(77, 13);
            this.LargeMap_TL_GraphicsLabel.TabIndex = 26;
            this.LargeMap_TL_GraphicsLabel.Text = "Graphics (TL) :";
            // 
            // LargeMap_BL_GraphicsLabel
            // 
            this.LargeMap_BL_GraphicsLabel.AutoSize = true;
            this.LargeMap_BL_GraphicsLabel.Location = new System.Drawing.Point(12, 51);
            this.LargeMap_BL_GraphicsLabel.Name = "LargeMap_BL_GraphicsLabel";
            this.LargeMap_BL_GraphicsLabel.Size = new System.Drawing.Size(77, 13);
            this.LargeMap_BL_GraphicsLabel.TabIndex = 24;
            this.LargeMap_BL_GraphicsLabel.Text = "Graphics (BL) :";
            // 
            // LargeMap_BR_GraphicsLabel
            // 
            this.LargeMap_BR_GraphicsLabel.AutoSize = true;
            this.LargeMap_BR_GraphicsLabel.Location = new System.Drawing.Point(174, 51);
            this.LargeMap_BR_GraphicsLabel.Name = "LargeMap_BR_GraphicsLabel";
            this.LargeMap_BR_GraphicsLabel.Size = new System.Drawing.Size(79, 13);
            this.LargeMap_BR_GraphicsLabel.TabIndex = 23;
            this.LargeMap_BR_GraphicsLabel.Text = "Graphics (BR) :";
            // 
            // LargeMap_BL_GraphicsPointerBox
            // 
            this.LargeMap_BL_GraphicsPointerBox.Hexadecimal = true;
            this.LargeMap_BL_GraphicsPointerBox.Location = new System.Drawing.Point(95, 49);
            this.LargeMap_BL_GraphicsPointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.LargeMap_BL_GraphicsPointerBox.Name = "LargeMap_BL_GraphicsPointerBox";
            this.LargeMap_BL_GraphicsPointerBox.Size = new System.Drawing.Size(70, 20);
            this.LargeMap_BL_GraphicsPointerBox.TabIndex = 20;
            this.LargeMap_BL_GraphicsPointerBox.ValueChanged += new System.EventHandler(this.LargeMap_BL_GraphicsPointerBox_ValueChanged);
            // 
            // LargeMap_TR_GraphicsLabel
            // 
            this.LargeMap_TR_GraphicsLabel.AutoSize = true;
            this.LargeMap_TR_GraphicsLabel.Location = new System.Drawing.Point(174, 21);
            this.LargeMap_TR_GraphicsLabel.Name = "LargeMap_TR_GraphicsLabel";
            this.LargeMap_TR_GraphicsLabel.Size = new System.Drawing.Size(79, 13);
            this.LargeMap_TR_GraphicsLabel.TabIndex = 25;
            this.LargeMap_TR_GraphicsLabel.Text = "Graphics (TR) :";
            // 
            // LargeMap_InsertButton
            // 
            this.LargeMap_InsertButton.Location = new System.Drawing.Point(343, 39);
            this.LargeMap_InsertButton.Name = "LargeMap_InsertButton";
            this.LargeMap_InsertButton.Size = new System.Drawing.Size(119, 36);
            this.LargeMap_InsertButton.TabIndex = 14;
            this.LargeMap_InsertButton.Text = "Insert image...";
            this.LargeMap_InsertButton.UseVisualStyleBackColor = true;
            this.LargeMap_InsertButton.Click += new System.EventHandler(this.LargeMap_InsertButton_Click);
            // 
            // LargeMap_TR_GraphicsPointerBox
            // 
            this.LargeMap_TR_GraphicsPointerBox.Hexadecimal = true;
            this.LargeMap_TR_GraphicsPointerBox.Location = new System.Drawing.Point(259, 19);
            this.LargeMap_TR_GraphicsPointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.LargeMap_TR_GraphicsPointerBox.Name = "LargeMap_TR_GraphicsPointerBox";
            this.LargeMap_TR_GraphicsPointerBox.Size = new System.Drawing.Size(70, 20);
            this.LargeMap_TR_GraphicsPointerBox.TabIndex = 18;
            this.LargeMap_TR_GraphicsPointerBox.ValueChanged += new System.EventHandler(this.LargeMap_TR_GraphicsPointerBox_ValueChanged);
            // 
            // LargeMap_PalettePointerBox
            // 
            this.LargeMap_PalettePointerBox.Hexadecimal = true;
            this.LargeMap_PalettePointerBox.Location = new System.Drawing.Point(392, 13);
            this.LargeMap_PalettePointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.LargeMap_PalettePointerBox.Name = "LargeMap_PalettePointerBox";
            this.LargeMap_PalettePointerBox.Size = new System.Drawing.Size(70, 20);
            this.LargeMap_PalettePointerBox.TabIndex = 16;
            this.LargeMap_PalettePointerBox.ValueChanged += new System.EventHandler(this.LargeMap_TL_PalettePointerBox_ValueChanged);
            // 
            // LargeMap_BR_GraphicsPointerBox
            // 
            this.LargeMap_BR_GraphicsPointerBox.Hexadecimal = true;
            this.LargeMap_BR_GraphicsPointerBox.Location = new System.Drawing.Point(259, 49);
            this.LargeMap_BR_GraphicsPointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.LargeMap_BR_GraphicsPointerBox.Name = "LargeMap_BR_GraphicsPointerBox";
            this.LargeMap_BR_GraphicsPointerBox.Size = new System.Drawing.Size(70, 20);
            this.LargeMap_BR_GraphicsPointerBox.TabIndex = 22;
            this.LargeMap_BR_GraphicsPointerBox.ValueChanged += new System.EventHandler(this.LargeMap_BR_GraphicsPointerBox_ValueChanged);
            // 
            // LargeMap_TL_PaletteLabel
            // 
            this.LargeMap_TL_PaletteLabel.AutoSize = true;
            this.LargeMap_TL_PaletteLabel.Location = new System.Drawing.Point(340, 15);
            this.LargeMap_TL_PaletteLabel.Name = "LargeMap_TL_PaletteLabel";
            this.LargeMap_TL_PaletteLabel.Size = new System.Drawing.Size(46, 13);
            this.LargeMap_TL_PaletteLabel.TabIndex = 17;
            this.LargeMap_TL_PaletteLabel.Text = "Palette :";
            // 
            // LargeMap_TL_GraphicsPointerBox
            // 
            this.LargeMap_TL_GraphicsPointerBox.Hexadecimal = true;
            this.LargeMap_TL_GraphicsPointerBox.Location = new System.Drawing.Point(95, 19);
            this.LargeMap_TL_GraphicsPointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.LargeMap_TL_GraphicsPointerBox.Name = "LargeMap_TL_GraphicsPointerBox";
            this.LargeMap_TL_GraphicsPointerBox.Size = new System.Drawing.Size(70, 20);
            this.LargeMap_TL_GraphicsPointerBox.TabIndex = 14;
            this.LargeMap_TL_GraphicsPointerBox.ValueChanged += new System.EventHandler(this.LargeMap_TL_GraphicsPointerBox_ValueChanged);
            // 
            // WorldMapEditor_FE6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 436);
            this.Controls.Add(this.LargeMap_GroupBox);
            this.Controls.Add(this.SmallMap_GroupBox);
            this.Controls.Add(this.LargeMap_ImageBox);
            this.Controls.Add(this.SmallMap_ImageBox);
            this.MinimumSize = new System.Drawing.Size(770, 474);
            this.Name = "WorldMapEditor_FE6";
            this.Text = "World Map Editor";
            this.SmallMap_GroupBox.ResumeLayout(false);
            this.SmallMap_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SmallMap_GraphicsPointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmallMap_PalettePointerBox)).EndInit();
            this.LargeMap_GroupBox.ResumeLayout(false);
            this.LargeMap_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_BL_GraphicsPointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_TR_GraphicsPointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_PalettePointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_BR_GraphicsPointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMap_TL_GraphicsPointerBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button SmallMap_InsertButton;
        private Components.ImageBox SmallMap_ImageBox;
        private Components.PaletteBox SmallMap_PaletteBox;
        private Components.ImageBox LargeMap_ImageBox;
        private Components.PaletteBox LargeMap_PaletteBox;
        private System.Windows.Forms.GroupBox SmallMap_GroupBox;
        private System.Windows.Forms.GroupBox LargeMap_GroupBox;
        private Components.PointerBox SmallMap_GraphicsPointerBox;
        private System.Windows.Forms.Label SmallMap_GraphicsLabel;
        private Components.PointerBox SmallMap_PalettePointerBox;
        private System.Windows.Forms.Label SmallMap_PaletteLabel;
        private Components.PointerBox LargeMap_TL_GraphicsPointerBox;
        private Components.PointerBox LargeMap_TR_GraphicsPointerBox;
        private Components.PointerBox LargeMap_BR_GraphicsPointerBox;
        private Components.PointerBox LargeMap_BL_GraphicsPointerBox;
        private Components.PointerBox LargeMap_PalettePointerBox;
        private System.Windows.Forms.Label LargeMap_TL_PaletteLabel;
        private System.Windows.Forms.Button LargeMap_InsertButton;
        private System.Windows.Forms.Label LargeMap_TL_GraphicsLabel;
        private System.Windows.Forms.Label LargeMap_BL_GraphicsLabel;
        private System.Windows.Forms.Label LargeMap_BR_GraphicsLabel;
        private System.Windows.Forms.Label LargeMap_TR_GraphicsLabel;
    }
}