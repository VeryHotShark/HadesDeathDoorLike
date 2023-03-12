using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class ParticleController : BaseBehaviour, IPoolable{
        private ParticleSystem _particle;
        
        void Awake() => _particle = GetComponentInChildren<ParticleSystem>();

        public void Play() => _particle.Play(true);

        public override void OnCustomUpdate(float deltaTime) {
            if(!_particle.IsAlive(true))
                PoolManager.Return(this);
        }
    }
}
