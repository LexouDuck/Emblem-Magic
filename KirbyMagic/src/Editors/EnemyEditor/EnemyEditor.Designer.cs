
namespace KirbyMagic.Editors
{
    partial class EnemyEditor
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
            this.EntryArrayBox = new Magic.Components.ByteArrayBox();
            this.pointerBox1 = new Magic.Components.PointerBox();
            this.Menu = new System.Windows.Forms.MenuStrip();
            this.imageBox1 = new Magic.Components.ImageBox();
            ((System.ComponentModel.ISupportInitialize)(this.pointerBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // EntryArrayBox
            // 
            this.EntryArrayBox.Location = new System.Drawing.Point(12, 32);
            this.EntryArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.EntryArrayBox.MinimumSize = new System.Drawing.Size(128, 26);
            this.EntryArrayBox.Size = new System.Drawing.Size(250, 26);
            this.EntryArrayBox.TabIndex = 0;
            // 
            // pointerBox1
            // 
            this.pointerBox1.Hexadecimal = true;
            this.pointerBox1.Location = new System.Drawing.Point(268, 34);
            this.pointerBox1.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.pointerBox1.Name = "pointerBox1";
            this.pointerBox1.Size = new System.Drawing.Size(70, 23);
            this.pointerBox1.TabIndex = 1;
            // 
            // Menu
            // 
            this.Menu.Location = new System.Drawing.Point(0, 0);
            this.Menu.Name = "Menu";
            this.Menu.Size = new System.Drawing.Size(800, 24);
            this.Menu.TabIndex = 2;
            // 
            // imageBox1
            // 
            this.imageBox1.BackColor = System.Drawing.SystemColors.Control;
            this.imageBox1.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.imageBox1.Location = new System.Drawing.Point(12, 64);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(128, 256);
            this.imageBox1.TabIndex = 3;
            this.imageBox1.TabStop = false;
            this.imageBox1.Text = "imageBox1";
            // 
            // EnemyEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.imageBox1);
            this.Controls.Add(this.pointerBox1);
            this.Controls.Add(this.EntryArrayBox);
            this.Controls.Add(this.Menu);
            this.MainMenuStrip = this.Menu;
            this.Name = "EnemyEditor";
            this.Text = "EnemyEditor";
            ((System.ComponentModel.ISupportInitialize)(this.pointerBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Magic.Components.ByteArrayBox EntryArrayBox;
        private Magic.Components.PointerBox pointerBox1;
        private System.Windows.Forms.MenuStrip Menu;
        private Magic.Components.ImageBox imageBox1;
    }
}