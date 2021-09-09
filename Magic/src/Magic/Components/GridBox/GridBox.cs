using System;
using System.Drawing;
using System.Windows.Forms;

namespace Magic.Components
{
    public partial class GridBox : Control
    {
        /// <summary>
        /// The dimensions of tiles on this Gridbox (pixels)
        /// </summary>
        public Int32 TileSize { get; set; }
        /// <summary>
        /// The image to show up behind the grid
        /// </summary>
        public Bitmap Display = null;
        /// <summary>
        /// Determines whether or not the grid is to show up on this GridBox
        /// </summary>
        public Boolean ShowGrid
        {
            get
            {
                return _ShowGrid;
            }
            set
            {
                _ShowGrid = value;
                this.Invalidate();
            }
        }
        Boolean _ShowGrid;
        /// <summary>
        /// A rectangle describing the currently selected tiles
        /// </summary>
        public Boolean[,] Selection
        {
            get
            {
                return _Selection;
            }
            set
            {
                _Selection = value;
                this.SelectionChanged(this, null);
                this.Invalidate();
            }
        }
        Boolean[,] _Selection;
        Point LastClick;

        public event EventHandler SelectionChanged = delegate { };



        public GridBox()
        {
            TileSize = 8;

            InitializeComponent();

            SetStyle(ControlStyles.Opaque, true);
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

            Selection = new Boolean[image.Width / TileSize, image.Height / TileSize];
        }
        /// <summary>
        /// Resets the control, losing current selection, and drawing an empty gray image.
        /// </summary>
        public void Reset()
        {
            Display = null;
            Selection = new Boolean[Width / TileSize, Height / TileSize];
        }



