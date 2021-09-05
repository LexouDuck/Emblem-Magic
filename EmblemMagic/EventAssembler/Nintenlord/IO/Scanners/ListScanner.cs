using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.IO.Scanners
{
    public sealed class ListScanner<T> : IStoringScanner<T>
    {
        readonly IList<T> list;
        int currentOffset;

        public ListScanner(IList<T> list)
        {
            this.list = list;
        }

        public T this[long offset]
        {
            get { return list[(int)offset]; }
        }

        public bool IsStored(long offset)
        {
            return offset >= 0 && offset < list.Count;
        }

        public bool IsStored(long offset, long length)
        {
            return offset >= 0 && offset + length <= list.Count;
        }

        public bool IsAtEnd
        {
            get { return currentOffset < list.Count; }
        }

        public long Offset
        {
            get
            {
                return currentOffset;
            }
            set
            {
                currentOffset = (int)value;
            }
        }

        public bool CanSeek
        {
            get { return true; }
        }

        public T Current
        {
            get { return list[currentOffset]; }
        }

        public bool MoveNext()
        {
            currentOffset++;
            return currentOffset < list.Count;
        }
    }
}
