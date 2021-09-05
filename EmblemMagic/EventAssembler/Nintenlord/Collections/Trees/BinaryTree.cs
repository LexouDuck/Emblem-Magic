using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nintenlord.Utility;

namespace Nintenlord.Collections.Trees
{
    /// <summary>
    /// Contains values in leafs only.
    /// </summary>
    /// <typeparam name="T">Type of the values to hold.</typeparam>
    public class BinaryTree<T> : ICollection<T>
    {
        public int Count
        {
            get;
            private set;
        }
        public int MaxDepth
        {
            get;
            private set;
        }

        public BinaryTreeNode<T> Head
        {
            get;
            private set;
        }
        
        public BinaryTree(BinaryTree<T> left, BinaryTree<T> right)
        {
            this.Count = left.Count + right.Count;
            this.MaxDepth = Math.Max(left.MaxDepth, right.MaxDepth) + 1;
            this.Head = new BinaryTreeNode<T>(left.Head, right.Head);
        }
        public BinaryTree(T value)
        {
            this.Head = new BinaryTreeNode<T>(value);
            this.Count = 1;
            this.MaxDepth = 1;
        }
        
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
            return this.Any(item2 => EqualityComparer<T>.Default.Equals(item, item2));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var item in this)
            {
                array[arrayIndex] = item;
                arrayIndex++;
            }
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
            return ((IValuedTree<T>)Head).BreadthFirstEnumerator().GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        public static implicit operator Dictionary<T, bool[]>(BinaryTree<T> tree)
        {
            Dictionary<T, bool[]> dict = new Dictionary<T, bool[]>();
            tree.Head.AddLeafValues(dict, new bool[] { });
            return dict;
        }
    }
}
