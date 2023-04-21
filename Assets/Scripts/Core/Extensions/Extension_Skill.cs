using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public static class Extension_Skill {
        public static List<IHittable> DoSphereDamage(IActor owner, float radius, int damage, Vector3? pos = null, LayerMask? mask = null,ParticleController vfx = null) {
            Vector3 position = pos ?? owner.CenterOfMass;
            
            if(vfx)
                PoolManager.Spawn(vfx, position, Quaternion.identity);
            
            Collider[] colliders = Physics.OverlapSphere(position, radius, mask ?? LayerManager.Masks.ACTORS);
            List<IHittable> hittables = new List<IHittable>();
            
            foreach (Collider col in colliders) {
                if(col == owner.Collider)
                    continue;
                
                IHittable hittable = col.GetComponentInParent<IHittable>();

                if (hittable != null) {
                    HitData hitData = new HitData() {
                        instigator = owner,
                        damage = damage,
                        position = position
                    };
                    
                    hittable.Hit(hitData);
                    hittables.Add(hittable);
                }
            }

            return hittables;
        }
    }
}