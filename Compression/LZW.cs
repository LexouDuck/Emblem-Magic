using System;
using System.IO;

namespace EmblemMagic.Compression
{
    /// <summary>
    /// Used for GIF file image data encoding
    /// </summary>
    public class LZW
    {
        static readonly int EOF = -1;
        static readonly int BITS = 12;
        static readonly int HASH_SIZE = 5003; // 80% occupancy

        int ImageWidth, ImageHeight;
        byte[] Pixels;
        int MinCodeSize;
        int Remaining;
        int Current;

        int BitsPerCode; // number of bits/code
        int MaxBits = BITS; // user settable max # bits/code
        int MaxCode; // maximum code, given n_bits
        int InvalidCode = 1 << BITS; // should NEVER generate this code

        int[] HashTable = new int[HASH_SIZE];
        int[] CodeTable = new int[HASH_SIZE];
        int HashSize = HASH_SIZE; // for dynamic table sizing
        int NextCode = 0; // first unused entry
        // block compression parameters - after all codes are used up, and compression rate changes, start over.
        bool ClearFlag = false;

        int InitialBits; // Initial number of bits

        int ClearCode;
        int EOICode;
        
        int CurrentAccumulated = 0;
        int CurrentBitAmount = 0;

        int[] BitMasks = new int[]
        {
            0x0000,
            0x0001,
            0x0003,
            0x0007,
            0x000F,
            0x001F,
            0x003F,
            0x007F,
            0x00FF,
            0x01FF,
            0x03FF,
            0x07FF,
            0x0FFF,
            0x1FFF,
            0x3FFF,
            0x7FFF,
            0xFFFF
        };
        int AccumulatedAmount; // Number of characters so far in this 'packet'
        byte[] Accumulator = new byte[256]; // Define the storage for the packet accumulator

        public LZW(int width, int height, byte[] pixels, int color_depth)
        {
            ImageWidth = width;
            ImageHeight = height;
            this.Pixels = pixels;
            MinCodeSize = Math.Max(2, color_depth);
        }

        public byte[] Compress()
        {
            Remaining = ImageWidth * ImageHeight; // reset navigation variables
            Current = 0;

            MemoryStream stream = new MemoryStream();
            stream.WriteByte(Convert.ToByte(MinCodeSize)); // write initiating minimal code size byte

            Encode(MinCodeSize + 1, stream); // compress and write the pixel data

            stream.WriteByte(0x00); // write block terminator
            return stream.ToArray();
        }

        

        void Encode(int init_bits, Stream stream)
        {
            int fcode;
            int i /* = 0 */;
            int c;
            int pixel;
            int disp;
            int hsize_reg;
            int hshift;
            
            InitialBits = init_bits;

            // Set up the necessary values
            ClearFlag = false;
            BitsPerCode = InitialBits;
            MaxCode = GetMaxCode(BitsPerCode);

            ClearCode = 1 << (init_bits - 1);
            EOICode = ClearCode + 1;
            NextCode = ClearCode + 2;

            AccumulatedAmount = 0; // clear packet

            pixel = GetNextPixel();

            hshift = 0;
            for (fcode = HashSize; fcode < 65536; fcode *= 2)
            {
                ++hshift;
            }
            hshift = 8 - hshift; // set hash code range bound
            hsize_reg = HashSize;
            this.ResetCodeTable(hsize_reg); // clear hash table

            this.Output(ClearCode, stream);

            OuterLoop: while ((c = GetNextPixel()) != EOF)
            {
                fcode = (c << MaxBits) + pixel;
                i = (c << hshift) ^ pixel; // xor hashing

                if (HashTable[i] == fcode)
                {
                    pixel = CodeTable[i];
                    continue;
                }
                else if (HashTable[i] >= 0) // non-empty slot
                {
                    disp = hsize_reg - i; // secondary hash (after G. Knott)
                    if (i == 0) disp = 1;
                    do
                    {
                        if ((i -= disp) < 0)
                        {
                            i += hsize_reg;
                        }
                        if (HashTable[i] == fcode)
                        {
                            pixel = CodeTable[i];
                            goto OuterLoop;
                        }
                    } while (HashTable[i] >= 0);
                }
                this.Output(pixel, stream);
                pixel = c;
                if (NextCode < InvalidCode)
                {
                    CodeTable[i] = NextCode++; // code -> hashtable
                    HashTable[i] = fcode;
                }
                else this.ClearTable(stream);
            }
            // Put out the final code.
            this.Output(pixel, stream);
            this.Output(EOICode, stream);
        }

