using GBA;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Magic
{
    public static class Util
    {
        /// <summary>
        /// Returns a bool for the bit at the given index (right-to-left)
        /// </summary>
        public static Boolean GetBit(this Byte data, Int32 index)
        {
            if (index < 0 || index >= 8)
                throw new Exception("bit index given is invalid.");

            Byte mask = (Byte)(1 << index);

            return (((data & mask) >> index) == 1) ? true : false;
        }
        /// <summary>
        /// Returns the given byte with the one bit at 'index' changed to 'set'.
        /// </summary>
        public static Byte SetBit(this Byte data, Int32 index, Boolean set)
        {
            Byte mask = (Byte)(1 << index);

            if (set) data |= mask;
            else     data &= (Byte)~mask;

            return data;
        }
        
        /// <summary>
        /// Returns a bitshifted number from the given area to bitmask (right-to-left)
        /// </summary>
        public static UInt32 GetBits(Byte[] data, Int32 index, Int32 length)
        {
            if (length == 0)
                return (0);
            if (length < 0)
                throw new Exception("negative length given.");
            if (length >= 32)
                throw new Exception("Length given is too large, must be below 32");
            if (index < 0 || (index / 8) >= data.Length || ((index + length) / 8) > data.Length)
                throw new Exception("requested bit area goes outside the byte array.\n" +
                    "Requested index:" + index + " length: " + length + ", data length is " + data.Length);
            Int32 start = index >> 3;
            Int32 end = (index + length) >> 3;
            if ((index + length) % 8 == 0)
                end -= 1;
            UInt32 mask;
            Int32 bit_index = index % 8;
            if (start == end)
            {
                mask = (UInt32)((0x1 << length) - 1);
                if (bit_index + length == 8)
                    return (data[start] & mask);
                else
                    return ((UInt32)(data[start] >> (8 - bit_index - length)) & mask);
            }
            Int32 sublength = length;
            UInt32 result = 0;
            for (Int32 i = start; i <= end; i++)
            {
                mask = (UInt32)((0x1 << length) - 1);
                if (i == start)
                {
                    if (sublength > 8) sublength = 8;
                    if (bit_index + sublength > 8) sublength -= bit_index;
                    mask = (UInt32)((0x1 << sublength) - 1);
                    result |= (data[i] & mask);
                    result <<= sublength;
                }
                else if (i == end)
                {
                    sublength = (index + length) % 8;
                    mask = (UInt32)((0x1 << sublength) - 1);
                    if (mask == 0) mask = 0xFF;
                    result |= (data[i] & mask) >> (8 - sublength);
                    break;
                }
                else
                {
                    result |= data[i];
                    result <<= 8;
                }
            }
            return (result);
        }
        /// <summary>
        /// Returns the given byte with the bits from 'index to 'length' replaced by a bitshifted 'set'.
        /// </summary>
        public static Byte SetBits(Byte data, Byte set, Int32 index, Int32 length)
        {
            if (index < 0 || index >= 8 || length <= 0 || index + length > 8)
                throw new Exception("bitmask area goes outside the byte.");

            Byte mask = (Byte)(((0x1 << length) - 1) << index);
            Byte result = (Byte)(data & ~mask);
            result &= (Byte)(set << index);
            return result;
        }

        /// <summary>
        /// Takes a char (0, 1, 2, ..., A, B, C etc) and returns the corresponding byte
        /// </summary>
        public static Byte DigitToByte(Char digit)
        {
            if (digit >= '0' && digit <= '9')
            {
                return (Byte)(digit - '0');
            }
            else if (digit >= 'a' && digit <= 'f')
            {
                return (Byte)(digit + 10 - 'a');
            }
            else if (digit >= 'A' && digit <= 'F')
            {
                return (Byte)(digit + 10 - 'A');
            }
            throw new Exception("Invalid hex digit: " + digit);
        }
        /// <summary>
        /// Takes a 0-15 number and returns a string char of the hex digit that number would be
        /// </summary>
        public static Char ByteToDigit(Byte number)
        {
            if (number >= 16) throw new Exception("given number is more than 1 hex digit");
            if (number <= 9)
            {
                return (Char)(number + '0');
            }
            else
            {
                return (Char)(number - 10 + 'A');
            }
        }

        /// <summary>
        /// Takes in a byte and returns a 2-length string of that byte as hex.
        /// </summary>
        public static String ByteToHex(Byte data)
        {
            String result = "";
            Int32 HI_nibble = data >> 4;
            Int32 LO_nibble = data & 0x0F;

            if (HI_nibble <= 9) { result += (Char)(HI_nibble + '0'); }
            else { result += (Char)(HI_nibble - 10 + 'A'); }
            // HI goes first
            if (LO_nibble <= 9) { result += (Char)(LO_nibble + '0'); }
            else { result += (Char)(LO_nibble - 10 + 'A'); }

            return result;
        }
        /// <summary>
        /// Takes a 2-length string and returns the corresponding byte
        /// </summary>
        public static Byte HexToByte(String oneByte)
        {
            if (oneByte.Length != 2) throw new Exception("given string has a length different than 2");

            Byte HI_nibble = DigitToByte(oneByte[0]);
            Byte LO_nibble = DigitToByte(oneByte[1]);
            LO_nibble += (Byte)(HI_nibble << 4);
            return LO_nibble;
        }
        /// <summary>
        /// Takes a byte array and returns a simple hex string of the given data
        /// </summary>
        public static String BytesToHex(Byte[] data)
        {
            String result = "";
            for (Int32 i = 0; i < data.Length; i++)
            {
                result += ByteToHex(data[i]);
            }
            return result;
        }
        /// <summary>
        /// Returns a byte array by parsing the given unspaced hex string
        /// </summary>
        public static Byte[] HexToBytes(String hex)
        {
            Byte[] result = new Byte[hex.Length / 2];
            String buffer;
            for (Int32 i = 0; i < result.Length; i++)
            {
                buffer = "";
                buffer += hex[i];
                buffer += hex[i + 1];
                result[i] = HexToByte(buffer);
            }
            return result;
        }

        /// <summary>
        /// Returns a one-line spaced hex respresentation of the given byte array - like "FF AB F0 00"
        /// </summary>
        public static String BytesToSpacedHex(Byte[] data)
        {
            String acc = "";
            foreach (Byte hex in data)
            {
                acc += ByteToHex(hex) + ' ';
            }
            return acc;
        }
        /// <summary>
        /// Takes in spaced hex, that looks like 2E 00 00 EA 24 FF AE 51 69 9A A2 21 3D 84 82 0A
        /// </summary>
        public static Byte[] SpacedHexToBytes(String spaced_hex)
        {
            String[] hex = spaced_hex.Trim().Split(new Char[] { ' ' });//nod to cam

            Byte[] data = new Byte[hex.Length];
            for (Int32 i = 0; i < hex.Length; i++)
            {
                data[i] = HexToByte(hex[i]);
            }
            return data;
        }
        
        /// <summary>
        /// Returns an unsigned 16-bit integer from the given 2-length byte array.
        /// </summary>
        public static UInt16 BytesToUInt16(Byte[] data, Boolean littleEndian)
        {
            if (data.Length != 2) throw new Exception("given data has invalid length: " + data.Length);
            UInt16 result = 0;
            if (littleEndian)
                result = (UInt16)(data[0] + (data[1] << 8));
            else result = (UInt16)(data[1] + (data[0] << 8));
            return result;
        }
        /// <summary>
        /// Returns a 2-length byte array from the given unsigned 16-bit integer
        /// </summary>
        public static Byte[] UInt16ToBytes(UInt16 number, Boolean littleEndian)
        {
            Byte[] result = new Byte[2];
            Byte[] bytes = BitConverter.GetBytes(number);
            if (littleEndian != BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            Array.Copy(bytes, result, 2);
            return result;
        }
        /// <summary>
        /// Returns a signed 32-bit integer from the given 4-length byte array
        /// </summary>
        public static UInt32 BytesToUInt32(Byte[] data, Boolean littleEndian)
        {
            if (data.Length != 4) throw new Exception("given data has invalid length: " + data.Length);
            UInt32 result;
            if (littleEndian)
                result = (UInt32)(data[0] + (data[1] << 8) + (data[2] << 16) + (data[3] << 24));
            else result = (UInt32)(data[3] + (data[2] << 8) + (data[1] << 16) + (data[0] << 24));
            return result;
        }
        /// <summary>
        /// Returns a 4-length byte array fro the given  signed 32-bit iitegeer
        /// </summary>
        public static Byte[] UInt32ToBytes(UInt32 number, Boolean littleEndian)
        {
            Byte[] result = new Byte[4];
            Byte[] bytes = BitConverter.GetBytes(number);
            if (littleEndian != BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            Array.Copy(bytes, result, 4);
            return result;
        }

        /// <summary>
        /// Returns a signed 16-bit integer from the given 2-length byte array.
        /// </summary>
        public static Int16 BytesToInt16(Byte[] data, Boolean littleEndian)
        {
            if (data.Length != 2) throw new Exception("given data has invalid length: " + data.Length);
            if (littleEndian != BitConverter.IsLittleEndian)
                Array.Reverse(data);
            return BitConverter.ToInt16(data, 0);
        }
        /// <summary>
        /// Returns a 2-length byte array from the given unsigned 16-bit integer
        /// </summary>
        public static Byte[] Int16ToBytes(Int16 number, Boolean littleEndian)
        {
            Byte[] result = new Byte[2];
            Byte[] bytes = BitConverter.GetBytes(number);
            if (littleEndian != BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            Array.Copy(bytes, result, 2);
            return result;
        }
        /// <summary>
        /// Returns a signed 32-bit integer from the given 4-length byte array
        /// </summary>
        public static Int32 BytesToInt32(Byte[] data, Boolean littleEndian)
        {
            if (data.Length != 4) throw new Exception("given data has invalid length: " + data.Length);
            if (littleEndian != BitConverter.IsLittleEndian)
                Array.Reverse(data);
            return BitConverter.ToInt32(data, 0);
        }
        /// <summary>
        /// Returns a 4-length byte array fro the given signed 32-bit integer
        /// </summary>
        public static Byte[] Int32ToBytes(Int32 number, Boolean littleEndian)
        {
            Byte[] result = new Byte[4];
            Byte[] bytes = BitConverter.GetBytes(number);
            if (littleEndian != BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            Array.Copy(bytes, result, 4);
            return result;
        }

        /// <summary>
        /// Returns a 4-length hex string of the given unsigned 16-bit integer
        /// </summary>
        public static String UInt16ToHex(UInt16 value)
        {
            return IntToHex(value).PadLeft(4, '0');
        }
        /// <summary>
        /// Returns an 8-length hex string of the given unsigned 32-bit integer
        /// </summary>
        public static String UInt32ToHex(UInt32 value)
        {
            return IntToHex(value).PadLeft(8, '0');
        }
        /// <summary>
        /// Returns an unsigned int corresponding to the given string hex, removes "0x" if necessary
        /// </summary>
        public static UInt32 HexToInt(String hex)
        {
            if (hex.StartsWith("0x") || hex.StartsWith("0X"))
                hex = hex.Substring(2);
            if (hex.Length > 8) throw new Exception("Given hex is of invalid length: " + hex.Length);

            UInt32 value = 0;
            for (Int32 i = 0; i < hex.Length; i++)
            {
                value = (value << 4) + DigitToByte(hex[i]);
            }
            return value;
        }
        /// <summary>
        /// Returns a variable-length hexadecimal string of the given int
        /// </summary>
        public static String IntToHex(UInt32 value)
        {
            String result = "";
            for (Int32 i = 0; i < 8; i++)
            {
                if (value == 0 && result.Length > 0) { break; }
                result = ByteToDigit((Byte)(value & 0xF)) + result;
                value >>= 4;
            }
            return result;
        }
        
        /// <summary>
        /// Returns a string represting a hex address, like "0x0000FF" - works with 32-bit ints
        /// </summary>
        public static String AddressToString(UInt32 address, Int32 fixedLength = 0)
        {
            String result = UInt32ToHex(address);
            String zeros = "";
            if (fixedLength != 0)
            {
                for (Int32 i = 0; i < fixedLength - result.Length; i++)
                {
                    zeros += "0";
                }
            }
            return "0x" + zeros + result;
        }
        /// <summary>
        /// Returns the GBA.Pointer corresponding to the given string address (removes "0x" if any)
        /// </summary>
        public static Pointer StringToAddress(String address)
        {
            if (address == null) throw new Exception("Address given is null.");
            String result;
            if (address.StartsWith("0x"))
                result = address.Substring(2);
            else if (address.StartsWith("$"))
                result = address.Substring(1);
            else
                result = address;
            if (result.Length > 8) throw new Exception("The given offset is longer than Int32 permits.");
            return new Pointer(HexToInt(result));
        }

        /// <summary>
        /// returns a 64-bit signed int from a byte array of any length 
        /// </summary>
        public static Int64 BytesToNumber(Byte[] data, Boolean littleEndian)
        {
            if (data == null) throw new Exception("data given is null.");
            switch (data.Length)
            {
                case 0: return 0;
                case 1: return data[0];
                case 2: return BytesToUInt16(data, littleEndian);
                case 3: return BytesToUInt32(new Byte[4] { 0, data[0], data[1], data[2] }, littleEndian);
                case 4: return BytesToUInt32(data, littleEndian);
                default: if (littleEndian != BitConverter.IsLittleEndian) Array.Reverse(data); return BitConverter.ToInt64(data, 0);
            }
        }
        /// <summary>
        /// Returns a byte array of 'length' from the number given.
        /// </summary>
        public static Byte[] NumberToBytes(Int64 number, Int32 length, Boolean signed)
        {
            if (length < 0) throw new Exception("requested byte array length cannot be negative.");
            Byte[] result = new Byte[length];
            switch (length)
            {
                case 0: return new Byte[0];
                case 1: return new Byte[1] { (Byte)number };
                case 2: return (signed) ?
                    Int16ToBytes((Int16)number, false) :
                    UInt16ToBytes((UInt16)number, false);
                case 3: throw new Exception("a 3-byte number has been requested");
                case 4: return (signed) ?
                    Int32ToBytes((Int32)number, false) :
                    UInt32ToBytes((UInt32)number, false);
                default:
                    Byte[] bytes = BitConverter.GetBytes(number);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(bytes);
                    return bytes;
            }
        }

        /// <summary>
        /// Takes a byte array of length 4 or under and returns a signed int from it (signed byte if data.Length == 1 for example)
        /// </summary>
        public static Int32 BytesToSInt(Byte[] data, Boolean littleEndian)
        {
            if (data.Length == 3) data = (littleEndian) ?
                    new Byte[4] { 0x00, data[0], data[1], data[2] } :
                    new Byte[4] { data[2], data[1], data[0], 0x00 };
            switch (data.Length)
            {
                case 1: return (Int32)(SByte)data[0];
                case 2: return (Int32)BytesToInt16(data, littleEndian);
                case 4: return (Int32)BytesToInt32(data, littleEndian);
                default: throw new Exception("data given has invalid length.");
            }
        }
        /// <summary>
        /// Takes a byte array of length 4 or under and returns an unsigned int from it
        /// </summary>
        public static UInt32 BytesToUInt(Byte[] data, Boolean littleEndian)
        {
            if (data.Length == 3) data = (littleEndian) ?
                    new Byte[4] { 0x00, data[0], data[1], data[2] } :
                    new Byte[4] { data[2], data[1], data[0], 0x00 };
            switch (data.Length)
            {
                case 1: return (UInt32)data[0];
                case 2: return (UInt32)BytesToUInt16(data, littleEndian);
                case 4: return (UInt32)BytesToUInt32(data, littleEndian);
                default: throw new Exception("data given has invalid length.");
            }
        }

        public static Boolean IsHexDigit(Char c)
        {
            return (('0' <= c && c <= '9') ||
                    ('A' <= c && c <= 'F') ||
                    ('a' <= c && c <= 'f'));
        }

        public static List<Boolean> IntToBits(Int32 i, Int32 arrayLength)
        {
            List<Boolean> bits = new List<Boolean>();

            Int32 temp = i;

            while (temp > 0)
            {
                if ((temp & 1) == 1)
                {
                    bits.Add(true);
                }
                else
                {
                    bits.Add(false);
                }

                temp >>= 1;
            }

            while (bits.Count < arrayLength)
            {
                bits.Add(false);
            }

            return bits;
        }
        public static Int32 BitsToInt(List<Boolean> bits)
        {
            Int32 result = 0;

            Int32 index = 0; // TODO this is broken

            foreach (Boolean b in bits)
            {
                if (b)
                {
                    result |= 1 << (bits.Count - index - 1);
                }

                index++;
            }

            return result;
        }
        public static List<Boolean> ByteToBits(Byte i, Int32 arrayLength)
        {
            List<Boolean> bits = new List<Boolean>();

            Int32 temp = i;

            while (temp > 0)
            {
                if ((temp & 1) == 1)
                {
                    bits.Add(true);
                }
                else
                {
                    bits.Add(false);
                }

                temp >>= 1;
            }

            while (bits.Count < arrayLength)
            {
                bits.Add(false);
            }

            return bits;
        }
        public static Byte BitsToByte(Boolean[] bits)
        {
            Byte result = 0;

            Int32 index = 8 - bits.Length;

            foreach (Boolean b in bits)
            {
                if (b)
                {
                    result |= (Byte)(1 << (7 - index));
                }

                index++;
            }

            return result;
        }

        /// <summary>
        /// Returns a string of the given byte amount, like "XX MB (XX,XXX,XXX Bytes)"
        /// </summary>
        public static String GetDisplayBytes(Int64 size)
        {
            const Int64 multi = 1024;
            Int64 kb = multi;
            Int64 mb = kb * multi;
            Int64 gb = mb * multi;
            Int64 tb = gb * multi;

            const String BYTES = "Bytes";
            const String KB = "KB";
            const String MB = "MB";
            const String GB = "GB";
            const String TB = "TB";

            String result;
            if (size < kb)
                result = String.Format("{0} {1}", size, BYTES);
            else if (size < mb)
                result = String.Format("{0} {1} ({2} Bytes)",
                    ConvertToOneDigit(size, kb), KB, ConvertBytesDisplay(size));
            else if (size < gb)
                result = String.Format("{0} {1} ({2} Bytes)",
                    ConvertToOneDigit(size, mb), MB, ConvertBytesDisplay(size));
            else if (size < tb)
                result = String.Format("{0} {1} ({2} Bytes)",
                    ConvertToOneDigit(size, gb), GB, ConvertBytesDisplay(size));
            else
                result = String.Format("{0} {1} ({2} Bytes)",
                    ConvertToOneDigit(size, tb), TB, ConvertBytesDisplay(size));

            return result;
        }
        static String ConvertBytesDisplay(Int64 size)
        {
            return size.ToString("###,###,###,###,###", CultureInfo.CurrentCulture);
        }
        static String ConvertToOneDigit(Int64 size, Int64 quan)
        {
            Double quotient = (Double)size / (Double)quan;
            String result = quotient.ToString("0.#", CultureInfo.CurrentCulture);
            return result;
        }


        /// <summary>
        /// Returns the encountered substring between both given characters
        /// </summary>
        public static String GetBetween(this String text, Char start_char, Char end_char, Int32 offset = 0)
        {
            while (text[offset] != start_char)
                offset++;
            offset++;
            Int32 length = 0;
            while (text[offset + length] != end_char)
                length++;
            return text.Substring(offset, length);
        }


        /*
        public static System.Drawing.Point operator * (System.Drawing.Point point, float scalar)
        {
            return new Point(point.X * scalar, point.Y * scalar);
        }
        public static System.Drawing.Point operator * (float scalar, System.Drawing.Point point)
        {
            return new Point(point.X * scalar, point.Y * scalar);
        }
        */
        /*
        public static System.Drawing.Size operator * (System.Drawing.Size size, float scalar)
        {
            return new Size(size.Width * scalar, size.Height * scalar);
        }
        public static System.Drawing.Size operator * (float scalar, System.Drawing.Size size)
        {
            return new Size(size.Width * scalar, size.Height * scalar);
        }
        */
    }
}
