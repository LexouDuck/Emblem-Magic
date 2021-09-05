using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nintenlord.IO.Scanners;

namespace Nintenlord.Parser.ParserCombinators.UnaryParsers
{
    public sealed class AlwaysParser<T> : Parser<T, T>
    {
        protected override T ParseMain(IScanner<T> scanner, out Match<T> match)
        {
            match = new Match<T>(scanner, 1);
            T result = scanner.Current;
            scanner.MoveNext();
            return result;
        }
    }
}
