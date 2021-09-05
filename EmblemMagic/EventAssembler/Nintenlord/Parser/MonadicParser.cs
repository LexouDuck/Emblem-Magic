// -----------------------------------------------------------------------
// <copyright file="MonadicParser.cs" company="">
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

    public delegate Tuple<TOut, Match<TIn>> MonadicParser<TIn, TOut>(IScanner<TIn> scanner);

    public static class MonadicParserHelpers
    {
        public static MonadicParser<TIn, TOut> SelectMany<TIn, TOut1, TOut, TOut2>(
            this MonadicParser<TIn, TOut1> parser1,
            Func<TOut1, MonadicParser<TIn, TOut2>> collectionSelector,
            Func<TOut1, TOut2, TOut> resultSelector)
        {
            return scanner =>
                {
                    var firsstResult = parser1(scanner);
                    if (!firsstResult.Item2.Success)
                    {
                        return Tuple.Create(default(TOut), firsstResult.Item2);
                    }
                    else
                    {
                        var secParser = collectionSelector(firsstResult.Item1);
                        var secResult = secParser(scanner);
                        if (!secResult.Item2.Success)
                        {
                            return Tuple.Create(default(TOut), secResult.Item2);
                        }
                        else
                        {
                            return Tuple.Create(
                                resultSelector(firsstResult.Item1, secResult.Item1),
                                firsstResult.Item2 + secResult.Item2);
                        }
                    }
                };
        }

        public static MonadicParser<TIn, TOut> Select<TIn, TOut, TOut1>(
            this MonadicParser<TIn, TOut1> parser,
            Func<TOut1, TOut> resultSelector)
        {
            return scanner =>
            {
                var firsstResult = parser(scanner);
                return Tuple.Create(!firsstResult.Item2.Success ? 
                    default(TOut) : 
                    resultSelector(firsstResult.Item1), firsstResult.Item2);
            };
        }

        public static MonadicParser<TIn, TOut> GetFromParser<TIn, TOut>(this IParser<TIn, TOut> parser)
        {
            return scanner =>
                {
                    Match<TIn> match;
                    var result = parser.Parse(scanner, out match);
                    return Tuple.Create(result, match);
                };
        }

        public static MonadicParser<TIn, TOut> Always<TIn, TOut>()
        {
            return scanner => Tuple.Create(default(TOut), new Match<TIn>(scanner, 0));
        }

        public static MonadicParser<TIn, TOut> Never<TIn, TOut>()
        {
            return scanner => Tuple.Create(default(TOut), new Match<TIn>(scanner, "Epic fail."));
        }

        public static IParser<TIn, TOut> GetParser<TIn, TOut>(this MonadicParser<TIn, TOut> parser)
        {
            return new FunctionParser<TIn, TOut>(x => parser(x));
        }

        //public static MonadicParser<TIn, TOut> Or<TOut>(
        //    this MonadicParser<TIn, TOut> first,
        //    MonadicParser<TIn, TOut> second)
        //{
        //    //return from a in first
        //    //       select null;
        //    return null;
        //}
    }
}
