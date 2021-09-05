using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Grammars
{
    public interface IStateMachine<TState, in TInput>
    {
        TState StartState { get; }
        
        IEnumerable<TState> GetStates();
        bool IsFinalState(TState state);
        TState Transition(TState currentState, TInput input);
    }

    public static class StateMachineHelpers
    {
        public static IEnumerable<TState> RunUntilFinalState<TState, TInput>(
            this IStateMachine<TState, TInput> machine, 
            IEnumerable<TInput> input)
        {
            return RunUntilFinalState(machine, input, machine.StartState);
        }

        public static IEnumerable<TState> RunUntilFinalState<TState, TInput>(
            this IStateMachine<TState, TInput> machine,
            IEnumerable<TInput> input,
            TState start)
        {
            foreach (var item in input)
            {
                yield return start;

                if (machine.IsFinalState(start))
                {
                    yield break;
                }

                start = machine.Transition(start, item);
            }
        }

        public static HistoryKeepingStateMachine<TState, TInput>
            GetHistoryKeeping<TState, TInput>(this IStateMachine<TState, TInput> machine)
        {
            return new HistoryKeepingStateMachine<TState, TInput>(machine);
        }
    }
}
