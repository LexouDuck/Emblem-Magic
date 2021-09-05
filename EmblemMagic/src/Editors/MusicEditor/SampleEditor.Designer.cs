namespace EmblemMagic.Editors
{
    partial class SampleEditor
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
            this.Loop_CheckBox = new System.Windows.Forms.CheckBox();
            this.Loop_NumberBox = new System.Windows.Forms.NumericUpDown();
            this.Pitch_Label = new System.Windows.Forms.Label();
            this.Loop_Label = new System.Windows.Forms.Label();
            this.Edit_GroupBox = new System.Windows.Forms.GroupBox();
            this.Play_Button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Rate_NumberBox = new System.Windows.Forms.NumericUpDown();
            this.Wave_SampleBox = new Magic.Components.SampleBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Insert = new System.Windows.Forms.ToolStripMenuItem();
            this.File_Save = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.Loop_NumberBox)).BeginInit();
            this.Edit_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Rate_NumberBox)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Loop_CheckBox
            // 
            this.Loop_CheckBox.AutoSize = true;
            this.Loop_CheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Loop_CheckBox.Location = new System.Drawing.Point(10, 20);
            this.Loop_CheckBox.Name = "Loop_CheckBox";
            this.Loop_CheckBox.Size = new System.Drawing.Size(62, 17);
            this.Loop_CheckBox.TabIndex = 0;
            this.Loop_CheckBox.Text = "Looped";
            this.Loop_CheckBox.UseVisualStyleBackColor = true;
            this.Loop_CheckBox.CheckedChanged += new System.EventHandler(this.Loop_CheckBox_CheckedChanged);
            // 
            // Loop_NumberBox
            // 
            this.Loop_NumberBox.Location = new System.Drawing.Point(174, 19);
            this.Loop_NumberBox.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.Loop_NumberBox.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.Loop_NumberBox.Name = "Loop_NumberBox";
            this.Loop_NumberBox.Size = new System.Drawing.Size(70, 20);
            this.Loop_NumberBox.TabIndex = 2;
            this.Loop_NumberBox.ValueChanged += new System.EventHandler(this.Loop_NumberBox_ValueChanged);
            // 
            // Pitch_Label
            // 
            this.Pitch_Label.AutoSize = true;
            this.Pitch_Label.Location = new System.Drawing.Point(8, 48);
            this.Pitch_Label.Name = "Pitch_Label";
            this.Pitch_Label.Size = new System.Drawing.Size(82, 13);
            this.Pitch_Label.TabIndex = 3;
            this.Pitch_Label.Text = "Sampling Rate :";
            // 
            // Loop_Label
            // 
            this.Loop_Label.AutoSize = true;
            this.Loop_Label.Location = new System.Drawing.Point(106, 21);
            this.Loop_Label.Name = "Loop_Label";
            this.Loop_Label.Size = new System.Drawing.Size(62, 13);
            this.Loop_Label.TabIndex = 4;
            this.Loop_Label.Text = "Loop Start :";
            // 
            // Edit_GroupBox
            // 
            this.Edit_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Edit_GroupBox.Controls.Add(this.Play_Button);
            this.Edit_GroupBox.Controls.Add(this.label1);
            this.Edit_GroupBox.Controls.Add(this.Rate_NumberBox);
            this.Edit_GroupBox.Controls.Add(this.Wave_SampleBox);
            this.Edit_GroupBox.Controls.Add(this.Loop_Label);
            this.Edit_GroupBox.Controls.Add(this.Loop_CheckBox);
            this.Edit_GroupBox.Controls.Add(this.Pitch_Label);
            this.Edit_GroupBox.Controls.Add(this.Loop_NumberBox);
            this.Edit_GroupBox.Location = new System.Drawing.Point(12, 27);
            this.Edit_GroupBox.Name = "Edit_GroupBox";
            this.Edit_GroupBox.Size = new System.Drawing.Size(250, 120);
            this.Edit_GroupBox.TabIndex = 5;
            this.Edit_GroupBox.TabStop = false;
            this.Edit_GroupBox.Text = "Edit Sample";
            // 
            // Play_Button
            // 
            this.Play_Button.Location = new System.Drawing.Point(198, 45);
            this.Play_Button.Name = "Play_Button";
            this.Play_Button.Size = new System.Drawing.Size(46, 21);
            this.Play_Button.TabIndex = 7;
            this.Play_Button.Text = "Play";
            this.Play_Button.UseVisualStyleBackColor = true;
            this.Play_Button.Click += new System.EventHandler(this.Play_Button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(172, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Hz";
            // 
            // Rate_NumberBox
            // 
            this.Rate_NumberBox.Location = new System.Drawing.Point(96, 46);
            this.Rate_NumberBox.Maximum = new decimal(new int[] {
            4194303,
            0,
            0,
            0});
            this.Rate_NumberBox.Name = "Rate_NumberBox";
            this.Rate_NumberBox.Size = new System.Drawing.Size(70, 20);
            this.Rate_NumberBox.TabIndex = 6;
            this.Rate_NumberBox.ValueChanged += new System.EventHandler(this.Rate_NumberBox_ValueChanged);
            // 
            // Wave_SampleBox
            // 
            this.Wave_SampleBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Wave_SampleBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Wave_SampleBox.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.Wave_SampleBox.Location = new System.Drawing.Point(6, 72);
            this.Wave_SampleBox.Name = "Wave_SampleBox";
            this.Wave_SampleBox.Size = new System.Drawing.Size(238, 42);
            this.Wave_SampleBox.TabIndex = 5;
            this.Wave_SampleBox.Text = "sampleBox1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(274, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "Editor_Menu";
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
            this.File_Insert.Size = new System.Drawing.Size(141, 22);
            this.File_Insert.Text = "Insert WAV...";
            this.File_Insert.Click += new System.EventHandler(this.File_Insert_Click);
            // 
            // File_Save
            // 
            this.File_Save.Name = "File_Save";
            this.File_Save.Size = new System.Drawing.Size(141, 22);
            this.File_Save.Text = "Save WAV...";
            this.File_Save.Click += new System.EventHandler(this.File_Save_Click);
            // 
            // SampleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 159);
            this.Controls.Add(this.Edit_GroupBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SampleEditor";
            this.Text = "Sample Editor";
            ((System.ComponentModel.ISupportInitialize)(this.Loop_NumberBox)).EndInit();
            this.Edit_GroupBox.ResumeLayout(false);
            this.Edit_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Rate_NumberBox)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox Loop_CheckBox;
        private System.Windows.Forms.NumericUpDown Loop_NumberBox;
        private System.Windows.Forms.Label Pitch_Label;
        private System.Windows.Forms.Label Loop_Label;
        private System.Windows.Forms.GroupBox Edit_GroupBox;
        private Magic.Components.SampleBox Wave_SampleBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Menu_File;
        private System.Windows.Forms.ToolStripMenuItem File_Insert;
        private System.Windows.Forms.ToolStripMenuItem File_Save;
        private System.Windows.Forms.NumericUpDown Rate_NumberBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Play_Button;
    }
}