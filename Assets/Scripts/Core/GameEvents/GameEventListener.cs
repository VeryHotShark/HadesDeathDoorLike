using UnityEngine;
using UnityEngine.Events;

namespace VHS {
    public class GameEventListener : BaseBehaviour {

        [SerializeField] private GameEvent _gameEventToListenTo = null;
        [SerializeField] private UnityEvent<Object> _onGameEventRaised;

        public void OnEventRaised(Object raiser) => _onGameEventRaised.Invoke(raiser);

        protected virtual void OnEnable() {
            if (_gameEventToListenTo)
                _gameEventToListenTo.RegisterListener(this);
        }

        protected virtual void OnDisable() {
            if (_gameEventToListenTo)
                _gameEventToListenTo.UnregisterListener(this);
        }

    }
}


