using Magic;
using GBA;
using System;
using System.Collections.Generic;

namespace Compression
{
    public static class LZ77
    {
        public const Int32 BLOCK_SIZE = 0x8;
        public const Int32 BUFFER_SIZE = 0x12;
        public const Int32 WINDOW_SIZE = 0x1000;



        /// <summary>
        /// Returns the length of the compressed data once decomrpessed, or -1 if can't be decompressed
        /// </summary>
        public static Int32 GetLength(Pointer data)
        {
            if (Core.ReadByte(data) != 0x10) return -1;

            UInt32 decomp_length = Core.ReadData(data, 4).GetUInt32(0, true) & 0x00FFFFFF;
            Int32 decomp_parse = 0;
            Pointer parse = data + 4;

            while (decomp_parse < decomp_length)
            {
                Byte isCompressed = Core.ReadByte(parse);
                parse += 1;
                for (Int32 i = 0; i < BLOCK_SIZE && decomp_parse < decomp_length; i++)
                {
                    if ((isCompressed & 0x80) != 0)
                    {
                        Byte AmountToCopy = (Byte)(((Core.ReadByte(parse) >> 4) & 0xF) + 3);
                        UInt16 CopyPosition = (UInt16)((((Core.ReadByte(parse) & 0xF) << 8) + Core.ReadByte(parse)) + 1);

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
        public static Byte[] Decompress(Pointer address)
        {
            Pointer pos = address;
            if (address == new Pointer()) return null;

            else if (Core.ReadByte(pos) != 0x10)
                throw new Exception("LZ77 compressed data is not valid, it should start with 0x10");
            pos += 1;

            Int32 offset = 0;
            Int32 length = Core.ReadByte(pos) + (Core.ReadByte(pos + 1) << 8) + (Core.ReadByte(pos + 2) << 16);
            pos += 3;

            Byte[] result = new Byte[length];

            Byte isCompressed;
            try
            {
                while (offset < length)
                {
                    isCompressed = Core.ReadByte(pos);
                    pos += 1;
                    // byte of 8 flags for 8 blocks
                    for (Int32 i = 0; i < BLOCK_SIZE; i++)
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
                            Int32 amountToCopy = ((block >> 4) & 0xF) + 3;
                            Int32 displacement = ((block & 0xF) << 8) | ((block >> 8) & 0xFF);

                            Int32 copyPosition = offset - displacement - 1;
                            if (copyPosition > length) throw new Exception();

                            for (Int32 j = 0; j < amountToCopy; j++)
                            {
                                Byte b = result[copyPosition + j];
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
        public static Int32[] Search(Byte[] input, Int32 position, Int32 length)
        {
            List<Int32> results = new List<Int32>();

            if (!(position < length))
                return new Int32[] { -1, 0 };
            if (position < 3 || (length - position) < 3)
                return new Int32[] { 0, 0 };

            for (Int32 i = 1; ((i < WINDOW_SIZE) && (i < position)); i++)
            {
                if (input[position - i - 1] == input[position])
                {
                    results.Add(i + 1);
                }
            }
            if (results.Count == 0)
                return new Int32[] { 0, 0 };

            Int32 amountOfBytes = 0;

            while (amountOfBytes < BUFFER_SIZE)
            {
                amountOfBytes++;
                Boolean searchComplete = false;
                for (Int32 i = results.Count - 1; i >= 0; i--)
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
                        return new Int32[] { 0, 0 };
                    }
                }
                if (searchComplete)
                    break;
            }

            //Length of data is first, then position
            return new Int32[] { amountOfBytes, results[0] };
        }

        /// <summary>
        /// Returns the given byte array, compressed using the LZ77 compression algorithm
        /// </summary>
        public static Byte[] Compress(Byte[] input)
        {
            Int32 length = input.Length;
            Int32 position = 0;

            List<Byte> result = new List<Byte>();
            result.Add((Byte)0x10);

            result.Add((Byte)(0xFF & (length)));
            result.Add((Byte)(0xFF & (length >> 8)));
            result.Add((Byte)(0xFF & (length >> 16)));

            Byte isCompressed = 0;
            Int32[] searchResult = null;
            Byte add = (Byte)0;
            List<Byte> tempList = null;

            while (position < length)
            {
                isCompressed = 0;
                tempList = new List<Byte>();

                for (Int32 i = 0; i < BLOCK_SIZE; i++)
                {
                    searchResult = Search(input, position, length);

                    if (searchResult[0] > 2)
                    {
                        add = (Byte)((((searchResult[0] - 3) & 0xF) << 4) + (((searchResult[1] - 1) >> 8) & 0xF));
                        tempList.Add(add);
                        add = (Byte)((searchResult[1] - 1) & 0xFF);
                        tempList.Add(add);
                        position += searchResult[0];
                        isCompressed |= (Byte)(1 << (BLOCK_SIZE - (i + 1)));
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