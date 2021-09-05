using System;
using System.Collections.Generic;
using System.Linq;

namespace Nintenlord.Collections.Dictionaries
{
    public class ArrayDictionary<TKey,TValue> : 
        List<KeyValuePair<TKey, TValue>>, 
        IDictionary<TKey,TValue>
    {
        IEqualityComparer<TKey> comparer;

        public ArrayDictionary()
            : this(EqualityComparer<TKey>.Default)
        {

        }

        public ArrayDictionary(int capacity)
            : this(capacity, EqualityComparer<TKey>.Default)
        {

        }

        public ArrayDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
            : this(collection, EqualityComparer<TKey>.Default)
        {

        }

        public ArrayDictionary(IEqualityComparer<TKey> comparer)
            : base()
        {
            this.comparer = comparer;
        }

        public ArrayDictionary(int capacity, IEqualityComparer<TKey> comparer)
            : base(capacity)
        {
            this.comparer = comparer;
        }

        public ArrayDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, 
            IEqualityComparer<TKey> comparer)
            : base(collection)
        {
            this.comparer = comparer;
        }

        #region IDictionary<TKey,TValue> Members

        public void Add(TKey key, TValue value)
        {
            if (ContainsKey(key))
            {
                throw new ArgumentException();
            }
            this.Add(new KeyValuePair<TKey,TValue>(key, value));
        }

        public bool ContainsKey(TKey key)
        {
            return this.Any(item => comparer.Equals(item.Key, key));
        }

        public ICollection<TKey> Keys
        {
            get 
            {
                List<TKey> keys = new List<TKey>(this.Count);
                keys.AddRange(this.Select(item => item.Key));
                return keys;
            }
        }

        public bool Remove(TKey key)
        {
            int index = this.FindIndex(x => comparer.Equals(key, x.Key));
            if (index == -1)
            {
                return false;
            }
            else
            {
                this.RemoveAt(index);
                return true;
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            foreach (var item in this)
            {
                if (comparer.Equals(key, item.Key))
                {
                    value = item.Value;
                    return true;
                }
            }
            value = default(TValue);
            return false;
        }

        public ICollection<TValue> Values
        {
            get
            {
                List<TValue> values = new List<TValue>(this.Count);
                values.AddRange(this.Select(item => item.Value));
                return values;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (this.TryGetValue(key, out value))
                {
                    return value;
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            set
            {
                int i;
                for (i = 0; i < this.Count; i++)
                {
                    if (comparer.Equals(this[i].Key, key))
                    {
                        break;
                    } 
                }
                if (i < this.Count)
                {
                    this[i] = new KeyValuePair<TKey, TValue>(key, value);
                }
                else
                {
                    this.Add(new KeyValuePair<TKey, TValue>(key, value));
                }
            }
        }

        #endregion

    }
}
