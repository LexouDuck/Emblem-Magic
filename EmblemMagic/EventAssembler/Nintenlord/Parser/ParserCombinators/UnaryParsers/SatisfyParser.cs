using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nintenlord.IO.Scanners;

namespace Nintenlord.Parser.ParserCombinators.UnaryParsers
{
    public sealed class SatisfyParser<T> : Parser<T,T>
    {
        readonly Predicate<T> comparer;

        public SatisfyParser(Predicate<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            this.comparer = comparer;
        }

        protected override T ParseMain(IScanner<T> scanner, out Match<T> match)
        {
            T unit = scanner.Current;
            if (comparer(unit))
            {
                match = new Match<T>(scanner, 1);
                scanner.MoveNext();
                return unit;
            }
            else
            {
                match = new Match<T>(scanner, "Predicate failed, got {0}", unit.ToString());
                return default(T);
            }
        }

        public override string ToString()
        {
            return "Predicate";
        }
    }
}
