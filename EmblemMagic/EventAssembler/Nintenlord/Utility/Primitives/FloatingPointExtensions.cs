// -----------------------------------------------------------------------
// <copyright file="FloatingPointHelper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Nintenlord.Utility.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class FloatingPointExtensions
    {
        public static IEnumerable<int> GetIntegersBetween(float min, float max)
        {
            if (min > max)
            {
                throw new ArgumentException("min is larger than max");
            }
            min = (float)Math.Ceiling(min);
            max = (float)Math.Floor(max);
            int minI = (int)min;
            int maxI = (int)max;

            for (int i = minI; i <= maxI; i++)
            {
                yield return i;
            }
        }

        public static bool IsInRange(this float val, float min, float max)
        {
            return val >= min && val <= max;
        }

        public static IEnumerable<float> GetFloats(int n)
        {
            for (int i = 0; i <= n; i++)
            {
                yield return (float)i / (float)n;
            }
        }
    }
}
