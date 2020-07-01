namespace EmblemMagic.Editors
{
    partial class KeyMapEditor
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
            this.Editor_Menu = new System.Windows.Forms.MenuStrip();
            this.Menu_View = new System.Windows.Forms.ToolStripMenuItem();
            this.View_HideUnused = new System.Windows.Forms.ToolStripMenuItem();
            this.Address_Label = new System.Windows.Forms.Label();
            this.Sample_Label = new System.Windows.Forms.Label();
            this.Sample_ByteBox = new EmblemMagic.Components.ByteBox();
            this.KeyMap_PianoBox = new EmblemMagic.Components.PianoBox();
            this.Editor_Menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Sample_ByteBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Editor_Menu
            // 
            this.Editor_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_View});
            this.Editor_Menu.Location = new System.Drawing.Point(0, 0);
            this.Editor_Menu.Name = "Editor_Menu";
            this.Editor_Menu.Size = new System.Drawing.Size(584, 24);
            this.Editor_Menu.TabIndex = 1;
            this.Editor_Menu.Text = "menuStrip1";
            // 
            // Menu_View
            // 
            this.Menu_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.View_HideUnused});
            this.Menu_View.Name = "Menu_View";
            this.Menu_View.Size = new System.Drawing.Size(44, 20);
            this.Menu_View.Text = "View";
            // 
            // View_HideUnused
            // 
            this.View_HideUnused.Name = "View_HideUnused";
            this.View_HideUnused.Size = new System.Drawing.Size(189, 22);
            this.View_HideUnused.Text = "Hide Unused Samples";
            // 
            // Address_Label
            // 
            this.Address_Label.AutoSize = true;
            this.Address_Label.Location = new System.Drawing.Point(373, 6);
            this.Address_Label.Name = "Address_Label";
            this.Address_Label.Size = new System.Drawing.Size(66, 13);
            this.Address_Label.TabIndex = 2;
            this.Address_Label.Text = "0x08FFFFFF";
            // 
            // Sample_Label
            // 
            this.Sample_Label.AutoSize = true;
            this.Sample_Label.Location = new System.Drawing.Point(244, 6);
            this.Sample_Label.Name = "Sample_Label";
            this.Sample_Label.Size = new System.Drawing.Size(77, 13);
            this.Sample_Label.TabIndex = 4;
            this.Sample_Label.Text = "Sample Index :";
            // 
            // Sample_ByteBox
            // 
            this.Sample_ByteBox.Hexadecimal = true;
            this.Sample_ByteBox.Location = new System.Drawing.Point(327, 4);
            this.Sample_ByteBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Sample_ByteBox.Name = "Sample_ByteBox";
            this.Sample_ByteBox.Size = new System.Drawing.Size(40, 20);
            this.Sample_ByteBox.TabIndex = 3;
            this.Sample_ByteBox.Value = ((byte)(0));
            this.Sample_ByteBox.ValueChanged += new System.EventHandler(this.Sample_ByteBox_ValueChanged);
            // 
            // KeyMap_PianoBox
            // 
            this.KeyMap_PianoBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.KeyMap_PianoBox.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KeyMap_PianoBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.KeyMap_PianoBox.Location = new System.Drawing.Point(12, 27);
            this.KeyMap_PianoBox.Name = "KeyMap_PianoBox";
            this.KeyMap_PianoBox.Octaves = 10;
            this.KeyMap_PianoBox.Selection = new bool[] {
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false};
            this.KeyMap_PianoBox.Size = new System.Drawing.Size(560, 43);
            this.KeyMap_PianoBox.TabIndex = 0;
            this.KeyMap_PianoBox.Text = "pianoBox1";
            this.KeyMap_PianoBox.ToggleSelection = true;
            // 
            // KeyMapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 82);
            this.Controls.Add(this.Sample_Label);
            this.Controls.Add(this.Sample_ByteBox);
            this.Controls.Add(this.Address_Label);
            this.Controls.Add(this.KeyMap_PianoBox);
            this.Controls.Add(this.Editor_Menu);
            this.MainMenuStrip = this.Editor_Menu;
            this.MinimumSize = new System.Drawing.Size(600, 120);
            this.Name = "KeyMapEditor";
            this.Text = "Key Map Editor";
            this.Editor_Menu.ResumeLayout(false);
            this.Editor_Menu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Sample_ByteBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Components.PianoBox KeyMap_PianoBox;
        private System.Windows.Forms.MenuStrip Editor_Menu;
        private System.Windows.Forms.ToolStripMenuItem Menu_View;
        private System.Windows.Forms.ToolStripMenuItem View_HideUnused;
        private System.Windows.Forms.Label Address_Label;
        private Components.ByteBox Sample_ByteBox;
        private System.Windows.Forms.Label Sample_Label;
    }
}