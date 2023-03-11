using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class PlayerSkillGroundSlam : PlayerSkill {
        public float _radius = 3.0f;
        public int _damage = 1;

        private HashSet<IHittable> _hittables = new HashSet<IHittable>();
        private Collider[] _colliders = new Collider[32];

        public override void FinishSkill_Hook() {
            int hitCount =
                Physics.OverlapSphereNonAlloc(Owner.CenterOfMass, _radius, _colliders, LayerManager.Masks.NPC);

            if (hitCount == 0)
                return;

            for (int i = 0; i < hitCount; i++) {
                Collider collider = _colliders[i];
                IHittable hittable = collider.GetComponentInParent<IHittable>();

                if (hittable != null) {
                    if (_hittables.Contains(hittable))
                        continue;

                    HitData hitData = new HitData {
                        damage = _damage,
                        actor = Owner,
                        position = collider.ClosestPoint(Owner.CenterOfMass),
                        direction = Owner.FeetPosition.DirectionTo(collider.transform.position)
                    };

                    hittable.Hit(hitData);
                    _hittables.Add(hittable);
                }
            }
            
            DebugExtension.DebugWireSphere(Owner.CenterOfMass, Color.red, _radius, 2.0f);
        }
    }
}