using System;
using System.Collections.Generic;
using System.Drawing;

namespace GBA
{
    /// <summary>
    /// This class works like a GBA.Tileset, except its tiles are accessed in X Y coordinates
    /// </summary>
    public class TileSheet : Tileset
    {
        /// <summary>
        /// This indexer is what makes the 'Tiles' array accessible.
        /// </summary>
        public Tile this[Int32 x, Int32 y]
        {
            get
            {
                if (x < 0 || x >= this.Width)  throw new ArgumentException("X given is out of bounds: " + x);
                if (y < 0 || y >= this.Height) throw new ArgumentException("Y given is out of bounds: " + y);

                return this.Sheet[x + y * this.Width];
            }
            set
            {
                if (x < 0 || x >= this.Width)  throw new ArgumentException("X given is out of bounds: " + x);
                if (y < 0 || y >= this.Height) throw new ArgumentException("Y given is out of bounds: " + y);

                this.Sheet[x + y * this.Width] = value;
            }
        }

        /// <summary>
        /// The Width of this TileSheet, in tiles
        /// </summary>
        public Int32 Width { get; set; }
        /// <summary>
        /// The Height of this TileSheet, in tiles
        /// </summary>
        public Int32 Height { get; set; }



        /// <summary>
        /// Creates an empty GBA.TileSheet with the given dimensions
        /// </summary>
        public TileSheet(Int32 width, Int32 height) : base(width * height)
        {
            this.Width = width;
            this.Height = height;
            for (Int32 i = 0; i < width * height; i++)
            {
                this.Sheet.Add(null);
            }
        }



        /// <summary>
        /// Checks if the tileset contains any flipped version of the given tile, returns null if nothing is found
        /// </summary>
        new public Tuple<Point, Boolean, Boolean> FindMatch(Tile tile)
        {
            Int32 index = this.Find(tile);

            if (index == -1)
            {
                index = this.Find(tile.FlipHorizontal());

                if (index == -1)
                {
                    index = this.Find(tile.FlipVertical());

                    if (index == -1)
                    {
                        index = this.Find(tile.FlipHorizontal().FlipVertical());

                        if (index == -1)
                        {
                            return null;
                        }
                        else return Tuple.Create(new Point(index % this.Width, index / this.Width), true, true);
                    }
                    else return Tuple.Create(new Point(index % this.Width, index / this.Width), false, true);
                }
                else return Tuple.Create(new Point(index % this.Width, index / this.Width), true, false);
            }
            else return Tuple.Create(new Point(index % this.Width, index / this.Width), false, false);
        }
        /// <summary>
        /// Returns a array of points at which to put each OAM on this TileSheet, or null if they can't all fit
        /// </summary>
        public Point[] CheckIfFits(List<Tuple<Point, Size>> OAMs)
        {
            Point[] result = new Point[OAMs.Count];
            Point position;
            for (Int32 i = 0; i < OAMs.Count; i++)
            {
                position = this.CheckIfFits(OAMs[i].Item2);

                if (position == new Point(-1, -1))
                {
                    for (Int32 j = 0; j < this.Sheet.Count; j++)
                    {
                        if (this.Sheet[j] == Tile.Empty) this.Sheet[j] = null;
                    }
                    return null;
                }
                else result[i] = position;
                
                for (Int32 x = 0; x < OAMs[i].Item2.Width; x++)
                for (Int32 y = 0; y < OAMs[i].Item2.Height; y++)
                {
                    this[position.X + x, position.Y + y] = Tile.Empty;
                }
            }
            for (Int32 j = 0; j < this.Sheet.Count; j++)
            {
                if (this.Sheet[j] == Tile.Empty) this.Sheet[j] = null;
            }
            return result;
        }
        /// <summary>
        /// Returns where to put an OAM of the given size on this TileSheet, or Point(-1,-1) if it can't fit
        /// </summary>
        public Point CheckIfFits(Size oam)
        {
            for (Int32 x = 0; x < this.Width; x++)
            for (Int32 y = 0; y < this.Height; y++)
            {
                for (Int32 tileY = 0; tileY < oam.Height; tileY++)
                for (Int32 tileX = 0; tileX < oam.Width; tileX++)
                {
                    try
                    {
                        if (this[x + tileX, y + tileY] != null)
                            goto Continue;
                    }
                    catch { goto Continue; }
                }

                return new Point(x, y);

                Continue: continue;
            }
            return new Point(-1, -1);
        }



        /// <summary>
        /// Returns a GBA.Image of this GBA.Tileset, with the given palette.
        /// </summary>
        public Image ToImage(Palette colors)
        {
            Int32 width = Tile.SIZE * this.Width;
            Int32 height = Tile.SIZE * this.Height;
            Byte[] result = new Byte[(width / 2) * height];
            Byte[] tile;
            Int32 ix, iy;
            Int32 index = 0;
            Int32 t = 0;
            for (Int32 i = 0; i < this.Count; i++)
            {
                ix = (i % this.Width) * Tile.SIZE;
                iy = (i / this.Width) * Tile.SIZE;
                tile = this.Sheet[i].Bytes;
                index = (ix / 2) + (iy * (width / 2));
                t = 0;
                for (Int32 y = 0; y < Tile.SIZE; y++)
                {
                    for (Int32 x = 0; x < Tile.SIZE; x += 2)
                    {
                        result[index++] = tile[t++];
                    }
                    index += (width - Tile.SIZE) / 2;
                }
            }
            return new Image(width, height, colors.ToBytes(false), result);
        }
    }
}