        /// <summary>
        /// Output the given code.
        /// Maintain a BITS character long buffer (so that 8 codes will fit in it exactly).
        /// Use the VAX insv instruction to insert each code in turn. When the buffer fills up empty it and start over.
        /// </summary>
        /// <param name="code">A n_bits-bit integer.  If == -1, then EOF.  This assumes that (n_bits) lteq (wordsize - 1).</param>
        void Output(int code, Stream stream)
        {
            CurrentAccumulated &= BitMasks[CurrentBitAmount];

            if (CurrentBitAmount > 0)
                CurrentAccumulated |= (code << CurrentBitAmount);
            else
                CurrentAccumulated = code;

            CurrentBitAmount += BitsPerCode;

            while (CurrentBitAmount >= 8)
            {
                this.Add((byte)(CurrentAccumulated & 0xff), stream);
                CurrentAccumulated >>= 8;
                CurrentBitAmount -= 8;
            }

            // If the next entry is going to be too big for the code size,
            // then increase it, if possible.
            if (NextCode > MaxCode || ClearFlag)
            {
                if (ClearFlag)
                {
                    MaxCode = GetMaxCode(BitsPerCode = InitialBits);
                    ClearFlag = false;
                }
                else
                {
                    ++BitsPerCode;
                    if (BitsPerCode == MaxBits)
                        MaxCode = InvalidCode;
                    else
                        MaxCode = GetMaxCode(BitsPerCode);
                }
            }

            if (code == EOICode)
            {
                // At EOF, write the rest of the buffer.
                while (CurrentBitAmount > 0)
                {
                    this.Add((byte)(CurrentAccumulated & 0xff), stream);
                    CurrentAccumulated >>= 8;
                    CurrentBitAmount -= 8;
                }

                this.Flush(stream);
            }
        }

        /// <summary>
        /// Returns the maximal value for a LZW code given the amount of bits for it
        /// </summary>
        int GetMaxCode(int n_bits)
        {
            return (1 << n_bits) - 1;
        }
        /// <summary>
        /// Returns the next pixel from the image
        /// </summary>
        int GetNextPixel()
        {
            --Remaining;
            if (Remaining < 0)
                return EOF;
            int temp = Current;
            if (temp <= Pixels.GetUpperBound(0))
            {
                byte pixel = Pixels[Current++];

                return (pixel & 0xFF);
            }
            return (0xFF);
        }

        /// <summary>
        /// Add a character to the end of the current packet, and if it is 254 characters, flush the packet onto the stream.
        /// </summary>
        void Add(byte c, Stream stream)
        {
            Accumulator[AccumulatedAmount++] = c;
            if (AccumulatedAmount >= 254)
                this.Flush(stream);
        }
        /// <summary>
        /// Flush the packet onto the stream, and reset the accumulator
        /// </summary>
        void Flush(Stream stream)
        {
            if (AccumulatedAmount > 0)
            {
                stream.WriteByte(Convert.ToByte(AccumulatedAmount));
                stream.Write(Accumulator, 0, AccumulatedAmount);
                AccumulatedAmount = 0;
            }
        }
        /// <summary>
        /// Clears the LZW code table
        /// </summary>
        void ClearTable(Stream stream)
        {
            ResetCodeTable(HashSize);
            NextCode = ClearCode + 2;
            ClearFlag = true;

            Output(ClearCode, stream);
        }
        void ResetCodeTable(int hsize)
        {
            for (int i = 0; i < hsize; ++i)
            {
                HashTable[i] = -1;
            }
        }
    }
}