using System;

namespace Magic.Components
{
    internal abstract class DataBlock
    {
        internal DataMap _map;
        internal DataBlock _nextBlock;
        internal DataBlock _previousBlock;

        public abstract Int64 Length
        {
            get;
        }

        public DataMap Map
        {
            get
            {
                return _map;
            }
        }

        public DataBlock NextBlock
        {
            get
            {
                return _nextBlock;
            }
        }

        public DataBlock PreviousBlock
        {
            get
            {
                return _previousBlock;
            }
        }

        public abstract void RemoveBytes(Int64 position, Int64 count);
    }
}
