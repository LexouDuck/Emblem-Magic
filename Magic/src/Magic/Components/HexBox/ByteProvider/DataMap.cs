using System;
using System.Collections;

namespace Magic.Components
{
    internal class DataMap : ICollection, IEnumerable
    {
        readonly Object _syncRoot = new Object();
        internal Int32 _count;
        internal DataBlock _firstBlock;
        internal Int32 _version;

        public DataMap()
        {
        }

        public DataMap(IEnumerable collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            foreach (DataBlock item in collection)
            {
                this.AddLast(item);
            }
        }

        public DataBlock FirstBlock
        {
            get
            {
                return this._firstBlock;
            }
        }

        public void AddAfter(DataBlock block, DataBlock newBlock)
        {
            this.AddAfterInternal(block, newBlock);
        }

        public void AddBefore(DataBlock block, DataBlock newBlock)
        {
            this.AddBeforeInternal(block, newBlock);
        }

        public void AddFirst(DataBlock block)
        {
            if (this._firstBlock == null)
            {
                this.AddBlockToEmptyMap(block);
            }
            else
            {
                this.AddBeforeInternal(this._firstBlock, block);
            }
        }

        public void AddLast(DataBlock block)
        {
            if (this._firstBlock == null)
            {
                this.AddBlockToEmptyMap(block);
            }
            else
            {
                this.AddAfterInternal(this.GetLastBlock(), block);
            }
        }

        public void Remove(DataBlock block)
        {
            this.RemoveInternal(block);
        }

        public void RemoveFirst()
        {
            if (this._firstBlock == null)
            {
                throw new InvalidOperationException("The collection is empty.");
            }
            this.RemoveInternal(this._firstBlock);
        }

        public void RemoveLast()
        {
            if (this._firstBlock == null)
            {
                throw new InvalidOperationException("The collection is empty.");
            }
            this.RemoveInternal(this.GetLastBlock());
		}

		public DataBlock Replace(DataBlock block, DataBlock newBlock)
		{
            this.AddAfterInternal(block, newBlock);
            this.RemoveInternal(block);
			return newBlock;
		}

        public void Clear()
        {
            DataBlock block = this.FirstBlock;
            while (block != null)
            {
                DataBlock nextBlock = block.NextBlock;
                this.InvalidateBlock(block);
                block = nextBlock;
            }
            this._firstBlock = null;
            this._count = 0;
            this._version++;
        }

        void AddAfterInternal(DataBlock block, DataBlock newBlock)
        {
            newBlock._previousBlock = block;
            newBlock._nextBlock = block._nextBlock;
            newBlock._map = this;

            if (block._nextBlock != null)
            {
                block._nextBlock._previousBlock = newBlock;
            }
            block._nextBlock = newBlock;

            this._version++;
            this._count++;
        }

        void AddBeforeInternal(DataBlock block, DataBlock newBlock)
        {
            newBlock._nextBlock = block;
            newBlock._previousBlock = block._previousBlock;
            newBlock._map = this;

            if (block._previousBlock != null)
            {
                block._previousBlock._nextBlock = newBlock;
            }
            block._previousBlock = newBlock;

            if (this._firstBlock == block)
            {
                this._firstBlock = newBlock;
            }
            this._version++;
            this._count++;
        }

        void RemoveInternal(DataBlock block)
        {
            DataBlock previousBlock = block._previousBlock;
            DataBlock nextBlock = block._nextBlock;

            if (previousBlock != null)
            {
                previousBlock._nextBlock = nextBlock;
            }

            if (nextBlock != null)
            {
                nextBlock._previousBlock = previousBlock;
            }

            if (this._firstBlock == block)
            {
                this._firstBlock = nextBlock;
            }

            this.InvalidateBlock(block);

            this._count--;
            this._version++;
        }

        DataBlock GetLastBlock()
        {
            DataBlock lastBlock = null;
            for (DataBlock block = this.FirstBlock; block != null; block = block.NextBlock)
            {
                lastBlock = block;
            }
            return lastBlock;
        }

        void InvalidateBlock(DataBlock block)
        {
            block._map = null;
            block._nextBlock = null;
            block._previousBlock = null;
        }

        void AddBlockToEmptyMap(DataBlock block)
        {
            block._map = this;
            block._nextBlock = null;
            block._previousBlock = null;

            this._firstBlock = block;
            this._version++;
            this._count++;
        }

        #region ICollection Members
        public void CopyTo(Array array, Int32 index)
        {
            DataBlock[] blockArray = array as DataBlock[];
            for (DataBlock block = this.FirstBlock; block != null; block = block.NextBlock)
            {
                blockArray[index++] = block;
            }
        }

        public Int32 Count
        {
            get
            {
                return this._count;
            }
        }

        public Boolean IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public Object SyncRoot
        {
            get
            {
                return this._syncRoot;
            }
        }
        #endregion

        #region IEnumerable Members
        public IEnumerator GetEnumerator()
        {
            return new Enumerator(this);
        }
        #endregion

        #region Enumerator Nested Type
        internal class Enumerator : IEnumerator, IDisposable
        {
            DataMap _map;
            DataBlock _current;
            Int32 _index;
            Int32 _version;

            internal Enumerator(DataMap map)
            {
                this._map = map;
                this._version = map._version;
                this._current = null;
                this._index = -1;
            }

            Object IEnumerator.Current
            {
                get
                {
                    if (this._index < 0 || this._index > this._map.Count)
                    {
                        throw new InvalidOperationException("Enumerator is positioned before the first element or after the last element of the collection.");
                    }
                    return this._current;
                }
            }

            public Boolean MoveNext()
            {
                if (this._version != this._map._version)
                {
                    throw new InvalidOperationException("Collection was modified after the enumerator was instantiated.");
                }

                if (this._index >= this._map.Count)
                {
                    return false;
                }

                if (++this._index == 0)
                {
                    this._current = this._map.FirstBlock;
                }
                else
                {
                    this._current = this._current.NextBlock;
                }

                return (this._index < this._map.Count);
            }

            void IEnumerator.Reset()
            {
                if (this._version != this._map._version)
                {
                    throw new InvalidOperationException("Collection was modified after the enumerator was instantiated.");
                }

                this._index = -1;
                this._current = null;
            }

            public void Dispose()
            {
            }
        }
        #endregion
    }
}
