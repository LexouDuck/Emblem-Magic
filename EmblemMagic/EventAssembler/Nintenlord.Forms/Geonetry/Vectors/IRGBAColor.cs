using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.ROMHacking.Vectors
{
    interface IRGBAColor : IRGBColor
    {
        int Alpha
        {
            get;
            set;
        }
    }
}
