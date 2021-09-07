namespace Magic.Editors
{
    partial class HexGoTo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            this.GoToCancelButton = new System.Windows.Forms.Button();
            this.GoToOKButton = new System.Windows.Forms.Button();
            this.GoToNumber = new System.Windows.Forms.NumericUpDown();
            this.GoToNumberCheckBox = new System.Windows.Forms.RadioButton();
            this.GoToAdressCheckBox = new System.Windows.Forms.RadioButton();
            this.GoToAdress = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.GoToNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GoToAdress)).BeginInit();
            this.SuspendLayout();
            // 
            // GoToCancelButton
            // 
            this.GoToCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.GoToCancelButton.Location = new System.Drawing.Point(12, 81);
            this.GoToCancelButton.Name = "GoToCancelButton";
            this.GoToCancelButton.Size = new System.Drawing.Size(86, 30);
            this.GoToCancelButton.TabIndex = 5;
            this.GoToCancelButton.Text = "Cancel";
            this.GoToCancelButton.Click += new System.EventHandler(this.Goto_Cancel_Click);
            // 
            // GoToOKButton
            // 
            this.GoToOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.GoToOKButton.Location = new System.Drawing.Point(104, 81);
            this.GoToOKButton.Name = "GoToOKButton";
            this.GoToOKButton.Size = new System.Drawing.Size(86, 30);
            this.GoToOKButton.TabIndex = 6;
            this.GoToOKButton.Text = "Go";
            this.GoToOKButton.Click += new System.EventHandler(this.GoTo_GoToOK_Click);
            // 
            // GoToNumber
            // 
            this.GoToNumber.Enabled = false;
            this.GoToNumber.Location = new System.Drawing.Point(104, 48);
            this.GoToNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.GoToNumber.Name = "GoToNumber";
            this.GoToNumber.Size = new System.Drawing.Size(86, 20);
            this.GoToNumber.TabIndex = 4;
            this.GoToNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // GoToNumberCheckBox
            // 
            this.GoToNumberCheckBox.AutoSize = true;
            this.GoToNumberCheckBox.Location = new System.Drawing.Point(12, 48);
            this.GoToNumberCheckBox.Name = "GoToNumberCheckBox";
            this.GoToNumberCheckBox.Size = new System.Drawing.Size(86, 17);
            this.GoToNumberCheckBox.TabIndex = 3;
            this.GoToNumberCheckBox.TabStop = true;
            this.GoToNumberCheckBox.Text = "Byte Number";
            this.GoToNumberCheckBox.UseVisualStyleBackColor = true;
            this.GoToNumberCheckBox.CheckedChanged += new System.EventHandler(this.GoTo_Number_Checked);
            // 
            // GoToAdressCheckBox
            // 
            this.GoToAdressCheckBox.AutoSize = true;
            this.GoToAdressCheckBox.Checked = true;
            this.GoToAdressCheckBox.Location = new System.Drawing.Point(12, 12);
            this.GoToAdressCheckBox.Name = "GoToAdressCheckBox";
            this.GoToAdressCheckBox.Size = new System.Drawing.Size(79, 17);
            this.GoToAdressCheckBox.TabIndex = 1;
            this.GoToAdressCheckBox.TabStop = true;
            this.GoToAdressCheckBox.Text = "Hex Adress";
            this.GoToAdressCheckBox.UseVisualStyleBackColor = true;
            this.GoToAdressCheckBox.CheckedChanged += new System.EventHandler(this.GoTo_Adress_Checked);
            // 
            // GoToAdress
            // 
            this.GoToAdress.Hexadecimal = true;
            this.GoToAdress.Location = new System.Drawing.Point(104, 12);
            this.GoToAdress.Name = "GoToAdress";
            this.GoToAdress.Size = new System.Drawing.Size(86, 20);
            this.GoToAdress.TabIndex = 2;
            // 
            // FormGoTo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(204, 122);
            this.Controls.Add(this.GoToAdress);
            this.Controls.Add(this.GoToAdressCheckBox);
            this.Controls.Add(this.GoToNumberCheckBox);
            this.Controls.Add(this.GoToCancelButton);
            this.Controls.Add(this.GoToOKButton);
            this.Controls.Add(this.GoToNumber);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(220, 160);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(220, 160);
            this.Name = "FormGoTo";
            this.ShowInTaskbar = false;
            this.Text = "Hex Editor - Go To";
            this.Activated += new System.EventHandler(this.UponOpen);
            ((System.ComponentModel.ISupportInitialize)(this.GoToNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GoToAdress)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.RadioButton GoToNumberCheckBox;
        private System.Windows.Forms.RadioButton GoToAdressCheckBox;
        private System.Windows.Forms.NumericUpDown GoToAdress;
        private System.Windows.Forms.NumericUpDown GoToNumber;
        private System.Windows.Forms.Button GoToCancelButton;
        private System.Windows.Forms.Button GoToOKButton;
    }
}