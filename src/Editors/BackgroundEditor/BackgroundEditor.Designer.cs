namespace EmblemMagic.Editors
{
    partial class BackgroundEditor
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
            this.EntryArrayBox = new EmblemMagic.Components.ByteArrayBox();
            this.Background_PaletteBox = new EmblemMagic.Components.PaletteBox();
            this.Background_ImageBox = new EmblemMagic.Components.ImageBox();
            this.Palette_PointerBox = new EmblemMagic.Components.PointerBox();
            this.Tileset_PointerBox = new EmblemMagic.Components.PointerBox();
            this.TSA_PointerBox = new EmblemMagic.Components.PointerBox();
            this.Palette_Label = new System.Windows.Forms.Label();
            this.Tileset_Label = new System.Windows.Forms.Label();
            this.TSA_Label = new System.Windows.Forms.Label();
            this.DialogArray_RadioButton = new System.Windows.Forms.RadioButton();
            this.BattleArray_RadioButton = new System.Windows.Forms.RadioButton();
            this.ScreenArray_RadioButton = new System.Windows.Forms.RadioButton();
            this.Editor_Menu = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Insert = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tool = new System.Windows.Forms.ToolStripMenuItem();
            this.Tool_OpenPaletteEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.Tool_OpenTSAEditor = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.Palette_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tileset_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TSA_PointerBox)).BeginInit();
            this.Editor_Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // EntryArrayBox
            // 
            this.EntryArrayBox.Location = new System.Drawing.Point(12, 45);
            this.EntryArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.EntryArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.EntryArrayBox.Name = "EntryArrayBox";
            this.EntryArrayBox.Size = new System.Drawing.Size(256, 26);
            this.EntryArrayBox.TabIndex = 0;
            this.EntryArrayBox.ValueChanged += new System.EventHandler(this.EntryArrayBox_ValueChanged);
            // 
            // Background_PaletteBox
            // 
            this.Background_PaletteBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Background_PaletteBox.ColorsPerLine = 16;
            this.Background_PaletteBox.ForeColor = System.Drawing.SystemColors.Control;
            this.Background_PaletteBox.Location = new System.Drawing.Point(12, 241);
            this.Background_PaletteBox.Name = "Background_PaletteBox";
            this.Background_PaletteBox.Size = new System.Drawing.Size(128, 80);
            this.Background_PaletteBox.TabIndex = 5;
            this.Background_PaletteBox.TabStop = false;
            this.Background_PaletteBox.Text = "GBAPaletteBox";
            this.Background_PaletteBox.Click += new System.EventHandler(this.Tool_OpenPaletteEditor_Click);
            // 
            // Background_ImageBox
            // 
            this.Background_ImageBox.BackColor = System.Drawing.SystemColors.Control;
            this.Background_ImageBox.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.Background_ImageBox.Location = new System.Drawing.Point(12, 75);
            this.Background_ImageBox.Name = "Background_ImageBox";
            this.Background_ImageBox.Size = new System.Drawing.Size(256, 160);
            this.Background_ImageBox.TabIndex = 1;
            this.Background_ImageBox.TabStop = false;
            this.Background_ImageBox.Text = "GBAImageBox";
            // 
            // Palette_PointerBox
            // 
            this.Palette_PointerBox.Hexadecimal = true;
            this.Palette_PointerBox.Location = new System.Drawing.Point(198, 241);
            this.Palette_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Palette_PointerBox.Name = "Palette_PointerBox";
            this.Palette_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Palette_PointerBox.TabIndex = 4;
            this.Palette_PointerBox.ValueChanged += new System.EventHandler(this.Palette_PointerBox_ValueChanged);
            // 
            // Tileset_PointerBox
            // 
            this.Tileset_PointerBox.Hexadecimal = true;
            this.Tileset_PointerBox.Location = new System.Drawing.Point(198, 267);
            this.Tileset_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Tileset_PointerBox.Name = "Tileset_PointerBox";
            this.Tileset_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Tileset_PointerBox.TabIndex = 2;
            this.Tileset_PointerBox.ValueChanged += new System.EventHandler(this.Tileset_PointerBox_ValueChanged);
            // 
            // TSA_PointerBox
            // 
            this.TSA_PointerBox.Hexadecimal = true;
            this.TSA_PointerBox.Location = new System.Drawing.Point(198, 293);
            this.TSA_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.TSA_PointerBox.Name = "TSA_PointerBox";
            this.TSA_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.TSA_PointerBox.TabIndex = 3;
            this.TSA_PointerBox.ValueChanged += new System.EventHandler(this.TSA_PointerBox_ValueChanged);
            // 
            // Palette_Label
            // 
            this.Palette_Label.AutoSize = true;
            this.Palette_Label.Location = new System.Drawing.Point(146, 243);
            this.Palette_Label.Name = "Palette_Label";
            this.Palette_Label.Size = new System.Drawing.Size(46, 13);
            this.Palette_Label.TabIndex = 8;
            this.Palette_Label.Text = "Palette :";
            // 
            // Tileset_Label
            // 
            this.Tileset_Label.AutoSize = true;
            this.Tileset_Label.Location = new System.Drawing.Point(148, 269);
            this.Tileset_Label.Name = "Tileset_Label";
            this.Tileset_Label.Size = new System.Drawing.Size(44, 13);
            this.Tileset_Label.TabIndex = 6;
            this.Tileset_Label.Text = "Tileset :";
            // 
            // TSA_Label
            // 
            this.TSA_Label.AutoSize = true;
            this.TSA_Label.Location = new System.Drawing.Point(158, 295);
            this.TSA_Label.Name = "TSA_Label";
            this.TSA_Label.Size = new System.Drawing.Size(34, 13);
            this.TSA_Label.TabIndex = 7;
            this.TSA_Label.Text = "TSA :";
            // 
            // DialogArray_RadioButton
            // 
            this.DialogArray_RadioButton.AutoSize = true;
            this.DialogArray_RadioButton.Checked = true;
            this.DialogArray_RadioButton.Location = new System.Drawing.Point(12, 27);
            this.DialogArray_RadioButton.Name = "DialogArray_RadioButton";
            this.DialogArray_RadioButton.Size = new System.Drawing.Size(74, 17);
            this.DialogArray_RadioButton.TabIndex = 9;
            this.DialogArray_RadioButton.TabStop = true;
            this.DialogArray_RadioButton.Text = "Scene BG";
            this.DialogArray_RadioButton.UseVisualStyleBackColor = true;
            this.DialogArray_RadioButton.Click += new System.EventHandler(this.Array_RadioButton_CheckedChanged);
            // 
            // BattleArray_RadioButton
            // 
            this.BattleArray_RadioButton.AutoSize = true;
            this.BattleArray_RadioButton.Location = new System.Drawing.Point(108, 27);
            this.BattleArray_RadioButton.Name = "BattleArray_RadioButton";
            this.BattleArray_RadioButton.Size = new System.Drawing.Size(70, 17);
            this.BattleArray_RadioButton.TabIndex = 10;
            this.BattleArray_RadioButton.Text = "Battle BG";
            this.BattleArray_RadioButton.UseVisualStyleBackColor = true;
            this.BattleArray_RadioButton.Click += new System.EventHandler(this.Array_RadioButton_CheckedChanged);
            // 
            // ScreenArray_RadioButton
            // 
            this.ScreenArray_RadioButton.AutoSize = true;
            this.ScreenArray_RadioButton.Location = new System.Drawing.Point(198, 27);
            this.ScreenArray_RadioButton.Name = "ScreenArray_RadioButton";
            this.ScreenArray_RadioButton.Size = new System.Drawing.Size(70, 17);
            this.ScreenArray_RadioButton.TabIndex = 11;
            this.ScreenArray_RadioButton.TabStop = true;
            this.ScreenArray_RadioButton.Text = "Cutscene";
            this.ScreenArray_RadioButton.UseVisualStyleBackColor = true;
            this.ScreenArray_RadioButton.Click += new System.EventHandler(this.Array_RadioButton_CheckedChanged);
            // 
            // Editor_Menu
            // 
            this.Editor_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File,
            this.Menu_Tool});
            this.Editor_Menu.Location = new System.Drawing.Point(0, 0);
            this.Editor_Menu.Name = "Editor_Menu";
            this.Editor_Menu.Size = new System.Drawing.Size(280, 24);
            this.Editor_Menu.TabIndex = 15;
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
            this.Tool_OpenTSAEditor.Name = "Tool_OpenTSAEditor";
            this.Tool_OpenTSAEditor.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.Tool_OpenTSAEditor.Size = new System.Drawing.Size(226, 22);
            this.Tool_OpenTSAEditor.Text = "Open TSA Editor...";
            this.Tool_OpenTSAEditor.Click += new System.EventHandler(this.Tool_OpenTSAEditor_Click);
            // 
            // BackgroundEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 327);
            this.Controls.Add(this.ScreenArray_RadioButton);
            this.Controls.Add(this.BattleArray_RadioButton);
            this.Controls.Add(this.DialogArray_RadioButton);
            this.Controls.Add(this.Palette_Label);
            this.Controls.Add(this.TSA_Label);
            this.Controls.Add(this.Tileset_Label);
            this.Controls.Add(this.Background_PaletteBox);
            this.Controls.Add(this.Palette_PointerBox);
            this.Controls.Add(this.TSA_PointerBox);
            this.Controls.Add(this.Tileset_PointerBox);
            this.Controls.Add(this.Background_ImageBox);
            this.Controls.Add(this.EntryArrayBox);
            this.Controls.Add(this.Editor_Menu);
            this.MainMenuStrip = this.Editor_Menu;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(296, 365);
            this.MinimumSize = new System.Drawing.Size(296, 365);
            this.Name = "BackgroundEditor";
            this.Text = "Background Editor";
            ((System.ComponentModel.ISupportInitialize)(this.Palette_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tileset_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TSA_PointerBox)).EndInit();
            this.Editor_Menu.ResumeLayout(false);
            this.Editor_Menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Components.ByteArrayBox EntryArrayBox;
        private Components.PaletteBox Background_PaletteBox;
        private Components.ImageBox Background_ImageBox;
        private Components.PointerBox Palette_PointerBox;
        private Components.PointerBox Tileset_PointerBox;
        private Components.PointerBox TSA_PointerBox;
        private System.Windows.Forms.Label Palette_Label;
        private System.Windows.Forms.Label Tileset_Label;
        private System.Windows.Forms.Label TSA_Label;
        private System.Windows.Forms.RadioButton DialogArray_RadioButton;
        private System.Windows.Forms.RadioButton BattleArray_RadioButton;
        private System.Windows.Forms.RadioButton ScreenArray_RadioButton;
        private System.Windows.Forms.MenuStrip Editor_Menu;
        private System.Windows.Forms.ToolStripMenuItem Menu_File;
        private System.Windows.Forms.ToolStripMenuItem File_Insert;
        private System.Windows.Forms.ToolStripMenuItem File_Save;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tool;
        private System.Windows.Forms.ToolStripMenuItem Tool_OpenTSAEditor;
        private System.Windows.Forms.ToolStripMenuItem Tool_OpenPaletteEditor;
    }
}