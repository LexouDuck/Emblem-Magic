// -----------------------------------------------------------------------
// <copyright file="ContextFreeGrammar.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Nintenlord.Grammars
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public sealed class ContextFreeGrammar<T>
    {
        IDictionary<T, T[][]> productions;
        T startingSymbol;
        T[] variables;
        T[] terminals;

        public T[][] this[T variable]
        {
            get
            {
                return productions[variable];
            }
        }
        public T StartingSymbol
        {
            get 
            {
                return startingSymbol;
            }
        }
        public IEnumerable<T> Variables
        {
            get { return variables; }
        }
        public IEnumerable<T> Terminals
        {
            get { return terminals; }
        }
        
        public T[] DeriveRandom(Random random)
        {
            List<T> word = new List<T>(20) {startingSymbol};
            while (true)
            {
                int i;
                for (i = 0; i < word.Count; i++)
                {
                    if (variables.Contains(word[i]))
                        break;
                }

                if (i == word.Count)
                    break;

                var rules = productions[word[i]];
                var ruleToUse = rules[random.Next(rules.Length)];

                word.RemoveAt(i);
                word.InsertRange(i, ruleToUse);
            }
            return word.ToArray();
        }
    }
}
