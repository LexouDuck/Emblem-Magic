using Magic;
using GBA;
using System;
using System.Collections.Generic;

namespace Compression
{
    /// <summary>
    /// Provides static methods for decompressing and compressing Huffman-encoded text
    /// </summary>
    public static class Huffman
    {
        /// <summary>
        /// Decompresses huffman-encoded text parsing through the huffman tree
        /// </summary>
        public static Byte[] Decompress(Pointer pointer, Pointer huffman_tree, Pointer root_node)
        {
            Byte[] result = new Byte[0x1000];
            Int32 parse = 0;
            Byte current = Core.ReadByte(pointer + parse); parse++;
            Pointer node = root_node;
            Int32 bit = 0;
            Int32 i = 0;
            while (i < 0x1000)
            {
                Int16 Lshort = Util.BytesToInt16(Core.ReadData(node, 2), true); node = node + 2;
                Int16 Rshort = Util.BytesToInt16(Core.ReadData(node, 2), true); node = node + 2;
                if (Rshort < 0)
                {   // reached leaf
                    node = root_node;
                    result[i++] = (Byte)Lshort;
                    if ((Lshort & 0xFF00) != 0)
                    { // two characters
                        if (i != 0x1000)
                        {
                            result[i++] = (Byte)(Lshort >> 8);
                        }
                    }
                    else if (Lshort == 0)
                    {
                        break;
                    }
                }
                else
                {
                    if (bit == 8)
                    {
                        bit = 0;
                        current = Core.ReadByte(pointer + parse); parse++;
                    }
                    Int16 offset = ((current & 1) == 0) ? Lshort : Rshort;
                    node = huffman_tree + offset * 4;
                    current >>= 1;
                    bit++;
                }
            }
            Byte[] output = new Byte[i];
            Array.Copy(result, output, i);
            return output;
        }
        /// <summary>
        /// Compresses the given byte array of ASCII text according to the current ROM's Huffman tree
        /// </summary>
        public static Byte[] Compress(Byte[] text, Pointer huffman_tree, Pointer root_node)
        {
            Byte[] result = new Byte[text.Length];
            return result;
        }
    }
}
