using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nintenlord.Utility;

namespace Nintenlord.Collections.DataChange
{
    [Serializable]
    public sealed class IndexOverlay : IIndexOverlay //, ISerializable
    {
        /// <summary>
        /// Foreach (x,l) in indexes: x >= 0 && l > 0 && next(x) > x + l;
        /// </summary>
        SortedDictionary<int, int> indexes;

        public IndexOverlay()
        {
            indexes = new SortedDictionary<int, int>();
        }

        #region IIndexOverlay Members

        public bool ContainsIndexes
        {
            get { return indexes.Count > 0; }
        }

        public int AmountOfIndexes
        {
            get
            {
                return indexes.Values.Sum();
            }
        }

        public int FirstIndex
        {
            get { return indexes.Keys.First(); }
        }

        public int LastIndex
        {
            get { return indexes.Last().Apply((x,y) => x + y); }
        }

        public bool ContainsIndex(int index)
        {
            if (index < 0)
            {
                throw new IndexOutOfRangeException();
            }
            //Works?
            return (from item in indexes 
                    where item.Key + item.Value >= index 
                    select index >= item.Key).FirstOrDefault();
        }

        public void AddIndex(int index)
        {
            if (index < 0)
            {
                throw new IndexOutOfRangeException();
            }
            int left;
            int right;
            FindClosestIndexes(index, out left, out right);
            bool touchesRight = right - 1 == index;
            bool touchesLeft = left != -1 && LastIndexOf(left) == index;

            if (touchesLeft)
            {
                if (touchesRight)
                {
                    indexes[left] = indexes[left] + 1 + indexes[right];
                    indexes.Remove(right);
                }
                else
                {
                    indexes[left] = indexes[left] + 1;
                }
            }
            else if (touchesRight)
            {
                indexes[index] = 1 + indexes[right];
                indexes.Remove(right);
            }
            else if (left == -1 || index > LastIndexOf(left))
            {
                indexes[index] = 1;
            }
        }
        
        public void AddIndexes(int index, int length)
        {
            if (index < 0 || length <= 0)
            {
                throw new IndexOutOfRangeException();
            }

            var touchingIndexes = FindAllIndexes(index, length);

            if (touchingIndexes.Count > 0)
            {
                int newStart = Math.Min(touchingIndexes[0], index);
                int newEnd = Math.Max(
                    LastIndexOf(touchingIndexes[touchingIndexes.Count - 1]), 
                    index + length);

                foreach (var item in touchingIndexes)
                {
                    indexes.Remove(item);
                }

                indexes.Add(newStart, newEnd - newStart);
            }
            else
            {
                indexes.Add(index, length);
            }

            //for (int i = 0; i < length; i++)
            //{
            //    this.AddIndex(index + i);
            //}
        }

        public bool RemoveIndex(int index)
        {
            if (index < 0)
            {
                throw new IndexOutOfRangeException();
            }
            int left;
            int right;
            FindClosestIndexes(index, out left, out right);
            bool touchesRight = right - 1 == index;
            bool touchesLeft = left != -1 && LastIndexOf(left) == index;
            bool result = false;

            if (left != -1)
            {
                if (index == LastIndexOf(left) - 1)
                {
                    if (indexes[left] == 1)
                    {
                        indexes.Remove(left);
                    }
                    else
                    {
                        indexes[left] = indexes[left] - 1;
                    }
                    result = true;
                }
                else if (index < LastIndexOf(left))
                {
                    int lengthToSplit = indexes[left];
                    indexes[left] = left - index;
                    indexes[index + 1] = lengthToSplit - 1 - (left + index);
                    result = true;
                }
            }
            if (right == index)
            {
                if (indexes[right] != 1)
                {
                    indexes[right + 1] = indexes[right] - 1;
                }
                indexes.Remove(right);
                result = true;
            }
            
            return result;
        }

        public bool RemoveIndexes(int index, int length)
        {
            if (index < 0 || length <= 0)
            {
                throw new IndexOutOfRangeException();
            }

            var touchingIndexes = FindAllIndexes(index, length);

            bool result = touchingIndexes.Count == 1;

            if (touchingIndexes.Count > 0)
            {
                if (touchingIndexes[0] >= index)
                {
                    indexes.Remove(touchingIndexes[0]);
                }
                else
                {
                    indexes[touchingIndexes[0]] = index - touchingIndexes[0];
                }

                if (touchingIndexes.Count > 1)
                {
                    for (int i = 1; i < touchingIndexes.Count - 1; i++)
                    {
                        indexes.Remove(touchingIndexes[i]);
                    }

                    int lastIndex = touchingIndexes[touchingIndexes.Count - 1];
                    int oldLength = indexes[lastIndex];
                    indexes.Remove(lastIndex);

                    int toRemove = (length + index - lastIndex);
                    int newLength = oldLength - toRemove;

                    indexes[lastIndex + toRemove] = newLength;
                }
            }

            //for (int i = 0; i < length; i++)
            //{
            //    result = result && this.RemoveIndex(index + i);
            //}
            return result;
        }

