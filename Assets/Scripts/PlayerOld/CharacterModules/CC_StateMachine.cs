using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class CC_StateMachine {
        // TODO zrobić tablicę która po nazwie wyszukuje Indexu State-a, by nie porównywać po Typach
        
        public event Action<CharacterControllerModule> OnStateChanged = delegate { };

        private List<CharacterControllerModule> _states = new List<CharacterControllerModule>();

        private CharacterControllerModule _currentState = null;
        private CharacterControllerModule _lastState = null;

        public CC_StateMachine(CharacterControllerModule initState) {
            _currentState = initState;
            AddState(initState);
        }

        private void AddState(CharacterControllerModule behaviour) {
            if (!_states.Contains(behaviour))
                _states.Add(behaviour);
        }

        public void SetState(CharacterControllerModule newState) {
            if (_currentState == newState)
                return;
            
            AddState(newState);

            _lastState = _currentState;

            _currentState.OnStateExit();
            _currentState = newState;
            _currentState.OnStateEnter();

            OnStateChanged(_currentState);
        }

        public void ResetStatesValues() {
            foreach (CharacterControllerModule state in _states)
                state.ResetValues();
        }

        public CharacterControllerModule CurrentState => _currentState;
        public CharacterControllerModule LastState => _lastState;
    }
}
