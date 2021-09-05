using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Grammars
{
    public sealed class CompleteStateMachine<T> : IStateMachine<T, T>
    {
        T startState;
        IEnumerable<T> statesToUse;
        Predicate<T> finalState;

        public CompleteStateMachine(T startState, IEnumerable<T> statesToUse, Predicate<T> finalState)
        {
            this.startState = startState;
            this.statesToUse = statesToUse;
            this.finalState = finalState;
        }

        #region IStateMachine<T,T> Members

        public T StartState
        {
            get { return startState; }
        }

        public IEnumerable<T> GetStates()
        {
            return statesToUse;
        }

        public bool IsFinalState(T state)
        {
            return finalState(state);
        }

        public T Transition(T currentState, T input)
        {
            return input;
        }

        #endregion
    }
}
