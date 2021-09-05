using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.ROMHacking.Vectors
{
    interface IPrimitiveVector<T>
    {
        T Primitive
        {
            get;
            set;
        }
    }
}
