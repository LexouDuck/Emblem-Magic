using Magic.Components;
using Magic;
using System;
using System.Drawing;

namespace GBA
{
    /// <summary>
    /// The GBA.Bitmap stores image data in 1bpp indexed format, with 16-bit colors in its palette
    /// </summary>
    public class Bitmap : IDisplayable
    {
        /// <summary>
        /// This indexer allows for quick access to pixel data in GBA.Color format.
        /// </summary>
        public Int32 this[Int32 x, Int32 y]
        {
            get
            {
                if (x < 0 || x >= Width) throw new ArgumentException("x given is out of bounds: " + x);
                if (y < 0 || y >= Height) throw new ArgumentException("y given is out of bounds: " + y);

                Int32 index = x + y * Width;
                return Bytes[index];
            }
            set
            {
                if (x < 0 || x >= Width)  throw new ArgumentException("x given is out of bounds: " + x);
                if (y < 0 || y >= Height) throw new ArgumentException("y given is out of bounds: " + y);

                Int32 index = x + y * Width;
                if (value == -1) throw new ArgumentException("Color wasn't found in palette: " + value);
                Bytes[index] = (Byte)value;
                return;
            }
        }
        public Color GetColor(Int32 x, Int32 y)
        {
            return Colors[this[x, y]];
        }
        public void SetColor(Int32 x, Int32 y, Color value)
        {
            this[x, y] = Colors.Find(value);
        }

        /// <summary>
        /// The Width of this GBA.Bitmap, in pixels.
        /// </summary>
        public Int32 Width { get; private set; }
        /// <summary>
        /// The Height of this GBA.Bitmap, in pixels.
        /// </summary>
        public Int32 Height { get; private set; }

        /// <summary>
        /// The palette of this GBA.Bitmap - can have more than 16 colors
        /// </summary>
        public Palette Colors { get; set; }

        /// <summary>
        /// This array holds 16bit 2BPP pixel data - it /is/ the bitmap, per se
        /// </summary>
        protected Byte[] Bytes { get; set; }



        /// <summary>
        /// Creates an empty GBA.Bitmap of the given dimensions.
        /// </summary>
        public Bitmap(Int32 width, Int32 height)
        {
            Load(width, height);
        }
        /// <summary>
        /// Creates a GBA.Bitmap from the given IDisplayable object
        /// </summary>
        public Bitmap(IDisplayable pixels)
        {
            Load(pixels.Width, pixels.Height);

            Int32 index = 0;
            Int32 pixel;
            Color color;
            for (Int32 y = 0; y < Height; y++)
            for (Int32 x = 0; x < Width; x++)
            {
                color = pixels.GetColor(x, y);
                pixel = Colors.Find(color);
                if (pixel == -1)
                {
                    pixel = Colors.Count;
                    Colors.Add(color);
                }
                Bytes[index++] = (Byte)pixel;
            }
        }
        /// <summary>
        /// Makes a GBA.Bitmap from the image at the given filepath (and using the given Palette if not null)
        /// </summary>
        public Bitmap(String filepath, Palette palette = null)
        {
            using (System.Drawing.Bitmap file = new System.Drawing.Bitmap(filepath))
            {
                Load(file.Width, file.Height);

                if (palette != null)
                {
                    Colors = palette;
                }
                Int32 index = 0;
                Int32 pixel;
                Color color;
                for (Int32 y = 0; y < Height; y++)
                for (Int32 x = 0; x < Width; x++)
                {
                    color = (GBA.Color)file.GetPixel(x, y);
                    pixel = Colors.Find(color);
                    if (pixel == -1)
                    {
                        if (palette == null)
                        {
                            pixel = Colors.Count;
                            Colors.Add(color);
                        }
                        else throw new Exception("A color in the bitmap was not found in the palette given: at x=" + x + ", y=" + y);
                    }
                    Bytes[index++] = (Byte)pixel;
                }
            }
        }
        /// <summary>
        /// Makes a bitmap from the given byte array (must in 1 byte per pixel format)
        /// </summary>
        public Bitmap(Int32 width, Int32 height, Byte[] palette, Byte[] data)
        {
            Load(width, height);

            Colors = new Palette(palette, palette.Length / 2);

            Array.Copy(data, Bytes, Math.Min(data.Length, Bytes.Length));
        }
        /// <summary>
        /// Copies an existing GBA.Bitmap or makes one from a subregion of an existing bitmap
        /// </summary>
        public Bitmap(Bitmap source, Rectangle region = new Rectangle())
        {
            if (region == new Rectangle())
            {
                Load(source.Width, source.Height);
                Colors = source.Colors;
                Array.Copy(source.Bytes, Bytes, Bytes.Length);
            }
            else
            {
                if (region.X < 0 || region.X + region.Width > source.Width
                 || region.Y < 0 || region.Y + region.Height > source.Height)
                    throw new Exception("Rectangle region goes outside the GBA.Bitmap.");

                Load(region.Width, region.Height);
                Colors = source.Colors;
                Int32 index = 0;
                for (Int32 y = 0; y < Height; y++)
                for (Int32 x = 0; x < Width; x++)
                {
                    Bytes[index++] = source.Bytes[(region.X + x) + (region.Y + y) * source.Width];
                }
            }
        }

        /// <summary>
        /// Initializes the basic fields of this instance.
        /// </summary>
        void Load(Int32 width, Int32 height)
        {
            Width = width;
            Height = height;
            Bytes = new Byte[Width * Height];
            Colors = new Palette(256);
        }



        /// <summary>
        /// Returns the 1BPP byte array of pixels for this GBA.Bitmap
        /// </summary>
        public Byte[] ToBytes()
        {
            return Bytes;
        }

        /// <summary>
        /// Returns an array of pixels from the requested region of the Bitmap
        /// </summary>
        public Color[,] GetPixels(Rectangle region)
        {
            if (region.X < 0 || region.Y + region.Width > Width || region.Y < 0 || region.Y + region.Height > Height)
                throw new Exception("The requested region is larger than the bitmap.");

            Color[,] result = new Color[region.Width, region.Height];
            for (Int32 y = 0; y < region.Height; y++)
            for (Int32 x = 0; x < region.Width; x++)
            {
                result[x + region.X, y + region.Y] = this.GetColor(x, y);
            }
            return result;
        }
        /// <summary>
        /// Sets the pixels in the given region of the GBA.Bitmap
        /// </summary>
        public void SetPixels(Func<Int32, Int32, Byte> displayfunc, Rectangle region)
        {
            if (region.X < 0 || region.Y + region.Width > Width || region.Y < 0 || region.Y + region.Height > Height)
                throw new Exception("The requested region is larger than the bitmap.");
            
            for (Int32 y = 0; y < region.Height; y++)
            for (Int32 x = 0; x < region.Width; x++)
            {
                this[x + region.X, y + region.Y] = displayfunc(x, y);
            }
        }



        /// <summary>
        /// Tells if this is an empty Bitmap or not.
        /// </summary>
        public Boolean IsEmpty()
        {
            for (Int32 i = 0; i < Bytes.Length; i++)
            {
                if (Bytes[i] != 0) { return false; }
            }
            return true;
        }
    }
}
