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
                return this._map;
            }
        }

        public DataBlock NextBlock
        {
            get
            {
                return this._nextBlock;
            }
        }

        public DataBlock PreviousBlock
        {
            get
            {
                return this._previousBlock;
            }
        }

        public abstract void RemoveBytes(Int64 position, Int64 count);
    }
}
