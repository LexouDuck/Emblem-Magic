// -----------------------------------------------------------------------
// <copyright file="FunctonParser.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Nintenlord.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using Nintenlord.IO.Scanners;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public sealed class FunctionParser<TIn, TOut> : Parser<TIn, TOut>
    {
        readonly Func<IScanner<TIn>, Tuple<TOut,  Match<TIn>>> parserFunction;

        public FunctionParser(Func<IScanner<TIn>, Tuple<TOut, Match<TIn>>> parserFunction)
        {
            this.parserFunction = parserFunction;
        }

        protected override TOut ParseMain(IScanner<TIn> scanner, out Match<TIn> match)
        {
            var parseResult = parserFunction(scanner);

            match = parseResult.Item2;
            return parseResult.Item1;
        }
    }
}
