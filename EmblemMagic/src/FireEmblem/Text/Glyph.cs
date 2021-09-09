using System;
using GBA;
using Magic;
using Magic.Components;

namespace EmblemMagic.FireEmblem
{
    /// <summary>
    /// The Glyph class represents a struct used for individual 16x16 font characters in the game
    /// </summary>
    public class Glyph : IDisplayable
    {
        public Int32 this[Int32 x, Int32 y]
        {
            get
            {
                Int32 index = ((x % 16) / 4) + ((y % 16) * 4);
                return ((Pixels[index] >> ((x % 4) * 2)) & 0x3);
            }
        }
        public Color GetColor(Int32 x, Int32 y)
        {
            Int32 index = ((x % 16) / 4) + ((y % 16) * 4);
            return Colors[(Pixels[index] >> ((x % 4) * 2)) & 0x3];
        }

        /// <summary>
        /// The length in bytes of a glyph struct in the ROM
        /// </summary>
        public const Int32 LENGTH = 0x48;

        public Int32 Width { get { return 16; } }
        public Int32 Height { get { return 16; } }

        /// <summary>
        /// The default 4-color palette used to show fonts
        /// </summary>
        public static Color[] Colors = new Color[4]
        {
            new Color(0x7FFF),
            new Color(0, 0x90, 0x90, 0x90),
            new Color(0, 0xCC, 0xCC, 0xCC),
            new Color()
        };

        /// <summary>
        /// The address of this glyph struct
        /// </summary>
        public Pointer Address { get; }
        /// <summary>
        /// the first 4 bytes of the glyph - "fontmap* linked_list_for_multibyte_encoding" says zahlman
        /// </summary>
        public Pointer LinkedAddress { get; }
        /// <summary>
        /// The byte at index 4 - is used for japanese fonts
        /// </summary>
        public Byte ShiftJIS { get; }
        /// <summary>
        /// The width of this glyph character when in a text
        /// </summary>
        public Byte TextWidth { get; }
        /// <summary>
        /// The byte array of 2bpp pixel data for this glyph character
        /// </summary>
        public Byte[] Pixels { get; }


        
        public Glyph(Pointer address)
        {
            Address = address;
            LinkedAddress = Core.ReadPointer(address);
            ShiftJIS = Core.ReadByte(address + 4);
            TextWidth = Core.ReadByte(address + 5);
            Pixels = Core.ReadData(address + 8, 4 * Height); // its 2bpp so there are 4 bytes per row
        }
        public Glyph(Bitmap image, Color[] colors)
        {
            ShiftJIS = 0;
            TextWidth = 0;
            Pixels = new Byte[4 * 16];
            Int32 index = 0;
            Int32[] row;
            for (Int32 y = 0; y < 16; y++)
            for (Int32 x = 0; x < 16; x += 4)
            {
                row = new Int32[4]
                {
                    image[x, y],
                    image[x + 1, y],
                    image[x + 2, y],
                    image[x + 3, y]
                };
                for (Int32 i = 0; i < 4; i++)
                {
                    if (row[i] != 0 && x + i > TextWidth)
                        TextWidth = (Byte)(x + i);
                }
                Pixels[index++] = (Byte)((row[3] << 6) | (row[2] << 4) | (row[1] << 2) | row[0]);
            }
            TextWidth++;
        }



        static Int32 GetColorIndex(Color[] colors, Color color)
        {
            for (Int32 i = 0; i < colors.Length; i++)
            {
                if (colors[i] == color)
                {
                    return i;
                }
            }
            return -1;
        }

        public Boolean IsEmpty()
        {
            for (Int32 i = 0; i < Pixels.Length; i++)
            {
                if (Pixels[i] != 0)
                    return false;
            }
            return true;
        }

        public Byte[] ToBytes()
        {
            Byte[] result = new Byte[LENGTH];

            result[5] = TextWidth;

            Array.Copy(Pixels, 0, result, 8, Pixels.Length);

            return result;
        }
        


        override public String ToString()
        {
            return "Glyph: " + Address;
        }
        override public Boolean Equals(Object other)
        {
            if (!(other is Glyph)) return false;
            Glyph glyph = (Glyph)other;
            for (Int32 i = 0; i < Pixels.Length; i++)
            {
                if (Pixels[i] != glyph.Pixels[i])
                    return false;
            }
            return true;
        }
        override public Int32 GetHashCode()
        {
            return Pixels.GetHashCode();
        }
    }
}
