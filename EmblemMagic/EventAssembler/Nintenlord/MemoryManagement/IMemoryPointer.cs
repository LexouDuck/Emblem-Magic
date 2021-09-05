using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.MemoryManagement
{
    public interface IMemoryPointer
    {
        bool IsNull { get; }
        int Offset { get; }
        int Size { get; }
    }

    public sealed class BySizeComparer : IComparer<IMemoryPointer>, IEqualityComparer<IMemoryPointer>
    {
        #region IComparer<GBAPointer> Members

        public int Compare(IMemoryPointer x, IMemoryPointer y)
        {
            return x.Size - y.Size;
        }

        #endregion

        #region IEqualityComparer<IMemoryPointer> Members

        public bool Equals(IMemoryPointer x, IMemoryPointer y)
        {
            return x.Size - y.Size == 0;
        }

        public int GetHashCode(IMemoryPointer obj)
        {
            return obj.Size;
        }

        #endregion
    }

    public sealed class ByOffsetComparer : IComparer<IMemoryPointer>, IEqualityComparer<IMemoryPointer>
    {
        #region IComparer<GBAPointer> Members

        public int Compare(IMemoryPointer x, IMemoryPointer y)
        {
            return x.Offset - y.Offset;
        }

        #endregion

        #region IEqualityComparer<IMemoryPointer> Members

        public bool Equals(IMemoryPointer x, IMemoryPointer y)
        {
            return x.Offset - y.Offset == 0;
        }

        public int GetHashCode(IMemoryPointer obj)
        {
            return obj.Offset;
        }

        #endregion
    }
}
