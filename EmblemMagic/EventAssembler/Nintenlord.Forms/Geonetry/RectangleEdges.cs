using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Forms.Utility
{
    [Flags]
    public enum RectangleEdges
    {
        None=0,
        Top=1,
        Bottom=2,
        HorizontalEdges=3,
        Left=4,
        TopLeft=5,
        BottomLeft=6,
        AllButRight=7,
        Right=8,
        TopRight=9,
        BottomRight=10,
        AllButLeft=11,
        VerticalEdges = 12,
        AllButBottom=13,
        AllButTop=14,
        All=15
    }
}
