namespace EmblemMagic.Editors
{
    partial class TextReplace
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextReplace));
            this.Find_ProgressBar = new System.Windows.Forms.ProgressBar();
            this.Find_Label = new System.Windows.Forms.Label();
            this.Find_Complete_Button = new System.Windows.Forms.RadioButton();
            this.Find_Current_Button = new System.Windows.Forms.RadioButton();
            this.ReplaceAll_Button = new System.Windows.Forms.Button();
            this.Replace_NextButton = new System.Windows.Forms.Button();
            this.Find_TextBox = new System.Windows.Forms.TextBox();
            this.Replace_Label = new System.Windows.Forms.Label();
            this.Replace_TextBox = new System.Windows.Forms.TextBox();
            this.MatchCaseCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // Find_ProgressBar
            // 
            this.Find_ProgressBar.Location = new System.Drawing.Point(12, 136);
            this.Find_ProgressBar.Name = "Find_ProgressBar";
            this.Find_ProgressBar.Size = new System.Drawing.Size(225, 20);
            this.Find_ProgressBar.TabIndex = 0;
            // 
            // Find_Label
            // 
            this.Find_Label.AutoSize = true;
            this.Find_Label.Location = new System.Drawing.Point(9, 11);
            this.Find_Label.Name = "Find_Label";
            this.Find_Label.Size = new System.Drawing.Size(44, 13);
            this.Find_Label.TabIndex = 0;
            this.Find_Label.Text = "Find in :";
            // 
            // Find_Complete_Button
            // 
            this.Find_Complete_Button.AutoSize = true;
            this.Find_Complete_Button.Checked = true;
            this.Find_Complete_Button.Location = new System.Drawing.Point(148, 9);
            this.Find_Complete_Button.Name = "Find_Complete_Button";
            this.Find_Complete_Button.Size = new System.Drawing.Size(95, 17);
            this.Find_Complete_Button.TabIndex = 7;
            this.Find_Complete_Button.TabStop = true;
            this.Find_Complete_Button.Text = "All Text Entries";
            this.Find_Complete_Button.UseVisualStyleBackColor = true;
            // 
            // Find_Current_Button
            // 
            this.Find_Current_Button.AutoSize = true;
            this.Find_Current_Button.Location = new System.Drawing.Point(59, 9);
            this.Find_Current_Button.Name = "Find_Current_Button";
            this.Find_Current_Button.Size = new System.Drawing.Size(83, 17);
            this.Find_Current_Button.TabIndex = 6;
            this.Find_Current_Button.Text = "Current Text";
            this.Find_Current_Button.UseVisualStyleBackColor = true;
            // 
            // ReplaceAll_Button
            // 
            this.ReplaceAll_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ReplaceAll_Button.Location = new System.Drawing.Point(12, 102);
            this.ReplaceAll_Button.Name = "ReplaceAll_Button";
            this.ReplaceAll_Button.Size = new System.Drawing.Size(110, 28);
            this.ReplaceAll_Button.TabIndex = 4;
            this.ReplaceAll_Button.Text = "Replace All";
            this.ReplaceAll_Button.UseVisualStyleBackColor = true;
            this.ReplaceAll_Button.Click += new System.EventHandler(this.ReplaceAll_Button_Click);
            // 
            // Replace_NextButton
            // 
            this.Replace_NextButton.Location = new System.Drawing.Point(128, 102);
            this.Replace_NextButton.Name = "Replace_NextButton";
            this.Replace_NextButton.Size = new System.Drawing.Size(109, 28);
            this.Replace_NextButton.TabIndex = 3;
            this.Replace_NextButton.Text = "Replace and Next";
            this.Replace_NextButton.UseVisualStyleBackColor = true;
            this.Replace_NextButton.Click += new System.EventHandler(this.ReplaceNext_Button_Click);
            // 
            // Find_TextBox
            // 
            this.Find_TextBox.Location = new System.Drawing.Point(12, 32);
            this.Find_TextBox.Name = "Find_TextBox";
            this.Find_TextBox.Size = new System.Drawing.Size(225, 20);
            this.Find_TextBox.TabIndex = 1;
            // 
            // Replace_Label
            // 
            this.Replace_Label.AutoSize = true;
            this.Replace_Label.Location = new System.Drawing.Point(9, 56);
            this.Replace_Label.Name = "Replace_Label";
            this.Replace_Label.Size = new System.Drawing.Size(75, 13);
            this.Replace_Label.TabIndex = 0;
            this.Replace_Label.Text = "Replace with :";
            // 
            // Replace_TextBox
            // 
            this.Replace_TextBox.Location = new System.Drawing.Point(12, 76);
            this.Replace_TextBox.Name = "Replace_TextBox";
            this.Replace_TextBox.Size = new System.Drawing.Size(225, 20);
            this.Replace_TextBox.TabIndex = 2;
            // 
            // MatchCaseCheckBox
            // 
            this.MatchCaseCheckBox.AutoSize = true;
            this.MatchCaseCheckBox.Checked = true;
            this.MatchCaseCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MatchCaseCheckBox.Location = new System.Drawing.Point(157, 55);
            this.MatchCaseCheckBox.Name = "MatchCaseCheckBox";
            this.MatchCaseCheckBox.Size = new System.Drawing.Size(83, 17);
            this.MatchCaseCheckBox.TabIndex = 5;
            this.MatchCaseCheckBox.Text = "Match Case";
            this.MatchCaseCheckBox.UseVisualStyleBackColor = true;
            // 
            // TextReplace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 171);
            this.Controls.Add(this.MatchCaseCheckBox);
            this.Controls.Add(this.Replace_Label);
            this.Controls.Add(this.Replace_TextBox);
            this.Controls.Add(this.Find_ProgressBar);
            this.Controls.Add(this.Find_Label);
            this.Controls.Add(this.Find_Complete_Button);
            this.Controls.Add(this.Find_Current_Button);
            this.Controls.Add(this.ReplaceAll_Button);
            this.Controls.Add(this.Replace_NextButton);
            this.Controls.Add(this.Find_TextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TextReplace";
            this.Text = "Replace Text";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar Find_ProgressBar;
        private System.Windows.Forms.Label Find_Label;
        private System.Windows.Forms.RadioButton Find_Complete_Button;
        private System.Windows.Forms.RadioButton Find_Current_Button;
        private System.Windows.Forms.Button ReplaceAll_Button;
        private System.Windows.Forms.Button Replace_NextButton;
        private System.Windows.Forms.TextBox Find_TextBox;
        private System.Windows.Forms.Label Replace_Label;
        private System.Windows.Forms.TextBox Replace_TextBox;
        private System.Windows.Forms.CheckBox MatchCaseCheckBox;
    }
}