using System;
using System.Collections.Generic;
using System.Linq;

namespace Nintenlord.Collections.Lists
{
    public sealed class StringWrapper : IList<char>
    {
        string baseString;

        public StringWrapper(string baseString)
        {
            this.baseString = baseString;
        }

        #region IList<char> Members

        public int IndexOf(char item)
        {
            return baseString.IndexOf(item);
        }

        public void Insert(int index, char item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public char this[int index]
        {
            get
            {
                return baseString[index];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region ICollection<char> Members

        public void Add(char item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(char item)
        {
            return baseString.Contains(item);
        }

        public void CopyTo(char[] array, int arrayIndex)
        {
            for (int i = 0; i < baseString.Length; i++)
            {
                array[arrayIndex + i] = baseString[i];
            }
        }

        public int Count
        {
            get { return baseString.Length; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(char item)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IEnumerable<char> Members

        public IEnumerator<char> GetEnumerator()
        {
            return baseString.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return baseString.GetEnumerator();
        }

        #endregion
    }
}
