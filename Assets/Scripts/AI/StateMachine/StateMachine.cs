using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class StateMachine<TState> where TState : class, IState {
        private class Transition {
            public readonly TState To;
            public readonly Func<bool> Condition;

            public Transition(TState to, Func<bool> condition) {
                To = to;
                Condition = condition;
            }
        }

        public event Action<TState> OnStateChanged = delegate { };

        private List<TState> _states = new();
        private List<Transition> _anyTransitions = new();
        private Dictionary<TState, List<Transition>> _stateTransitions = new();

        private TState _defaultState;
        private TState _currentState;
        private TState _lastState;

        public TState DefaultState => _defaultState;
        public TState CurrentState => _currentState;
        public TState LastState => _lastState;

        public StateMachine(TState initState) {
            _defaultState = initState;
            _currentState = initState;
            AddState(initState);
        }

        public void AddTransitionTo(TState fromState, TState toState, Func<bool> condition) {
            if (!_stateTransitions.ContainsKey(fromState)) 
                _stateTransitions.Add(fromState, new ());
            
            Transition transition = new Transition(toState, condition);    
            _stateTransitions[fromState].Add(transition);
        }

        public void AddAnyTransitionTo(TState state, Func<bool> condition) {
            Transition transition = new Transition(state, condition);
            _anyTransitions.Add(transition);
        }

        private void AddState(TState behaviour) {
            if (!_states.Contains(behaviour))
                _states.Add(behaviour);
        }

        public bool SetState(TState newState, bool forceSet = false) {
            bool transitionInvalid =
                _currentState == newState || !_currentState.CanExitState() || !newState.CanEnterState();

            if (!forceSet && transitionInvalid)
                return false;

            AddState(newState);

            _lastState = _currentState;

            _currentState.OnExit();
            _currentState = newState;
            _currentState.OnEnter();

            OnStateChanged(_currentState);
            return true;
        }

        public void Tick(float dt) {
            Transition transition = CheckForTransition();

            if (transition != null)
                SetState(transition.To);

            _currentState.OnTick(dt);
        }

        private Transition CheckForTransition() {
            foreach (Transition anyTransition in _anyTransitions) {
                if (anyTransition.Condition())
                    return anyTransition;
            }

            if (_stateTransitions.TryGetValue(_currentState, out List<Transition> transitions)) {
                foreach (Transition transition in transitions) {
                    if (transition.Condition())
                        return transition;
                }
            }

            return null;
        }

        public void ResetStates() {
            foreach (TState state in _states)
                state.OnReset();
        }

        public void TransitionToDefaultState() => SetState(_defaultState);
        public void TransitionToLastState() => SetState(_lastState);
    }
}