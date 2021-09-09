using Magic;
using System;

namespace GBA
{
    /// <summary>
    /// The GBA.Pointer type is basically an int, and this type implicitly converts itself with the int type
    /// </summary>
    public struct Pointer
    {
        /// <summary>
        /// At runtime on a GBA, address are offset because of the hardware - pointers in the ROM all start with 0X08
        /// </summary>
        public const Int32 HardwareOffset = 0x08000000;



        /// <summary>
        /// The address in the ROM that this Pointer points to.
        /// </summary>
        public UInt32 Address { get; private set; }



        /// <summary>
        /// Makes a Pointer from the given byte array. The 2 bools will decide how the byte array is read.
        /// </summary>
        public Pointer(Byte[] data, Boolean littleEndian = true, Boolean hardwareOffset = true)
        {
            if (data.Length != 4)
                throw new Exception("given byte array is of invalid length: " + data.Length);

            if (data[0] == 0 &&
                data[1] == 0 &&
                data[2] == 0 &&
                data[3] == 0) {
                Address = 0;
                return;
            }
            UInt32 address = Util.BytesToUInt32(data, littleEndian);

            if (hardwareOffset)
            {
                if (address < HardwareOffset)
                    throw new Exception("Pointer is smaller than the GBA hardware offset: " + Util.AddressToString(address, 8));
                address -= HardwareOffset;
            }
            //if (address >= Core.CurrentROMSize)
            //    throw new Exception("Pointer is beyond the range of the ROM: " + Util.AddressToString(address, 8));

            Address = (UInt32)address;
        }
        /// <summary>
        /// Makes a Pointer from the given address. The 2 bools will decide how the int address is read.
        /// </summary>
        public Pointer(UInt32 address, Boolean littleEndian = false, Boolean hardwareOffset = false)
        {
            if (address == 0)
            {
                Address = 0;
                return;
            }
            if (littleEndian)
            {
                Byte[] pointer = Util.UInt32ToBytes(address, true);
                address = Util.BytesToUInt32(pointer, false);
            }
            if (hardwareOffset)
            {
                if (address < HardwareOffset)
                    throw new Exception("Pointer is smaller than the GBA hardware offset: " + Util.AddressToString(address, 8));
                address -= HardwareOffset;
            }
            //if (address >= Core.CurrentROMSize)
            //    throw new Exception("Pointer is beyond the range of the ROM: " + Util.AddressToString(address, 8));

            Address = address;
        }



        /// <summary>
        /// Returns a 4-byte array of this pointer, that differs depending on littleEndian and hardwareOffset
        /// </summary>
        public Byte[] ToBytes(Boolean littleEndian = true, Boolean hardwareOffset = true)
        {
            if (Address == 0) return new Byte[4] { 0x00, 0x00, 0x00, 0x00 };

            return Util.UInt32ToBytes((hardwareOffset) ? Address + HardwareOffset : Address, littleEndian);
        }
        


        override public String ToString()
        {
            return Util.AddressToString(Address);
        }
        override public Boolean Equals(Object other)
        {
            if (!(other is Pointer)) return false;
            Pointer pointer = (Pointer)other;
            return (Address == pointer.Address);
        }
        override public Int32 GetHashCode()
        {
            return Address.GetHashCode();
        }

        public static Boolean operator ==(Pointer left, Pointer right)
        {
            return (left.Address == right.Address);
        }
        public static Boolean operator !=(Pointer left, Pointer right)
        {
            return (left.Address != right.Address);
        }

        public static Pointer operator +(Pointer left, Pointer right)
        {
            return new Pointer((UInt32)(left.Address + right.Address));
        }
        public static Pointer operator +(Pointer left, UInt32 right)
        {
            return new Pointer(left.Address + right);
        }
        public static Pointer operator +(Pointer left, Int32 right)
        {
            return new Pointer((UInt32)(left.Address + right));
        }
        public static Int32 operator -(Pointer left, Pointer right)
        {
            return (Int32)(left.Address - right.Address);
        }
        public static Pointer operator -(Pointer left, UInt32 right)
        {
            return new Pointer(left.Address - right);
        }
        public static Pointer operator -(Pointer left, Int32 right)
        {
            return new Pointer((UInt32)(left.Address - right));
        }

        static public implicit operator UInt32(Pointer pointer)
        {
            return pointer.Address;
        }
        static public implicit operator Int32(Pointer pointer)
        {
            return (Int32)pointer.Address;
        }

        internal void CompareTo(Pointer item1)
        {
            throw new NotImplementedException();
        }
    }
}
