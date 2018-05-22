using GBA;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace EmblemMagic
{
    static class Util
    {
        /// <summary>
        /// Returns a bool for the bit at the given index (right-to-left)
        /// </summary>
        public static bool GetBit(this byte data, int index)
        {
            if (index < 0 || index >= 8)
                throw new Exception("bit index given is invalid.");

            byte mask = (byte)(1 << index);

            return (((data & mask) >> index) == 1) ? true : false;
        }
        /// <summary>
        /// Returns the given byte with the one bit at 'index' changed to 'set'.
        /// </summary>
        public static Byte SetBit(this byte data, int index, bool set)
        {
            byte mask = (byte)(1 << index);

            if (set) data |= mask;
            else     data &= (byte)~mask;

            return data;
        }
        
        /// <summary>
        /// Returns a bitshifted number from the given area to bitmask (right-to-left)
        /// </summary>
        public static Byte GetBits(byte data, int index, int length)
        {
            if (index < 0 || index >= 8 || length <= 0 || index + length > 8)
                throw new Exception("bitmask area goes outside the byte. index:" + index + " length: " + length);

            byte mask = (byte)(((0x1 << length) - 1) << index);

            return (byte)((data & mask) >> index);
        }
        /// <summary>
        /// Returns the given byte with the bits from 'index to 'length' replaced by a bitshifted 'set'.
        /// </summary>
        public static Byte SetBits(byte data, byte set, int index, int length)
        {
            if (index < 0 || index >= 8 || length <= 0 || index + length > 8)
                throw new Exception("bitmask area goes outside the byte.");

            byte mask = (byte)(((0x1 << length) - 1) << index);
            byte result = (byte)(data & ~mask);
            result &= (byte)(set << index);
            return result;
        }

        /// <summary>
        /// Takes a char (0, 1, 2, ..., A, B, C etc) and returns the corresponding byte
        /// </summary>
        public static Byte DigitToByte(char digit)
        {
            if (digit >= '0' && digit <= '9')
            {
                return (byte)(digit - '0');
            }
            else if (digit >= 'a' && digit <= 'f')
            {
                return (byte)(digit + 10 - 'a');
            }
            else if (digit >= 'A' && digit <= 'F')
            {
                return (byte)(digit + 10 - 'A');
            }
            throw new Exception("Invalid hex digit: " + digit);
        }
        /// <summary>
        /// Takes a 0-15 number and returns a string char of the hex digit that number would be
        /// </summary>
        public static Char ByteToDigit(byte number)
        {
            if (number >= 16) throw new Exception("given number is more than 1 hex digit");
            if (number <= 9)
            {
                return (char)(number + '0');
            }
            else
            {
                return (char)(number - 10 + 'A');
            }
        }

        /// <summary>
        /// Takes in a byte and returns a 2-length string of that byte as hex.
        /// </summary>
        public static String ByteToHex(byte data)
        {
            string result = "";
            int HI_nibble = data >> 4;
            int LO_nibble = data & 0x0F;

            if (HI_nibble <= 9) { result += (char)(HI_nibble + '0'); }
            else { result += (char)(HI_nibble - 10 + 'A'); }
            // HI goes first
            if (LO_nibble <= 9) { result += (char)(LO_nibble + '0'); }
            else { result += (char)(LO_nibble - 10 + 'A'); }

            return result;
        }
        /// <summary>
        /// Takes a 2-length string and returns the corresponding byte
        /// </summary>
        public static Byte HexToByte(String oneByte)
        {
            if (oneByte.Length != 2) throw new Exception("given string has a length different than 2");

            byte HI_nibble = DigitToByte(oneByte[0]);
            byte LO_nibble = DigitToByte(oneByte[1]);
            LO_nibble += (byte)(HI_nibble << 4);
            return LO_nibble;
        }
        /// <summary>
        /// Takes a byte array and returns a simple hex string of the given data
        /// </summary>
        public static String BytesToHex(byte[] data)
        {
            string result = "";
            for (int i = 0; i < data.Length; i++)
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
            byte[] result = new byte[hex.Length / 2];
            string buffer;
            for (int i = 0; i < result.Length; i++)
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
        public static String BytesToSpacedHex(byte[] data)
        {
            string acc = "";
            foreach (byte hex in data)
            {
                acc += ByteToHex(hex) + ' ';
            }
            return acc;
        }
        /// <summary>
        /// Takes in spaced hex, that looks like 2E 00 00 EA 24 FF AE 51 69 9A A2 21 3D 84 82 0A
        /// </summary>
        public static byte[] SpacedHexToBytes(string spaced_hex)
        {
            string[] hex = spaced_hex.Trim().Split(new char[] { ' ' });//nod to cam

            byte[] data = new byte[hex.Length];
            for (int i = 0; i < hex.Length; i++)
            {
                data[i] = HexToByte(hex[i]);
            }
            return data;
        }
        
        /// <summary>
        /// Returns an unsigned 16-bit integer from the given 2-length byte array.
        /// </summary>
        public static UInt16 BytesToUInt16(byte[] data, bool littleEndian)
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
        public static byte[] UInt16ToBytes(UInt16 number, bool littleEndian)
        {
            byte[] result = new byte[2];
            byte[] bytes = BitConverter.GetBytes(number);
            if (littleEndian != BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            Array.Copy(bytes, result, 2);
            return result;
        }
        /// <summary>
        /// Returns a signed 32-bit integer from the given 4-length byte array
        /// </summary>
        public static UInt32 BytesToUInt32(byte[] data, bool littleEndian)
        {
            if (data.Length != 4) throw new Exception("given data has invalid length: " + data.Length);
            UInt32 result;
            if (littleEndian)
                result = (uint)(data[0] + (data[1] << 8) + (data[2] << 16) + (data[3] << 24));
            else result = (uint)(data[3] + (data[2] << 8) + (data[1] << 16) + (data[0] << 24));
            return result;
        }
        /// <summary>
        /// Returns a 4-length byte array fro the given  signed 32-bit iitegeer
        /// </summary>
        public static byte[] UInt32ToBytes(UInt32 number, bool littleEndian)
        {
            byte[] result = new byte[4];
            byte[] bytes = BitConverter.GetBytes(number);
            if (littleEndian != BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            Array.Copy(bytes, result, 4);
            return result;
        }

        /// <summary>
        /// Returns a signed 16-bit integer from the given 2-length byte array.
        /// </summary>
        public static Int16 BytesToInt16(byte[] data, bool littleEndian)
        {
            if (data.Length != 2) throw new Exception("given data has invalid length: " + data.Length);
            if (littleEndian != BitConverter.IsLittleEndian)
                Array.Reverse(data);
            return BitConverter.ToInt16(data, 0);
        }
        /// <summary>
        /// Returns a 2-length byte array from the given unsigned 16-bit integer
        /// </summary>
        public static byte[] Int16ToBytes(Int16 number, bool littleEndian)
        {
            byte[] result = new byte[2];
            byte[] bytes = BitConverter.GetBytes(number);
            if (littleEndian != BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            Array.Copy(bytes, result, 2);
            return result;
        }
        /// <summary>
        /// Returns a signed 32-bit integer from the given 4-length byte array
        /// </summary>
        public static Int32 BytesToInt32(byte[] data, bool littleEndian)
        {
            if (data.Length != 4) throw new Exception("given data has invalid length: " + data.Length);
            if (littleEndian != BitConverter.IsLittleEndian)
                Array.Reverse(data);
            return BitConverter.ToInt32(data, 0);
        }
        /// <summary>
        /// Returns a 4-length byte array fro the given signed 32-bit integer
        /// </summary>
        public static byte[] Int32ToBytes(Int32 number, bool littleEndian)
        {
            byte[] result = new byte[4];
            byte[] bytes = BitConverter.GetBytes(number);
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

            uint value = 0;
            for (int i = 0; i < hex.Length; i++)
            {
                value = (value << 4) + DigitToByte(hex[i]);
            }
            return value;
        }
        /// <summary>
        /// Returns a variable-length hexadecimal string of the given int
        /// </summary>
        public static String IntToHex(uint value)
        {
            string result = "";
            for (int i = 0; i < 8; i++)
            {
                if (value == 0 && result.Length > 0) { break; }
                result = ByteToDigit((byte)(value & 0xF)) + result;
                value >>= 4;
            }
            return result;
        }
        
        /// <summary>
        /// Returns a string represting a hex address, like "0x0000FF" - works with 32-bit ints
        /// </summary>
        public static String AddressToString(uint address, int fixedLength = 0)
        {
            string result = UInt32ToHex(address);
            string zeros = "";
            if (fixedLength != 0)
            {
                for (int i = 0; i < fixedLength - result.Length; i++)
                {
                    zeros += "0";
                }
            }
            return "0x" + zeros + result;
        }
        /// <summary>
        /// Returns the GBA.Pointer corresponding to the given string address (removes "0x" if any)
        /// </summary>
        public static Pointer StringToAddress(string address)
        {
            if (address == null) throw new Exception("Address given is null.");
            string result = address.Contains("0x") ? address.Remove(0, 2) : address;
            if (result.Length > 8) throw new Exception("The given offset is longer than Int32 permits.");
            return new Pointer(HexToInt(result));
        }

        /// <summary>
        /// returns a 64-bit signed int from a byte array of any length 
        /// </summary>
        public static Int64 BytesToNumber(byte[] data, bool littleEndian)
        {
            if (data == null) throw new Exception("data given is null.");
            switch (data.Length)
            {
                case 0: return 0;
                case 1: return data[0];
                case 2: return BytesToUInt16(data, littleEndian);
                case 3: return BytesToUInt32(new byte[4] { 0, data[0], data[1], data[2] }, littleEndian);
                case 4: return BytesToUInt32(data, littleEndian);
                default: if (littleEndian != BitConverter.IsLittleEndian) Array.Reverse(data); return BitConverter.ToInt64(data, 0);
            }
        }
        /// <summary>
        /// Returns a byte array of 'length' from the number given.
        /// </summary>
        public static byte[] NumberToBytes(Int64 number, int length, bool signed)
        {
            if (length < 0) throw new Exception("requested byte array length cannot be negative.");
            byte[] result = new byte[length];
            switch (length)
            {
                case 0: return new byte[0];
                case 1: return new byte[1] { (byte)number };
                case 2: return (signed) ?
                    Int16ToBytes((Int16)number, false) :
                    UInt16ToBytes((UInt16)number, false);
                case 3: throw new Exception("a 3-byte number has been requested");
                case 4: return (signed) ?
                    Int32ToBytes((Int32)number, false) :
                    UInt32ToBytes((UInt32)number, false);
                default:
                    byte[] bytes = BitConverter.GetBytes(number);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(bytes);
                    return bytes;
            }
        }

        /// <summary>
        /// Takes a byte array of length 4 or under and returns a signed int from it (signed byte if data.Length == 1 for example)
        /// </summary>
        public static Int32 BytesToSInt(byte[] data, bool littleEndian)
        {
            if (data.Length == 3) data = (littleEndian) ?
                    new byte[4] { 0x00, data[0], data[1], data[2] } :
                    new byte[4] { data[2], data[1], data[0], 0x00 };
            switch (data.Length)
            {
                case 1: return (Int32)(sbyte)data[0];
                case 2: return (Int32)BytesToInt16(data, littleEndian);
                case 4: return (Int32)BytesToInt32(data, littleEndian);
                default: throw new Exception("data given has invalid length.");
            }
        }
        /// <summary>
        /// Takes a byte array of length 4 or under and returns an unsigned int from it
        /// </summary>
        public static UInt32 BytesToUInt(byte[] data, bool littleEndian)
        {
            if (data.Length == 3) data = (littleEndian) ?
                    new byte[4] { 0x00, data[0], data[1], data[2] } :
                    new byte[4] { data[2], data[1], data[0], 0x00 };
            switch (data.Length)
            {
                case 1: return (UInt32)data[0];
                case 2: return (UInt32)BytesToUInt16(data, littleEndian);
                case 4: return (UInt32)BytesToUInt32(data, littleEndian);
                default: throw new Exception("data given has invalid length.");
            }
        }

        public static List<bool> IntToBits(int i, int arrayLength)
        {
            List<bool> bits = new List<bool>();

            int temp = i;

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
        public static int BitsToInt(List<bool> bits)
        {
            int result = 0;

            int index = 0; // TODO this is broken

            foreach (bool b in bits)
            {
                if (b)
                {
                    result |= 1 << (bits.Count - index - 1);
                }

                index++;
            }

            return result;
        }
        public static List<bool> ByteToBits(byte i, int arrayLength)
        {
            List<bool> bits = new List<bool>();

            int temp = i;

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
        public static byte BitsToByte(bool[] bits)
        {
            byte result = 0;

            int index = 8 - bits.Length;

            foreach (bool b in bits)
            {
                if (b)
                {
                    result |= (byte)(1 << (7 - index));
                }

                index++;
            }

            return result;
        }

        /// <summary>
        /// Returns a string of the given byte amount, like "XX MB (XX,XXX,XXX Bytes)"
        /// </summary>
        public static string GetDisplayBytes(long size)
        {
            const long multi = 1024;
            long kb = multi;
            long mb = kb * multi;
            long gb = mb * multi;
            long tb = gb * multi;

            const string BYTES = "Bytes";
            const string KB = "KB";
            const string MB = "MB";
            const string GB = "GB";
            const string TB = "TB";

            string result;
            if (size < kb)
                result = string.Format("{0} {1}", size, BYTES);
            else if (size < mb)
                result = string.Format("{0} {1} ({2} Bytes)",
                    ConvertToOneDigit(size, kb), KB, ConvertBytesDisplay(size));
            else if (size < gb)
                result = string.Format("{0} {1} ({2} Bytes)",
                    ConvertToOneDigit(size, mb), MB, ConvertBytesDisplay(size));
            else if (size < tb)
                result = string.Format("{0} {1} ({2} Bytes)",
                    ConvertToOneDigit(size, gb), GB, ConvertBytesDisplay(size));
            else
                result = string.Format("{0} {1} ({2} Bytes)",
                    ConvertToOneDigit(size, tb), TB, ConvertBytesDisplay(size));

            return result;
        }
        static string ConvertBytesDisplay(long size)
        {
            return size.ToString("###,###,###,###,###", CultureInfo.CurrentCulture);
        }
        static string ConvertToOneDigit(long size, long quan)
        {
            double quotient = (double)size / (double)quan;
            string result = quotient.ToString("0.#", CultureInfo.CurrentCulture);
            return result;
        }


        /// <summary>
        /// Returns the encountered substring between both given characters
        /// </summary>
        public static string GetBetween(this string text, char start_char, char end_char, int offset = 0)
        {
            while (text[offset] != start_char)
                offset++;
            offset++;
            int length = 0;
            while (text[offset + length] != end_char)
                length++;
            return text.Substring(offset, length);
        }
    }
}
