using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Utility
{
    /// <summary>
    /// Object that has a unique name 
    /// </summary>
    /// <typeparam name="T">Type of the name</typeparam>
    public interface INamed<out T>
    {
        /// <summary>
        /// Name of the object. Must not change in equals mind
        /// </summary>
        T Name { get; }
    }

    public static class NamedHelper
    {
        public static Dictionary<T, TNamed> GetDictionary<T, TNamed>(this IEnumerable<TNamed> named)
            where TNamed : INamed<T>
        {
            return named.ToDictionary(item => item.Name);
        }

        public static bool AllAreUnique<T, TNamed>(IEnumerable<TNamed> items)
            where TNamed : INamed<T>
        {
            ISet<T> set = new HashSet<T>();
            foreach (var item in items)
            {
                if (set.Contains(item.Name))
                {
                    return false;
                }
                set.Add(item.Name);
            }
            return true;
        }

        public static IEnumerable<KeyValuePair<T, TNamed>> GetEnumerator<T, TNamed>(this IEnumerable<TNamed> named)
            where TNamed : INamed<T>
        {
            return named.Select(item => new KeyValuePair<T, TNamed>(item.Name, item));
        }

        public class NamedEqualityComparer<T, TNamed> 
            : IEqualityComparer<TNamed>
            where TNamed : INamed<T>
        {
            IEqualityComparer<T> coreComp;

            public NamedEqualityComparer() 
                : this(EqualityComparer<T>.Default)
            {

            }

            public NamedEqualityComparer(IEqualityComparer<T> comparer)
            {
                this.coreComp = comparer;
            }

            #region IEqualityComparer<NamedT> Members

            public bool Equals(TNamed x, TNamed y)
            {
                return coreComp.Equals(x.Name, y.Name);
            }

            public int GetHashCode(TNamed obj)
            {
                return coreComp.GetHashCode(obj.Name);
            }

            #endregion
        }
    }
}
