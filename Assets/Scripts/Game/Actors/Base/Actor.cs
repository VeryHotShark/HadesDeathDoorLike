using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS { 
    [RequireComponent(typeof(HitProcessorComponent))]
    public abstract class Actor : BaseBehaviour, IHittable, IActor {
        
        public Action<HitData> OnHit = delegate {  };
        public Action<HitPoints> OnHealthChanged = delegate { };
        
        public event Action<IActor> OnDeath = delegate {  };
        public event Action OnPostInitialized = delegate { };

        [SerializeField] private GameEvent _deathEvent;
        [SerializeField] private GameEvent _hitEvent;

        protected CapsuleCollider _capsuleCollider;
        protected HitProcessorComponent _hitProcessorComponent;
        protected DeathProcessorComponent _deathProcessorComponent;

        public float Radius => _capsuleCollider.radius;
        public Collider Collider => _capsuleCollider;
        public GameObject GameObject => gameObject;

        public HitData LastHitData { get; set; }
        public HitData LastDealtData { get; set; }
        
        public HitPoints HitPoints => _hitProcessorComponent.HitPoints;

        public virtual Vector3 FeetPosition => transform.position;
        public virtual Vector3 CenterOfMass => FeetPosition + Vector3.up;
        public virtual Vector3 Forward => transform.forward;

        public bool IsAlive => _hitProcessorComponent.HitPoints.AboveZero;

        private void Awake() {
            GetComponents();
            Initialize();
        }

        protected virtual void GetComponents() {
            _hitProcessorComponent = GetComponent<HitProcessorComponent>();
            _capsuleCollider = GetComponentInChildren<CapsuleCollider>();
        }

        protected virtual void Initialize() => _hitProcessorComponent.HitPoints.Reset();

        public virtual void Hit(HitData hitData) {
            _hitEvent?.Raise(this);
            _hitProcessorComponent.Hit(hitData);
        }

        public virtual void Die() {
            _deathEvent?.Raise(this);
            OnDeath(this);
        }

        public virtual void OnMyAttackParried(HitData hitData) { }
    }

    public class Actor<T> : Actor where T : Actor{
        private ActorComponent<T>[] _actorComponents;

        protected override void GetComponents() {
            _actorComponents = GetComponentsInChildren<ActorComponent<T>>();
            base.GetComponents();
        }

        /// <summary>
        /// When overriding call base method at the end, so callback will be properly called
        /// </summary>
        protected override void Initialize() {
            base.Initialize();
            
            HitPoints.OnChanged = hitPoints => OnHealthChanged(hitPoints);            
            
            foreach (ActorComponent<T> actorComponent in _actorComponents) 
                actorComponent.OnActorInitialized(this as T);
        }
    }
}
