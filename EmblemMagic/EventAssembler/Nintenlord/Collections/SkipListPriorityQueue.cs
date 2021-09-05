using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Nintenlord.Collections
{
    public class SkipListPriorityQueue<TPriority, TValue> : IPriorityQueue<TPriority,TValue>
    {
        Random random;
        int maxLevel;
        int currentMaxLevel;
        double propability;
        int count;
        int version;
        IComparer<TPriority> comparer;
        SkipListNode<TPriority, TValue> head;

        public SkipListPriorityQueue()
            : this(100, Comparer<TPriority>.Default, 0.5, new Random())
        {

        }

        public SkipListPriorityQueue(int maxLevel)
            : this(maxLevel, Comparer<TPriority>.Default, 0.5, new Random())
        {

        }

        public SkipListPriorityQueue(int maxLevel, IComparer<TPriority> comparer)
            : this(maxLevel, comparer, 0.5, new Random())
        {

        }

        public SkipListPriorityQueue(int maxLevel, IComparer<TPriority> comparer, double propability)
            : this(maxLevel, comparer, propability, new Random())
        {

        }

        public SkipListPriorityQueue(int maxLevel, IComparer<TPriority> comparer, double propability, Random random)
        {
            this.propability = propability;
            this.random = random;
            this.comparer = comparer;
            this.maxLevel = maxLevel;
            this.currentMaxLevel = 1;
            this.count = 0;
            this.version = 0;
            this.head = new SkipListNode<TPriority, TValue>(maxLevel);
            for (int i = 0; i < maxLevel; i++)
            {
                this.head[i] = head;
            }
        }

        private int NewLevel()
        {
            int level = 1;
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

        private void UpdateLevel(int startFrom)
        {
            int i;
            for (i = startFrom; i >= 0; i--)
            {
                if (head[i] != head)
                {
                    break;
                }
            }
            if (i == -1)
            {
                currentMaxLevel = 1;
            }
            else
            {
                currentMaxLevel = i + 1;
            }
        }

        public void Enqueue(TValue value, TPriority priority)
        {
            if (priority == null)
            {
                throw new ArgumentNullException();
            }

            SkipListNode<TPriority, TValue> currentNode = head;
            SkipListNode<TPriority, TValue>[] toUpdate =
                new SkipListNode<TPriority, TValue>[currentMaxLevel];

            for (int level = currentMaxLevel - 1; level >= 0; level--)
            {
                while (currentNode[level] != head &&
                    comparer.Compare(priority, currentNode[level].Key) > 0)
                    currentNode = currentNode[level];
                toUpdate[level] = currentNode;
            }

            currentNode = currentNode[0];
            
            int newLevel = this.NewLevel();
            SkipListNode<TPriority, TValue> newNode =
                new SkipListNode<TPriority, TValue>(priority, value, newLevel);

            if (newLevel == currentMaxLevel)
            {
                //head[newLevel - 1] = newNode;
                currentMaxLevel++;
            }

            for (int i = 0; i < newLevel; i++)
            {
                newNode[i] = toUpdate[i][i];
                toUpdate[i][i] = newNode;
            }

            if (!newNode.Validate())
            {
                throw new FormatException("Nawt");
            }
            this.version++;
            count++;
        }

        public TValue Dequeue()
        {
            if (count == 0)
            {
                throw new InvalidOperationException("Empty");
            }
            SkipListNode<TPriority, TValue> first = head[0];
            for (int i = 0; i < first.AmountOfNodes; i++)
            {
                head[i] = first[i];
            }
            UpdateLevel(currentMaxLevel);
            this.version++;
            count--;
            return first.Value;
        }
        
        public TValue Dequeue(out TPriority priority)
        {
            priority = head[0].Key;
            return Dequeue();
        }

        public TValue Peek()
        {
            if (count == 0)
            {
                throw new InvalidOperationException("Empty");
            }
            return head[0].Value;
        }

        public TPriority PeekPriority()
        {
            if (count == 0)
            {
                throw new InvalidOperationException("Empty");
            }
            return head[0].Key;
        }
        
        public bool Contains(TValue value, TPriority priority)
        {
            if (priority == null)
            {
                throw new ArgumentNullException();
            }

            SkipListNode<TPriority, TValue> currentNode = head;

            for (int level = currentMaxLevel - 1; level >= 0; level--)
            {
                while (currentNode[level] != head &&
                    comparer.Compare(priority, currentNode[level].Key) > 0)
                    currentNode = currentNode[level];
            }

            currentNode = currentNode[0];
            
            while (comparer.Compare(priority, currentNode.Key) == 0)
            {
                if (EqualityComparer<TValue>.Default.Equals(value,currentNode.Value))
                {
                    return true;
                }
                currentNode = currentNode[0];
            }
            return false;
        }

        public bool Contains(TValue value)
        {
            SkipListNode<TPriority, TValue> currentNode = head;
            while (currentNode[0] != head)
            {
                if (EqualityComparer<TValue>.Default.Equals(value,currentNode.Value))
                {
                    return true;
                }
                currentNode = currentNode[0];
            }
            return false;
        }

        public bool Contains(TValue value, out TPriority priority)
        {
            SkipListNode<TPriority, TValue> currentNode = head;
            while (currentNode[0] != head)
            {
                if (EqualityComparer<TValue>.Default.Equals(value, currentNode.Value))
                {
                    priority = currentNode.Key;
                    return true;
                }
            }
            priority = currentNode.Key;
            return false;
        }

        public bool Remove(TValue value, TPriority priority)
        {
            if (priority == null)
            {
                throw new ArgumentNullException();
            }

            SkipListNode<TPriority, TValue> currentNode = head;
            SkipListNode<TPriority, TValue>[] toUpdate =
                new SkipListNode<TPriority, TValue>[currentMaxLevel];

            for (int level = currentMaxLevel - 1; level >= 0; level--)
            {
                while (currentNode[level] != head &&
                    comparer.Compare(priority, currentNode[level].Key) > 0)
                    currentNode = currentNode[level];
                toUpdate[level] = currentNode;
            }

            currentNode = currentNode[0];

            while (comparer.Compare(priority, currentNode.Key) == 0)
            {
                if (EqualityComparer<TValue>.Default.Equals(value, currentNode.Value))
                {
                    for (int i = 0; i < currentNode.AmountOfNodes; i++)
                    {
                        toUpdate[i][i] = currentNode[i];
                        currentNode[i] = null;
                    }
                    UpdateLevel(currentMaxLevel);
                    count--;
                    this.version++;
                    return true;
                }                
                for (int i = 0; i < currentNode.AmountOfNodes; i++)
                {
                    toUpdate[i] = currentNode;
                }

                currentNode = currentNode[0];
            }
            return false;
        }
        
        public void Clear()
        {
            for (int i = 0; i < head.AmountOfNodes; i++)
            {
                head[i] = head;
            }
            this.version++;
            count = 0;
        }
        
        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TPriority, TValue>> GetEnumerator()
        {
            if (count == 0)
                yield break;

            var current = head;
            do
            {
                current = current[0];
                yield return new KeyValuePair<TPriority, TValue>(current.Key, current.Value);
            }
            while(current != null);
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            foreach (KeyValuePair<TPriority, TValue> item in this)
            {
                array.SetValue(item.Value, index++);
            }
        }

        public int Count
        {
            get { return count; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return this; }
        }

        #endregion

    }
}
