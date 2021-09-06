using GBA;
using System;

namespace Magic
{
    /// <summary>
    /// An area of marked space on the ROM, with a reference to the 'Mark' itself
    /// </summary>
    public class Space
    {
        /// <summary>
        /// The marking type of this area of marked space.
        /// </summary>
        public Mark Marked { get; set; }

        /// <summary>
        /// The address at which this marked ROM space starts.
        /// </summary>
        public Pointer Address { get; set; }

        /// <summary>
        /// The address at which this marked ROM space ends.
        /// </summary>
        public Pointer EndByte { get; set; }



        /// <summary>
        /// Gets the length of the area of marked space.
        /// </summary>
        public Int32 Length
        {
            get
            {
                return EndByte - Address;
            }
        }



        public Space(Mark mark, Int32 start, Int32 end)
        {
            if (end < start) throw new Exception("space cannot be negative.");
            
            Marked = mark;
            Address = new Pointer((UInt32)start);
            EndByte = new Pointer((UInt32)end);
        }
        /// <summary>
        /// Constructor used for loading from file.
        /// </summary>
        public Space(Mark mark, UInt32 start, UInt32 end)
        {
            if (end < start) throw new Exception("space cannot be negative.");

            Marked = mark;
            Address = new Pointer(start);
            EndByte = new Pointer(end);
        }

        public Boolean Contains(Pointer address)
        {
            if (address >= Address && address < EndByte)
                return true;
            else return false;
        }
    }
}
