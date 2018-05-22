using EmblemMagic;
using System;
using System.Collections;

namespace GBA
{
    /// <summary>
    /// Represents a 16-bit GBA Color, in A1B5G5R5 format
    /// </summary>
    public struct Color
    {
        public const ushort NO_ALPHA = 0x7FFF;
        public const ushort ALPHA = 0x8000;

        const ushort BITS_R = 0x001F;
        const ushort BITS_G = 0x03E0;
        const ushort BITS_B = 0x7C00;

        /// <summary>
        /// The 16-bit number that is the GBA.Color itself - this is the only field of this struct.
        /// </summary>
        public UInt16 Value { get; set; }



        public Color(Byte[] data)
        {
            Value = Util.BytesToUInt16(data, false);
        }
        public Color(UInt16 value)
        {
            Value = value;
        }
        public Color(UInt32 value)
        {
            Value = Get16bitColor(value, (value & 0xFF000000) != 0);
        }
        public Color(byte alpha, byte red, byte green, byte blue)
        {
            Value = Get16bitColor((alpha == 0) ? false : true, red, green, blue);
        }



        public byte GetValueR()
        {
            return (byte)(Value & BITS_R);
        }
        public byte GetValueG()
        {
            return (byte)((Value & BITS_G) >> 5);
        }
        public byte GetValueB()
        {
            return (byte)((Value & BITS_B) >> 10);
        }
        public bool GetAlpha()
        {
            return ((Value & ALPHA) == ALPHA);
        }

        public Color SetValueR(byte set)
        {
            if (set >= 32) throw new Exception("Cannot set GBA.Color channel to " + set);
            return new Color((UInt16)((Value & (ALPHA | BITS_G | BITS_B)) | set));
        }
        public Color SetValueG(byte set)
        {
            if (set >= 32) throw new Exception("Cannot set GBA.Color channel to " + set);
            return new Color((UInt16)((Value & (ALPHA | BITS_R | BITS_B)) | (set << 5)));
        }
        public Color SetValueB(byte set)
        {
            if (set >= 32) throw new Exception("Cannot set GBA.Color channel to " + set);
            return new Color((UInt16)((Value & (ALPHA | BITS_R | BITS_G)) | (set << 10)));
        }
        public Color SetAlpha(bool set)
        {
            if (set) return new Color((UInt16)(Value | ALPHA));
            else  return new Color((UInt16)(Value & NO_ALPHA));
        }

        /// <summary>
        /// This is the same as simply calling 'Value'
        /// </summary>
        public UInt16 To16bit()
        {
            return Value;
        }
        /// <summary>
        /// Returns 'this' Color as a 32-bit AARRGGBB color.
        /// </summary>
        public UInt32 To32bit()
        {
            return Get32bitColor(Value);
        }
        /// <summary>
        /// Returns 'this' Color as a 2-length byte array.
        /// </summary>
        public Byte[] ToBytes(bool littleEndian)
        {
            return Util.UInt16ToBytes(Value, littleEndian);
        }

        /// <summary>
        /// Returns the color defined by the given channels, in A1B5G5R5 16-bit format
        /// </summary>
        public static UInt16 Get16bitColor(bool alpha, byte red, byte green, byte blue)
        {
            red   = (byte)(red   >> 3);
            green = (byte)(green >> 3);
            blue  = (byte)(blue  >> 3);
            return (UInt16)(((alpha ? 1 : 0) << 15) | (blue << 10) | (green << 5) | (red));
        }
        /// <summary>
        /// Returns the given 32-bit Color as A1B5G5R5 16-bit format
        /// </summary>
        public static UInt16 Get16bitColor(UInt32 color, bool alpha = false)
        {
            byte a = (byte)((color & 0xFF000000) >> 0x18);
            byte r = (byte)((color & 0x00FF0000) >> 0x10);
            byte g = (byte)((color & 0x0000FF00) >> 0x08);
            byte b = (byte) (color & 0x000000FF);
            return Get16bitColor((a == 0) ? false : true, r, g, b);
        }
        /// <summary>
        /// Returns the given 16bit color as AARRGGBB 32bit
        /// </summary>
        public static UInt32 Get32bitColor(UInt16 color)
        {
            UInt32 result = (color >= ALPHA) ? 0x0 : 0xFF000000;
            result |= (uint)((color & BITS_B) >>  7);
            result |= (uint)((color & BITS_G) <<  6);
            result |= (uint)((color & BITS_R) << 19);
            return result;
        }

        /// <summary>
        /// Returns a blend of this Color and the one given
        /// </summary>
        public Color Blend(Color color)
        {
            byte r1 = GetValueR(); byte r2 = color.GetValueR();
            byte g1 = GetValueG(); byte g2 = color.GetValueG();
            byte b1 = GetValueB(); byte b2 = color.GetValueB();
            Color result = new Color();
            result.Value |= (UInt16) ((r1 + r2) / 2);
            result.Value |= (UInt16)(((g1 + g2) / 2) << 5);
            result.Value |= (UInt16)(((b1 + b2) / 2) << 10);
            return result;
        }

        /// <summary>
        /// Returns the index of the color nearest to the one given (possibly -1)
        /// </summary>
        public static int GetNearest(Palette palette, Color color)
        {
            byte R = color.GetValueR();
            byte G = color.GetValueG();
            byte B = color.GetValueB();
            int r;
            int g;
            int b;
            int sum;
            int min = 65536;
            int result = -1;
            for (int i = 0; i < palette.Count; i++)
            {
                r = Math.Abs(R - palette[i].GetValueR()); 
                g = Math.Abs(G - palette[i].GetValueG()); 
                b = Math.Abs(B - palette[i].GetValueB());
                sum = r + g + b;
                if (sum < min)
                {
                    min = sum;
                    result = i;
                }
            }
            return result;
        }



        override public string ToString()
        {
            return "GBA.Color: "
                + Util.UInt16ToHex(Value) + " | "
                + "R: " + GetValueR() + ", "
                + "G: " + GetValueG() + ", "
                + "B: " + GetValueB() + ", "
                + "A: " + (Value >= ALPHA);
        }
        override public bool Equals(object other)
        {
            if (!(other is Color)) return false;
            Color color = (Color)other;
            return (Value == color.Value);
        }
        override public int GetHashCode()
        {
            return Value.GetHashCode();
        }
        
        public static bool operator ==(Color left, Color right)
        {
            return (left.Value == right.Value);
        }
        public static bool operator !=(Color left, Color right)
        {
            return (left.Value != right.Value);
        }

        public static explicit operator System.Drawing.Color(GBA.Color color)
        {
            return System.Drawing.Color.FromArgb((int)color.To32bit());
        }
        public static explicit operator GBA.Color(System.Drawing.Color color)
        {
            return new Color(Get16bitColor((color.A != 0xFF), color.R, color.G, color.B));
        }
    }
}
