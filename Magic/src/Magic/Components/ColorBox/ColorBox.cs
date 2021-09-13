using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Magic.Components
{
    public partial class ColorBox : Control
    {
        /// <summary>
        /// The actual color of this ColorBox, setting this affects the display
        /// </summary>
        public Color Color
        {
            get
            {
                return this._color;
            }
            set
            {
                this._color = value;
                this.Invalidate();
                return;
            }
        } Color _color;

        /// <summary>
        /// Whether or not this ColorBox should be drawn with a gradient to white
        /// </summary>
        public System.Boolean Gradient
        {
            get
            {
                return this._gradient;
            }
            set
            {
                this._gradient = value;
                this.Invalidate();
                return;
            }
        }
        System.Boolean _gradient;
        /// <summary>
        /// Whether or not to show a contour around the ColorBox
        /// </summary>
        public System.Boolean Selected
        {
            get
            {
                return this._selected;
            }
            set
            {
                this._selected = value;
                this.Invalidate();
                return;
            }
        }
        System.Boolean _selected;



        public ColorBox()
        {
            this.InitializeComponent();

            this.SetStyle(ControlStyles.Opaque, false);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (Brush bg = new SolidBrush(SystemColors.Control))
            {
                e.Graphics.FillRectangle(bg, 0, 0, this.Width, this.Height);
            }
            if (this.Selected)
            {
                using (Pen outline = new Pen(SystemColors.Highlight))
                {
                    e.Graphics.DrawRectangle(outline, 0, 0, this.Width - 1, this.Height - 1);
                }
            }
            
            if (this.Color == Color.Empty || this.Color.A == 0)
            using (Pen outline = new Pen(SystemColors.AppWorkspace))
            {
                e.Graphics.DrawRectangle(outline, 2, 2, this.Width - 4, this.Height - 4);
            }

            if (this.Gradient)
            using (LinearGradientBrush color = new LinearGradientBrush(
                new Rectangle(0, 0, this.Width, this.Height), Color.White, this.Color, LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(color, 2, 2, this.Width - 4, this.Height - 4);
            } 
            else using (Brush color = new SolidBrush(this.Color))
            {
                e.Graphics.FillRectangle(color, 2, 2, this.Width - 4, this.Height - 4);
            }
        }
    }
}