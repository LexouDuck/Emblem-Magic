// -----------------------------------------------------------------------
// <copyright file="ArrayBuilder.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Nintenlord.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A lightweight structure for building arrays if size is known beforehand.
    /// </summary>
    public struct ArrayBuilder<T>
    {
        private int currentIndex;
        private T[] arrayToBuild;

        public int ArrayLength
        {
            get { return currentIndex; }
        }

        public ArrayBuilder(int startLength)
        {
            arrayToBuild = new T[startLength];
            currentIndex = 0;
        }

        public void AddItem(T item)
        {
            if (arrayToBuild.Length == currentIndex)
            {
                throw new InvalidOperationException("Trying to add too many items.");
                //Array.Resize(ref arrayToBuild, arrayToBuild.Length * 2);
            }
            arrayToBuild[currentIndex] = item;
            currentIndex++;
        }

        public void AddRange(IEnumerable<T> rangeToAdd)
        {
            foreach (var item in rangeToAdd)
            {
                this.AddItem(item);
            }
        }

        public void RemoveLast()
        {
            arrayToBuild[currentIndex] = default(T);
            currentIndex--;
        }

        public T[] GetArray()
        {
            var temp = arrayToBuild;
            arrayToBuild = null;
            return temp;
        }
    }
}
