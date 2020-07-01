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
        public Tile this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= Width)  throw new ArgumentException("X given is out of bounds: " + x);
                if (y < 0 || y >= Height) throw new ArgumentException("Y given is out of bounds: " + y);

                return Sheet[x + y * Width];
            }
            set
            {
                if (x < 0 || x >= Width)  throw new ArgumentException("X given is out of bounds: " + x);
                if (y < 0 || y >= Height) throw new ArgumentException("Y given is out of bounds: " + y);

                Sheet[x + y * Width] = value;
            }
        }

        /// <summary>
        /// The Width of this TileSheet, in tiles
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// The Height of this TileSheet, in tiles
        /// </summary>
        public int Height { get; set; }



        /// <summary>
        /// Creates an empty GBA.TileSheet with the given dimensions
        /// </summary>
        public TileSheet(int width, int height) : base(width * height)
        {
            Width = width;
            Height = height;
            for (int i = 0; i < width * height; i++)
            {
                Sheet.Add(null);
            }
        }



        /// <summary>
        /// Checks if the tileset contains any flipped version of the given tile, returns null if nothing is found
        /// </summary>
        new public Tuple<Point, bool, bool> FindMatch(Tile tile)
        {
            int index = Find(tile);

            if (index == -1)
            {
                index = Find(tile.FlipHorizontal());

                if (index == -1)
                {
                    index = Find(tile.FlipVertical());

                    if (index == -1)
                    {
                        index = Find(tile.FlipHorizontal().FlipVertical());

                        if (index == -1)
                        {
                            return null;
                        }
                        else return Tuple.Create(new Point(index % Width, index / Width), true, true);
                    }
                    else return Tuple.Create(new Point(index % Width, index / Width), false, true);
                }
                else return Tuple.Create(new Point(index % Width, index / Width), true, false);
            }
            else return Tuple.Create(new Point(index % Width, index / Width), false, false);
        }
        /// <summary>
        /// Returns a array of points at which to put each OAM on this TileSheet, or null if they can't all fit
        /// </summary>
        public Point[] CheckIfFits(List<Tuple<Point, Size>> OAMs)
        {
            Point[] result = new Point[OAMs.Count];
            Point position;
            for (int i = 0; i < OAMs.Count; i++)
            {
                position = CheckIfFits(OAMs[i].Item2);

                if (position == new Point(-1, -1))
                {
                    for (int j = 0; j < Sheet.Count; j++)
                    {
                        if (Sheet[j] == Tile.Empty) Sheet[j] = null;
                    }
                    return null;
                }
                else result[i] = position;
                
                for (int x = 0; x < OAMs[i].Item2.Width; x++)
                for (int y = 0; y < OAMs[i].Item2.Height; y++)
                {
                    this[position.X + x, position.Y + y] = Tile.Empty;
                }
            }
            for (int j = 0; j < Sheet.Count; j++)
            {
                if (Sheet[j] == Tile.Empty) Sheet[j] = null;
            }
            return result;
        }
        /// <summary>
        /// Returns where to put an OAM of the given size on this TileSheet, or Point(-1,-1) if it can't fit
        /// </summary>
        public Point CheckIfFits(Size oam)
        {
            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
            {
                for (int tileY = 0; tileY < oam.Height; tileY++)
                for (int tileX = 0; tileX < oam.Width; tileX++)
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
            int width = Tile.SIZE * Width;
            int height = Tile.SIZE * Height;
            byte[] result = new byte[(width / 2) * height];
            byte[] tile;
            int ix, iy;
            int index = 0;
            int t = 0;
            for (int i = 0; i < Count; i++)
            {
                ix = (i % Width) * Tile.SIZE;
                iy = (i / Width) * Tile.SIZE;
                tile = Sheet[i].Bytes;
                index = (ix / 2) + (iy * (width / 2));
                t = 0;
                for (int y = 0; y < Tile.SIZE; y++)
                {
                    for (int x = 0; x < Tile.SIZE; x += 2)
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
