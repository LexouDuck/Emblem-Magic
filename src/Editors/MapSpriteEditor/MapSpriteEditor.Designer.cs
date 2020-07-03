namespace EmblemMagic.Editors
{
    partial class MapSpriteEditor
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
            this.EditGroupBox = new System.Windows.Forms.GroupBox();
            this.Edit_ImageBox = new EmblemMagic.Components.ImageBox();
            this.UnkownLabel = new System.Windows.Forms.Label();
            this.IdlePointerBox = new EmblemMagic.Components.PointerBox();
            this.SizeLabel = new System.Windows.Forms.Label();
            this.WalkPointerBox = new EmblemMagic.Components.PointerBox();
            this.AnimPointerLabel = new System.Windows.Forms.Label();
            this.AnimPointerBox = new EmblemMagic.Components.PointerBox();
            this.WalkPointerLabel = new System.Windows.Forms.Label();
            this.SizeComboBox = new System.Windows.Forms.ComboBox();
            this.IdlePointerLabel = new System.Windows.Forms.Label();
            this.UnknownNumberBox = new EmblemMagic.Components.ByteBox();
            this.IdleEntryLabel = new System.Windows.Forms.Label();
            this.WalkEntryLabel = new System.Windows.Forms.Label();
            this.IdleEntryArrayBox = new EmblemMagic.Components.ByteArrayBox();
            this.WalkEntryArrayBox = new EmblemMagic.Components.ByteArrayBox();
            this.TestGroupBox = new System.Windows.Forms.GroupBox();
            this.Test_Selected = new System.Windows.Forms.RadioButton();
            this.Test_Idle = new System.Windows.Forms.RadioButton();
            this.PaletteArrayBox = new EmblemMagic.Components.ByteArrayBox();
            this.Test_WalkSide = new System.Windows.Forms.RadioButton();
            this.Test_WalkUp = new System.Windows.Forms.RadioButton();
            this.Test_WalkDown = new System.Windows.Forms.RadioButton();
            this.Test_PaletteBox = new EmblemMagic.Components.PaletteBox();
            this.Test_TrackBar = new System.Windows.Forms.TrackBar();
            this.Test_ImageBox = new EmblemMagic.Components.ImageBox();
            this.Editor_Menu = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Insert = new System.Windows.Forms.ToolStripMenuItem();
            this.File_SaveData = new System.Windows.Forms.ToolStripMenuItem();
            this.EditGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IdlePointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WalkPointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnimPointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnknownNumberBox)).BeginInit();
            this.TestGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Test_TrackBar)).BeginInit();
            this.Editor_Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // EditGroupBox
            // 
            this.EditGroupBox.Controls.Add(this.Edit_ImageBox);
            this.EditGroupBox.Controls.Add(this.UnkownLabel);
            this.EditGroupBox.Controls.Add(this.IdlePointerBox);
            this.EditGroupBox.Controls.Add(this.SizeLabel);
            this.EditGroupBox.Controls.Add(this.WalkPointerBox);
            this.EditGroupBox.Controls.Add(this.AnimPointerLabel);
            this.EditGroupBox.Controls.Add(this.AnimPointerBox);
            this.EditGroupBox.Controls.Add(this.WalkPointerLabel);
            this.EditGroupBox.Controls.Add(this.SizeComboBox);
            this.EditGroupBox.Controls.Add(this.IdlePointerLabel);
            this.EditGroupBox.Controls.Add(this.UnknownNumberBox);
            this.EditGroupBox.Location = new System.Drawing.Point(12, 91);
            this.EditGroupBox.Name = "EditGroupBox";
            this.EditGroupBox.Size = new System.Drawing.Size(310, 153);
            this.EditGroupBox.TabIndex = 13;
            this.EditGroupBox.TabStop = false;
            this.EditGroupBox.Text = "Edit Map Sprite Array";
            // 
            // Edit_ImageBox
            // 
            this.Edit_ImageBox.BackColor = System.Drawing.SystemColors.Control;
            this.Edit_ImageBox.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.Edit_ImageBox.Location = new System.Drawing.Point(144, 19);
            this.Edit_ImageBox.Name = "Edit_ImageBox";
            this.Edit_ImageBox.Size = new System.Drawing.Size(160, 128);
            this.Edit_ImageBox.TabIndex = 0;
            this.Edit_ImageBox.TabStop = false;
            this.Edit_ImageBox.Text = "imageBox1";
            // 
            // UnkownLabel
            // 
            this.UnkownLabel.AutoSize = true;
            this.UnkownLabel.Location = new System.Drawing.Point(22, 128);
            this.UnkownLabel.Name = "UnkownLabel";
            this.UnkownLabel.Size = new System.Drawing.Size(59, 13);
            this.UnkownLabel.TabIndex = 12;
            this.UnkownLabel.Text = "Unknown :";
            // 
            // IdlePointerBox
            // 
            this.IdlePointerBox.Hexadecimal = true;
            this.IdlePointerBox.Location = new System.Drawing.Point(68, 45);
            this.IdlePointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.IdlePointerBox.Name = "IdlePointerBox";
            this.IdlePointerBox.Size = new System.Drawing.Size(70, 20);
            this.IdlePointerBox.TabIndex = 1;
            this.Help_ToolTip.SetToolTip(this.IdlePointerBox, "Pointer to the pixel data for the current \"idle\" map sprite.\r\nWill write to ROM i" +
        "f changed. Is repointed when inserting a new map sprite with \"File -> Insert ima" +
        "ge...\".");
            // 
            // SizeLabel
            // 
            this.SizeLabel.AutoSize = true;
            this.SizeLabel.Location = new System.Drawing.Point(29, 22);
            this.SizeLabel.Name = "SizeLabel";
            this.SizeLabel.Size = new System.Drawing.Size(33, 13);
            this.SizeLabel.TabIndex = 11;
            this.SizeLabel.Text = "Size :";
            // 
            // WalkPointerBox
            // 
            this.WalkPointerBox.Hexadecimal = true;
            this.WalkPointerBox.Location = new System.Drawing.Point(68, 72);
            this.WalkPointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.WalkPointerBox.Name = "WalkPointerBox";
            this.WalkPointerBox.Size = new System.Drawing.Size(70, 20);
            this.WalkPointerBox.TabIndex = 2;
            this.Help_ToolTip.SetToolTip(this.WalkPointerBox, "Pointer to the pixel data for the current \"move\" map sprite.\r\nWill write to ROM i" +
        "f changed. Is repointed when inserting a new map sprite with \"File -> Insert ima" +
        "ge...\".");
            // 
            // AnimPointerLabel
            // 
            this.AnimPointerLabel.AutoSize = true;
            this.AnimPointerLabel.Location = new System.Drawing.Point(3, 101);
            this.AnimPointerLabel.Name = "AnimPointerLabel";
            this.AnimPointerLabel.Size = new System.Drawing.Size(59, 13);
            this.AnimPointerLabel.TabIndex = 10;
            this.AnimPointerLabel.Text = "Animation :";
            // 
            // AnimPointerBox
            // 
            this.AnimPointerBox.Hexadecimal = true;
            this.AnimPointerBox.Location = new System.Drawing.Point(68, 99);
            this.AnimPointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.AnimPointerBox.Name = "AnimPointerBox";
            this.AnimPointerBox.Size = new System.Drawing.Size(70, 20);
            this.AnimPointerBox.TabIndex = 3;
            this.Help_ToolTip.SetToolTip(this.AnimPointerBox, "Pointer to the animation data for the current \"idle\" map sprite.\r\nWill write to R" +
        "OM if changed.");
            // 
            // WalkPointerLabel
            // 
            this.WalkPointerLabel.AutoSize = true;
            this.WalkPointerLabel.Location = new System.Drawing.Point(24, 74);
            this.WalkPointerLabel.Name = "WalkPointerLabel";
            this.WalkPointerLabel.Size = new System.Drawing.Size(38, 13);
            this.WalkPointerLabel.TabIndex = 9;
            this.WalkPointerLabel.Text = "Walk :";
            // 
            // SizeComboBox
            // 
            this.SizeComboBox.Location = new System.Drawing.Point(68, 19);
            this.SizeComboBox.Name = "SizeComboBox";
            this.SizeComboBox.Size = new System.Drawing.Size(70, 21);
            this.SizeComboBox.TabIndex = 5;
            this.Help_ToolTip.SetToolTip(this.SizeComboBox, "This dropdown selects the size of the current \"idle\" map sprite.\r\nWill write to R" +
        "OM if changed. Is changed automatically when inserting a new map sprite with \"Fi" +
        "le -> Insert image...\".");
            // 
            // IdlePointerLabel
            // 
            this.IdlePointerLabel.AutoSize = true;
            this.IdlePointerLabel.Location = new System.Drawing.Point(32, 47);
            this.IdlePointerLabel.Name = "IdlePointerLabel";
            this.IdlePointerLabel.Size = new System.Drawing.Size(30, 13);
            this.IdlePointerLabel.TabIndex = 8;
            this.IdlePointerLabel.Text = "Idle :";
            // 
            // UnknownNumberBox
            // 
            this.UnknownNumberBox.Hexadecimal = true;
            this.UnknownNumberBox.Location = new System.Drawing.Point(88, 126);
            this.UnknownNumberBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.UnknownNumberBox.Name = "UnknownNumberBox";
            this.UnknownNumberBox.Size = new System.Drawing.Size(50, 20);
            this.UnknownNumberBox.TabIndex = 6;
            this.Help_ToolTip.SetToolTip(this.UnknownNumberBox, "An unkown number within each map sprite struct.\r\nWill write to ROM if changed.");
            this.UnknownNumberBox.Value = ((byte)(0));
            this.UnknownNumberBox.ValueChanged += new System.EventHandler(this.Unknown_NumBox_ValueChanged);
            // 
            // IdleEntryLabel
            // 
            this.IdleEntryLabel.AutoSize = true;
            this.IdleEntryLabel.Location = new System.Drawing.Point(12, 31);
            this.IdleEntryLabel.Name = "IdleEntryLabel";
            this.IdleEntryLabel.Size = new System.Drawing.Size(57, 13);
            this.IdleEntryLabel.TabIndex = 7;
            this.IdleEntryLabel.Text = "Idle Entry :";
            // 
            // WalkEntryLabel
            // 
            this.WalkEntryLabel.AutoSize = true;
            this.WalkEntryLabel.Location = new System.Drawing.Point(5, 62);
            this.WalkEntryLabel.Name = "WalkEntryLabel";
            this.WalkEntryLabel.Size = new System.Drawing.Size(65, 13);
            this.WalkEntryLabel.TabIndex = 16;
            this.WalkEntryLabel.Text = "Walk Entry :";
            // 
            // IdleEntryArrayBox
            // 
            this.IdleEntryArrayBox.Location = new System.Drawing.Point(73, 27);
            this.IdleEntryArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.IdleEntryArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.IdleEntryArrayBox.Size = new System.Drawing.Size(249, 26);
            this.IdleEntryArrayBox.TabIndex = 4;
            this.Help_ToolTip.SetToolTip(this.IdleEntryArrayBox, "Select an \"idle\" map sprite to view/edit.");
            this.IdleEntryArrayBox.ValueChanged += new System.EventHandler(this.EntryArrayBox_ValueChanged);
            // 
            // WalkEntryArrayBox
            // 
            this.WalkEntryArrayBox.Location = new System.Drawing.Point(73, 59);
            this.WalkEntryArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.WalkEntryArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.WalkEntryArrayBox.Size = new System.Drawing.Size(249, 26);
            this.WalkEntryArrayBox.TabIndex = 15;
            this.Help_ToolTip.SetToolTip(this.WalkEntryArrayBox, "Select the \"move\" map sprite to view/edit.");
            this.WalkEntryArrayBox.ValueChanged += new System.EventHandler(this.EntryArrayBox_ValueChanged);
            // 
            // TestGroupBox
            // 
            this.TestGroupBox.Controls.Add(this.Test_Selected);
            this.TestGroupBox.Controls.Add(this.Test_Idle);
            this.TestGroupBox.Controls.Add(this.PaletteArrayBox);
            this.TestGroupBox.Controls.Add(this.Test_WalkSide);
            this.TestGroupBox.Controls.Add(this.Test_WalkUp);
            this.TestGroupBox.Controls.Add(this.Test_WalkDown);
            this.TestGroupBox.Controls.Add(this.Test_PaletteBox);
            this.TestGroupBox.Controls.Add(this.Test_TrackBar);
            this.TestGroupBox.Controls.Add(this.Test_ImageBox);
            this.TestGroupBox.Location = new System.Drawing.Point(12, 250);
            this.TestGroupBox.Name = "TestGroupBox";
            this.TestGroupBox.Size = new System.Drawing.Size(310, 90);
            this.TestGroupBox.TabIndex = 14;
            this.TestGroupBox.TabStop = false;
            this.TestGroupBox.Text = "Map Sprite Test View";
            // 
            // Test_Selected
            // 
            this.Test_Selected.AutoSize = true;
            this.Test_Selected.Location = new System.Drawing.Point(87, 42);
            this.Test_Selected.Name = "Test_Selected";
            this.Test_Selected.Size = new System.Drawing.Size(67, 17);
            this.Test_Selected.TabIndex = 9;
            this.Test_Selected.Text = "Selected";
            this.Help_ToolTip.SetToolTip(this.Test_Selected, "Check this radio button to view the cursor-hover map sprite anim in the preview.");
            this.Test_Selected.UseVisualStyleBackColor = true;
            this.Test_Selected.CheckedChanged += new System.EventHandler(this.Test_Selected_CheckedChanged);
            // 
            // Test_Idle
            // 
            this.Test_Idle.AutoSize = true;
            this.Test_Idle.Checked = true;
            this.Test_Idle.Location = new System.Drawing.Point(87, 19);
            this.Test_Idle.Name = "Test_Idle";
            this.Test_Idle.Size = new System.Drawing.Size(67, 17);
            this.Test_Idle.TabIndex = 8;
            this.Test_Idle.TabStop = true;
            this.Test_Idle.Text = "Standing";
            this.Help_ToolTip.SetToolTip(this.Test_Idle, "Check this radio button to view the idle map sprite anim in the preview.\r\n");
            this.Test_Idle.UseVisualStyleBackColor = true;
            this.Test_Idle.CheckedChanged += new System.EventHandler(this.Test_Idle_CheckedChanged);
            // 
            // PaletteArrayBox
            // 
            this.PaletteArrayBox.Location = new System.Drawing.Point(79, 62);
            this.PaletteArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.PaletteArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.PaletteArrayBox.Size = new System.Drawing.Size(137, 26);
            this.PaletteArrayBox.TabIndex = 7;
            this.Help_ToolTip.SetToolTip(this.PaletteArrayBox, "Select the map sprite palette to use for displaying the map sprite preview.");
            this.PaletteArrayBox.ValueChanged += new System.EventHandler(this.PaletteArrayBox_ValueChanged);
            // 
            // Test_WalkSide
            // 
            this.Test_WalkSide.AutoSize = true;
            this.Test_WalkSide.Location = new System.Drawing.Point(6, 19);
            this.Test_WalkSide.Name = "Test_WalkSide";
            this.Test_WalkSide.Size = new System.Drawing.Size(74, 17);
            this.Test_WalkSide.TabIndex = 6;
            this.Test_WalkSide.Text = "Walk Side";
            this.Help_ToolTip.SetToolTip(this.Test_WalkSide, "Check this radio button to view the move-to-the-side map sprite anim in the previ" +
        "ew.");
            this.Test_WalkSide.UseVisualStyleBackColor = true;
            this.Test_WalkSide.CheckedChanged += new System.EventHandler(this.Test_WalkUp_CheckedChanged);
            // 
            // Test_WalkUp
            // 
            this.Test_WalkUp.AutoSize = true;
            this.Test_WalkUp.Location = new System.Drawing.Point(6, 65);
            this.Test_WalkUp.Name = "Test_WalkUp";
            this.Test_WalkUp.Size = new System.Drawing.Size(67, 17);
            this.Test_WalkUp.TabIndex = 5;
            this.Test_WalkUp.Text = "Walk Up";
            this.Help_ToolTip.SetToolTip(this.Test_WalkUp, "Check this radio button to view the move-upwards map sprite anim in the preview.\r" +
        "\n");
            this.Test_WalkUp.UseVisualStyleBackColor = true;
            this.Test_WalkUp.CheckedChanged += new System.EventHandler(this.Test_WalkDown_CheckedChanged);
            // 
            // Test_WalkDown
            // 
            this.Test_WalkDown.AutoSize = true;
            this.Test_WalkDown.Location = new System.Drawing.Point(6, 42);
            this.Test_WalkDown.Name = "Test_WalkDown";
            this.Test_WalkDown.Size = new System.Drawing.Size(81, 17);
            this.Test_WalkDown.TabIndex = 4;
            this.Test_WalkDown.Text = "Walk Down";
            this.Help_ToolTip.SetToolTip(this.Test_WalkDown, "Check this radio button to view the move-downwards map sprite anim in the preview" +
        ".");
            this.Test_WalkDown.UseVisualStyleBackColor = true;
            this.Test_WalkDown.CheckedChanged += new System.EventHandler(this.Test_WalkSide_CheckedChanged);
            // 
            // Test_PaletteBox
            // 
            this.Test_PaletteBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Test_PaletteBox.ColorsPerLine = 8;
            this.Test_PaletteBox.ForeColor = System.Drawing.SystemColors.Control;
            this.Test_PaletteBox.Location = new System.Drawing.Point(222, 64);
            this.Test_PaletteBox.Name = "Test_PaletteBox";
            this.Test_PaletteBox.Size = new System.Drawing.Size(80, 20);
            this.Test_PaletteBox.TabIndex = 1;
            this.Test_PaletteBox.TabStop = false;
            this.Test_PaletteBox.Text = "GBAPaletteBox";
            this.Help_ToolTip.SetToolTip(this.Test_PaletteBox, "The currently selected map sprite palette.\r\nClick on this to open a PaletteEditor" +
        ", to modify this palette.");
            this.Test_PaletteBox.Click += new System.EventHandler(this.PaletteBox_Click);
            // 
            // Test_TrackBar
            // 
            this.Test_TrackBar.Location = new System.Drawing.Point(160, 19);
            this.Test_TrackBar.Maximum = 2;
            this.Test_TrackBar.Name = "Test_TrackBar";
            this.Test_TrackBar.Size = new System.Drawing.Size(104, 45);
            this.Test_TrackBar.TabIndex = 2;
            this.Help_ToolTip.SetToolTip(this.Test_TrackBar, "This slider selects the frame to display in the map sprite preview.");
            this.Test_TrackBar.ValueChanged += new System.EventHandler(this.Test_TrackBar_ValueChanged);
            // 
            // Test_ImageBox
            // 
            this.Test_ImageBox.BackColor = System.Drawing.SystemColors.Control;
            this.Test_ImageBox.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.Test_ImageBox.Location = new System.Drawing.Point(270, 14);
            this.Test_ImageBox.Name = "Test_ImageBox";
            this.Test_ImageBox.Size = new System.Drawing.Size(32, 32);
            this.Test_ImageBox.TabIndex = 0;
            this.Test_ImageBox.TabStop = false;
            this.Test_ImageBox.Text = "imageBox1";
            // 
            // Editor_Menu
            // 
            this.Editor_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File});
            this.Editor_Menu.Location = new System.Drawing.Point(0, 0);
            this.Editor_Menu.Name = "Editor_Menu";
            this.Editor_Menu.Size = new System.Drawing.Size(334, 24);
            this.Editor_Menu.TabIndex = 17;
            this.Editor_Menu.Text = "menuStrip1";
            // 
            // Menu_File
            // 
            this.Menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File_Insert,
            this.File_SaveData});
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
            // File_SaveData
            // 
            this.File_SaveData.Name = "File_SaveData";
            this.File_SaveData.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.File_SaveData.Size = new System.Drawing.Size(185, 22);
            this.File_SaveData.Text = "Save image...";
            this.File_SaveData.Click += new System.EventHandler(this.File_Save_Click);
            // 
            // MapSpriteEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 352);
            this.Controls.Add(this.WalkEntryLabel);
            this.Controls.Add(this.WalkEntryArrayBox);
            this.Controls.Add(this.TestGroupBox);
            this.Controls.Add(this.EditGroupBox);
            this.Controls.Add(this.IdleEntryLabel);
            this.Controls.Add(this.IdleEntryArrayBox);
            this.Controls.Add(this.Editor_Menu);
            this.MainMenuStrip = this.Editor_Menu;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(350, 390);
            this.MinimumSize = new System.Drawing.Size(350, 390);
            this.Name = "MapSpriteEditor";
            this.Text = "Map Sprite Editor";
            this.EditGroupBox.ResumeLayout(false);
            this.EditGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IdlePointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WalkPointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnimPointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnknownNumberBox)).EndInit();
            this.TestGroupBox.ResumeLayout(false);
            this.TestGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Test_TrackBar)).EndInit();
            this.Editor_Menu.ResumeLayout(false);
            this.Editor_Menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox EditGroupBox;
        private EmblemMagic.Components.ByteArrayBox IdleEntryArrayBox;
        private EmblemMagic.Components.ByteArrayBox WalkEntryArrayBox;
        private System.Windows.Forms.Label IdleEntryLabel;
        private System.Windows.Forms.Label WalkEntryLabel;
        private EmblemMagic.Components.PaletteBox Test_PaletteBox;
        private EmblemMagic.Components.ByteArrayBox PaletteArrayBox;
        private System.Windows.Forms.ComboBox SizeComboBox;
        private EmblemMagic.Components.PointerBox IdlePointerBox;
        private EmblemMagic.Components.PointerBox WalkPointerBox;
        private EmblemMagic.Components.PointerBox AnimPointerBox;
        private EmblemMagic.Components.ByteBox UnknownNumberBox;
        private System.Windows.Forms.Label SizeLabel;
        private System.Windows.Forms.Label IdlePointerLabel;
        private System.Windows.Forms.Label WalkPointerLabel;
        private System.Windows.Forms.Label AnimPointerLabel;
        private System.Windows.Forms.Label UnkownLabel;
        private EmblemMagic.Components.ImageBox Edit_ImageBox;
        private System.Windows.Forms.GroupBox TestGroupBox;
        private System.Windows.Forms.TrackBar Test_TrackBar;
        private System.Windows.Forms.RadioButton Test_WalkSide;
        private System.Windows.Forms.RadioButton Test_WalkUp;
        private System.Windows.Forms.RadioButton Test_WalkDown;
        private System.Windows.Forms.RadioButton Test_Selected;
        private System.Windows.Forms.RadioButton Test_Idle;
        private EmblemMagic.Components.ImageBox Test_ImageBox;
        private System.Windows.Forms.MenuStrip Editor_Menu;
        private System.Windows.Forms.ToolStripMenuItem Menu_File;
        private System.Windows.Forms.ToolStripMenuItem File_Insert;
        private System.Windows.Forms.ToolStripMenuItem File_SaveData;
    }
}