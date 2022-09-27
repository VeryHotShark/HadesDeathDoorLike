using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS { 
    [RequireComponent(typeof(HitProcessorComponent))]
    public abstract class Actor : BaseBehaviour, IHittable, IActor {
        public Action<HitData> OnHit = delegate {  };
        public event Action<IActor> OnDeath = delegate {  };

        protected HitProcessorComponent _hitProcessorComponent;
        protected DeathProcessorComponent _deathProcessorComponent;
        
        public GameObject GameObject => gameObject;
        
        public virtual Vector3 FeetPosition => transform.position;
        public virtual Vector3 CenterOfMass => FeetPosition + Vector3.up;
        public virtual Vector3 Forward => transform.forward;

        public bool IsAlive => _hitProcessorComponent.HitPoints.AboveZero;

        private void Awake() {
            GetComponents();
            Initialize();
        }

        protected virtual void GetComponents() => _hitProcessorComponent = GetComponent<HitProcessorComponent>();

        protected virtual void Initialize() {}

        public virtual void Hit(HitData hitData) => _hitProcessorComponent.Hit(hitData);
        public virtual void Die() => OnDeath(this);
    }
}
