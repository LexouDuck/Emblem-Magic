using System;
using System.Collections.Generic;

namespace Nintenlord.Collections.Dictionaries
{
    /// <summary>
    /// Enumerates an enumerable of keys and enumerable of values 
    /// as a enumerable of keys and values.
    /// </summary>
    /// <typeparam name="TKey">Keys of the enumerable to enumerate.</typeparam>
    /// <typeparam name="TValue">Values of the enumerable to enumerate.</typeparam>
    /// <typeparam name="TEnumerable">The enumerable type to enumerate.</typeparam>
    /// <remarks>Third type parameter is ugly, but necessary due to covariance restraints.</remarks>
    public class DictionaryOfEnumerablesEnumarator<TKey, TValue, TEnumerable> :
        IEnumerator<KeyValuePair<TKey, TValue>> where TEnumerable : IEnumerable<TValue>
    {
        IEnumerator<KeyValuePair<TKey, TEnumerable>> currentCollection;
        IEnumerator<TValue> currentValue;

        public DictionaryOfEnumerablesEnumarator(
            IEnumerable<KeyValuePair<TKey, TEnumerable>> baseCollection)
        {
            this.currentCollection = baseCollection.GetEnumerator();
        }

        #region IEnumerator<KeyValuePair<TKey,TValue>> Members

        public KeyValuePair<TKey, TValue> Current
        {
            get 
            {
                return new KeyValuePair<TKey, TValue>(
                    currentCollection.Current.Key, currentValue.Current
                    );
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            currentCollection.Dispose();
            currentCollection = null;
            currentValue.Dispose();
            currentValue = null;
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IEnumerator Members

        object System.Collections.IEnumerator.Current
        {
            get { return this.Current; }
        }

        public bool MoveNext()
        {
            if (currentValue != null &&
                currentValue.MoveNext())
            {
                return true;
            }
            else
            {
                if (currentCollection.MoveNext())
                {
                    if (currentValue != null)
                        currentValue.Dispose();

                    currentValue = currentCollection.Current.Value.GetEnumerator();
                    while (!currentValue.MoveNext())
                    {
                        if (currentCollection.MoveNext())
                        {
                            currentValue.Dispose();
                            currentValue = currentCollection.Current.Value.GetEnumerator();  
                        }
                        else
                        {
                            return false;
                        }                      
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void Reset()
        {
            currentCollection.Reset();
            if (currentValue != null)
            {
                currentValue.Dispose();
                currentValue = null;
            }
        }

        #endregion
    }
}
