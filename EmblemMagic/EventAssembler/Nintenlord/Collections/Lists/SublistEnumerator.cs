using System;
using System.Collections.Generic;

namespace Nintenlord.Collections.Lists
{
    public sealed class SublistEnumerator<T> : IEnumerator<T>
    {
        IList<T> list;
        int startIndex;
        int index;
        int endIndex;

        public SublistEnumerator(IList<T> list, int startIndex, int length)
        {
            this.list = list;
            this.startIndex = startIndex;
            this.index = startIndex - 1;
            this.endIndex = startIndex + length;
        }

        #region IEnumerator<T> Members

        public T Current
        {
            get 
            {
                if (index < startIndex || index >= endIndex)
                {
                    throw new InvalidOperationException();
                }
                return list[index]; 
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            list = null;
            index = 0;
            startIndex = 0;
            endIndex = 0;
        }

        #endregion

        #region IEnumerator Members

        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            return (++index) < endIndex;
        }

        public void Reset()
        {
            index = startIndex;
        }

        #endregion
    }
}