        public KeyValuePair<int, int>[] GetIndexAreas()
        {
            return indexes.ToArray();
        }

        public bool ContainsAnyIndex(int index, int length)
        {
            int startLeft, startRight;
            FindClosestIndexes(index, out startLeft, out startRight);
            int endLeft, endRight;
            FindClosestIndexes(index + length, out endLeft, out endRight);

            if (index <= endLeft) // If a new range starts mid-range
            {
                return true;
            }
            else
            {
                return LastIndexOf(startLeft) > index;//If previous laps with range
            }

            //return (LastIndexOf(startLeft))

            //for (int i = 0; i < length; i++)
            //{
            //    if (ContainsIndex(index + i))
            //        return true;
            //}
            //return false;
        }
        
        public bool ContainsAllIndexes(int index, int length)
        {
            int endLeft, endRight;
            FindClosestIndexes(index + length, out endLeft, out endRight);

            return endLeft != -1 && (endLeft <= index && LastIndexOf(endLeft) >= index + length);
        }
        
        public IEnumerator<KeyValuePair<int, int>> GetRanges(int min, int max)
        {
            return indexes.Where(range => range.Key >= min && range.Key + range.Value <= max).GetEnumerator();
        }

        IEnumerator<int> IIndexOverlay.GetEnumerator(int min, int max)
        {
            foreach (var range in indexes)
            {
                int start = Math.Max(min, range.Key);
                int end = Math.Min(max, range.Key + range.Value);
                for (int i = start; i < end; i++)
                {
                    yield return i;
                }
            }
        }

        #endregion
        
        #region IEnumerable<int> Members

        public IEnumerator<int> GetEnumerator()
        {
            return ((IIndexOverlay)this).GetEnumerator(FirstIndex, LastIndex);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region IEnumerable<KeyValuePair<int,int>> Members

        IEnumerator<KeyValuePair<int, int>> IEnumerable<KeyValuePair<int, int>>.GetEnumerator()
        {
            return indexes.GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Finds the closest indexes to index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="left">Largest value smaller than index or -1.</param>
        /// <param name="right">Smallest value larger or equal to index or -1.</param>
        /// <returns>True if both left and right exist, else false.</returns>
        private bool FindClosestIndexes(int index, out int left, out int right)
        {
            right = -1;
            left = -1;
            foreach (var item in indexes.Keys)
            {
                if (item >= index)
                {
                    right = item;
                    break;
                }
                left = item;
            }
            return left != -1 && right != -1;
        }

        private List<int> FindAllIndexes(int start, int length)
        {
            List<int> items = new List<int>();

            foreach (var item in indexes.Keys)
            {
                if (item < start)
                {
                    if (LastIndexOf(item) >= start)
                    {
                        items.Add(item);
                    }
                }
                else
                {
                    if (start + length > item)
                    {
                        break;
                    }
                    items.Add(item);
                }
            }

            return items;
        }

        private int LastIndexOf(int index)
        {
            return indexes[index] + index;
        }

        private bool IsContained(int index, int inIndex)
        {
            return inIndex != -1 
                && inIndex <= index 
                && index < LastIndexOf(inIndex);
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder("{\n");
            foreach (var item in indexes)
            {
                output.AppendFormat("{0}: {1},\n", item.Key, item.Value);
            }
            output.Append("}");
            return output.ToString();
        }

        public bool IsInValidState()
        {
            KeyValuePair<int, int> latest;
            IEnumerator<KeyValuePair<int, int>> enumerator = indexes.GetEnumerator();
            if (!enumerator.MoveNext())//If no values.
            {
                return true;//Empty is a valid state.
            }

            latest = enumerator.Current;
            while (enumerator.MoveNext())
            {
                if (latest.Apply((x,y)=> x+y) >= enumerator.Current.Key //Not touching or overlapping with next
                    || enumerator.Current.Key < 0
                    || enumerator.Current.Value <= 0)
                {
                    return false;
                }
                latest = enumerator.Current;
            }
            return true;
        }
    }
}
