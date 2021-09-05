using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nintenlord.IO.Scanners;

namespace Nintenlord.Parser.ParserCombinators.BinaryParsers
{
    public sealed class CombineParser<TIn, TMiddle1, TMiddle2, TOut> : Parser<TIn, TOut>
    {
        readonly Func<TMiddle1, TMiddle2, TOut> combiner;
        readonly IParser<TIn, TMiddle1> first;
        readonly IParser<TIn, TMiddle2> second;
        
        public CombineParser(IParser<TIn, TMiddle1> first, IParser<TIn, TMiddle2> second,
            Func<TMiddle1, TMiddle2, TOut> combiner)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");
            if (combiner == null)
                throw new ArgumentNullException("combiner");

            this.first = first;
            this.second = second;
            this.combiner = combiner;
        }

        protected override TOut ParseMain(IScanner<TIn> scanner, out Match<TIn> match)
        {
            var mid1 = first.Parse(scanner, out match);

            if (!match.Success)
            {
                return default(TOut);
            }

            Match<TIn> secondMatch;
            var mid2 = second.Parse(scanner, out secondMatch);
            match += secondMatch;

            return !match.Success ? default(TOut) : combiner(mid1, mid2);
        }
    }

    public sealed class CombineParser<TIn, TMiddle1, TMiddle2, TMiddle3, TOut> : Parser<TIn, TOut>
    {
        readonly Func<TMiddle1, TMiddle2, TMiddle3, TOut> combiner;
        readonly IParser<TIn, TMiddle1> first;
        readonly IParser<TIn, TMiddle2> second;
        readonly IParser<TIn, TMiddle3> third;

        public CombineParser(
            IParser<TIn, TMiddle1> first, 
            IParser<TIn, TMiddle2> second,
            IParser<TIn, TMiddle3> third,
            Func<TMiddle1, TMiddle2, TMiddle3, TOut> combiner)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");
            if (third == null)
                throw new ArgumentNullException("third");
            if (combiner == null)
                throw new ArgumentNullException("combiner");

            this.first = first;
            this.second = second;
            this.third = third;
            this.combiner = combiner;
        }

        protected override TOut ParseMain(IScanner<TIn> scanner, out Match<TIn> match)
        {
            TMiddle1 mid1 = first.Parse(scanner, out match);

            if (!match.Success)
            {
                return default(TOut);
            }

            Match<TIn> tempMatch;
            TMiddle2 mid2 = second.Parse(scanner, out tempMatch);
            match += tempMatch;

            if (!match.Success)
            {
                return default(TOut);
            }

            TMiddle3 mid3 = third.Parse(scanner, out tempMatch);
            match += tempMatch;

            if (!match.Success)
            {
                return default(TOut);
            }

            return combiner(mid1, mid2, mid3);
        }
    }
    
    public sealed class CombineParser<TIn, TMiddle1, TMiddle2, TMiddle3, TMiddle4, TOut> : Parser<TIn, TOut>
    {
        readonly Func<TMiddle1, TMiddle2, TMiddle3, TMiddle4, TOut> combiner;
        readonly IParser<TIn, TMiddle1> first;
        readonly IParser<TIn, TMiddle2> second;
        readonly IParser<TIn, TMiddle3> third;
        readonly IParser<TIn, TMiddle4> fourth;

        public CombineParser(
            IParser<TIn, TMiddle1> first,
            IParser<TIn, TMiddle2> second,
            IParser<TIn, TMiddle3> third,
            IParser<TIn, TMiddle4> fourth,
            Func<TMiddle1, TMiddle2, TMiddle3, TMiddle4, TOut> combiner)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");
            if (third == null)
                throw new ArgumentNullException("third");
            if (fourth == null)
                throw new ArgumentNullException("fourth");
            if (combiner == null)
                throw new ArgumentNullException("combiner");

            this.first = first;
            this.second = second;
            this.third = third;
            this.fourth = fourth;
            this.combiner = combiner;
        }

        protected override TOut ParseMain(IScanner<TIn> scanner, out Match<TIn> match)
        {
            TMiddle1 mid1 = first.Parse(scanner, out match);

            if (!match.Success)
            {
                return default(TOut);
            }
            Match<TIn> secondMatch;

            TMiddle2 mid2 = second.Parse(scanner, out secondMatch);
            match += secondMatch;
            if (!match.Success)
            {
                return default(TOut);
            }

            TMiddle3 mid3 = third.Parse(scanner, out secondMatch);
            match += secondMatch;
            if (!match.Success)
            {
                return default(TOut);
            }

            TMiddle4 mid4 = fourth.Parse(scanner, out secondMatch);
            match += secondMatch;
            if (!match.Success)
            {
                return default(TOut);
            }

            return combiner(mid1, mid2, mid3, mid4);
        }
    }
}
