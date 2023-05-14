using System;
using System.Collections;
using System.Collections.Generic;
using Animancer.Examples.AnimatorControllers.GameKit;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class PlayerSkillGrenade : PlayerSkill {
        public StatusSO _statusToApply;
        
        public ParticleController _explosionVFX;
        public SkillIndicator _indicatorVFX;

        public float _maxDistance = 5.0f;
        public float _radius = 3.0f;
        public int _damage = 1;
        
        private HashSet<IHittable> _hittables = new HashSet<IHittable>();
        private Collider[] _colliders = new Collider[32];

        private float _sqrMaxDistance;
        private Vector3 _castOffset;
        private Vector3 _castPosition;
        private SkillIndicator _indicatorInstance;

        public override void OnInitialize() {
            base.OnInitialize();
            _sqrMaxDistance = _maxDistance.Square();
        }

        public override void OnReset() {
            base.OnReset();
            _hittables.Clear();
        }

        public override void OnCastStart() {
            _indicatorInstance = PoolManager.Spawn(_indicatorVFX, Owner.CenterOfMass, Quaternion.identity);
            _indicatorInstance.InitCircle(_radius);

            if (Owner.InputController.IsGamepad) {
                _castOffset = Owner.CharacterController.LastNonZeroLookInput * (_maxDistance / 2.0f);
                _indicatorInstance.transform.position += _castOffset;
            } 
            else
                _indicatorInstance.transform.position = Vector3.down * 10.0f;
        }

        public override void OnCastFinish() {
            PoolManager.Return(_indicatorInstance);
        }

        public override void OnAbort() {
            if(_indicatorInstance != null)
                PoolManager.Return(_indicatorInstance);
        }

        public override void OnCastTick(float deltaTime) {
            if (Owner.InputController.IsGamepad) {
                _castOffset += Owner.CharacterController.LookInput * (30.0f * deltaTime);
                _castPosition = Owner.CenterOfMass + _castOffset;
            }
            else {
                Vector3 cursorPos = Owner.InputController.CharacterInputs.MousePos;
                _castPosition = cursorPos;
            }
            
            float distanceSqrToCast = Owner.CenterOfMass.DistanceSquaredTo(_castPosition);

            if (distanceSqrToCast > _sqrMaxDistance) {
                _castPosition = Owner.CenterOfMass + Owner.CharacterController.LastNonZeroLookInput * _maxDistance;
                _castOffset = _castPosition - Owner.CenterOfMass;
            }
            
            _indicatorInstance.transform.position = _castPosition;
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
                        instigator = Owner,
                        statusToApply = _statusToApply.GetInstance(),
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