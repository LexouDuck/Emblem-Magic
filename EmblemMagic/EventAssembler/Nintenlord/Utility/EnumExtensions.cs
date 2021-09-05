using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Utility
{
    public static class EnumExtensions
    {
        public static bool TryGetEnum<T>(this string name, out T result)
        {
            Type type = typeof(T);
            if (!Enum.GetNames(type).Contains(name))
            {
                result = default(T); 
                return false;
            }

            result = (T)Enum.Parse(type, name);
            return true;
        }
    }
}
