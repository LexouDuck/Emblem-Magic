// -----------------------------------------------------------------------
// <copyright file="IStoringScanner.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Nintenlord.IO.Scanners
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A scanner that keeps old tokens and allows them to be accessed.
    /// </summary>
    public interface IStoringScanner<out T> : IScanner<T>
    {
        /// <summary>
        /// Returns the token at offset.
        /// </summary>
        /// <param name="offset">Offset that is smaller or equal than the current offset.</param>
        /// <returns>Token at offset.</returns>
        T this[long offset] { get; }

        bool IsStored(long offset);
        bool IsStored(long offset, long length);
    }

    public static class StoringScannerHelpers
    {
        public static T[] GetRange<T>(this IStoringScanner<T> s, long index, int length)
        {
            var list = new List<T>(length);

            checked
            {
                for (var i = index; i < index + length; i++)
                {
                    list.Add(s[i]);
                }
            }

            return list.ToArray();
        }
    }
}
