using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nintenlord.IO.Scanners;

namespace Nintenlord.Parser.ParserCombinators
{
    public sealed class ManyTillParser<TIn, TEnd, TOut> : RepeatingParser<TIn, TOut>
    {
        readonly IParser<TIn, TOut> results;
        readonly IParser<TIn, TEnd> ender;

        public ManyTillParser(IParser<TIn, TOut> results, IParser<TIn, TEnd> ender)
        {
            if (results == null)
                throw new ArgumentNullException("results");
            if (ender == null)
                throw new ArgumentNullException("ender");
            this.results = results;
            this.ender = ender;
        }

        protected override IEnumerable<TOut> Enumerate(IScanner<TIn> scanner)
        {
            //Is this correct?
            while (true)
            {
                var pos = scanner.Offset;
                Match<TIn> latestMatch;
                ender.Parse(scanner, out latestMatch);
                if (latestMatch.Success)
                {                    
                    InnerMatch += latestMatch;
                    yield break;
                }
                if (pos != scanner.Offset)
                {
                    throw new InvalidOperationException(string.Format("Parser {0} advanced the stream.", ender));
                }

                TOut outRes = results.Parse(scanner, out latestMatch);
                InnerMatch += latestMatch;

                if (latestMatch.Success)
                    yield return outRes;
                else
                    yield break;
            }

            //InnerMatch += latestMatch;
        }
    }
}
