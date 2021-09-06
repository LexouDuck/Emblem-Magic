using System;

namespace Magic.Components
{
    internal sealed class FileDataBlock : DataBlock
    {
        Int64 _length;
        Int64 _fileOffset;

        public FileDataBlock(Int64 fileOffset, Int64 length)
        {
            _fileOffset = fileOffset;
            _length = length;
        }

        public Int64 FileOffset
        {
            get
            {
                return _fileOffset;
            }
        }

        public override Int64 Length
        {
            get
            {
                return _length;
            }
        }

        public void SetFileOffset(Int64 value)
        {
            _fileOffset = value;
        }

        public void RemoveBytesFromEnd(Int64 count)
        {
            if (count > _length)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            _length -= count;
        }

        public void RemoveBytesFromStart(Int64 count)
        {
            if (count > _length)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            _fileOffset += count;
            _length -= count;
        }

        public override void RemoveBytes(Int64 position, Int64 count)
        {
            if (position > _length)
            {
                throw new ArgumentOutOfRangeException("position");
            }

            if (position + count > _length)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            Int64 prefixLength = position;
            Int64 prefixFileOffset = _fileOffset;

            Int64 suffixLength = _length - count - prefixLength;
            Int64 suffixFileOffset = _fileOffset + position + count;

            if (prefixLength > 0 && suffixLength > 0)
            {
                _fileOffset = prefixFileOffset;
                _length = prefixLength;
                _map.AddAfter(this, new FileDataBlock(suffixFileOffset, suffixLength));
                return;
            }

            if (prefixLength > 0)
            {
                _fileOffset = prefixFileOffset;
                _length = prefixLength;
            }
            else
            {
                _fileOffset = suffixFileOffset;
                _length = suffixLength;
            }
        }
    }
}
