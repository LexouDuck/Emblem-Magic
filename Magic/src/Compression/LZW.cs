using System;
using System.IO;

namespace Magic.Compression
{
    /// <summary>
    /// Used for GIF file image data encoding
    /// </summary>
    public class LZW
    {
        static readonly Int32 EOF = -1;
        static readonly Int32 BITS = 12;
        static readonly Int32 HASH_SIZE = 5003; // 80% occupancy

        Int32 ImageWidth, ImageHeight;
        Byte[] Pixels;
        Int32 MinCodeSize;
        Int32 Remaining;
        Int32 Current;

        Int32 BitsPerCode; // number of bits/code
        Int32 MaxBits = BITS; // user settable max # bits/code
        Int32 MaxCode; // maximum code, given n_bits
        Int32 InvalidCode = 1 << BITS; // should NEVER generate this code

        Int32[] HashTable = new Int32[HASH_SIZE];
        Int32[] CodeTable = new Int32[HASH_SIZE];
        Int32 HashSize = HASH_SIZE; // for dynamic table sizing
        Int32 NextCode = 0; // first unused entry
        // block compression parameters - after all codes are used up, and compression rate changes, start over.
        Boolean ClearFlag = false;

        Int32 InitialBits; // Initial number of bits

        Int32 ClearCode;
        Int32 EOICode;

        Int32 CurrentAccumulated = 0;
        Int32 CurrentBitAmount = 0;

        Int32[] BitMasks = new Int32[]
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
        Int32 AccumulatedAmount; // Number of characters so far in this 'packet'
        Byte[] Accumulator = new Byte[256]; // Define the storage for the packet accumulator

        public LZW(Int32 width, Int32 height, Byte[] pixels, Int32 color_depth)
        {
            ImageWidth = width;
            ImageHeight = height;
            this.Pixels = pixels;
            MinCodeSize = Math.Max(2, color_depth);
        }

        public Byte[] Compress()
        {
            Remaining = ImageWidth * ImageHeight; // reset navigation variables
            Current = 0;

            MemoryStream stream = new MemoryStream();
            stream.WriteByte(Convert.ToByte(MinCodeSize)); // write initiating minimal code size byte

            Encode(MinCodeSize + 1, stream); // compress and write the pixel data

            stream.WriteByte(0x00); // write block terminator
            return stream.ToArray();
        }

        

        void Encode(Int32 init_bits, Stream stream)
        {
            Int32 fcode;
            Int32 i /* = 0 */;
            Int32 c;
            Int32 pixel;
            Int32 disp;
            Int32 hsize_reg;
            Int32 hshift;
            
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
        void Output(Int32 code, Stream stream)
        {
            CurrentAccumulated &= BitMasks[CurrentBitAmount];

            if (CurrentBitAmount > 0)
                CurrentAccumulated |= (code << CurrentBitAmount);
            else
                CurrentAccumulated = code;

            CurrentBitAmount += BitsPerCode;

            while (CurrentBitAmount >= 8)
            {
                this.Add((Byte)(CurrentAccumulated & 0xff), stream);
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
                    this.Add((Byte)(CurrentAccumulated & 0xff), stream);
                    CurrentAccumulated >>= 8;
                    CurrentBitAmount -= 8;
                }

                this.Flush(stream);
            }
        }

        /// <summary>
        /// Returns the maximal value for a LZW code given the amount of bits for it
        /// </summary>
        Int32 GetMaxCode(Int32 n_bits)
        {
            return (1 << n_bits) - 1;
        }
        /// <summary>
        /// Returns the next pixel from the image
        /// </summary>
        Int32 GetNextPixel()
        {
            --Remaining;
            if (Remaining < 0)
                return EOF;
            Int32 temp = Current;
            if (temp <= Pixels.GetUpperBound(0))
            {
                Byte pixel = Pixels[Current++];

                return (pixel & 0xFF);
            }
            return (0xFF);
        }

        /// <summary>
        /// Add a character to the end of the current packet, and if it is 254 characters, flush the packet onto the stream.
        /// </summary>
        void Add(Byte c, Stream stream)
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
        void ResetCodeTable(Int32 hsize)
        {
            for (Int32 i = 0; i < hsize; ++i)
            {
                HashTable[i] = -1;
            }
        }
    }
}