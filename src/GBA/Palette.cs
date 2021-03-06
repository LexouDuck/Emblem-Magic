﻿using Compression;
using EmblemMagic;
using System;
using System.Collections.Generic;
using System.IO;

namespace GBA
{
    /// <summary>
    /// Respresents 16-color GBA palettes, with colors in A1R5B5G5 - color 0 is always the alpha color.
    /// </summary>
    public class Palette
    {
        /// <summary>
        /// The length of a 16-color GBA.Palette's byte array.
        /// </summary>
        public const int LENGTH = 32;
        /// <summary>
        /// The typical amount of colors in a GBA.Palette
        /// </summary>
        public const int MAX = 16;


        /// <summary>
        /// Defines the indexer, to allow acessing Colors from the instance itself - like palette[0]
        /// </summary>
        public Color this[int index]
        {
            get
            {
                if (index >= Maximum) throw new ArgumentException("index cannot be " + Maximum + " or more");
                else if (index >= Count) return new Color();//throw new Exception("index out of range, should be under " + Count);
                return Colors[index];
            }
            set
            {
                if (index >= Maximum) throw new ArgumentException("index cannot be " + Maximum + " or more");
                else if (index >= Count) throw new ArgumentException("index out of range, should be under " + Count);
                Colors[index] = value;
            }
        }

        /// <summary>
        /// 2-byte A1R5B5G5 values for each of the 16 colours
        /// </summary>
        List<Color> Colors { get; set; }

        /// <summary>
        /// The maximum amount of colors for this GBA.Palette
        /// </summary>
        public int Maximum
        {
            get
            {
                return Colors.Capacity;
            }
            set
            {
                Colors.Capacity = value;
            }
        }

        /// <summary>
        /// Gets the amount of colors currently contained in this Palette.
        /// </summary>
        public int Count
        {
            get
            {
                return Colors.Count;
            }
        }

        /// <summary>
        /// Gets a bool telling whether or not this Palette has as many colors as Maximum allows.
        /// </summary>
        public bool IsFull
        {
            get
            {
                return (Count == Maximum);
            }
        }



