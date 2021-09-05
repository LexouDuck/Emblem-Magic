using System;
using System.Collections.Generic;
using System.Text;

namespace Nintenlord.Utility
{
    public static class TypeHelpers
    {
        public static bool InheritsFrom(this Type type, Type inheritsFrom)
        {
            while (type != null)
            {
                if (type == inheritsFrom)
                {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
        }

    }
}
