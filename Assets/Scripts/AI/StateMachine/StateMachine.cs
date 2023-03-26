using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<TState> where TState : class, IState  {
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
    private List<Transition> _transitions = new();

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
    
    public void AddTransitionTo(TState state, Func<bool> condition) {
        AddState(state);
        Transition transition = new Transition(state, condition);
        _transitions.Add(transition);
    }

    private void AddState(TState behaviour) {
        if (!_states.Contains(behaviour))
            _states.Add(behaviour);
    }

    public void SetState(TState newState) {
        if (_currentState == newState || !newState.CanEnterState())
            return;

        AddState(newState);

        _lastState = _currentState;

        _currentState.OnExit();
        _currentState = newState;
        _currentState.OnEnter();

        OnStateChanged(_currentState);
    }
    
    public void Tick(float dt) {
        Transition transition = CheckForTransition();

        if (transition != null)
            SetState(transition.To);

        _currentState.OnTick(dt);
    }

    private Transition CheckForTransition() {
        foreach (Transition transition in _transitions) {
            if (transition.Condition())
                return transition;
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