using System;
using System.Windows.Forms;

namespace EmblemMagic.Editors
{
    partial class BattleScreenEditor
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
            this.EntryArrayBox = new Magic.Components.ByteArrayBox();
            this.Platform_ImageBox = new Magic.Components.ImageBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Platform_Tileset_PointerBox = new Magic.Components.PointerBox();
            this.Platform_Palette_PointerBox = new Magic.Components.PointerBox();
            this.Platform_Name_TextBox = new System.Windows.Forms.TextBox();
            this.Platform_PaletteBox = new Magic.Components.PaletteBox();
            this.Platform_GroupBox = new System.Windows.Forms.GroupBox();
            this.Editor_Menu = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Insert_Screen = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Insert_Platform = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.File_Save_Screen = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Save_Platform = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tools = new System.Windows.Forms.ToolStripMenuItem();
            this.Tool_OpenPalette_Screen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.Tool_OpenPalette_Platform = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_View = new System.Windows.Forms.ToolStripMenuItem();
            this.View_ShowGrid = new System.Windows.Forms.ToolStripMenuItem();
            this.Screen_GridBox = new Magic.Components.GridBox();
            this.Screen_GroupBox = new System.Windows.Forms.GroupBox();
            this.Screen_FlipV_CheckBox = new System.Windows.Forms.CheckBox();
            this.Screen_PaletteBox = new Magic.Components.PaletteBox();
            this.Screen_FlipH_CheckBox = new System.Windows.Forms.CheckBox();
            this.Screen_TileIndex_NumBox = new System.Windows.Forms.NumericUpDown();
            this.Screen_Palette_NumBox = new System.Windows.Forms.NumericUpDown();
            this.Screen_Palette_Label = new System.Windows.Forms.Label();
            this.Screen_Tile_Label = new System.Windows.Forms.Label();
            this.Screen_MagicButton = new Magic.Components.MagicButton(App);
            this.Platform_MagicButton = new Magic.Components.MagicButton(App);
            ((System.ComponentModel.ISupportInitialize)(this.Platform_Tileset_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Platform_Palette_PointerBox)).BeginInit();
            this.Platform_GroupBox.SuspendLayout();
            this.Editor_Menu.SuspendLayout();
            this.Screen_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Screen_TileIndex_NumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Screen_Palette_NumBox)).BeginInit();
            this.SuspendLayout();
            // 
            // EntryArrayBox
            // 
            this.EntryArrayBox.Location = new System.Drawing.Point(6, 19);
            this.EntryArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.EntryArrayBox.MinimumSize = new System.Drawing.Size(128, 26);
            this.EntryArrayBox.Size = new System.Drawing.Size(214, 26);
            this.EntryArrayBox.TabIndex = 0;
            this.Help_ToolTip.SetToolTip(this.EntryArrayBox, "Select en entry in the list of battle platforms to view/edit.");
            this.EntryArrayBox.ValueChanged += new System.EventHandler(this.EntryArrayBox_ValueChanged);
            // 
            // Platform_ImageBox
            // 
            this.Platform_ImageBox.BackColor = System.Drawing.SystemColors.Control;
            this.Platform_ImageBox.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.Platform_ImageBox.Location = new System.Drawing.Point(146, 54);
            this.Platform_ImageBox.Name = "Platform_ImageBox";
            this.Platform_ImageBox.Size = new System.Drawing.Size(104, 80);
            this.Platform_ImageBox.TabIndex = 1;
            this.Platform_ImageBox.TabStop = false;
            this.Platform_ImageBox.Text = "imageBox1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Tileset :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Palette :";
            // 
            // Platform_Tileset_PointerBox
            // 
            this.Platform_Tileset_PointerBox.Hexadecimal = true;
            this.Platform_Tileset_PointerBox.Location = new System.Drawing.Point(70, 74);
            this.Platform_Tileset_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Platform_Tileset_PointerBox.Name = "Platform_Tileset_PointerBox";
            this.Platform_Tileset_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Platform_Tileset_PointerBox.TabIndex = 6;
            this.Help_ToolTip.SetToolTip(this.Platform_Tileset_PointerBox, "Pointer to the tileset pixel data for the currently selected battle platform.\r\nWi" +
        "ll write to ROM if changed. Is repointed when inserting an image with \"File -> I" +
        "nsert battle platform...\".");
            this.Platform_Tileset_PointerBox.ValueChanged += new System.EventHandler(this.Platform_Tileset_PointerBox_ValueChanged);
            // 
            // Platform_Palette_PointerBox
            // 
            this.Platform_Palette_PointerBox.Hexadecimal = true;
            this.Platform_Palette_PointerBox.Location = new System.Drawing.Point(70, 100);
            this.Platform_Palette_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Platform_Palette_PointerBox.Name = "Platform_Palette_PointerBox";
            this.Platform_Palette_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Platform_Palette_PointerBox.TabIndex = 7;
            this.Help_ToolTip.SetToolTip(this.Platform_Palette_PointerBox, "Pointer to the palette for the currently selected battle platform sprite.\r\nWill w" +
        "rite to ROM if changed. Is repointed when inserting an image with \"File -> Inser" +
        "t battle platform...\".");
            this.Platform_Palette_PointerBox.ValueChanged += new System.EventHandler(this.Platform_Palette_PointerBox_ValueChanged);
            // 
            // Platform_Name_TextBox
            // 
            this.Platform_Name_TextBox.Font = new System.Drawing.Font("Consolas", 8F);
            this.Platform_Name_TextBox.Location = new System.Drawing.Point(53, 48);
            this.Platform_Name_TextBox.Name = "Platform_Name_TextBox";
            this.Platform_Name_TextBox.Size = new System.Drawing.Size(87, 20);
            this.Platform_Name_TextBox.TabIndex = 8;
            this.Help_ToolTip.SetToolTip(this.Platform_Name_TextBox, "The name of this battle platform (serves no purpose).\r\nWill write to ROM if chang" +
        "ed. Max length is 12 characters.");
            this.Platform_Name_TextBox.TextChanged += new System.EventHandler(this.Platform_Name_TextBox_TextChanged);
            // 
            // Platform_PaletteBox
            // 
            this.Platform_PaletteBox.ColorsPerLine = 16;
            this.Platform_PaletteBox.Location = new System.Drawing.Point(12, 126);
            this.Platform_PaletteBox.Name = "Platform_PaletteBox";
            this.Platform_PaletteBox.Size = new System.Drawing.Size(128, 8);
            this.Platform_PaletteBox.TabIndex = 9;
            this.Platform_PaletteBox.TabStop = false;
            this.Platform_PaletteBox.Text = "paletteBox1";
            this.Help_ToolTip.SetToolTip(this.Platform_PaletteBox, "The palette for the currently selected battle platform sprite.\r\nClicking in this " +
        "will open an appropriate PaletteEditor, to modify this palette.");
            this.Platform_PaletteBox.Click += new System.EventHandler(this.Tool_OpenPalette_Platform_Click);
            // 
            // Platform_GroupBox
            // 
            this.Platform_GroupBox.Controls.Add(this.Platform_MagicButton);
            this.Platform_GroupBox.Controls.Add(this.EntryArrayBox);
            this.Platform_GroupBox.Controls.Add(this.Platform_PaletteBox);
            this.Platform_GroupBox.Controls.Add(this.Platform_ImageBox);
            this.Platform_GroupBox.Controls.Add(this.Platform_Name_TextBox);
            this.Platform_GroupBox.Controls.Add(this.label1);
            this.Platform_GroupBox.Controls.Add(this.Platform_Palette_PointerBox);
            this.Platform_GroupBox.Controls.Add(this.label2);
            this.Platform_GroupBox.Controls.Add(this.Platform_Tileset_PointerBox);
            this.Platform_GroupBox.Controls.Add(this.label3);
            this.Platform_GroupBox.Location = new System.Drawing.Point(12, 223);
            this.Platform_GroupBox.Name = "Platform_GroupBox";
            this.Platform_GroupBox.Size = new System.Drawing.Size(256, 140);
            this.Platform_GroupBox.TabIndex = 10;
            this.Platform_GroupBox.TabStop = false;
            this.Platform_GroupBox.Text = "Battle Platforms";
            // 
            // Editor_Menu
            // 
            this.Editor_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File,
            this.Menu_Tools,
            this.Menu_View});
            this.Editor_Menu.Location = new System.Drawing.Point(0, 0);
            this.Editor_Menu.Name = "Editor_Menu";
            this.Editor_Menu.Size = new System.Drawing.Size(280, 24);
            this.Editor_Menu.TabIndex = 11;
            this.Editor_Menu.Text = "menuStrip1";
            // 
            // Menu_File
            // 
            this.Menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File_Insert_Screen,
            this.File_Insert_Platform,
            this.toolStripSeparator1,
            this.File_Save_Screen,
            this.File_Save_Platform});
            this.Menu_File.Name = "Menu_File";
            this.Menu_File.Size = new System.Drawing.Size(37, 20);
            this.Menu_File.Text = "File";
            // 
            // File_Insert_Screen
            // 
            this.File_Insert_Screen.Name = "File_Insert_Screen";
            this.File_Insert_Screen.Size = new System.Drawing.Size(219, 22);
            this.File_Insert_Screen.Text = "Insert Battle Screen Frame...";
            this.File_Insert_Screen.Click += new System.EventHandler(this.File_Insert_Screen_Click);
            // 
            // File_Insert_Platform
            // 
            this.File_Insert_Platform.Name = "File_Insert_Platform";
            this.File_Insert_Platform.Size = new System.Drawing.Size(219, 22);
            this.File_Insert_Platform.Text = "Insert Battle Platform...";
            this.File_Insert_Platform.Click += new System.EventHandler(this.File_Insert_Platform_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(216, 6);
            // 
            // File_Save_Screen
            // 
            this.File_Save_Screen.Name = "File_Save_Screen";
            this.File_Save_Screen.Size = new System.Drawing.Size(219, 22);
            this.File_Save_Screen.Text = "Save Battle Screen Frame...";
            this.File_Save_Screen.Click += new System.EventHandler(this.File_Save_Screen_Click);
            // 
            // File_Save_Platform
            // 
            this.File_Save_Platform.Name = "File_Save_Platform";
            this.File_Save_Platform.Size = new System.Drawing.Size(219, 22);
            this.File_Save_Platform.Text = "Save Battle Platform...";
            this.File_Save_Platform.Click += new System.EventHandler(this.File_Save_Platform_Click);
            // 
            // Menu_Tools
            // 
            this.Menu_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Tool_OpenPalette_Screen,
            this.toolStripSeparator2,
            this.Tool_OpenPalette_Platform});
            this.Menu_Tools.Name = "Menu_Tools";
            this.Menu_Tools.Size = new System.Drawing.Size(48, 20);
            this.Menu_Tools.Text = "Tools";
            // 
            // Tool_OpenPalette_Screen
            // 
            this.Tool_OpenPalette_Screen.Name = "Tool_OpenPalette_Screen";
            this.Tool_OpenPalette_Screen.Size = new System.Drawing.Size(292, 22);
            this.Tool_OpenPalette_Screen.Text = "Open Battle Screen Frame Palette Editor...";
            this.Tool_OpenPalette_Screen.Click += new System.EventHandler(this.Tool_OpenPalette_Screen_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(289, 6);
            // 
            // Tool_OpenPalette_Platform
            // 
            this.Tool_OpenPalette_Platform.Name = "Tool_OpenPalette_Platform";
            this.Tool_OpenPalette_Platform.Size = new System.Drawing.Size(292, 22);
            this.Tool_OpenPalette_Platform.Text = "Open Battle Platform Palette Editor...";
            this.Tool_OpenPalette_Platform.Click += new System.EventHandler(this.Tool_OpenPalette_Platform_Click);
            // 
            // Menu_View
            // 
            this.Menu_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.View_ShowGrid});
            this.Menu_View.Name = "Menu_View";
            this.Menu_View.Size = new System.Drawing.Size(44, 20);
            this.Menu_View.Text = "View";
            // 
            // View_ShowGrid
            // 
            this.View_ShowGrid.CheckOnClick = true;
            this.View_ShowGrid.Name = "View_ShowGrid";
            this.View_ShowGrid.Size = new System.Drawing.Size(223, 22);
            this.View_ShowGrid.Text = "Show Battle Screen TSA Grid";
            this.View_ShowGrid.Click += new System.EventHandler(this.View_ShowGrid_Click);
            // 
            // Screen_GridBox
            // 
            this.Screen_GridBox.Location = new System.Drawing.Point(12, 27);
            this.Screen_GridBox.Name = "Screen_GridBox";
            this.Screen_GridBox.Selection = null;
            this.Screen_GridBox.ShowGrid = false;
            this.Screen_GridBox.Size = new System.Drawing.Size(256, 104);
            this.Screen_GridBox.TabIndex = 10;
            this.Screen_GridBox.TabStop = false;
            this.Screen_GridBox.Text = "Screen_GridBox";
            this.Screen_GridBox.TileSize = 8;
            this.Help_ToolTip.SetToolTip(this.Screen_GridBox, "This ImageBox displays the battle screen frame.\r\nYou can click on this to select " +
        "one or several tiles (with CTRL & SHIFT), and edit the TSA array data for these " +
        "tiles.");
            this.Screen_GridBox.SelectionChanged += new System.EventHandler(this.Screen_GridBox_SelectionChanged);
            // 
            // Screen_GroupBox
            // 
            this.Screen_GroupBox.Controls.Add(this.Screen_MagicButton);
            this.Screen_GroupBox.Controls.Add(this.Screen_FlipV_CheckBox);
            this.Screen_GroupBox.Controls.Add(this.Screen_PaletteBox);
            this.Screen_GroupBox.Controls.Add(this.Screen_FlipH_CheckBox);
            this.Screen_GroupBox.Controls.Add(this.Screen_TileIndex_NumBox);
            this.Screen_GroupBox.Controls.Add(this.Screen_Palette_NumBox);
            this.Screen_GroupBox.Controls.Add(this.Screen_Palette_Label);
            this.Screen_GroupBox.Controls.Add(this.Screen_Tile_Label);
            this.Screen_GroupBox.Location = new System.Drawing.Point(12, 137);
            this.Screen_GroupBox.Name = "Screen_GroupBox";
            this.Screen_GroupBox.Size = new System.Drawing.Size(256, 80);
            this.Screen_GroupBox.TabIndex = 12;
            this.Screen_GroupBox.TabStop = false;
            this.Screen_GroupBox.Text = "Battle Screen Frame";
            // 
            // Screen_FlipV_CheckBox
            // 
            this.Screen_FlipV_CheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Screen_FlipV_CheckBox.AutoSize = true;
            this.Screen_FlipV_CheckBox.Location = new System.Drawing.Point(6, 57);
            this.Screen_FlipV_CheckBox.Name = "Screen_FlipV_CheckBox";
            this.Screen_FlipV_CheckBox.Size = new System.Drawing.Size(80, 17);
            this.Screen_FlipV_CheckBox.TabIndex = 7;
            this.Screen_FlipV_CheckBox.Text = "Flip Vertical";
            this.Help_ToolTip.SetToolTip(this.Screen_FlipV_CheckBox, "Whether or not the currently selected tile is flipped horizontally or not.\r\nWill " +
        "write to ROM if changed.");
            this.Screen_FlipV_CheckBox.UseVisualStyleBackColor = true;
            this.Screen_FlipV_CheckBox.CheckedChanged += new System.EventHandler(this.Screen_FlipV_CheckBox_CheckedChanged);
            // 
            // Screen_PaletteBox
            // 
            this.Screen_PaletteBox.ColorsPerLine = 16;
            this.Screen_PaletteBox.Location = new System.Drawing.Point(100, 42);
            this.Screen_PaletteBox.Name = "Screen_PaletteBox";
            this.Screen_PaletteBox.Size = new System.Drawing.Size(128, 32);
            this.Screen_PaletteBox.TabIndex = 8;
            this.Screen_PaletteBox.TabStop = false;
            this.Screen_PaletteBox.Text = "paletteBox1";
            this.Help_ToolTip.SetToolTip(this.Screen_PaletteBox, "The palettes used by the battle screen frame.\r\nClicking on this will open a Palet" +
        "teEditor, to modify these palettes.");
            this.Screen_PaletteBox.Click += new System.EventHandler(this.Tool_OpenPalette_Screen_Click);
            // 
            // Screen_FlipH_CheckBox
            // 
            this.Screen_FlipH_CheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Screen_FlipH_CheckBox.AutoSize = true;
            this.Screen_FlipH_CheckBox.Location = new System.Drawing.Point(6, 41);
            this.Screen_FlipH_CheckBox.Name = "Screen_FlipH_CheckBox";
            this.Screen_FlipH_CheckBox.Size = new System.Drawing.Size(92, 17);
            this.Screen_FlipH_CheckBox.TabIndex = 6;
            this.Screen_FlipH_CheckBox.Text = "Flip Horizontal";
            this.Help_ToolTip.SetToolTip(this.Screen_FlipH_CheckBox, "Whether or not the currently selected tile is flipped horizontally or not.\r\nWill " +
        "write to ROM if changed.");
            this.Screen_FlipH_CheckBox.UseVisualStyleBackColor = true;
            this.Screen_FlipH_CheckBox.CheckedChanged += new System.EventHandler(this.Screen_FlipH_CheckBox_CheckedChanged);
            // 
            // Screen_TileIndex_NumBox
            // 
            this.Screen_TileIndex_NumBox.Location = new System.Drawing.Point(70, 15);
            this.Screen_TileIndex_NumBox.Maximum = new decimal(new int[] {
            1023,
            0,
            0,
            0});
            this.Screen_TileIndex_NumBox.Name = "Screen_TileIndex_NumBox";
            this.Screen_TileIndex_NumBox.Size = new System.Drawing.Size(57, 20);
            this.Screen_TileIndex_NumBox.TabIndex = 4;
            this.Help_ToolTip.SetToolTip(this.Screen_TileIndex_NumBox, "The TileID of the currently selected TSA square in the above display.\r\nWill write" +
        " to ROM if changed.");
            this.Screen_TileIndex_NumBox.ValueChanged += new System.EventHandler(this.Screen_Tile_NumBox_ValueChanged);
            // 
            // Screen_Palette_NumBox
            // 
            this.Screen_Palette_NumBox.Location = new System.Drawing.Point(193, 15);
            this.Screen_Palette_NumBox.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.Screen_Palette_NumBox.Name = "Screen_Palette_NumBox";
            this.Screen_Palette_NumBox.Size = new System.Drawing.Size(57, 20);
            this.Screen_Palette_NumBox.TabIndex = 2;
            this.Help_ToolTip.SetToolTip(this.Screen_Palette_NumBox, "The palette index for the currently selected TSA tile.\r\nWill write to ROM if chan" +
        "ged.");
            this.Screen_Palette_NumBox.ValueChanged += new System.EventHandler(this.Screen_Palette_NumBox_ValueChanged);
            // 
            // Screen_Palette_Label
            // 
            this.Screen_Palette_Label.AutoSize = true;
            this.Screen_Palette_Label.Location = new System.Drawing.Point(141, 17);
            this.Screen_Palette_Label.Name = "Screen_Palette_Label";
            this.Screen_Palette_Label.Size = new System.Drawing.Size(46, 13);
            this.Screen_Palette_Label.TabIndex = 3;
            this.Screen_Palette_Label.Text = "Palette :";
            // 
            // Screen_Tile_Label
            // 
            this.Screen_Tile_Label.AutoSize = true;
            this.Screen_Tile_Label.Location = new System.Drawing.Point(8, 17);
            this.Screen_Tile_Label.Name = "Screen_Tile_Label";
            this.Screen_Tile_Label.Size = new System.Drawing.Size(58, 13);
            this.Screen_Tile_Label.TabIndex = 5;
            this.Screen_Tile_Label.Text = "Tile index :";
            // 
            // Screen_MagicButton
            // 
            this.Screen_MagicButton.Location = new System.Drawing.Point(232, 42);
            this.Screen_MagicButton.MaximumSize = new System.Drawing.Size(24, 24);
            this.Screen_MagicButton.MinimumSize = new System.Drawing.Size(24, 24);
            this.Screen_MagicButton.Name = "Screen_MagicButton";
            this.Screen_MagicButton.Size = new System.Drawing.Size(24, 24);
            this.Screen_MagicButton.TabIndex = 9;
            this.Help_ToolTip.SetToolTip(this.Screen_MagicButton, "Clicking on this button opens the GraphicsEditor, to view the raw graphics data o" +
        "f the battle screen frame.");
            this.Screen_MagicButton.UseVisualStyleBackColor = true;
            this.Screen_MagicButton.Click += new System.EventHandler(this.Screen_MagicButton_Click);
            // 
            // Platform_MagicButton
            // 
            this.Platform_MagicButton.Location = new System.Drawing.Point(226, 18);
            this.Platform_MagicButton.MaximumSize = new System.Drawing.Size(24, 24);
            this.Platform_MagicButton.MinimumSize = new System.Drawing.Size(24, 24);
            this.Platform_MagicButton.Name = "Platform_MagicButton";
            this.Platform_MagicButton.Size = new System.Drawing.Size(24, 24);
            this.Platform_MagicButton.TabIndex = 10;
            this.Help_ToolTip.SetToolTip(this.Platform_MagicButton, "Clicking on this button opens the GraphicsEditor, to view the raw graphics data o" +
        "f the battle screen frame.");
            this.Platform_MagicButton.UseVisualStyleBackColor = true;
            this.Platform_MagicButton.Click += new System.EventHandler(this.Platform_MagicButton_Click);
            // 
            // BattleScreenEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 374);
            this.Controls.Add(this.Screen_GroupBox);
            this.Controls.Add(this.Screen_GridBox);
            this.Controls.Add(this.Platform_GroupBox);
            this.Controls.Add(this.Editor_Menu);
            this.MainMenuStrip = this.Editor_Menu;
            this.MaximumSize = new System.Drawing.Size(296, 412);
            this.MinimumSize = new System.Drawing.Size(296, 412);
            this.Name = "BattleScreenEditor";
            this.Text = "Battle Screen Editor";
            ((System.ComponentModel.ISupportInitialize)(this.Platform_Tileset_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Platform_Palette_PointerBox)).EndInit();
            this.Platform_GroupBox.ResumeLayout(false);
            this.Platform_GroupBox.PerformLayout();
            this.Editor_Menu.ResumeLayout(false);
            this.Editor_Menu.PerformLayout();
            this.Screen_GroupBox.ResumeLayout(false);
            this.Screen_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Screen_TileIndex_NumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Screen_Palette_NumBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Magic.Components.ByteArrayBox EntryArrayBox;
        private Magic.Components.ImageBox Platform_ImageBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Magic.Components.PointerBox Platform_Tileset_PointerBox;
        private Magic.Components.PointerBox Platform_Palette_PointerBox;
        private System.Windows.Forms.TextBox Platform_Name_TextBox;
        private Magic.Components.PaletteBox Platform_PaletteBox;
        private GroupBox Platform_GroupBox;
        private MenuStrip Editor_Menu;
        private ToolStripMenuItem Menu_File;
        private ToolStripMenuItem File_Insert_Screen;
        private Magic.Components.GridBox Screen_GridBox;
        private GroupBox Screen_GroupBox;
        private NumericUpDown Screen_TileIndex_NumBox;
        private CheckBox Screen_FlipV_CheckBox;
        private NumericUpDown Screen_Palette_NumBox;
        private CheckBox Screen_FlipH_CheckBox;
        private Label Screen_Palette_Label;
        private Label Screen_Tile_Label;
        private ToolStripMenuItem Menu_View;
        private ToolStripMenuItem View_ShowGrid;
        private ToolStripMenuItem Menu_Tools;
        private ToolStripMenuItem Tool_OpenPalette_Platform;
        private ToolStripMenuItem Tool_OpenPalette_Screen;
        private Magic.Components.PaletteBox Screen_PaletteBox;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem File_Save_Screen;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem File_Insert_Platform;
        private ToolStripMenuItem File_Save_Platform;
        private Magic.Components.MagicButton Platform_MagicButton;
        private Magic.Components.MagicButton Screen_MagicButton;
    }
}