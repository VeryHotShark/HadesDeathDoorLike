using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class CC_StateMachine {
        // TODO zrobić tablicę która po nazwie wyszukuje Indexu State-a, by nie porównywać po Typach
        
        public event Action<OldCharacterControllerModule> OnStateChanged = delegate { };

        private List<OldCharacterControllerModule> _states = new List<OldCharacterControllerModule>();

        private OldCharacterControllerModule _currentState = null;
        private OldCharacterControllerModule _lastState = null;

        public CC_StateMachine(OldCharacterControllerModule initState) {
            _currentState = initState;
            AddState(initState);
        }

        private void AddState(OldCharacterControllerModule behaviour) {
            if (!_states.Contains(behaviour))
                _states.Add(behaviour);
        }

        public void SetState(OldCharacterControllerModule newState) {
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
            foreach (OldCharacterControllerModule state in _states)
                state.ResetValues();
        }

        public OldCharacterControllerModule CurrentState => _currentState;
        public OldCharacterControllerModule LastState => _lastState;
    }
}
