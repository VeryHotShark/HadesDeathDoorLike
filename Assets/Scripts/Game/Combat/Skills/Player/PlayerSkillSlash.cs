using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

namespace VHS {
    public class PlayerSkillSlash : PlayerSkill {
        public StatusSO _statusToApply;
        
        [Header("GFX")]
        public ParticleController _redImpaxtVFX;
        public SkillIndicator _skillIndicator;
        public ClipTransition _windupClip;
        public ClipTransition _slashClip;
        
        
        [Header("settings")]
        public float _distance = 10.0f;
        public float _radius = 3.0f;
        public AnimationCurve _curve = AnimationCurve.EaseInOut(0,0,1,1);

        private HashSet<IHittable> _hittables = new HashSet<IHittable>();
        private Collider[] _colliders = new Collider[32];
        private SkillIndicator _indicatorInstance;

        private float _timer;
        private float _speed;
        private Vector3 _endPosition;
        private Vector3 _startPosition;
        
        public override void OnReset() {
            base.OnReset();
            _hittables.Clear();
        }

        public override void OnCastStart() {
            Owner.Animancer.Play(_windupClip);
            _indicatorInstance = PoolManager.Spawn(_skillIndicator, Owner.CharacterController.Motor.TransientPosition, Quaternion.LookRotation(Owner.CharacterController.LastNonZeroLookInput));
            _indicatorInstance.InitRectangle(_distance, _radius);
        }
        
        public override void OnCastTick(float deltaTime) => _indicatorInstance.transform.rotation = Quaternion.LookRotation(Owner.CharacterController.LastNonZeroLookInput);

        public override void OnCastFinish() {
            PoolManager.Return(_indicatorInstance);
        }

        public override void OnAbort() {
            if(_indicatorInstance)
                PoolManager.Return(_indicatorInstance);
        }

        public override void OnSkillStart() {
            base.OnSkillStart();
            
            Owner.Animancer.Play(_slashClip);
            _timer = 0.0f;
            _startPosition = Owner.CharacterController.Motor.TransientPosition;
            Vector3 direction = Owner.CharacterController.LastNonZeroLookInput;
            _endPosition = _startPosition + (direction * _distance);
            Owner.CharacterController.Motor.SetRotation(Quaternion.LookRotation(direction));
        }

        public override void OnSkillTick(float deltaTime) {
            MoveCharacter(deltaTime);
            CheckForHittables();
        }

        private void MoveCharacter(float dt) {
            _timer += dt;
            float t = _curve.Evaluate(_timer / _skillDuration);
            Vector3 desiredPos = Vector3.Lerp(_startPosition, _endPosition, t);
            // Owner.CharacterController.Motor.MoveCharacter(desiredPos); // this considers collisons
            Owner.CharacterController.Motor.SetPosition(desiredPos); // this teleports through
        }

        private void CheckForHittables() {
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
                        damage = 1,
                        instigator = Owner,
                        statusToApply = _statusToApply.GetInstance(),
                        position = collider.ClosestPoint(Owner.FeetPosition),
                        direction = Owner.FeetPosition.DirectionTo(collider.transform.position)
                    };
                    
                    PoolManager.Spawn(_redImpaxtVFX, hitData.position, Quaternion.identity);
                    hittable.Hit(hitData);
                    _hittables.Add(hittable);
                }
            }
        }

        public override void OnSkillFinish() => Owner.CharacterController.SetLastDirectionToForward();
    }
}
