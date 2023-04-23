using System;
using UnityEngine;

namespace VHS {
    // IT IS NOT ABSTRACT FOR A PURPOS, BECAUSE PASSIVE SO CANT SERIALIZE ABSTRACT
    [Serializable]
    public class Passive {
        private Actor _owner;
        public Actor Owner => _owner;

        public PassiveTrigger _trigger;
        [SerializeReference] public PassiveAction _action;

        public void Init(Actor actor) {
            _owner = actor;
            _action.Init(_owner);
            _trigger.OnTriggered = PerformAction;
        }

        public void Enable() {
            _action.OnEnable();
            _trigger.OnEnable();
        }

        public void Disable() {
            _action.OnDisable();
            _trigger.OnDisable();
        }

        private void PerformAction() => _action.PerformAction();
    }
    
    [Serializable]
    public class Passive<T>:  Passive where T : Actor {
        public new T Owner => (T) base.Owner;
    }
}
