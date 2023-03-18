using System.Collections.Generic;
using UnityEngine;
using System;

namespace VHS {
    public class GameEvent : ScriptableObject {

        public Action<GameEvent> OnEventRaised = delegate { };

        public static Action<GameEvent> _OnEventRaised = delegate { };

        private List<GameEventListener> listeners = new List<GameEventListener>();

        private void OnEnable() {
            listeners = new List<GameEventListener>();
        }

        public GameEventStorage GameEventStorage { get; private set; }

        public void Raise(UnityEngine.Object sender) {
            Debug.Log(string.Format("Event {0} raised by {1}", name, sender.name));

            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnEventRaised(this);

            OnEventRaised(this);
            _OnEventRaised(this);
        }

        public void RegisterListener(GameEventListener listener) {
            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }

        public void UnregisterListener(GameEventListener listener) {
            if (listeners.Contains(listener))
                listeners.Remove(listener);
        }

        public void SetGameEventStorage(GameEventStorage gameEventStorage) => GameEventStorage = gameEventStorage;

    }
}


