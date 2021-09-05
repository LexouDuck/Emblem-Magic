using Magic;
using GBA;
using System;
using System.Collections.Generic;

namespace Compression
{
    public static class LZ77
    {
        public const int BLOCK_SIZE = 0x8;
        public const int BUFFER_SIZE = 0x12;
        public const int WINDOW_SIZE = 0x1000;



        /// <summary>
        /// Returns the length of the compressed data once decomrpessed, or -1 if can't be decompressed
        /// </summary>
        public static int GetLength(Pointer data)
        {
            if (Core.ReadByte(data) != 0x10) return -1;

            uint decomp_length = Core.ReadData(data, 4).GetUInt32(0, true) & 0x00FFFFFF;
            int decomp_parse = 0;
            Pointer parse = data + 4;

            while (decomp_parse < decomp_length)
            {
                byte isCompressed = Core.ReadByte(parse);
                parse += 1;
                for (int i = 0; i < BLOCK_SIZE && decomp_parse < decomp_length; i++)
                {
                    if ((isCompressed & 0x80) != 0)
                    {
                        byte AmountToCopy = (byte)(((Core.ReadByte(parse) >> 4) & 0xF) + 3);
                        ushort CopyPosition = (ushort)((((Core.ReadByte(parse) & 0xF) << 8) + Core.ReadByte(parse)) + 1);

                        if (CopyPosition > decomp_parse)
                            return -1;

                        decomp_parse += AmountToCopy;
                        parse += 2;
                    }
                    else
                    {
                        decomp_parse++;
                        parse += 1;
                    }
                    unchecked
                    {
                        isCompressed <<= 1;
                    }
                }
            }
            if (parse % 4 != 0) parse += 4 - parse % 4;
            return parse;
        }

        /// <summary>
        /// Decompress the given byte array holding GBA formatted LZ77 data
        /// </summary>
        public static byte[] Decompress(Pointer address)
        {
            Pointer pos = address;
            if (address == new Pointer()) return null;

            else if (Core.ReadByte(pos) != 0x10)
                throw new Exception("LZ77 compressed data is not valid, it should start with 0x10");
            pos += 1;

            int offset = 0;
            int length = Core.ReadByte(pos) + (Core.ReadByte(pos + 1) << 8) + (Core.ReadByte(pos + 2) << 16);
            pos += 3;

            byte[] result = new byte[length];

            byte isCompressed;
            try
            {
                while (offset < length)
                {
                    isCompressed = Core.ReadByte(pos);
                    pos += 1;
                    // byte of 8 flags for 8 blocks
                    for (int i = 0; i < BLOCK_SIZE; i++)
                    {
                        if ((isCompressed & (0x80 >> i)) == 0)
                        {   // so this block isn't compressed, copy a single byte
                            result[offset++] = Core.ReadByte(pos);
                            pos += 1;
                        }
                        else
                        {
                            UInt16 block = (UInt16)(Core.ReadByte(pos) | (Core.ReadByte(pos + 1) << 8));
                            pos += 2;
                            // load LZ copy block
                            int amountToCopy = ((block >> 4) & 0xF) + 3;
                            int displacement = ((block & 0xF) << 8) | ((block >> 8) & 0xFF);

                            int copyPosition = offset - displacement - 1;
                            if (copyPosition > length) throw new Exception();

                            for (int j = 0; j < amountToCopy; j++)
                            {
                                byte b = result[copyPosition + j];
                                result[offset++] = b;
                            }
                        }

                        if (offset >= length) break;
                    }
                }
            }
            catch
            {
                return result;
            }
            return result;
        }



        /// <summary>
        /// Returns info about the result of an LZ77 compression search given the array to search through,
        /// the position to search from, and the length of array that is being compressed.
        /// </summary>
        public static int[] Search(byte[] input, int position, int length)
        {
            List<int> results = new List<int>();

            if (!(position < length))
                return new int[] { -1, 0 };
            if (position < 3 || (length - position) < 3)
                return new int[] { 0, 0 };

            for (int i = 1; ((i < WINDOW_SIZE) && (i < position)); i++)
            {
                if (input[position - i - 1] == input[position])
                {
                    results.Add(i + 1);
                }
            }
            if (results.Count == 0)
                return new int[] { 0, 0 };

            int amountOfBytes = 0;

            while (amountOfBytes < BUFFER_SIZE)
            {
                amountOfBytes++;
                bool searchComplete = false;
                for (int i = results.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        if (input[position + amountOfBytes] !=
                            input[position + (amountOfBytes % results[i]) - results[i]])
                        {
                            if (results.Count > 1)
                                results.RemoveAt(i);
                            else
                                searchComplete = true;
                        }
                    }
                    catch
                    {
                        return new int[] { 0, 0 };
                    }
                }
                if (searchComplete)
                    break;
            }

            //Length of data is first, then position
            return new int[] { amountOfBytes, results[0] };
        }

        /// <summary>
        /// Returns the given byte array, compressed using the LZ77 compression algorithm
        /// </summary>
        public static byte[] Compress(byte[] input)
        {
            int length = input.Length;
            int position = 0;

            List<Byte> result = new List<Byte>();
            result.Add((byte)0x10);

            result.Add((byte)(0xFF & (length)));
            result.Add((byte)(0xFF & (length >> 8)));
            result.Add((byte)(0xFF & (length >> 16)));

            byte isCompressed = 0;
            int[] searchResult = null;
            Byte add = (byte)0;
            List<Byte> tempList = null;

            while (position < length)
            {
                isCompressed = 0;
                tempList = new List<Byte>();

                for (int i = 0; i < BLOCK_SIZE; i++)
                {
                    searchResult = Search(input, position, length);

                    if (searchResult[0] > 2)
                    {
                        add = (byte)((((searchResult[0] - 3) & 0xF) << 4) + (((searchResult[1] - 1) >> 8) & 0xF));
                        tempList.Add(add);
                        add = (byte)((searchResult[1] - 1) & 0xFF);
                        tempList.Add(add);
                        position += searchResult[0];
                        isCompressed |= (byte)(1 << (BLOCK_SIZE - (i + 1)));
                    }
                    else if (searchResult[0] >= 0)
                    {
                        tempList.Add(input[position++]);
                    }
                    else break;
                }

                result.Add(isCompressed);
                result.AddRange(tempList);
            }

            while (result.Count % 4 != 0)
            {
                result.Add(0);
            }

            return result.ToArray();
        }
    }
}