using System.Collections.Generic;

namespace Nintenlord.Collections.DataChange
{
    public interface IIndexOverlay : IEnumerable<int>, IEnumerable<KeyValuePair<int,int>>
    {
        bool ContainsIndexes { get; }
        int AmountOfIndexes { get; }
        int LastIndex { get; }
        int FirstIndex { get; }

        bool ContainsIndex(int index);
        bool ContainsAnyIndex(int index, int length);
        bool ContainsAllIndexes(int index, int length);

        void AddIndex(int index);
        void AddIndexes(int index, int length);

        bool RemoveIndex(int index);
        bool RemoveIndexes(int index, int length);

        /// <summary>
        /// Enumerates through all ranges that fully belong to [start, end).
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        IEnumerator<KeyValuePair<int, int>> GetRanges(int start, int end);
        /// <summary>
        /// Enumerates through values that are in [start, end)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        IEnumerator<int> GetEnumerator(int start, int end);
    }
}
