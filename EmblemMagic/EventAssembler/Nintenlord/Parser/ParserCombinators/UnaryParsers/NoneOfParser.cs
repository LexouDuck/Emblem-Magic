using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Parser.ParserCombinators.UnaryParsers
{
    public sealed class NoneOfParser<T> : Parser<T, T>
    {
        ICollection<T> invalidValues;

        public NoneOfParser(ICollection<T> invalidValues)
        {
            if (invalidValues == null)
                throw new ArgumentNullException("invalidValues");

            this.invalidValues = invalidValues;
        }


        protected override T ParseMain(IO.Scanners.IScanner<T> scanner, out Match<T> match)
        {
            T val = scanner.Current;

            if (!invalidValues.Contains(val))
            {
                match = new Match<T>(scanner, 1);
                scanner.MoveNext();
            }
            else
            {
                match = new Match<T>(scanner, "Invalid value {0}", val);
                val = default(T);
            }

            return val;
        }
    }
}
