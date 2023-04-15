namespace Magic
{
    partial class MarkSpace
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MarkSpace));
            this.Space_MarkAsLabel = new System.Windows.Forms.Label();
            this.Space_MarkAsComboBox = new Magic.Components.MarkingBox(App.MHF.Marks);
            this.Space_EndByteLabel = new System.Windows.Forms.RadioButton();
            this.Space_LengthLabel = new System.Windows.Forms.RadioButton();
            this.Space_EndByteBox = new Magic.Components.PointerBox();
            this.Space_OKButton = new System.Windows.Forms.Button();
            this.Space_AddressLabel = new System.Windows.Forms.Label();
            this.Space_AddressBox = new Magic.Components.PointerBox();
            this.Space_LengthBox = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.Space_EndByteBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Space_AddressBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Space_LengthBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Space_MarkAsLabel
            // 
            this.Space_MarkAsLabel.AutoSize = true;
            this.Space_MarkAsLabel.Location = new System.Drawing.Point(193, 9);
            this.Space_MarkAsLabel.Name = "Space_MarkAsLabel";
            this.Space_MarkAsLabel.Size = new System.Drawing.Size(48, 13);
            this.Space_MarkAsLabel.TabIndex = 12;
            this.Space_MarkAsLabel.Text = "Mark as:";
            // 
            // Space_MarkAsComboBox
            // 
            this.Space_MarkAsComboBox.FormattingEnabled = true;
            this.Space_MarkAsComboBox.Location = new System.Drawing.Point(244, 6);
            this.Space_MarkAsComboBox.Name = "Space_MarkAsComboBox";
            this.Space_MarkAsComboBox.Size = new System.Drawing.Size(78, 21);
            this.Space_MarkAsComboBox.TabIndex = 13;
            // 
            // Space_EndByteLabel
            // 
            this.Space_EndByteLabel.AutoSize = true;
            this.Space_EndByteLabel.Checked = true;
            this.Space_EndByteLabel.Location = new System.Drawing.Point(10, 34);
            this.Space_EndByteLabel.Name = "Space_EndByteLabel";
            this.Space_EndByteLabel.Size = new System.Drawing.Size(88, 17);
            this.Space_EndByteLabel.TabIndex = 8;
            this.Space_EndByteLabel.TabStop = true;
            this.Space_EndByteLabel.Text = "End Address:";
            this.Space_EndByteLabel.UseVisualStyleBackColor = true;
            this.Space_EndByteLabel.Click += new System.EventHandler(this.Space_EndByteLabel_Click);
            // 
            // Space_LengthLabel
            // 
            this.Space_LengthLabel.AutoSize = true;
            this.Space_LengthLabel.Enabled = false;
            this.Space_LengthLabel.Location = new System.Drawing.Point(10, 60);
            this.Space_LengthLabel.Name = "Space_LengthLabel";
            this.Space_LengthLabel.Size = new System.Drawing.Size(61, 17);
            this.Space_LengthLabel.TabIndex = 10;
            this.Space_LengthLabel.Text = "Length:";
            this.Space_LengthLabel.UseVisualStyleBackColor = true;
            this.Space_LengthLabel.Click += new System.EventHandler(this.Space_LengthLabel_Click);
            // 
            // Space_EndByteBox
            // 
            this.Space_EndByteBox.Hexadecimal = true;
            this.Space_EndByteBox.Location = new System.Drawing.Point(104, 34);
            this.Space_EndByteBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Space_EndByteBox.Name = "Space_EndByteBox";
            this.Space_EndByteBox.Size = new System.Drawing.Size(83, 20);
            this.Space_EndByteBox.TabIndex = 9;
            // 
            // Space_OKButton
            // 
            this.Space_OKButton.Location = new System.Drawing.Point(196, 34);
            this.Space_OKButton.Name = "Space_OKButton";
            this.Space_OKButton.Size = new System.Drawing.Size(126, 45);
            this.Space_OKButton.TabIndex = 14;
            this.Space_OKButton.Text = "Mark ROM Space";
            this.Space_OKButton.UseVisualStyleBackColor = true;
            this.Space_OKButton.Click += new System.EventHandler(this.Space_OKButton_Click);
            // 
            // Space_AddressLabel
            // 
            this.Space_AddressLabel.AutoSize = true;
            this.Space_AddressLabel.Location = new System.Drawing.Point(18, 10);
            this.Space_AddressLabel.Name = "Space_AddressLabel";
            this.Space_AddressLabel.Size = new System.Drawing.Size(73, 13);
            this.Space_AddressLabel.TabIndex = 6;
            this.Space_AddressLabel.Text = "Start Address:";
            // 
            // Space_AddressBox
            // 
            this.Space_AddressBox.Hexadecimal = true;
            this.Space_AddressBox.Location = new System.Drawing.Point(104, 7);
            this.Space_AddressBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Space_AddressBox.Name = "Space_AddressBox";
            this.Space_AddressBox.Size = new System.Drawing.Size(83, 20);
            this.Space_AddressBox.TabIndex = 7;
            // 
            // Space_LengthBox
            // 
            this.Space_LengthBox.Enabled = false;
            this.Space_LengthBox.Hexadecimal = true;
            this.Space_LengthBox.Location = new System.Drawing.Point(104, 60);
            this.Space_LengthBox.Name = "Space_LengthBox";
            this.Space_LengthBox.Size = new System.Drawing.Size(83, 20);
            this.Space_LengthBox.TabIndex = 11;
            // 
            // MarkSpace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 92);
            this.Controls.Add(this.Space_MarkAsLabel);
            this.Controls.Add(this.Space_MarkAsComboBox);
            this.Controls.Add(this.Space_EndByteLabel);
            this.Controls.Add(this.Space_LengthLabel);
            this.Controls.Add(this.Space_EndByteBox);
            this.Controls.Add(this.Space_OKButton);
            this.Controls.Add(this.Space_AddressLabel);
            this.Controls.Add(this.Space_AddressBox);
            this.Controls.Add(this.Space_LengthBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MarkSpace";
            this.Text = "Mark Space";
            ((System.ComponentModel.ISupportInitialize)(this.Space_EndByteBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Space_AddressBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Space_LengthBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Space_MarkAsLabel;
        private Components.MarkingBox Space_MarkAsComboBox;
        private System.Windows.Forms.RadioButton Space_EndByteLabel;
        private System.Windows.Forms.RadioButton Space_LengthLabel;
        private Components.PointerBox Space_EndByteBox;
        private System.Windows.Forms.Button Space_OKButton;
        private System.Windows.Forms.Label Space_AddressLabel;
        private Components.PointerBox Space_AddressBox;
        private System.Windows.Forms.NumericUpDown Space_LengthBox;
    }
}