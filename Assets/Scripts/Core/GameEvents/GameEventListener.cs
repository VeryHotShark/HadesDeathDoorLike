﻿using UnityEngine;

namespace VHS {
    public abstract class GameEventListener : BaseBehaviour {

        [SerializeField] private GameEvent gameEventToListenTo = null;

        public abstract void OnEventRaised(UnityEngine.Object raiser);

        protected virtual void OnEnable() {
            if (gameEventToListenTo)
                gameEventToListenTo.RegisterListener(this);
        }

        protected virtual void OnDisable() {
            if (gameEventToListenTo)
                gameEventToListenTo.UnregisterListener(this);
        }

    }
}


