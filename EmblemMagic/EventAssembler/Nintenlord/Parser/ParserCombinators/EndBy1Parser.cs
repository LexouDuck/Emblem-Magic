using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nintenlord.IO.Scanners;

namespace Nintenlord.Parser.ParserCombinators
{
    public sealed class EndBy1Parser<TIn, TEnd, TOut> : RepeatingParser<TIn, TOut>
    {
        readonly IParser<TIn, TOut> results;
        readonly IParser<TIn, TEnd> separator;

        public EndBy1Parser(IParser<TIn, TOut> results, IParser<TIn, TEnd> separator)
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
            Match<TIn> latestMatch;
            TOut prim = results.Parse(scanner, out latestMatch);
            InnerMatch += latestMatch;
            if (latestMatch.Success)
            {
                yield return prim;
                while (true)
                {
                    separator.Parse(scanner, out latestMatch);
                    InnerMatch += latestMatch;

                    if (!latestMatch.Success)
                        yield break;

                    prim = results.Parse(scanner, out latestMatch);
                    if (latestMatch.Success)
                    {
                        InnerMatch += latestMatch;
                        yield return prim;
                    }
                    else yield break;
                }
            }
        }
    }
}
