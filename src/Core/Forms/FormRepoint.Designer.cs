namespace EmblemMagic
{
    partial class FormRepoint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRepoint));
            this.TextLabel = new System.Windows.Forms.Label();
            this.BoxLayout = new System.Windows.Forms.TableLayoutPanel();
            this.Confirm = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.AutoSequence = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TextLabel
            // 
            this.TextLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextLabel.Location = new System.Drawing.Point(12, 10);
            this.TextLabel.Name = "TextLabel";
            this.TextLabel.Size = new System.Drawing.Size(310, 77);
            this.TextLabel.TabIndex = 0;
            this.TextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BoxLayout
            // 
            this.BoxLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BoxLayout.AutoScroll = true;
            this.BoxLayout.ColumnCount = 4;
            this.BoxLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.BoxLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.BoxLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.BoxLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.BoxLayout.Location = new System.Drawing.Point(12, 90);
            this.BoxLayout.Name = "BoxLayout";
            this.BoxLayout.RowCount = 1;
            this.BoxLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.BoxLayout.Size = new System.Drawing.Size(310, 30);
            this.BoxLayout.TabIndex = 1;
            // 
            // Confirm
            // 
            this.Confirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Confirm.Location = new System.Drawing.Point(223, 126);
            this.Confirm.Name = "Confirm";
            this.Confirm.Size = new System.Drawing.Size(99, 30);
            this.Confirm.TabIndex = 2;
            this.Confirm.Text = "Repoint";
            this.Confirm.UseVisualStyleBackColor = true;
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Cancel.Location = new System.Drawing.Point(12, 126);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(99, 30);
            this.Cancel.TabIndex = 3;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // AutoSequence
            // 
            this.AutoSequence.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AutoSequence.Location = new System.Drawing.Point(117, 126);
            this.AutoSequence.Name = "AutoSequence";
            this.AutoSequence.Size = new System.Drawing.Size(100, 30);
            this.AutoSequence.TabIndex = 4;
            this.AutoSequence.Text = "Auto-Sequence";
            this.AutoSequence.UseVisualStyleBackColor = true;
            this.AutoSequence.Click += new System.EventHandler(this.AutoSequence_Click);
            // 
            // FormRepoint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 162);
            this.Controls.Add(this.AutoSequence);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Confirm);
            this.Controls.Add(this.BoxLayout);
            this.Controls.Add(this.TextLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(350, 600);
            this.MinimumSize = new System.Drawing.Size(350, 170);
            this.Name = "FormRepoint";
            this.Text = "Repoint";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label TextLabel;
        private System.Windows.Forms.TableLayoutPanel BoxLayout;
        private System.Windows.Forms.Button Confirm;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button AutoSequence;
    }
}