        /// <summary>
        /// Creates an empty GBA.Palette.
        /// </summary>
        public Palette(int maximum = MAX)
        {
            Colors = new List<Color>(maximum);
        }
        /// <summary>
        /// Creates a Palette by reading a sequence of 2-byte colors from a byte array.
        /// </summary>
        public Palette(byte[] data, int maximum = MAX)
        {
            if (data == null || data.Length % 2 != 0 || data.Length > maximum * 2)
                throw new Exception("given byte array has invalid length.");

            Colors = new List<Color>(maximum);
            byte[] buffer = new byte[2];
            for (int i = 0; i < data.Length / 2; i++)
            {
                buffer[0] = data[i * 2 + 1];
                buffer[1] = data[i * 2];
                Colors.Add(new Color(buffer));
            }
        }
        /// <summary>
        /// Creates a Palette from a palette file (if its an image file, must be 16 pixels in width)
        /// </summary>
        public Palette(string filepath, int maximum = MAX)
        {
            Colors = new List<Color>(maximum);

            if (filepath.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                filepath.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ||
                filepath.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
            {
                Bitmap source;
                try
                {
                    source = new GBA.Bitmap(filepath);
                }
                catch { throw new Exception("Could not open palette file:\n" + filepath); }

                bool addEveryPixel = (source.Width == 16);
                int index = 0;
                Color color;
                for (int y = 0; y < source.Height; y++)
                for (int x = 0; x < source.Width; x++)
                {
                    color = source.GetColor(x, y);
                    if (!addEveryPixel && this.Contains(color)) continue;
                    else
                    {
                        if (this.IsFull) throw new Exception("This palette cannot hold more than " + maximum + " colors.");
                        else
                        {
                            this.Colors.Add(color);
                            index++;
                        }
                    }
                }
            }
            else if (filepath.EndsWith(".pal", StringComparison.OrdinalIgnoreCase))
            {
                byte[] palette = File.ReadAllBytes(filepath);

                if (palette[0] == 0x43 &&
                    palette[1] == 0x4C &&
                    palette[2] == 0x52 &&
                    palette[3] == 0x58) // 'CLRX' is the header for usenti .pal files
                {
                    string[] file = File.ReadAllLines(filepath);
                    int channelBits;
                    int colorAmount;

                    int parse = 5;
                    int length = 0;

                    while (parse + length < file[0].Length && file[0][parse + length] != ' ')
                        length++;
                    channelBits = int.Parse(file[0].Substring(parse, length));

                    parse += length + 1;
                    length = 0;

                    while (parse + length < file[0].Length && file[0][parse + length] != ' ')
                        length++;
                    colorAmount = int.Parse(file[0].Substring(parse, length));

                    uint color = 0x00000000;
                    Palette colors = new Palette(colorAmount);
                    for (int i = 0; i < (colorAmount / 4); i++)
                    {
                        parse = 0;
                        length = 0;

                        for (int j = 0; j < 4; j++)
                        {
                            while (parse + length < file[1 + i].Length &&
                                file[1 + i][parse + length] != ' ')
                                length++;

                            color = Util.HexToInt(file[1 + i].Substring(parse, length));
                            colors.Add(new Color(color));

                            parse += length + 1;
                            length = 0;
                        }
                    }
                    palette = colors.ToBytes(false);
                }
                else if (palette.Length != maximum * 2) throw new Exception(
                        "Cannot load palette, the file given is of invalid length.\n" +
                        "It should be " + maximum * 2 + " bytes long.");
                

                byte[] buffer = new byte[2];
                for (int i = 0; i < palette.Length / 2; i++)
                {
                    buffer[0] = palette[i * 2 + 1];
                    buffer[1] = palette[i * 2];
                    Colors.Add(new Color(buffer));
                }
            }
            else throw new Exception("Invalid filetype given to load palette.");
        }
        /// <summary>
        /// Creates a Palette from the given array of GBA colors.
        /// </summary>
        public Palette(Color[] colors)
        {
            if (colors == null || colors.Length >= MAX)
                throw new Exception("The given Color array has more than " + Maximum + " colors");

            Colors = new List<Color>(colors);
        }
        /// <summary>
        /// Copy constructor
        /// </summary>
        public Palette(Palette source)
        {
            Colors = new List<Color>(source.Colors);
        }



        /// <summary>
        /// Adds a new color to this Palette
        /// </summary>
        public void Add(Color color)
        {
            if (this.IsFull)
                throw new Exception("This palette cannot hold more than " + Maximum + " colors");

            Colors.Add(color);
        }
        /// <summary>
        /// Adds all the colors of the given palette at the end of this one, and returns the new length of this Palette
        /// </summary>
        public int Add(Palette palette)
        {
            for (int i = 0; i < palette.Count; i++)
            {
                if (this.IsFull) throw new Exception("This palette cannot hold more than " + Maximum + " colors");
                Colors.Add(palette[i]);
            }
            return Count;
        }

        /// <summary>
        /// Checks if the given color is already present in this palette and returns its index (-1 if not found)
        /// </summary>
        public int Find(Color color)
        {
            for (int i = 0; i < Count; i++)
            {
                if (Colors[i] == color) { return i; }
            }
            return -1;
        }
        /// <summary>
        /// Returns true if the given color is present in this palette
        /// </summary>
        public bool Contains(Color color)
        {
            return Colors.Contains(color);
        }
        
        /// <summary>
        /// Replaces a color.
        /// </summary>
        public void Set(int index, Color color)
        {
            if (index < 0 || index >= Count)
                throw new Exception("index out of bounds");
            Colors[index] = color;
        }
        /// <summary>
        /// Swaps the location of 2 colors in the GBA.Palette
        /// </summary>
        public void Swap(int fromIndex, int toIndex)
        {
            Color color = Colors[toIndex];
            Colors[toIndex] = Colors[fromIndex];
            Colors[fromIndex] = color;
        }
        /// <summary>
        /// Sorts the colors of this Palette according to the given comparison
        /// </summary>
        public void Sort(Comparison<Color> comparison)
        {
            Colors.Sort(comparison);
        }



        /// <summary>
        /// Returns the amount of colors in common between the two palettes
        /// </summary>
        public int Matches(Palette other)
        {
            int result = 0;
            for (int i = 0; i < other.Count; i++)
            {
                if (Colors.Contains(other[i]))
                    result++;
            }
            return result;
        }

        /// <summary>
        /// Recolors the palette, checking from oldColors and replacing with newColors at the corresponding indices. 
        /// </summary>
        public Palette Recolor(Color[] oldColors, Color[] newColors)
        {
            Palette result = new Palette(this);

            if (oldColors.Length > Maximum || newColors.Length > Maximum)
                throw new Exception("Palettes aren't the same length");
            if (oldColors.Length != newColors.Length)
                throw new Exception("Palettes aren't the same length");

            for (int i = 0; i < Count; ++i)
            {
                for (int j = 0; j < oldColors.Length; ++j)
                {
                    if (result.Colors[i] == oldColors[j])
                    {
                        result.Colors[i] = newColors[j];
                    }
                }
            }
            return result;
        }



        /// <summary>
        /// Returns an empty palette (with every color being black, 0x0000)
        /// </summary>
        public static Palette Empty(int max)
        {
            Palette result = new Palette(max);
            for (int i = 0; i < max; i++)
            {
                result.Add(new Color(0x0000));
            }
            return result;
        }

        /// <summary>
        /// Returns a Palette with the alpha bit of all its colors set to 0
        /// </summary>
        public static Palette Opacify(Palette palette)
        {
            Palette result = new Palette(palette.Maximum);
            for (int i = 0; i < palette.Count; i++)
            {
                result.Add(palette[i].SetAlpha(false));
            }
            return result;
        }

        /// <summary>
        /// Merges several (up to 16) 16-color palettes together
        /// </summary>
        public static Palette Merge(Palette[] palettes)
        {
            if (palettes.Length > 16) throw new Exception("Cannot merge more than 16 palettes together.");

            Palette result = new Palette(palettes.Length * GBA.Palette.MAX);
            for (int p = 0; p < palettes.Length; p++)
            {
                if (palettes[p].Maximum != 16)
                    throw new Exception("Palette number " + p + " has invalid color amount");

                result.Add(palettes[p]);
            }
            return result;
        }
        /// <summary>
        /// Splits the given palette into a jagged array of 16-color palettes
        /// </summary>
        public static Palette[] Split(Palette palette, int paletteAmount)
        {
            if (palette.Count > 256) throw new Exception("Cannot split more than 16 palettes");

            Palette[] result = new Palette[paletteAmount];
            for (int i = 0; i < paletteAmount; i++)
            {
                result[i] = new Palette(MAX);
                for (int j = 0; j < MAX; j++)
                {
                    if (16 * i + j >= palette.Count)
                        result[i].Add(new Color());
                    else result[i].Add(palette[16 * i + j]);
                }
            }
            return result;
        }



        /// <summary>
        /// Returns a byte array of this GBA.Palette.
        /// </summary>
        public Byte[] ToBytes(bool compressed)
        {
            byte[] result = new byte[Count * 2];
            byte[] color;
            int index = 0;
            for (int i = 0; i < Count; i++)
            {
                color = Colors[i].ToBytes(true);
                result[index] = color[0];
                result[index + 1] = color[1];
                index += 2;
            }
            return compressed ? LZ77.Compress(result) : result;
        }



        override public string ToString()
        {
            String result = "GBA.Palette:";
            for (int i = 0; i < Count; ++i)
            {
                result += "\n" + i + " - " + Colors[i].ToString();
            }
            return result;
        }
        override public bool Equals(object other)
        {
            Palette palette = (Palette)other;
            if (Count == palette.Count)
            {
                for (int i = 0; i < Maximum; i++)
                {
                    if (Colors[i] != palette[i]) return false;
                }
                return true;
            }
            else return false;
        }
        override public int GetHashCode()
        {
            int result = Count;
            for (int i = 0; i < Count; ++i)
            {
                result ^= Colors[i].Value;
            }
            return result;
        }
    }
}