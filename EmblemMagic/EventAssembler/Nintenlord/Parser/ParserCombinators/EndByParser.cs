using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nintenlord.IO.Scanners;

namespace Nintenlord.Parser.ParserCombinators
{
    public sealed class EndByParser<TIn, TEnd, TOut> : RepeatingParser<TIn, TOut>
    {
        readonly IParser<TIn, TOut> results;
        readonly IParser<TIn, TEnd> separator;

        public EndByParser(IParser<TIn, TOut> results, IParser<TIn, TEnd> separator)
        {
            if (results == null)
                throw new ArgumentNullException("results");
            if (separator == null)
                throw new ArgumentNullException("separator");
            this.results = results;
            this.separator = separator;
        }

        protected override IEnumerable<TOut> Enumerate(IScanner<TIn> scanner)
        {
            while (true)
            {
                Match<TIn> latestMatch;
                TOut outRes = results.Parse(scanner, out latestMatch);
                if (latestMatch.Success)
                {
                    InnerMatch += latestMatch;
                    yield return outRes;
                }
                else yield break;

                separator.Parse(scanner, out latestMatch);
                InnerMatch += latestMatch;

                if (!latestMatch.Success)
                    yield break;
            }
        }
    }
}
