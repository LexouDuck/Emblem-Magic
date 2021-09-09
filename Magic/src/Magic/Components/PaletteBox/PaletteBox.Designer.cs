namespace Magic.Components
{
    public partial class PaletteBox
    {
        /// <summary>
        /// An event handler that calls the 'OnPaint' method
        /// </summary>
        void Redraw(object sender, System.EventArgs e)
        {
            this.Invalidate();
        }

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

            this.ColorsPerLine = 8;
            this.Name = "GBAPaletteBox";
            this.Size = new System.Drawing.Size(64, 16);
            this.SizeChanged += new System.EventHandler(this.Redraw);
            this.TabStop = false;

            this.ResumeLayout(false);
        }
    }
}
