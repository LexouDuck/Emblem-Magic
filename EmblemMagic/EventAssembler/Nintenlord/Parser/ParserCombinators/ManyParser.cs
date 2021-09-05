using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nintenlord.IO.Scanners;

namespace Nintenlord.Parser.ParserCombinators
{
    public sealed class ManyParser<TIn, TOut> : RepeatingParser<TIn, TOut>
    {
        readonly IParser<TIn, TOut> toRepeat;

        public ManyParser(IParser<TIn, TOut> toRepeat)
        {
            if (toRepeat == null)
                throw new ArgumentNullException("toRepeat");
            this.toRepeat = toRepeat;
        }

        protected override IEnumerable<TOut> Enumerate(IScanner<TIn> scanner)
        {
            while (!scanner.IsAtEnd)
            {
                Match<TIn> latestMatch;
                TOut prim = toRepeat.Parse(scanner, out latestMatch);
                if (latestMatch.Success)
                {
                    InnerMatch += latestMatch;
                    yield return prim;
                }
                else
                {
                    yield break;
                }
            }
        }

        public override string ToString()
        {
            return string.Format("({0})*", toRepeat);
        }
    }
}
