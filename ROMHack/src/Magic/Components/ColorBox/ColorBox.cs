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
                return _color;
            }
            set
            {
                _color = value;
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
                return _gradient;
            }
            set
            {
                _gradient = value;
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
                return _selected;
            }
            set
            {
                _selected = value;
                this.Invalidate();
                return;
            }
        }
        System.Boolean _selected;



        public ColorBox()
        {
            InitializeComponent();

            SetStyle(ControlStyles.Opaque, false);
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (Brush bg = new SolidBrush(SystemColors.Control))
            {
                e.Graphics.FillRectangle(bg, 0, 0, Width, Height);
            }
            if (Selected)
            {
                using (Pen outline = new Pen(SystemColors.Highlight))
                {
                    e.Graphics.DrawRectangle(outline, 0, 0, Width - 1, Height - 1);
                }
            }
            
            if (Color == Color.Empty || Color.A == 0)
            using (Pen outline = new Pen(SystemColors.AppWorkspace))
            {
                e.Graphics.DrawRectangle(outline, 2, 2, Width - 4, Height - 4);
            }

            if (Gradient)
            using (LinearGradientBrush color = new LinearGradientBrush(
                new Rectangle(0, 0, Width, Height), Color.White, this.Color, LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(color, 2, 2, Width - 4, Height - 4);
            } 
            else using (Brush color = new SolidBrush(this.Color))
            {
                e.Graphics.FillRectangle(color, 2, 2, Width - 4, Height - 4);
            }
        }
    }
}