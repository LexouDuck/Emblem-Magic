using System;
using System.Collections.Generic;

namespace Nintenlord.Utility
{
    public sealed class CanCauseError<T, TError>
    {
        bool error;
        T result;
        TError errorState;
        public bool CausedError
        {
            get { return error; }
        }
        public T Result
        {
            get
            {
                if (error)
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    return result;
                }
            }
        }
        public TError ErrorState
        {
            get
            {
                if (!error)
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    return errorState;
                }
            }

        }

        public static CanCauseError<T, TError> NoError(T result)
        {
            var results = new CanCauseError<T, TError> {result = result, error = false};
            return results;
        }

        public static CanCauseError<T, TError> Error(TError error)
        {
            var result = new CanCauseError<T, TError> {error = true, errorState = error};
            return result;
        }

        public static implicit operator CanCauseError<T, TError>(T value)
        {
            return NoError(value);
        }
    }

    /// <summary>
    /// Represents a computation that can succesfully return a value or fail with error string.
    /// </summary>
    /// <typeparam name="T">Type of succesful value</typeparam>
    public sealed class CanCauseError<T>
    {
        private static readonly Dictionary<string, CanCauseError<T>> cachedErrors 
            = new Dictionary<string, CanCauseError<T>>();

        T result;
        bool error;
        Lazy<string> errorMessage;
        public bool CausedError
        {
            get { return error; }
        }
        public string ErrorMessage
        {
            get
            {
                if (error)
                {
                    return errorMessage.Value;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }
        public T Result
        {
            get 
            {
                if (error)
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    return result;
                }
            }
        }

        public override string ToString()
        {
            return error ? string.Format("Error: {0}", errorMessage) : string.Format("Success: {0}", result);
        }

        public CanCauseError<TOut> ConvertError<TOut>()
        {
            if (!this.error)
            {
                throw new InvalidOperationException();
            }
            var result = new CanCauseError<TOut>
                {
                    error = this.error,
                    result = default(TOut),
                    errorMessage = this.errorMessage
                };
            return result;
        }

        public static CanCauseError<T> NoError(T result)
        {
            CanCauseError<T> results = new CanCauseError<T> {result = result, error = false};
            return results;
        }

        public static CanCauseError<T> Error(string errorMessages)
        {
            CanCauseError<T> result;
            if (!cachedErrors.TryGetValue(errorMessages, out result))
            {
                result = new CanCauseError<T>
                    {
                        error = true, 
                        errorMessage = new Lazy<string>(() => errorMessages)
                    };
                cachedErrors[errorMessages] = result;
            }
            return result;
        }

        public static CanCauseError<T> Error(string errorMessages, params object[] objects)
        {
            CanCauseError<T> result = new CanCauseError<T>
                {
                    error = true,
                    errorMessage = new Lazy<string>(() => string.Format(errorMessages, objects))
                };

            return result;
        }
        
        public static implicit operator bool(CanCauseError<T> error)
        {
            return error.CausedError;
        }
        
        public static implicit operator CanCauseError<T>(T value)
        {
            return NoError(value);
        }

        public static explicit operator CanCauseError(CanCauseError<T> error)
        {
            return error.CausedError ? CanCauseError.Error(error.errorMessage.Value) : CanCauseError.NoError;
        }

        public static explicit operator CanCauseError<T>(CanCauseError error)
        {
            return error.CausedError ? CanCauseError<T>.Error(error.ErrorMessage) : CanCauseError<T>.NoError(default(T));
        }
    }

    /// <summary>
    /// Represents a computation that can succeed or fail with error string.
    /// </summary>
    public sealed class CanCauseError
    {
        private static readonly CanCauseError noError;
        private static readonly Dictionary<string, CanCauseError> cachedErrors
            = new Dictionary<string, CanCauseError>();

        static CanCauseError()
        {
            noError = new CanCauseError {error = false};
        }

        bool error;
        string errorMessage;
        public bool CausedError
        {
            get { return error; }
        }
        public string ErrorMessage
        {
            get 
            {
                if (error)
                {
                    return errorMessage;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public static CanCauseError NoError
        {
            get { return noError; }
        }

        public static CanCauseError Error(string errorMessages)
        {
            CanCauseError result;
            if (!cachedErrors.TryGetValue(errorMessages, out result))
            {
                result = new CanCauseError {error = true, errorMessage = errorMessages};
                cachedErrors[errorMessages] = result;
            }
            return result;
        }

        public static CanCauseError Error(string errorFormat, params object[] objects)
        {
            return Error(string.Format(errorFormat, objects));
        }
        
        public static implicit operator bool(CanCauseError error)
        {
            return error.CausedError;
        }

        public override string ToString()
        {
            return errorMessage;
        }
    }
    
    public static class CanCauseErrorHelpers
    {
        public static CanCauseError<C> SelectMany<A, B, C>(
            this CanCauseError<A> a, Func<A, CanCauseError<B>> func, Func<A, B, C> select)
        {
            if (a.CausedError)
            {
                return a.ConvertError<C>();
            }
            else
            {
                var b = func(a.Result);
                return b.CausedError ? b.ConvertError<C>() : 
                                       select(a.Result, b.Result);
            }
        }

        public static CanCauseError<C, Error> SelectMany<A, B, C, Error>(
            this CanCauseError<A, Error> a, Func<A, CanCauseError<B, Error>> func, Func<A, B, C> select)
        {
            if (a.CausedError)
            {
                return CanCauseError<C, Error>.Error(a.ErrorState);
            }
            else
            {
                var b = func(a.Result);
                return b.CausedError ? CanCauseError<C, Error>.Error(b.ErrorState) : 
                                       select(a.Result, b.Result);
            }
        }

        public static CanCauseError<TOut> Select<TIn, TOut>(
            this CanCauseError<TIn> x, Func<TIn, TOut> f)
        {
            return f.Map(x);
        }

        public static CanCauseError<T> Where<T>(this CanCauseError<T> error, Func<T, bool> predicate)
        {
            if (error.CausedError || predicate(error.Result))
            {
                return error;
            }
            else
            {
                return CanCauseError<T>.Error("Value failed predicate");
            }
        }

        public static CanCauseError ActionIfSuccess<T>(this Func<CanCauseError<T>> function, Action<T> action)
        {
            var result = function();
            if (result.CausedError)
            {
                return (CanCauseError)result;
            }
            else
            {
                action(result.Result);
                return CanCauseError.NoError;
            }
        }

        public static CanCauseError<TOut> ConvertError<TIn, TOut>(this CanCauseError<TIn> error)
        {
            if (error.CausedError)
            {
                return error.ConvertError<TOut>();
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        public static CanCauseError<T> ExceptionToError<T>(this Func<T> f)
        {
            try
            {
                var result = f();
                return CanCauseError<T>.NoError(result);
            }
            catch (Exception e)
            {
                return CanCauseError<T>.Error(e.Message);
            }
        }

        public static CanCauseError ExceptionToError(this Action f)
        {
            try
            {
                f();
                return CanCauseError.NoError;
            }
            catch (Exception e)
            {
                return CanCauseError.Error(e.Message);
            }
        }

        public static Func<CanCauseError<TIn>, CanCauseError<TOut>> Map<TIn, TOut>(this Func<TIn, TOut> f)
        {
            return f.Map;
        }

        public static Func<CanCauseError<TIn1>, CanCauseError<TIn2>, CanCauseError<TOut>> Map<TIn1, TIn2, TOut>(
            this Func<TIn1, TIn2, TOut> f)
        {
            return f.Map;
        }

        public static CanCauseError<TOut> Map<TIn, TOut>(
            this Func<TIn, TOut> f, CanCauseError<TIn> x)
        {
            return x.CausedError ? x.ConvertError<TOut>() : f(x.Result);
        }

        public static CanCauseError<TOut> Map<TIn1, TIn2, TOut>(
            this Func<TIn1, TIn2, TOut> f, CanCauseError<TIn1> x, CanCauseError<TIn2> y)
        {
            if (x.CausedError)
            {
                return x.ConvertError<TOut>();
            }
            else if (y.CausedError)
            {
                return y.ConvertError<TOut>();
            }
            else
            {
                return f(x.Result, y.Result);
            }
        }

        public static T ValueOrDefault<T>(this CanCauseError<T> error, T defaultValue = default(T))
        {
            return error.CausedError
                       ? defaultValue
                       : error.Result;
        }

        public static T ValueOrDefault<T>(this CanCauseError<T> error, Func<T> defaultValue)
        {
            return error.CausedError
                       ? defaultValue()
                       : error.Result;
        }


        public static CanCauseError<T> Bind<T>(this Func<CanCauseError<T>> first, Func<T, CanCauseError<T>> second)
        {
            CanCauseError<T> firstResult = first();
            return firstResult.CausedError ? firstResult : second(firstResult.Result);
        }

        public static Func<TIn, CanCauseError<TOut>> Bind<TIn, TMiddle, TOut>(this Func<TIn, CanCauseError<TMiddle>> first, Func<TMiddle, CanCauseError<TOut>> second)
        {
            return x =>
            {
                var firstRes = first(x);
                return firstRes.CausedError ? firstRes.ConvertError<TOut>() : second(firstRes.Result);
            };
        }

        public static CanCauseError<T> Bind<T>(this IEnumerable<Func<T, CanCauseError<T>>> functions, T start)
        {
            CanCauseError<T> result = CanCauseError<T>.NoError(start);
            foreach (var func in functions)
            {
                result = func(result.Result);
                if (result.CausedError)
                {
                    break;
                }
            }
            return result;
        }
        
        public static CanCauseError Bind(this Func<CanCauseError> first, Func<CanCauseError> second)
        {
            CanCauseError firstResult = first();
            return firstResult.CausedError ? firstResult : second();
        }

        public static CanCauseError Bind(this IEnumerable<Func<CanCauseError>> functions)
        {
            CanCauseError result = null;
            foreach (var func in functions)
            {
                result = func();
                if (result.CausedError)
                {
                    break;
                }
            }
            return result;
        }

        public static CanCauseError<TValue> TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            TValue value;
            return dict.TryGetValue(key, out value) 
                ? value 
                : CanCauseError<TValue>.Error("No value with key {0} found.", key);
        }
    }
}
