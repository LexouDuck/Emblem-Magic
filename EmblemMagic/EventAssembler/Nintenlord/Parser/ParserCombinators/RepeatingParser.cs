using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nintenlord.IO.Scanners;

namespace Nintenlord.Parser.ParserCombinators
{
    /// <summary>
    /// Abstract class that makes sure that enumerations results are handled properly and correct result 
    /// and match is returned.
    /// </summary>
    public abstract class RepeatingParser<TIn, TOut> : Parser<TIn, List<TOut>>
    {
        private Match<TIn> innerMatch;

        protected Match<TIn> InnerMatch
        {
            get { return innerMatch; }
            set { innerMatch = value; }
        }

        protected override List<TOut> ParseMain(IScanner<TIn> scanner, out Match<TIn> match)
        {
            var temp = innerMatch;

            innerMatch = new Match<TIn>(scanner);
            var result = Enumerate(scanner).ToList();
            match = innerMatch;

            innerMatch = temp;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>If you use enumerator blocks, make sure you add to the InnerMatch before yielding.</remarks>
        /// <param name="scanner"></param>
        /// <returns></returns>
        protected abstract IEnumerable<TOut> Enumerate(IScanner<TIn> scanner);
    }
}
