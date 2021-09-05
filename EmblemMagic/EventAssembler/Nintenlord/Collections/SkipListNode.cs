using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Collections
{
    public class SkipListNode<TKey, TValue>
    {
        TKey key;
        TValue value;
        SkipListNode<TKey, TValue>[] next;

        public TKey Key
        {
            get { return key; }
            internal set { key = value; }
        }
        public TValue Value
        {
            get { return value; }
            internal set { this.value = value; }
        }
        public SkipListNode<TKey, TValue> this[int index]
        {
            get { return next[index]; }
            internal set { next[index] = value; }
        }
        public int AmountOfNodes
        {
            get { return next.Length; }
        }

        internal SkipListNode(int amountOfNodes)
        {
            next = new SkipListNode<TKey, TValue>[amountOfNodes];
        }

        internal SkipListNode(TKey key, TValue value, int amountOfNodes)
        {
            next = new SkipListNode<TKey, TValue>[amountOfNodes];
            this.value = value;
            this.key = key;
        }

        public static explicit operator KeyValuePair<TKey, TValue>(SkipListNode<TKey, TValue> i)
        {
            return new KeyValuePair<TKey, TValue>(i.key, i.value);
        }

        internal bool Validate()
        {
            return this.next.All(t => t != this);
        }
    }
}
