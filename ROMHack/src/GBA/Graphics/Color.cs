using Magic;
using System;
using System.Collections;

namespace GBA
{
    /// <summary>
    /// Represents a 16-bit GBA Color, in A1B5G5R5 format
    /// </summary>
    public struct Color
    {
        public const UInt16 NO_ALPHA = 0x7FFF;
        public const UInt16 ALPHA = 0x8000;

        const UInt16 BITS_R = 0x001F;
        const UInt16 BITS_G = 0x03E0;
        const UInt16 BITS_B = 0x7C00;

        const UInt16 OFFSET_R = 0;
        const UInt16 OFFSET_G = 5;
        const UInt16 OFFSET_B = 10;
        const UInt16 OFFSET_A = 15;

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
        public Color(Byte alpha, Byte red, Byte green, Byte blue)
        {
            Value = Get16bitColor((alpha == 0) ? false : true, red, green, blue);
        }



        public Byte GetValueR()
        {
            return (Byte)(Value & BITS_R);
        }
        public Byte GetValueG()
        {
            return (Byte)((Value & BITS_G) >> 5);
        }
        public Byte GetValueB()
        {
            return (Byte)((Value & BITS_B) >> 10);
        }
        public Boolean GetAlpha()
        {
            return ((Value & ALPHA) == ALPHA);
        }

        public Color SetValueR(Byte set)
        {
            if (set >= 32) throw new Exception("Cannot set GBA.Color channel to " + set);
            return new Color((UInt16)((Value & (ALPHA | BITS_G | BITS_B)) | set));
        }
        public Color SetValueG(Byte set)
        {
            if (set >= 32) throw new Exception("Cannot set GBA.Color channel to " + set);
            return new Color((UInt16)((Value & (ALPHA | BITS_R | BITS_B)) | (set << 5)));
        }
        public Color SetValueB(Byte set)
        {
            if (set >= 32) throw new Exception("Cannot set GBA.Color channel to " + set);
            return new Color((UInt16)((Value & (ALPHA | BITS_R | BITS_G)) | (set << 10)));
        }
        public Color SetAlpha(Boolean set)
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
        public Byte[] ToBytes(Boolean littleEndian)
        {
            return Util.UInt16ToBytes(Value, littleEndian);
        }

        /// <summary>
        /// Returns the color defined by the given channels, in A1B5G5R5 16-bit format
        /// </summary>
        public static UInt16 Get16bitColor(Boolean alpha, Byte red, Byte green, Byte blue)
        {
            red   = (Byte)(red   >> 3);
            green = (Byte)(green >> 3);
            blue  = (Byte)(blue  >> 3);
            return (UInt16)(((alpha ? 1 : 0) << 15) | (blue << 10) | (green << 5) | (red));
        }
        /// <summary>
        /// Returns the given 32-bit Color as A1B5G5R5 16-bit format
        /// </summary>
        public static UInt16 Get16bitColor(UInt32 color, Boolean alpha = false)
        {
            Byte a = (Byte)((color & 0xFF000000) >> 0x18);
            Byte r = (Byte)((color & 0x00FF0000) >> 0x10);
            Byte g = (Byte)((color & 0x0000FF00) >> 0x08);
            Byte b = (Byte) (color & 0x000000FF);
            return Get16bitColor((a == 0) ? false : true, r, g, b);
        }
        /// <summary>
        /// Returns the given 16bit color as AARRGGBB 32bit
        /// </summary>
        public static UInt32 Get32bitColor(UInt16 color)
        {
            UInt32 result = (color >= ALPHA) ? 0x0 : 0xFF000000;
            UInt32 channel_B = (UInt32)((color & BITS_B) >> OFFSET_B);
            UInt32 channel_G = (UInt32)((color & BITS_G) >> OFFSET_G);
            UInt32 channel_R = (UInt32)((color & BITS_R) >> OFFSET_R);
            result |= (UInt32)(Math.Round(channel_B * (255.0 / 31.0))) << 0;
            result |= (UInt32)(Math.Round(channel_G * (255.0 / 31.0))) << 8;
            result |= (UInt32)(Math.Round(channel_R * (255.0 / 31.0))) << 16;
            return result;
        }

        /// <summary>
        /// Returns a blend of this Color and the one given
        /// </summary>
        public Color Blend(Color color)
        {
            Byte r1 = GetValueR(); Byte r2 = color.GetValueR();
            Byte g1 = GetValueG(); Byte g2 = color.GetValueG();
            Byte b1 = GetValueB(); Byte b2 = color.GetValueB();
            Color result = new Color();
            result.Value |= (UInt16) ((r1 + r2) / 2);
            result.Value |= (UInt16)(((g1 + g2) / 2) << 5);
            result.Value |= (UInt16)(((b1 + b2) / 2) << 10);
            return result;
        }

        /// <summary>
        /// Returns the index of the color nearest to the one given (possibly -1)
        /// </summary>
        public static Int32 GetNearest(Palette palette, Color color)
        {
            Byte R = color.GetValueR();
            Byte G = color.GetValueG();
            Byte B = color.GetValueB();
            Int32 r;
            Int32 g;
            Int32 b;
            Int32 sum;
            Int32 min = 65536;
            Int32 result = -1;
            for (Int32 i = 0; i < palette.Count; i++)
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



        override public String ToString()
        {
            return "GBA.Color: "
                + Util.UInt16ToHex(Value) + " | "
                + "R: " + GetValueR() + ", "
                + "G: " + GetValueG() + ", "
                + "B: " + GetValueB() + ", "
                + "A: " + (Value >= ALPHA);
        }
        override public Boolean Equals(Object other)
        {
            if (!(other is Color)) return false;
            Color color = (Color)other;
            return (Value == color.Value);
        }
        override public Int32 GetHashCode()
        {
            return Value.GetHashCode();
        }
        
        public static Boolean operator ==(Color left, Color right)
        {
            return (left.Value == right.Value);
        }
        public static Boolean operator !=(Color left, Color right)
        {
            return (left.Value != right.Value);
        }

        public static explicit operator System.Drawing.Color(GBA.Color color)
        {
            return System.Drawing.Color.FromArgb((Int32)color.To32bit());
        }
        public static explicit operator GBA.Color(System.Drawing.Color color)
        {
            return new Color(Get16bitColor((color.A != 0xFF), color.R, color.G, color.B));
        }
    }
}
