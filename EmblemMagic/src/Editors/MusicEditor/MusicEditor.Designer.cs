namespace EmblemMagic.Editors
{
    partial class MusicEditor
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
                if (Preview != null)
                {
                    Preview.Dispose();
                    Preview = null;
                }
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
            this.Editor_Menu = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Insert = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_View = new System.Windows.Forms.ToolStripMenuItem();
            this.View_HideInstruments = new System.Windows.Forms.ToolStripMenuItem();
            this.Entry_PointerBox = new Magic.Components.PointerBox();
            this.Entry_ByteBox1 = new Magic.Components.ByteBox();
            this.Entry_ByteBox2 = new Magic.Components.ByteBox();
            this.TrackAmount_ByteBox = new Magic.Components.ByteBox();
            this.Music_Unknown_ByteBox = new Magic.Components.ByteBox();
            this.Music_Reverb_ByteBox = new Magic.Components.ByteBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Instrument_PointerBox = new Magic.Components.PointerBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Music_Tracker = new Magic.Components.TrackerGrid();
            this.Instrument_ListBox = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Music_Priority_Label = new System.Windows.Forms.Label();
            this.Music_Priority_ByteBox = new Magic.Components.ByteBox();
            this.PlayStop_Button = new System.Windows.Forms.Button();
            this.Editor_Menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Entry_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Entry_ByteBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Entry_ByteBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackAmount_ByteBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Music_Unknown_ByteBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Music_Reverb_ByteBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Instrument_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Music_Tracker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Music_Priority_ByteBox)).BeginInit();
            this.SuspendLayout();
            // 
            // EntryArrayBox
            // 
            this.EntryArrayBox.Location = new System.Drawing.Point(12, 27);
            this.EntryArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.EntryArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.EntryArrayBox.Name = "EntryArrayBox";
            this.EntryArrayBox.Size = new System.Drawing.Size(260, 26);
            this.EntryArrayBox.TabIndex = 0;
            this.EntryArrayBox.ValueChanged += new System.EventHandler(this.EntryArrayBox_ValueChanged);
            // 
            // Editor_Menu
            // 
            this.Editor_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File,
            this.Menu_View});
            this.Editor_Menu.Location = new System.Drawing.Point(0, 0);
            this.Editor_Menu.Name = "Editor_Menu";
            this.Editor_Menu.Size = new System.Drawing.Size(584, 24);
            this.Editor_Menu.TabIndex = 1;
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
            this.File_Insert.Size = new System.Drawing.Size(140, 22);
            this.File_Insert.Text = "Insert MIDI...";
            this.File_Insert.Click += new System.EventHandler(this.File_Insert_Click);
            // 
            // File_Save
            // 
            this.File_Save.Name = "File_Save";
            this.File_Save.Size = new System.Drawing.Size(140, 22);
            this.File_Save.Text = "Save MIDI...";
            this.File_Save.Click += new System.EventHandler(this.File_Save_Click);
            // 
            // Menu_View
            // 
            this.Menu_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.View_HideInstruments});
            this.Menu_View.Name = "Menu_View";
            this.Menu_View.Size = new System.Drawing.Size(44, 20);
            this.Menu_View.Text = "View";
            // 
            // View_HideInstruments
            // 
            this.View_HideInstruments.CheckOnClick = true;
            this.View_HideInstruments.Name = "View_HideInstruments";
            this.View_HideInstruments.Size = new System.Drawing.Size(208, 22);
            this.View_HideInstruments.Text = "Hide Unused Instruments";
            this.View_HideInstruments.Click += new System.EventHandler(this.View_HideInstruments_Click);
            // 
            // Entry_PointerBox
            // 
            this.Entry_PointerBox.Hexadecimal = true;
            this.Entry_PointerBox.Location = new System.Drawing.Point(278, 30);
            this.Entry_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Entry_PointerBox.Name = "Entry_PointerBox";
            this.Entry_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Entry_PointerBox.TabIndex = 2;
            this.Entry_PointerBox.ValueChanged += new System.EventHandler(this.Entry_PointerBox_ValueChanged);
            // 
            // Entry_ByteBox1
            // 
            this.Entry_ByteBox1.Hexadecimal = true;
            this.Entry_ByteBox1.Location = new System.Drawing.Point(35, 77);
            this.Entry_ByteBox1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Entry_ByteBox1.Name = "Entry_ByteBox1";
            this.Entry_ByteBox1.Size = new System.Drawing.Size(40, 20);
            this.Entry_ByteBox1.TabIndex = 3;
            this.Entry_ByteBox1.Value = ((byte)(0));
            this.Entry_ByteBox1.ValueChanged += new System.EventHandler(this.Entry_ByteBox1_ValueChanged);
            // 
            // Entry_ByteBox2
            // 
            this.Entry_ByteBox2.Hexadecimal = true;
            this.Entry_ByteBox2.Location = new System.Drawing.Point(81, 77);
            this.Entry_ByteBox2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Entry_ByteBox2.Name = "Entry_ByteBox2";
            this.Entry_ByteBox2.Size = new System.Drawing.Size(40, 20);
            this.Entry_ByteBox2.TabIndex = 4;
            this.Entry_ByteBox2.Value = ((byte)(0));
            this.Entry_ByteBox2.ValueChanged += new System.EventHandler(this.Entry_ByteBox2_ValueChanged);
            // 
            // TrackAmount_ByteBox
            // 
            this.TrackAmount_ByteBox.Hexadecimal = true;
            this.TrackAmount_ByteBox.Location = new System.Drawing.Point(99, 106);
            this.TrackAmount_ByteBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.TrackAmount_ByteBox.Name = "TrackAmount_ByteBox";
            this.TrackAmount_ByteBox.Size = new System.Drawing.Size(40, 20);
            this.TrackAmount_ByteBox.TabIndex = 5;
            this.TrackAmount_ByteBox.Value = ((byte)(0));
            this.TrackAmount_ByteBox.ValueChanged += new System.EventHandler(this.TrackAmount_ByteBox_ValueChanged);
            // 
            // Music_Unknown_ByteBox
            // 
            this.Music_Unknown_ByteBox.Hexadecimal = true;
            this.Music_Unknown_ByteBox.Location = new System.Drawing.Point(99, 184);
            this.Music_Unknown_ByteBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Music_Unknown_ByteBox.Name = "Music_Unknown_ByteBox";
            this.Music_Unknown_ByteBox.Size = new System.Drawing.Size(40, 20);
            this.Music_Unknown_ByteBox.TabIndex = 6;
            this.Music_Unknown_ByteBox.Value = ((byte)(0));
            this.Music_Unknown_ByteBox.ValueChanged += new System.EventHandler(this.Music_Unknown_ByteBox_ValueChanged);
            // 
            // Music_Reverb_ByteBox
            // 
            this.Music_Reverb_ByteBox.Hexadecimal = true;
            this.Music_Reverb_ByteBox.Location = new System.Drawing.Point(99, 158);
            this.Music_Reverb_ByteBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Music_Reverb_ByteBox.Name = "Music_Reverb_ByteBox";
            this.Music_Reverb_ByteBox.Size = new System.Drawing.Size(40, 20);
            this.Music_Reverb_ByteBox.TabIndex = 7;
            this.Music_Reverb_ByteBox.Value = ((byte)(0));
            this.Music_Reverb_ByteBox.ValueChanged += new System.EventHandler(this.Music_Reverb_ByteBox_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Track Amount :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 186);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Unknown :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Reverb :";
            // 
            // Instrument_PointerBox
            // 
            this.Instrument_PointerBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Instrument_PointerBox.Hexadecimal = true;
            this.Instrument_PointerBox.Location = new System.Drawing.Point(502, 30);
            this.Instrument_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Instrument_PointerBox.Name = "Instrument_PointerBox";
            this.Instrument_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Instrument_PointerBox.TabIndex = 11;
            this.Instrument_PointerBox.ValueChanged += new System.EventHandler(this.Instrument_PointerBox_ValueChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(429, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Instruments :";
            // 
            // Music_Tracker
            // 
            this.Music_Tracker.AllowUserToAddRows = false;
            this.Music_Tracker.AllowUserToDeleteRows = false;
            this.Music_Tracker.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Music_Tracker.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Music_Tracker.Font = new System.Drawing.Font("Consolas", 8F);
            this.Music_Tracker.Location = new System.Drawing.Point(12, 213);
            this.Music_Tracker.Name = "Music_Tracker";
            this.Music_Tracker.ReadOnly = true;
            this.Music_Tracker.RowHeadersWidth = 30;
            this.Music_Tracker.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.Music_Tracker.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Music_Tracker.Size = new System.Drawing.Size(560, 237);
            this.Music_Tracker.TabIndex = 14;
            // 
            // Instrument_ListBox
            // 
            this.Instrument_ListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Instrument_ListBox.Font = new System.Drawing.Font("Consolas", 8F);
            this.Instrument_ListBox.FormattingEnabled = true;
            this.Instrument_ListBox.Location = new System.Drawing.Point(145, 59);
            this.Instrument_ListBox.MultiColumn = true;
            this.Instrument_ListBox.Name = "Instrument_ListBox";
            this.Instrument_ListBox.Size = new System.Drawing.Size(427, 147);
            this.Instrument_ListBox.TabIndex = 15;
            this.Instrument_ListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Instrument_ListBox_MouseDoubleClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Track Group bytes:";
            // 
            // Music_Priority_Label
            // 
            this.Music_Priority_Label.AutoSize = true;
            this.Music_Priority_Label.Location = new System.Drawing.Point(49, 134);
            this.Music_Priority_Label.Name = "Music_Priority_Label";
            this.Music_Priority_Label.Size = new System.Drawing.Size(44, 13);
            this.Music_Priority_Label.TabIndex = 18;
            this.Music_Priority_Label.Text = "Priority :";
            // 
            // Music_Priority_ByteBox
            // 
            this.Music_Priority_ByteBox.Hexadecimal = true;
            this.Music_Priority_ByteBox.Location = new System.Drawing.Point(99, 132);
            this.Music_Priority_ByteBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Music_Priority_ByteBox.Name = "Music_Priority_ByteBox";
            this.Music_Priority_ByteBox.Size = new System.Drawing.Size(40, 20);
            this.Music_Priority_ByteBox.TabIndex = 17;
            this.Music_Priority_ByteBox.Value = ((byte)(0));
            this.Music_Priority_ByteBox.ValueChanged += new System.EventHandler(this.Music_Priority_ByteBox_ValueChanged);
            // 
            // PlayStop_Button
            // 
            this.PlayStop_Button.Enabled = false;
            this.PlayStop_Button.Location = new System.Drawing.Point(354, 30);
            this.PlayStop_Button.Name = "PlayStop_Button";
            this.PlayStop_Button.Size = new System.Drawing.Size(69, 23);
            this.PlayStop_Button.TabIndex = 19;
            this.PlayStop_Button.Text = "Play/Stop";
            this.PlayStop_Button.UseVisualStyleBackColor = true;
            this.PlayStop_Button.Click += new System.EventHandler(this.PlayStop_Button_Click);
            // 
            // MusicEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 462);
            this.Controls.Add(this.PlayStop_Button);
            this.Controls.Add(this.Music_Priority_Label);
            this.Controls.Add(this.Music_Priority_ByteBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Instrument_ListBox);
            this.Controls.Add(this.Music_Tracker);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Instrument_PointerBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Music_Reverb_ByteBox);
            this.Controls.Add(this.Music_Unknown_ByteBox);
            this.Controls.Add(this.TrackAmount_ByteBox);
            this.Controls.Add(this.Entry_ByteBox2);
            this.Controls.Add(this.Entry_ByteBox1);
            this.Controls.Add(this.Entry_PointerBox);
            this.Controls.Add(this.EntryArrayBox);
            this.Controls.Add(this.Editor_Menu);
            this.MainMenuStrip = this.Editor_Menu;
            this.MinimumSize = new System.Drawing.Size(525, 500);
            this.Name = "MusicEditor";
            this.Text = "Music Editor";
            this.Editor_Menu.ResumeLayout(false);
            this.Editor_Menu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Entry_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Entry_ByteBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Entry_ByteBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackAmount_ByteBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Music_Unknown_ByteBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Music_Reverb_ByteBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Instrument_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Music_Tracker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Music_Priority_ByteBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Magic.Components.ByteArrayBox EntryArrayBox;
        private System.Windows.Forms.MenuStrip Editor_Menu;
        private System.Windows.Forms.ToolStripMenuItem Menu_File;
        private Magic.Components.PointerBox Entry_PointerBox;
        private Magic.Components.ByteBox Entry_ByteBox1;
        private Magic.Components.ByteBox Entry_ByteBox2;
        private Magic.Components.ByteBox TrackAmount_ByteBox;
        private Magic.Components.ByteBox Music_Unknown_ByteBox;
        private Magic.Components.ByteBox Music_Reverb_ByteBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Magic.Components.PointerBox Instrument_PointerBox;
        private System.Windows.Forms.Label label4;
        private Magic.Components.TrackerGrid Music_Tracker;
        private System.Windows.Forms.ListBox Instrument_ListBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripMenuItem Menu_View;
        private System.Windows.Forms.ToolStripMenuItem View_HideInstruments;
        private System.Windows.Forms.ToolStripMenuItem File_Insert;
        private System.Windows.Forms.ToolStripMenuItem File_Save;
        private System.Windows.Forms.Label Music_Priority_Label;
        private Magic.Components.ByteBox Music_Priority_ByteBox;
        private System.Windows.Forms.Button PlayStop_Button;
    }
}