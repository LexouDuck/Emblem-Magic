using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace Magic.Components
{
    /// <summary>
    /// A control that shows ranges as rectangles in a bar, on a given total ratio
    /// </summary>
    public class MarkedSpaceBar : Control
    {
        /// <summary>
        /// The total number used to calculate a ratio so as to get the rectangles to show up in their place.
        /// </summary>
        public UInt32 Total;
        /// <summary>
        /// The first tuple item, Space, stores Marking info and original space start/end.
        /// The second tuple item, Range, stores the rectangle coordinates on the bar in pixels
        /// </summary>
        public List<Tuple<Space, Range>> Ranges;

        public MarkedSpaceBar()
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            Ranges = new List<Tuple<Space, Range>>();
        }


        /// <summary>
        /// Resets the data bindings of this SpaceMarkingBar
        /// </summary>
        /// <param name="total">The total this bar respresents</param>
        /// <param name="list">The list of spaces to show up</param>
        public void Load(uint total, List<Space> list)
        {
            Total = total;
            Ranges.Clear();
            foreach (Space space in list)
            {
                this.AddMarkedSpace(new Space(space.Marked, space.Address, space.EndByte));
            }
            this.Invalidate();
        }

        void UpdateRanges()
        {
            int offset;
            int endoff;
            float ratio;
            Space space;
            for (int i = 0; i < Ranges.Count; i++)
            {
                space = Ranges[i].Item1;

                ratio = (float)(int)space.Address / (float)Total;
                offset = (int)(ratio * Width);

                ratio = (float)(int)space.EndByte / (float)Total;
                endoff = Math.Max(offset + 1, (int)(ratio * Width));

                Ranges[i] = Tuple.Create(space, new Range(offset, endoff));
            }
        }
        void AddMarkedSpace(Space space)
        {
            int offset;
            int endoff;
            float ratio;
            ratio = (float)(int)space.Address / (float)Total;
            offset = (int)(ratio * Width);

            ratio = (float)(int)space.EndByte / (float)Total;
            endoff = Math.Max(offset + 1, (int)(ratio * Width));

            Ranges.Add(Tuple.Create(space, new Range(offset, endoff)));
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            UpdateRanges();

            using (Brush background = new SolidBrush(this.BackColor))
            {   // Create a brush that will draw the background of the bar
                e.Graphics.FillRectangle(background, 0, 0, Width, Height);

                foreach (var range in Ranges)
                {
                    using (LinearGradientBrush marked = new LinearGradientBrush(
                        new Rectangle(0, 0, Width, Height),
                        Color.White, range.Item1.Marked.Color, LinearGradientMode.Vertical))
                    {
                        e.Graphics.FillRectangle(marked, range.Item2.Start, 1, range.Item2.Length, Height - 1);
                    }   // and a gradient for the marked bits themselves
                }
            }
        }

        #region Designer Code

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

            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.Name = "MarkedSpaceBar";
            this.Size = new System.Drawing.Size(235, 37);
            this.SizeChanged += new System.EventHandler(this.Redraw);

            this.ResumeLayout(false);
        }
        #endregion
    }
}
