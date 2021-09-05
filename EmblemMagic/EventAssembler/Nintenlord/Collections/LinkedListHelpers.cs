using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nintenlord.Collections.Lists;

namespace Nintenlord.Collections
{
    public static class LinkedListHelpers
    {
        public static IEnumerable<T> GetEnumerator<T>(LinkedListNode<T> start, LinkedListNode<T> end = null)
        {
            if (start == null)
            {
                throw new ArgumentNullException();
            }
            if (end != null && start.List != end.List)
            {
                throw new ArgumentException();
            }

            while (start != end)
            {
                yield return start.Value;
                start = start.Next;
            }
        }

        public static void RemoveExtra<T>(this LinkedList<T> item, int maxSize)
        {
            while (item.Count > maxSize)
            {
                item.RemoveFirst();
            }
        }

        public static IEnumerable<T> EnumerateLast<T>(this LinkedList<T> item, int amount)
        {
            var start = item.Last;
            for (int i = 1; i < amount; i++)
            {
                start = start.Previous;
            }
            return GetEnumerator(start);
        }

        public static void RemoveExtra<T>(this LinkedArrayList<T> item, int maxSize)
        {
            while (item.Count > maxSize)
            {
                item.RemoveFirst();
            }
        }

        public static IEnumerable<T> EnumerateLast<T>(this LinkedArrayList<T> item, int amount)
        {
            var enumerator = item.GetEnumerator(item.Count - amount, amount);

            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
    }
}
