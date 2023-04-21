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
        
        [Flags]
        private enum TrapActivationActor {
            Player = 1 << 1,
            Enemy = 1 << 2,
        }

        [SerializeField] private TrapActivationType _activationType;
        [SerializeField] private TrapActivationActor _activationActor;
        [SerializeField] private Timer _cooldown = new Timer();

        protected bool _isActive = true; 
        public virtual bool CanActivate => _isActive && !_cooldown.IsActive;
        
        public void Hit(HitData hitData) {
            if (_activationType.HasFlag(TrapActivationType.Hit)) {
                if(hitData.instigator == null)
                    return;
                
                bool actorIsPlayer = hitData.instigator is Player;

                if (actorIsPlayer && _activationActor.HasFlag(TrapActivationActor.Player)) 
                    TryActivate(hitData.instigator);
                else if (_activationActor.HasFlag(TrapActivationActor.Enemy))
                    TryActivate(hitData.instigator);
            }
        }

        public void OnActorTriggerEnter(IActor actor) {
            if (_activationType.HasFlag(TrapActivationType.Trigger)) {
                bool actorIsPlayer = actor is Player;

                if (actorIsPlayer && _activationActor.HasFlag(TrapActivationActor.Player)) 
                    TryActivate(actor);
                else if (_activationActor.HasFlag(TrapActivationActor.Enemy))
                    TryActivate(actor);
            }
        }
        
        public void OnActorTriggerExit(IActor actor) { }

        public bool TryActivate(IActor actor) {
            if (CanActivate) {
                _cooldown.Start();   
                OnActivate(actor);
            }

            return CanActivate;
        }

        public void Deactivate() {
            _isActive = false;
            OnDeactivate();
        }
        
        protected abstract void OnActivate(IActor actor);
        protected virtual void OnDeactivate() { }

    }
}