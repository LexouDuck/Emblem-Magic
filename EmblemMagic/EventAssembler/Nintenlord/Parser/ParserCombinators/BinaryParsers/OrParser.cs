using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nintenlord.IO.Scanners;

namespace Nintenlord.Parser.ParserCombinators.BinaryParsers
{
    public sealed class OrParser<TIn, TOut> : Parser<TIn, TOut>
    {
        readonly IParser<TIn, TOut> first, second;

        public OrParser(IParser<TIn, TOut> first, IParser<TIn, TOut> second)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

            this.first = first;
            this.second = second;
        }

        protected override TOut ParseMain(IScanner<TIn> scanner, out Match<TIn> match)
        {
            var currentOffset = scanner.Offset;
            TOut result = first.Parse(scanner, out match);

            if (!match.Success)
            {
                if (currentOffset == scanner.Offset)
                {
                    result = second.Parse(scanner, out match);
                }
                else
                {
                    result = default(TOut);
                    match = new Match<TIn>(scanner, "First option consumed input.");
                }
            }

            return result;
        }

        public override string ToString()
        {
            return string.Format("({0})|({1})", first, second);
        }
    }
}
