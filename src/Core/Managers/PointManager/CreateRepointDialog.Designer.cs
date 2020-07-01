namespace EmblemMagic
{
    partial class CreateRepointDialog
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
            this.Default_PointerBox = new EmblemMagic.Components.PointerBox();
            this.TopInfo_Label = new System.Windows.Forms.Label();
            this.AssetName_TextBox = new System.Windows.Forms.TextBox();
            this.Create_Button = new System.Windows.Forms.Button();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.Repoint_PointerBox = new EmblemMagic.Components.PointerBox();
            this.Default_Label = new System.Windows.Forms.Label();
            this.AssetName_Label = new System.Windows.Forms.Label();
            this.Repoint_Label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Default_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Repoint_PointerBox)).BeginInit();
            this.SuspendLayout();
            // 
            // DefaultPointerBox
            // 
            this.Default_PointerBox.Hexadecimal = true;
            this.Default_PointerBox.Location = new System.Drawing.Point(176, 33);
            this.Default_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Default_PointerBox.Name = "DefaultPointerBox";
            this.Default_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Default_PointerBox.TabIndex = 0;
            // 
            // TopInfoLabel
            // 
            this.TopInfo_Label.AutoSize = true;
            this.TopInfo_Label.Location = new System.Drawing.Point(12, 9);
            this.TopInfo_Label.Name = "TopInfoLabel";
            this.TopInfo_Label.Size = new System.Drawing.Size(238, 13);
            this.TopInfo_Label.TabIndex = 1;
            this.TopInfo_Label.Text = "Please enter the information for this asset pointer.";
            // 
            // AssetNameTextBox
            // 
            this.AssetName_TextBox.Location = new System.Drawing.Point(90, 92);
            this.AssetName_TextBox.Name = "AssetNameTextBox";
            this.AssetName_TextBox.Size = new System.Drawing.Size(156, 20);
            this.AssetName_TextBox.TabIndex = 2;
            // 
            // CreateButton
            // 
            this.Create_Button.Location = new System.Drawing.Point(142, 121);
            this.Create_Button.Name = "CreateButton";
            this.Create_Button.Size = new System.Drawing.Size(104, 33);
            this.Create_Button.TabIndex = 3;
            this.Create_Button.Text = "Create";
            this.Create_Button.UseVisualStyleBackColor = true;
            // 
            // CancelButton
            // 
            this.Cancel_Button.Location = new System.Drawing.Point(12, 121);
            this.Cancel_Button.Name = "CancelButton";
            this.Cancel_Button.Size = new System.Drawing.Size(103, 33);
            this.Cancel_Button.TabIndex = 4;
            this.Cancel_Button.Text = "Cancel";
            this.Cancel_Button.UseVisualStyleBackColor = true;
            // 
            // RepointPointerBox
            // 
            this.Repoint_PointerBox.Hexadecimal = true;
            this.Repoint_PointerBox.Location = new System.Drawing.Point(176, 63);
            this.Repoint_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Repoint_PointerBox.Name = "RepointPointerBox";
            this.Repoint_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Repoint_PointerBox.TabIndex = 5;
            // 
            // DefaultLabel
            // 
            this.Default_Label.AutoSize = true;
            this.Default_Label.Location = new System.Drawing.Point(14, 35);
            this.Default_Label.Name = "DefaultLabel";
            this.Default_Label.Size = new System.Drawing.Size(88, 13);
            this.Default_Label.TabIndex = 6;
            this.Default_Label.Text = "Default Address :";
            // 
            // AssetNameLabel
            // 
            this.AssetName_Label.AutoSize = true;
            this.AssetName_Label.Location = new System.Drawing.Point(14, 95);
            this.AssetName_Label.Name = "AssetNameLabel";
            this.AssetName_Label.Size = new System.Drawing.Size(70, 13);
            this.AssetName_Label.TabIndex = 7;
            this.AssetName_Label.Text = "Asset Name :";
            // 
            // RepointLabel
            // 
            this.Repoint_Label.AutoSize = true;
            this.Repoint_Label.Location = new System.Drawing.Point(14, 65);
            this.Repoint_Label.Name = "RepointLabel";
            this.Repoint_Label.Size = new System.Drawing.Size(149, 13);
            this.Repoint_Label.TabIndex = 8;
            this.Repoint_Label.Text = "Repointed Address (optional) :";
            // 
            // NewRepointDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 166);
            this.Controls.Add(this.Repoint_Label);
            this.Controls.Add(this.AssetName_Label);
            this.Controls.Add(this.Default_Label);
            this.Controls.Add(this.Repoint_PointerBox);
            this.Controls.Add(this.Cancel_Button);
            this.Controls.Add(this.Create_Button);
            this.Controls.Add(this.AssetName_TextBox);
            this.Controls.Add(this.TopInfo_Label);
            this.Controls.Add(this.Default_PointerBox);
            this.Name = "NewRepointDialog";
            this.Text = "Create Asset Pointer";
            ((System.ComponentModel.ISupportInitialize)(this.Default_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Repoint_PointerBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Components.PointerBox Default_PointerBox;
        private System.Windows.Forms.Label TopInfo_Label;
        private System.Windows.Forms.TextBox AssetName_TextBox;
        private System.Windows.Forms.Button Create_Button;
        private System.Windows.Forms.Button Cancel_Button;
        private Components.PointerBox Repoint_PointerBox;
        private System.Windows.Forms.Label Default_Label;
        private System.Windows.Forms.Label AssetName_Label;
        private System.Windows.Forms.Label Repoint_Label;
    }
}