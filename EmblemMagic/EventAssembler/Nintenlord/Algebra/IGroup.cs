using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Algebra
{
    public interface IGroup<T>
    {
        IEnumerable<T> GetItems();

        T Inverse(T item);
        
        T Operation(T left, T right);
    }
}
