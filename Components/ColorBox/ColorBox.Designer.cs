namespace EmblemMagic.Components
{
    public partial class ColorBox
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

        /// <summary> 
        /// Required method for Designer support
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "GBAColorBox";
            this.Size = new System.Drawing.Size(16, 16);
            this.TabStop = false;

            this.ResumeLayout(false);
        }
    }
}
