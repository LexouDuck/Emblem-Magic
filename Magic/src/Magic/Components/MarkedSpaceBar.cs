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
            this.InitializeComponent();

            this.DoubleBuffered = true;

            this.Ranges = new List<Tuple<Space, Range>>();
        }


        /// <summary>
        /// Resets the data bindings of this SpaceMarkingBar
        /// </summary>
        /// <param name="total">The total this bar respresents</param>
        /// <param name="list">The list of spaces to show up</param>
        public void Load(UInt32 total, List<Space> list)
        {
            this.Total = total;
            this.Ranges.Clear();
            foreach (Space space in list)
            {
                this.AddMarkedSpace(new Space(space.Marked, space.Address, space.EndByte));
            }
            this.Invalidate();
        }

        void UpdateRanges()
        {
            Int32 offset;
            Int32 endoff;
            Single ratio;
            Space space;
            for (Int32 i = 0; i < this.Ranges.Count; i++)
            {
                space = this.Ranges[i].Item1;

                ratio = (Single)(Int32)space.Address / (Single)this.Total;
                offset = (Int32)(ratio * this.Width);

                ratio = (Single)(Int32)space.EndByte / (Single)this.Total;
                endoff = Math.Max(offset + 1, (Int32)(ratio * this.Width));

                this.Ranges[i] = Tuple.Create(space, new Range(offset, endoff));
            }
        }
        void AddMarkedSpace(Space space)
        {
            Int32 offset;
            Int32 endoff;
            Single ratio;
            ratio = (Single)(Int32)space.Address / (Single)this.Total;
            offset = (Int32)(ratio * this.Width);

            ratio = (Single)(Int32)space.EndByte / (Single)this.Total;
            endoff = Math.Max(offset + 1, (Int32)(ratio * this.Width));

            this.Ranges.Add(Tuple.Create(space, new Range(offset, endoff)));
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            this.UpdateRanges();

            using (Brush background = new SolidBrush(this.BackColor))
            {   // Create a brush that will draw the background of the bar
                e.Graphics.FillRectangle(background, 0, 0, this.Width, this.Height);

                foreach (var range in this.Ranges)
                {
                    using (LinearGradientBrush marked = new LinearGradientBrush(
                        new Rectangle(0, 0, this.Width, this.Height),
                        Color.White, range.Item1.Marked.Color, LinearGradientMode.Vertical))
                    {
                        e.Graphics.FillRectangle(marked, range.Item2.Start, 1, range.Item2.Length, this.Height - 1);
                    }   // and a gradient for the marked bits themselves
                }
            }
        }

        #region Designer Code

        /// <summary>
        /// An event handler that calls the 'OnPaint' method
        /// </summary>
        void Redraw(Object sender, EventArgs e)
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
        protected override void Dispose(Boolean disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
