using System.Text;
using System.IO;

namespace Nintenlord.IO.Bits
{
    public class BitReader : BinaryReader
    {
        byte buffer;
        int index = 8;

        public BitReader(Stream stream)
            : base(stream) { }

        public BitReader(Stream stream, Encoding encoding)
            : base(stream, encoding) { }

        /// <summary>
        /// Reads one bit from stream.
        /// </summary>
        /// <returns>True of read bit was 1, else false</returns>
        public bool ReadBit()
        {
            if (index > 7)
            {
                buffer = this.ReadByte();
                index = index % 8;
            }

            bool result = (buffer & (1 << index)) != 0;
            //bool result = (buffer & (0x80 >> index)) != 0;
            index++;
            return result;
        }

        public bool IsAtEnd
        {
            get { return this.BaseStream.Position >= this.BaseStream.Length && index > 7; }
        }
    }
}
