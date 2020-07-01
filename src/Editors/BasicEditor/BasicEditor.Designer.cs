namespace EmblemMagic.Editors
{
    partial class BasicEditor
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
            this.Read_GroupBox = new System.Windows.Forms.GroupBox();
            this.Read_LZ77CheckBox = new System.Windows.Forms.CheckBox();
            this.Read_Button = new System.Windows.Forms.Button();
            this.Read_AddressLabel = new System.Windows.Forms.Label();
            this.Read_LengthLabel = new System.Windows.Forms.Label();
            this.Read_AddressBox = new EmblemMagic.Components.PointerBox();
            this.Read_LengthBox = new System.Windows.Forms.NumericUpDown();
            this.Write_GroupBox = new System.Windows.Forms.GroupBox();
            this.Write_HexBox = new EmblemMagic.Components.HexBox();
            this.Write_LZ77CheckBox = new System.Windows.Forms.CheckBox();
            this.Write_Button = new System.Windows.Forms.Button();
            this.Write_AddressBox = new EmblemMagic.Components.PointerBox();
            this.Write_AddressLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Output_TextBox = new System.Windows.Forms.TextBox();
            this.Read_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Read_AddressBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Read_LengthBox)).BeginInit();
            this.Write_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Write_AddressBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Read_GroupBox
            // 
            this.Read_GroupBox.Controls.Add(this.Read_LZ77CheckBox);
            this.Read_GroupBox.Controls.Add(this.Read_Button);
            this.Read_GroupBox.Controls.Add(this.Read_AddressLabel);
            this.Read_GroupBox.Controls.Add(this.Read_LengthLabel);
            this.Read_GroupBox.Location = new System.Drawing.Point(12, 9);
            this.Read_GroupBox.Name = "Read_GroupBox";
            this.Read_GroupBox.Size = new System.Drawing.Size(321, 75);
            this.Read_GroupBox.TabIndex = 0;
            this.Read_GroupBox.TabStop = false;
            this.Read_GroupBox.Text = "Read from ROM";
            // 
            // Read_LZ77CheckBox
            // 
            this.Read_LZ77CheckBox.AutoSize = true;
            this.Read_LZ77CheckBox.Location = new System.Drawing.Point(198, 15);
            this.Read_LZ77CheckBox.Name = "Read_LZ77CheckBox";
            this.Read_LZ77CheckBox.Size = new System.Drawing.Size(112, 17);
            this.Read_LZ77CheckBox.TabIndex = 8;
            this.Read_LZ77CheckBox.Text = "LZ77 Compressed";
            this.Read_LZ77CheckBox.UseVisualStyleBackColor = true;
            this.Read_LZ77CheckBox.CheckedChanged += new System.EventHandler(this.Read_LZ77CheckBox_CheckedChanged);
            // 
            // Read_Button
            // 
            this.Read_Button.Location = new System.Drawing.Point(181, 38);
            this.Read_Button.Name = "Read_Button";
            this.Read_Button.Size = new System.Drawing.Size(129, 27);
            this.Read_Button.TabIndex = 3;
            this.Read_Button.Text = "Read data";
            this.Read_Button.UseVisualStyleBackColor = true;
            this.Read_Button.Click += new System.EventHandler(this.ReadData_Click);
            // 
            // Read_AddressLabel
            // 
            this.Read_AddressLabel.Location = new System.Drawing.Point(6, 20);
            this.Read_AddressLabel.Name = "Read_AddressLabel";
            this.Read_AddressLabel.Size = new System.Drawing.Size(61, 18);
            this.Read_AddressLabel.TabIndex = 0;
            this.Read_AddressLabel.Text = "Address : ";
            this.Read_AddressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Read_LengthLabel
            // 
            this.Read_LengthLabel.Location = new System.Drawing.Point(6, 46);
            this.Read_LengthLabel.Name = "Read_LengthLabel";
            this.Read_LengthLabel.Size = new System.Drawing.Size(61, 18);
            this.Read_LengthLabel.TabIndex = 0;
            this.Read_LengthLabel.Text = "Length : ";
            this.Read_LengthLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Read_AddressBox
            // 
            this.Read_AddressBox.Hexadecimal = true;
            this.Read_AddressBox.Location = new System.Drawing.Point(79, 29);
            this.Read_AddressBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Read_AddressBox.Name = "Read_AddressBox";
            this.Read_AddressBox.Size = new System.Drawing.Size(70, 20);
            this.Read_AddressBox.TabIndex = 1;
            // 
            // Read_LengthBox
            // 
            this.Read_LengthBox.Hexadecimal = true;
            this.Read_LengthBox.Location = new System.Drawing.Point(79, 55);
            this.Read_LengthBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Read_LengthBox.Name = "Read_LengthBox";
            this.Read_LengthBox.Size = new System.Drawing.Size(70, 20);
            this.Read_LengthBox.TabIndex = 2;
            this.Read_LengthBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Write_GroupBox
            // 
            this.Write_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Write_GroupBox.Controls.Add(this.Write_HexBox);
            this.Write_GroupBox.Controls.Add(this.Write_LZ77CheckBox);
            this.Write_GroupBox.Controls.Add(this.Write_Button);
            this.Write_GroupBox.Controls.Add(this.Write_AddressBox);
            this.Write_GroupBox.Location = new System.Drawing.Point(12, 91);
            this.Write_GroupBox.Name = "Write_GroupBox";
            this.Write_GroupBox.Size = new System.Drawing.Size(321, 209);
            this.Write_GroupBox.TabIndex = 0;
            this.Write_GroupBox.TabStop = false;
            this.Write_GroupBox.Text = "Write to ROM";
            // 
            // Write_HexBox
            // 
            this.Write_HexBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Write_HexBox.Font = new System.Drawing.Font("Consolas", 8F);
            this.Write_HexBox.InfoForeColor = System.Drawing.Color.Empty;
            this.Write_HexBox.Location = new System.Drawing.Point(6, 79);
            this.Write_HexBox.Name = "Write_HexBox";
            this.Write_HexBox.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.Write_HexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(135)))), ((int)(((byte)(206)))), ((int)(((byte)(235)))));
            this.Write_HexBox.Size = new System.Drawing.Size(309, 124);
            this.Write_HexBox.TabIndex = 5;
            this.Write_HexBox.UseFixedBytesPerLine = true;
            this.Write_HexBox.VScrollBarVisible = true;
            // 
            // Write_LZ77CheckBox
            // 
            this.Write_LZ77CheckBox.AutoSize = true;
            this.Write_LZ77CheckBox.Location = new System.Drawing.Point(198, 23);
            this.Write_LZ77CheckBox.Name = "Write_LZ77CheckBox";
            this.Write_LZ77CheckBox.Size = new System.Drawing.Size(112, 17);
            this.Write_LZ77CheckBox.TabIndex = 7;
            this.Write_LZ77CheckBox.Text = "LZ77 Compressed";
            this.Write_LZ77CheckBox.UseVisualStyleBackColor = true;
            // 
            // Write_Button
            // 
            this.Write_Button.Location = new System.Drawing.Point(181, 46);
            this.Write_Button.Name = "Write_Button";
            this.Write_Button.Size = new System.Drawing.Size(129, 27);
            this.Write_Button.TabIndex = 6;
            this.Write_Button.Text = "Write data";
            this.Write_Button.UseVisualStyleBackColor = true;
            this.Write_Button.Click += new System.EventHandler(this.WriteData_Click);
            // 
            // Write_AddressBox
            // 
            this.Write_AddressBox.Hexadecimal = true;
            this.Write_AddressBox.Location = new System.Drawing.Point(67, 26);
            this.Write_AddressBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Write_AddressBox.Name = "Write_AddressBox";
            this.Write_AddressBox.Size = new System.Drawing.Size(70, 20);
            this.Write_AddressBox.TabIndex = 4;
            // 
            // Write_AddressLabel
            // 
            this.Write_AddressLabel.Location = new System.Drawing.Point(18, 117);
            this.Write_AddressLabel.Name = "Write_AddressLabel";
            this.Write_AddressLabel.Size = new System.Drawing.Size(61, 18);
            this.Write_AddressLabel.TabIndex = 8;
            this.Write_AddressLabel.Text = "Address : ";
            this.Write_AddressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(15, 147);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 20);
            this.label5.TabIndex = 0;
            this.label5.Text = "Data to write :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // OutputBox
            // 
            this.Output_TextBox.AcceptsReturn = true;
            this.Output_TextBox.AcceptsTab = true;
            this.Output_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Output_TextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.Output_TextBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Output_TextBox.Location = new System.Drawing.Point(339, 12);
            this.Output_TextBox.Multiline = true;
            this.Output_TextBox.Name = "OutputBox";
            this.Output_TextBox.ReadOnly = true;
            this.Output_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Output_TextBox.Size = new System.Drawing.Size(313, 288);
            this.Output_TextBox.TabIndex = 7;
            // 
            // BasicEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 312);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Write_AddressLabel);
            this.Controls.Add(this.Output_TextBox);
            this.Controls.Add(this.Read_LengthBox);
            this.Controls.Add(this.Read_AddressBox);
            this.Controls.Add(this.Read_GroupBox);
            this.Controls.Add(this.Write_GroupBox);
            this.MaximumSize = new System.Drawing.Size(680, 5000);
            this.MinimumSize = new System.Drawing.Size(356, 265);
            this.Name = "BasicEditor";
            this.Text = "Basic ROM Editor";
            this.Read_GroupBox.ResumeLayout(false);
            this.Read_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Read_AddressBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Read_LengthBox)).EndInit();
            this.Write_GroupBox.ResumeLayout(false);
            this.Write_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Write_AddressBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Read_Button;
        private System.Windows.Forms.Label Read_AddressLabel;
        private System.Windows.Forms.Label Read_LengthLabel;
        private System.Windows.Forms.Label Write_AddressLabel;
        private System.Windows.Forms.Label label5;
        private EmblemMagic.Components.PointerBox Read_AddressBox;
        private System.Windows.Forms.NumericUpDown Read_LengthBox;
        private EmblemMagic.Components.PointerBox Write_AddressBox;
        private Components.HexBox Write_HexBox;
        private System.Windows.Forms.Button Write_Button;
        private System.Windows.Forms.TextBox Output_TextBox;
        private System.Windows.Forms.GroupBox Read_GroupBox;
        private System.Windows.Forms.GroupBox Write_GroupBox;
        private System.Windows.Forms.CheckBox Read_LZ77CheckBox;
        private System.Windows.Forms.CheckBox Write_LZ77CheckBox;
    }
}