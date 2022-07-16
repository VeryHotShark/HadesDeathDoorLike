using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public abstract class Projectile : BaseBehaviour {
        [SerializeField] private float _damage;

        protected abstract bool CheckForCollision();
        protected abstract void OnHit();
        
        public virtual void Init() {}
    }
}