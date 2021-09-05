using System;
using System.Collections.Generic;

namespace Nintenlord.Collections.DataChange
{
    /// <summary>
    /// Collection to keep track of changes to an array of data
    /// </summary>
    /// <typeparam name="T">Type whose array is to be changed</typeparam>
    public interface IDataChange<T> : IEquatable<IDataChange<T>>, IEquatable<T[]>, IEnumerable<T>, IEnumerable<KeyValuePair<int, T[]>>
    {
        /// <summary>
        /// Returns false if Apply doesn't change the data, else true
        /// </summary>
        bool ChangesAnything { get; }

        /// <summary>
        /// Returns the amount of changed indexes.
        /// </summary>
        int AmountOfChanges { get; }
        /// <summary>
        /// Throws exception if ChangesAnything == false.
        /// Else returns the first offset this instance changes.
        /// </summary>
        int FirstOffset { get; }

        /// <summary>
        /// Throws exception if ChangesAnything == false.
        /// Else returns the last changed offset + 1.
        /// </summary>
        int LastOffset { get; }

        /// <summary>
        /// Adds new change. If old change and new change overlap, new overwrites
        /// </summary>
        /// <param name="offset">Non-negative offset of data</param>
        /// <param name="data">Array of data that changes at offset</param>
        void AddChangedData(int offset, T[] data);

        /// <summary>
        /// Adds new change. If old change and new change overlap, new overwrites
        /// </summary>
        /// <param name="offset">Non-negative offset of data</param>
        /// <param name="data">Array of data that changes at offset</param>
        /// <param name="index">Index to start including from</param>
        /// <param name="count">Amount of items to add</param>
        void AddChangedData(int offset, T[] data, int index, int count);

        /// <summary>
        /// Applies changes to array. Array is rezised if necessary
        /// </summary>
        /// <param name="data">Data to apply to</param>
        /// <returns>New data where changes were applied to</returns>
        void Apply(ref T[] data);

        /// <summary>
        /// Removes all changes. ChangesAnything == false after this.
        /// </summary>
        void Clear();
    }
}
