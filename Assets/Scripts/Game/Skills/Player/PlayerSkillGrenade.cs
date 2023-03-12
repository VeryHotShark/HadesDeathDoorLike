using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class PlayerSkillGrenade : PlayerSkill {
        public ParticleController _explosionVFX;
        public ParticleController _indicatorVFX;
        
        
        public float _radius = 3.0f;
        public int _damage = 1;

        private HashSet<IHittable> _hittables = new HashSet<IHittable>();
        private Collider[] _colliders = new Collider[32];

        private Vector3 _castPosition;
        private ParticleController _indicatorInstance;

        public override void OnReset() {
            _hittables.Clear();
        }

        public override void OnCastStart() {
            _indicatorInstance = PoolManager.Spawn(_indicatorVFX,
                Owner.PlayerController.Camera.CursorTransform.position, Quaternion.identity);
            _indicatorInstance.transform.SetParent(Owner.PlayerController.Camera.CursorTransform);
            
        }

        public override void OnCastFinish() {
            PoolManager.Return(_indicatorInstance);
        }

        public override void OnCastTick(float deltaTime) {
            _castPosition = Owner.PlayerController.Camera.CursorTransform.position;
            DebugExtension.DebugWireSphere(_castPosition, Color.yellow, _radius);
        }

        public override void OnSkillFinish() {
            int hitCount =
                Physics.OverlapSphereNonAlloc(_castPosition, _radius, _colliders, LayerManager.Masks.NPC);
            
            ParticleController particleController = PoolManager.Spawn(_explosionVFX, _castPosition, Quaternion.identity);
            particleController.Play();
            
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
                        position = collider.ClosestPoint(_castPosition),
                        direction = _castPosition.DirectionTo(collider.transform.position)
                    };

                    hittable.Hit(hitData);
                    _hittables.Add(hittable);
                }
            }
        }
    }
}