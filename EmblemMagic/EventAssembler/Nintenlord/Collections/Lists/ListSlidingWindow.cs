using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Collections.Lists
{
    public sealed class ListSlidingWindow<T> : IList<T>
    {
        readonly IList<T> items;        
        readonly int windowSize;

        int start;
        int end;

        public ListSlidingWindow(int windowSize, IList<T> listToWrap)
        {
            items = listToWrap;
            this.windowSize = windowSize;
            start = 0;
            end = 0;
        }

        public IList<T> Items
        {
            get { return items; }
        } 

        public int WindowSize
        {
            get { return windowSize; }
        }

        public void Advance()
        {
            end++;
            if (end - start > windowSize)
            {
                start++;
            }
        }

        #region IList<T> Members
        
        public int IndexOf(T item)
        {
            for (int i = start; i < end; i++)
            {
                if (EqualityComparer<T>.Default.Equals(items[i], item))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public T this[int index]
        {
            get
            {
                if (index >= this.Count)
                {
                    throw new IndexOutOfRangeException();
                }
                return items[start + index];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = start; i < end; i++)
            {
                array[arrayIndex] = items[i];
                arrayIndex++;
            }
        }

        public int Count
        {
            get { return end - start; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return new SublistEnumerator<T>(items, start, end - start);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
