namespace EmblemMagic.Editors
{
    partial class PaletteEditor
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
            this.Status = new System.Windows.Forms.ToolStripLabel();
            this.TrackBar_R = new System.Windows.Forms.TrackBar();
            this.TrackBar_G = new System.Windows.Forms.TrackBar();
            this.TrackBar_B = new System.Windows.Forms.TrackBar();
            this.NumBox_R = new System.Windows.Forms.NumericUpDown();
            this.NumBox_G = new System.Windows.Forms.NumericUpDown();
            this.NumBox_B = new System.Windows.Forms.NumericUpDown();
            this.Label_R = new System.Windows.Forms.Label();
            this.Label_G = new System.Windows.Forms.Label();
            this.Label_B = new System.Windows.Forms.Label();
            this.NumBox_16bit = new System.Windows.Forms.NumericUpDown();
            this.NumBox_32bit = new System.Windows.Forms.NumericUpDown();
            this.Label_16bit = new System.Windows.Forms.Label();
            this.Label_32bit = new System.Windows.Forms.Label();
            this.AlphaCheckBox = new System.Windows.Forms.CheckBox();
            this.SwapButton = new System.Windows.Forms.Button();
            this.LoadButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.MenuBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar_R)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar_G)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar_B)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumBox_R)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumBox_G)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumBox_B)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumBox_16bit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumBox_32bit)).BeginInit();
            this.SuspendLayout();
            // 
            // MenuBar
            // 
            this.MenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Status});
            this.MenuBar.Location = new System.Drawing.Point(0, 0);
            this.MenuBar.Name = "MenuBar";
            this.MenuBar.Size = new System.Drawing.Size(374, 24);
            this.MenuBar.TabIndex = 33;
            this.MenuBar.Text = "menuStrip1";
            // 
            // Status
            // 
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(193, 17);
            this.Status.Text = "Palette\'s Parent Entry text - Address";
            // 
            // TrackBar_R
            // 
            this.TrackBar_R.Location = new System.Drawing.Point(47, 27);
            this.TrackBar_R.Maximum = 31;
            this.TrackBar_R.Name = "TrackBar_R";
            this.TrackBar_R.Size = new System.Drawing.Size(268, 45);
            this.TrackBar_R.TabIndex = 0;
            this.TrackBar_R.TabStop = false;
            this.TrackBar_R.Scroll += new System.EventHandler(this.TrackBar_R_Scroll);
            // 
            // TrackBar_G
            // 
            this.TrackBar_G.Location = new System.Drawing.Point(47, 62);
            this.TrackBar_G.Maximum = 31;
            this.TrackBar_G.Name = "TrackBar_G";
            this.TrackBar_G.Size = new System.Drawing.Size(268, 45);
            this.TrackBar_G.TabIndex = 17;
            this.TrackBar_G.TabStop = false;
            this.TrackBar_G.Scroll += new System.EventHandler(this.TrackBar_G_Scroll);
            // 
            // TrackBar_B
            // 
            this.TrackBar_B.Location = new System.Drawing.Point(47, 97);
            this.TrackBar_B.Maximum = 31;
            this.TrackBar_B.Name = "TrackBar_B";
            this.TrackBar_B.Size = new System.Drawing.Size(268, 45);
            this.TrackBar_B.TabIndex = 18;
            this.TrackBar_B.TabStop = false;
            this.TrackBar_B.Scroll += new System.EventHandler(this.TrackBar_B_Scroll);
            // 
            // NumBox_R
            // 
            this.NumBox_R.Location = new System.Drawing.Point(321, 33);
            this.NumBox_R.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.NumBox_R.Name = "NumBox_R";
            this.NumBox_R.Size = new System.Drawing.Size(38, 20);
            this.NumBox_R.TabIndex = 3;
            this.NumBox_R.ValueChanged += new System.EventHandler(this.NumBox_R_ValueChanged);
            // 
            // NumBox_G
            // 
            this.NumBox_G.Location = new System.Drawing.Point(321, 68);
            this.NumBox_G.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.NumBox_G.Name = "NumBox_G";
            this.NumBox_G.Size = new System.Drawing.Size(38, 20);
            this.NumBox_G.TabIndex = 4;
            this.NumBox_G.ValueChanged += new System.EventHandler(this.NumBox_G_ValueChanged);
            // 
            // NumBox_B
            // 
            this.NumBox_B.Location = new System.Drawing.Point(321, 103);
            this.NumBox_B.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.NumBox_B.Name = "NumBox_B";
            this.NumBox_B.Size = new System.Drawing.Size(38, 20);
            this.NumBox_B.TabIndex = 5;
            this.NumBox_B.ValueChanged += new System.EventHandler(this.NumBox_B_ValueChanged);
            // 
            // Label_R
            // 
            this.Label_R.Location = new System.Drawing.Point(1, 27);
            this.Label_R.Name = "Label_R";
            this.Label_R.Size = new System.Drawing.Size(50, 29);
            this.Label_R.TabIndex = 22;
            this.Label_R.Text = "Red :";
            this.Label_R.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_G
            // 
            this.Label_G.Location = new System.Drawing.Point(1, 62);
            this.Label_G.Name = "Label_G";
            this.Label_G.Size = new System.Drawing.Size(50, 29);
            this.Label_G.TabIndex = 23;
            this.Label_G.Text = "Green :";
            this.Label_G.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_B
            // 
            this.Label_B.Location = new System.Drawing.Point(1, 97);
            this.Label_B.Name = "Label_B";
            this.Label_B.Size = new System.Drawing.Size(50, 29);
            this.Label_B.TabIndex = 24;
            this.Label_B.Text = "Blue :";
            this.Label_B.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumBox_16bit
            // 
            this.NumBox_16bit.Hexadecimal = true;
            this.NumBox_16bit.Location = new System.Drawing.Point(155, 135);
            this.NumBox_16bit.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumBox_16bit.Name = "NumBox_16bit";
            this.NumBox_16bit.Size = new System.Drawing.Size(60, 20);
            this.NumBox_16bit.TabIndex = 1;
            this.NumBox_16bit.ValueChanged += new System.EventHandler(this.NumBox_16bit_ValueChanged);
            // 
            // NumBox_32bit
            // 
            this.NumBox_32bit.Hexadecimal = true;
            this.NumBox_32bit.Location = new System.Drawing.Point(277, 135);
            this.NumBox_32bit.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.NumBox_32bit.Name = "NumBox_32bit";
            this.NumBox_32bit.Size = new System.Drawing.Size(82, 20);
            this.NumBox_32bit.TabIndex = 2;
            this.NumBox_32bit.ValueChanged += new System.EventHandler(this.NumBox_32bit_ValueChanged);
            // 
            // Label_16bit
            // 
            this.Label_16bit.AutoSize = true;
            this.Label_16bit.Location = new System.Drawing.Point(110, 137);
            this.Label_16bit.Name = "Label_16bit";
            this.Label_16bit.Size = new System.Drawing.Size(39, 13);
            this.Label_16bit.TabIndex = 31;
            this.Label_16bit.Text = "16-bit :";
            // 
            // Label_32bit
            // 
            this.Label_32bit.AutoSize = true;
            this.Label_32bit.Location = new System.Drawing.Point(233, 137);
            this.Label_32bit.Name = "Label_32bit";
            this.Label_32bit.Size = new System.Drawing.Size(39, 13);
            this.Label_32bit.TabIndex = 30;
            this.Label_32bit.Text = "32-bit :";
            // 
            // AlphaCheckBox
            // 
            this.AlphaCheckBox.AutoSize = true;
            this.AlphaCheckBox.Location = new System.Drawing.Point(13, 137);
            this.AlphaCheckBox.Name = "AlphaCheckBox";
            this.AlphaCheckBox.Size = new System.Drawing.Size(67, 17);
            this.AlphaCheckBox.TabIndex = 6;
            this.AlphaCheckBox.Text = "Alpha bit";
            this.AlphaCheckBox.UseVisualStyleBackColor = true;
            this.AlphaCheckBox.CheckedChanged += new System.EventHandler(this.AlphaCheckBox_CheckedChanged);
            // 
            // SwapButton
            // 
            this.SwapButton.Location = new System.Drawing.Point(13, 171);
            this.SwapButton.Name = "SwapButton";
            this.SwapButton.Size = new System.Drawing.Size(112, 28);
            this.SwapButton.TabIndex = 7;
            this.SwapButton.Text = "Swap Colors";
            this.SwapButton.UseVisualStyleBackColor = true;
            this.SwapButton.Click += new System.EventHandler(this.SwapButton_Click);
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(248, 170);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(112, 28);
            this.LoadButton.TabIndex = 8;
            this.LoadButton.Text = "Load from file...";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(131, 171);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(112, 27);
            this.SaveButton.TabIndex = 9;
            this.SaveButton.Text = "Save to file..";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // PaletteEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 212);
            this.Controls.Add(this.AlphaCheckBox);
            this.Controls.Add(this.Label_16bit);
            this.Controls.Add(this.Label_32bit);
            this.Controls.Add(this.NumBox_32bit);
            this.Controls.Add(this.NumBox_16bit);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.SwapButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.Label_B);
            this.Controls.Add(this.Label_G);
            this.Controls.Add(this.Label_R);
            this.Controls.Add(this.NumBox_B);
            this.Controls.Add(this.NumBox_G);
            this.Controls.Add(this.NumBox_R);
            this.Controls.Add(this.TrackBar_B);
            this.Controls.Add(this.TrackBar_G);
            this.Controls.Add(this.TrackBar_R);
            this.Controls.Add(this.MenuBar);
            this.MainMenuStrip = this.MenuBar;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(390, 250);
            this.Name = "PaletteEditor";
            this.Text = "Palette Editor";
            this.MenuBar.ResumeLayout(false);
            this.MenuBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar_R)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar_G)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar_B)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumBox_R)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumBox_G)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumBox_B)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumBox_16bit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumBox_32bit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuBar;
        private System.Windows.Forms.ToolStripLabel Status;
        private System.Windows.Forms.TrackBar TrackBar_R;
        private System.Windows.Forms.TrackBar TrackBar_G;
        private System.Windows.Forms.TrackBar TrackBar_B;
        private System.Windows.Forms.NumericUpDown NumBox_R;
        private System.Windows.Forms.NumericUpDown NumBox_G;
        private System.Windows.Forms.NumericUpDown NumBox_B;
        private System.Windows.Forms.Label Label_R;
        private System.Windows.Forms.Label Label_G;
        private System.Windows.Forms.Label Label_B;
        private System.Windows.Forms.Label Label_16bit;
        private System.Windows.Forms.Label Label_32bit;
        private System.Windows.Forms.NumericUpDown NumBox_16bit;
        private System.Windows.Forms.NumericUpDown NumBox_32bit;
        private System.Windows.Forms.CheckBox AlphaCheckBox;
        private System.Windows.Forms.Button SwapButton;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.Button SaveButton;
    }
}