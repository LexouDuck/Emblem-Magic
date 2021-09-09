using Compression;
using System;
using System.Drawing;

namespace GBA
{
    /// <summary>
    /// This class, when instantiated, represents a TSA array for tiling information
    /// </summary>
    public class TSA_Array
    {
        public TSA this[Int32 x, Int32 y]
        {
            get
            {
                if (x < 0 || x >= Width)  throw new ArgumentException("X given is out of bounds: " + x);
                if (y < 0 || y >= Height) throw new ArgumentException("Y given is out of bounds: " + y);

                Int32 index = x + y * Width;

                return Tiles[index];
            }
            set
            {
                if (x < 0 || x >= Width)  throw new ArgumentException("X given is out of bounds: " + x);
                if (y < 0 || y >= Height) throw new ArgumentException("Y given is out of bounds: " + y);

                Int32 index = x + y * Width;

                Tiles[index] = value;
            }
        }

        /// <summary>
        /// The actual TSA Array that the TSA_Array class is a wrapper for
        /// </summary>
        public TSA[] Tiles { get; set; }

        /// <summary>
        /// The width of this TSA
        /// </summary>
        public Int32 Width { get; set; }
        /// <summary>
        /// The height of this TSA
        /// </summary>
        public Int32 Height { get; set; }



        public TSA_Array(Int32 width, Int32 height)
        {
            Load(width, height);
        }
        public TSA_Array(Int32 width, Int32 height, Byte[] data)
        {
            Load(width, height);

            for (Int32 i = 0; i < Tiles.Length; i++)
            {
                if (i * 2 + 1 > data.Length) return;

                Tiles[i] = new TSA((UInt16)(data[i * 2] | (data[i * 2 + 1] << 8)));
            }
        }
        public TSA_Array(TSA_Array source, Int32 offset = 0, Rectangle region = new Rectangle())
        {
            if (region == new Rectangle())
            {
                Load(source.Width, source.Height);
            }
            else
            {
                Load(region.Width, region.Height);
            }

            if (offset == 0)
            {
                for (Int32 y = 0; y < Height; y++)
                for (Int32 x = 0; x < Width; x++)
                {
                    Tiles[x + y * Width] = source[region.X + x, region.Y + y];
                }
            }
            else
            {
                TSA tsa;
                for (Int32 y = 0; y < Height; y++)
                for (Int32 x = 0; x < Width; x++)
                {
                    Tiles[x + y * Width] = source[region.X + x, region.Y + y];
                    tsa = this[x, y];
                    this[x, y] = new TSA(
                        (UInt16)(offset + tsa.TileIndex),
                        tsa.Palette,
                        tsa.FlipH,
                        tsa.FlipV);
                }
            }
        }
        void Load(Int32 width, Int32 height)
        {
            Width = width;
            Height = height;
            Tiles = new TSA[width * height];
        }



        public void SetTile(Int32 x, Int32 y, UInt16 tileIndex)
        {
            TSA tsa = this[x, y];
            this[x, y] = new TSA(
                tileIndex,
                tsa.Palette,
                tsa.FlipH,
                tsa.FlipV);
        }
        public void SetPalette(Int32 x, Int32 y, Byte palette)
        {
            TSA tsa = this[x, y];
            this[x, y] = new TSA(
                tsa.TileIndex,
                palette,
                tsa.FlipH,
                tsa.FlipV);
        }
        public void SetFlipH(Int32 x, Int32 y, Boolean flipH)
        {
            TSA tsa = this[x, y];
            this[x, y] = new TSA(
                tsa.TileIndex,
                tsa.Palette,
                flipH,
                tsa.FlipV);
        }
        public void SetFlipV(Int32 x, Int32 y, Boolean flipV)
        {
            TSA tsa = this[x, y];
            this[x, y] = new TSA(
                tsa.TileIndex,
                tsa.Palette,
                tsa.FlipH,
                flipV);
        }



        /// <summary>
        /// Returns this TSA array as a byte array, LZ77 compressed or not
        /// </summary>
        public Byte[] ToBytes(Boolean compressed, Boolean flipRows)
        {
            Byte[] result = new Byte[Tiles.Length * 2];
            for (Int32 i = 0; i < Tiles.Length; i++)
            {
                result[i * 2] = (Byte)(Tiles[i].Value & 0x00FF);
                result[i * 2 + 1] = (Byte)((Tiles[i].Value & 0xFF00) >> 8);
            }

            if (flipRows) result = FlipRows(Width, Height, result);

            return compressed ? LZ77.Compress(result) : result;
        }



        public static Byte[] FlipRows(Int32 width, Int32 height, Byte[] tsa_data)
        {
            Byte[] result = new Byte[2 + tsa_data.Length];
            result[0] = (Byte)(width - 1);
            result[1] = (Byte)(height - 1);
            Int32 row = width * 2;
            for (Int32 i = 0; i < height; i++)
            {
                Array.Copy(tsa_data, (height - 1 - i) * row, result, 2 + i * row, row);
            }
            return result;
        }
        /// <summary>
        /// Generates a simple TSA Array where every tile index increments and has palette #0, not flipped
        /// </summary>
        public static TSA_Array GetBasicTSA(Int32 width, Int32 height)
        {
            TSA_Array result = new TSA_Array(width, height);
            Int32 index = 0;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                result[x, y] = new TSA((UInt16)(index++ % TSA.MAX_TILES));
            }
            return result;
        }
    }
}
