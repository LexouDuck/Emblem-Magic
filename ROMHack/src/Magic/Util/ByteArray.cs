using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic
{
    /// <summary>
    /// Extension methods for byte arrays, so as to allow the same functionality as unsafe code with pointers
    /// </summary>
    public static class ByteArray
    {
        /// <summary>
        /// Returns a subsection of 'this' byte array.
        /// </summary>
        public static byte[] GetBytes(this byte[] data, uint offset, int length = -1)
        {
            if (length < 0) length = data.Length - (int)offset;
            byte[] result = new byte[length];
            Array.Copy(data, offset, result, 0, length);
            return result;
        }
        /// <summary>
        /// Returns an ASCII string from 'this' byte array
        /// </summary>
        public static String GetASCII(this byte[] data, uint offset, int length = -1)
        {
            if (length < 0) length = data.Length - (int)offset;
            byte[] result = new byte[length];
            Array.Copy(data, offset, result, 0, length);
            return Encoding.ASCII.GetString(result);
        }

        public static Int16 GetInt16(this byte[] data, uint offset, bool littleEndian)
        {
            byte[] result = new byte[2];
            Array.Copy(data, offset, result, 0, 2);
            return Util.BytesToInt16(result, littleEndian);
        }
        public static Int32 GetInt32(this byte[] data, uint offset, bool littleEndian)
        {
            byte[] result = new byte[4];
            Array.Copy(data, offset, result, 0, 4);
            return Util.BytesToInt32(result, littleEndian);
        }
        public static Int64 GetInt64(this byte[] data, uint offset, bool littleEndian)
        {
            byte[] result = new byte[8];
            Array.Copy(data, offset, result, 0, 8);
            if (littleEndian != BitConverter.IsLittleEndian) Array.Reverse(result);
            return BitConverter.ToInt64(result, 0);
        }
        public static UInt16 GetUInt16(this byte[] data, uint offset, bool littleEndian)
        {
            byte[] result = new byte[2];
            Array.Copy(data, offset, result, 0, 2);
            return Util.BytesToUInt16(result, littleEndian);
        }
        public static UInt32 GetUInt32(this byte[] data, uint offset, bool littleEndian)
        {
            byte[] result = new byte[4];
            Array.Copy(data, offset, result, 0, 4);
            return Util.BytesToUInt32(result, littleEndian);
        }
        public static UInt64 GetUInt64(this byte[] data, uint offset, bool littleEndian)
        {
            byte[] result = new byte[8];
            Array.Copy(data, offset, result, 0, 8);
            if (littleEndian != BitConverter.IsLittleEndian) Array.Reverse(result);
            return BitConverter.ToUInt64(result, 0);
        }

        public static Byte[] Make_Int32(UInt32 input)
        {
            byte[] result = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian) Array.Reverse(result);
            return result;
        }
        public static Byte[] Make_Int64(UInt64 input)
        {
            byte[] result = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian) Array.Reverse(result);
            return result;
        }
        public static Byte[] Make_ASCII(String input)
        {
            byte[] result = System.Text.Encoding.ASCII.GetBytes(input);
            return result;
        }
    }
}
