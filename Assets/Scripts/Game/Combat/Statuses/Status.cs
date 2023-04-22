using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VHS {
    [Serializable]
    public abstract class Status {
        [SerializeField] protected Timer _durationTimer;
        
        protected Npc _npc;
        protected NpcStatusComponent _statusComponent;

        public void Init(Npc npc, NpcStatusComponent statusComponent) {
            _npc = npc;
            _statusComponent = statusComponent;
            _durationTimer.OnEnd = RemoveSelfOnEnd;
        }

        public virtual void OnReapplied() => _durationTimer?.Start();
        public abstract void OnApplied();
        public abstract void OnTick(float dt);
        public abstract void OnRemoved();

        private void RemoveSelfOnEnd() => _statusComponent.RemoveStatus(this);
    }
}
