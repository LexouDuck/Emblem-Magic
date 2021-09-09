namespace Magic.Editors
{
    public partial class HexFind : System.Windows.Forms.Form
    {
        private System.Windows.Forms.CheckBox MatchCaseCheckBox;
        private System.Windows.Forms.RadioButton FindTextRadioBox;
        private System.Windows.Forms.TextBox FindTextBox;
        private System.Windows.Forms.RadioButton FindHexRadioBox;
        private Components.HexBox FindHexBox;
        private System.Windows.Forms.Button FindOkButton;
        private System.Windows.Forms.Button FindCancelButton;
        private System.Windows.Forms.Label PercentLabel;
        private System.Windows.Forms.Label FindingLabel;
        private System.Windows.Forms.Timer FindPercent;
        private System.Windows.Forms.Timer FindTimer;
        private System.ComponentModel.IContainer components;

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
            this.components = new System.ComponentModel.Container();
            this.FindTextRadioBox = new System.Windows.Forms.RadioButton();
            this.FindOkButton = new System.Windows.Forms.Button();
            this.FindCancelButton = new System.Windows.Forms.Button();
            this.PercentLabel = new System.Windows.Forms.Label();
            this.FindingLabel = new System.Windows.Forms.Label();
            this.FindPercent = new System.Windows.Forms.Timer(this.components);
            this.FindTimer = new System.Windows.Forms.Timer(this.components);
            this.FindHexRadioBox = new System.Windows.Forms.RadioButton();
            this.FindTextBox = new System.Windows.Forms.TextBox();
            this.MatchCaseCheckBox = new System.Windows.Forms.CheckBox();
            this.FindHexBox = new Components.HexBox();
            this.SuspendLayout();
            // 
            // FindTextRadioBox
            // 
            this.FindTextRadioBox.Checked = true;
            this.FindTextRadioBox.Location = new System.Drawing.Point(12, 12);
            this.FindTextRadioBox.Name = "FindTextRadioBox";
            this.FindTextRadioBox.Size = new System.Drawing.Size(104, 17);
            this.FindTextRadioBox.TabIndex = 4;
            this.FindTextRadioBox.TabStop = true;
            this.FindTextRadioBox.Text = "ASCII Text";
            this.FindTextRadioBox.CheckedChanged += new System.EventHandler(this.Find_FindText_Checked);
            // 
            // FindOkButton
            // 
            this.FindOkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.FindOkButton.Location = new System.Drawing.Point(147, 259);
            this.FindOkButton.Name = "FindOkButton";
            this.FindOkButton.Size = new System.Drawing.Size(125, 40);
            this.FindOkButton.TabIndex = 1;
            this.FindOkButton.Text = "Find";
            this.FindOkButton.Click += new System.EventHandler(this.Find_FindOKButton_Click);
            // 
            // FindCancelButton
            // 
            this.FindCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FindCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.FindCancelButton.Location = new System.Drawing.Point(12, 260);
            this.FindCancelButton.Name = "FindCancelButton";
            this.FindCancelButton.Size = new System.Drawing.Size(125, 40);
            this.FindCancelButton.TabIndex = 0;
            this.FindCancelButton.Text = "Cancel";
            this.FindCancelButton.Click += new System.EventHandler(this.Find_CancelButton_Click);
            // 
            // lblPercent
            // 
            this.PercentLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PercentLabel.Location = new System.Drawing.Point(210, 233);
            this.PercentLabel.Name = "lblPercent";
            this.PercentLabel.Size = new System.Drawing.Size(62, 23);
            this.PercentLabel.TabIndex = 3;
            // 
            // lblFinding
            // 
            this.FindingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.FindingLabel.ForeColor = System.Drawing.Color.Blue;
            this.FindingLabel.Location = new System.Drawing.Point(144, 233);
            this.FindingLabel.Name = "lblFinding";
            this.FindingLabel.Size = new System.Drawing.Size(57, 23);
            this.FindingLabel.TabIndex = 2;
            // 
            // timerPercent
            // 
            this.FindPercent.Tick += new System.EventHandler(this.Find_TimerPercent_Tick);
            // 
            // timer
            // 
            this.FindTimer.Interval = 50;
            this.FindTimer.Tick += new System.EventHandler(this.Find_Timer_Tick);
            // 
            // FindHexRadioBox
            // 
            this.FindHexRadioBox.Location = new System.Drawing.Point(12, 61);
            this.FindHexRadioBox.Name = "FindHexRadioBox";
            this.FindHexRadioBox.Size = new System.Drawing.Size(87, 24);
            this.FindHexRadioBox.TabIndex = 7;
            this.FindHexRadioBox.Text = "Hex Values";
            this.FindHexRadioBox.CheckedChanged += new System.EventHandler(this.Find_FindHex_Checked);
            // 
            // FindTextBox
            // 
            this.FindTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FindTextBox.Location = new System.Drawing.Point(12, 35);
            this.FindTextBox.Name = "FindTextBox";
            this.FindTextBox.Size = new System.Drawing.Size(260, 20);
            this.FindTextBox.TabIndex = 8;
            // 
            // MatchCaseCheckBox
            // 
            this.MatchCaseCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MatchCaseCheckBox.AutoSize = true;
            this.MatchCaseCheckBox.Location = new System.Drawing.Point(189, 12);
            this.MatchCaseCheckBox.Name = "MatchCaseCheckBox";
            this.MatchCaseCheckBox.Size = new System.Drawing.Size(83, 17);
            this.MatchCaseCheckBox.TabIndex = 9;
            this.MatchCaseCheckBox.Text = "Match Case";
            this.MatchCaseCheckBox.UseVisualStyleBackColor = true;
            // 
            // FindHexBox
            // 
            this.FindHexBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FindHexBox.Enabled = false;
            this.FindHexBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FindHexBox.InfoForeColor = System.Drawing.Color.Empty;
            this.FindHexBox.Location = new System.Drawing.Point(12, 91);
            this.FindHexBox.Name = "FindHexBox";
            this.FindHexBox.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.FindHexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.FindHexBox.Size = new System.Drawing.Size(260, 139);
            this.FindHexBox.TabIndex = 6;
            // 
            // FormFind
            // 
            this.AcceptButton = this.FindOkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(284, 312);
            this.Controls.Add(this.MatchCaseCheckBox);
            this.Controls.Add(this.FindTextBox);
            this.Controls.Add(this.FindHexRadioBox);
            this.Controls.Add(this.FindingLabel);
            this.Controls.Add(this.FindCancelButton);
            this.Controls.Add(this.PercentLabel);
            this.Controls.Add(this.FindOkButton);
            this.Controls.Add(this.FindTextRadioBox);
            this.Controls.Add(this.FindHexBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 350);
            this.Name = "FormFind";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Hex Editor - Find";
            this.Activated += new System.EventHandler(this.Core_OnFocus);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    }
}
