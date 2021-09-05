using Magic.Components;
using Magic;
using System;
using System.Drawing;

namespace GBA
{
    /// <summary>
    /// The GBA.Image class stores image data in 4bpp tile format (pixels indexed to a GBA.Palette)
    /// As such, its Palette is limited to holding 16 colors.
    /// </summary>
    public class Image : IDisplayable
    {
        /// <summary>
        /// This indexer allows for quick access to pixel data in GBA.Color format.
        /// </summary>
        public int this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= Width)  throw new ArgumentException("X given is out of bounds: " + x);
                if (y < 0 || y >= Height) throw new ArgumentException("Y given is out of bounds: " + y);

                int index = (x / 2) + (y * (Width / 2));
                if (index < 0 || index >= Bytes.Length)
                    throw new ArgumentException("index is outside of the byte array.");
                return ((x % 2 == 0) ?
                    (Bytes[index] & 0x0F) :
                    (Bytes[index] & 0xF0) >> 4);
            }
        }
        public Color GetColor(int x, int y)
        {
            return (Colors[this[x, y]]);
        }

        /// <summary>
        /// The Width of this GBA.Image, in pixels.
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// The Height of this GBA.Image, in pixels.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// This array updates the bitmap when modified - holds indexed 16bit color pixel data, two pixels per byte
        /// </summary>
        public Byte[] Bytes { get; set; }

        /// <summary>
        /// The Palette associated with this GBA Image
        /// </summary>
        public Palette Colors { get; set; }

        
        
        /// <summary>
        /// Makes a GBA.Image from the image at the given filepath (and using the given Palette if not null)
        /// </summary>
        public Image(string filepath, Palette palette = null)
        {
            try
            {
                using (System.Drawing.Bitmap file = new System.Drawing.Bitmap(filepath))
                {
                    Load(file.Width, file.Height, palette ?? new Palette(16));

                    Color color;
                    int HI_nibble;
                    int LO_nibble;
                    int index = 0;
                    if (palette == null)
                    {
                        for (int y = 0; y < Height; y++)
                        for (int x = 0; x < Width; x += 2)
                        {
                            color = (GBA.Color)file.GetPixel(x, y);
                            LO_nibble = Colors.Find(color);
                            if (LO_nibble == -1)
                            {
                                LO_nibble = Colors.Count;
                                Colors.Add(color);
                            }
                            color = (GBA.Color)file.GetPixel(x + 1, y);
                            HI_nibble = Colors.Find(color);
                            if (HI_nibble == -1)
                            {
                                HI_nibble = Colors.Count;
                                Colors.Add(color);
                            }

                            Bytes[index++] = (byte)((HI_nibble << 4) | LO_nibble);
                        }
                    }
                    else
                    {
                        for (int y = 0; y < Height; y++)
                        for (int x = 0; x < Width; x += 2)
                        {
                            color = (GBA.Color)file.GetPixel(x, y);
                            LO_nibble = palette.Find(color);
                            if (LO_nibble == -1)
                                LO_nibble = Color.GetNearest(palette, color);
                            color = (GBA.Color)file.GetPixel(x + 1, y);
                            HI_nibble = palette.Find(color);
                            if (HI_nibble == -1)
                                HI_nibble = Color.GetNearest(palette, color);

                            if (HI_nibble == -1 || LO_nibble == -1)
                                throw new Exception("A color in the image was not found in the palette given.");

                            Bytes[index++] = (byte)((HI_nibble << 4) | LO_nibble);
                        }
                        Colors = palette;
                    }
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("The image file could not be read:\n" + filepath, ex);
            }
        }
        /// <summary>
        /// Creates a GBA.Image from byte array  data
        /// </summary>
        public Image(int width, int height, byte[] palette, byte[] data)
        {
            Load(width, height, new Palette(palette));

            Array.Copy(data, Bytes, Math.Min(data.Length, Bytes.Length));
        }
        /// <summary>
        /// Copy constructor - if palette is null, takes the palette from the source GBA.Image, otherwise recolors it.
        /// </summary>
        public Image(Image source, Palette palette = null)
        {
            Load(source.Width, source.Height, source.Colors);

            Array.Copy(source.Bytes, Bytes, Bytes.Length);

            if (palette != null) this.Recolor(palette);
        }
        /// <summary>
        /// Creates a GBA.Image from a GBA.Bitmap and the given GBA.Palette (set palette as null and it will generate a Palette)
        /// If colors are found in the Bitmap from outside the given Palette, an error will be reported.
        /// </summary>
        public Image(Bitmap image, Palette palette = null)
        {
            Load(image.Width, image.Height, new Palette(16));

            int index = 0;
            Color color;
            int HI_nibble;
            int LO_nibble;

            if (palette == null)
            {
                for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x += 2)
                {
                    color = image.GetColor(x, y);
                    LO_nibble = Colors.Find(color);
                    if (LO_nibble == -1)
                    {
                        LO_nibble = Colors.Count;
                        Colors.Add(color);
                    }
                    color = image.GetColor(x + 1, y);
                    HI_nibble = Colors.Find(color);
                    if (HI_nibble == -1)
                    {
                        HI_nibble = Colors.Count;
                        Colors.Add(color);
                    }

                    Bytes[index++] = (byte)((HI_nibble << 4) | LO_nibble);
                }
            }
            else
            {
                for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x += 2)
                {
                    color = image.GetColor(x + 1, y);
                    HI_nibble = palette.Find(color);
                    color = image.GetColor(x, y);
                    LO_nibble = palette.Find(color);

                    if (HI_nibble == -1 || LO_nibble == -1)
                        throw new Exception("A color in the image was not found in the palette.");
                    else Bytes[index] = (byte)((HI_nibble << 4) | LO_nibble);
                    index++;
                }
                Colors = palette;
            }
        }
        /// <summary>
        /// Creates a new GBA.Image from a sub-region of an existing image.
        /// </summary>
        public Image(Bitmap image, Rectangle region, Palette palette = null)
        {
            Load(region.Width, region.Height, new Palette(16));

            int index = 0;
            Color color;
            int HI_nibble;
            int LO_nibble;

            if (palette == null)
            {
                for (int y = 0; y < region.Height; y++)
                for (int x = 0; x < region.Width; x += 2)
                {
                    color = image.GetColor(region.X + x, region.Y + y);
                    LO_nibble = Colors.Find(color);
                    if (LO_nibble == -1)
                    {
                        LO_nibble = Colors.Count;
                        Colors.Add(color);
                    }
                    color = image.GetColor(region.X + x + 1, region.Y + y);
                    HI_nibble = Colors.Find(color);
                    if (HI_nibble == -1)
                    {
                        HI_nibble = Colors.Count;
                        Colors.Add(color);
                    }

                    Bytes[index++] = (byte)((HI_nibble << 4) | LO_nibble);
                }
            }
            else
            {
                for (int y = 0; y < region.Height; y++)
                for (int x = 0; x < region.Width; x += 2)
                {
                    color = image.GetColor(region.X + x + 1, region.Y + y);
                    HI_nibble = palette.Find(color);
                    color = image.GetColor(region.X + x, region.Y + y);
                    LO_nibble = palette.Find(color);

                    if (HI_nibble == -1 || LO_nibble == -1)
                        throw new Exception("A color in the image was not found in the given palette.\n" +
                            "At coordinates: x = " + (region.X + x) + ", y = " + (region.Y + y));
                    else Bytes[index] = (byte)((HI_nibble << 4) | LO_nibble);
                    index++;
                }
                Colors = palette;
            }
        }

        /// <summary>
        /// Initializes the basic fields of this instance.
        /// </summary>
        void Load(int width, int height, Palette palette)
        {
            if ((width % 8) != 0 || (height % 8) != 0)
                throw new Exception("Width and Height should be multiples of 8 pixels");
            if (palette.Maximum < GBA.Palette.MAX)
                throw new Exception("Palette should have at least 16 colors.");

            Width = width;
            Height = height;
            Bytes = new byte[(Width * Height) / 2];
            Colors = palette;
        }



        /// <summary>
        /// Tells if this is an empty Bitmap or not.
        /// </summary>
        public bool IsEmpty()
        {
            for (int i = 0; i < Bytes.Length; i++)
            {
                if (Bytes[i] != 0) { return false; }
            }
            return true;
        }
        public bool IsRegionEmpty(Rectangle region)
        {
            for (int y = 0; y < region.Height; y++)
            for (int x = 0; x < region.Width; x++)
            {
                if (this[x + region.X, y + region.Y] > 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns true if both palette are identical (that means both have the same indexing order)
        /// </summary>
        public bool SamePaletteAs(Image other)
        {
            return Colors.Equals(other.Colors);
        }

        /// <summary>
        /// Returns true if the images have the same pixels.
        /// </summary>
        public bool Matches(Image other)
        {
            if (Width != other.Width || Height != other.Height) return false;
            else
            {
                Color[,] image1 = GetPixels(new Rectangle(0, 0, Width, Height));
                Color[,] image2 = other.GetPixels(new Rectangle(0, 0, Width, Height));
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        if (image1[x, y] == image2[x, y]) return false;
                    }
                }
                return true;
            }
        }
        /// <summary>
        /// Returns true if the subregion of the other image matches the region at (x,y) of this image.
        /// </summary>
        public bool Matches(int at_x, int at_y, Image other, Rectangle region)
        {
            if (at_x < 0 || at_y < 0 || (at_x + region.Width) > Width || (at_y + region.Height) > Height)
                throw new Exception("Invalid selection(s)");

            if (Width != other.Width || Height != other.Height) return false;
            else
            {
                Color[,] image1 = GetPixels(new Rectangle(at_x, at_y, region.Width, region.Height));
                Color[,] image2 = other.GetPixels(region);
                int width = image1.GetLength(0);
                int height = image1.GetLength(1);
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (image1[x, y] == image2[x, y]) return false;
                    }
                }
                return true;
            }
        }



        /// <summary>
        /// Recolors this Image to the Palette given
        /// </summary>
        public void Recolor(Palette palette)
        {
            if (Colors.Count != palette.Count)
                throw new Exception("Palette has a different color count than the current palette");

            Colors = new Palette(palette);
        }



        /// <summary>
        /// Returns a tile from the image at the given pixel coordinates
        /// </summary>
        public Tile GetTile(int x, int y)
        {
            if (x < 0 || x + 8 > Width || y < 0 || y + 8 > Height)
                throw new Exception("The requested tile is (at least partially) out of bounds.");

            if (x % 2 == 0)
            {
                byte[] result = new byte[GBA.Tile.LENGTH];
                int index;
                int i = 0;
                for (int tileY = 0; tileY < 8; tileY++)
                for (int tileX = 0; tileX < 8; tileX += 2)
                {
                    index = (x + tileX) / 2 + (y + tileY) * (Width / 2);
                    result[i++] = Bytes[index];
                }
                return new Tile(result);
            }
            else // its more complicated, we need to change the bytes
            {
                Tile result = new Tile(new byte[Tile.LENGTH]);
                int index;
                int pixel;
                for (int tileY = 0; tileY < 8; tileY++)
                for (int tileX = 0; tileX < 8; tileX++)
                {
                    index = ((x + tileX) / 2) + ((y + tileY) * (Width / 2));
                    pixel = ((x + tileX) % 2 == 0) ?
                        (Bytes[index] & 0x0F) :
                        (Bytes[index] & 0xF0) >> 4;
                    result[tileX, tileY] = pixel;
                }
                return result;
            }
        }
        /// <summary>
        /// Returns an array of pixels from the requested region of this GBA.Image
        /// </summary>
        public Color[,] GetPixels(Rectangle region)
        {
            if (region.X < 0 || region.X + region.Width > Width || region.Y < 0 || region.Y + region.Height > Height)
                throw new Exception("The requested region is larger than the bitmap.");

            Color[,] result = new Color[region.Width, region.Height];
            
            for (int y = 0; y < region.Height; y++)
            for (int x = 0; x < region.Width; x++)
            {
                result[x, y] = this.GetColor(region.X + x, region.Y + y);
            }
            return result;
        }
        /// <summary>
        /// Sets the pixels in the given region of this GBA.Image
        /// </summary>
        public void SetPixels(Color[,] pixels, Rectangle region)
        {
            if (pixels.Length != region.Width * region.Height)
                throw new Exception("The array length doesn't match the region size.");
            if (region.X < 0 || region.Y + region.Width > Width || region.Y < 0 || region.Y + region.Height > Height)
                throw new Exception("The requested region is larger than the bitmap.");

            for (int y = region.Y; y < region.Height; y++)
            {
                for (int x = region.X; x < region.Width; x++)
                {
                    if (x < 0 || x > Width)  throw new Exception("X given is out of bounds: " + x);
                    if (y < 0 || y > Height) throw new Exception("Y given is out of bounds: " + y);

                    int index = (x / 2) + (y * (Width / 2));
                    if (index < 0 || index >= Bytes.Length) throw new Exception("index is outside of the byte array.");

                    int pixel = Colors.Find(pixels[x, y]);
                    if (pixel == -1) throw new Exception("Color to set was not found in the palette.");

                    Bytes[index] = (x % 2 == 0) ?
                        (byte)((Bytes[index] & 0xF0) | (pixel & 0x0F)) :
                        (byte)((Bytes[index] & 0x0F) |((pixel & 0x0F) << 4));
                }
            }
        }



        override public string ToString()
        {
            String result = "GBA.Image:";
            int length = Width / 2;
            byte[] line = new byte[length];
            for (int i = 0; i < Height; i++)
            {
                Array.Copy(Bytes, i * length, line, 0, length);
                result += "\n" + Util.BytesToSpacedHex(line);
            }
            return result;
        }
        override public bool Equals(object other)
        {
            Image image = (Image)other;

            if (Bytes.Length == image.Bytes.Length && image.Colors.Equals(image.Colors))
            {
                for (int i = 0; i < Bytes.Length; i++)
                {
                    if (Bytes[i] != image.Bytes[i]) return false;
                }
                return true;
            }
            else return false;
        }
        override public int GetHashCode()
        {
            return ((Width & 0xFFFF) << 16) | (Height & 0xFFFF);
        }
    }
}
