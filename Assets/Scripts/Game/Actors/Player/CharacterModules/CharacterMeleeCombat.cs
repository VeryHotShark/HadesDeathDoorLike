using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using MEC;
using Sirenix.OdinInspector;
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
            [Range(0.0f,180.0f)] public float angle = 180.0f;
        }

        [Header("VFX")]
        [SerializeField] private DamagePopUp _damagePopUp;
        [SerializeField] private GameObject _slashParticle;

        [Header("General")]
        [SerializeField] private float _slowDownSharpness = 10.0f;
        [SerializeField] private Timer _comboCooldown = new(0.5f);
        
        [Header("Input Buffers")] 
        [SerializeField] private float _dashLightBuffer = 0.4f;
        [SerializeField] private float _dashHeavyBuffer = 0.6f;

        [SerializeField] private Timer _preAttackBuffer = new(0.3f);
        [SerializeField] private Timer _postAttackBuffer = new(0.3f);

        [Header("Events")] 
        [SerializeField] private GameEvent _lightAttackEvent;
        [SerializeField] private GameEvent _heavyAttackEvent;
        [SerializeField] private GameEvent _lightDashAttackEvent;
        [SerializeField] private GameEvent _heavyDashAttackEvent;

        [Space]
        [SerializeField] private GameEvent _anyHitEvent;
        [SerializeField] private GameEvent _lightHitEvent;
        [SerializeField] private GameEvent _heavyHitEvent;
        [SerializeField] private GameEvent _lightDashHitEvent;
        [SerializeField] private GameEvent _heavyDashHitEvent;
        

        [Header("Attacks")] [SerializeField] private List<AttackInfo> _lightAttacks;
        [SerializeField] private AttackInfo _heavyAttack;
        [SerializeField] private AttackInfo _dashLightAttack;
        [SerializeField] private AttackInfo _dashHeavyAttack;

        [Header("Hit")] [SerializeField] private float _hitDelay = 0.2f;
        [SerializeField] private MMF_Player _lightHitFeedback;
        [SerializeField] private MMF_Player _heavyHitFeedback;

        private GameObject _slash;

        private int _attackIndex;

        private bool _lastAttackPrimary;
        private bool _heavyAttackHeld;
        private bool _heavyAttackReached;

        private AttackInfo _currentAttack;

        public Timer CurrentAttackTimer => _currentAttack.duration;

        public bool IsOnCooldown => _comboCooldown.IsActive;
        public bool IsDuringAttack => CurrentAttackTimer.IsActive;
        public bool IsDuringLastAttack => _attackIndex >= _lightAttacks.Count;
        public bool IsDuringInputBuffering => _preAttackBuffer.IsActive || _postAttackBuffer.IsActive;

        private void Awake() => _currentAttack = _lightAttacks[0];

        private void OnEnable() {
            foreach (AttackInfo attack in _lightAttacks)
                attack.duration.OnEnd += OnAttackEnd;

            _dashLightAttack.duration.OnEnd += OnAttackEnd;
            _dashHeavyAttack.duration.OnEnd += OnAttackEnd;
            _heavyAttack.duration.OnEnd += OnAttackEnd;
            _postAttackBuffer.OnEnd += OnPostInputBufferEnd;
        }

        private void OnDisable() {
            foreach (AttackInfo attack in _lightAttacks)
                attack.duration.OnEnd -= OnAttackEnd;

            _dashLightAttack.duration.OnEnd -= OnAttackEnd;
            _dashHeavyAttack.duration.OnEnd -= OnAttackEnd;
            _heavyAttack.duration.OnEnd -= OnAttackEnd;
            _postAttackBuffer.OnEnd -= OnPostInputBufferEnd;
        }

        private void OnPostInputBufferEnd() {
            if (!IsDuringAttack && !_heavyAttackHeld)
                Controller.TransitionToDefaultState();
        }

        private void OnAttackEnd() {
            if (IsDuringLastAttack)
                _comboCooldown.Start();
            else
                _postAttackBuffer.Start();

            if (!IsDuringInputBuffering && !_heavyAttackHeld)
                Controller.TransitionToDefaultState();
        }

        public override void OnEnter() => _attackIndex = 0;
        public override void OnExit() => _attackIndex = 0;

        private void ResetPrimaryAttack() {
            _attackIndex = 0;
            _lastAttackPrimary = false;
        }

        private void DashLightAttack() {
            ResetPrimaryAttack();
            _lightDashAttackEvent?.Raise(this);
            SpawnAttack(_dashLightAttack, new Vector3(0.3f, 0.3f, 1f));
        }

        private void DashHeavyAttack() {
            ResetPrimaryAttack();
            _heavyDashAttackEvent?.Raise(this);
            SpawnAttack(_dashHeavyAttack, new Vector3(1, 1f, 0.4f));
        }

        private void LightAttack() {
            _lastAttackPrimary = true;
            _preAttackBuffer.Reset();

            _lightAttackEvent?.Raise(this);
            SpawnAttack(_lightAttacks[_attackIndex], Vector3.one * 0.25f, _attackIndex % 2 == 0);
            _attackIndex++;
        }

        private void HeavyAttack() {
            ResetPrimaryAttack();
            _heavyAttackEvent?.Raise(this);
            SpawnAttack(_heavyAttack, Vector3.one * 0.4f);
            _heavyAttackReached = false;
        }

        private void SpawnAttack(AttackInfo attackInfo, Vector3 slashSize, bool flipSlash = false) {
            _currentAttack = attackInfo;
            Attack(_currentAttack);
            SpawnSlash(slashSize, flipSlash);
        }

        private void Attack(AttackInfo attackInfo) {
            attackInfo.duration.Start();

            Motor.SetRotation(Controller.LastCharacterInputs.CursorRotation);
            Controller.LastNonZeroMoveInput = Controller.LookInput;
            Controller.AddVelocity(attackInfo.pushForce * Controller.LookInput);

            Timing.CallDelayed(_hitDelay, () => CheckForHittables(attackInfo), gameObject);
        }

        private void SpawnSlash(Vector3 size, bool flip = false) {
            if (_slash)
                Destroy(_slash);

            Quaternion rot = Quaternion.LookRotation(Controller.LookInput);
            _slash = Instantiate(_slashParticle, Parent.CenterOfMass, rot, Parent.transform);

            size.z *= 1.2f;

            if (flip)
                size.x *= -1.0f;

            _slash.transform.localScale = size;
        }

        private void CheckForHittables(AttackInfo attackInfo) {
            Vector3 position = Motor.TransientPosition + Motor.CharacterTransformToCapsuleCenter +
                               Motor.CharacterForward * attackInfo.zOffset;
            
            DebugExtension.DebugWireSphere(position, _lastAttackPrimary ? Color.yellow : Color.red, attackInfo.radius,
                attackInfo.duration.Duration);

            Collider[] _colliders =
                Physics.OverlapSphere(position, attackInfo.radius, LayerManager.Masks.DEFAULT_AND_NPC);

            bool hitSomething = false;
            
            if(_colliders.Length == 0)
                return;

            foreach (Collider collider in _colliders) {
                IHittable hittable = collider.GetComponentInParent<IHittable>();
                

                Vector3 hitDirection = Parent.FeetPosition.DirectionTo(collider.transform.position).Flatten();

                float dot = Vector3.Dot(Controller.ControlledCharacter.Forward, hitDirection);
                float hitAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;

                if (hittable == null || hitAngle > attackInfo.angle)
                    continue;
                
                HitData hitData = new HitData {
                    damage = 1,
                    actor = Parent,
                    position = collider.ClosestPoint(Parent.CenterOfMass),
                    direction = hitDirection
                };
                
                hitSomething = true;
                hittable.Hit(hitData);
                Parent.OnMeleeHit(hitData);

                /* Damage PopUp
                Quaternion rotationToCamera = Quaternion.LookRotation(Player.Camera.transform.forward);
                DamagePopUp damagePopUp = PoolManager.Spawn(_damagePopUp, collider.gameObject.transform.position + Vector3.up * 3.0f ,rotationToCamera);
                damagePopUp.transform.localScale = Vector3.one * 0.15f;
                damagePopUp.Init(hitData.damage, 1.0f);
                */
            }

            if (hitSomething) {
                if (_lastAttackPrimary)
                    _lightHitFeedback.PlayFeedbacks();
                else
                    _heavyHitFeedback.PlayFeedbacks();
                
                _anyHitEvent?.Raise(this);
            }
        }

        public override void SetInputs(CharacterInputs inputs) {
            if (IsDuringLastAttack) {
                _heavyAttackHeld = false;
                return;
            }

            _heavyAttackHeld = inputs.Primary.Held;

            if (inputs.Primary.Performed)
                _heavyAttackReached = true;

            if (inputs.Primary.Released) {
                if (!CurrentAttackTimer.IsActive) {
                    float _attackSinceRoll = Time.time - Controller.RollModule.LastRollTimestamp;

                    //TODO Fix this -> Controller.RollModule.DuringRoll
                    if (_heavyAttackReached) {
                        if (_attackSinceRoll < _dashHeavyBuffer)
                            DashHeavyAttack();
                        else 
                            HeavyAttack();
                    }
                    else {
                        if (_attackSinceRoll < _dashLightBuffer)
                            DashLightAttack();
                        else
                            LightAttack();
                    }
                }
                else
                    _preAttackBuffer.Start();
            }

            // Handle Pre Input Buffering
            if (!CurrentAttackTimer.IsActive && _preAttackBuffer.IsActive)
                LightAttack();
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            float t = 1 - Mathf.Exp(-_slowDownSharpness * deltaTime);
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, t);
        }
    }
}