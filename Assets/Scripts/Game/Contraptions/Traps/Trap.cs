using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public abstract class Trap : BaseBehaviour, IHittable, ITriggerable {
        [Flags]
        private enum TrapActivationType {
            Hit = 1 << 1,
            Trigger = 1 << 2,
        }

        [SerializeField] private TrapActivationType _activationType;
        [SerializeField] private Timer _cooldown;

        protected bool _isActive = true; 
        public virtual bool CanActivate => _isActive && !_cooldown.IsActive;
        
        public void Hit(HitData hitData) {
            if (_activationType.HasFlag(TrapActivationType.Hit))
                TryActivate();
        }

        public void OnActorTriggerEnter(IActor Actor) {
            if(_activationType.HasFlag(TrapActivationType.Trigger))
                TryActivate();
        }
        
        public void OnActorTriggerExit(IActor Actor) { }

        public bool TryActivate() {
            if (CanActivate) {
                _cooldown.Start();   
                OnActivate();
            }

            return CanActivate;
        }

        public void Deactivate() {
            _isActive = false;
            OnDeactivate();
        }
        
        protected abstract void OnActivate();
        protected virtual void OnDeactivate() { }

    }
}