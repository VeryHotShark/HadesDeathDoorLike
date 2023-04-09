using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VHS {
    public abstract class Projectile : BaseBehaviour, IPoolable {
        [SerializeField, Tooltip("Custom Update needs to be enabled")]
        private float _lifeTime;
        
        [SerializeField] protected int _damage;
        [SerializeField] protected MMF_Player _hitFeedback;

        public Action<Projectile, HitData> OnHit = delegate { };

        protected float _lifeTimer;
        protected IActor _owner;

        protected abstract bool CheckForCollision();
        protected abstract void Hit();

        public override void OnCustomUpdate(float deltaTime) {
            _lifeTimer -= deltaTime;

            if (_lifeTimer < 0.0f) {
                OnLifetimeExpired();
                _lifeTimer = Mathf.Infinity;
            }
        }

        protected virtual void OnLifetimeExpired() => PoolManager.Return(this);

        protected virtual void RaiseEvents(HitData hitData) {
            OnHit(this, hitData);

            if (_owner != null)
                _owner.LastDealtData = hitData;
        }
        
        protected virtual void PlayHitFX() {
            if (_hitFeedback) {
                // TODO make this spawn Feedback
                _hitFeedback.transform.SetParent(null);
                _hitFeedback.PlayFeedbacks();
            }
        }

        public virtual void Init(IActor owner = null) {
            _owner = owner;
            _lifeTimer = _lifeTime;
        }

        public virtual void OnReturnToPool() { }
        public virtual void OnSpawnFromPool() { }
    }
}