using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Grammars
{
    public sealed class HistoryKeepingStateMachine<TState, TInput> 
        : IStateMachine<TState[], HistoryKeepingStateMachine<TState, TInput>.Input>
    {
        public sealed class Input
        {
            public readonly bool returnToPrevious;
            public readonly TInput toMove;

            private Input(bool returnToPrevious, TInput toMove)
            {
                this.returnToPrevious = returnToPrevious;
                this.toMove = toMove;
            }

            public static readonly Input Return = new Input(true, default(TInput));
            public static Input GetMove(TInput toMove)
            {
                return new Input(false, toMove);
            }
        }

        TState[] startState;
        IStateMachine<TState, TInput> stateMachine;

        public HistoryKeepingStateMachine(IStateMachine<TState, TInput> stateMachine)
        {
            this.stateMachine = stateMachine;
            startState = new[] { stateMachine.StartState };
        }

        #region IStateMachine<TState[],TInput> Members

        public TState[] StartState
        {
            get { return startState; }
        }

        public IEnumerable<TState[]> GetStates()
        {
            //Yeah
            throw new NotImplementedException();
        }

        public bool IsFinalState(TState[] state)
        {
            return stateMachine.IsFinalState(state[state.Length - 1]);
        }

        public TState[] Transition(TState[] currentState, Input input)
        {
            TState[] newState;

            if (!input.returnToPrevious)
            {
                newState  = new TState[currentState.Length + 1];
                newState[currentState.Length] =
                    stateMachine.Transition(currentState[currentState.Length - 1],
                    input.toMove);
            }
            else
            {
                newState = new TState[currentState.Length - 1];
            }

            int length = Math.Min(newState.Length, currentState.Length);

            for (int i = 0; i < length; i++)
            {
                newState[i] = currentState[i];
            }

            return newState;
        }

        #endregion
    }
}
