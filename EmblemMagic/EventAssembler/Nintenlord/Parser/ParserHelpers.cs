// -----------------------------------------------------------------------
// <copyright file="ParserHelpers.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Nintenlord.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Nintenlord.Parser.ParserCombinators;
    using Nintenlord.Parser.ParserCombinators.BinaryParsers;
    using Nintenlord.Parser.ParserCombinators.UnaryParsers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ParserHelpers
    {
        public static SequenceParser<T, TOut> ParseSequence<T, TOut>(IEnumerable<T> sequence)
        {
            return new SequenceParser<T, TOut>(sequence);
        }

        public static FunctionParser<TIn, TOut> Parser<TIn, TOut>(
            this Func<Nintenlord.IO.Scanners.IScanner<TIn>, Tuple<TOut,  Match<TIn>>> fun)
        {
            return new FunctionParser<TIn, TOut>(fun);
        }

        public static BetweenParser<TIn, TStart, TEnd, TOut> Between<TIn, TOut, TStart, TEnd>(
            this IParser<TIn, TOut> parser, 
            IParser<TIn, TStart> start, 
            IParser<TIn, TEnd> end)
        {
            return new BetweenParser<TIn, TStart, TEnd, TOut>(start, parser, end);
        }

        public static ManyParser<TIn, TOut> Many<TIn, TOut>(this IParser<TIn, TOut> parser)
        {
            return new ManyParser<TIn, TOut>(parser);
        }

        public static Many1Parser<TIn, TOut> Many1<TIn, TOut>(this IParser<TIn, TOut> parser)
        {
            return new Many1Parser<TIn, TOut>(parser);
        }

        public static ManyTillParser<TIn, TEnd, TOut> ManyTill<TIn, TEnd, TOut>(
            this IParser<TIn, TOut> results, IParser<TIn, TEnd> ender)
        {
            return new ManyTillParser<TIn, TEnd, TOut>(results, ender);
        }

        public static OrParser<TIn, TOut> Or<TIn, TOut>(this IParser<TIn, TOut> choise1, IParser<TIn, TOut> choise2)
        {
            return new OrParser<TIn, TOut>(choise1, choise2);
        }

        public static ChoiseParser<TIn, TOut> Choise<TIn, TOut>(params IParser<TIn, TOut>[] parsers)
        {
            return new ChoiseParser<TIn, TOut>(parsers);
        }

        public static ChoiseParser<TIn, TOut> Choise<TIn, TOut>(this IEnumerable<IParser<TIn, TOut>> parsers)
        {
            return new ChoiseParser<TIn, TOut>(parsers);
        }

        public static TransformParser<TIn, TMiddle, TOut> Transform<TIn, TMiddle, TOut>(
            this IParser<TIn, TMiddle> parser, Converter<TMiddle, TOut> f)
        {
            return new TransformParser<TIn, TMiddle, TOut>(parser, f);
        }

        public static SeparatedBy1Parser<TIn, TSeb, TOut> SepBy1<TIn, TSeb, TOut>(
            this IParser<TIn, TOut> parser, IParser<TIn, TSeb> separator)
        {
            return new SeparatedBy1Parser<TIn, TSeb, TOut>(separator, parser);
        }

        public static SeparatedByParser<TIn, TSeb, TOut> SepBy<TIn, TSeb, TOut>(
            this IParser<TIn, TOut> parser, IParser<TIn, TSeb> separator)
        {
            return new SeparatedByParser<TIn, TSeb, TOut>(separator, parser);
        }

        public static SatisfyParser<T> Satisfy<T>(this Predicate<T> predicate)
        {
            return new SatisfyParser<T>(predicate);
        }

        public static OptionalParser<TIn, TOut> Optional<TIn, TOut>(this IParser<TIn, TOut> parser)
        {
            return new OptionalParser<TIn, TOut>(parser);
        }

        public static OptionalParser<TIn, TOut> Optional<TIn, TOut>(this IParser<TIn, TOut> parser, TOut defaultVal)
        {
            return new OptionalParser<TIn, TOut>(parser, defaultVal);
        }

        public static NameParser<TIn, TOut> Name<TIn, TOut>(this IParser<TIn, TOut> parser, string name)
        {
            return new NameParser<TIn, TOut>(parser, name);
        }

        public static SafeFailureCheckerParser<TIn, TOut> AddCheck<TIn, TOut>(this IParser<TIn, TOut> parser)
        {
            return new SafeFailureCheckerParser<TIn, TOut>(parser);
        }

        public static SafeFailureCheckerParser<TIn, TOut> AddCheck<TIn, TOut>(this IParser<TIn, TOut> parser, string text)
        {
            return new SafeFailureCheckerParser<TIn, TOut>(parser, text);
        }

        public static CombineParser<TIn, TMiddle1, TMiddle2, TOut> Combine<TIn, TMiddle1, TMiddle2, TOut>(
            this IParser<TIn, TMiddle1> first, 
            IParser<TIn, TMiddle2> second, 
            Func<TMiddle1, TMiddle2, TOut> comb)
        {
            return new CombineParser<TIn, TMiddle1, TMiddle2, TOut>(first, second, comb);
        }

        public static CombineParser<TIn, TMiddle1, TMiddle2, TMiddle3, TOut> 
            Combine<TIn, TMiddle1, TMiddle2, TMiddle3, TOut>(
            this IParser<TIn, TMiddle1> first,
            IParser<TIn, TMiddle2> second,
            IParser<TIn, TMiddle3> third,
            Func<TMiddle1, TMiddle2, TMiddle3, TOut> comb)
        {
            return new CombineParser<TIn, TMiddle1, TMiddle2, TMiddle3, TOut>(first, second, third, comb);
        }

        public static CombineParser<TIn, TMiddle1, TMiddle2, TMiddle3, TMiddle4, TOut>
            Combine<TIn, TMiddle1, TMiddle2, TMiddle3, TMiddle4, TOut>(
            this IParser<TIn, TMiddle1> first,
            IParser<TIn, TMiddle2> second,
            IParser<TIn, TMiddle3> third,
            IParser<TIn,TMiddle4> fourth,
            Func<TMiddle1, TMiddle2, TMiddle3, TMiddle4, TOut> comb)
        {
            return new CombineParser<TIn, TMiddle1, TMiddle2, TMiddle3, TMiddle4, TOut>
                (first, second, third, fourth, comb);
        }

        public static LazyParser<TIn, TOut> Lazy<TIn, TOut>(this Func<IParser<TIn, TOut>> parserFactory)
        {
            return new LazyParser<TIn, TOut>(new Lazy<IParser<TIn, TOut>>(parserFactory));
        }
    }
}
