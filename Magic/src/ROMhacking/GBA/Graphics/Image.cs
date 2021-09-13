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
        public Int32 this[Int32 x, Int32 y]
        {
            get
            {
                if (x < 0 || x >= this.Width)  throw new ArgumentException("X given is out of bounds: " + x);
                if (y < 0 || y >= this.Height) throw new ArgumentException("Y given is out of bounds: " + y);

                Int32 index = (x / 2) + (y * (this.Width / 2));
                if (index < 0 || index >= this.Bytes.Length)
                    throw new ArgumentException("index is outside of the byte array.");
                return ((x % 2 == 0) ?
                    (this.Bytes[index] & 0x0F) :
                    (this.Bytes[index] & 0xF0) >> 4);
            }
        }
        public Color GetColor(Int32 x, Int32 y)
        {
            return (this.Colors[this[x, y]]);
        }

        /// <summary>
        /// The Width of this GBA.Image, in pixels.
        /// </summary>
        public Int32 Width { get; private set; }
        /// <summary>
        /// The Height of this GBA.Image, in pixels.
        /// </summary>
        public Int32 Height { get; private set; }

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
        public Image(String filepath, Palette palette = null)
        {
            try
            {
                using (System.Drawing.Bitmap file = new System.Drawing.Bitmap(filepath))
                {
                    this.Load(file.Width, file.Height, palette ?? new Palette(16));

                    Color color;
                    Int32 HI_nibble;
                    Int32 LO_nibble;
                    Int32 index = 0;
                    if (palette == null)
                    {
                        for (Int32 y = 0; y < this.Height; y++)
                        for (Int32 x = 0; x < this.Width; x += 2)
                        {
                            color = (GBA.Color)file.GetPixel(x, y);
                            LO_nibble = this.Colors.Find(color);
                            if (LO_nibble == -1)
                            {
                                LO_nibble = this.Colors.Count;
                                    this.Colors.Add(color);
                            }
                            color = (GBA.Color)file.GetPixel(x + 1, y);
                            HI_nibble = this.Colors.Find(color);
                            if (HI_nibble == -1)
                            {
                                HI_nibble = this.Colors.Count;
                                    this.Colors.Add(color);
                            }

                                this.Bytes[index++] = (Byte)((HI_nibble << 4) | LO_nibble);
                        }
                    }
                    else
                    {
                        for (Int32 y = 0; y < this.Height; y++)
                        for (Int32 x = 0; x < this.Width; x += 2)
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

                                this.Bytes[index++] = (Byte)((HI_nibble << 4) | LO_nibble);
                        }
                        this.Colors = palette;
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
        public Image(Int32 width, Int32 height, Byte[] palette, Byte[] data)
        {
            this.Load(width, height, new Palette(palette));

            Array.Copy(data, this.Bytes, Math.Min(data.Length, this.Bytes.Length));
        }
        /// <summary>
        /// Copy constructor - if palette is null, takes the palette from the source GBA.Image, otherwise recolors it.
        /// </summary>
        public Image(Image source, Palette palette = null)
        {
            this.Load(source.Width, source.Height, source.Colors);

            Array.Copy(source.Bytes, this.Bytes, this.Bytes.Length);

            if (palette != null) this.Recolor(palette);
        }
        /// <summary>
        /// Creates a GBA.Image from a GBA.Bitmap and the given GBA.Palette (set palette as null and it will generate a Palette)
        /// If colors are found in the Bitmap from outside the given Palette, an error will be reported.
        /// </summary>
        public Image(Bitmap image, Palette palette = null)
        {
            this.Load(image.Width, image.Height, new Palette(16));

            Int32 index = 0;
            Color color;
            Int32 HI_nibble;
            Int32 LO_nibble;

            if (palette == null)
            {
                for (Int32 y = 0; y < this.Height; y++)
                for (Int32 x = 0; x < this.Width; x += 2)
                {
                    color = image.GetColor(x, y);
                    LO_nibble = this.Colors.Find(color);
                    if (LO_nibble == -1)
                    {
                        LO_nibble = this.Colors.Count;
                            this.Colors.Add(color);
                    }
                    color = image.GetColor(x + 1, y);
                    HI_nibble = this.Colors.Find(color);
                    if (HI_nibble == -1)
                    {
                        HI_nibble = this.Colors.Count;
                            this.Colors.Add(color);
                    }

                        this.Bytes[index++] = (Byte)((HI_nibble << 4) | LO_nibble);
                }
            }
            else
            {
                for (Int32 y = 0; y < this.Height; y++)
                for (Int32 x = 0; x < this.Width; x += 2)
                {
                    color = image.GetColor(x + 1, y);
                    HI_nibble = palette.Find(color);
                    color = image.GetColor(x, y);
                    LO_nibble = palette.Find(color);

                    if (HI_nibble == -1 || LO_nibble == -1)
                        throw new Exception("A color in the image was not found in the palette.");
                    else this.Bytes[index] = (Byte)((HI_nibble << 4) | LO_nibble);
                    index++;
                }
                this.Colors = palette;
            }
        }
        /// <summary>
        /// Creates a new GBA.Image from a sub-region of an existing image.
        /// </summary>
        public Image(Bitmap image, Rectangle region, Palette palette = null)
        {
            this.Load(region.Width, region.Height, new Palette(16));

            Int32 index = 0;
            Color color;
            Int32 HI_nibble;
            Int32 LO_nibble;

            if (palette == null)
            {
                for (Int32 y = 0; y < region.Height; y++)
                for (Int32 x = 0; x < region.Width; x += 2)
                {
                    color = image.GetColor(region.X + x, region.Y + y);
                    LO_nibble = this.Colors.Find(color);
                    if (LO_nibble == -1)
                    {
                        LO_nibble = this.Colors.Count;
                            this.Colors.Add(color);
                    }
                    color = image.GetColor(region.X + x + 1, region.Y + y);
                    HI_nibble = this.Colors.Find(color);
                    if (HI_nibble == -1)
                    {
                        HI_nibble = this.Colors.Count;
                            this.Colors.Add(color);
                    }

                        this.Bytes[index++] = (Byte)((HI_nibble << 4) | LO_nibble);
                }
            }
            else
            {
                for (Int32 y = 0; y < region.Height; y++)
                for (Int32 x = 0; x < region.Width; x += 2)
                {
                    color = image.GetColor(region.X + x + 1, region.Y + y);
                    HI_nibble = palette.Find(color);
                    color = image.GetColor(region.X + x, region.Y + y);
                    LO_nibble = palette.Find(color);

                    if (HI_nibble == -1 || LO_nibble == -1)
                        throw new Exception("A color in the image was not found in the given palette.\n" +
                            "At coordinates: x = " + (region.X + x) + ", y = " + (region.Y + y));
                    else this.Bytes[index] = (Byte)((HI_nibble << 4) | LO_nibble);
                    index++;
                }
                this.Colors = palette;
            }
        }

        /// <summary>
        /// Initializes the basic fields of this instance.
        /// </summary>
        void Load(Int32 width, Int32 height, Palette palette)
        {
            if ((width % 8) != 0 || (height % 8) != 0)
                throw new Exception("Width and Height should be multiples of 8 pixels");
            if (palette.Maximum < GBA.Palette.MAX)
                throw new Exception("Palette should have at least 16 colors.");

            this.Width = width;
            this.Height = height;
            this.Bytes = new Byte[(this.Width * this.Height) / 2];
            this.Colors = palette;
        }



        /// <summary>
        /// Tells if this is an empty Bitmap or not.
        /// </summary>
        public Boolean IsEmpty()
        {
            for (Int32 i = 0; i < this.Bytes.Length; i++)
            {
                if (this.Bytes[i] != 0) { return false; }
            }
            return true;
        }
        public Boolean IsRegionEmpty(Rectangle region)
        {
            for (Int32 y = 0; y < region.Height; y++)
            for (Int32 x = 0; x < region.Width; x++)
            {
                if (this[x + region.X, y + region.Y] > 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns true if both palette are identical (that means both have the same indexing order)
        /// </summary>
        public Boolean SamePaletteAs(Image other)
        {
            return this.Colors.Equals(other.Colors);
        }

        /// <summary>
        /// Returns true if the images have the same pixels.
        /// </summary>
        public Boolean Matches(Image other)
        {
            if (this.Width != other.Width || this.Height != other.Height) return false;
            else
            {
                Color[,] image1 = this.GetPixels(new Rectangle(0, 0, this.Width, this.Height));
                Color[,] image2 = other.GetPixels(new Rectangle(0, 0, this.Width, this.Height));
                for (Int32 y = 0; y < this.Height; y++)
                {
                    for (Int32 x = 0; x < this.Width; x++)
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
        public Boolean Matches(Int32 at_x, Int32 at_y, Image other, Rectangle region)
        {
            if (at_x < 0 || at_y < 0 || (at_x + region.Width) > this.Width || (at_y + region.Height) > this.Height)
                throw new Exception("Invalid selection(s)");

            if (this.Width != other.Width || this.Height != other.Height) return false;
            else
            {
                Color[,] image1 = this.GetPixels(new Rectangle(at_x, at_y, region.Width, region.Height));
                Color[,] image2 = other.GetPixels(region);
                Int32 width = image1.GetLength(0);
                Int32 height = image1.GetLength(1);
                for (Int32 y = 0; y < height; y++)
                {
                    for (Int32 x = 0; x < width; x++)
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
            if (this.Colors.Count != palette.Count)
                throw new Exception("Palette has a different color count than the current palette");

            this.Colors = new Palette(palette);
        }



        /// <summary>
        /// Returns a tile from the image at the given pixel coordinates
        /// </summary>
        public Tile GetTile(Int32 x, Int32 y)
        {
            if (x < 0 || x + 8 > this.Width || y < 0 || y + 8 > this.Height)
                throw new Exception("The requested tile is (at least partially) out of bounds.");

            if (x % 2 == 0)
            {
                Byte[] result = new Byte[GBA.Tile.LENGTH];
                Int32 index;
                Int32 i = 0;
                for (Int32 tileY = 0; tileY < 8; tileY++)
                for (Int32 tileX = 0; tileX < 8; tileX += 2)
                {
                    index = (x + tileX) / 2 + (y + tileY) * (this.Width / 2);
                    result[i++] = this.Bytes[index];
                }
                return new Tile(result);
            }
            else // its more complicated, we need to change the bytes
            {
                Tile result = new Tile(new Byte[Tile.LENGTH]);
                Int32 index;
                Int32 pixel;
                for (Int32 tileY = 0; tileY < 8; tileY++)
                for (Int32 tileX = 0; tileX < 8; tileX++)
                {
                    index = ((x + tileX) / 2) + ((y + tileY) * (this.Width / 2));
                    pixel = ((x + tileX) % 2 == 0) ?
                        (this.Bytes[index] & 0x0F) :
                        (this.Bytes[index] & 0xF0) >> 4;
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
            if (region.X < 0 || region.X + region.Width > this.Width || region.Y < 0 || region.Y + region.Height > this.Height)
                throw new Exception("The requested region is larger than the bitmap.");

            Color[,] result = new Color[region.Width, region.Height];
            
            for (Int32 y = 0; y < region.Height; y++)
            for (Int32 x = 0; x < region.Width; x++)
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
            if (region.X < 0 || region.Y + region.Width > this.Width || region.Y < 0 || region.Y + region.Height > this.Height)
                throw new Exception("The requested region is larger than the bitmap.");

            for (Int32 y = region.Y; y < region.Height; y++)
            {
                for (Int32 x = region.X; x < region.Width; x++)
                {
                    if (x < 0 || x > this.Width)  throw new Exception("X given is out of bounds: " + x);
                    if (y < 0 || y > this.Height) throw new Exception("Y given is out of bounds: " + y);

                    Int32 index = (x / 2) + (y * (this.Width / 2));
                    if (index < 0 || index >= this.Bytes.Length) throw new Exception("index is outside of the byte array.");

                    Int32 pixel = this.Colors.Find(pixels[x, y]);
                    if (pixel == -1) throw new Exception("Color to set was not found in the palette.");

                    this.Bytes[index] = (x % 2 == 0) ?
                        (Byte)((this.Bytes[index] & 0xF0) | (pixel & 0x0F)) :
                        (Byte)((this.Bytes[index] & 0x0F) |((pixel & 0x0F) << 4));
                }
            }
        }



        override public String ToString()
        {
            String result = "GBA.Image:";
            Int32 length = this.Width / 2;
            Byte[] line = new Byte[length];
            for (Int32 i = 0; i < this.Height; i++)
            {
                Array.Copy(this.Bytes, i * length, line, 0, length);
                result += "\n" + Util.BytesToSpacedHex(line);
            }
            return result;
        }
        override public Boolean Equals(Object other)
        {
            Image image = (Image)other;

            if (this.Bytes.Length == image.Bytes.Length && image.Colors.Equals(image.Colors))
            {
                for (Int32 i = 0; i < this.Bytes.Length; i++)
                {
                    if (this.Bytes[i] != image.Bytes[i]) return false;
                }
                return true;
            }
            else return false;
        }
        override public Int32 GetHashCode()
        {
            return ((this.Width & 0xFFFF) << 16) | (this.Height & 0xFFFF);
        }
    }
}
