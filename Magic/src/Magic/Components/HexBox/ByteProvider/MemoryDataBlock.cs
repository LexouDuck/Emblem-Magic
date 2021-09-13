using System;

namespace Magic.Components
{
    internal sealed class MemoryDataBlock : DataBlock
    {
        Byte[] _data;

        public MemoryDataBlock(Byte data)
        {
            this._data = new Byte[] { data };
        }

        public MemoryDataBlock(Byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            this._data = (Byte[])data.Clone();
        }

        public override Int64 Length
        {
            get
            {
                return this._data.LongLength;
            }
        }

        public Byte[] Data
        {
            get
            {
                return this._data;
            }
        }

        public void AddByteToEnd(Byte value)
        {
            Byte[] newData = new Byte[this._data.LongLength + 1];
            this._data.CopyTo(newData, 0);
            newData[newData.LongLength - 1] = value;
            this._data = newData;
        }

        public void AddByteToStart(Byte value)
        {
            Byte[] newData = new Byte[this._data.LongLength + 1];
            newData[0] = value;
            this._data.CopyTo(newData, 1);
            this._data = newData;
        }

        public void InsertBytes(Int64 position, Byte[] data)
        {
            Byte[] newData = new Byte[this._data.LongLength + data.LongLength];
            if (position > 0)
            {
                Array.Copy(this._data, 0, newData, 0, position);
            }
            Array.Copy(data, 0, newData, position, data.LongLength);
            if (position < this._data.LongLength)
            {
                Array.Copy(this._data, position, newData, position + data.LongLength, this._data.LongLength - position);
            }
            this._data = newData;
        }

        public override void RemoveBytes(Int64 position, Int64 count)
        {
            Byte[] newData = new Byte[this._data.LongLength - count];

            if (position > 0)
            {
                Array.Copy(this._data, 0, newData, 0, position);
            }
            if (position + count < this._data.LongLength)
            {
                Array.Copy(this._data, position + count, newData, position, newData.LongLength - position);
            }

            this._data = newData;
        }
    }
}
