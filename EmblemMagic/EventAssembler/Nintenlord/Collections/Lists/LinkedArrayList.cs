using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nintenlord.Utility.Primitives;
using System.Diagnostics.Contracts;

namespace Nintenlord.Collections.Lists
{
    /// <summary>
    /// List based collection allowing fast additions and removals from start and end
    /// </summary>
    /// <typeparam name="T">Type of items in collection</typeparam>
    public sealed class LinkedArrayList<T> : ICollection<T>, IList<T>
    {
        T[] items;
        int count;
        int firstReservedIndex;//The first reserved index
        int firstFreeIndex;//The first free index

        public T First
        {
            get
            {
                Contract.Requires<InvalidOperationException>
                    (count > 0, "Can't take the first of empty collection.");

                return items[firstReservedIndex];
            }
        }
        public T Last
        {
            get
            {
                Contract.Requires<InvalidOperationException>
                    (count > 0, "Can't take the last of empty collection.");

                return firstFreeIndex > 0 ? items[firstFreeIndex - 1] : items[items.Length - 1];
            }
        }

        public T this[int index]
        {
            get
            {
                if (index >= count || index < 0)
                    throw new IndexOutOfRangeException();
                return items[ToInternalIndex(index)];
            }
            set
            {
                if (index >= count || index < 0)
                    throw new IndexOutOfRangeException();
                items[ToInternalIndex(index)] = value;
            }
        }

        public LinkedArrayList() : this(4)
        {

        }

        public LinkedArrayList(int capacity)
        {
            IntegerExtensions.ToPower2(ref capacity);
            items = new T[capacity];
            count = 0;
            firstReservedIndex = 0;
            firstFreeIndex = 0;
        }

        public LinkedArrayList(IEnumerable<T> collection) : this(collection.Count())
        {
            foreach (var item in collection)
            {
                AddLast(item);
            }
        }


        public void AddFirst(T item)
        {
            if (count == 0)//Empty collection
            {
                items[0] = item;
                count = 1;
                firstReservedIndex = 0;
                firstFreeIndex = 1;
            }
            else
            {
                if (count == items.Length)//Full collection
                    Resize(items.Length * 2);

                if (firstReservedIndex == 0)//Reached end, looping
                    firstReservedIndex = items.Length;

                firstReservedIndex--;
                items[firstReservedIndex] = item;
                count++;
            }
        }

        public void AddLast(T item)
        {
            if (count == 0)//Empty collection
            {
                items[0] = item;
                count = 1;
                firstReservedIndex = 0;
                firstFreeIndex = 1;
            }
            else
            {
                if (count == items.Length)//Full collection
                    Resize(items.Length * 2);

                if (firstFreeIndex == items.Length)//Reached end, looping
                    firstFreeIndex = 0;

                items[firstFreeIndex] = item;
                count++;
                firstFreeIndex++;
            }
        }

        public void RemoveFirst()
        {
            if (count > 0)
            {
                items[firstReservedIndex] = default(T);
                firstReservedIndex++;
                count--;

                if (firstReservedIndex == items.Length)
                    firstReservedIndex = 0;
            }
        }

        public void RemoveLast()
        {
            if (count > 0)
            {
                if (firstFreeIndex == 0)
                    firstFreeIndex = items.Length;

                firstFreeIndex--;
                items[firstFreeIndex] = default(T);
                count--;
            }
        }


        private int Find(T item)
        {
            var comparer = EqualityComparer<T>.Default;
            if (count == 0)
            {
                return -1;
            }
            else if (firstReservedIndex < firstFreeIndex)
            {
                for (int i = firstReservedIndex; i < firstFreeIndex; i++)
                {
                    if (comparer.Equals(items[i], item))
                    {
                        return i;
                    }
                }
                return -1;
            }
            else
            {
                for (int i = firstReservedIndex; i < items.Length; i++)
                {
                    if (comparer.Equals(items[i], item))
                    {
                        return i;
                    }
                }
                for (int i = 0; i < firstFreeIndex; i++)
                {
                    if (comparer.Equals(items[i], item))
                    {
                        return i;
                    }
                }
                return -1;
            }
        }

        private void InsertAtInternalIndex(int internalIndex, T item)
        {
            Contract.Requires(count < items.Length);
            //There is always room on the array at this point

            if (firstReservedIndex < firstFreeIndex)
            {
                if (firstReservedIndex == 0)
                {
                    //Expand at the end
                    T[] temp = new T[firstFreeIndex - internalIndex];
                    Array.Copy(items, internalIndex, temp, 0, temp.Length);
                    Array.Copy(temp, 0, items, internalIndex+1, temp.Length);

                    items[internalIndex] = item;
                    firstFreeIndex++;
                }
                else
                {
                    //Expand at the start
                    T[] temp = new T[internalIndex - firstReservedIndex + 1];
                    Array.Copy(items, firstReservedIndex, temp, 0, temp.Length);
                    Array.Copy(temp, 0, items, firstReservedIndex - 1, temp.Length);

                    items[internalIndex] = item;
                    firstReservedIndex--;
                }
            }
            else //Array is looped
            {
                if (internalIndex < firstFreeIndex)
                {
                    //Expand at the end
                    T[] temp = new T[firstFreeIndex - internalIndex];
                    Array.Copy(items, internalIndex, temp, 0, temp.Length);
                    Array.Copy(temp, 0, items, internalIndex + 1, temp.Length);

                    items[internalIndex] = item;
                    firstFreeIndex++;
                }
                else//if ()
                {
                    //Expand at the start
                    T[] temp = new T[internalIndex - firstReservedIndex + 1];
                    Array.Copy(items, firstReservedIndex, temp, 0, temp.Length);
                    Array.Copy(temp, 0, items, firstReservedIndex - 1, temp.Length);

                    items[internalIndex] = item;
                    firstReservedIndex--;
                }
            }
            if (firstFreeIndex == items.Length)
            {
                firstFreeIndex = 0;
            }
            if (firstReservedIndex == -1)
            {
                firstReservedIndex = items.Length - 1;
            }
            count++;
        }

