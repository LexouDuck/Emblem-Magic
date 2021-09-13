using Magic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Magic.Components
{
    public class ImageBox : Control
    {
        /// <summary>
        /// The image that this ImageBox will show. GBA.Bitmap, GBA.Image, and GBA.Sprite implement IDisplayable
        /// </summary>
        public IDisplayable Display { get; private set; }



        public ImageBox()
        {
            this.InitializeComponent();

            this.Display = null;

            this.SetStyle(ControlStyles.Opaque, false);
            this.SetStyle(ControlStyles.Selectable, false);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }
        /// <summary>
        /// Loads the image given to display it onto this ImageBox.
        /// </summary>
        public void Load(IDisplayable image)
        {
            this.Display = image;
            this.Invalidate();
        }
        /// <summary>
        /// Resets the dispay of this ImageBox (show the default gray rectangle)
        /// </summary>
        public void Reset()
        {
            this.Display = null;
            this.Invalidate();
        }



        /// <summary>
        /// An event handler that calls the 'OnPaint' method
        /// </summary>
        void Redraw(Object sender, System.EventArgs e)
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
                if (this.Display == null)
                {
                    using (Brush outline = new SolidBrush(this.ForeColor))
                    {
                        e.Graphics.FillRectangle(outline,
                            0, 0,
                            this.Width,
                            this.Height);
                    }
                    using (Brush filling = new SolidBrush(this.BackColor))
                    {
                        e.Graphics.FillRectangle(filling,
                            1, 1,
                            this.Width  - 2,
                            this.Height - 2);
                    }
                }
                else
                {
                    Int32 offsetX = (this.Width  / 2) - (this.Display.Width  / 2);
                    Int32 offsetY = (this.Height / 2) - (this.Display.Height / 2);
                    Color pixel;
                    Brush color;
                    for (Int32 y = 0; y < this.Display.Height; y++)
                    for (Int32 x = 0; x < this.Display.Width; x++)
                    {
                        pixel = (Color)this.Display.GetColor(x, y);
                        color = new SolidBrush(pixel);

                        e.Graphics.FillRectangle(color,
                            offsetX + x,
                            offsetY + y,
                            1, 1);
                    }
                }
                base.OnPaint(e);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error updating the ImageBox.", ex);
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
            this.Name = "ImageBox";
            this.Size = new System.Drawing.Size(this.Width, this.Height);
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
        protected override void Dispose(Boolean disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}