using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VHS {
    [Serializable]
    public abstract class Status {
        [SerializeField] protected float _interval = 0.0f;
        [SerializeField] protected float _duration = 0.0f;

        private float _durationTimer = 0.0f;
        private float _intervalTimer = 0.0f;
        
        protected Npc _npc;
        protected NpcStatusComponent _statusComponent;

        public void Init(Npc npc, NpcStatusComponent statusComponent) {
            _npc = npc;
            _statusComponent = statusComponent;
        }

        public virtual void OnReapplied() => _durationTimer = 0.0f;
        public virtual void OnApplied() {}

        public void OnTick(float dt) {
            if (_interval > 0.0f) {
                _intervalTimer += dt;

                if (_intervalTimer > _interval) {
                    _intervalTimer = 0.0f;
                    OnInterval(_interval);
                }
            }
            else 
                OnInterval(dt);
            
            if (_duration > 0.0f) {
                _durationTimer += dt;

                if (_durationTimer > _duration)
                    RemoveSelf();
            }
        }

        protected virtual void OnInterval(float interval) { }
        public virtual  void OnRemoved() {}
        
        private void RemoveSelf() => _statusComponent.RemoveStatus(this);
    }
}