        private void RemoveAtInternal(int internalIndex)
        {
            if (count > 0)
            {
                if (IsProperInternalIndex(internalIndex))
                {
                    if (firstReservedIndex < firstFreeIndex)
                    {                        
                        for (int i = internalIndex; i < firstFreeIndex; i++)
                        {
                            items[i] = items[i + 1];
                        }
                        firstFreeIndex--;
                        items[firstFreeIndex] = default(T);
                    }
                    else
                    {
                        if (internalIndex < firstReservedIndex)
                        {
                            for (int i = internalIndex; i < firstFreeIndex; i++)
                            {
                                items[i] = items[i + 1];
                            }
                            firstFreeIndex--;
                            items[firstFreeIndex] = default(T);
                        }
                        else
                        {
                            for (int i = internalIndex; i >= firstReservedIndex; i--)
                            {
                                items[i] = items[i - 1];
                            }
                            items[firstReservedIndex] = default(T);
                            firstReservedIndex++;
                        }
                    }
                    count--;
                }
            }
        }

        private void Resize(int newLength)
        {
            T[] newItems = new T[newLength];
            if (count > 0)
            {
                if (firstReservedIndex < firstFreeIndex) //Items do not loop
                {
                    Array.Copy(items, firstReservedIndex, newItems, 0, firstFreeIndex - firstReservedIndex);
                }
                else
                {
                    int interMediateIndex = items.Length;
                    Array.Copy(items, firstReservedIndex, newItems, 0, items.Length - firstReservedIndex);
                    Array.Copy(items, 0, newItems, items.Length - firstReservedIndex, firstFreeIndex);
                }
                //Since both branches copy data to the beginning
                firstReservedIndex = 0;
                firstFreeIndex = count;
            }
            items = newItems;
        }
        
        private int ToInternalIndex(int index)
        {
            var unloopedIndex = index + firstReservedIndex;

            return unloopedIndex - (unloopedIndex < items.Length ? 0 : items.Length);
        }

        private int ToRealIndex(int internalIndex)
        {
            var temp = internalIndex - firstReservedIndex;

            return temp < 0 ? temp + items.Length : temp;
        }

        private bool IsProperInternalIndex(int index)
        {
            if (firstReservedIndex < firstFreeIndex)
            {
                return index.IsInRangeHO(firstReservedIndex, firstFreeIndex);
            }
            else if (firstReservedIndex > firstFreeIndex)
            {
                return index.IsInRangeHO(firstReservedIndex, items.Length) 
                    || index.IsInRangeHO(0, firstFreeIndex);
            }
            else return false;
        }

        private IEnumerator<T> InternalGetEnumerator(int start, int end)
        {
            if (start < end)
            {
                for (int i = start; i < end; i++)
                {
                    yield return items[i];
                }
            }
            else
            {
                for (int i = start; i < items.Length; i++)
                {
                    yield return items[i];
                }
                for (int i = 0; i < end; i++)
                {
                    yield return items[i];
                }
            }
        }
        
        #region ICollection<T> Members

        public void Add(T item)
        {
            AddLast(item);
        }

        public void Clear()
        {
            count = 0;
            firstReservedIndex = 0;
            firstFreeIndex = 0;
            Array.Clear(items, 0, items.Length);
        }

        public bool Contains(T item)
        {
            return Find(item) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var item in this)
            {
                array[arrayIndex] = item;
                arrayIndex++;
            }
        }

        public int Count
        {
            get { return count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            int index = Find(item);
            if (index >= 0)
                RemoveAtInternal(index);
            return index >= 0;
        }
        
        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return InternalGetEnumerator(firstReservedIndex, firstFreeIndex);
        }

        public IEnumerator<T> GetEnumerator(int start, int length)
        {
            return InternalGetEnumerator(
                ToInternalIndex(start), 
                ToInternalIndex(length));
        }
        
        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
        
        #region IList<T> Members

        public int IndexOf(T item)
        {
            return ToRealIndex(Find(item));
        }

        public void Insert(int index, T item)
        {
            Contract.Requires<IndexOutOfRangeException>(index >= 0 && index <= count);

            if (index == count)
            {
                this.AddLast(item);
            }
            else
            {
                if (count == items.Length)
                {
                    Resize(items.Length * 2);
                }
                InsertAtInternalIndex(ToInternalIndex(index), item);
            }
        }

        public void RemoveAt(int index)
        {
            Contract.Requires<IndexOutOfRangeException>(index >= 0 && index < count);

            RemoveAtInternal(ToInternalIndex(index));
        }

        #endregion
    }
}
