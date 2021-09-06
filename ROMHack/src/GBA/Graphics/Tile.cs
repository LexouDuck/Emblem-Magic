using Magic;
using System;

namespace GBA
{
    // Format of GBA graphics:
    // 8x8 tiles with 32 bytes per tile; each nibble of each byte is a single pixel
    // Nibbles are reversed for each byte; bytes are in order
    // So if the top left tile of an image has something like 10 32 54 76
    // and the tile to the right of it begins with 98 BA DC FE
    // then the first 16 pixels along the top row will count 0-F

    /// <summary>
    /// Represents a 32-byte 8x8 pixel tile with reversed byte nibbles, as is the GBA format.
    /// This holds no color information on its own - only color indices.
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// The usual width and height of a tile
        /// </summary>
        public const Int32 SIZE = 8;
        /// <summary>
        /// The amount of bytes for one 8x8 tile of pixel data in 4bpp
        /// </summary>
        public const Int32 LENGTH = 32;


        /// <summary>
        /// An empty tile (all pixels are 0)
        /// </summary>
        public static Tile Empty = new Tile(new Byte[LENGTH]);



        /// <summary>
        /// The 0-15 int of the color index in the palette for the pixel at the given coordinates.
        /// </summary>
        public Int32 this[Int32 x, Int32 y]
        {
            get
            {
                if (x < 0 || x >= SIZE) throw new ArgumentException("X given is out of bounds: " + x);
                if (y < 0 || y >= SIZE) throw new ArgumentException("Y given is out of bounds: " + y);

                Int32 index = (x / 2) + (y * (SIZE / 2));
                if (index < 0 || index >= Bytes.Length)
                    throw new ArgumentException("index is outside of the byte array." + Bytes.Length);
                else return (x % 2 == 0) ?
                     (Bytes[index] & 0x0F) :
                    ((Bytes[index] & 0xF0) >> 4);
            }
            set
            {
                if (x < 0 || x >= SIZE) throw new ArgumentException("X given is out of bounds: " + x);
                if (y < 0 || y >= SIZE) throw new ArgumentException("Y given is out of bounds: " + y);

                Int32 index = (x / 2) + (y * (SIZE / 2));
                if (index < 0 || index >= Bytes.Length)
                    throw new ArgumentException("index is outside of the byte array.");
                else Bytes[index] = (x % 2 == 0) ?
                    (Byte)((Bytes[index] & 0xF0) | (value & 0x0F)) :
                    (Byte)((Bytes[index] & 0x0F) |((value & 0x0F) << 4));
                return;
            }
        }

        /// <summary>
        /// The byte array that holds the pixel data for this tile: 1 digit per pixel, nibbles reversed.
        /// </summary>
        public Byte[] Bytes { get; private set; }
        


        /// <summary>
        /// Creates a size x size Tile from the given byte array (size must be a multiple of 8)
        /// </summary>
        public Tile(Byte[] data)
        {
            if (data.Length != LENGTH)
                throw new Exception("data given has invalid length, it should be 32 bytes long");
            
            Bytes = new Byte[LENGTH];
            Array.Copy(data, Bytes, LENGTH);
        }
        /// <summary>
        /// Creates a (palette-less) Tile from the given 8x8 GBA.Image
        /// </summary>
        public Tile(Image image)
        {
            if (image.Width != SIZE || image.Height != SIZE)
                throw new Exception("given GBA.Image is not 8x8 pixels");

            Bytes = new Byte[LENGTH];
            Array.Copy(image.Bytes, Bytes, LENGTH);
        }



        /// <summary>
        /// Tells whether or not this Tile is blank.
        /// </summary>
        public Boolean IsEmpty()
        {
            for (Int32 i = 0; i < Bytes.Length; i++)
            {
                if (Bytes[i] != 0) return false;
            }
            return true;
        }

        /// <summary>
        /// Returns this tile, flipped horizontally
        /// </summary>
        public Tile FlipVertical()
        {
            Byte[] result = new Byte[Bytes.Length];
            Int32 width = SIZE / 2;
            Int32 height = SIZE;
            for (Int32 row = 0; row < height; row++)
            {
                Array.Copy(Bytes, row * width, result, (height - 1 - row) * width, width);
            }
            return new Tile(result);
        }
        /// <summary>
        /// Returns this tile, flipped vertically
        /// </summary>
        public Tile FlipHorizontal()
        {
            Byte[] result = new Byte[Bytes.Length];
            Int32 width = SIZE / 2;
            Int32 height = SIZE;
            Byte[] buffer = new Byte[width];
            for (Int32 row = 0; row < height; row++)
            {
                Array.Copy(Bytes, row * width, buffer, 0, width);
                Array.Reverse(buffer);
                for (Int32 i = 0; i < width; i++)
                {
                    result[row * width + i] = (Byte)(((buffer[i] & 0x0F) << 4) | ((buffer[i] & 0xF0) >> 4));
                }
            }
            return new Tile(result);
        }
        /// <summary>
        /// Returns a Color array of the pixels of this Tile by looking up colors from the given Palette.
        /// </summary>
        public Color[,] GetPixels(Palette colors)
        {
            Color[,] result = new Color[SIZE, SIZE];
            Int32 index = 0;
            for (Int32 y = 0; y < SIZE; y++)
            {
                for (Int32 x = 0; x < SIZE; x += 2)
                {
                    result[x, y] = colors[(Bytes[index] & 0x0F)];
                    result[x + 1, y] = colors[(Bytes[index] & 0xF0) >> 4];
                    index++;
                }
            }
            return result;
        }



        override public String ToString()
        {
            String result = "";
            for (Int32 y = 0; y < SIZE; y++)
            {
                for (Int32 x = 0; x < SIZE; x++)
                {
                    result += Util.ByteToDigit((Byte)this[x, y]);
                    result += " ";
                }
                result += "\n";
            }
            return "GBA.Tile:\n" + result;
        }
        override public Boolean Equals(Object other)
        {
            if (!(other is Tile)) return false;
            Tile tile = (Tile)other;
            if (this == tile) return true;
            for (Int32 i = 0; i < Bytes.Length; i++)
            {
                if (Bytes[i] != tile.Bytes[i]) return false;
            }
            return true;
        }
        override public Int32 GetHashCode()
        {
            return Bytes.GetHashCode();
        }
    }
}
