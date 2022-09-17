using System;
using System.Collections;
using System.Collections.Generic;
using Ludiq.PeekCore.CodeDom;
using UnityEngine;

namespace VHS { 
    [RequireComponent(typeof(HitProcessorComponent))]
    public abstract class Actor : BaseBehaviour, IHittable, IActor {
        public Action<HitData> OnHit = delegate {  };
        public Action<IActor> OnDeath = delegate {  };

        protected HitProcessorComponent _hitProcessorComponent;
        public virtual Vector3 FeetPosition => transform.position;
        public virtual Vector3 CenterOfMass => FeetPosition + Vector3.up;

        private void Awake() {
            GetComponents();
            Initialize();
        }

        protected virtual void GetComponents() => _hitProcessorComponent = GetComponent<HitProcessorComponent>();

        protected virtual void Initialize() {}

        public virtual void Hit(HitData hitData) => _hitProcessorComponent.Hit(hitData);
    }
}
