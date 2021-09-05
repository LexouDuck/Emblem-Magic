using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Nintenlord.Collections.Dictionaries
{
    [Serializable]
    class SerializableDictionary<Tkey, TValue> : ISerializable
    {
        private const string TypeName = "Type";
        private const string ValuesName = "Values";

        IDictionary<Tkey, TValue> baseDictionary;

        public IDictionary<Tkey, TValue> Base
        {
            get { return baseDictionary; }
        }

        public SerializableDictionary()
        {
            baseDictionary = new Dictionary<Tkey, TValue>();
        }

        public SerializableDictionary(IDictionary<Tkey, TValue> baseDictionary)
        {
            this.baseDictionary = baseDictionary;
        }
        
        #region ISerializable Members

        public SerializableDictionary(SerializationInfo info, StreamingContext context)
        {
            Type dictType = info.GetValue(TypeName, typeof(Type)) as Type;
            baseDictionary = dictType.TypeInitializer.Invoke(null) as IDictionary<Tkey, TValue>;
            KeyValuePair<Tkey, TValue>[] values = info.GetValue(ValuesName,
                typeof(KeyValuePair<Tkey, TValue>[])) as KeyValuePair<Tkey, TValue>[];
            foreach (var item in values)
            {
                baseDictionary.Add(item);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(TypeName, baseDictionary.GetType());
            info.AddValue(ValuesName, baseDictionary.ToArray());
        }

        #endregion
    }
}
