using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nintenlord.IO.Scanners;

namespace Nintenlord.Parser.ParserCombinators
{
    public sealed class SeparatedByParser<TIn, TSeb, TOut> : RepeatingParser<TIn, TOut>
    {
        readonly IParser<TIn, TSeb> separator;
        readonly IParser<TIn, TOut> results;

        public SeparatedByParser(IParser<TIn, TSeb> results, IParser<TIn, TOut> separated)
        {
            if (results == null)
                throw new ArgumentNullException("results");
            if (separated == null)
                throw new ArgumentNullException("separated");
            this.results = separated;
            this.separator = results;
        }

        protected override IEnumerable<TOut> Enumerate(IScanner<TIn> scanner)
        {
            while (true)
            {
                Match<TIn> latestMatch;
                TOut prim = results.Parse(scanner, out latestMatch);
                InnerMatch += latestMatch;

                if (latestMatch.Success)
                    yield return prim;
                else
                    yield break;

                separator.Parse(scanner, out latestMatch);
                if (latestMatch.Success)
                    InnerMatch += latestMatch;
                else yield break;
            }
        }
    }
}
