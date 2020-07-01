using EmblemMagic;
using System;

namespace GBA
{
    public class TileMap
    {
        /// <summary>
        /// This returns the index of the tile at coordinates, index -1 means an empty tile
        /// </summary>
        public int this[int x, int y]
        {
            get
            {
                if (x < 0 || x > Width)  throw new ArgumentException("X given is out of bounds: " + x + ", should be < " + Width);
                if (y < 0 || y > Height) throw new ArgumentException("Y given is out of bounds: " + y + ", should be < " + Height);
                
                return (Tiles[x + y * Width] == null) ? -1 : (int)Tiles[x + y * Width];
            }
            set
            {
                if (x < 0 || x > Width)  throw new ArgumentException("X given is out of bounds: " + x + ", should be < " + Width);
                if (y < 0 || y > Height) throw new ArgumentException("Y given is out of bounds: " + y + ", should be < " + Height);

                Tiles[x + y * Width] = value;
            }
        }

        /// <summary>
        /// The amount of Tiles along the X-axis in this Tilemap
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// The amount of Tiles along the Y-axis in this Tilemap
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// This nullable int array holds the tile mapping data for this sprite
        /// </summary>
        int?[] Tiles { get; set; }



        /// <summary>
        /// Creates a GBA.Tilemap of the given width and height, in tiles.
        /// </summary>
        public TileMap(int width, int height)
        {
            Load(width, height);
        }
        /// <summary>
        /// Creates a GBA.Tilemap from the given mapping data
        /// </summary>
        public TileMap(int?[,] map)
        {
            Load(map.GetLength(0), map.GetLength(1));
            Map(map);
        }

        /// <summary>
        /// Initializes some basic fields of this instance.
        /// </summary>
        void Load(int width, int height)
        {
            Width = width;
            Height = height;
            Tiles = new int?[width * height];
        }


        
        /// <summary>
        /// Maps a region of tiles according to the given 2-dimensional Tile array and offsets
        /// </summary>
        public void Map(int?[,] mapping, int offsetX = 0, int offsetY = 0)
        {
            int width = mapping.GetLength(0);
            int height = mapping.GetLength(1);

            if (width > Width) throw new Exception("GBA.TileMap: given mapping has a larger width than the TileMap");
            if (height > Height) throw new Exception("GBA.TileMap: given mapping has a larger height than the TileMap");
            if (offsetX < 0 || offsetX + width > Width) throw new Exception("GBA.TileMap: given map goes out of bounds");
            if (offsetY < 0 || offsetY + height > Height) throw new Exception("GBA.TileMap: given map goes out of bounds");

            int index;
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                index = (offsetX + x) + (offsetY + y) * Width;
                Tiles[index] = mapping[x, y];
            }
        }



        /// <summary>
        /// Returns a simple tilemap with incrementing index, of the given dimensions
        /// </summary>
        public static TileMap GetBasicMap(int width, int height)
        {
            TileMap result = new TileMap(width, height);
            int index = 0;
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                result.Tiles[x + y * width] = index++;
            }
            return result;
        }
        /// <summary>
        /// Places the tiles of this this Tilemap onto a bigger empty map, and returns it
        /// </summary>
        public static int?[,] Place(int?[,] map, int offsetX, int offsetY, int newWidth, int newHeight)
        {
            int?[,] result = new int?[newWidth, newHeight];
            int width = map.GetLength(0);
            int height = map.GetLength(1);
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                if (offsetX + x < 0 || offsetX + x >= newWidth) continue;
                if (offsetY + y < 0 || offsetY + y >= newHeight) continue;
                result[offsetX + x, offsetY + y] = map[x, y];
            }
            return result;
        }
        /// <summary>
        /// Adds the given value to each byte of a byte tilemap (null will become 0x00), and swaps the X and Y axes
        /// </summary>
        public static int?[,] Convert(int?[,] map, int add = 0)
        {
            int width = map.GetLength(1);
            int height = map.GetLength(0);
            int?[,] result = new int?[width, height];
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                result[x, y] = map[y, x] + add;
            }
            return result;
        }



        override public string ToString()
        {
            string result = "GBA.TileMap: " + Width + "x" + Height;
            byte[] buffer = new byte[Width];
            for (int i = 0; i < Height; i++)
            {
                Array.Copy(Tiles, i * Width, buffer, 0, Width);
                result += "\n" + Util.BytesToSpacedHex(buffer);
            }
            return result;
        }
        override public bool Equals(object other)
        {
            if (!(other is TileMap)) return false;
            TileMap tilemap = (TileMap)other;
            if (Width == tilemap.Width && Height == tilemap.Height)
            {
                for (int i = 0; i < Tiles.Length; i++)
                {
                    if (Tiles[i] != tilemap.Tiles[i]) return false;
                }
                return true;
            }
            else return false;
        }
        override public int GetHashCode()
        {
            return Tiles.GetHashCode();
        }
    }
}