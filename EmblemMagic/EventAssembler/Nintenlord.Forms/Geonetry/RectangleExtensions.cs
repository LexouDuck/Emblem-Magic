using System;
using System.Drawing;
using System.Collections.Generic;
using Nintenlord.Utility.Primitives;

namespace Nintenlord.Forms.Utility
{
    public static class RectangleExtensions
    {
        public static void ClingInside(ref Rectangle area, ref Rectangle smallRect, int distance)
        {
            if (smallRect.Top - area.Top <= distance)
            {
                smallRect.Y = area.Y;
            }
            else if (area.Bottom - smallRect.Bottom <= distance)
            {
                smallRect.Y = area.Bottom - smallRect.Height;
            }

            if (smallRect.Left - area.Left <= distance)
            {
                smallRect.X = area.X;
            }
            else if (area.Right - smallRect.Right <= distance)
            {
                smallRect.X = area.Right - smallRect.Width;
            }
        }

        public static void ClingInside2(ref Rectangle area, ref Rectangle smallRect, int distance)
        {
            if (Math.Abs(smallRect.Left - area.Left) <= distance)
            {
                smallRect.X = area.Left;
            }
            else if (Math.Abs(smallRect.Right - area.Right) <= distance)
            {
                smallRect.X = area.Right - smallRect.Width;
            }

            if (Math.Abs(smallRect.Top - area.Top) <= distance)
            {
                smallRect.Y = area.Top;
            }
            else if (Math.Abs(smallRect.Bottom - area.Bottom) <= distance)
            {
                smallRect.Y = area.Bottom - smallRect.Height;
            }
        }

        public static void Cling(ref Rectangle area, ref Rectangle smallRect, int distance)
        {
            if (Math.Abs(smallRect.Right - area.Left) <= distance)
            {
                smallRect.X = area.X - smallRect.Width;
            }
            else if (Math.Abs(smallRect.Left - area.Right) <= distance)
            {
                smallRect.X = area.Right;
            }

            if (Math.Abs(smallRect.Right - area.Left) <= distance)
            {
                smallRect.Y = area.Y - smallRect.Width;
            }
            else if (Math.Abs(smallRect.Top - area.Bottom) <= distance)
            {
                smallRect.Y = area.Bottom;
            }
        }

        public static void ClingIfCloser(ref Rectangle areaToClingTo, ref Rectangle area, int distance)
        {
            //RectangleEdges closeEdges = WhichEdgesAreClose(ref areaToClingTo, ref area, distance);
            //if ((closeEdges & RectangleEdges.Top) == RectangleEdges.Top)
            //{
            //    ClingToEdges(ref areaToClingTo, ref area, RectangleEdges.Top);
            //}

            //if ((closeEdges & RectangleEdges.Bottom) == RectangleEdges.Bottom)
            //{
            //    ClingToEdges(ref areaToClingTo, ref area, RectangleEdges.Bottom);
            //}

            //if ((closeEdges & RectangleEdges.Left) == RectangleEdges.Left)
            //{
            //    ClingToEdges(ref areaToClingTo, ref area, RectangleEdges.Left);
            //}

            //if ((closeEdges & RectangleEdges.Right) == RectangleEdges.Right)
            //{
            //    ClingToEdges(ref areaToClingTo, ref area, RectangleEdges.Right);
            //}
        }
        
        public static void ClingToEdges(ref Rectangle areaToClingTo, ref Rectangle area, RectangleEdges edge, RectangleEdges edgeToClingTo)
        {
            if ((edge & RectangleEdges.Top) == RectangleEdges.Top)
            {
                area.Y = areaToClingTo.Y;
            }
            if ((edge & RectangleEdges.Bottom) == RectangleEdges.Bottom)
            {
                area.Y = areaToClingTo.Y + areaToClingTo.Height - area.Height;
            }
            if ((edge & RectangleEdges.Left) == RectangleEdges.Left)
            {
                area.X = areaToClingTo.X;
            }
            if ((edge & RectangleEdges.Right) == RectangleEdges.Right)
            {
                area.X = areaToClingTo.X + areaToClingTo.Width - area.Width;
            }
        }

        public static KeyValuePair<RectangleEdges, RectangleEdges>[] 
            WhichEdgesAreClose(ref Rectangle area1, ref Rectangle area2, int distance)
        {
            RectangleEdges result = RectangleEdges.None;

            if ((Math.Abs(area1.Top - area2.Top) <= distance ||
                Math.Abs(area1.Top - area2.Bottom) <= distance) &&
                RangesCollide(area1.Left,area1.Right, area1.Left,area1.Right))
            {
                result |= RectangleEdges.Top;
            }

            if ((Math.Abs(area1.Bottom - area2.Bottom) <= distance ||
                Math.Abs(area1.Bottom - area2.Top) <= distance) &&
                RangesCollide(area1.Left, area1.Right, area1.Left, area1.Right))
            {
                result |= RectangleEdges.Bottom;
            }

            if ((Math.Abs(area1.Left - area2.Left) <= distance ||
                Math.Abs(area1.Left - area2.Right) <= distance) &&
                RangesCollide(area1.Top, area1.Bottom, area1.Top, area1.Bottom))
            {
                result |= RectangleEdges.Left;
            }

            if ((Math.Abs(area1.Right - area2.Right) <= distance ||
                Math.Abs(area1.Right - area2.Left) <= distance) &&
                RangesCollide(area1.Top, area1.Bottom, area1.Top, area1.Bottom))
            {
                result |= RectangleEdges.Right;
            }

            return null;
        }

        public static bool RangesCollide(int min1, int max1, int min2, int max2)
        {
            return ((min1 <= min2 && max1 >= max2) ||
                   min1.IsInRange(min2, max2) ||
                   max1.IsInRange(min2, max2));
        }
    }
}