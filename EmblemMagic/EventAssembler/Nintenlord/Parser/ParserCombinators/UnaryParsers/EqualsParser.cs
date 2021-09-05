using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Parser.ParserCombinators.UnaryParsers
{
    public sealed class EqualsParser<T> : Parser<T, T>
        where T : IEquatable<T>
    {
        readonly T item;
        public EqualsParser(T item)
        {
            this.item = item;
        }

        protected override T ParseMain(IO.Scanners.IScanner<T> scanner, out Match<T> match)
        {
            T result = scanner.Current;
            if (!item.Equals(result))
            {
                match = new Match<T>(scanner, "Expected {0}, got {1}", item, result);
                return default(T);
            }
            match = new Match<T>(scanner, 1);
            scanner.MoveNext();
            return result;
        }
    }
}
