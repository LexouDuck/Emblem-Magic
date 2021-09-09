using System;
using GBA;

namespace Magic
{
    /// <summary>
    /// A struct to describe a range of space.
    /// </summary>
    public struct Range
    {
        /// <summary>
        /// The starting point of this range.
        /// </summary>
        public UInt32 Start;

        /// <summary>
        /// The ending point of this range.
        /// </summary>
        public UInt32 End;

        /// <summary>
        /// The length of this range.
        /// </summary>
        public UInt32 Length
        {
            get
            {
                return End - Start;
            }
        }

        public Range(Int32 start, Int32 end)
        {
            // if (start == end) throw new Exception("Range cannot be of length 0");
            if (end < start) throw new Exception("Range cannot be negative, 'End' must be greater than 'Start'.");

            Start = (UInt32)start;
            End = (UInt32)end;
        }


        override public Boolean Equals(Object other)
        {
            if (!(other is Range)) return false;
            Range range = (Range)other;
            return (Start == range.Start && End == range.End);
        }
        override public Int32 GetHashCode()
        {
            return Start.GetHashCode() ^ End.GetHashCode();
        }
        override public String ToString()
        {
            return "Range, from " + Util.AddressToString(Start, 8) + " to " + Util.AddressToString(End, 8);
        }

        public static Boolean operator ==(Range left, Range right)
        {
            return (left.Start == right.Start && left.End == right.End);
        }
        public static Boolean operator !=(Range left, Range right)
        {
            return (left.Start != right.Start || left.End != right.End);
        }
    }
}
