using System.Collections.Generic;

namespace Nintenlord.Collections.Dictionaries
{
    interface ITriDictionary<TKey1, TKey2, TValue> : 
        IDictionary<KeyValuePair<TKey1,TKey2>,TValue>
    {
        TValue this[TKey1 key1, TKey2 key2] { get; set; }
        IDictionary<TKey1, TKey2> KeyMappings { get; }

        bool Contains(TKey1 key1, TKey2 key2);
        void Add(TKey1 key1, TKey2 key2, TValue value);
        bool Remove(TKey1 key1, TKey2 key2);
        bool TryGetValue(TKey1 key1, TKey2 key2, out TValue value);
    }


}
