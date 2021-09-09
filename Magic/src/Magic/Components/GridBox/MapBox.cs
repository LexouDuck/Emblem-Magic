using System;
using System.Drawing;
using System.Windows.Forms;

namespace Magic.Components
{
    public partial class MapBox : Control
    {
        /// <summary>
        /// Determines whether or not the grid is to show up on this MapBox
        /// </summary>
        public Boolean ShowGrid { get; set; }
        /// <summary>
        /// The dimensions of tiles on this MapBox (pixels)
        /// </summary>
        public Int32 TileSize { get; set; }
        /// <summary>
        /// The image to show up behind the grid
        /// </summary>
        public Bitmap Display = null;
        /// <summary>
        /// The coordinates of the tile that the mouse is currently over
        /// </summary>
        public Point Hovered { get; set; }
        /// <summary>
        /// A boolean map of the hover-highlighted tiles
        /// </summary>
        public Boolean[,] Hover { get; set; }



        public MapBox()
        {
            ShowGrid = false;
            TileSize = 8;
            Hovered = new Point();

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
        public void Load(IDisplayable image)
        {
            if (image == null)
            {
                Display = null;
                this.Invalidate();
                return;
            }
            if (image.Width % TileSize != 0)
                throw new Exception("Image width is not a multiple of " + TileSize);
            if (image.Height % TileSize != 0)
                throw new Exception("Image height is not a multiple of " + TileSize);

            Width = image.Width;
            Height = image.Height;
            Display = new Bitmap(Width, Height);
            for (Int32 y = 0; y < Height; y++)
            for (Int32 x = 0; x < Width; x++)
            {
                Display.SetPixel(x, y, (System.Drawing.Color)image.GetColor(x, y));
            }

            Hovered = new Point();
            Hover = null;

            this.Invalidate();
        }
        /// <summary>
        /// Resets the control, losing current selection, and drawing all colors as empty/transparent.
        /// </summary>
        public void Reset()
        {
            Display = null;

            Hovered = new Point();
            Hover = null;

            this.Invalidate();
        }


        
        /// <summary>
        /// Redraws the control based on which tile is being hovered over
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            Hovered = new Point(e.X / TileSize, e.Y / TileSize);

            this.Invalidate();
        }
        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (Display == null)
            {
                using (Brush gap = new SolidBrush(SystemColors.AppWorkspace))
                {
                    e.Graphics.FillRectangle(gap, 0, 0, 240, 160);
                }
                return;
            }
            else
            {
                e.Graphics.DrawImage(Display, 0, 0);
            }
            if (ShowGrid)
            {
                using (Pen select = new Pen(SystemColors.Highlight))
                using (Pen normal = new Pen(SystemColors.ControlDarkDark))
                {
                    Int32 width = Width / TileSize;
                    Int32 height = Height / TileSize;

                    for (Int32 y = 0; y < height; y++)
                    for (Int32 x = 0; x < width; x++)
                    {
                        if (Hover != null &&
                            x >= Hovered.X && x < Hovered.X + Hover.GetLength(0) &&
                            y >= Hovered.Y && y < Hovered.Y + Hover.GetLength(1) &&
                            Hover[x - Hovered.X, y - Hovered.Y])
                        {
                            e.Graphics.DrawRectangle(select,
                                x * TileSize,
                                y * TileSize,
                                TileSize - 1, TileSize - 1);
                        }
                        else
                        {
                            e.Graphics.DrawRectangle(normal,
                                x * TileSize,
                                y * TileSize,
                                TileSize - 1, TileSize - 1);
                        }
                    }
                }
            }
            else if (Hover != null)
            {
                using (Pen select = new Pen(SystemColors.Highlight))
                {
                    for (Int32 y = 0; y < Hover.GetLength(1); y++)
                    for (Int32 x = 0; x < Hover.GetLength(0); x++)
                    {
                        if (Hover[x, y])
                        {
                            e.Graphics.DrawRectangle(select,
                                (Hovered.X + x) * TileSize,
                                (Hovered.Y + y) * TileSize,
                                TileSize - 1, TileSize - 1);
                        }
                    }
                }
            }
            base.OnPaint(e);
        }
    }
}
