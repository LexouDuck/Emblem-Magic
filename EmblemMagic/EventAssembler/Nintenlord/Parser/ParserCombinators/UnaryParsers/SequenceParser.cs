// -----------------------------------------------------------------------
// <copyright file="SequenceParser.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Nintenlord.Parser.ParserCombinators.UnaryParsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Nintenlord.IO.Scanners;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public sealed class SequenceParser<T, TOut> : Parser<T, TOut>
    {
        readonly T[] sequence;
        readonly IEqualityComparer<T> equalityComparer;

        public SequenceParser(IEnumerable<T> sequenceToFind, IEqualityComparer<T> equalityComparer)
        {
            if (equalityComparer == null)
                throw new ArgumentNullException("equalityComparer");
            if (sequenceToFind == null)
                throw new ArgumentNullException("sequenceToFind");
            this.sequence = sequenceToFind.ToArray();
            this.equalityComparer = equalityComparer;
        }

        public SequenceParser(IEnumerable<T> sequenceToFind)
            : this(sequenceToFind, EqualityComparer<T>.Default)
        {

        }

        protected override TOut ParseMain(IScanner<T> scanner, out Match<T> match)
        {
            var startOffset = scanner.Offset;
            int i;
            for (i = 0; i < sequence.Length && !scanner.IsAtEnd; i++)
            {
                var current = scanner.Current;

                if (!equalityComparer.Equals(sequence[i], current))
                {
                    break;
                }
                scanner.MoveNext();
            }

            //Reached end without failing?
            match = (i == sequence.Length) 
                  ? new Match<T>(scanner, startOffset, sequence.Length) 
                  : new Match<T>(scanner, "Failed to match the sequence.");

            return default(TOut);
        }
    }
}
