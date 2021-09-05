using System;
using Nintenlord.IO.Scanners;
using Nintenlord.Parser.ParserCombinators.BinaryParsers;

namespace Nintenlord.Parser
{
    public abstract class Parser<TIn, TOut> : IParser<TIn,TOut>
    {
        protected Parser()
        {

        }

        protected abstract TOut ParseMain(IScanner<TIn> scanner, out Match<TIn> match);

        /// <summary>
        /// Parses input.
        /// </summary>
        /// <param name="scanner">Object to scan data from.</param>
        /// <param name="match">Match of the parser in scanner.</param>
        /// <returns>Result of parsing if match.Success is true, else undefined.</returns>
        public virtual TOut Parse(IScanner<TIn> scanner, out Match<TIn> match)
        {
            var result = ParseMain(scanner, out match);

            if (ParseEvent != null)
            {
                ParseEvent(this, new ParsingEventArgs<TIn, TOut>(match, result));
            }
            return result;
        }
        
        public event EventHandler<ParsingEventArgs<TIn, TOut>> ParseEvent;
        
        public override string ToString()
        {
            return this.GetType().Name;
        }

        public static OrParser<TIn, TOut> operator |(Parser<TIn, TOut> p1, IParser<TIn, TOut> p2)
        {
            return new OrParser<TIn, TOut>(p1, p2);
        }

        public static OrParser<TIn, TOut> operator |(Parser<TIn, TOut> p1, Parser<TIn, TOut> p2)
        {
            return new OrParser<TIn, TOut>(p1, p2);
        }

        public static OrParser<TIn, TOut> operator |(IParser<TIn, TOut> p1, Parser<TIn, TOut> p2)
        {
            return new OrParser<TIn, TOut>(p1, p2);
        }
    }
}
