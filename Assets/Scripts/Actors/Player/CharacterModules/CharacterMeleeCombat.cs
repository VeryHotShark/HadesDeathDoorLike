using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using MEC;
using UnityEngine.Serialization;

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
        
        private enum AttackType {
            Light,
            Heavy
        } 

        [Header("General")]
        [SerializeField] private float _slowDownSharpness = 10.0f;
        [SerializeField] private Timer _comboCooldown = new(0.5f);
        [SerializeField] private Timer _preAttackBufferTimer = new(0.3f);
        [SerializeField] private Timer _postAttackBufferTimer = new(0.3f);

        [FormerlySerializedAs("_attacks")]
        [Header("Attacks")] 
        [SerializeField] private List<AttackInfo> _lightAttacks;
        [SerializeField] private List<AttackInfo> _heavyAttacks;
        
        [Header("Hit")]
        [SerializeField] private float _hitDelay = 0.2f;
        [FormerlySerializedAs("_hitFeedback")] [SerializeField] private MMF_Player _lightHitFeedback;
        [SerializeField] private MMF_Player _heavyHitFeedback;

        private int _comboIndex;
        private int _attackIndex;
        
        private bool _duringLastAttack;
        private bool _lastAttackPrimary;
        private bool _heavyAttackPerformed;

        private Queue<AttackType> _queuedAttacks = new Queue<AttackType>();

        private AttackInfo _currentAttack;

        public Timer ComboCooldown => _comboCooldown;
        public Timer AttackTimer => _currentAttack.duration;
        public Timer PreAttackBufferTimer => _preAttackBufferTimer;

        public bool IsOnCooldown => _comboCooldown.IsActive;
        public bool IsDuringAttack => AttackTimer.IsActive;
        public bool IsDuringLastAttack => _attackIndex >=
                                          (_lastAttackPrimary ? _lightAttacks.Count : _heavyAttacks.Count);
        public bool IsDuringInputBuffering => _preAttackBufferTimer.IsActive || _postAttackBufferTimer.IsActive;

        private void Awake() => _currentAttack = _lightAttacks[0];

        private void OnEnable() {
            foreach (AttackInfo attack in _lightAttacks) 
                attack.duration.OnEnd += OnAttackEnd;

            foreach (AttackInfo attack in _heavyAttacks)
                attack.duration.OnEnd += OnAttackEnd;
                
            _postAttackBufferTimer.OnEnd += OnPostInputBufferEnd;
        }

        private void OnDisable() {
            foreach (AttackInfo attack in _lightAttacks) 
                attack.duration.OnEnd -= OnAttackEnd;
            
            foreach (AttackInfo attack in _heavyAttacks)
                attack.duration.OnEnd -= OnAttackEnd;
            
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
            _queuedAttacks.Clear();
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
            if (!_lastAttackPrimary)
                _attackIndex = 0;
            
            _lastAttackPrimary = true;
            
            _currentAttack = _lightAttacks[_attackIndex];
            Attack(_currentAttack);
            _attackIndex++;
        }

        private void HeavyAttack() {
            _heavyAttackPerformed = true;
            
            if (_lastAttackPrimary)
                _attackIndex = 0;
            
            _lastAttackPrimary = false;

            _currentAttack = _heavyAttacks[_attackIndex];
           Attack(_currentAttack);
           _attackIndex++;
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
            
            DebugExtension.DebugWireSphere(position,_lastAttackPrimary ? Color.yellow : Color.red, attackInfo.radius, attackInfo.duration.Duration);

            if (hitSomething) {
                if(_lastAttackPrimary)   
                    _lightHitFeedback.PlayFeedbacks();
                else
                    _heavyHitFeedback.PlayFeedbacks();
            }
        }

        public override void SetInputs(CharacterInputs inputs) {
            if (IsDuringLastAttack)
                return;

            if (inputs.PrimaryAttackPerformed) {
                if (AttackTimer.IsActive) {
                    _preAttackBufferTimer.Start();
                    _queuedAttacks.Enqueue(AttackType.Heavy);   
                }
                else
                    HeavyAttack();
            }
            
            if (inputs.PrimaryAttackReleased ) {
                if (_heavyAttackPerformed) 
                    _heavyAttackPerformed = false;
                else
                {
                    if (AttackTimer.IsActive) {
                        _preAttackBufferTimer.Start();
                        _queuedAttacks.Enqueue(AttackType.Light);   
                    }
                    else
                        LightAttack();
                }
            }

            // Handle Pre Input Buffering
            if (!AttackTimer.IsActive && _preAttackBufferTimer.IsActive) {
                if (_queuedAttacks.Count > 0) {
                    AttackType queuedAttack = _queuedAttacks.Dequeue();
                    
                    if(queuedAttack == AttackType.Light)
                        LightAttack();
                    else
                        HeavyAttack();
                }
            } 
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            float t = 1 - Mathf.Exp(-_slowDownSharpness * deltaTime);
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, t);
        }
    }
}