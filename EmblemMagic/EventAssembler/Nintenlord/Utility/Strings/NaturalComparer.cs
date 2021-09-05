using System;
using System.Collections.Generic;

namespace Nintenlord.Utility.Strings
{
    public class NaturalComparer : IComparer<string>
    {


        #region IComparer<string> Members

        public int Compare(string x, string y)
        {
            int length = Math.Min(x.Length, y.Length);

            for (int i = 0; i < length; i++)
            {
                if (x[i] != y[i])
                {
                    if (x[i] == '_')
                        return 1;
                    else if (y[i] == '_')
                        return -1;
                    else
                        return x[i].CompareTo(y[i]);
                }
            }
            return x.Length - y.Length;
        }

        #endregion
    }
}
