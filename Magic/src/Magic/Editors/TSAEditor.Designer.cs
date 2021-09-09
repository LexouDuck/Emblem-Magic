namespace Magic.Editors
{
    partial class TSAEditor
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
            this.MenuBar = new System.Windows.Forms.MenuStrip();
            this.EntryInfo_Label = new System.Windows.Forms.ToolStripLabel();
            this.Palette_NumBox = new System.Windows.Forms.NumericUpDown();
            this.PaletteLabel = new System.Windows.Forms.Label();
            this.TileLabel = new System.Windows.Forms.Label();
            this.TileIndex_NumBox = new System.Windows.Forms.NumericUpDown();
            this.TSA_GridBox = new Magic.Components.GridBox();
            this.FlipH_CheckBox = new System.Windows.Forms.CheckBox();
            this.FlipV_CheckBox = new System.Windows.Forms.CheckBox();
            this.Edit_GroupBox = new System.Windows.Forms.GroupBox();
            this.Coordinates_Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.CurrentValue_Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.Load_Button = new System.Windows.Forms.Button();
            this.Save_Button = new System.Windows.Forms.Button();
            this.Status = new System.Windows.Forms.StatusStrip();
            this.TSA_Panel = new System.Windows.Forms.Panel();
            this.ViewGrid_CheckBox = new System.Windows.Forms.CheckBox();
            this.MenuBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Palette_NumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TileIndex_NumBox)).BeginInit();
            this.Edit_GroupBox.SuspendLayout();
            this.Status.SuspendLayout();
            this.TSA_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuBar
            // 
            this.MenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EntryInfo_Label});
            this.MenuBar.Location = new System.Drawing.Point(0, 0);
            this.MenuBar.Name = "MenuBar";
            this.MenuBar.Size = new System.Drawing.Size(270, 24);
            this.MenuBar.TabIndex = 1;
            this.MenuBar.Text = "menuStrip1";
            // 
            // EntryInfo_Label
            // 
            this.EntryInfo_Label.Name = "EntryInfo_Label";
            this.EntryInfo_Label.Size = new System.Drawing.Size(161, 17);
            this.EntryInfo_Label.Text = "TSA Entry Name - [ADDRESS]";
            // 
            // Palette_NumBox
            // 
            this.Palette_NumBox.Location = new System.Drawing.Point(72, 45);
            this.Palette_NumBox.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.Palette_NumBox.Name = "Palette_NumBox";
            this.Palette_NumBox.Size = new System.Drawing.Size(57, 20);
            this.Palette_NumBox.TabIndex = 2;
            this.Palette_NumBox.ValueChanged += new System.EventHandler(this.Palette_NumBox_ValueChanged);
            // 
            // PaletteLabel
            // 
            this.PaletteLabel.AutoSize = true;
            this.PaletteLabel.Location = new System.Drawing.Point(20, 47);
            this.PaletteLabel.Name = "PaletteLabel";
            this.PaletteLabel.Size = new System.Drawing.Size(46, 13);
            this.PaletteLabel.TabIndex = 3;
            this.PaletteLabel.Text = "Palette :";
            // 
            // TileLabel
            // 
            this.TileLabel.AutoSize = true;
            this.TileLabel.Location = new System.Drawing.Point(8, 21);
            this.TileLabel.Name = "TileLabel";
            this.TileLabel.Size = new System.Drawing.Size(58, 13);
            this.TileLabel.TabIndex = 5;
            this.TileLabel.Text = "Tile index :";
            // 
            // TileIndex_NumBox
            // 
            this.TileIndex_NumBox.Location = new System.Drawing.Point(72, 19);
            this.TileIndex_NumBox.Maximum = new decimal(new int[] {
            1023,
            0,
            0,
            0});
            this.TileIndex_NumBox.Name = "TileIndex_NumBox";
            this.TileIndex_NumBox.Size = new System.Drawing.Size(57, 20);
            this.TileIndex_NumBox.TabIndex = 4;
            this.TileIndex_NumBox.ValueChanged += new System.EventHandler(this.TileIndex_NumBox_ValueChanged);
            // 
            // TSA_GridBox
            // 
            this.TSA_GridBox.Location = new System.Drawing.Point(3, 3);
            this.TSA_GridBox.Name = "TSA_GridBox";
            this.TSA_GridBox.Selection = null;
            this.TSA_GridBox.ShowGrid = true;
            this.TSA_GridBox.Size = new System.Drawing.Size(240, 160);
            this.TSA_GridBox.TabIndex = 0;
            this.TSA_GridBox.TabStop = false;
            this.TSA_GridBox.Text = "TSAGridBox";
            this.TSA_GridBox.TileSize = 8;
            this.TSA_GridBox.SelectionChanged += new System.EventHandler(this.TSA_GridBox_SelectionChanged);
            // 
            // FlipH_CheckBox
            // 
            this.FlipH_CheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FlipH_CheckBox.AutoSize = true;
            this.FlipH_CheckBox.Location = new System.Drawing.Point(187, 20);
            this.FlipH_CheckBox.Name = "FlipH_CheckBox";
            this.FlipH_CheckBox.Size = new System.Drawing.Size(53, 17);
            this.FlipH_CheckBox.TabIndex = 6;
            this.FlipH_CheckBox.Text = "Flip H";
            this.FlipH_CheckBox.UseVisualStyleBackColor = true;
            this.FlipH_CheckBox.CheckedChanged += new System.EventHandler(this.FlipH_CheckBox_CheckedChanged);
            // 
            // FlipV_CheckBox
            // 
            this.FlipV_CheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FlipV_CheckBox.AutoSize = true;
            this.FlipV_CheckBox.Location = new System.Drawing.Point(187, 46);
            this.FlipV_CheckBox.Name = "FlipV_CheckBox";
            this.FlipV_CheckBox.Size = new System.Drawing.Size(52, 17);
            this.FlipV_CheckBox.TabIndex = 7;
            this.FlipV_CheckBox.Text = "Flip V";
            this.FlipV_CheckBox.UseVisualStyleBackColor = true;
            this.FlipV_CheckBox.CheckedChanged += new System.EventHandler(this.FlipV_CheckBox_CheckedChanged);
            // 
            // Edit_GroupBox
            // 
            this.Edit_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Edit_GroupBox.Controls.Add(this.TileIndex_NumBox);
            this.Edit_GroupBox.Controls.Add(this.FlipV_CheckBox);
            this.Edit_GroupBox.Controls.Add(this.Palette_NumBox);
            this.Edit_GroupBox.Controls.Add(this.FlipH_CheckBox);
            this.Edit_GroupBox.Controls.Add(this.PaletteLabel);
            this.Edit_GroupBox.Controls.Add(this.TileLabel);
            this.Edit_GroupBox.Location = new System.Drawing.Point(12, 232);
            this.Edit_GroupBox.Name = "Edit_GroupBox";
            this.Edit_GroupBox.Size = new System.Drawing.Size(246, 75);
            this.Edit_GroupBox.TabIndex = 8;
            this.Edit_GroupBox.TabStop = false;
            this.Edit_GroupBox.Text = "Edit TSA";
            // 
            // Coordinates_Label
            // 
            this.Coordinates_Label.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.Coordinates_Label.Name = "Coordinates_Label";
            this.Coordinates_Label.Size = new System.Drawing.Size(75, 17);
            this.Coordinates_Label.Text = "X : ___, Y : ___";
            this.Coordinates_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CurrentValue_Label
            // 
            this.CurrentValue_Label.Name = "CurrentValue_Label";
            this.CurrentValue_Label.Size = new System.Drawing.Size(180, 17);
            this.CurrentValue_Label.Spring = true;
            this.CurrentValue_Label.Text = "TSA Value :";
            this.CurrentValue_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Load_Button
            // 
            this.Load_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Load_Button.Location = new System.Drawing.Point(170, 196);
            this.Load_Button.Name = "Load_Button";
            this.Load_Button.Size = new System.Drawing.Size(88, 33);
            this.Load_Button.TabIndex = 11;
            this.Load_Button.Text = "Load TSA file...";
            this.Load_Button.UseVisualStyleBackColor = true;
            this.Load_Button.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // Save_Button
            // 
            this.Save_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Save_Button.Location = new System.Drawing.Point(84, 196);
            this.Save_Button.Name = "Save_Button";
            this.Save_Button.Size = new System.Drawing.Size(80, 33);
            this.Save_Button.TabIndex = 12;
            this.Save_Button.Text = "Save to file...";
            this.Save_Button.UseVisualStyleBackColor = true;
            this.Save_Button.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // Status
            // 
            this.Status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CurrentValue_Label,
            this.Coordinates_Label});
            this.Status.Location = new System.Drawing.Point(0, 310);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(270, 22);
            this.Status.TabIndex = 13;
            // 
            // TSA_Panel
            // 
            this.TSA_Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TSA_Panel.AutoScroll = true;
            this.TSA_Panel.Controls.Add(this.TSA_GridBox);
            this.TSA_Panel.Location = new System.Drawing.Point(12, 27);
            this.TSA_Panel.Name = "TSA_Panel";
            this.TSA_Panel.Size = new System.Drawing.Size(246, 166);
            this.TSA_Panel.TabIndex = 14;
            // 
            // checkBox1
            // 
            this.ViewGrid_CheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ViewGrid_CheckBox.AutoSize = true;
            this.ViewGrid_CheckBox.Checked = true;
            this.ViewGrid_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ViewGrid_CheckBox.Location = new System.Drawing.Point(12, 205);
            this.ViewGrid_CheckBox.Name = "checkBox1";
            this.ViewGrid_CheckBox.Size = new System.Drawing.Size(71, 17);
            this.ViewGrid_CheckBox.TabIndex = 15;
            this.ViewGrid_CheckBox.Text = "View Grid";
            this.ViewGrid_CheckBox.UseVisualStyleBackColor = true;
            this.ViewGrid_CheckBox.CheckedChanged += new System.EventHandler(this.ViewGrid_CheckBox_CheckedChanged);
            // 
            // TSAEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 332);
            this.Controls.Add(this.ViewGrid_CheckBox);
            this.Controls.Add(this.TSA_Panel);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.Load_Button);
            this.Controls.Add(this.Edit_GroupBox);
            this.Controls.Add(this.Save_Button);
            this.Controls.Add(this.MenuBar);
            this.MainMenuStrip = this.MenuBar;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(286, 300);
            this.Name = "TSAEditor";
            this.Text = "TSA Editor";
            this.MenuBar.ResumeLayout(false);
            this.MenuBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Palette_NumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TileIndex_NumBox)).EndInit();
            this.Edit_GroupBox.ResumeLayout(false);
            this.Edit_GroupBox.PerformLayout();
            this.Status.ResumeLayout(false);
            this.Status.PerformLayout();
            this.TSA_Panel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Components.GridBox TSA_GridBox;
        private System.Windows.Forms.MenuStrip MenuBar;
        private System.Windows.Forms.ToolStripLabel EntryInfo_Label;
        private System.Windows.Forms.NumericUpDown Palette_NumBox;
        private System.Windows.Forms.Label PaletteLabel;
        private System.Windows.Forms.Label TileLabel;
        private System.Windows.Forms.NumericUpDown TileIndex_NumBox;
        private System.Windows.Forms.CheckBox FlipH_CheckBox;
        private System.Windows.Forms.CheckBox FlipV_CheckBox;
        private System.Windows.Forms.GroupBox Edit_GroupBox;
        private System.Windows.Forms.ToolStripStatusLabel Coordinates_Label;
        private System.Windows.Forms.ToolStripStatusLabel CurrentValue_Label;
        private System.Windows.Forms.Button Load_Button;
        private System.Windows.Forms.Button Save_Button;
        private System.Windows.Forms.StatusStrip Status;
        private System.Windows.Forms.Panel TSA_Panel;
        private System.Windows.Forms.CheckBox ViewGrid_CheckBox;
    }
}