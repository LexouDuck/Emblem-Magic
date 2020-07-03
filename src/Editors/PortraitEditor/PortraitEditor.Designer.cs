namespace EmblemMagic.Editors
{
    partial class PortraitEditor
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
            this.Test_ImageBox = new EmblemMagic.Components.ImageBox();
            this.Image_ImageBox = new EmblemMagic.Components.ImageBox();
            this.Palette_PointerBox = new EmblemMagic.Components.PointerBox();
            this.Mouth_PointerBox = new EmblemMagic.Components.PointerBox();
            this.Chibi_PointerBox = new EmblemMagic.Components.PointerBox();
            this.Image_PointerBox = new EmblemMagic.Components.PointerBox();
            this.Palette_PaletteBox = new EmblemMagic.Components.PaletteBox();
            this.Image_Label = new System.Windows.Forms.Label();
            this.Chibi_Label = new System.Windows.Forms.Label();
            this.Mouth_Label = new System.Windows.Forms.Label();
            this.EyesClosed_CheckBox = new System.Windows.Forms.CheckBox();
            this.Palette_Label = new System.Windows.Forms.Label();
            this.MouthY_ByteBox = new EmblemMagic.Components.ByteBox();
            this.MouthX_ByteBox = new EmblemMagic.Components.ByteBox();
            this.BlinkY_ByteBox = new EmblemMagic.Components.ByteBox();
            this.BlinkX_ByteBox = new EmblemMagic.Components.ByteBox();
            this.Edit_GroupBox = new System.Windows.Forms.GroupBox();
            this.Placement_GroupBox = new System.Windows.Forms.GroupBox();
            this.Placement_Eyes_Label = new System.Windows.Forms.Label();
            this.Placement_Mouth_Label = new System.Windows.Forms.Label();
            this.Test_GroupBox = new System.Windows.Forms.GroupBox();
            this.Test_Mouth_Frown_RadioButton = new System.Windows.Forms.RadioButton();
            this.Test_Mouth_Smile_RadioButton = new System.Windows.Forms.RadioButton();
            this.Test_Mouth_TrackBar = new System.Windows.Forms.TrackBar();
            this.Test_Blink_TrackBar = new System.Windows.Forms.TrackBar();
            this.Test_Blink_Label = new System.Windows.Forms.Label();
            this.Editor_Menu = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Insert = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.EntryArrayBox = new EmblemMagic.Components.ShortArrayBox();
            ((System.ComponentModel.ISupportInitialize)(this.Palette_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Mouth_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chibi_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MouthY_ByteBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MouthX_ByteBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BlinkY_ByteBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BlinkX_ByteBox)).BeginInit();
            this.Edit_GroupBox.SuspendLayout();
            this.Placement_GroupBox.SuspendLayout();
            this.Test_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Test_Mouth_TrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_Blink_TrackBar)).BeginInit();
            this.Editor_Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // Test_ImageBox
            // 
            this.Test_ImageBox.BackColor = System.Drawing.SystemColors.Control;
            this.Test_ImageBox.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.Test_ImageBox.Location = new System.Drawing.Point(9, 19);
            this.Test_ImageBox.Name = "Test_ImageBox";
            this.Test_ImageBox.Size = new System.Drawing.Size(96, 80);
            this.Test_ImageBox.TabIndex = 0;
            this.Test_ImageBox.TabStop = false;
            this.Test_ImageBox.Text = "imageBox1";
            this.Help_ToolTip.SetToolTip(this.Test_ImageBox, "A preview of the current character portrait. Try changing the values with control" +
        "s on the right.");
            // 
            // Image_ImageBox
            // 
            this.Image_ImageBox.BackColor = System.Drawing.SystemColors.Control;
            this.Image_ImageBox.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.Image_ImageBox.Location = new System.Drawing.Point(132, 19);
            this.Image_ImageBox.Name = "Image_ImageBox";
            this.Image_ImageBox.Size = new System.Drawing.Size(128, 112);
            this.Image_ImageBox.TabIndex = 0;
            this.Image_ImageBox.TabStop = false;
            this.Image_ImageBox.Text = "imageBox2";
            // 
            // Palette_PointerBox
            // 
            this.Palette_PointerBox.Hexadecimal = true;
            this.Palette_PointerBox.Location = new System.Drawing.Point(56, 23);
            this.Palette_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Palette_PointerBox.Name = "Palette_PointerBox";
            this.Palette_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Palette_PointerBox.TabIndex = 5;
            this.Help_ToolTip.SetToolTip(this.Palette_PointerBox, "Pointer to the palette for this character portrait.\r\nWill write to ROM if changed" +
        ".");
            this.Palette_PointerBox.ValueChanged += new System.EventHandler(this.Palette_PointerBox_Changed);
            // 
            // Mouth_PointerBox
            // 
            this.Mouth_PointerBox.Hexadecimal = true;
            this.Mouth_PointerBox.Location = new System.Drawing.Point(56, 103);
            this.Mouth_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Mouth_PointerBox.Name = "Mouth_PointerBox";
            this.Mouth_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Mouth_PointerBox.TabIndex = 4;
            this.Help_ToolTip.SetToolTip(this.Mouth_PointerBox, "Pointer to the tileset/pixel data for the mouth frames of this portrait.\r\nWill wr" +
        "ite to ROM if changed.");
            this.Mouth_PointerBox.ValueChanged += new System.EventHandler(this.Mouth_PointerBox_Changed);
            // 
            // Chibi_PointerBox
            // 
            this.Chibi_PointerBox.Hexadecimal = true;
            this.Chibi_PointerBox.Location = new System.Drawing.Point(56, 75);
            this.Chibi_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Chibi_PointerBox.Name = "Chibi_PointerBox";
            this.Chibi_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Chibi_PointerBox.TabIndex = 3;
            this.Help_ToolTip.SetToolTip(this.Chibi_PointerBox, "Pointer to the chibi mini-portrait pixel data.\r\nWill write to ROM if changed.");
            this.Chibi_PointerBox.ValueChanged += new System.EventHandler(this.Chibi_PointerBox_Changed);
            // 
            // Image_PointerBox
            // 
            this.Image_PointerBox.Hexadecimal = true;
            this.Image_PointerBox.Location = new System.Drawing.Point(56, 49);
            this.Image_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Image_PointerBox.Name = "Image_PointerBox";
            this.Image_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Image_PointerBox.TabIndex = 2;
            this.Help_ToolTip.SetToolTip(this.Image_PointerBox, "Pointer to the main tileset/pixel data for this character portrait.\r\nWill write t" +
        "o ROM if changed.");
            this.Image_PointerBox.ValueChanged += new System.EventHandler(this.Image_PointerBox_Changed);
            // 
            // Palette_PaletteBox
            // 
            this.Palette_PaletteBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Palette_PaletteBox.ColorsPerLine = 16;
            this.Palette_PaletteBox.ForeColor = System.Drawing.SystemColors.Control;
            this.Palette_PaletteBox.Location = new System.Drawing.Point(132, 137);
            this.Palette_PaletteBox.Name = "Palette_PaletteBox";
            this.Palette_PaletteBox.Size = new System.Drawing.Size(128, 8);
            this.Palette_PaletteBox.TabIndex = 0;
            this.Palette_PaletteBox.TabStop = false;
            this.Palette_PaletteBox.Text = "paletteBox1";
            this.Palette_PaletteBox.Click += new System.EventHandler(this.Palette_PaletteBox_Click);
            // 
            // Image_Label
            // 
            this.Image_Label.AutoSize = true;
            this.Image_Label.Location = new System.Drawing.Point(16, 51);
            this.Image_Label.Name = "Image_Label";
            this.Image_Label.Size = new System.Drawing.Size(36, 13);
            this.Image_Label.TabIndex = 0;
            this.Image_Label.Text = "Main :";
            // 
            // Chibi_Label
            // 
            this.Chibi_Label.AutoSize = true;
            this.Chibi_Label.Location = new System.Drawing.Point(16, 79);
            this.Chibi_Label.Name = "Chibi_Label";
            this.Chibi_Label.Size = new System.Drawing.Size(36, 13);
            this.Chibi_Label.TabIndex = 0;
            this.Chibi_Label.Text = "Chibi :";
            // 
            // Mouth_Label
            // 
            this.Mouth_Label.AutoSize = true;
            this.Mouth_Label.Location = new System.Drawing.Point(9, 105);
            this.Mouth_Label.Name = "Mouth_Label";
            this.Mouth_Label.Size = new System.Drawing.Size(43, 13);
            this.Mouth_Label.TabIndex = 0;
            this.Mouth_Label.Text = "Mouth :";
            // 
            // EyesClosed_CheckBox
            // 
            this.EyesClosed_CheckBox.AutoSize = true;
            this.EyesClosed_CheckBox.Location = new System.Drawing.Point(9, 126);
            this.EyesClosed_CheckBox.Name = "EyesClosed_CheckBox";
            this.EyesClosed_CheckBox.Size = new System.Drawing.Size(120, 17);
            this.EyesClosed_CheckBox.TabIndex = 10;
            this.EyesClosed_CheckBox.Text = "Eyes Always Closed";
            this.Help_ToolTip.SetToolTip(this.EyesClosed_CheckBox, "If checked, this portrait will be displayed ingame with eyes always closed.\r\nWill" +
        " write to ROM if changed.");
            this.EyesClosed_CheckBox.UseVisualStyleBackColor = true;
            this.EyesClosed_CheckBox.CheckedChanged += new System.EventHandler(this.EyesClosed_CheckBox_Changed);
            // 
            // Palette_Label
            // 
            this.Palette_Label.AutoSize = true;
            this.Palette_Label.Location = new System.Drawing.Point(6, 25);
            this.Palette_Label.Name = "Palette_Label";
            this.Palette_Label.Size = new System.Drawing.Size(46, 13);
            this.Palette_Label.TabIndex = 0;
            this.Palette_Label.Text = "Palette :";
            // 
            // MouthY_ByteBox
            // 
            this.MouthY_ByteBox.Hexadecimal = true;
            this.MouthY_ByteBox.Location = new System.Drawing.Point(90, 17);
            this.MouthY_ByteBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.MouthY_ByteBox.Name = "MouthY_ByteBox";
            this.MouthY_ByteBox.Size = new System.Drawing.Size(38, 20);
            this.MouthY_ByteBox.TabIndex = 7;
            this.Help_ToolTip.SetToolTip(this.MouthY_ByteBox, "The Y coordinate of the mouth sprite on this portrait.\r\nWill write to ROM if chan" +
        "ged.");
            this.MouthY_ByteBox.Value = ((byte)(0));
            this.MouthY_ByteBox.ValueChanged += new System.EventHandler(this.MouthY_NumBox_Changed);
            // 
            // MouthX_ByteBox
            // 
            this.MouthX_ByteBox.Hexadecimal = true;
            this.MouthX_ByteBox.Location = new System.Drawing.Point(50, 17);
            this.MouthX_ByteBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.MouthX_ByteBox.Name = "MouthX_ByteBox";
            this.MouthX_ByteBox.Size = new System.Drawing.Size(38, 20);
            this.MouthX_ByteBox.TabIndex = 6;
            this.Help_ToolTip.SetToolTip(this.MouthX_ByteBox, "The X coordinate of the mouth sprite on this portrait.\r\nWill write to ROM if chan" +
        "ged.");
            this.MouthX_ByteBox.Value = ((byte)(0));
            this.MouthX_ByteBox.ValueChanged += new System.EventHandler(this.MouthX_NumBox_Changed);
            // 
            // BlinkY_ByteBox
            // 
            this.BlinkY_ByteBox.Hexadecimal = true;
            this.BlinkY_ByteBox.Location = new System.Drawing.Point(210, 17);
            this.BlinkY_ByteBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.BlinkY_ByteBox.Name = "BlinkY_ByteBox";
            this.BlinkY_ByteBox.Size = new System.Drawing.Size(38, 20);
            this.BlinkY_ByteBox.TabIndex = 9;
            this.Help_ToolTip.SetToolTip(this.BlinkY_ByteBox, "The Y coordinate for the blinking eyes sprite on this portrait.\r\nWill write to RO" +
        "M if changed.");
            this.BlinkY_ByteBox.Value = ((byte)(0));
            this.BlinkY_ByteBox.ValueChanged += new System.EventHandler(this.BlinkY_NumBox_Changed);
            // 
            // BlinkX_ByteBox
            // 
            this.BlinkX_ByteBox.Hexadecimal = true;
            this.BlinkX_ByteBox.Location = new System.Drawing.Point(170, 17);
            this.BlinkX_ByteBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.BlinkX_ByteBox.Name = "BlinkX_ByteBox";
            this.BlinkX_ByteBox.Size = new System.Drawing.Size(38, 20);
            this.BlinkX_ByteBox.TabIndex = 8;
            this.Help_ToolTip.SetToolTip(this.BlinkX_ByteBox, "The X coordinate for the blinking eyes sprite on this portrait.\r\nWill write to RO" +
        "M if changed.");
            this.BlinkX_ByteBox.Value = ((byte)(0));
            this.BlinkX_ByteBox.ValueChanged += new System.EventHandler(this.BlinkX_NumBox_Changed);
            // 
            // Edit_GroupBox
            // 
            this.Edit_GroupBox.Controls.Add(this.Placement_GroupBox);
            this.Edit_GroupBox.Controls.Add(this.Palette_PointerBox);
            this.Edit_GroupBox.Controls.Add(this.EyesClosed_CheckBox);
            this.Edit_GroupBox.Controls.Add(this.Palette_PaletteBox);
            this.Edit_GroupBox.Controls.Add(this.Palette_Label);
            this.Edit_GroupBox.Controls.Add(this.Image_ImageBox);
            this.Edit_GroupBox.Controls.Add(this.Mouth_PointerBox);
            this.Edit_GroupBox.Controls.Add(this.Chibi_PointerBox);
            this.Edit_GroupBox.Controls.Add(this.Image_PointerBox);
            this.Edit_GroupBox.Controls.Add(this.Image_Label);
            this.Edit_GroupBox.Controls.Add(this.Mouth_Label);
            this.Edit_GroupBox.Controls.Add(this.Chibi_Label);
            this.Edit_GroupBox.Location = new System.Drawing.Point(12, 59);
            this.Edit_GroupBox.Name = "Edit_GroupBox";
            this.Edit_GroupBox.Size = new System.Drawing.Size(266, 196);
            this.Edit_GroupBox.TabIndex = 0;
            this.Edit_GroupBox.TabStop = false;
            this.Edit_GroupBox.Text = "Edit Portrait Array";
            // 
            // Placement_GroupBox
            // 
            this.Placement_GroupBox.Controls.Add(this.Placement_Eyes_Label);
            this.Placement_GroupBox.Controls.Add(this.Placement_Mouth_Label);
            this.Placement_GroupBox.Controls.Add(this.MouthY_ByteBox);
            this.Placement_GroupBox.Controls.Add(this.MouthX_ByteBox);
            this.Placement_GroupBox.Controls.Add(this.BlinkX_ByteBox);
            this.Placement_GroupBox.Controls.Add(this.BlinkY_ByteBox);
            this.Placement_GroupBox.Location = new System.Drawing.Point(6, 144);
            this.Placement_GroupBox.Name = "Placement_GroupBox";
            this.Placement_GroupBox.Size = new System.Drawing.Size(254, 46);
            this.Placement_GroupBox.TabIndex = 0;
            this.Placement_GroupBox.TabStop = false;
            this.Placement_GroupBox.Text = "Placement";
            // 
            // Placement_Eyes_Label
            // 
            this.Placement_Eyes_Label.AutoSize = true;
            this.Placement_Eyes_Label.Location = new System.Drawing.Point(134, 20);
            this.Placement_Eyes_Label.Name = "Placement_Eyes_Label";
            this.Placement_Eyes_Label.Size = new System.Drawing.Size(36, 13);
            this.Placement_Eyes_Label.TabIndex = 11;
            this.Placement_Eyes_Label.Text = "Eyes :";
            // 
            // Placement_Mouth_Label
            // 
            this.Placement_Mouth_Label.AutoSize = true;
            this.Placement_Mouth_Label.Location = new System.Drawing.Point(6, 19);
            this.Placement_Mouth_Label.Name = "Placement_Mouth_Label";
            this.Placement_Mouth_Label.Size = new System.Drawing.Size(43, 13);
            this.Placement_Mouth_Label.TabIndex = 10;
            this.Placement_Mouth_Label.Text = "Mouth :";
            // 
            // Test_GroupBox
            // 
            this.Test_GroupBox.Controls.Add(this.Test_Mouth_Frown_RadioButton);
            this.Test_GroupBox.Controls.Add(this.Test_Mouth_Smile_RadioButton);
            this.Test_GroupBox.Controls.Add(this.Test_Mouth_TrackBar);
            this.Test_GroupBox.Controls.Add(this.Test_Blink_TrackBar);
            this.Test_GroupBox.Controls.Add(this.Test_Blink_Label);
            this.Test_GroupBox.Controls.Add(this.Test_ImageBox);
            this.Test_GroupBox.Location = new System.Drawing.Point(12, 261);
            this.Test_GroupBox.Name = "Test_GroupBox";
            this.Test_GroupBox.Size = new System.Drawing.Size(266, 109);
            this.Test_GroupBox.TabIndex = 0;
            this.Test_GroupBox.TabStop = false;
            this.Test_GroupBox.Text = "Portrait Test View";
            // 
            // Test_Mouth_Frown_RadioButton
            // 
            this.Test_Mouth_Frown_RadioButton.AutoSize = true;
            this.Test_Mouth_Frown_RadioButton.Location = new System.Drawing.Point(190, 82);
            this.Test_Mouth_Frown_RadioButton.Name = "Test_Mouth_Frown_RadioButton";
            this.Test_Mouth_Frown_RadioButton.Size = new System.Drawing.Size(54, 17);
            this.Test_Mouth_Frown_RadioButton.TabIndex = 14;
            this.Test_Mouth_Frown_RadioButton.Text = "Frown";
            this.Help_ToolTip.SetToolTip(this.Test_Mouth_Frown_RadioButton, "If checked, display the regular mouth frames.");
            this.Test_Mouth_Frown_RadioButton.UseVisualStyleBackColor = true;
            // 
            // Test_Mouth_Smile_RadioButton
            // 
            this.Test_Mouth_Smile_RadioButton.AutoSize = true;
            this.Test_Mouth_Smile_RadioButton.Checked = true;
            this.Test_Mouth_Smile_RadioButton.Location = new System.Drawing.Point(132, 82);
            this.Test_Mouth_Smile_RadioButton.Name = "Test_Mouth_Smile_RadioButton";
            this.Test_Mouth_Smile_RadioButton.Size = new System.Drawing.Size(50, 17);
            this.Test_Mouth_Smile_RadioButton.TabIndex = 13;
            this.Test_Mouth_Smile_RadioButton.TabStop = true;
            this.Test_Mouth_Smile_RadioButton.Text = "Smile";
            this.Help_ToolTip.SetToolTip(this.Test_Mouth_Smile_RadioButton, "If checked, display the smiling mouth frames.");
            this.Test_Mouth_Smile_RadioButton.UseVisualStyleBackColor = true;
            this.Test_Mouth_Smile_RadioButton.CheckedChanged += new System.EventHandler(this.Test_ViewBox_Changed);
            // 
            // Test_Mouth_TrackBar
            // 
            this.Test_Mouth_TrackBar.Location = new System.Drawing.Point(121, 54);
            this.Test_Mouth_TrackBar.Maximum = 3;
            this.Test_Mouth_TrackBar.Name = "Test_Mouth_TrackBar";
            this.Test_Mouth_TrackBar.Size = new System.Drawing.Size(131, 45);
            this.Test_Mouth_TrackBar.TabIndex = 12;
            this.Help_ToolTip.SetToolTip(this.Test_Mouth_TrackBar, "Select a mouth frame to display in the portrait preview (open-to-closed).");
            this.Test_Mouth_TrackBar.ValueChanged += new System.EventHandler(this.Test_ViewBox_Changed);
            // 
            // Test_Blink_TrackBar
            // 
            this.Test_Blink_TrackBar.Location = new System.Drawing.Point(160, 13);
            this.Test_Blink_TrackBar.Maximum = 2;
            this.Test_Blink_TrackBar.Name = "Test_Blink_TrackBar";
            this.Test_Blink_TrackBar.Size = new System.Drawing.Size(92, 45);
            this.Test_Blink_TrackBar.TabIndex = 12;
            this.Help_ToolTip.SetToolTip(this.Test_Blink_TrackBar, "Select which blinking eyes frame to view in the portrait preview.");
            this.Test_Blink_TrackBar.ValueChanged += new System.EventHandler(this.Test_ViewBox_Changed);
            // 
            // Test_Blink_Label
            // 
            this.Test_Blink_Label.AutoSize = true;
            this.Test_Blink_Label.Location = new System.Drawing.Point(118, 19);
            this.Test_Blink_Label.Name = "Test_Blink_Label";
            this.Test_Blink_Label.Size = new System.Drawing.Size(36, 13);
            this.Test_Blink_Label.TabIndex = 0;
            this.Test_Blink_Label.Text = "Blink :";
            // 
            // Editor_Menu
            // 
            this.Editor_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File});
            this.Editor_Menu.Location = new System.Drawing.Point(0, 0);
            this.Editor_Menu.Name = "Editor_Menu";
            this.Editor_Menu.Size = new System.Drawing.Size(290, 24);
            this.Editor_Menu.TabIndex = 2;
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
            this.File_Insert.Size = new System.Drawing.Size(185, 22);
            this.File_Insert.Text = "Insert image...";
            this.File_Insert.Click += new System.EventHandler(this.File_Insert_Click);
            // 
            // File_Save
            // 
            this.File_Save.Name = "File_Save";
            this.File_Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.File_Save.Size = new System.Drawing.Size(185, 22);
            this.File_Save.Text = "Save image...";
            this.File_Save.Click += new System.EventHandler(this.File_Save_Click);
            // 
            // EntryArrayBox
            // 
            this.EntryArrayBox.Location = new System.Drawing.Point(12, 27);
            this.EntryArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.EntryArrayBox.MinimumSize = new System.Drawing.Size(128, 26);
            this.EntryArrayBox.Size = new System.Drawing.Size(266, 26);
            this.EntryArrayBox.TabIndex = 3;
            this.Help_ToolTip.SetToolTip(this.EntryArrayBox, "Select an entry in the portrait array to view/edit.");
            this.EntryArrayBox.ValueChanged += new System.EventHandler(this.EntryArrayBox_ValueChanged);
            // 
            // PortraitEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 382);
            this.Controls.Add(this.EntryArrayBox);
            this.Controls.Add(this.Test_GroupBox);
            this.Controls.Add(this.Edit_GroupBox);
            this.Controls.Add(this.Editor_Menu);
            this.MainMenuStrip = this.Editor_Menu;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(306, 420);
            this.MinimumSize = new System.Drawing.Size(306, 420);
            this.Name = "PortraitEditor";
            this.Text = "Portrait Editor";
            ((System.ComponentModel.ISupportInitialize)(this.Palette_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Mouth_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chibi_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MouthY_ByteBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MouthX_ByteBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BlinkY_ByteBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BlinkX_ByteBox)).EndInit();
            this.Edit_GroupBox.ResumeLayout(false);
            this.Edit_GroupBox.PerformLayout();
            this.Placement_GroupBox.ResumeLayout(false);
            this.Placement_GroupBox.PerformLayout();
            this.Test_GroupBox.ResumeLayout(false);
            this.Test_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Test_Mouth_TrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_Blink_TrackBar)).EndInit();
            this.Editor_Menu.ResumeLayout(false);
            this.Editor_Menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private EmblemMagic.Components.ImageBox Test_ImageBox;
        private EmblemMagic.Components.ImageBox Image_ImageBox;
        private EmblemMagic.Components.PointerBox Palette_PointerBox;
        private EmblemMagic.Components.PointerBox Mouth_PointerBox;
        private EmblemMagic.Components.PointerBox Chibi_PointerBox;
        private EmblemMagic.Components.PointerBox Image_PointerBox;
        private EmblemMagic.Components.PaletteBox Palette_PaletteBox;
        private System.Windows.Forms.Label Image_Label;
        private System.Windows.Forms.Label Chibi_Label;
        private System.Windows.Forms.Label Mouth_Label;
        private System.Windows.Forms.CheckBox EyesClosed_CheckBox;
        private System.Windows.Forms.Label Palette_Label;
        private EmblemMagic.Components.ByteBox MouthY_ByteBox;
        private EmblemMagic.Components.ByteBox MouthX_ByteBox;
        private EmblemMagic.Components.ByteBox BlinkY_ByteBox;
        private EmblemMagic.Components.ByteBox BlinkX_ByteBox;
        private System.Windows.Forms.GroupBox Edit_GroupBox;
        private System.Windows.Forms.GroupBox Placement_GroupBox;
        private System.Windows.Forms.GroupBox Test_GroupBox;
        private System.Windows.Forms.RadioButton Test_Mouth_Frown_RadioButton;
        private System.Windows.Forms.RadioButton Test_Mouth_Smile_RadioButton;
        private System.Windows.Forms.TrackBar Test_Mouth_TrackBar;
        private System.Windows.Forms.TrackBar Test_Blink_TrackBar;
        private System.Windows.Forms.Label Test_Blink_Label;
        private System.Windows.Forms.Label Placement_Eyes_Label;
        private System.Windows.Forms.Label Placement_Mouth_Label;
        private System.Windows.Forms.MenuStrip Editor_Menu;
        private System.Windows.Forms.ToolStripMenuItem Menu_File;
        private System.Windows.Forms.ToolStripMenuItem File_Insert;
        private System.Windows.Forms.ToolStripMenuItem File_Save;
        private Components.ShortArrayBox EntryArrayBox;
    }
}