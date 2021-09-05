using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Forms.Vectors
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
