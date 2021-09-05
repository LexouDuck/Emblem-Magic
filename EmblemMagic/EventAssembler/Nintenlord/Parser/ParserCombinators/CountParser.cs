using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nintenlord.IO.Scanners;

namespace Nintenlord.Parser.ParserCombinators
{
    public sealed class CountParser<TIn, TOut> : RepeatingParser<TIn, TOut>
    {
        readonly IParser<TIn, TOut> parser;
        readonly int count;

        public CountParser(IParser<TIn, TOut> parser, int count)
        {
            if (parser == null)
                throw new ArgumentNullException("parser");
            this.parser = parser;
            this.count = count;
        }

        protected override IEnumerable<TOut> Enumerate(IScanner<TIn> scanner)
        {
            for (int i = 0; i < count; i++)
            {
                Match<TIn> latestMatch;
                var temp = parser.Parse(scanner, out latestMatch);
                InnerMatch += latestMatch;

                if (!latestMatch.Success)
                    yield break;

                yield return temp;
            }
        }
    }
}
