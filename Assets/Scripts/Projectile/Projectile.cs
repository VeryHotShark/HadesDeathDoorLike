using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace VHS {
    public abstract class Projectile : BaseBehaviour {
        [SerializeField] private float _damage;
        [SerializeField] protected MMF_Player _hitFeedback;

        protected IActor _owner;

        protected abstract bool CheckForCollision();
        protected abstract void OnHit();

        protected virtual void PlayHitFX() {
            if (_hitFeedback) {
                // TODO make this spawn Feedback
                _hitFeedback.transform.SetParent(null);
                _hitFeedback.PlayFeedbacks();
            }
        }

        public virtual void Init(IActor owner = null) {
            _owner = owner;
        }
    }
}