using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Parser.ParserCombinators.UnaryParsers
{
    public sealed class NameParser<TIn,TOut> : Parser<TIn,TOut>
    {
        readonly string name;
        readonly IParser<TIn, TOut> parser;

        public NameParser(IParser<TIn, TOut> parser, string name)
        {
            if (parser == null)
                throw new ArgumentNullException("parser");
            if (name == null)
                throw new ArgumentNullException("name");

            this.parser = parser;
            this.name = name;
        }

        protected override TOut ParseMain(IO.Scanners.IScanner<TIn> scanner, out Match<TIn> match)
        {
            TOut result = parser.Parse(scanner, out match);
            if (!match.Success)
            {
                match = new Match<TIn>(scanner, 
                    result == null ? "Expected {0}" : "Expected {0}, got {1}", name, result);
                result = default(TOut);
            }

            return result;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
