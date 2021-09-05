// -----------------------------------------------------------------------
// <copyright file="DeterministicFiniteAutomata.cs" company="">
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
    /// Deterministic finite automaton
    /// </summary>
    public sealed class DeterministicFiniteAutomaton<TState, TAlphabet>
    {
        readonly Dictionary<Tuple<TState, TAlphabet>, TState> transition;
        readonly Predicate<TState> finalStatePredicate;
        readonly TState startState;

        TState currentState;

        /// <summary>
        /// If the DFA is in final state.
        /// </summary>
        public bool IsInFinalState
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructs a new DFA.
        /// </summary>
        /// <param name="transitions">Needs to be complete aka have all possible transitions</param>
        /// <param name="finalStates">All final states of DFA</param>
        /// <param name="startState">The start state of DFA</param>
        public DeterministicFiniteAutomaton(IEnumerable<Tuple<TState, TAlphabet, TState>> transitions,
            Predicate<TState> finalStates, TState startState)
        {
            this.transition = new Dictionary<Tuple<TState, TAlphabet>, TState>();
            foreach (var item in transitions)
            {
                transition[Tuple.Create(item.Item1, item.Item2)] = item.Item3;
            }
            this.finalStatePredicate = finalStates;
            this.startState = startState;

            this.Reset();
        }

        private DeterministicFiniteAutomaton(DeterministicFiniteAutomaton<TState, TAlphabet> cloneMe)
            : this(cloneMe.transition, cloneMe.finalStatePredicate, cloneMe.startState, cloneMe.currentState)
        {

        }

        private DeterministicFiniteAutomaton(
            Dictionary<Tuple<TState, TAlphabet>, TState> transition,
            Predicate<TState> finalStates,
            TState startState,
            TState currentState)
        {
            this.finalStatePredicate = finalStates;
            this.startState = startState;
            this.transition = transition;
            this.currentState = currentState;
        }

        /// <summary>
        /// Moves the DFA to the next state.
        /// </summary>
        /// <param name="nextAlphabet">Next input alphabet</param>
        public void Move(TAlphabet nextAlphabet)
        {
            this.currentState = transition[Tuple.Create(this.currentState, nextAlphabet)];
            IsInFinalState = finalStatePredicate(this.currentState);
        }

        /// <summary>
        /// Sets the DFA to starting state.
        /// </summary>
        public void Reset()
        {
            this.currentState = startState;
            IsInFinalState = finalStatePredicate(this.currentState);
        }

        /// <summary>
        /// Creates a clone with same current state.
        /// </summary>
        /// <returns></returns>
        public DeterministicFiniteAutomaton<TState, TAlphabet> Clone()
        {
            return new DeterministicFiniteAutomaton<TState, TAlphabet>(this);
        }

        /// <summary>
        /// Creates a clone that uses current state asd it's start state.
        /// </summary>
        /// <returns></returns>
        public DeterministicFiniteAutomaton<TState, TAlphabet> CloneWithCurrentStateAsStart()
        {
            return new DeterministicFiniteAutomaton<TState, TAlphabet>(
                this.transition,
                this.finalStatePredicate,
                this.currentState,
                this.currentState);
        }
    }
}
