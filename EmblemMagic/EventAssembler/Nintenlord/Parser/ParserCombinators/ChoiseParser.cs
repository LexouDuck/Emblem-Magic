using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nintenlord.Collections;

namespace Nintenlord.Parser.ParserCombinators
{
    public sealed class ChoiseParser<TIn, TOut> : Parser<TIn, TOut>
    {
        readonly IParser<TIn, TOut>[] options;

        public ChoiseParser(IEnumerable<IParser<TIn, TOut>> options)
        {
            if (options == null)
                throw new ArgumentNullException("options");
            this.options = options.ToArray();

            if (this.options.Any(x => x == null))
                throw new ArgumentNullException("options");
        }

        protected override TOut ParseMain(IO.Scanners.IScanner<TIn> scanner, out Match<TIn> match)
        {
            foreach (var item in options)
            {
                var result = item.Parse(scanner, out match);
                if (match.Success)
                {
                    return result;
                }
            }
            match = new Match<TIn>(scanner, "No match");
            return default(TOut);
        }

        public override string ToString()
        {
            return options.ToElementWiseString("|", "(", ")");
        }
    }
}
