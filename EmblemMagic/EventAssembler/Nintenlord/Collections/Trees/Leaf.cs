// -----------------------------------------------------------------------
// <copyright file="Leaf.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Nintenlord.Collections.Trees
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    public sealed class Leaf<T> : IValuedTree<T>
    {
        T value;

        public Leaf(T value)
        {
            this.value = value;
        }

        #region ITree<T> Members

        public IEnumerable<IValuedTree<T>> GetChildren()
        {
            yield break;
        }

        public T Value
        {
            get { return value; }
        }

        public bool HasValue
        {
            get { return true; }
        }

        #endregion
    }
}
