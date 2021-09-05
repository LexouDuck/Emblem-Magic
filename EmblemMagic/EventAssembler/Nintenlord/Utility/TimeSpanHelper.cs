using System;
using System.Collections.Generic;
using System.Text;

namespace Nintenlord.Utility
{
    public static class TimeSpanHelper
    {
        public static float FloatDivide(this TimeSpan timeSpan, TimeSpan divideWith)
        {
            return ((float)timeSpan.Ticks) / ((float)divideWith.Ticks);
        } 

        public static TimeSpan Divide(this TimeSpan timeSpan, int divideWith)
        {
            return TimeSpan.FromTicks(timeSpan.Ticks / divideWith);
        }

        public static int Divide(this TimeSpan timeSpan, TimeSpan divideWith)
        {
            return (int)(timeSpan.Ticks / divideWith.Ticks);
        }

        public static TimeSpan Multiply(this TimeSpan timeSpan, int value)
        {
            return TimeSpan.FromTicks(timeSpan.Ticks * value);
        }

        public static TimeSpan Multiply(this TimeSpan timeSpan, float value)
        {
            return TimeSpan.FromMilliseconds(timeSpan.TotalMilliseconds * value);
        }

        public static TimeSpan Part(this TimeSpan timeSpan, int toMultiply, int toDivide)
        {
            return TimeSpan.FromTicks(timeSpan.Ticks * toMultiply / toDivide);
        }

        public static TimeSpan Lerp(TimeSpan beginning, TimeSpan end, float i)
        {
            return beginning + Multiply((end - beginning) , i);
        }
    }
}
