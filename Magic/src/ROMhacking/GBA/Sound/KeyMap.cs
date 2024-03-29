using System;
using System.Collections.Generic;

namespace GBA
{
    public struct KeyMap
    {
        public const Int32 LENGTH = 128;

        public Byte this[Int32 index]
        {
            get
            {
                return this.Data[index];
            }
        }

        public KeyMap(Byte[] data)
        {
            if (data.Length != LENGTH)
                throw new Exception("Data given has invalid length.");

            this.Data = data;
        }

        Byte[] Data;

        public Byte[] ToBytes()
        {
            return this.Data;
        }
    }
}
