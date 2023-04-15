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
            this.ShowGrid = false;
            this.TileSize = 8;
            this.Hovered = new Point();

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
        public void Load(IDisplayable image)
        {
            if (image == null)
            {
                this.Display = null;
                this.Invalidate();
                return;
            }
            if (image.Width % this.TileSize != 0)
                throw new Exception("Image width is not a multiple of " + this.TileSize);
            if (image.Height % this.TileSize != 0)
                throw new Exception("Image height is not a multiple of " + this.TileSize);

            this.Width = image.Width;
            this.Height = image.Height;
            this.Display = new Bitmap(this.Width, this.Height);
            for (Int32 y = 0; y < this.Height; y++)
            for (Int32 x = 0; x < this.Width; x++)
            {
                    this.Display.SetPixel(x, y, (System.Drawing.Color)image.GetColor(x, y));
            }

            this.Hovered = new Point();
            this.Hover = null;

            this.Invalidate();
        }
        /// <summary>
        /// Resets the control, losing current selection, and drawing all colors as empty/transparent.
        /// </summary>
        public void Reset()
        {
            this.Display = null;

            this.Hovered = new Point();
            this.Hover = null;

            this.Invalidate();
        }


        
        /// <summary>
        /// Redraws the control based on which tile is being hovered over
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            this.Hovered = new Point(e.X / this.TileSize, e.Y / this.TileSize);

            this.Invalidate();
        }
        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Display == null)
            {
                using (Brush gap = new SolidBrush(SystemColors.AppWorkspace))
                {
                    e.Graphics.FillRectangle(gap, 0, 0, 240, 160);
                }
                return;
            }
            else
            {
                e.Graphics.DrawImage(this.Display, 0, 0);
            }
            if (this.ShowGrid)
            {
                using (Pen select = new Pen(SystemColors.Highlight))
                using (Pen normal = new Pen(SystemColors.ControlDarkDark))
                {
                    Int32 width = this.Width / this.TileSize;
                    Int32 height = this.Height / this.TileSize;

                    for (Int32 y = 0; y < height; y++)
                    for (Int32 x = 0; x < width; x++)
                    {
                        if (this.Hover != null &&
                            x >= this.Hovered.X && x < this.Hovered.X + this.Hover.GetLength(0) &&
                            y >= this.Hovered.Y && y < this.Hovered.Y + this.Hover.GetLength(1) &&
                            this.Hover[x - this.Hovered.X, y - this.Hovered.Y])
                        {
                            e.Graphics.DrawRectangle(select,
                                x * this.TileSize,
                                y * this.TileSize,
                                this.TileSize - 1, this.TileSize - 1);
                        }
                        else
                        {
                            e.Graphics.DrawRectangle(normal,
                                x * this.TileSize,
                                y * this.TileSize,
                                this.TileSize - 1, this.TileSize - 1);
                        }
                    }
                }
            }
            else if (this.Hover != null)
            {
                using (Pen select = new Pen(SystemColors.Highlight))
                {
                    for (Int32 y = 0; y < this.Hover.GetLength(1); y++)
                    for (Int32 x = 0; x < this.Hover.GetLength(0); x++)
                    {
                        if (this.Hover[x, y])
                        {
                            e.Graphics.DrawRectangle(select,
                                (this.Hovered.X + x) * this.TileSize,
                                (this.Hovered.Y + y) * this.TileSize,
                                this.TileSize - 1, this.TileSize - 1);
                        }
                    }
                }
            }
            base.OnPaint(e);
        }
    }
}
