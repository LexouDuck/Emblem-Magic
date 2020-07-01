using System;
using System.Drawing;
using System.Windows.Forms;

namespace EmblemMagic.Components
{
    public partial class PaletteBox : Control
    {
        /// <summary>
        /// The GBA.Palette associated with this PaletteBox
        /// </summary>
        public GBA.Palette Colors { get; private set; }
        /// <summary>
        /// Amount of colors to show up on each line for this PaletteBox
        /// </summary>
        public int ColorsPerLine { get; set; }



        public PaletteBox()
        {
            InitializeComponent();

            SetStyle(ControlStyles.Opaque, false);
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        /// <summary>
        /// Loads the given GBA.Palette onto the control.
        /// </summary>
        public void Load(GBA.Palette palette)
        {
            if (palette == null)
                throw new Exception("Palette given is null.");
            Colors = palette;
            this.Invalidate();
        }
        /// <summary>
        /// Resets the control, losing current selection, and drawing all colors as empty/transparent.
        /// </summary>
        public void Reset()
        {
            Colors = null;
            this.Invalidate();
        }

        
        
        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            using (Brush gap = new SolidBrush(SystemColors.Control))
            {
                e.Graphics.FillRectangle(gap, 0, 0, Width, Height);
            }

            int size = Width / ColorsPerLine;
            int x, y;
            int length;
            if (Colors == null)
            {
                length = ColorsPerLine * (Height / size);

                for (int i = 0; i < length; i++)
                {
                    x = (i % ColorsPerLine) * size;
                    y = (i / ColorsPerLine) * size;
                    
                    using (Pen brush = new Pen(SystemColors.AppWorkspace))
                    {
                        e.Graphics.DrawRectangle(brush, x + 1, y + 1, size - 3, size - 3);
                    }
                }
            }
            else
            {
                length = Colors.Count;

                for (int i = 0; i < length; i++)
                {
                    x = (i % ColorsPerLine) * size;
                    y = (i / ColorsPerLine) * size;

                    if (Colors[i].GetAlpha())
                    {
                        using (Pen brush = new Pen(SystemColors.AppWorkspace))
                        {
                            e.Graphics.DrawRectangle(brush, x + 1, y + 1, size - 3, size - 3);
                        }
                    }
                    else
                    {
                        using (Brush brush = new SolidBrush((Color)Colors[i]))
                        {
                            e.Graphics.FillRectangle(brush, x + 1, y + 1, size - 2, size - 2);
                        }
                    }
                }
            }

            base.OnPaint(e);
        }
    }
}