        /// <summary>
        /// Returns true if no tiles are selected in the grid
        /// </summary>
        public Boolean SelectionIsEmpty()
        {
            if (Selection == null) return true;
            Int32 width = Selection.GetLength(0);
            Int32 height = Selection.GetLength(1);
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (Selection[x, y]) return false;
            }
            return true;
        }
        /// <summary>
        /// Returns true if a single tile of the grid is selected
        /// </summary>
        public Boolean SelectionIsSingle()
        {
            if (Selection == null) return false;
            Int32 width = Selection.GetLength(0);
            Int32 height = Selection.GetLength(1);
            Boolean match = false;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (match)
                {
                    if (Selection[x, y]) return false;
                }
                else if (Selection[x, y]) match = true;
            }
            return match;
        }
        /// <summary>
        /// Returns the amount of tiles currently selected
        /// </summary>
        public Int32 GetSelectionAmount()
        {
            if (Selection == null) return 0;
            Int32 result = 0;
            Int32 width = Selection.GetLength(0);
            Int32 height = Selection.GetLength(1);
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (Selection[x, y]) result++;
            }
            return result;
        }
        /// <summary>
        /// Returns the X and Y coordinates of the first selected tile encountered or (-1, -1) if nothing found
        /// </summary>
        public Point GetSelectionCoords()
        {
            if (Selection == null) return new Point(-1, -1);
            Int32 width = Selection.GetLength(0);
            Int32 height = Selection.GetLength(1);
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (Selection[x, y]) return new Point(x, y);
            }
            return new Point(-1, -1);
        }
        /// <summary>
        /// Returns the smallest rectangle which contains all currently selected tiles
        /// </summary>
        public Rectangle GetSelectionRectangle()
        {
            Int32 width  = Selection.GetLength(0);
            Int32 height = Selection.GetLength(1);
            Int32 x_min = width;
            Int32 y_min = height;
            Int32 x_max = 0;
            Int32 y_max = 0;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (Selection[x, y])
                {
                    if (x < x_min) x_min = x;
                    if (x > x_max) x_max = x;
                    if (y < y_min) y_min = y;
                    if (y > y_max) y_max = y;
                }
            }
            x_max += 1;
            y_max += 1;
            if (x_min == width && y_min == height && x_max == 0 && y_max == 0) return new Rectangle();
            else return new Rectangle(x_min, y_min, x_max - x_min, y_max - y_min);
        }



        /// <summary>
        /// Selects a tile, or, if SHIFT is held, extends the previous selection
        /// </summary>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            this.Focus();

            if (ModifierKeys == Keys.Shift)
            {
                Selection = new Boolean[Display.Width / TileSize, Display.Height / TileSize];
                Int32 x1 = LastClick.X;
                Int32 y1 = LastClick.Y;
                Int32 x2 = (e.X / TileSize);
                Int32 y2 = (e.Y / TileSize);
                for (Int32 y = y1; (y1 < y2) ? (y <= y2) : (y >= y2); y += (y1 < y2) ? 1 : -1)
                for (Int32 x = x1; (x1 < x2) ? (x <= x2) : (x >= x2); x += (x1 < x2) ? 1 : -1)
                {
                    Selection[x, y] = true;
                }
            }
            else if (ModifierKeys == Keys.Control)
            {
                LastClick = new Point(e.X / TileSize, e.Y / TileSize);
                Selection[LastClick.X, LastClick.Y] = !Selection[LastClick.X, LastClick.Y];
            }
            else
            {
                Selection = new Boolean[Display.Width / TileSize, Display.Height / TileSize];
                LastClick = new Point(e.X / TileSize, e.Y / TileSize);
                Selection[LastClick.X, LastClick.Y] = true;
            }
            base.OnMouseClick(e);
            this.SelectionChanged(this, null);
            this.Invalidate();
        }
        /// <summary>
        /// Allows selecting tiles on the gridbox with the arrowkeys(and shift/ctrl)
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            Int32 offsetX = 0;
            Int32 offsetY = 0;
            switch (e.KeyCode)
            {
                case Keys.Left:  offsetX = -1; break;
                case Keys.Right: offsetX =  1; break;
                case Keys.Up:    offsetY = -1; break;
                case Keys.Down:  offsetY =  1; break;
            }
            if (offsetX == 0 && offsetY == 0) return;
            else
            {
                Int32 width = Selection.GetLength(0);
                Int32 height = Selection.GetLength(1);
                Boolean[,] selection = new Boolean[width, height];
                if (e.Shift) // SHIFT key is pressed
                {
                    for (Int32 y = 0; y < height; y++)
                    for (Int32 x = 0; x < width; x++)
                    {
                        if (x - offsetX >= 0 && x - offsetX < width &&
                            y - offsetY >= 0 && y - offsetY < height &&
                            Selection[x - offsetX, y - offsetY])
                             selection[x, y] = true;
                        else selection[x, y] = Selection[x, y];
                    }
                    Selection = selection;
                }
                else if (e.Control) // CTRL key is pressed
                {
                    for (Int32 y = 0; y < height; y++)
                    for (Int32 x = 0; x < width; x++)
                    {
                        if (x + offsetX >= 0 && x + offsetX < width &&
                            y + offsetY >= 0 && y + offsetY < height)
                             selection[x + offsetX, y + offsetY] = Selection[x, y];
                    }
                    Selection = selection;
                }
                else
                {
                    if (LastClick.X + offsetX >= 0 && LastClick.X + offsetX < width &&
                        LastClick.Y + offsetY >= 0 && LastClick.Y + offsetY < height)
                    {
                        selection[LastClick.X + offsetX, LastClick.Y + offsetY] = true;
                        Selection = selection;
                        LastClick = GetSelectionCoords();
                    }
                }
            }
        }
        protected override Boolean IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                    return true;
                case Keys.Shift | Keys.Right:
                case Keys.Shift | Keys.Left:
                case Keys.Shift | Keys.Up:
                case Keys.Shift | Keys.Down:
                    return true;
                case Keys.Control | Keys.Right:
                case Keys.Control | Keys.Left:
                case Keys.Control | Keys.Up:
                case Keys.Control | Keys.Down:
                    return true;
            }
            return base.IsInputKey(keyData);
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
                    e.Graphics.FillRectangle(gap, 0, 0, Width, Height);
                }
            }
            else
            {
                e.Graphics.DrawImage(Display, 0, 0);
            }

            Int32 width = Width / TileSize;
            Int32 height = Height / TileSize;
            
            using (Pen select = new Pen(SystemColors.Highlight))
            using (Pen normal = new Pen(SystemColors.ControlDarkDark))
            {
                for (Int32 tileY = 0; tileY < height; tileY++)
                for (Int32 tileX = 0; tileX < width; tileX++)
                {
                    if (Selection != null && Selection[tileX, tileY])
                    {
                        e.Graphics.DrawRectangle(select, tileX * TileSize, tileY * TileSize, TileSize - 1, TileSize - 1);
                    }
                    else if (ShowGrid)
                    {
                        e.Graphics.DrawRectangle(normal, tileX * TileSize, tileY * TileSize, TileSize - 1, TileSize - 1);
                    }
                }
            }

            base.OnPaint(e);
        }
    }
}
