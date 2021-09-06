using System;

namespace Magic.Components
{
    internal sealed class MemoryDataBlock : DataBlock
    {
        Byte[] _data;

        public MemoryDataBlock(Byte data)
        {
            _data = new Byte[] { data };
        }

        public MemoryDataBlock(Byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            _data = (Byte[])data.Clone();
        }

        public override Int64 Length
        {
            get
            {
                return _data.LongLength;
            }
        }

        public Byte[] Data
        {
            get
            {
                return _data;
            }
        }

        public void AddByteToEnd(Byte value)
        {
            Byte[] newData = new Byte[_data.LongLength + 1];
            _data.CopyTo(newData, 0);
            newData[newData.LongLength - 1] = value;
            _data = newData;
        }

        public void AddByteToStart(Byte value)
        {
            Byte[] newData = new Byte[_data.LongLength + 1];
            newData[0] = value;
            _data.CopyTo(newData, 1);
            _data = newData;
        }

        public void InsertBytes(Int64 position, Byte[] data)
        {
            Byte[] newData = new Byte[_data.LongLength + data.LongLength];
            if (position > 0)
            {
                Array.Copy(_data, 0, newData, 0, position);
            }
            Array.Copy(data, 0, newData, position, data.LongLength);
            if (position < _data.LongLength)
            {
                Array.Copy(_data, position, newData, position + data.LongLength, _data.LongLength - position);
            }
            _data = newData;
        }

        public override void RemoveBytes(Int64 position, Int64 count)
        {
            Byte[] newData = new Byte[_data.LongLength - count];

            if (position > 0)
            {
                Array.Copy(_data, 0, newData, 0, position);
            }
            if (position + count < _data.LongLength)
            {
                Array.Copy(_data, position + count, newData, position, newData.LongLength - position);
            }

            _data = newData;
        }
    }
}
