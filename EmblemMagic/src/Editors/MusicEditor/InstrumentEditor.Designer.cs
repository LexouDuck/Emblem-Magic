namespace EmblemMagic.Editors
{
    partial class InstrumentEditor
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
            this.Entry_Label = new System.Windows.Forms.Label();
            this.Type_ComboBox = new System.Windows.Forms.ComboBox();
            this.Type_Label = new System.Windows.Forms.Label();
            this.Edit_GroupBox = new System.Windows.Forms.GroupBox();
            this.KeyMap_Button = new System.Windows.Forms.Button();
            this.Sample_Button = new System.Windows.Forms.Button();
            this.DutyPeriod_ComboBox = new System.Windows.Forms.ComboBox();
            this.DutyPeriod_Label = new System.Windows.Forms.Label();
            this.KeyMap_Label = new System.Windows.Forms.Label();
            this.KeyMap_PointerBox = new Magic.Components.PointerBox();
            this.Sample_Label = new System.Windows.Forms.Label();
            this.Envelope_Label = new System.Windows.Forms.Label();
            this.Envelope_Attack_ByteBox = new Magic.Components.ByteBox();
            this.Envelope_Decay_ByteBox = new Magic.Components.ByteBox();
            this.Envelope_Sustain_ByteBox = new Magic.Components.ByteBox();
            this.Envelope_Release_ByteBox = new Magic.Components.ByteBox();
            this.Sample_PointerBox = new Magic.Components.PointerBox();
            this.Panning_Label = new System.Windows.Forms.Label();
            this.Unused_Label = new System.Windows.Forms.Label();
            this.Panning_ByteBox = new Magic.Components.ByteBox();
            this.Unused_ByteBox = new Magic.Components.ByteBox();
            this.BaseKey_Label = new System.Windows.Forms.Label();
            this.BaseKey_ByteArrayBox = new Magic.Components.ByteArrayBox();
            this.Entry_ByteBox = new Magic.Components.ByteBox();
            this.Instrument_PianoBox = new Magic.Components.PianoBox();
            this.Editor_Menu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Address_Label = new System.Windows.Forms.Label();
            this.Edit_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.KeyMap_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Envelope_Attack_ByteBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Envelope_Decay_ByteBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Envelope_Sustain_ByteBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Envelope_Release_ByteBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Sample_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Panning_ByteBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Unused_ByteBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Entry_ByteBox)).BeginInit();
            this.Editor_Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // Entry_Label
            // 
            this.Entry_Label.AutoSize = true;
            this.Entry_Label.Location = new System.Drawing.Point(390, 6);
            this.Entry_Label.Name = "Entry_Label";
            this.Entry_Label.Size = new System.Drawing.Size(37, 13);
            this.Entry_Label.TabIndex = 1;
            this.Entry_Label.Text = "Entry :";
            // 
            // Type_ComboBox
            // 
            this.Type_ComboBox.FormattingEnabled = true;
            this.Type_ComboBox.Location = new System.Drawing.Point(52, 18);
            this.Type_ComboBox.Name = "Type_ComboBox";
            this.Type_ComboBox.Size = new System.Drawing.Size(122, 21);
            this.Type_ComboBox.TabIndex = 2;
            this.Type_ComboBox.SelectedIndexChanged += new System.EventHandler(this.Type_ComboBox_SelectedIndexChanged);
            // 
            // Type_Label
            // 
            this.Type_Label.AutoSize = true;
            this.Type_Label.Location = new System.Drawing.Point(9, 21);
            this.Type_Label.Name = "Type_Label";
            this.Type_Label.Size = new System.Drawing.Size(37, 13);
            this.Type_Label.TabIndex = 3;
            this.Type_Label.Text = "Type :";
            // 
            // Edit_GroupBox
            // 
            this.Edit_GroupBox.Controls.Add(this.KeyMap_Button);
            this.Edit_GroupBox.Controls.Add(this.Sample_Button);
            this.Edit_GroupBox.Controls.Add(this.DutyPeriod_ComboBox);
            this.Edit_GroupBox.Controls.Add(this.DutyPeriod_Label);
            this.Edit_GroupBox.Controls.Add(this.KeyMap_Label);
            this.Edit_GroupBox.Controls.Add(this.KeyMap_PointerBox);
            this.Edit_GroupBox.Controls.Add(this.Sample_Label);
            this.Edit_GroupBox.Controls.Add(this.Envelope_Label);
            this.Edit_GroupBox.Controls.Add(this.Envelope_Attack_ByteBox);
            this.Edit_GroupBox.Controls.Add(this.Envelope_Decay_ByteBox);
            this.Edit_GroupBox.Controls.Add(this.Envelope_Sustain_ByteBox);
            this.Edit_GroupBox.Controls.Add(this.Envelope_Release_ByteBox);
            this.Edit_GroupBox.Controls.Add(this.Sample_PointerBox);
            this.Edit_GroupBox.Controls.Add(this.Panning_Label);
            this.Edit_GroupBox.Controls.Add(this.Unused_Label);
            this.Edit_GroupBox.Controls.Add(this.Panning_ByteBox);
            this.Edit_GroupBox.Controls.Add(this.Unused_ByteBox);
            this.Edit_GroupBox.Controls.Add(this.BaseKey_Label);
            this.Edit_GroupBox.Controls.Add(this.BaseKey_ByteArrayBox);
            this.Edit_GroupBox.Controls.Add(this.Type_ComboBox);
            this.Edit_GroupBox.Controls.Add(this.Type_Label);
            this.Edit_GroupBox.Location = new System.Drawing.Point(12, 30);
            this.Edit_GroupBox.Name = "Edit_GroupBox";
            this.Edit_GroupBox.Size = new System.Drawing.Size(560, 100);
            this.Edit_GroupBox.TabIndex = 4;
            this.Edit_GroupBox.TabStop = false;
            this.Edit_GroupBox.Text = "Edit Instrument";
            // 
            // KeyMap_Button
            // 
            this.KeyMap_Button.Location = new System.Drawing.Point(322, 42);
            this.KeyMap_Button.Name = "KeyMap_Button";
            this.KeyMap_Button.Size = new System.Drawing.Size(40, 20);
            this.KeyMap_Button.TabIndex = 23;
            this.KeyMap_Button.Text = "Edit...";
            this.KeyMap_Button.UseVisualStyleBackColor = true;
            this.KeyMap_Button.Click += new System.EventHandler(this.KeyMap_Button_Click);
            // 
            // Sample_Button
            // 
            this.Sample_Button.Location = new System.Drawing.Point(322, 19);
            this.Sample_Button.Name = "Sample_Button";
            this.Sample_Button.Size = new System.Drawing.Size(40, 20);
            this.Sample_Button.TabIndex = 22;
            this.Sample_Button.Text = "Edit...";
            this.Sample_Button.UseVisualStyleBackColor = true;
            this.Sample_Button.Click += new System.EventHandler(this.Sample_Button_Click);
            // 
            // DutyPeriod_ComboBox
            // 
            this.DutyPeriod_ComboBox.FormattingEnabled = true;
            this.DutyPeriod_ComboBox.Location = new System.Drawing.Point(450, 18);
            this.DutyPeriod_ComboBox.Name = "DutyPeriod_ComboBox";
            this.DutyPeriod_ComboBox.Size = new System.Drawing.Size(103, 21);
            this.DutyPeriod_ComboBox.TabIndex = 21;
            this.DutyPeriod_ComboBox.SelectedIndexChanged += new System.EventHandler(this.DutyPeriod_ComboBox_SelectedIndexChanged);
            // 
            // DutyPeriod_Label
            // 
            this.DutyPeriod_Label.AutoSize = true;
            this.DutyPeriod_Label.Location = new System.Drawing.Point(376, 21);
            this.DutyPeriod_Label.Name = "DutyPeriod_Label";
            this.DutyPeriod_Label.Size = new System.Drawing.Size(68, 13);
            this.DutyPeriod_Label.TabIndex = 20;
            this.DutyPeriod_Label.Text = "Duty Period :";
            // 
            // KeyMap_Label
            // 
            this.KeyMap_Label.AutoSize = true;
            this.KeyMap_Label.Location = new System.Drawing.Point(185, 44);
            this.KeyMap_Label.Name = "KeyMap_Label";
            this.KeyMap_Label.Size = new System.Drawing.Size(55, 13);
            this.KeyMap_Label.TabIndex = 19;
            this.KeyMap_Label.Text = "Key Map :";
            // 
            // KeyMap_PointerBox
            // 
            this.KeyMap_PointerBox.Hexadecimal = true;
            this.KeyMap_PointerBox.Location = new System.Drawing.Point(246, 42);
            this.KeyMap_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.KeyMap_PointerBox.Name = "KeyMap_PointerBox";
            this.KeyMap_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.KeyMap_PointerBox.TabIndex = 18;
            this.KeyMap_PointerBox.ValueChanged += new System.EventHandler(this.KeyMap_PointerBox_ValueChanged);
            // 
            // Sample_Label
            // 
            this.Sample_Label.AutoSize = true;
            this.Sample_Label.Location = new System.Drawing.Point(192, 21);
            this.Sample_Label.Name = "Sample_Label";
            this.Sample_Label.Size = new System.Drawing.Size(48, 13);
            this.Sample_Label.TabIndex = 16;
            this.Sample_Label.Text = "Sample :";
            // 
            // Envelope_Label
            // 
            this.Envelope_Label.AutoSize = true;
            this.Envelope_Label.Location = new System.Drawing.Point(374, 56);
            this.Envelope_Label.Name = "Envelope_Label";
            this.Envelope_Label.Size = new System.Drawing.Size(182, 13);
            this.Envelope_Label.TabIndex = 15;
            this.Envelope_Label.Text = "Attack    Decay     Sustain    Release";
            // 
            // Envelope_Attack_ByteBox
            // 
            this.Envelope_Attack_ByteBox.Hexadecimal = true;
            this.Envelope_Attack_ByteBox.Location = new System.Drawing.Point(375, 72);
            this.Envelope_Attack_ByteBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Envelope_Attack_ByteBox.Name = "Envelope_Attack_ByteBox";
            this.Envelope_Attack_ByteBox.Size = new System.Drawing.Size(40, 20);
            this.Envelope_Attack_ByteBox.TabIndex = 14;
            this.Envelope_Attack_ByteBox.Value = ((byte)(0));
            this.Envelope_Attack_ByteBox.ValueChanged += new System.EventHandler(this.Envelope_Attack_ByteBox_ValueChanged);
            // 
            // Envelope_Decay_ByteBox
            // 
            this.Envelope_Decay_ByteBox.Hexadecimal = true;
            this.Envelope_Decay_ByteBox.Location = new System.Drawing.Point(421, 72);
            this.Envelope_Decay_ByteBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Envelope_Decay_ByteBox.Name = "Envelope_Decay_ByteBox";
            this.Envelope_Decay_ByteBox.Size = new System.Drawing.Size(40, 20);
            this.Envelope_Decay_ByteBox.TabIndex = 13;
            this.Envelope_Decay_ByteBox.Value = ((byte)(0));
            this.Envelope_Decay_ByteBox.ValueChanged += new System.EventHandler(this.Envelope_Decay_ByteBox_ValueChanged);
            // 
            // Envelope_Sustain_ByteBox
            // 
            this.Envelope_Sustain_ByteBox.Hexadecimal = true;
            this.Envelope_Sustain_ByteBox.Location = new System.Drawing.Point(467, 72);
            this.Envelope_Sustain_ByteBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Envelope_Sustain_ByteBox.Name = "Envelope_Sustain_ByteBox";
            this.Envelope_Sustain_ByteBox.Size = new System.Drawing.Size(40, 20);
            this.Envelope_Sustain_ByteBox.TabIndex = 12;
            this.Envelope_Sustain_ByteBox.Value = ((byte)(0));
            this.Envelope_Sustain_ByteBox.ValueChanged += new System.EventHandler(this.Envelope_Sustain_ByteBox_ValueChanged);
            // 
            // Envelope_Release_ByteBox
            // 
            this.Envelope_Release_ByteBox.Hexadecimal = true;
            this.Envelope_Release_ByteBox.Location = new System.Drawing.Point(513, 72);
            this.Envelope_Release_ByteBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Envelope_Release_ByteBox.Name = "Envelope_Release_ByteBox";
            this.Envelope_Release_ByteBox.Size = new System.Drawing.Size(40, 20);
            this.Envelope_Release_ByteBox.TabIndex = 11;
            this.Envelope_Release_ByteBox.Value = ((byte)(0));
            this.Envelope_Release_ByteBox.ValueChanged += new System.EventHandler(this.Envelope_Release_ByteBox_ValueChanged);
            // 
            // Sample_PointerBox
            // 
            this.Sample_PointerBox.Hexadecimal = true;
            this.Sample_PointerBox.Location = new System.Drawing.Point(246, 19);
            this.Sample_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Sample_PointerBox.Name = "Sample_PointerBox";
            this.Sample_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Sample_PointerBox.TabIndex = 10;
            this.Sample_PointerBox.ValueChanged += new System.EventHandler(this.Sample_PointerBox_ValueChanged);
            // 
            // Panning_Label
            // 
            this.Panning_Label.AutoSize = true;
            this.Panning_Label.Location = new System.Drawing.Point(25, 48);
            this.Panning_Label.Name = "Panning_Label";
            this.Panning_Label.Size = new System.Drawing.Size(103, 13);
            this.Panning_Label.TabIndex = 9;
            this.Panning_Label.Text = "Left/Right Panning :";
            // 
            // Unused_Label
            // 
            this.Unused_Label.AutoSize = true;
            this.Unused_Label.Location = new System.Drawing.Point(55, 74);
            this.Unused_Label.Name = "Unused_Label";
            this.Unused_Label.Size = new System.Drawing.Size(73, 13);
            this.Unused_Label.TabIndex = 8;
            this.Unused_Label.Text = "Unused byte :";
            // 
            // Panning_ByteBox
            // 
            this.Panning_ByteBox.Hexadecimal = true;
            this.Panning_ByteBox.Location = new System.Drawing.Point(134, 46);
            this.Panning_ByteBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Panning_ByteBox.Name = "Panning_ByteBox";
            this.Panning_ByteBox.Size = new System.Drawing.Size(40, 20);
            this.Panning_ByteBox.TabIndex = 7;
            this.Panning_ByteBox.Value = ((byte)(0));
            this.Panning_ByteBox.ValueChanged += new System.EventHandler(this.Panning_ByteBox_ValueChanged);
            // 
            // Unused_ByteBox
            // 
            this.Unused_ByteBox.Hexadecimal = true;
            this.Unused_ByteBox.Location = new System.Drawing.Point(134, 72);
            this.Unused_ByteBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Unused_ByteBox.Name = "Unused_ByteBox";
            this.Unused_ByteBox.Size = new System.Drawing.Size(40, 20);
            this.Unused_ByteBox.TabIndex = 6;
            this.Unused_ByteBox.Value = ((byte)(0));
            this.Unused_ByteBox.ValueChanged += new System.EventHandler(this.Unused_ByteBox_ValueChanged);
            // 
            // BaseKey_Label
            // 
            this.BaseKey_Label.AutoSize = true;
            this.BaseKey_Label.Location = new System.Drawing.Point(182, 74);
            this.BaseKey_Label.Name = "BaseKey_Label";
            this.BaseKey_Label.Size = new System.Drawing.Size(58, 13);
            this.BaseKey_Label.TabIndex = 5;
            this.BaseKey_Label.Text = "Base Key :";
            // 
            // BaseKey_ByteArrayBox
            // 
            this.BaseKey_ByteArrayBox.Location = new System.Drawing.Point(246, 68);
            this.BaseKey_ByteArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.BaseKey_ByteArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.BaseKey_ByteArrayBox.Name = "BaseKey_ByteArrayBox";
            this.BaseKey_ByteArrayBox.Size = new System.Drawing.Size(116, 26);
            this.BaseKey_ByteArrayBox.TabIndex = 4;
            this.BaseKey_ByteArrayBox.ValueChanged += new System.EventHandler(this.BaseKey_ByteArrayBox_ValueChanged);
            // 
            // Entry_ByteBox
            // 
            this.Entry_ByteBox.Hexadecimal = true;
            this.Entry_ByteBox.Location = new System.Drawing.Point(433, 4);
            this.Entry_ByteBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Entry_ByteBox.Name = "Entry_ByteBox";
            this.Entry_ByteBox.Size = new System.Drawing.Size(40, 20);
            this.Entry_ByteBox.TabIndex = 6;
            this.Entry_ByteBox.Value = ((byte)(0));
            this.Entry_ByteBox.ValueChanged += new System.EventHandler(this.Entry_ByteBox_ValueChanged);
            // 
            // Instrument_PianoBox
            // 
            this.Instrument_PianoBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Instrument_PianoBox.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Instrument_PianoBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Instrument_PianoBox.Location = new System.Drawing.Point(12, 136);
            this.Instrument_PianoBox.Name = "Instrument_PianoBox";
            this.Instrument_PianoBox.Octaves = 10;
            this.Instrument_PianoBox.Selection = new bool[] {
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false};
            this.Instrument_PianoBox.Size = new System.Drawing.Size(560, 45);
            this.Instrument_PianoBox.TabIndex = 7;
            this.Instrument_PianoBox.Text = "PianoBox";
            this.Instrument_PianoBox.ToggleSelection = false;
            // 
            // Editor_Menu
            // 
            this.Editor_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.Editor_Menu.Location = new System.Drawing.Point(0, 0);
            this.Editor_Menu.Name = "Editor_Menu";
            this.Editor_Menu.Size = new System.Drawing.Size(584, 24);
            this.Editor_Menu.TabIndex = 8;
            this.Editor_Menu.Text = "Menu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // Address_Label
            // 
            this.Address_Label.AutoSize = true;
            this.Address_Label.Location = new System.Drawing.Point(483, 6);
            this.Address_Label.Name = "Address_Label";
            this.Address_Label.Size = new System.Drawing.Size(66, 13);
            this.Address_Label.TabIndex = 9;
            this.Address_Label.Text = "0x08FFFFFF";
            // 
            // InstrumentEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 192);
            this.Controls.Add(this.Address_Label);
            this.Controls.Add(this.Instrument_PianoBox);
            this.Controls.Add(this.Entry_ByteBox);
            this.Controls.Add(this.Edit_GroupBox);
            this.Controls.Add(this.Entry_Label);
            this.Controls.Add(this.Editor_Menu);
            this.MainMenuStrip = this.Editor_Menu;
            this.MinimumSize = new System.Drawing.Size(600, 230);
            this.Name = "InstrumentEditor";
            this.Text = "Instrument Editor";
            this.Edit_GroupBox.ResumeLayout(false);
            this.Edit_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.KeyMap_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Envelope_Attack_ByteBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Envelope_Decay_ByteBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Envelope_Sustain_ByteBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Envelope_Release_ByteBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Sample_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Panning_ByteBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Unused_ByteBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Entry_ByteBox)).EndInit();
            this.Editor_Menu.ResumeLayout(false);
            this.Editor_Menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label Entry_Label;
        private System.Windows.Forms.ComboBox Type_ComboBox;
        private System.Windows.Forms.Label Type_Label;
        private System.Windows.Forms.GroupBox Edit_GroupBox;
        private System.Windows.Forms.Label BaseKey_Label;
        private Magic.Components.ByteArrayBox BaseKey_ByteArrayBox;
        private System.Windows.Forms.Label Panning_Label;
        private System.Windows.Forms.Label Unused_Label;
        private Magic.Components.ByteBox Panning_ByteBox;
        private Magic.Components.ByteBox Unused_ByteBox;
        private System.Windows.Forms.Label Envelope_Label;
        private Magic.Components.ByteBox Envelope_Attack_ByteBox;
        private Magic.Components.ByteBox Envelope_Decay_ByteBox;
        private Magic.Components.ByteBox Envelope_Sustain_ByteBox;
        private Magic.Components.ByteBox Envelope_Release_ByteBox;
        private Magic.Components.PointerBox Sample_PointerBox;
        private System.Windows.Forms.Label Sample_Label;
        private System.Windows.Forms.Label DutyPeriod_Label;
        private System.Windows.Forms.Label KeyMap_Label;
        private Magic.Components.PointerBox KeyMap_PointerBox;
        private System.Windows.Forms.ComboBox DutyPeriod_ComboBox;
        private System.Windows.Forms.Button KeyMap_Button;
        private System.Windows.Forms.Button Sample_Button;
        private Magic.Components.ByteBox Entry_ByteBox;
        private Magic.Components.PianoBox Instrument_PianoBox;
        private System.Windows.Forms.MenuStrip Editor_Menu;
        private System.Windows.Forms.Label Address_Label;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    }
}