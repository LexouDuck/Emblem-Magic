using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Utility
{
    public static class FunctionExtensions
    {
        #region Func

        public static T Apply<T, TIn1, TIn2>(this Tuple<TIn1, TIn2> tupple, Func<TIn1, TIn2, T> func)
        {
            return func.Invoke(tupple.Item1, tupple.Item2);
        }

        public static T Apply<T, TIn1, TIn2, TIn3>(this Tuple<TIn1, TIn2, TIn3> tupple, Func<TIn1, TIn2, TIn3, T> func)
        {
            return func.Invoke(tupple.Item1, tupple.Item2, tupple.Item3);
        }

        public static T Apply<T, TIn1, TIn2, TIn3, TIn4>(this Tuple<TIn1, TIn2, TIn3, TIn4> tupple, Func<TIn1, TIn2, TIn3, TIn4, T> func)
        {
            return func.Invoke(tupple.Item1, tupple.Item2, tupple.Item3, tupple.Item4);
        }

        public static T Apply<T, TIn1, TIn2, TIn3, TIn4, TIn5>(this Tuple<TIn1, TIn2, TIn3, TIn4, TIn5> tupple,
            Func<TIn1, TIn2, TIn3, TIn4, TIn5, T> func)
        {
            return func.Invoke(tupple.Item1, tupple.Item2, tupple.Item3, tupple.Item4, tupple.Item5);
        }

        public static T Apply<T, TIn1, TIn2, TIn3, TIn4, TIn5, TIn6>(this Tuple<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6> tupple,
            Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, T> func)
        {
            return func.Invoke(tupple.Item1, tupple.Item2, tupple.Item3, tupple.Item4, tupple.Item5, tupple.Item6);
        }

        public static T Apply<T, TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7>(this Tuple<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7> tupple,
            Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, T> func)
        {
            return func.Invoke(tupple.Item1, tupple.Item2, tupple.Item3, tupple.Item4, tupple.Item5, tupple.Item6, tupple.Item7);
        }



        public static T Apply<T, TIn1, TIn2>(this Func<TIn1, TIn2, T> func, Tuple<TIn1, TIn2> tupple)
        {
            return func(tupple.Item1, tupple.Item2);
        }

        public static T Apply<T, TIn1, TIn2, TIn3>(this Func<TIn1, TIn2, TIn3, T> func, Tuple<TIn1, TIn2, TIn3> tupple)
        {
            return func(tupple.Item1, tupple.Item2, tupple.Item3);
        }

        public static T Apply<T, TIn1, TIn2, TIn3, TIn4>(this Func<TIn1, TIn2, TIn3, TIn4, T> func, Tuple<TIn1, TIn2, TIn3, TIn4> tupple)
        {
            return func(tupple.Item1, tupple.Item2, tupple.Item3, tupple.Item4);
        }

        public static T Apply<T, TIn1, TIn2, TIn3, TIn4, TIn5>(this Func<TIn1, TIn2, TIn3, TIn4, TIn5, T> func,
            Tuple<TIn1, TIn2, TIn3, TIn4, TIn5> tupple)
        {
            return func(tupple.Item1, tupple.Item2, tupple.Item3, tupple.Item4, tupple.Item5);
        }

        public static T Apply<T, TIn1, TIn2, TIn3, TIn4, TIn5, TIn6>(this Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, T> func,
            Tuple<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6> tupple)
        {
            return func(tupple.Item1, tupple.Item2, tupple.Item3, tupple.Item4, tupple.Item5, tupple.Item6);
        }

        public static T Apply<T, TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7>(this Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, T> func,
            Tuple<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7> tupple)
        {
            return func(tupple.Item1, tupple.Item2, tupple.Item3, tupple.Item4, tupple.Item5, tupple.Item6, tupple.Item7);
        }

        #endregion

        #region Action

        public static void Apply<TIn1, TIn2>(this Tuple<TIn1, TIn2> tupple, Action<TIn1, TIn2> func)
        {
            func.Invoke(tupple.Item1, tupple.Item2);
        }

        public static void Apply<TIn1, TIn2, TIn3>(this Tuple<TIn1, TIn2, TIn3> tupple, Action<TIn1, TIn2, TIn3> func)
        {
            func.Invoke(tupple.Item1, tupple.Item2, tupple.Item3);
        }

        public static void Apply<TIn1, TIn2, TIn3, TIn4>(this Tuple<TIn1, TIn2, TIn3, TIn4> tupple, Action<TIn1, TIn2, TIn3, TIn4> func)
        {
            func.Invoke(tupple.Item1, tupple.Item2, tupple.Item3, tupple.Item4);
        }

        public static void Apply<TIn1, TIn2, TIn3, TIn4, TIn5>(this Tuple<TIn1, TIn2, TIn3, TIn4, TIn5> tupple,
            Action<TIn1, TIn2, TIn3, TIn4, TIn5> func)
        {
            func.Invoke(tupple.Item1, tupple.Item2, tupple.Item3, tupple.Item4, tupple.Item5);
        }

        public static void Apply<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6>(this Tuple<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6> tupple,
            Action<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6> func)
        {
            func.Invoke(tupple.Item1, tupple.Item2, tupple.Item3, tupple.Item4, tupple.Item5, tupple.Item6);
        }

        public static void Apply<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7>(this Tuple<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7> tupple,
            Action<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7> func)
        {
            func.Invoke(tupple.Item1, tupple.Item2, tupple.Item3, tupple.Item4, tupple.Item5, tupple.Item6, tupple.Item7);
        }




        public static void Apply<TIn1, TIn2>(this Action<Tuple<TIn1, TIn2>> func, TIn1 arg1, TIn2 arg2)
        {
            func(Tuple.Create(arg1, arg2));
        }

        
        #endregion



        public static Func<TIn1, Func<TIn2, T>> Curry<T, TIn1, TIn2>(Func<TIn1, TIn2, T> func)
        {
            return x => y => func(x, y);
        }

        public static Func<TIn1, Func<TIn2, Func<TIn3, T>>> Curry<T, TIn1, TIn2, TIn3>(Func<TIn1, TIn2, TIn3, T> func)
        {
            return x => y => z => func(x, y, z);
        }

        public static Func<TIn1, Func<TIn2, Func<TIn3, Func<TIn4, T>>>> Curry<T, TIn1, TIn2, TIn3, TIn4>(
            Func<TIn1, TIn2, TIn3, TIn4, T> func)
        {
            return x => y => z => w => func(x, y, z, w);
        }

        public static Func<TIn1, Func<TIn2, Func<TIn3, Func<TIn4, Func<TIn5, T>>>>> Curry<T, TIn1, TIn2, TIn3, TIn4, TIn5>(
            Func<TIn1, TIn2, TIn3, TIn4, TIn5, T> func)
        {
            return x => y => z => w => f => func(x, y, z, w, f);
        }

        public static Func<TIn1, Func<TIn2, Func<TIn3, Func<TIn4, Func<TIn5, Func<TIn6, T>>>>>> Curry<T, TIn1, TIn2, TIn3, TIn4, TIn5, TIn6>(
            Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, T> func)
        {
            return x => y => z => w => f => e => func(x, y, z, w, f, e);
        }

        public static Func<TIn1, Func<TIn2, Func<TIn3, Func<TIn4, Func<TIn5, Func<TIn6, Func<TIn7, T>>>>>>> 
            Curry<T, TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7>(
            Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, T> func)
        {
            return x => y => z => w => f => e => g => func(x, y, z, w, f, e, g);
        }


        public static Tuple<T1, T2, T3> Flatten<T1, T2, T3>(this Tuple<T1, Tuple<T2, T3>> tuple)
        {
            return Tuple.Create(tuple.Item1, tuple.Item2.Item1, tuple.Item2.Item2);
        }

        public static Tuple<T1, T2, T3> Flatten<T1, T2, T3>(this Tuple<Tuple<T1, T2>, T3> tuple)
        {
            return Tuple.Create(tuple.Item1.Item1, tuple.Item1.Item2, tuple.Item2);
        }

        public static Action ApplyResult<T>(this Func<T> calculation, Action<T> application)
        {
            return () => application(calculation());
        }

        public static Action<T> Concatenate<T>(params Action<T>[] actions)
        {
            return x =>
                {
                    foreach (var action in actions)
                    {
                        action(x);
                    }
                };
        }
    }
}
