using System;
using System.Collections.Generic;
using System.Linq;

namespace JohaToolkit.UnityEngine.DataStructures.StateMachine
{
    public class StateMachine
    {
        private readonly Dictionary<IState, List<(IState, Func<bool>)>> _transitions = new();
        private readonly List<(IState, Func<bool>)> _anyTransitions = new();
        
        private IState _currentState;
        public IState CurrentState
        {
            get => _currentState;
            private set
            {
                IState oldState = _currentState;
                _currentState.ExitState();
                _currentState = value;
                _currentState.EnterState();
                StateChanged?.Invoke(oldState, _currentState);
            }
        }

        public Action<IState, IState> StateChanged;

        public StateMachine(IState initialState)
        {
            _currentState = initialState;
            _currentState.EnterState();
        }
        
        public void AddTransition(IState from, IState to, Func<bool> condition)
        {
            if (!_transitions.ContainsKey(from))
            {
                _transitions.Add(from, new List<(IState, Func<bool>)>());
                return;
            }
            
            _transitions[from].Add((to, condition));
        }

        public void AddAnyTransition(IState to, Func<bool> condition)
        {
            _anyTransitions.Add((to, condition));
        }

        public void UpdateMachine()
        {
            List<(IState, Func<bool>)> transitions = new();
            transitions.AddRange(_transitions[CurrentState]);
            transitions.AddRange(_anyTransitions);
            
            foreach ((IState, Func<bool>) transition in transitions)
            {
                if (!transition.Item2.Invoke())
                    continue;
                Transition(transition.Item1);
                break;
            }
            CurrentState.UpdateState();
        }

        public void Transition(IState to)
        {
            CurrentState = to;
        }
    }
}
