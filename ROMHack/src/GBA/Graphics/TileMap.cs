using Magic;
using System;

namespace GBA
{
    public class TileMap
    {
        /// <summary>
        /// This returns the index of the tile at coordinates, index -1 means an empty tile
        /// </summary>
        public Int32 this[Int32 x, Int32 y]
        {
            get
            {
                if (x < 0 || x > Width)  throw new ArgumentException("X given is out of bounds: " + x + ", should be < " + Width);
                if (y < 0 || y > Height) throw new ArgumentException("Y given is out of bounds: " + y + ", should be < " + Height);
                
                return (Tiles[x + y * Width] == null) ? -1 : (Int32)Tiles[x + y * Width];
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
        public Int32 Width { get; private set; }
        /// <summary>
        /// The amount of Tiles along the Y-axis in this Tilemap
        /// </summary>
        public Int32 Height { get; private set; }

        /// <summary>
        /// This nullable int array holds the tile mapping data for this sprite
        /// </summary>
        Int32?[] Tiles { get; set; }



        /// <summary>
        /// Creates a GBA.Tilemap of the given width and height, in tiles.
        /// </summary>
        public TileMap(Int32 width, Int32 height)
        {
            Load(width, height);
        }
        /// <summary>
        /// Creates a GBA.Tilemap from the given mapping data
        /// </summary>
        public TileMap(Int32?[,] map)
        {
            Load(map.GetLength(0), map.GetLength(1));
            Map(map);
        }

        /// <summary>
        /// Initializes some basic fields of this instance.
        /// </summary>
        void Load(Int32 width, Int32 height)
        {
            Width = width;
            Height = height;
            Tiles = new Int32?[width * height];
        }


        
        /// <summary>
        /// Maps a region of tiles according to the given 2-dimensional Tile array and offsets
        /// </summary>
        public void Map(Int32?[,] mapping, Int32 offsetX = 0, Int32 offsetY = 0)
        {
            Int32 width = mapping.GetLength(0);
            Int32 height = mapping.GetLength(1);

            if (width > Width) throw new Exception("GBA.TileMap: given mapping has a larger width than the TileMap");
            if (height > Height) throw new Exception("GBA.TileMap: given mapping has a larger height than the TileMap");
            if (offsetX < 0 || offsetX + width > Width) throw new Exception("GBA.TileMap: given map goes out of bounds");
            if (offsetY < 0 || offsetY + height > Height) throw new Exception("GBA.TileMap: given map goes out of bounds");

            Int32 index;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                index = (offsetX + x) + (offsetY + y) * Width;
                Tiles[index] = mapping[x, y];
            }
        }



        /// <summary>
        /// Returns a simple tilemap with incrementing index, of the given dimensions
        /// </summary>
        public static TileMap GetBasicMap(Int32 width, Int32 height)
        {
            TileMap result = new TileMap(width, height);
            Int32 index = 0;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                result.Tiles[x + y * width] = index++;
            }
            return result;
        }
        /// <summary>
        /// Places the tiles of this this Tilemap onto a bigger empty map, and returns it
        /// </summary>
        public static Int32?[,] Place(Int32?[,] map, Int32 offsetX, Int32 offsetY, Int32 newWidth, Int32 newHeight)
        {
            Int32?[,] result = new Int32?[newWidth, newHeight];
            Int32 width = map.GetLength(0);
            Int32 height = map.GetLength(1);
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
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
        public static Int32?[,] Convert(Int32?[,] map, Int32 add = 0)
        {
            Int32 width = map.GetLength(1);
            Int32 height = map.GetLength(0);
            Int32?[,] result = new Int32?[width, height];
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                result[x, y] = map[y, x] + add;
            }
            return result;
        }



        override public String ToString()
        {
            String result = "GBA.TileMap: " + Width + "x" + Height;
            Byte[] buffer = new Byte[Width];
            for (Int32 i = 0; i < Height; i++)
            {
                Array.Copy(Tiles, i * Width, buffer, 0, Width);
                result += "\n" + Util.BytesToSpacedHex(buffer);
            }
            return result;
        }
        override public Boolean Equals(Object other)
        {
            if (!(other is TileMap)) return false;
            TileMap tilemap = (TileMap)other;
            if (Width == tilemap.Width && Height == tilemap.Height)
            {
                for (Int32 i = 0; i < Tiles.Length; i++)
                {
                    if (Tiles[i] != tilemap.Tiles[i]) return false;
                }
                return true;
            }
            else return false;
        }
        override public Int32 GetHashCode()
        {
            return Tiles.GetHashCode();
        }
    }
}