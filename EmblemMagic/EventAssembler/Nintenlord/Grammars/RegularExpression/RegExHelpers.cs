// -----------------------------------------------------------------------
// <copyright file="RegExHelpers.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Nintenlord.Grammars.RegularExpression
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Numerics;
    using Nintenlord.Grammars.RegularExpression.Tree;
    using Nintenlord.Collections;
    using Nintenlord.Graph;
    using Nintenlord.Graph.PathFinding;
    using Nintenlord.Utility;
    using Nintenlord.Utility.Primitives;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class RegExHelpers
    {
        public static bool IsEmpty<TLetter>(IRegExExpressionTree<TLetter> exp)
        {
            switch (exp.Type)
            {
                case RegExNodeTypes.Letter:
                    return false;
                case RegExNodeTypes.EmptyWord:
                    return false;
                case RegExNodeTypes.Empty:
                    return true;
                case RegExNodeTypes.KleeneClosure:
                    return exp.GetChildren().All(IsEmpty);
                case RegExNodeTypes.Choise:
                    return exp.GetChildren().All(IsEmpty);
                case RegExNodeTypes.Concatenation:
                    return exp.GetChildren().Any(IsEmpty);
                default:
                    throw new ArgumentException();
            }
        }

        public static bool IsEmptyWord<TLetter>(IRegExExpressionTree<TLetter> exp)
        {
            switch (exp.Type)
            {
                case RegExNodeTypes.Letter:
                case RegExNodeTypes.Empty:
                    return false;
                case RegExNodeTypes.EmptyWord:
                    return true;
                case RegExNodeTypes.KleeneClosure:
                    return exp.GetChildren().All(IsEmptyWord);
                case RegExNodeTypes.Choise:
                    return exp.GetChildren().All(IsEmptyWord);
                case RegExNodeTypes.Concatenation:
                    return exp.GetChildren().All(IsEmptyWord);
                default:
                    throw new ArgumentException();
            }
        }

        public static bool IsInfinite<TLetter>(IRegExExpressionTree<TLetter> exp)
        {
            switch (exp.Type)
            {
                case RegExNodeTypes.Letter:
                    return false;
                case RegExNodeTypes.EmptyWord:
                    return false;
                case RegExNodeTypes.Empty:
                    return false;
                case RegExNodeTypes.KleeneClosure:
                    return !(IsEmpty(exp) || IsEmptyWord(exp));
                case RegExNodeTypes.Choise:
                    return exp.GetChildren().Any(IsEmpty);
                case RegExNodeTypes.Concatenation:
                    return exp.GetChildren().All(x => !IsEmpty(x)) && exp.GetChildren().Any(IsEmpty);
                default:
                    throw new ArgumentException();
            }
        }

        public static DeterministicFiniteAutomaton<bool[], TLetter> GetDFA<TLetter>(IRegExExpressionTree<TLetter> exp, IEnumerable<TLetter> alphabet)
        {
            int n = 0;
            var epsNFA = GetNFA(exp, () => n++);
            
            bool[] startState = new bool[n];
            startState[epsNFA.startState] = true;

            var finalState = epsNFA.finalState;
            Predicate<bool[]> isFinal = x => x[finalState];

            List<Tuple<bool[], TLetter, bool[]>> transitions = new List<Tuple<bool[],TLetter,bool[]>>();

            foreach (var letter in alphabet)
	        {
                transitions.AddRange(GetPWSTransitionsWithLetter(n, epsNFA, letter));
            }

            return new DeterministicFiniteAutomaton<bool[], TLetter>(transitions, isFinal, startState);
        }

        private static IEnumerable<Tuple<bool[], TLetter, bool[]>> GetPWSTransitionsWithLetter<TLetter>(
            int n, EpsilonDFA<int, TLetter> epsNFA, TLetter letter)
        {
            bool[][] reachable = new bool[n][];
            for (int i = 0; i < n; i++)
            {
                var states = epsNFA.ReachableStates(i, letter);
                reachable[i] = new bool[n];
                foreach (var endState in states)
                {
                    reachable[i][endState] = true;
                }
            }

            foreach (var powerSet in AllStates(n))
            {
                bool[] endPowerSet = new bool[n];
                for (int i = 0; i < n; i++)
                {
                    if (powerSet[i])
                    {
                        for (int j = 0; j < n; j++)
                        {
                            endPowerSet[j] |= reachable[i][j];//So many things can go wrong here.
                        }
                    }
                }
                yield return Tuple.Create(powerSet, letter, endPowerSet);
            }
        }

        public static IEnumerable<bool[]> AllStates(int length)
        {
            bool[] values = new bool[length];

            if (length < 32)
            {
                int pow = 1 << length;
                yield return values.Clone() as bool[];

                for (int i = 1; i <= pow; i++)
                {
                    int index = i.TrailingZeroCount();
                    values[index] ^= true;
                    yield return values.Clone() as bool[];
                }
            }
            else if (length <=64)
            {
                long pow = (long)1 << length;
                yield return values.Clone() as bool[];

                for (long i = 1; i <= pow; i++)
                {
                    int index = i.TrailingZeroCount();
                    values[index] ^= true;
                    yield return values.Clone() as bool[];
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static EpsilonDFA<TState, TLetter> GetNFA<TState, TLetter>(
            IRegExExpressionTree<TLetter> exp, Func<TState> getNewState)
        {
            switch (exp.Type)
            {
                case RegExNodeTypes.Letter:
                    return EpsilonDFA<TState, TLetter>.Letter(
                        ((Letter<TLetter>)exp).LetterToMatch, 
                        getNewState);

                case RegExNodeTypes.EmptyWord:
                    return EpsilonDFA<TState, TLetter>.EmptyWord(getNewState);

                case RegExNodeTypes.Empty:
                    return EpsilonDFA<TState, TLetter>.Empty(getNewState);

                case RegExNodeTypes.KleeneClosure:
                    return EpsilonDFA<TState, TLetter>.KleeneClosure(
                        GetNFA(exp.GetChildren().First(), getNewState),
                        getNewState);

                case RegExNodeTypes.Choise:
                    return EpsilonDFA<TState, TLetter>.Choise(
                        GetNFA(exp.GetChildren().First(), getNewState),
                        GetNFA(exp.GetChildren().Last(), getNewState),
                        getNewState);

                case RegExNodeTypes.Concatenation:
                    return EpsilonDFA<TState, TLetter>.Concatenation(
                        GetNFA(exp.GetChildren().First(), getNewState),
                        GetNFA(exp.GetChildren().Last(), getNewState),
                        getNewState);

                default:
                    throw new ArgumentException();
            }
        }
        
        public static IRegExExpressionTree<TLetter> Simplify<TLetter>(IRegExExpressionTree<TLetter> exp)
        {
            switch (exp.Type)
            {
                case RegExNodeTypes.Letter:
                case RegExNodeTypes.EmptyWord:
                case RegExNodeTypes.Empty:
                    return exp;//No change

                case RegExNodeTypes.KleeneClosure:
                    var child = exp.GetChildren().First();
                    if (IsEmptyWord(child))
                    {
                        return child;//(epsilon)* = epsilon
                    }
                    else if (child.Type == RegExNodeTypes.Choise)
                    {
                        var first = child.GetChildren().First();
                        var second = child.GetChildren().Last();

                        if (IsEmptyWord(first))
                        {
                            return new KleeneClosure<TLetter>(second); //(epsilon + r)* = r*
                        }
                        else if (IsEmptyWord(second))
                        {
                            return new KleeneClosure<TLetter>(first); //(r + epsilon)* = r*
                        }
                    }
                    return exp;//No change

                case RegExNodeTypes.Choise:
                    {
                        var first = exp.GetChildren().First();
                        var second = exp.GetChildren().Last();

                        if (IsEmpty(first))
                        {
                            return second; //empty + r = r
                        }
                        else if (IsEmpty(second))
                        {
                            return first; //r + empty = r
                        }
                    }
                    return exp;//No change

                case RegExNodeTypes.Concatenation:
                    {
                        var first = exp.GetChildren().First();
                        var second = exp.GetChildren().Last();

                        if (IsEmpty(first))
                        {
                            return first; //empty r = empty
                        }
                        else if (IsEmpty(second))
                        {
                            return second; //r empty = empty
                        }

                        if (IsEmptyWord(first))
                        {
                            return second; //epsilon r = r
                        }
                        else if (IsEmptyWord(second))
                        {
                            return first; //r epsilon = r
                        }

                        if (first.Type == RegExNodeTypes.Choise)
                        {
                            var first2 = first.GetChildren().First();
                            var second2 = first.GetChildren().Last();

                            return new Choise<TLetter>(
                                new Concatenation<TLetter>(first2, second),
                                new Concatenation<TLetter>(second2, second)
                                );//(s + t) r = s r + t r
                        }
                        else if (second.Type == RegExNodeTypes.Choise)
                        {
                            var first2 = second.GetChildren().First();
                            var second2 = second.GetChildren().Last();

                            return new Choise<TLetter>(
                                new Concatenation<TLetter>(first, first2),
                                new Concatenation<TLetter>(first, second2)
                                );//r (s + t) r = r s + r t
                        }
                    }
                    return exp;//No change
                default:
                    throw new ArgumentException();
            }
        }

        private class EpsilonDFA<TState, TLetter> : IGraph<TState>
        {
            /*
             //For IGraph<TState> implementation
             public readonly int stateCount;
             public readonly IEnumerable<TState> states;
             */

            public readonly IEnumerable<Tuple<TState, TLetter, TState>> transitions;
            public readonly IEnumerable<Tuple<TState, TState>> epsilonTransitions;
            public readonly TState startState;
            public readonly TState finalState;

            private EpsilonDFA(
                IEnumerable<Tuple<TState, TLetter, TState>> transitions,
                IEnumerable<Tuple<TState, TState>> epsilonTransitions,
                TState startState,
                TState finalState)
            {
                this.transitions = transitions;
                this.epsilonTransitions = epsilonTransitions;
                this.startState = startState;
                this.finalState = finalState;
            }

            public static EpsilonDFA<TState, TLetter> Choise(
                EpsilonDFA<TState, TLetter> first,
                EpsilonDFA<TState, TLetter> second, 
                Func<TState> getNewState)
            {
                var start = getNewState();
                var final = getNewState();

                var epsilonTransitions =
                    first.epsilonTransitions.Concat(
                    second.epsilonTransitions).Concat(
                        Tuple.Create(start, first.startState),
                        Tuple.Create(start, second.startState),
                        Tuple.Create(first.finalState, final),
                        Tuple.Create(second.finalState, final));

                var transitions = first.transitions.Concat(second.transitions);

                return new EpsilonDFA<TState, TLetter>(transitions, epsilonTransitions, start, final);
            }

            public static EpsilonDFA<TState, TLetter> Concatenation(
                EpsilonDFA<TState, TLetter> first,
                EpsilonDFA<TState, TLetter> second,
                Func<TState> getNewState)
            {
                var epsilonTransitions =
                    first.epsilonTransitions.Concat(
                    second.epsilonTransitions).Concat(
                        Tuple.Create(first.finalState, second.startState));

                var transitions = first.transitions.Concat(second.transitions);

                return new EpsilonDFA<TState, TLetter>(transitions, epsilonTransitions, 
                    first.startState, second.finalState);
            }

            public static EpsilonDFA<TState, TLetter> KleeneClosure(
                EpsilonDFA<TState, TLetter> toRepeat,
                Func<TState> getNewState)
            {
                var onlyNew = getNewState();

                var epsilonTransitions =
                    toRepeat.epsilonTransitions.Concat(
                        Tuple.Create(onlyNew, toRepeat.startState),
                        Tuple.Create(toRepeat.finalState, onlyNew));

                return new EpsilonDFA<TState, TLetter>(toRepeat.transitions, epsilonTransitions, onlyNew, onlyNew);
            }

            public static EpsilonDFA<TState, TLetter> Letter(TLetter letter, Func<TState> getNewState)
            {
                var start = getNewState();
                var final = getNewState();
                return new EpsilonDFA<TState, TLetter>(
                    Tuple.Create(start, letter, final).GetArray(),
                    Enumerable.Empty<Tuple<TState, TState>>(),
                    start, 
                    final);
            }

            public static EpsilonDFA<TState, TLetter> EmptyWord(Func<TState> getNewState)
            {
                var only = getNewState();

                return new EpsilonDFA<TState, TLetter>(
                    Enumerable.Empty<Tuple<TState, TLetter, TState>>(),
                    Enumerable.Empty<Tuple<TState, TState>>(),
                    only,
                    only);
            }

            public static EpsilonDFA<TState, TLetter> Empty(Func<TState> getNewState)
            {
                var start = getNewState();
                var final = getNewState();
                return new EpsilonDFA<TState, TLetter>(
                    Enumerable.Empty<Tuple<TState, TLetter, TState>>(),
                    Enumerable.Empty<Tuple<TState, TState>>(),
                    start,
                    final);
            }

            public HashSet<TState> EpsClosure(TState state)
            {
                return Dijkstra_algorithm.GetConnectedNodes(state, this, EqualityComparer<TState>.Default);
            }

            public HashSet<TState> ReachableStates(TState state, TLetter letter)
            {
                throw new NotImplementedException();
            }

            #region IGraph<TState> Members

            public int NodeCount
            {
                get { throw new NotImplementedException(); }
            }

            public IEnumerable<TState> GetNeighbours(TState node)
            {
                return from item in epsilonTransitions 
                       where EqualityComparer<TState>.Default.Equals(item.Item1, node) 
                       select item.Item2;
            }

            public bool IsEdge(TState node1, TState node2)
            {
                return epsilonTransitions.Contains(Tuple.Create(node1, node2));
            }

            #endregion

            #region IEnumerable<TState> Members

            public IEnumerator<TState> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            #endregion
        }
    }
}
