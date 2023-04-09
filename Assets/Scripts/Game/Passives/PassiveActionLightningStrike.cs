using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VHS {
    [Serializable]
    public class PassiveActionLightningStrike : PassiveAction<Actor> {
        public int damage = 1;
        public int maxHitCount = 1;
        public float radius = 5.0f;
        public Feedback hitFeedback;
        public ParticleController particle;

        public override void PerformAction() {
            Collider[] overlapColliders = Physics.OverlapSphere(_actor.FeetPosition, radius, LayerManager.Masks.NPC);
            List<Collider> hitColliders = new List<Collider>(overlapColliders);

            bool hitSomething = false;

            if (hitColliders.Count == 0)
                return;

            int maxCount = Mathf.Min(hitColliders.Count, maxHitCount);
                
            for (int i = 0; i < maxCount; i++) {
                Collider collider = hitColliders.GetRandomElement();
                IHittable hittable = collider.GetComponentInParent<IHittable>();

                if (hittable != null) {
                    HitData hitData = new HitData() {
                        position = collider.transform.position,
                        actor = _owner,
                        hittable = hittable,
                        damage = damage,
                        direction = Vector3.down
                    };
                    
                    hittable.Hit(hitData);
                    PoolManager.Spawn(particle, hitData.position, Quaternion.identity);
                    hitSomething = true;
                }

                hitColliders.Remove(collider);
            }

            if (hitSomething)
                PoolManager.Spawn(hitFeedback);
        }
    }
}
