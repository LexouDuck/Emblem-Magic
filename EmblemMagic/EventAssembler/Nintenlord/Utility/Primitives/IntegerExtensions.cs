using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Utility.Primitives
{
    /// <summary>
    /// Extensions and helper methods to integers
    /// </summary>
    public static class IntegerExtensions
    {
        public static bool IsInRange(this int i, int min, int max)
        {
            return i <= max && i >= min;
        }

        public static bool IsInRangeHO(this int i, int min, int max)
        {
            return i < max && i >= min;
        }

        public static void Clamp(ref int i, int min, int max)
        {
            if (i < min)
            {
                i = min;
            }
            else if (i > max)
            {
                i = max;
            }
        }
        public static int Clamp(this int i, int min, int max)
        {
            return i < min ? min : 
                   i > max ? max : i;
        }

        public static int ToMod(this int i, int mod)
        {
            if (i % mod != 0)
            {
                i += mod - i % mod;
            }
            return i;
        }

        public static void ToMod(ref int i, int mod)
        {
            if (i % mod != 0)
            {
                i += mod - i % mod;
            }
        }

        public static string ToHexString(this int i, string prefix)
        {
            return ToHexString(i, prefix, "");
        }
        public static string ToHexString(this int i, string prefix, string postfix)
        {
            return prefix + Convert.ToString(i, 16).ToUpper() + postfix;
        }

        public static string ToBinString(this int i, string postfix)
        {
            return ToBinString(i, "", postfix);
        }
        public static string ToBinString(this int i, string prefix, string postfix)
        {
            return prefix + Convert.ToString(i, 2) + postfix;
        }

        public static bool Intersects(int index1, int length1, int index2, int length2)
        {
            if (length1 == 0 || length2 == 0)
                return false;
            return (index1 < index2 + length2 && index1 >= index2) ||
                   (index2 < index1 + length1 && index2 >= index1);
        }

        public static int ToPower2(this int value)
        {
            ToPower2(ref value);
            return value;
        }

        public static void ToPower2(ref int value)
        {
            value--;
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;
            value++;
        }

        public static int TrailingZeroCount(this int value)
        {
            if (value == 0) return 32;

            int result = 0;

            if ((value & 0x0000FFFF) == 0)
            {
                result += 16;
                value >>= 16;
            }
            if ((value & 0x000000FF) == 0)
            {
                result += 8;
                value >>= 8;
            }
            if ((value & 0x0000000F) == 0)
            {
                result += 4;
                value >>= 4;
            }
            if ((value & 0x00000003) == 0)
            {
                result += 2;
                value >>= 2;
            }
            if ((value & 0x00000001) == 0)
            {
                result += 1;
                //value >>= 1;
            }

            return result;
        }

        public static int LeadingZeroCount(this int value)
        {
            if (value == 0) return 32;

            int result = 0;

            if ((value & 0xFFFF0000) == 0)
            {
                result += 16;
                value <<= 16;
            }
            if ((value & 0xFF000000) == 0)
            {
                result += 8;
                value <<= 8;
            }
            if ((value & 0xF0000000) == 0)
            {
                result += 4;
                value <<= 4;
            }
            if ((value & 0xC0000000) == 0)
            {
                result += 2;
                value <<= 2;
            }
            if ((value & 0x80000000) == 0)
            {
                result += 1;
                //value <<= 1;
            }

            return result;
        }

        public static int TrailingZeroCount(this long value)
        {
            if (value == 0) return 64;

            int result = 0;

            if ((value & 0x00000000FFFFFFFF) == 0)
            {
                result += 32;
                value >>= 32;
            }
            if ((value & 0x000000000000FFFF) == 0)
            {
                result += 16;
                value >>= 16;
            }
            if ((value & 0x00000000000000FF) == 0)
            {
                result += 8;
                value >>= 8;
            }
            if ((value & 0x000000000000000F) == 0)
            {
                result += 4;
                value >>= 4;
            }
            if ((value & 0x0000000000000003) == 0)
            {
                result += 2;
                value >>= 2;
            }
            if ((value & 0x0000000000000001) == 0)
            {
                result += 1;
                //value >>= 1;
            }

            return result;
        }

        public static int LeadingZeroCount(this ulong value)
        {
            if (value == 0) return 64;

            int result = 0;

            if ((value & 0xFFFFFFFF00000000) == 0)
            {
                result += 32;
                value <<= 32;
            }
            if ((value & 0xFFFF000000000000) == 0)
            {
                result += 16;
                value <<= 16;
            }
            if ((value & 0xFF00000000000000) == 0)
            {
                result += 8;
                value <<= 8;
            }
            if ((value & 0xF000000000000000) == 0)
            {
                result += 4;
                value <<= 4;
            }
            if ((value & 0xC000000000000000) == 0)
            {
                result += 2;
                value <<= 2;
            }
            if ((value & 0x8000000000000000) == 0)
            {
                result += 1;
                //value <<= 1;
            }

            return result;
        }

        public static IEnumerable<int> GetIntegers(int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                yield return i;
            }
        }

        [Obsolete("Use System.Linq.Enumerable.Range instead", true)]
        public static IEnumerable<int> GetRange(int start, int length)
        {
            for (int i = start; i < start + length; i++)
            {
                yield return i;
            }
        }

        public static IEnumerable<int> GetIntegersNear(this int x, int range)
        {
            for (int i = x - range; i <= x + range; i++)
            {
                yield return i;
            }
        }

        public static int Lerp(int min, int max, double val, MidpointRounding roundingMode)
        {
            return min + (int)Math.Round((max - min) * val, roundingMode);
        }
    }
}
