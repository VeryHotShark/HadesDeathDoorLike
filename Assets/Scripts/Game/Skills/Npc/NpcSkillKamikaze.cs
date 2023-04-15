using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class NpcSkillKamikaze : NpcSkill {
        public float _explodeRadius = 3.0f;
        public int _explodeDamage = 1;
        public ParticleController _explosionParticle;

        public override void OnCastStart() {
            Owner.AIAgent.Stop();
            Owner.AIAgent.Stop();
            Owner.SetState(NpcState.Attacking);
        }

        public override void OnSkillFinish() {
            Vector3 explodePos = Owner.CenterOfMass;
            PoolManager.Spawn(_explosionParticle, explodePos, Quaternion.identity);
            Collider[] colliders = Physics.OverlapSphere(explodePos, _explodeRadius, LayerManager.Masks.ACTORS);

            foreach (Collider col in colliders) {
                if(col == Owner.Collider)
                    continue;
                
                IHittable hittable = col.GetComponentInParent<IHittable>();

                if (hittable != null) {
                    HitData hitData = new HitData() {
                        actor = Owner,
                        damage = _explodeDamage,
                        position = explodePos
                    };
                    
                    hittable.Hit(hitData);
                }
            }
            
            Owner.Kill(Owner);
        }
    }
}
