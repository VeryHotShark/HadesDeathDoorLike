using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace VHS {
    public class CharacterMeleeCombat : CharacterModule {
        [Serializable]
        public class AttackInfo {
            public Timer duration = new(0.3f);
            public float pushForce = 10.0f;
            public float zOffset = 1.0f;
            public float radius = 1.0f;
        }

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
        private AttackInfo _currentAttack;

        public Timer ComboCooldown => _comboCooldown;
        public Timer AttackTimer => _currentAttack.duration;
        public Timer PreAttackBufferTimer => _preAttackBufferTimer;

        public bool IsOnCooldown => _comboCooldown.IsActive;
        public bool IsDuringAttack => AttackTimer.IsActive;
        public bool IsDurinLastAttack => _attackIndex >= _attacks.Count;
        public bool IsDuringInputBuffering => _preAttackBufferTimer.IsActive || _postAttackBufferTimer.IsActive;

        private void Awake() => _currentAttack = _attacks[0];

        private void OnEnable() {
            foreach (AttackInfo attack in _attacks) 
                attack.duration.OnEnd += OnAttackEnd;

            _postAttackBufferTimer.OnEnd += OnPostInputBufferEnd;
        }

        private void OnDisable() {
            foreach (AttackInfo attack in _attacks) 
                attack.duration.OnEnd -= OnAttackEnd;
            
            _postAttackBufferTimer.OnEnd -= OnPostInputBufferEnd;
        }

        private void OnPostInputBufferEnd() {
            if(!IsDuringAttack)
                Controller.TransitionToDefaultState();
        }

        private void OnAttackEnd() {
            if(!IsDurinLastAttack)
                _postAttackBufferTimer.Start();
            else
                _comboCooldown.Start();
            
            if(!IsDuringInputBuffering)
                Controller.TransitionToDefaultState();
        }

        public override void OnEnter() => _attackIndex = 0;
        public override void OnExit() => _attackIndex = 0;

        private void Attack() {
            _currentAttack = _attacks[_attackIndex];
            
            _preAttackBufferTimer.Reset();
            _currentAttack.duration.Start();
            Motor.SetRotation(Controller.LastCharacterInputs.CursorRotation);
            Controller.LastNonZeroMoveInput = Controller.LookInput;
            Controller.AddVelocity(_currentAttack.pushForce * Controller.LookInput);
            
            _attackIndex++;

            this.CallWithDelay(CheckForHittables, _hitDelay);
        }

        private void CheckForHittables() {
            Vector3 position = Motor.TransientPosition + Motor.CharacterTransformToCapsuleCenter +
                               Motor.CharacterForward * _currentAttack.zOffset;

            Collider[] _colliders = Physics.OverlapSphere(position, _currentAttack.radius, LayerManager.Masks.DEFAULT_AND_NPC);

            bool hitSomething = false;
            
            if (_colliders.Length > 0) {
                foreach (Collider collider in _colliders) {
                    IHittable hittable = collider.GetComponentInParent<IHittable>();

                    if (hittable != null) {
                        hittable.OnHit();
                        hitSomething = true;   
                    }
                }
            } 
            
            DebugExtension.DebugWireSphere(position, _currentAttack.radius, 1.0f);
            
            if(hitSomething && _hitFeedback)
                _hitFeedback.PlayFeedbacks();
        }

        public override void SetInputs(CharacterInputs inputs) {
            Controller.MoveInput = Vector3.zero;

            if (IsDurinLastAttack)
                return;

            if (inputs.AttackDown) {
                if(AttackTimer.IsActive)   
                    _preAttackBufferTimer.Start();
                else 
                    Attack();
            }

            // Handle Pre Input Buffering
            if (!AttackTimer.IsActive && _preAttackBufferTimer.IsActive) 
                Attack();
            
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) { }
    }
}