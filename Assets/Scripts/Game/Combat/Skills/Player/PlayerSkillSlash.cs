using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

namespace VHS {
    public class PlayerSkillSlash : PlayerSkill {
        [Header("GFX")]
        public ParticleController _redImpaxtVFX;
        public SkillIndicator _skillIndicator;
        public ClipTransition _windupClip;
        public ClipTransition _slashClip;
        
        
        [Header("settins")]
        public float _distance = 10.0f;
        public float _radius = 3.0f;
        public AnimationCurve _curve = AnimationCurve.EaseInOut(0,0,1,1);

        private HashSet<IHittable> _hittables = new HashSet<IHittable>();
        private Collider[] _colliders = new Collider[32];
        private SkillIndicator _indicatorInstance;

        private float _timer;
        private float _speed;
        private Vector3 _endPosition;
        private Vector3 _cursorPosition;
        private Vector3 _startPosition;
        
        public override void OnReset() {
            _hittables.Clear();
        }
        
        public override void OnCastStart() {
            Quaternion rotationToCursor = Quaternion.LookRotation(Owner.PlayerController.CharacterDirectionToCursor);
            Owner.Animancer.Play(_windupClip);
            _indicatorInstance = PoolManager.Spawn(_skillIndicator,
                Owner.CharacterController.Motor.TransientPosition, rotationToCursor);
            _indicatorInstance.InitRectangle(_distance, _radius);
        }
        
        public override void OnCastTick(float deltaTime) {
            _cursorPosition = Owner.PlayerController.Camera.CursorTransform.position;
            _indicatorInstance.transform.rotation = Quaternion.LookRotation(Owner.PlayerController.CharacterDirectionToCursor);
            DebugExtension.DebugWireSphere(_cursorPosition, Color.yellow, _radius);
        }
        
        public override void OnCastFinish() {
            PoolManager.Return(_indicatorInstance);
        }

        public override void OnAbort() {
            if(_indicatorInstance)
                PoolManager.Return(_indicatorInstance);
        }

        public override void OnSkillStart() {
            Owner.Animancer.Play(_slashClip);
            _timer = 0.0f;
            _startPosition = Owner.CharacterController.Motor.TransientPosition;
            Vector3 direction = _startPosition.DirectionTo(_cursorPosition).Flatten();
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
                        position = collider.ClosestPoint(_cursorPosition),
                        direction = _cursorPosition.DirectionTo(collider.transform.position)
                    };

                    PoolManager.Spawn(_redImpaxtVFX, hitData.position, Quaternion.identity);
                    hittable.Hit(hitData);
                    _hittables.Add(hittable);
                }
            }
        }

        public override void OnSkillFinish() {
            Owner.CharacterController.SetLastDirectionToForward();
        }
    }
}
