using System;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class Passive {
        private Actor _owner;
        public Actor Owner => _owner;

        public PassiveTrigger _trigger;
        [SerializeReference] public PassiveAction _action;

        public void Init(Actor actor) {
            _owner = actor;
            _action.Init(_owner);
        }

        public void Enable() {
            _action.OnEnable();
            _trigger.OnEnable();
            _trigger.OnTriggered += PerformAction;
        }

        public void Disable() {
            _action.OnDisable();
            _trigger.OnDisable();
            _trigger.OnTriggered -= PerformAction;
        }

        private void PerformAction() => _action.PerformAction();
    }
    
    [Serializable]
    public class Passive<T>:  Passive where T : Actor {
        public new T Owner => (T) base.Owner;
    }
}
