using EmblemMagic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EmblemMagic.Components
{
    public class ImageBox : Control
    {
        /// <summary>
        /// The image that this ImageBox will show. GBA.Bitmap, GBA.Image, and GBA.Sprite implement IDisplayable
        /// </summary>
        public IDisplayable Display { get; private set; }



        public ImageBox()
        {
            InitializeComponent();

            Display = null;

            SetStyle(ControlStyles.Opaque, false);
            SetStyle(ControlStyles.Selectable, false);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }
        /// <summary>
        /// Loads the image given to display it onto this ImageBox.
        /// </summary>
        public void Load(IDisplayable image)
        {
            Display = image;
            this.Invalidate();
        }
        /// <summary>
        /// Resets the dispay of this ImageBox (show the default gray rectangle)
        /// </summary>
        public void Reset()
        {
            Display = null;
            this.Invalidate();
        }



        /// <summary>
        /// An event handler that calls the 'OnPaint' method
        /// </summary>
        void Redraw(object sender, System.EventArgs e)
        {
            this.Invalidate();
        }
        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                if (Display == null)
                {
                    using (Brush outline = new SolidBrush(this.ForeColor))
                    {
                        e.Graphics.FillRectangle(outline, 0, 0, Width, Height);
                    }
                    using (Brush filling = new SolidBrush(this.BackColor))
                    {
                        e.Graphics.FillRectangle(filling, 1, 1, Width - 2, Height - 2);
                    }
                }
                else
                {
                    int offsetX = (Width / 2) - (Display.Width / 2);
                    int offsetY = (Height / 2) - (Display.Height / 2);
                    Color pixel;
                    Brush color;
                    for (int y = 0; y < Display.Height; y++)
                    for (int x = 0; x < Display.Width; x++)
                    {
                        pixel = (Color)Display.GetColor(x, y);
                        color = new SolidBrush(pixel);

                        e.Graphics.FillRectangle(color, offsetX + x, offsetY + y, 1, 1);
                    }
                }

                base.OnPaint(e);
            }
            catch (Exception ex)
            {
                Program.ShowError("There has been an error updating the ImageBox.", ex);
            }
        }



        /// <summary> 
        /// Required method for Designer support
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.BackColor = System.Drawing.SystemColors.Control;
            this.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.Name = "GBAImageBox";
            this.Size = new System.Drawing.Size(Width, Height);
            this.TabStop = false;

            this.ResumeLayout(false);
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
    }
}