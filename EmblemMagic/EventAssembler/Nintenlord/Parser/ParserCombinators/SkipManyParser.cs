using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Parser.ParserCombinators
{
    public sealed class SkipManyParser<TIn, TOut> : Parser<TIn, TOut>
    {
        readonly IParser<TIn, TOut> toRepeat;

        public SkipManyParser(IParser<TIn, TOut> toRepeat)
        {
            if (toRepeat == null)
                throw new ArgumentNullException("toRepeat");
            this.toRepeat = toRepeat;
        }

        protected override TOut ParseMain(IO.Scanners.IScanner<TIn> scanner, out Match<TIn> match)
        {
            match = new Match<TIn>(scanner, 0);
            while (true)
            {
                Match<TIn> latestMatch;
                toRepeat.Parse(scanner, out latestMatch);
                if (latestMatch.Success)
                {
                    match += latestMatch;
                }
                else break;
            }

            return default(TOut);
        }
    }
}
