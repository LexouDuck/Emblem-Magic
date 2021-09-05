using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Collections.DataChange
{
    /// <summary>
    /// Collection to keep track of changes to a array of data
    /// </summary>
    /// <typeparam name="T">Type whose array is to be changed</typeparam>
    public sealed class DataChange<T> : IDataChange<T>
    {
        SortedList<int, T[]> dataToChange;

        public int LastOffset
        {
            get
            {
                int lastKey = dataToChange.Keys[dataToChange.Count-1];
                return lastKey + dataToChange[lastKey].Length;
            }
        }

        public int FirstOffset
        {
            get
            {
                int firstKey = dataToChange.Keys[0];
                return firstKey;
            }

        }

        public bool ChangesAnything
        {
            get
            {
                return dataToChange.Count != 0;
            }
        }

        /// <summary>
        /// Creates a new DataChange.
        /// </summary>
        public DataChange()
        {
            dataToChange = new SortedList<int, T[]>();
        }

        public void AddChangedData(int offset, T[] data)
        {
            this.AddChangedData(offset, data, 0, data.Length);
        }

        public void AddChangedData(int offset, T[] data, int index, int count)
        {
            if (offset < 0)
                throw new IndexOutOfRangeException("Negative offset was passed.");
            if (data == null)
                throw new ArgumentNullException();
            if (index + count > data.Length)
                throw new IndexOutOfRangeException();

            List<int> intersectedKeys = GetIntersectingKeys(offset, count).ToList();

            int newOffset;
            int newLastOffset;

            if (intersectedKeys.Count > 0)
            {
                newOffset = Math.Min(intersectedKeys[0], offset);
                int lastKey = intersectedKeys[intersectedKeys.Count - 1];
                newLastOffset = Math.Max(lastKey + dataToChange[lastKey].Length, offset + count);
            }
            else
            {
                newOffset = offset;
                newLastOffset = offset + count;
            }

            T[] newData = new T[newLastOffset - newOffset];
            foreach (int item in intersectedKeys)
            {
                T[] oldData = dataToChange[item];
                Array.Copy(oldData, 0, newData, item - newOffset, oldData.Length);
                dataToChange.Remove(item);
            }

            Array.Copy(data, index, newData, offset - newOffset, count);
            dataToChange.Add(newOffset, newData);
        }

        /// <summary>
        /// A sorted list of intersected keys.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private IEnumerable<int> GetIntersectingKeys(int offset, int count)
        {
            return from item in (IEnumerable<KeyValuePair<int, T[]>>) this 
                   where Intersects(offset, count, item.Key, item.Value.Length) 
                   select item.Key;
        }

        public void Apply(ref T[] data)
        {
            if (ChangesAnything)
            {
                if (this.LastOffset > data.Length)
                {
                    Array.Resize(ref data, this.LastOffset);
                }

                ApplyTo(data);                
            }
        }

        private void ApplyTo(T[] data)
        {
            foreach (KeyValuePair<int, T[]> item in dataToChange)
            {
                Array.Copy(item.Value, 0, data, item.Key, item.Value.Length);
            }
        }

        /// <summary>
        /// Returns The string representation of this instance.
        /// </summary>
        /// <returns>The string representation of this instance.</returns>
        public override string ToString()
        {
            StringBuilder text = new StringBuilder();
            foreach (KeyValuePair<int, T[]> item in dataToChange)
            {
                text.Append(item.Key + ": ");
                foreach (T item2 in item.Value)
                {
                    text.Append(item2 + " ");
                }
                text.AppendLine();
            }
            return text.ToString();
        }

        public int AmountOfChanges
        {
            get
            {
                return dataToChange.Values.Sum(item => item.Length);
            }
        }
        
        public bool Equals(IDataChange<T> other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(T[] other)
        {
            IEqualityComparer<T> comp = EqualityComparer<T>.Default;

            return !(from item in dataToChange 
                     let offset = item.Key 
                     where item.Value.Where((t, i) => !comp.Equals(other[offset + i], t)).Any() 
                     select item).Any();
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return new ChangeEnumerator(this);
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region IEnumerable<KeyValuePair<int,T[]>> Members

        IEnumerator<KeyValuePair<int, T[]>> IEnumerable<KeyValuePair<int, T[]>>.GetEnumerator()
        {
            return dataToChange.GetEnumerator();
        }

        #endregion

        private class ChangeEnumerator : IEnumerator<T>
        {
            IEnumerator<KeyValuePair<int, T[]>> enume;
            DataChange<T> parent;
            int index;
            bool moveNext;

            public ChangeEnumerator(DataChange<T> parent)
            {
                this.parent = parent;
                this.enume = parent.dataToChange.GetEnumerator();
                this.Reset();
            }

            #region IEnumerator<T> Members

            public T Current
            {
                get { return enume.Current.Value[index]; }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                enume.Dispose();
                parent = null;
                index = 0;
            }

            #endregion

            #region IEnumerator Members

            object IEnumerator.Current
            {
                get { return this.Current; }
            }

            public bool MoveNext()
            {
                if (moveNext)
                {
                    if (enume.MoveNext())
                    {
                        index = 0;
                        moveNext = false;
                        return true;
                    }
                    else return false;
                }
                else
                {
                    index++;
                    moveNext = enume.Current.Value.Length == index - 1;
                    return true;
                }
            }

            public void Reset()
            {
                index = 0;
                enume.Reset();
                moveNext = true;
            }

            #endregion
        }


        public static DataChange<T> GetData(T[] array, IIndexOverlay overlay)
        {
            if (!overlay.ContainsIndexes)
            {
                return new DataChange<T>();
            }
            if (overlay.LastIndex > array.Length)
            {
                throw new IndexOutOfRangeException();
            }

            DataChange<T> result = new DataChange<T>();
            foreach (var item in (overlay as IEnumerable<KeyValuePair<int,int>>))
            {
                T[] temp = new T[item.Value];
                Array.Copy(array, item.Key, temp, 0, temp.Length);
                result.AddChangedData(item.Key, array);
            }
            return result;
        }

        public static bool Intersects(int index1, int length1, int index2, int length2)
        {
            if (length1 == 0 || length2 == 0)
                return false;
            return (index1 < index2 + length2 && index1 >= index2) ||
                   (index2 < index1 + length1 && index2 >= index1);
        }

        #region IDataChange<T> Members


        public void Clear()
        {
            dataToChange.Clear();
        }

        #endregion
    }
}
//Test code:
/*
            byte[] data = new byte[100];
            DataChange<byte> changedData = new DataChange<byte>();
            Random rand = new Random(DateTime.Now.Millisecond);
            rand.NextBytes(data);
            for (int i = 0; i < 10; i++)
            {
                int offset = rand.Next(data.Length);
                int lenght = rand.Next(data.Length / 10);
                byte[] dataToChange = new byte[lenght];
                rand.NextBytes(dataToChange);
                changedData.AddChangedData(offset, dataToChange);
            }
            changedData.AddChangedData(data.Length, new byte[] { 0, 0, 0, 0, 0 });
            byte[] newData = changedData.Apply(data);

            StreamWriter sw = new StreamWriter(@"C:\Users\Timo\Desktop\text.txt");
            sw.WriteLine("Original data:");
            foreach (byte item in data)
            {
                sw.Write(item + " ");
            }
            sw.WriteLine();

            sw.WriteLine("Changed data:");
            sw.WriteLine(changedData);
            sw.WriteLine("Result:");
            foreach (byte item in newData)
            {
                sw.Write(item + " ");
            }
            sw.WriteLine();
            sw.WriteLine();
            sw.Close();
            MessageBox.Show("Finished.");
 */