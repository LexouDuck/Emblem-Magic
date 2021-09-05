using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Utility
{
    public class FunctionComparer<T> : IComparer<T>
    {
        Func<T, int> valueFunction;

        public FunctionComparer(Func<T, int> valueFunction)
        {
            this.valueFunction = valueFunction;
        }

        #region IComparer<Node> Members

        public int Compare(T x, T y)
        {
            return valueFunction(x) - valueFunction(y);
        }

        #endregion

        public static explicit operator FunctionComparer<T>(Func<T, int> valueFunction)
        {
            return new FunctionComparer<T>(valueFunction);
        }
    }
}
