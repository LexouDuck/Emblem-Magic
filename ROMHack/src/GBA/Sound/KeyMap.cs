using System;
using System.Collections.Generic;

namespace GBA
{
    public struct KeyMap
    {
        public const int LENGTH = 128;

        public byte this[int index]
        {
            get
            {
                return Data[index];
            }
        }

        public KeyMap(byte[] data)
        {
            if (data.Length != LENGTH)
                throw new Exception("Data given has invalid length.");

            Data = data;
        }

        byte[] Data;

        public byte[] ToBytes()
        {
            return Data;
        }
    }
}
