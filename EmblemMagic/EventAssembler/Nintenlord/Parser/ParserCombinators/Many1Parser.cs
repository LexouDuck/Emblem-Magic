using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nintenlord.IO.Scanners;

namespace Nintenlord.Parser.ParserCombinators
{
    public sealed class Many1Parser<TIn, TOut> : RepeatingParser<TIn, TOut>
    {
        readonly IParser<TIn, TOut> toRepeat;

        public Many1Parser(IParser<TIn, TOut> toRepeat)
        {
            if (toRepeat == null)
                throw new ArgumentNullException("toRepeat");
            this.toRepeat = toRepeat;
        }

        protected override IEnumerable<TOut> Enumerate(IScanner<TIn> scanner)
        {
            Match<TIn> latestMatch;

            TOut prim = toRepeat.Parse(scanner, out latestMatch);
            if (!latestMatch.Success)
            {
                InnerMatch += latestMatch;
            }
            else
            {
                InnerMatch += latestMatch;
                yield return prim;

                while (true)
                {
                    prim = toRepeat.Parse(scanner, out latestMatch);
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
        }

        public override string ToString()
        {
            return string.Format("({0})+", toRepeat);
        }
    }
}
