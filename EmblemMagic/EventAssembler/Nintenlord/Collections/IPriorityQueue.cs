using System;
using System.Collections;
using System.Collections.Generic;

namespace Nintenlord.Collections
{
    public interface IPriorityQueue<TPriority, TValue> : 
        IEnumerable<KeyValuePair<TPriority, TValue>>, ICollection
    {
        TValue Dequeue();
        TValue Dequeue(out TPriority priority);
        //TValue DequeueLast();
        //TValue DequeueLast(out TPriority priority);

        void Enqueue(TValue value, TPriority priority);
        TValue Peek();
        TPriority PeekPriority();
        bool Contains(TValue value, TPriority priority);
        bool Contains(TValue value);
        bool Contains(TValue value, out TPriority priority);
        bool Remove(TValue value, TPriority priority);
        void Clear();

        //void RemoveEach(Predicate<TValue> test);
    }
}
