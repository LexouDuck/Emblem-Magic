using System;
using System.Drawing;
using System.Windows.Forms;

namespace Magic.Components
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
        public Int32 ColorsPerLine { get; set; }



        public PaletteBox()
        {
            this.InitializeComponent();

            this.SetStyle(ControlStyles.Opaque, false);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        /// <summary>
        /// Loads the given GBA.Palette onto the control.
        /// </summary>
        public void Load(GBA.Palette palette)
        {
            if (palette == null)
                throw new Exception("Palette given is null.");
            this.Colors = palette;
            this.Invalidate();
        }
        /// <summary>
        /// Resets the control, losing current selection, and drawing all colors as empty/transparent.
        /// </summary>
        public void Reset()
        {
            this.Colors = null;
            this.Invalidate();
        }

        
        
        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            using (Brush gap = new SolidBrush(SystemColors.Control))
            {
                e.Graphics.FillRectangle(gap, 0, 0, this.Width, this.Height);
            }

            Int32 size = this.Width / this.ColorsPerLine;
            Int32 x, y;
            Int32 length;
            if (this.Colors == null)
            {
                length = this.ColorsPerLine * (this.Height / size);

                for (Int32 i = 0; i < length; i++)
                {
                    x = (i % this.ColorsPerLine) * size;
                    y = (i / this.ColorsPerLine) * size;
                    
                    using (Pen brush = new Pen(SystemColors.AppWorkspace))
                    {
                        e.Graphics.DrawRectangle(brush, x + 1, y + 1, size - 3, size - 3);
                    }
                }
            }
            else
            {
                length = this.Colors.Count;

                for (Int32 i = 0; i < length; i++)
                {
                    x = (i % this.ColorsPerLine) * size;
                    y = (i / this.ColorsPerLine) * size;

                    if (this.Colors[i].GetAlpha())
                    {
                        using (Pen brush = new Pen(SystemColors.AppWorkspace))
                        {
                            e.Graphics.DrawRectangle(brush, x + 1, y + 1, size - 3, size - 3);
                        }
                    }
                    else
                    {
                        using (Brush brush = new SolidBrush((Color)this.Colors[i]))
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
