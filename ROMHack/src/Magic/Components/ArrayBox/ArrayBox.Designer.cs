namespace Magic.Components
{
    partial class ArrayBox<T>
    {
        /// <summary>
        /// An event handler that calls the 'OnPaint' method
        /// </summary>
        protected void Redraw(object sender, System.EventArgs e)
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
        protected void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ArrayBox
            // 
            this.Name = "ArrayBox";
            this.Size = new System.Drawing.Size(100, 26);
            this.MinimumSize = new System.Drawing.Size(100, 26);
            this.MaximumSize = new System.Drawing.Size(10000, 26);
            this.SizeChanged += new System.EventHandler(this.Redraw);
            // 
            // NumberBox
            // 
            this.EntryValueBox.Location = new System.Drawing.Point(2, 2);
            this.EntryValueBox.Size = new System.Drawing.Size(40, 26);
            this.EntryValueBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
            // 
            // ListEntry
            // 
            this.EntryComboBox.Location = new System.Drawing.Point(48, 2);
            this.EntryComboBox.Size = new System.Drawing.Size(50, 26);
            this.EntryComboBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.EntryComboBox.FormattingEnabled = true;
            
            this.Controls.Add(EntryValueBox);
            this.Controls.Add(EntryComboBox);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        /// <summary>
        /// The entry selector control
        /// </summary>
        protected System.Windows.Forms.NumericUpDown EntryValueBox;
        /// <summary>
        /// The name of the current entry
        /// </summary>
        protected System.Windows.Forms.ComboBox EntryComboBox;
    }
}
