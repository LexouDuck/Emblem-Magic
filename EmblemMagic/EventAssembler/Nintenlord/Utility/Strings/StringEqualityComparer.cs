using System;
using System.Collections.Generic;
using System.Linq;

namespace Nintenlord.Utility.Strings
{
    class StringEqualityComparer : IEqualityComparer<string>
    {
        #region IEqualityComparer<string> Members

        public bool Equals(string x, string y)
        {
            if (x.Length != y.Length)
            {
                return false;
            }
            return !x.Where((t, i) => t != y[i]).Any();
        }

        public int GetHashCode(string obj)
        {
            const int max = 16;
            int result = 0;
            int min = Math.Min(max, obj.Length);
            for (int i = 0; i < min; i++)
            {
                result |= obj[i].GetHashCode() >> ((16 / max) * i);
            }
            return result;
        }

        #endregion
    }
}
