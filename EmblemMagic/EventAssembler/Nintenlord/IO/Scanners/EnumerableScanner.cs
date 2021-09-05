using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.IO.Scanners
{
    public sealed class EnumerableScanner<T> : IScanner<T>
    {
        IEnumerator<T> enumerator;
        int offset; 

        public EnumerableScanner(IEnumerable<T> toEnum)
        {
            enumerator = toEnum.GetEnumerator();
            IsAtEnd = !enumerator.MoveNext();
            offset = 0;
        }

        #region IScanner<T> Members

        public bool IsAtEnd
        {
            get;
            private set;
        }

        public long Offset
        {
            get
            {
                return offset;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public bool MoveNext()
        {
            offset++;
            return (IsAtEnd = !enumerator.MoveNext());
        }

        public T Current
        {
            get
            {
                return enumerator.Current;
            }
        }

        public bool CanSeek
        {
            get { return false; }
        }

        #endregion
    }
}
