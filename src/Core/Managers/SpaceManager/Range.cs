using System;
using GBA;

namespace EmblemMagic
{
    /// <summary>
    /// A struct to describe a range of space.
    /// </summary>
    public struct Range
    {
        /// <summary>
        /// The starting point of this range.
        /// </summary>
        public uint Start;

        /// <summary>
        /// The ending point of this range.
        /// </summary>
        public uint End;

        /// <summary>
        /// The length of this range.
        /// </summary>
        public uint Length
        {
            get
            {
                return End - Start;
            }
        }

        public Range(int start, int end)
        {
            // if (start == end) throw new Exception("Range cannot be of length 0");
            if (end < start) throw new Exception("Range cannot be negative, 'End' must be greater than 'Start'.");

            Start = (uint)start;
            End = (uint)end;
        }


        override public bool Equals(object other)
        {
            if (!(other is Range)) return false;
            Range range = (Range)other;
            return (Start == range.Start && End == range.End);
        }
        override public int GetHashCode()
        {
            return Start.GetHashCode() ^ End.GetHashCode();
        }
        override public string ToString()
        {
            return "Range, from " + Util.AddressToString(Start, 8) + " to " + Util.AddressToString(End, 8);
        }

        public static bool operator ==(Range left, Range right)
        {
            return (left.Start == right.Start && left.End == right.End);
        }
        public static bool operator !=(Range left, Range right)
        {
            return (left.Start != right.Start || left.End != right.End);
        }
    }
}
