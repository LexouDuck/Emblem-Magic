namespace EmblemMagic.Components
{
    public partial class GridBox
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
            if (Display != null) Display.Dispose();

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
            
            this.Name = "GridBox";
            this.Size = new System.Drawing.Size(GBA.Screen.WIDTH, GBA.Screen.HEIGHT);
            this.TabStop = false;

            this.ResumeLayout(false);
        }
    }
}
