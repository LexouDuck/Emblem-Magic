namespace EmblemMagic.Editors
{
    partial class TextFind
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextFind));
            this.Find_TextBox = new System.Windows.Forms.TextBox();
            this.Find_Next_Button = new System.Windows.Forms.Button();
            this.Find_Prev_Button = new System.Windows.Forms.Button();
            this.Find_Current_Button = new System.Windows.Forms.RadioButton();
            this.Find_Complete_Button = new System.Windows.Forms.RadioButton();
            this.Find_Label = new System.Windows.Forms.Label();
            this.Find_ProgressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // Find_TextBox
            // 
            this.Find_TextBox.Location = new System.Drawing.Point(12, 30);
            this.Find_TextBox.Name = "Find_TextBox";
            this.Find_TextBox.Size = new System.Drawing.Size(225, 20);
            this.Find_TextBox.TabIndex = 1;
            // 
            // Find_Next_Button
            // 
            this.Find_Next_Button.Location = new System.Drawing.Point(128, 56);
            this.Find_Next_Button.Name = "Find_Next_Button";
            this.Find_Next_Button.Size = new System.Drawing.Size(109, 28);
            this.Find_Next_Button.TabIndex = 3;
            this.Find_Next_Button.Text = "Find Next";
            this.Find_Next_Button.UseVisualStyleBackColor = true;
            this.Find_Next_Button.Click += new System.EventHandler(this.Find_Next_Button_Click);
            // 
            // Find_Prev_Button
            // 
            this.Find_Prev_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Find_Prev_Button.Location = new System.Drawing.Point(12, 56);
            this.Find_Prev_Button.Name = "Find_Prev_Button";
            this.Find_Prev_Button.Size = new System.Drawing.Size(110, 28);
            this.Find_Prev_Button.TabIndex = 2;
            this.Find_Prev_Button.Text = "Find Previous";
            this.Find_Prev_Button.UseVisualStyleBackColor = true;
            this.Find_Prev_Button.Click += new System.EventHandler(this.Find_Prev_Button_Click);
            // 
            // Find_Current_Button
            // 
            this.Find_Current_Button.AutoSize = true;
            this.Find_Current_Button.Location = new System.Drawing.Point(59, 7);
            this.Find_Current_Button.Name = "Find_Current_Button";
            this.Find_Current_Button.Size = new System.Drawing.Size(83, 17);
            this.Find_Current_Button.TabIndex = 4;
            this.Find_Current_Button.Text = "Current Text";
            this.Find_Current_Button.UseVisualStyleBackColor = true;
            // 
            // Find_Complete_Button
            // 
            this.Find_Complete_Button.AutoSize = true;
            this.Find_Complete_Button.Checked = true;
            this.Find_Complete_Button.Location = new System.Drawing.Point(148, 7);
            this.Find_Complete_Button.Name = "Find_Complete_Button";
            this.Find_Complete_Button.Size = new System.Drawing.Size(95, 17);
            this.Find_Complete_Button.TabIndex = 5;
            this.Find_Complete_Button.TabStop = true;
            this.Find_Complete_Button.Text = "All Text Entries";
            this.Find_Complete_Button.UseVisualStyleBackColor = true;
            // 
            // Find_Label
            // 
            this.Find_Label.AutoSize = true;
            this.Find_Label.Location = new System.Drawing.Point(9, 9);
            this.Find_Label.Name = "Find_Label";
            this.Find_Label.Size = new System.Drawing.Size(44, 13);
            this.Find_Label.TabIndex = 0;
            this.Find_Label.Text = "Find in :";
            // 
            // Find_ProgressBar
            // 
            this.Find_ProgressBar.Location = new System.Drawing.Point(12, 90);
            this.Find_ProgressBar.Name = "Find_ProgressBar";
            this.Find_ProgressBar.Size = new System.Drawing.Size(225, 20);
            this.Find_ProgressBar.TabIndex = 0;
            // 
            // TextFind
            // 
            this.AcceptButton = this.Find_Next_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Find_Prev_Button;
            this.ClientSize = new System.Drawing.Size(249, 122);
            this.Controls.Add(this.Find_ProgressBar);
            this.Controls.Add(this.Find_Label);
            this.Controls.Add(this.Find_Complete_Button);
            this.Controls.Add(this.Find_Current_Button);
            this.Controls.Add(this.Find_Prev_Button);
            this.Controls.Add(this.Find_Next_Button);
            this.Controls.Add(this.Find_TextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(265, 160);
            this.MinimumSize = new System.Drawing.Size(265, 160);
            this.Name = "TextFind";
            this.Text = "Find Text";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Find_TextBox;
        private System.Windows.Forms.Button Find_Next_Button;
        private System.Windows.Forms.Button Find_Prev_Button;
        private System.Windows.Forms.RadioButton Find_Current_Button;
        private System.Windows.Forms.RadioButton Find_Complete_Button;
        private System.Windows.Forms.Label Find_Label;
        private System.Windows.Forms.ProgressBar Find_ProgressBar;
    }
}