using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nintenlord.Utility;
using Nintenlord.Utility.Primitives;

namespace Nintenlord.MemoryManagement
{
    [Serializable]
    public struct OffsetSizePair : IEquatable<OffsetSizePair>, IComparable<OffsetSizePair>, IEnumerable<int>
    {
        public int Offset;
        public int Size;

        public OffsetSizePair(int offset, int size)
        {
            this.Offset = offset;
            this.Size = size;
        }

        #region IEquatable<OffsetSizePair> Members

        public bool Equals(OffsetSizePair other)
        {
            return this.Offset == other.Offset &&
                this.Size == other.Size;
        }

        #endregion

        #region IComparable<OffsetSizePair> Members

        public int CompareTo(OffsetSizePair other)
        {
            return this.Offset - other.Offset;
        }

        #endregion

        #region IEnumerable<int> Members

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < Size; i++)
            {
                yield return Offset + i;
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion


        public override bool Equals(object obj)
        {
            return obj is OffsetSizePair && this.Equals((OffsetSizePair)obj);
        }

        public override int GetHashCode()
        {
            return Offset ^ Size;
        }
        
        public override string ToString()
        {
            return string.Format("Offset: ${0} Size: 0x{1}", Offset.ToHexString(""), Size.ToHexString(""));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="memory">Needs to be sorted and non-negative.</param>
        /// <returns></returns>
        public static IEnumerable<OffsetSizePair> EnumerateAsPairs(IEnumerable<int> memory)
        {
            int previous = -2;

            int start = -1;
            foreach (var item in memory)
            {
                if (previous + 1 != item)
                {
                    if (start >= 0)
                    {
                        yield return new OffsetSizePair(start, previous - start + 1);
                    }
                    start = item;
                }
            }
        }


    }
}
