namespace KirbyMagic.Editors
{
    partial class TitleScreenEditor
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
            this.Test_ImageBox = new Magic.Components.ImageBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Insert = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.BG_CheckBox = new System.Windows.Forms.CheckBox();
            this.MG_CheckBox = new System.Windows.Forms.CheckBox();
            this.FG_CheckBox = new System.Windows.Forms.CheckBox();
            this.Test_PaletteBox = new Magic.Components.PaletteBox();
            this.BG_MagicButton = new Magic.Components.MagicButton(App);
            this.MG_MagicButton = new Magic.Components.MagicButton(App);
            this.FG_MagicButton = new Magic.Components.MagicButton(App);
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Test_ImageBox
            // 
            this.Test_ImageBox.BackColor = System.Drawing.SystemColors.Control;
            this.Test_ImageBox.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.Test_ImageBox.Location = new System.Drawing.Point(12, 27);
            this.Test_ImageBox.Name = "Test_ImageBox";
            this.Test_ImageBox.Size = new System.Drawing.Size(240, 160);
            this.Test_ImageBox.TabIndex = 0;
            this.Test_ImageBox.TabStop = false;
            this.Test_ImageBox.Text = "imageBox1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(266, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File_Insert,
            this.File_Save});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // File_Insert
            // 
            this.File_Insert.Name = "File_Insert";
            this.File_Insert.Size = new System.Drawing.Size(152, 22);
            this.File_Insert.Text = "Insert image...";
            this.File_Insert.Click += new System.EventHandler(this.File_Insert_Click);
            // 
            // File_Save
            // 
            this.File_Save.Name = "File_Save";
            this.File_Save.Size = new System.Drawing.Size(152, 22);
            this.File_Save.Text = "Save image...";
            this.File_Save.Click += new System.EventHandler(this.File_Save_Click);
            // 
            // BG_CheckBox
            // 
            this.BG_CheckBox.AutoSize = true;
            this.BG_CheckBox.Checked = true;
            this.BG_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BG_CheckBox.Location = new System.Drawing.Point(176, 198);
            this.BG_CheckBox.Name = "BG_CheckBox";
            this.BG_CheckBox.Size = new System.Drawing.Size(70, 17);
            this.BG_CheckBox.TabIndex = 2;
            this.BG_CheckBox.Text = "BG Layer";
            this.Help_ToolTip.SetToolTip(this.BG_CheckBox, "If checked, display the background image for the title screen.");
            this.BG_CheckBox.UseVisualStyleBackColor = true;
            this.BG_CheckBox.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // MG_CheckBox
            // 
            this.MG_CheckBox.AutoSize = true;
            this.MG_CheckBox.Checked = true;
            this.MG_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MG_CheckBox.Location = new System.Drawing.Point(176, 228);
            this.MG_CheckBox.Name = "MG_CheckBox";
            this.MG_CheckBox.Size = new System.Drawing.Size(72, 17);
            this.MG_CheckBox.TabIndex = 3;
            this.MG_CheckBox.Text = "MG Layer";
            this.Help_ToolTip.SetToolTip(this.MG_CheckBox, "If checked, display the middle layer of the title screen");
            this.MG_CheckBox.UseVisualStyleBackColor = true;
            this.MG_CheckBox.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // FG_CheckBox
            // 
            this.FG_CheckBox.AutoSize = true;
            this.FG_CheckBox.Checked = true;
            this.FG_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FG_CheckBox.Location = new System.Drawing.Point(176, 258);
            this.FG_CheckBox.Name = "FG_CheckBox";
            this.FG_CheckBox.Size = new System.Drawing.Size(69, 17);
            this.FG_CheckBox.TabIndex = 4;
            this.FG_CheckBox.Text = "FG Layer";
            this.Help_ToolTip.SetToolTip(this.FG_CheckBox, "If checked, display the foreground layer of the title screen.");
            this.FG_CheckBox.UseVisualStyleBackColor = true;
            this.FG_CheckBox.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // Test_PaletteBox
            // 
            this.Test_PaletteBox.ColorsPerLine = 16;
            this.Test_PaletteBox.Location = new System.Drawing.Point(12, 193);
            this.Test_PaletteBox.Name = "Test_PaletteBox";
            this.Test_PaletteBox.Size = new System.Drawing.Size(128, 128);
            this.Test_PaletteBox.TabIndex = 5;
            this.Test_PaletteBox.TabStop = false;
            this.Test_PaletteBox.Text = "paletteBox1";
            this.Help_ToolTip.SetToolTip(this.Test_PaletteBox, "This is the palette(s) used to display the game\'s title screen.\r\nClick on this to" +
        " open a PaletteEditor, to modify this palette.");
            // 
            // BG_MagicButton
            // 
            this.BG_MagicButton.Location = new System.Drawing.Point(146, 193);
            this.BG_MagicButton.MaximumSize = new System.Drawing.Size(24, 24);
            this.BG_MagicButton.MinimumSize = new System.Drawing.Size(24, 24);
            this.BG_MagicButton.Name = "BG_MagicButton";
            this.BG_MagicButton.Size = new System.Drawing.Size(24, 24);
            this.BG_MagicButton.TabIndex = 6;
            this.Help_ToolTip.SetToolTip(this.BG_MagicButton, "This is a shortcut to the GraphicsEditor, to view/edit the title screen backgroun" +
        "d layer.");
            this.BG_MagicButton.UseVisualStyleBackColor = true;
            this.BG_MagicButton.Click += new System.EventHandler(this.BG_MagicButton_Click);
            // 
            // MG_MagicButton
            // 
            this.MG_MagicButton.Location = new System.Drawing.Point(146, 223);
            this.MG_MagicButton.MaximumSize = new System.Drawing.Size(24, 24);
            this.MG_MagicButton.MinimumSize = new System.Drawing.Size(24, 24);
            this.MG_MagicButton.Name = "MG_MagicButton";
            this.MG_MagicButton.Size = new System.Drawing.Size(24, 24);
            this.MG_MagicButton.TabIndex = 7;
            this.Help_ToolTip.SetToolTip(this.MG_MagicButton, "This is a shortcut to the GraphicsEditor, to view/edit the title screen middle la" +
        "yer.\r\n");
            this.MG_MagicButton.UseVisualStyleBackColor = true;
            this.MG_MagicButton.Click += new System.EventHandler(this.MG_MagicButton_Click);
            // 
            // FG_MagicButton
            // 
            this.FG_MagicButton.Location = new System.Drawing.Point(146, 253);
            this.FG_MagicButton.MaximumSize = new System.Drawing.Size(24, 24);
            this.FG_MagicButton.MinimumSize = new System.Drawing.Size(24, 24);
            this.FG_MagicButton.Name = "FG_MagicButton";
            this.FG_MagicButton.Size = new System.Drawing.Size(24, 24);
            this.FG_MagicButton.TabIndex = 8;
            this.Help_ToolTip.SetToolTip(this.FG_MagicButton, "This is a shortcut to the GraphicsEditor, to view/edit the title screen foregroun" +
        "d layer.\r\n");
            this.FG_MagicButton.UseVisualStyleBackColor = true;
            this.FG_MagicButton.Click += new System.EventHandler(this.FG_MagicButton_Click);
            // 
            // TitleScreenEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 335);
            this.Controls.Add(this.FG_MagicButton);
            this.Controls.Add(this.MG_MagicButton);
            this.Controls.Add(this.BG_MagicButton);
            this.Controls.Add(this.Test_PaletteBox);
            this.Controls.Add(this.FG_CheckBox);
            this.Controls.Add(this.MG_CheckBox);
            this.Controls.Add(this.BG_CheckBox);
            this.Controls.Add(this.Test_ImageBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "TitleScreenEditor";
            this.Text = "Title Screen Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Magic.Components.ImageBox Test_ImageBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.CheckBox BG_CheckBox;
        private System.Windows.Forms.CheckBox MG_CheckBox;
        private System.Windows.Forms.CheckBox FG_CheckBox;
        private Magic.Components.PaletteBox Test_PaletteBox;
        private Magic.Components.MagicButton BG_MagicButton;
        private Magic.Components.MagicButton MG_MagicButton;
        private Magic.Components.MagicButton FG_MagicButton;
        private System.Windows.Forms.ToolStripMenuItem File_Insert;
        private System.Windows.Forms.ToolStripMenuItem File_Save;
    }
}