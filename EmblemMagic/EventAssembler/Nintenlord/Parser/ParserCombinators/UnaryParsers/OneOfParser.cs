using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Parser.ParserCombinators.UnaryParsers
{
    public sealed class OneOfParser<T> : Parser<T, T>
    {
        readonly ICollection<T> validValues;

        public OneOfParser(ICollection<T> validValues)
        {
            if (validValues == null)
                throw new ArgumentNullException("validValues");
            this.validValues = validValues;
        }


        protected override T ParseMain(IO.Scanners.IScanner<T> scanner, out Match<T> match)
        {
            T val = scanner.Current;

            if (validValues.Contains(val))
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
