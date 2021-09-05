// -----------------------------------------------------------------------
// <copyright file="MicsUtilities.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Nintenlord.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class MicsUtilities
    {
        public static void Swap<T>(ref T first, ref T second)
        {
            var temp = first;
            first = second;
            second = temp;
        }

        public static IEnumerable<int> GetRandomIntegers(this Random random, IEnumerable<int> maxValues)
        {
            foreach (var maxValue in maxValues)
            {
                yield return random.Next(maxValue);
            }
        }

        public static T Cast<T>(this object item)
        {
            return (T)item;
        }
    }
}
