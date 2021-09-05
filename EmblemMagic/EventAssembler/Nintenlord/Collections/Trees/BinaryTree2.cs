using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nintenlord.Utility;

namespace Nintenlord.Collections.Trees
{
    /// <typeparam name="T">Type of the values to hold.</typeparam>
    public class BinaryTree2<T> : IEnumerable<T>
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

        public BinaryTree2(BinaryTree2<T> left, BinaryTree2<T> right)
        {
            this.Count = left.Count + right.Count;
            this.MaxDepth = Math.Max(left.MaxDepth, right.MaxDepth) + 1;
            this.Head = new BinaryTreeNode<T>(left.Head, right.Head);
        }
        public BinaryTree2(BinaryTree2<T> left, BinaryTree2<T> right, T value)
        {
            this.Count = left.Count + right.Count + 1;
            this.MaxDepth = Math.Max(left.MaxDepth, right.MaxDepth) + 1;
            this.Head = new BinaryTreeNode<T>(left.Head, right.Head, value);
        }
        public BinaryTree2(T value)
        {
            this.Head = new BinaryTreeNode<T>(value);
            this.Count = 1;
            this.MaxDepth = 1;
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

        public static implicit operator Dictionary<T, bool[]>(BinaryTree2<T> tree)
        {
            Dictionary<T, bool[]> dict = new Dictionary<T, bool[]>();
            tree.Head.AddLeafValues(dict, new bool[] { });
            return dict;
        }
    }
}
