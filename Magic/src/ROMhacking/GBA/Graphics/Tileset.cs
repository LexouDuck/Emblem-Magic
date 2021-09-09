using Compression;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GBA
{
    /// <summary>
    /// A GBA.Tileset is a collection of GBA.Tiles which themselves are simply 32byte-long byte arrays
    /// </summary>
    public class Tileset
    {
        /// <summary>
        /// This indexer is what makes the 'Tiles' array accessible.
        /// </summary>
        public Tile this[Int32 index]
        {
            get
            {
                if (index < 0 || index >= Count) return Tile.Empty;
                    //throw new Exception("Given index " + index + " exceeds this GBA.Tileset (" + Count + " tiles)");

                return Sheet[index];
            }
            set
            {
                if (index < 0 || index >= Count)
                    throw new Exception("Given index " + index + " exceeds this GBA.Tileset (" + Count + " tiles)");

                Sheet[index] = value;
            }
        }

        /// <summary>
        /// Gets the amount of tiles in this GBA.Tileset
        /// </summary>
        public Int32 Count
        {
            get
            {
                return Sheet.Count;
            }
        }
        /// <summary>
        /// Gets the maximum amount of tiles this Tileset can contain
        /// </summary>
        public Int32 Maximum
        {
            get
            {
                return Sheet.Capacity;
            }
        }



        /// <summary>
        /// The array holding the tiles in the correct order.
        /// </summary>
        protected List<Tile> Sheet { get; set; }



        /// <summary>
        /// Creates an empty GBA.Tileset with the given Tile.SIZE
        /// </summary>
        public Tileset(Int32 maximum = 0)
        {
            Load(maximum);
        }
        /// <summary>
        /// Creates a GBA.Tileset of 8x8 tiles from the given tile data (must be a multiple of 32bytes long)
        /// </summary>
        public Tileset(Byte[] data, Int32 maximum = 0)
        {
            if (data.Length % Tile.LENGTH != 0)
                throw new Exception("the data given has an invalid length.");
            
            Load(maximum);

            Byte[] buffer = new Byte[Tile.LENGTH];
            for (Int32 i = 0; i < (data.Length / Tile.LENGTH); i++)
            {
                Array.Copy(data, i * Tile.LENGTH, buffer, 0, Tile.LENGTH);
                Sheet.Add(new Tile(buffer));
            }
        }
        /// <summary>
        /// Creates a Tileset of size x size tiles, by parsing through a GBA.Image (adding every tile)
        /// </summary>
        public Tileset(Image image, Int32 maximum = 0)
        {
            Load(maximum);
            
            for (Int32 y = 0; y < image.Height; y += Tile.SIZE)
            for (Int32 x = 0; x < image.Width; x += Tile.SIZE)
            {
                this.Add(image.GetTile(x, y));
            }
        }

        /// <summary>
        /// Initializes the Tileset
        /// </summary>
        void Load(Int32 maximum)
        {
            Sheet = (maximum == 0) ?
                new List<Tile>() :
                new List<Tile>(maximum);
        }



        /// <summary>
        /// Adds a new tile to this tileset.
        /// </summary>
        public void Add(Tile tile)
        {
            Sheet.Add(tile);
        }
        /// <summary>
        /// Adds a set of tiles from another tileset to this one, starting at 'index' and adding 'amount' tiles (all if amount==0)
        /// </summary>
        public void AddTileset(Tileset tileset, Int32 index = 0, Int32 amount = 0)
        {
            if (amount == 0)
                amount = tileset.Count - index;
            for (Int32 i = index; i < amount; i++)
            {
                this.Add(tileset[i]);
            }
        }
        /// <summary>
        /// Parses the given byte array, adding all and any tiles to this GBA.Tileset.
        /// </summary>
        public void Parse(Byte[] data)
        {
            if (data == null) throw new Exception("data given is null");
            Byte[] buffer = new Byte[Tile.LENGTH];
            for (Int32 parse = 0; parse < data.Length; parse += buffer.Length)
            {
                Array.Copy(data, parse, buffer, 0, buffer.Length);
                Sheet.Add(new Tile(buffer));
            }
        }
        /// <summary>
        /// Parses the given image, adding the tiles to their corresponding indices in the byte map
        /// </summary>
        public void Parse(Image image, Int32?[,] map)
        {
            Int32 width = map.GetLength(0);
            Int32 height = map.GetLength(1);

            if (image.Width != width * Tile.SIZE || image.Height != height * Tile.SIZE)
                throw new Exception("given image and map have dimensions that do not match.");

            Int32 length = 0;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (map[x, y] == null) continue;
                if (map[x, y] > length) length = (Int32)map[x, y];
            }
            Tile[] tileset = new Tile[++length];
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (map[x, y] == null) continue;
                else
                {
                    tileset[(Int32)map[x, y]] = image.GetTile(x * Tile.SIZE, y * Tile.SIZE);
                }
            }
            for (Int32 i = 0; i < tileset.Length; i++)
            {
                Sheet.Add(tileset[i] ?? Tile.Empty);
            }
        }
        
        /// <summary>
        /// Returns true if a matching tile was found in the tileset (doesn't check for H&V flips)
        /// </summary>
        public Boolean Contains(Tile tile)
        {
            for (Int32 i = 0; i < Count; i++)
            {
                if (Sheet[i] != null && Sheet[i].Equals(tile)) return true;
            }
            return false;
        }
        /// <summary>
        /// Returns the index of the given tile in this Tileset, return -1 the tile was not found.
        /// </summary>
        public Int32 Find(Tile tile)
        {
            for (Int32 i = 0; i < Count; i++)
            {
                if (Sheet[i] != null && Sheet[i].Equals(tile)) return i;
            }
            return -1;
        }
        /// <summary>
        /// Checks if the tileset contains any flipped version of the given tile, returns null if nothing is found
        /// </summary>
        public Tuple<Int32, Boolean, Boolean> FindMatch(Tile tile)
        {
            Int32 index = Find(tile);

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
                        else return Tuple.Create(index, true, true);
                    }
                    else return Tuple.Create(index, false, true);
                }
                else return Tuple.Create(index, true, false);
            }
            else return Tuple.Create(index, false, false);
        }
        /// <summary>
        /// Returns a subset of the tiles of this Tileset
        /// </summary>
        public Tileset GetTiles(Int32 offset, Int32 length)
        {
            Tileset result = new Tileset(length);
            for (Int32 i = 0; i < length; i++)
            {
                if (offset + i >= Sheet.Count)
                    break;
                result.Add(Sheet[offset + i]);
            }
            return result;
        }

        /// <summary>
        /// Returns a GBA.Image of this GBA.Tileset, with the given palette and dimensions
        /// </summary>
        public Image ToImage(Int32 widthTiles, Int32 heightTiles, Byte[] palette)
        {
            Int32 width = widthTiles * Tile.SIZE;
            Int32 height = heightTiles * Tile.SIZE;
            Int32 length = widthTiles * heightTiles;
            Byte[] result = new Byte[(width / 2) * height];
            Byte[] tile;
            Int32 ix, iy;
            Int32 index;
            Int32 t = 0;
            for (Int32 i = 0; (i < length) && (i < Sheet.Count); i++)
            {
                ix = (i % widthTiles) * Tile.SIZE;
                iy = (i / widthTiles) * Tile.SIZE;
                index = (ix / 2) + (iy * (width / 2));
                tile = (Sheet[i] == null) ? new Byte[Tile.LENGTH] : Sheet[i].Bytes;
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
            return new Image(width, height, palette, result);
        }
        /// <summary>
        /// Returns the tiles of this Tileset (empty 0th tile excluded) written sequentially in a byte array
        /// </summary>
        public Byte[] ToBytes(Boolean compressed)
        {
            Byte[] result = new Byte[Count * GBA.Tile.LENGTH];
            Int32 index = 0;
            for (Int32 i = 0; i < Count; i++)
            {
                Array.Copy(Sheet[i].Bytes, 0, result, index, Sheet[i].Bytes.Length);
                index += Sheet[i].Bytes.Length;
            }

            return compressed ? LZ77.Compress(result) : result;
        }



        override public String ToString()
        {
            String result = "GBA.Tileset: " + Count + " tiles.";
            foreach (Tile tile in Sheet)
            {
                result += "\n\n" + tile;
            }
            return result;
        }
        override public Boolean Equals(Object other)
        {
            if (!(other is Tileset)) return false;
            Tileset tileset = (Tileset)other;
            foreach (Tile tile in Sheet)
            {
                for (Int32 i = 0; i < Count; i++)
                {
                    if (!tileset[i].Equals(tile)) return false;
                }
            }
            return true;
        }
        override public Int32 GetHashCode()
        {
            return Sheet.GetHashCode();
        }
    }
}
