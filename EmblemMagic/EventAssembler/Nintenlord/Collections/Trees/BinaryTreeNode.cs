using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Collections.Trees
{
    public class BinaryTreeNode<T> : IValuedTree<T>
    {
        public T Value
        {
            get;
            private set;
        }

        public BinaryTreeNode<T> Parent
        {
            get;
            private set;
        }
        public BinaryTreeNode<T> Left
        {
            get;
            private set;
        }
        public BinaryTreeNode<T> Right
        {
            get;
            private set;
        }
        public bool IsLeaf
        {
            get;
            private set;
        }
        public bool HasValue
        {
            get;
            private set;
        }

        public BinaryTreeNode()
        {

        }
        public BinaryTreeNode(T value)
        {
            this.Value = value;
            IsLeaf = true;
            HasValue = true;
        }
        public BinaryTreeNode(BinaryTreeNode<T> left, BinaryTreeNode<T> right)
        {
            this.Left = left;
            this.Right = right;
            left.Parent = this;
            right.Parent = this;
            IsLeaf = false;
            HasValue = false;
        }
        public BinaryTreeNode(BinaryTreeNode<T> left, BinaryTreeNode<T> right, T value)
        {
            this.Left = left;
            this.Right = right;
            left.Parent = this;
            right.Parent = this;
            this.Value = value;
            IsLeaf = false;
            HasValue = true;
        }

        public void AddLeafValues(ICollection<T> collection)
        {
            if (this.HasValue)
            {
                collection.Add(this.Value);
            }
            if (Left != null)
            {
                this.Left.AddLeafValues(collection);
            }
            if (Right != null)
            {
                this.Right.AddLeafValues(collection);
            }
        }

        public void AddLeafValues(IDictionary<T, bool[]> values, IList<bool> branches)
        {
            if (this.HasValue)
            {
                values[Value] = branches.ToArray();
            }
            if (Left != null)
            {
                branches.Add(false);
                Left.AddLeafValues(values, branches);
                branches.RemoveAt(branches.Count - 1);

            }
            if (Right != null)
            {
                branches.Add(true);
                Right.AddLeafValues(values, branches);
                branches.RemoveAt(branches.Count - 1);
            }
        }

        #region ITree<T> Members

        public IEnumerable<IValuedTree<T>> GetChildren()
        {
            yield return Left;
            yield return Right;
        }

        #endregion
    }
}
