using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Utility
{
    public sealed class DisjointPair<T1, T2>
    {
        readonly bool hasFirst;
        readonly T1 first;
        readonly T2 second;

        public bool HasFirst
        {
            get { return hasFirst; }
        }
        public bool HasSecond
        {
            get { return !hasFirst; }
        }
        public T1 First
        {
            get 
            {
                if (!hasFirst)
                    throw new InvalidOperationException();

                return first;
            }
        }
        public T2 Second
        {
            get
            {
                if (hasFirst)
                    throw new InvalidOperationException();

                return second;
            }
        }

        public DisjointPair(T1 item)
        {
            hasFirst = true;
            first = item;
            second = default(T2);
        }

        public DisjointPair(T2 item)
        {
            hasFirst = false;
            first = default(T1);
            second = item;
        }

        public static implicit operator DisjointPair<T1, T2>(T1 item)
        {
            return new DisjointPair<T1, T2>(item);
        }
        public static implicit operator DisjointPair<T1, T2>(T2 item)
        {
            return new DisjointPair<T1, T2>(item);
        }
        public static explicit operator T1(DisjointPair<T1, T2> item)
        {
            return item.First;
        }
        public static explicit operator T2(DisjointPair<T1, T2> item)
        {
            return item.Second;
        }

        public void Apply(Action<T1> first, Action<T2> second)
        {
            if (hasFirst)
            {
                first(this.first);
            }
            else
            {
                second(this.second);
            }
        }

        public T Apply<T>(Func<T1, T> first, Func<T2, T> second)
        {
            return hasFirst ? first(this.first) : second(this.second);
        }
    }
}
