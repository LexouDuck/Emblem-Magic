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
            this.Idle_PointerBox = new EmblemMagic.Components.PointerBox();
            this.Idle_Size_Label = new System.Windows.Forms.Label();
            this.Move_PointerBox = new EmblemMagic.Components.PointerBox();
            this.AnimPointerLabel = new System.Windows.Forms.Label();
            this.AnimPointerBox = new EmblemMagic.Components.PointerBox();
            this.MovePointerLabel = new System.Windows.Forms.Label();
            this.Idle_Size_ComboBox = new System.Windows.Forms.ComboBox();
            this.Idle_Pointer_Label = new System.Windows.Forms.Label();
            this.UnknownNumberBox = new EmblemMagic.Components.ByteBox();
            this.Idle_Entry_Label = new System.Windows.Forms.Label();
            this.Move_Entry_Label = new System.Windows.Forms.Label();
            this.Idle_EntryArrayBox = new EmblemMagic.Components.ByteArrayBox();
            this.Move_EntryArrayBox = new EmblemMagic.Components.ByteArrayBox();
            this.TestGroupBox = new System.Windows.Forms.GroupBox();
            this.Test_Selected = new System.Windows.Forms.RadioButton();
            this.Test_Idle = new System.Windows.Forms.RadioButton();
            this.PaletteArrayBox = new EmblemMagic.Components.ByteArrayBox();
            this.Test_MoveSide = new System.Windows.Forms.RadioButton();
            this.Test_MoveUp = new System.Windows.Forms.RadioButton();
            this.Test_MoveDown = new System.Windows.Forms.RadioButton();
            this.Test_PaletteBox = new EmblemMagic.Components.PaletteBox();
            this.Test_TrackBar = new System.Windows.Forms.TrackBar();
            this.Test_ImageBox = new EmblemMagic.Components.ImageBox();
            this.Editor_Menu = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Insert = new System.Windows.Forms.ToolStripMenuItem();
            this.File_SaveData = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.File_Tools = new System.Windows.Forms.ToolStripMenuItem();
            this.Entry_DecrementBoth_Button = new System.Windows.Forms.Button();
            this.Entry_IncrementBoth_Button = new System.Windows.Forms.Button();
            this.Idle_MagicButton = new EmblemMagic.Components.MagicButton();
            this.Move_MagicButton = new EmblemMagic.Components.MagicButton();
            this.File_Tools_CreateImage = new System.Windows.Forms.ToolStripMenuItem();
            this.EditGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Idle_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Move_PointerBox)).BeginInit();
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
            this.EditGroupBox.Controls.Add(this.Idle_PointerBox);
            this.EditGroupBox.Controls.Add(this.Idle_Size_Label);
            this.EditGroupBox.Controls.Add(this.Move_PointerBox);
            this.EditGroupBox.Controls.Add(this.AnimPointerLabel);
            this.EditGroupBox.Controls.Add(this.AnimPointerBox);
            this.EditGroupBox.Controls.Add(this.MovePointerLabel);
            this.EditGroupBox.Controls.Add(this.Idle_Size_ComboBox);
            this.EditGroupBox.Controls.Add(this.Idle_Pointer_Label);
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
            // Idle_PointerBox
            // 
            this.Idle_PointerBox.Hexadecimal = true;
            this.Idle_PointerBox.Location = new System.Drawing.Point(68, 45);
            this.Idle_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Idle_PointerBox.Name = "Idle_PointerBox";
            this.Idle_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Idle_PointerBox.TabIndex = 1;
            this.Help_ToolTip.SetToolTip(this.Idle_PointerBox, "Pointer to the pixel data for the current \"idle\" map sprite.\r\nWill write to ROM i" +
        "f changed. Is repointed when inserting a new map sprite with \"File -> Insert ima" +
        "ge...\".");
            // 
            // Idle_Size_Label
            // 
            this.Idle_Size_Label.AutoSize = true;
            this.Idle_Size_Label.Location = new System.Drawing.Point(2, 22);
            this.Idle_Size_Label.Name = "Idle_Size_Label";
            this.Idle_Size_Label.Size = new System.Drawing.Size(60, 13);
            this.Idle_Size_Label.TabIndex = 11;
            this.Idle_Size_Label.Text = "IDLE Size :";
            // 
            // Move_PointerBox
            // 
            this.Move_PointerBox.Hexadecimal = true;
            this.Move_PointerBox.Location = new System.Drawing.Point(68, 72);
            this.Move_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Move_PointerBox.Name = "Move_PointerBox";
            this.Move_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Move_PointerBox.TabIndex = 2;
            this.Help_ToolTip.SetToolTip(this.Move_PointerBox, "Pointer to the pixel data for the current \"move\" map sprite.\r\nWill write to ROM i" +
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
            // MovePointerLabel
            // 
            this.MovePointerLabel.AutoSize = true;
            this.MovePointerLabel.Location = new System.Drawing.Point(18, 74);
            this.MovePointerLabel.Name = "MovePointerLabel";
            this.MovePointerLabel.Size = new System.Drawing.Size(44, 13);
            this.MovePointerLabel.TabIndex = 9;
            this.MovePointerLabel.Text = "MOVE :";
            // 
            // Idle_Size_ComboBox
            // 
            this.Idle_Size_ComboBox.Location = new System.Drawing.Point(68, 19);
            this.Idle_Size_ComboBox.Name = "Idle_Size_ComboBox";
            this.Idle_Size_ComboBox.Size = new System.Drawing.Size(70, 21);
            this.Idle_Size_ComboBox.TabIndex = 5;
            this.Help_ToolTip.SetToolTip(this.Idle_Size_ComboBox, "This dropdown selects the size of the current \"idle\" map sprite.\r\nWill write to R" +
        "OM if changed. Is changed automatically when inserting a new map sprite with \"Fi" +
        "le -> Insert image...\".");
            // 
            // Idle_Pointer_Label
            // 
            this.Idle_Pointer_Label.AutoSize = true;
            this.Idle_Pointer_Label.Location = new System.Drawing.Point(25, 47);
            this.Idle_Pointer_Label.Name = "Idle_Pointer_Label";
            this.Idle_Pointer_Label.Size = new System.Drawing.Size(37, 13);
            this.Idle_Pointer_Label.TabIndex = 8;
            this.Idle_Pointer_Label.Text = "IDLE :";
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
            // Idle_Entry_Label
            // 
            this.Idle_Entry_Label.AutoSize = true;
            this.Idle_Entry_Label.Location = new System.Drawing.Point(12, 31);
            this.Idle_Entry_Label.Name = "Idle_Entry_Label";
            this.Idle_Entry_Label.Size = new System.Drawing.Size(44, 13);
            this.Idle_Entry_Label.TabIndex = 7;
            this.Idle_Entry_Label.Text = "\"IDLE\":";
            // 
            // Move_Entry_Label
            // 
            this.Move_Entry_Label.AutoSize = true;
            this.Move_Entry_Label.Location = new System.Drawing.Point(5, 62);
            this.Move_Entry_Label.Name = "Move_Entry_Label";
            this.Move_Entry_Label.Size = new System.Drawing.Size(51, 13);
            this.Move_Entry_Label.TabIndex = 16;
            this.Move_Entry_Label.Text = "\"MOVE\":";
            // 
            // Idle_EntryArrayBox
            // 
            this.Idle_EntryArrayBox.Location = new System.Drawing.Point(62, 27);
            this.Idle_EntryArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.Idle_EntryArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.Idle_EntryArrayBox.Size = new System.Drawing.Size(225, 26);
            this.Idle_EntryArrayBox.TabIndex = 4;
            this.Help_ToolTip.SetToolTip(this.Idle_EntryArrayBox, "Select an \"idle\" map sprite to view/edit.");
            this.Idle_EntryArrayBox.ValueChanged += new System.EventHandler(this.EntryArrayBox_ValueChanged);
            // 
            // Move_EntryArrayBox
            // 
            this.Move_EntryArrayBox.Location = new System.Drawing.Point(62, 59);
            this.Move_EntryArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.Move_EntryArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.Move_EntryArrayBox.Size = new System.Drawing.Size(225, 26);
            this.Move_EntryArrayBox.TabIndex = 15;
            this.Help_ToolTip.SetToolTip(this.Move_EntryArrayBox, "Select the \"move\" map sprite to view/edit.");
            this.Move_EntryArrayBox.ValueChanged += new System.EventHandler(this.EntryArrayBox_ValueChanged);
            // 
            // TestGroupBox
            // 
            this.TestGroupBox.Controls.Add(this.Test_Selected);
            this.TestGroupBox.Controls.Add(this.Test_Idle);
            this.TestGroupBox.Controls.Add(this.PaletteArrayBox);
            this.TestGroupBox.Controls.Add(this.Test_MoveSide);
            this.TestGroupBox.Controls.Add(this.Test_MoveUp);
            this.TestGroupBox.Controls.Add(this.Test_MoveDown);
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
            // Test_MoveSide
            // 
            this.Test_MoveSide.AutoSize = true;
            this.Test_MoveSide.Location = new System.Drawing.Point(6, 19);
            this.Test_MoveSide.Name = "Test_MoveSide";
            this.Test_MoveSide.Size = new System.Drawing.Size(74, 17);
            this.Test_MoveSide.TabIndex = 6;
            this.Test_MoveSide.Text = "Move Side";
            this.Help_ToolTip.SetToolTip(this.Test_MoveSide, "Check this radio button to view the move-to-the-side map sprite anim in the previ" +
        "ew.");
            this.Test_MoveSide.UseVisualStyleBackColor = true;
            this.Test_MoveSide.CheckedChanged += new System.EventHandler(this.Test_MoveUp_CheckedChanged);
            // 
            // Test_MoveUp
            // 
            this.Test_MoveUp.AutoSize = true;
            this.Test_MoveUp.Location = new System.Drawing.Point(6, 65);
            this.Test_MoveUp.Name = "Test_MoveUp";
            this.Test_MoveUp.Size = new System.Drawing.Size(67, 17);
            this.Test_MoveUp.TabIndex = 5;
            this.Test_MoveUp.Text = "Move Up";
            this.Help_ToolTip.SetToolTip(this.Test_MoveUp, "Check this radio button to view the move-upwards map sprite anim in the preview.\r" +
        "\n");
            this.Test_MoveUp.UseVisualStyleBackColor = true;
            this.Test_MoveUp.CheckedChanged += new System.EventHandler(this.Test_MoveDown_CheckedChanged);
            // 
            // Test_MoveDown
            // 
            this.Test_MoveDown.AutoSize = true;
            this.Test_MoveDown.Location = new System.Drawing.Point(6, 42);
            this.Test_MoveDown.Name = "Test_MoveDown";
            this.Test_MoveDown.Size = new System.Drawing.Size(81, 17);
            this.Test_MoveDown.TabIndex = 4;
            this.Test_MoveDown.Text = "Move Down";
            this.Help_ToolTip.SetToolTip(this.Test_MoveDown, "Check this radio button to view the move-downwards map sprite anim in the preview" +
        ".");
            this.Test_MoveDown.UseVisualStyleBackColor = true;
            this.Test_MoveDown.CheckedChanged += new System.EventHandler(this.Test_MoveSide_CheckedChanged);
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
            this.Menu_File,
            this.File_Tools});
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
            this.File_SaveData,
            this.toolStripSeparator1});
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(182, 6);
            // 
            // File_Tools
            // 
            this.File_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File_Tools_CreateImage});
            this.File_Tools.Name = "File_Tools";
            this.File_Tools.Size = new System.Drawing.Size(48, 20);
            this.File_Tools.Text = "Tools";
            // 
            // Entry_DecrementBoth_Button
            // 
            this.Entry_DecrementBoth_Button.Location = new System.Drawing.Point(258, 0);
            this.Entry_DecrementBoth_Button.Name = "Entry_DecrementBoth_Button";
            this.Entry_DecrementBoth_Button.Size = new System.Drawing.Size(29, 26);
            this.Entry_DecrementBoth_Button.TabIndex = 18;
            this.Entry_DecrementBoth_Button.Text = "-";
            this.Help_ToolTip.SetToolTip(this.Entry_DecrementBoth_Button, "Click on this button to decrement both the current \"idle\" sprite and \"move\" sprit" +
        "e.");
            this.Entry_DecrementBoth_Button.UseVisualStyleBackColor = true;
            this.Entry_DecrementBoth_Button.Click += new System.EventHandler(this.Entry_DecrementBoth_Button_Click);
            // 
            // Entry_IncrementBoth_Button
            // 
            this.Entry_IncrementBoth_Button.Location = new System.Drawing.Point(293, 0);
            this.Entry_IncrementBoth_Button.Name = "Entry_IncrementBoth_Button";
            this.Entry_IncrementBoth_Button.Size = new System.Drawing.Size(29, 26);
            this.Entry_IncrementBoth_Button.TabIndex = 19;
            this.Entry_IncrementBoth_Button.Text = "+";
            this.Help_ToolTip.SetToolTip(this.Entry_IncrementBoth_Button, "Click on this button to increment both the current \"idle\" sprite and \"move\" sprit" +
        "e.");
            this.Entry_IncrementBoth_Button.UseVisualStyleBackColor = true;
            this.Entry_IncrementBoth_Button.Click += new System.EventHandler(this.Entry_IncrementBoth_Button_Click);
            // 
            // Idle_MagicButton
            // 
            this.Idle_MagicButton.Location = new System.Drawing.Point(293, 27);
            this.Idle_MagicButton.MaximumSize = new System.Drawing.Size(24, 24);
            this.Idle_MagicButton.MinimumSize = new System.Drawing.Size(24, 24);
            this.Idle_MagicButton.Name = "Idle_MagicButton";
            this.Idle_MagicButton.Size = new System.Drawing.Size(24, 24);
            this.Idle_MagicButton.TabIndex = 20;
            this.Idle_MagicButton.UseVisualStyleBackColor = true;
            this.Idle_MagicButton.Click += new System.EventHandler(this.Idle_MagicButton_Click);
            // 
            // Move_MagicButton
            // 
            this.Move_MagicButton.Location = new System.Drawing.Point(293, 59);
            this.Move_MagicButton.MaximumSize = new System.Drawing.Size(24, 24);
            this.Move_MagicButton.MinimumSize = new System.Drawing.Size(24, 24);
            this.Move_MagicButton.Name = "Move_MagicButton";
            this.Move_MagicButton.Size = new System.Drawing.Size(24, 24);
            this.Move_MagicButton.TabIndex = 21;
            this.Move_MagicButton.UseVisualStyleBackColor = true;
            this.Move_MagicButton.Click += new System.EventHandler(this.Move_MagicButton_Click);
            // 
            // File_Tools_CreateImage
            // 
            this.File_Tools_CreateImage.Name = "File_Tools_CreateImage";
            this.File_Tools_CreateImage.Size = new System.Drawing.Size(301, 22);
            this.File_Tools_CreateImage.Text = "Create map sprite image from idle+move...";
            this.File_Tools_CreateImage.Click += new System.EventHandler(this.File_Tools_CreateImage_Click);
            // 
            // MapSpriteEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 352);
            this.Controls.Add(this.Move_MagicButton);
            this.Controls.Add(this.Idle_MagicButton);
            this.Controls.Add(this.Entry_IncrementBoth_Button);
            this.Controls.Add(this.Entry_DecrementBoth_Button);
            this.Controls.Add(this.Move_Entry_Label);
            this.Controls.Add(this.Move_EntryArrayBox);
            this.Controls.Add(this.TestGroupBox);
            this.Controls.Add(this.EditGroupBox);
            this.Controls.Add(this.Idle_Entry_Label);
            this.Controls.Add(this.Idle_EntryArrayBox);
            this.Controls.Add(this.Editor_Menu);
            this.MainMenuStrip = this.Editor_Menu;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(350, 390);
            this.MinimumSize = new System.Drawing.Size(350, 390);
            this.Name = "MapSpriteEditor";
            this.Text = "Map Sprite Editor";
            this.EditGroupBox.ResumeLayout(false);
            this.EditGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Idle_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Move_PointerBox)).EndInit();
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
        private EmblemMagic.Components.ByteArrayBox Idle_EntryArrayBox;
        private EmblemMagic.Components.ByteArrayBox Move_EntryArrayBox;
        private System.Windows.Forms.Label Idle_Entry_Label;
        private System.Windows.Forms.Label Move_Entry_Label;
        private EmblemMagic.Components.PaletteBox Test_PaletteBox;
        private EmblemMagic.Components.ByteArrayBox PaletteArrayBox;
        private System.Windows.Forms.ComboBox Idle_Size_ComboBox;
        private EmblemMagic.Components.PointerBox Idle_PointerBox;
        private EmblemMagic.Components.PointerBox Move_PointerBox;
        private EmblemMagic.Components.PointerBox AnimPointerBox;
        private EmblemMagic.Components.ByteBox UnknownNumberBox;
        private System.Windows.Forms.Label Idle_Size_Label;
        private System.Windows.Forms.Label Idle_Pointer_Label;
        private System.Windows.Forms.Label MovePointerLabel;
        private System.Windows.Forms.Label AnimPointerLabel;
        private System.Windows.Forms.Label UnkownLabel;
        private EmblemMagic.Components.ImageBox Edit_ImageBox;
        private System.Windows.Forms.GroupBox TestGroupBox;
        private System.Windows.Forms.TrackBar Test_TrackBar;
        private System.Windows.Forms.RadioButton Test_MoveSide;
        private System.Windows.Forms.RadioButton Test_MoveUp;
        private System.Windows.Forms.RadioButton Test_MoveDown;
        private System.Windows.Forms.RadioButton Test_Selected;
        private System.Windows.Forms.RadioButton Test_Idle;
        private EmblemMagic.Components.ImageBox Test_ImageBox;
        private System.Windows.Forms.MenuStrip Editor_Menu;
        private System.Windows.Forms.ToolStripMenuItem Menu_File;
        private System.Windows.Forms.ToolStripMenuItem File_Insert;
        private System.Windows.Forms.ToolStripMenuItem File_SaveData;
        private System.Windows.Forms.Button Entry_DecrementBoth_Button;
        private System.Windows.Forms.Button Entry_IncrementBoth_Button;
        private Components.MagicButton Idle_MagicButton;
        private Components.MagicButton Move_MagicButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem File_Tools;
        private System.Windows.Forms.ToolStripMenuItem File_Tools_CreateImage;
    }
}