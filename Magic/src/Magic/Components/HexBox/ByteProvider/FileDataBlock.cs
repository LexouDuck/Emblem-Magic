using System;

namespace Magic.Components
{
    internal sealed class FileDataBlock : DataBlock
    {
        Int64 _length;
        Int64 _fileOffset;

        public FileDataBlock(Int64 fileOffset, Int64 length)
        {
            this._fileOffset = fileOffset;
            this._length = length;
        }

        public Int64 FileOffset
        {
            get
            {
                return this._fileOffset;
            }
        }

        public override Int64 Length
        {
            get
            {
                return this._length;
            }
        }

        public void SetFileOffset(Int64 value)
        {
            this._fileOffset = value;
        }

        public void RemoveBytesFromEnd(Int64 count)
        {
            if (count > this._length)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            this._length -= count;
        }

        public void RemoveBytesFromStart(Int64 count)
        {
            if (count > this._length)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            this._fileOffset += count;
            this._length -= count;
        }

        public override void RemoveBytes(Int64 position, Int64 count)
        {
            if (position > this._length)
            {
                throw new ArgumentOutOfRangeException("position");
            }

            if (position + count > this._length)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            Int64 prefixLength = position;
            Int64 prefixFileOffset = this._fileOffset;

            Int64 suffixLength = this._length - count - prefixLength;
            Int64 suffixFileOffset = this._fileOffset + position + count;

            if (prefixLength > 0 && suffixLength > 0)
            {
                this._fileOffset = prefixFileOffset;
                this._length = prefixLength;
                this._map.AddAfter(this, new FileDataBlock(suffixFileOffset, suffixLength));
                return;
            }

            if (prefixLength > 0)
            {
                this._fileOffset = prefixFileOffset;
                this._length = prefixLength;
            }
            else
            {
                this._fileOffset = suffixFileOffset;
                this._length = suffixLength;
            }
        }
    }
}
