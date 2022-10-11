using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using MEC;

namespace VHS {
    /// <summary>
    /// This whole thing might be moved to Skill
    /// </summary>
    public class CharacterMeleeCombat : CharacterModule {
        [Serializable]
        public class AttackInfo {
            public Timer duration = new(0.3f);
            public float pushForce = 10.0f;
            public float zOffset = 1.0f;
            public float radius = 1.0f;
        }

        [SerializeField] private float _slowDownSharpness = 10.0f;
        
        [Header("Heavy Attack")]
        [SerializeField] private float _heavyAttackThreshold = 0.2f;

        [SerializeField] private AttackInfo _heavyAttackInfo;

        [Header("Attacks")] 
        [SerializeField] private List<AttackInfo> _attacks;
        [SerializeField] private Timer _comboCooldown = new(0.5f);
        [SerializeField] private Timer _preAttackBufferTimer = new(0.3f);
        [SerializeField] private Timer _postAttackBufferTimer = new(0.3f);

        [Header("Hit")]
        [SerializeField] private float _hitDelay = 0.2f;
        [SerializeField] private MMF_Player _hitFeedback;

        private int _attackIndex;
        private bool _duringLastAtttack;
        private bool _heavyAttackPerformed;
        private AttackInfo _currentAttack;

        public Timer ComboCooldown => _comboCooldown;
        public Timer AttackTimer => _currentAttack.duration;
        public Timer PreAttackBufferTimer => _preAttackBufferTimer;

        public bool IsOnCooldown => _comboCooldown.IsActive;
        public bool IsDuringAttack => AttackTimer.IsActive || _heavyAttackInfo.duration.IsActive;
        public bool IsDuringLastAttack => _attackIndex >= _attacks.Count;
        public bool IsDuringInputBuffering => _preAttackBufferTimer.IsActive || _postAttackBufferTimer.IsActive;

        private void Awake() => _currentAttack = _attacks[0];

        private void OnEnable() {
            foreach (AttackInfo attack in _attacks) 
                attack.duration.OnEnd += OnAttackEnd;

            _heavyAttackInfo.duration.OnEnd += OnAttackEnd;
            _postAttackBufferTimer.OnEnd += OnPostInputBufferEnd;
        }

        private void OnDisable() {
            foreach (AttackInfo attack in _attacks) 
                attack.duration.OnEnd -= OnAttackEnd;
            
            _heavyAttackInfo.duration.OnEnd -= OnAttackEnd;
            _postAttackBufferTimer.OnEnd -= OnPostInputBufferEnd;
        }

        private void OnPostInputBufferEnd() {
            if(!IsDuringAttack)
                Controller.TransitionToDefaultState();
        }

        private void OnAttackEnd() {
            if(!IsDuringLastAttack)
                _postAttackBufferTimer.Start();
            else
                _comboCooldown.Start();
            
            if(!IsDuringInputBuffering)
                Controller.TransitionToDefaultState();
        }

        public override void OnEnter() {
            _heavyAttackPerformed = false;
            _attackIndex = 0;
        }

        public override void OnExit() => _attackIndex = 0;

        private void Attack(AttackInfo attackInfo) {
            _preAttackBufferTimer.Reset();
            attackInfo.duration.Start();

            Motor.SetRotation(Controller.LastCharacterInputs.CursorRotation);
            Controller.LastNonZeroMoveInput = Controller.LookInput;
            Controller.AddVelocity(attackInfo.pushForce * Controller.LookInput);
            
            Timing.CallDelayed(_hitDelay, () => CheckForHittables(attackInfo), gameObject);
        }

        private void LightAttack() {
            _currentAttack = _attacks[_attackIndex];
            Attack(_currentAttack);
            _attackIndex++;
        }

        private void HeavyAttack() {
           Attack(_heavyAttackInfo);
        }

        private void CheckForHittables(AttackInfo attackInfo) {
            Vector3 position = Motor.TransientPosition + Motor.CharacterTransformToCapsuleCenter +
                               Motor.CharacterForward * attackInfo.zOffset;

            Collider[] _colliders = Physics.OverlapSphere(position, attackInfo.radius, LayerManager.Masks.DEFAULT_AND_NPC);

            bool hitSomething = false;
            
            if (_colliders.Length > 0) {
                foreach (Collider collider in _colliders) {
                    IHittable hittable = collider.GetComponentInParent<IHittable>();

                    if (hittable != null) {
                        HitData hitData = new HitData {
                            damage = 1,
                            dealer = Parent,
                            position = collider.ClosestPoint(Parent.CenterOfMass),
                            direction = Parent.FeetPosition.DirectionTo(collider.transform.position)
                        };
                            
                        hittable.Hit(hitData);
                        hitSomething = true;   
                    }
                }
            } 
            
            DebugExtension.DebugWireSphere(position, attackInfo.radius, 1.0f);
            
            if(hitSomething && _hitFeedback)
                _hitFeedback.PlayFeedbacks();
        }

        public override void SetInputs(CharacterInputs inputs) {
            // Controller.MoveInput = Vector3.zero;

            if (IsDuringLastAttack)
                return;
            
            if (inputs.PrimaryAttackHeld) {
                _heavyAttackPerformed = true;
                HeavyAttack();                
            }
            else if (inputs.PrimaryAttackUp ) {
                if (!_heavyAttackPerformed) {
                    if(AttackTimer.IsActive)   
                        _preAttackBufferTimer.Start();
                    else 
                        LightAttack();
                }
                else
                    _heavyAttackPerformed = false;
            }

            // Handle Pre Input Buffering
            if (!AttackTimer.IsActive && _preAttackBufferTimer.IsActive) 
                LightAttack();
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            float t = 1 - Mathf.Exp(-_slowDownSharpness * deltaTime);
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, t);
        }
    }
}