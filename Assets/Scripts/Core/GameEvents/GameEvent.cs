using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VHS {
    [CreateAssetMenu(menuName = "VHS/GameEvent")]
    public class GameEvent : ScriptableObject {
        public Action<UnityEngine.Object> OnEventRaised = delegate { };

        private List<GameEventListener> _listeners = new List<GameEventListener>();

        private void OnEnable() => _listeners = new List<GameEventListener>();

        public GameEventStorage GameEventStorage { get; private set; }

        public void Raise(UnityEngine.Object sender) {
            // Debug.LogError(string.Format("Event {0} raised by {1}", name, sender.name));

            for (int i = _listeners.Count - 1; i >= 0; i--)
                _listeners[i].OnEventRaised(sender);
            
            OnEventRaised(sender);
        }

        public void RegisterListener(GameEventListener listener) {
            if (!_listeners.Contains(listener))
                _listeners.Add(listener);
        }

        public void UnregisterListener(GameEventListener listener) {
            if (_listeners.Contains(listener))
                _listeners.Remove(listener);
        }

        public void SetGameEventStorage(GameEventStorage gameEventStorage) => GameEventStorage = gameEventStorage;
    }
}


