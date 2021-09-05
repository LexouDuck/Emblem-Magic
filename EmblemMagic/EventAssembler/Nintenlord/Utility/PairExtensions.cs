using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Utility
{
    public static class PairExtensions
    {
        public static TResult Apply<T1, T2, TResult>(this KeyValuePair<T1,T2> item, 
            Func<T1,T2, TResult> f)
        {
            return f(item.Key, item.Value);
        }
        
        public static bool Equals<T1, T2>(this KeyValuePair<T1, T2> item, T2 toCompare)
        {
            return EqualityComparer<T2>.Default.Equals(toCompare, item.Value);
        }

        public static bool Equals<T1, T2>(this KeyValuePair<T1, T2> item, T1 toCompare)
        {
            return EqualityComparer<T1>.Default.Equals(toCompare, item.Key);
        }
    }
}
