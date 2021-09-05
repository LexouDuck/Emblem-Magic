// -----------------------------------------------------------------------
// <copyright file="Reference.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Nintenlord.Utility
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public sealed class Reference<T> where T : struct
    {
        public T Value
        {
            get;
            set;
        }

        public Reference(T value)
        {
            this.Value = value;
        }
    }

    public static class ReferenceHelpers
    {
        public static Reference<TOut> Map<TIn, TOut>(this Func<TIn, TOut> f, Reference<TIn> input)
            where TIn : struct
            where TOut : struct
        {
            return new Reference<TOut>(f(input.Value));
        }


        public static Reference<C> SelectMany<A, B, C>(
            this Reference<A> a, Func<A, Reference<B>> func, Func<A, B, C> select)
            where A : struct
            where B : struct
            where C : struct
        {
            var b = func(a.Value);

            return new Reference<C>(select(a.Value, b.Value));            
        }

        public static Reference<TOut> Select<TIn, TOut>(
            this Reference<TIn> x, Func<TIn, TOut> f)
            where TIn : struct
            where TOut : struct
        {
            return f.Map(x);
        }
    }
}
