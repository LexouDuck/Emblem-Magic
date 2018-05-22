using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using EmblemMagic.FireEmblem;

namespace EmblemMagic.Components
{
    /// <summary>
    /// A control that displays the waveform of a sample in 8-bit signed PCM data
    /// </summary>
    public class SampleBox : Control
    {
        int Loop;
        byte[] Data;

        public SampleBox()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
        }
        public void Load(Sample sample)
        {
            Loop = sample.Looped ? (int)sample.LoopStart : -1;
            Data = sample.PCM_Data;

            this.Invalidate();
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (Brush background = new SolidBrush(this.BackColor))
            {   // Create a brush that will draw the background of the bar
                e.Graphics.FillRectangle(background, 0, 0, Width, Height);
            }

            if (Data != null)
            {
                using (Pen wave = new Pen(this.ForeColor))
                {
                    float amplitude;
                    int width = this.Width;
                    int height = this.Height / 2;
                    for (int i = 0; i < width; i++)
                    {
                        amplitude = ((Data[i * Data.Length / Width] - 128) / (float)128) * height;
                        e.Graphics.DrawLine(wave,
                            i, height + amplitude,
                            i, height + amplitude * -1);
                    }
                }
                if (Loop != -1)
                {
                    using (Pen loop = new Pen(SystemColors.Highlight))
                    {
                        e.Graphics.DrawLine(loop,
                            Loop * Width / Data.Length, 0,
                            Loop * Width / Data.Length, this.Height);
                    }
                }
            }
        }

        #region Component Designer generated code

        /// <summary>
        /// An event handler that calls the 'OnPaint' method
        /// </summary>
        void Redraw(object sender, EventArgs e)
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

            this.BackColor = SystemColors.AppWorkspace;
            this.ForeColor = SystemColors.ControlLight;
            this.Name = "SampleBox";
            this.Size = new Size(200, 40);
            this.SizeChanged += new System.EventHandler(this.Redraw);

            this.ResumeLayout(false);
        }
        #endregion
    }
}
