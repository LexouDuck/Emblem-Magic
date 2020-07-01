using System;
using System.Collections.Generic;

namespace Compression
{
    /// <summary>
    /// This class provides static methods to convert variable-length encoded quantities unsigned 64-bit ints and back
    /// </summary>
    public static class VLQ
    {
        /// <summary>
        /// Returns a byte array of the encoded VLQ for the given number
        /// </summary>
        public static byte[] Encode(UInt64 number)
        {
            List<byte> result = new List<byte>(8);
            UInt64 temp = 0x7F & number;
            number >>= 7;
            while (number != 0x00)
            {
                result.Add((byte)temp);
                number--;
                temp = 0x7F & number;
                number >>= 7;
            }
            result.Add((byte)(0x80 | temp));
            return result.ToArray();
        }
        /// <summary>
        /// Returns the UInt64 number described in the given VLQ byte array
        /// </summary>
        public static UInt64 Decode(byte[] vlq)
        {
            UInt64 result = 0;
            int shift = 1;
            int index = 0;
            byte temp = vlq[index++];
            result += (UInt64)((temp & 0x7F) * shift);
            while ((temp & 0x80) == 0)
            {
                shift <<= 7;
                result += (UInt64)shift;
                temp = vlq[index++];
                result += (UInt64)((temp & 0x7F) * shift);
            }
            return result;
        }
    }
}