using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Nintenlord.Collections
{
    /// <summary>
    /// ftp://ftp.cs.umd.edu/pub/skipLists/skiplists.pdf
    /// A probabilistic alternative to balanced trees.
    /// </summary>
    /// <typeparam name="TKey">Type of the key to use.</typeparam>
    /// <typeparam name="TValue">Type of the value to use.</typeparam>
    public class SkipList<TKey, TValue> : IDictionary<TKey, TValue>
    {
        Random random;
        int maxLevel;
        double propability;
        IComparer<TKey> comparer;
        SkipListNode<TKey, TValue> head;

        int currentMaxLevel;
        int count;

        /// <summary>
        /// Constructs a new instance of skiplist.
        /// </summary>
        /// <param name="maxLevel">Maximun level for skiplist to achieve.</param>
        public SkipList(int maxLevel)
        {
            random = new Random();
            propability = 0.5;
            this.maxLevel = maxLevel;
            currentMaxLevel = 0;
            count = 0;
            comparer = Comparer<TKey>.Default;

            head = new SkipListNode<TKey, TValue>(maxLevel);
            for (int i = 0; i < maxLevel; i++)
            {
                head[i] = head;
            }
            random.Next();
        }

        //TODO: More constructors.

        private int NewLevel()
        {
            int level = 0;
            while (level < currentMaxLevel && random.NextDouble() < propability)
            {
                level++;
            }
            if (level == maxLevel)
            {
                level--;
            }
            return level;
        }

        private class SkipListEnumarator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            SkipListNode<TKey, TValue> currentNode;
            SkipList<TKey, TValue> list;

            public SkipListEnumarator(SkipList<TKey, TValue> list)
            {
                this.list = list;
                Reset();
            }

            #region IEnumerator<T> Members

            public KeyValuePair<TKey, TValue> Current
            {
                get { return (KeyValuePair<TKey, TValue>)currentNode; }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                currentNode = null;
                list = null;
            }

            #endregion

            #region IEnumerator Members

            object IEnumerator.Current
            {
                get { return currentNode.Value; }
            }

            public bool MoveNext()
            {
                if (currentNode[0] != list.head)
                {
                    currentNode = currentNode[0];
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public void Reset()
            {
                currentNode = list.head;
            }

            #endregion
        }

        #region IEnumerable<KeyValuePair<TKey, TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new SkipListEnumarator(this);
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region ICollection<KeyValuePair<TKey, TValue>> Members

        public void Clear()
        {
            for (int i = 0; i < maxLevel; i++)
            {
                head[i] = null;
            }
            count = 0;
            currentMaxLevel = 0;
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            SkipListNode<TKey, TValue> current = this.head[0];
            for (int i = 0; i < this.count; i++)
            {
                array[i + arrayIndex] = (KeyValuePair<TKey, TValue>)current;
                current = current[0];
            }
        }

        public int Count
        {
            get { return count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            TValue value;
            return this.TryGetValue(item.Key, out value)
                && EqualityComparer<TValue>.Default.Equals(value, item.Value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            TValue value;
            if (this.TryGetValue(item.Key, out value) 
                && EqualityComparer<TValue>.Default.Equals(value, item.Value))
            {
                return this.Remove(item.Key);
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region IDictionary<TKey,TValue> Members

        public void Add(TKey key, TValue value)
        {
            this.Insert(key, value, true);
        }

        protected void Insert(TKey key, TValue value, bool addNew)
        {
            if (key == null)
            {
                throw new ArgumentNullException();
            }

            SkipListNode<TKey, TValue> currentNode = head;
            SkipListNode<TKey, TValue>[] toUpdate = new  SkipListNode<TKey, TValue>[currentMaxLevel];

            for (int level = currentMaxLevel - 1; level >= 0; level--)
            {
                while (currentNode[level].Key != null && 
                    comparer.Compare(key, currentNode[level].Key) > 0)
                    currentNode = currentNode[level];
                toUpdate[level] = currentNode;
            }

            currentNode = currentNode[0];

            if (comparer.Compare(currentNode.Key,key) == 0)
            {
                if (addNew)
                    throw new ArgumentException("");
                else
                    currentNode.Value = value;
            }
            else
            {
                int newLevel = this.NewLevel();
                SkipListNode<TKey, TValue> newNode = new SkipListNode<TKey,TValue>(key,value,newLevel);
                if (newLevel == currentMaxLevel)
                {
                    head[newLevel - 1] = newNode;//Necessary?
                    currentMaxLevel++;
                }

                for (int i = 0; i < newLevel; i++)
                {
                    newNode[i] = toUpdate[i][i];
                    toUpdate[i][i] = newNode;
                }
                this.count++;
            }
        }

        protected SkipListNode<TKey, TValue> GetNode(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException();
            }
            SkipListNode<TKey, TValue> currentNode = head;

            for (int level = currentMaxLevel - 1; level >= 0; level--)
            {
                while (currentNode[level].Key != null &&
                    comparer.Compare(key, currentNode[level].Key) > 0)
                    currentNode = currentNode[level];
            }

            currentNode = currentNode[0];
            return comparer.Compare(currentNode.Key, key) == 0 ? currentNode : null;
        }

        public bool ContainsKey(TKey key)
        {
            return GetNode(key) != null;
        }

        public bool Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException();

            SkipListNode<TKey, TValue> currentNode = head;
            SkipListNode<TKey,TValue>[] toUpdate = new SkipListNode<TKey,TValue>[currentMaxLevel];

            for (int level = currentMaxLevel - 1; level >= 0; level--)
            {
                while (currentNode[level].Key != null && 
                    comparer.Compare(key, currentNode[level].Key) > 0)
                    currentNode = currentNode[level];
                toUpdate[level] = currentNode;
            }

            currentNode = currentNode[0];

            if (comparer.Compare(currentNode.Key,key) == 0)
            {
                for (int i = 0; i < currentMaxLevel; i++)
                {
                    if (toUpdate[i][i] != currentNode)
                        break;
                    toUpdate[i][i] = currentNode[i];
                }

                while (currentMaxLevel > 0 && head[currentMaxLevel - 1] == head)
                {
                    currentMaxLevel--;
                }
                return true;
            }
            else return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            SkipListNode<TKey, TValue> node = GetNode(key);
            if (node != null)
            {
                value = node.Value;
                return true;
            }
            else
            {
                value = default(TValue);
                return false;
            }
        }

        public ICollection<TKey> Keys
        {
            get 
            { 
                ICollection<TKey> collection = new LinkedList<TKey>();
                SkipListNode<TKey, TValue> node = head[0];
                while (node != head)
                {
                    collection.Add(node.Key);
                    node = node[0];
                }
                return collection;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                ICollection<TValue> collection = new LinkedList<TValue>();
                SkipListNode<TKey, TValue> node = head[0];
                while (node != head)
                {
                    collection.Add(node.Value);
                    node = node[0];
                }
                return collection;
            }
        }
        
        public TValue this[TKey key]
        {
            get
            {
                SkipListNode<TKey, TValue> node = GetNode(key);
                if (node != null)
                {
                    return node.Value;
                }
                else
                {
                    throw new KeyNotFoundException("Key not found.");
                }
            }
            set
            {
                Insert(key, value, false);                
            }
        }

        #endregion
        
        /// <summary>
        /// Calculates the recommended max level to use for constructing a new skiplist.
        /// </summary>
        /// <param name="propability">A propability between values 0 and 1.</param>
        /// <param name="capacity">Amount of items skiplist will be storing.</param>
        /// <returns>The recomended max level for skiplist with passed propability and capacity.</returns>
        public static int RecommendedMaxLevel(double propability, int capacity)
        {
            return (int)(- Math.Log(2 * capacity) / Math.Log(propability));
        }

        /// <summary>
        /// Calculates the recommended probapility to use for constructing a new skiplist.
        /// </summary>
        /// <param name="maxLevel">Maximun level for skiplist to use.</param>
        /// <param name="capacity">Amount of items skiplist will be storing.</param>
        /// <returns>The recommended probapility for skiplist with passes values.</returns>
        public static double RecommendedProbapility(int maxLevel, int capacity)
        {
            return Math.Pow(2 * capacity, 1/(double)maxLevel);
        }        
    }
}