using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class TrapColumn : Trap {
        [SerializeField] private float _radius = 5.0f;
        [SerializeField] private ParticleController _particle;
        
        protected override void OnActivate(IActor actor) {
            List<IHittable> hittables = Extension_Skill.DoSphereDamage(actor, _radius, 1, transform.position, LayerManager.Masks.ACTORS);

            foreach (IHittable hittable in hittables) {
                PoolManager.Spawn(_particle, hittable.gameObject.transform.position, Quaternion.identity);
            }
        }
        
        private void OnDrawGizmos() {
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
