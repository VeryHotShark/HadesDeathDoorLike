using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace VHS {
    public class CharacterMeleeCombat : CharacterModule {
        [Serializable]
        public struct AttackInfo {
            
        }
        
        [SerializeField] private MMF_Player _hitFeedback;
        [SerializeField] private float _pushForce = 10.0f;
        [SerializeField] private Timer _attackTimer = new Timer(0.3f);

        [Header("Combo")] 
        [SerializeField] private int _maxComboChain = 3;
        [SerializeField] private Timer _comboCooldown = new Timer(0.5f);
        [SerializeField] private Timer _attackBufferTimer = new Timer(0.2f);

        [Header("Collision Check")]
        [SerializeField] private float _hitDelay = 0.2f;
        [SerializeField] private float _radius = 0.5f;
        [SerializeField] private float _zOffset = 2f;

        private int _attackCount = 0;

        public Timer AttackTimer => _attackTimer;
        public Timer ComboCooldown => _comboCooldown;
        public Timer AttackBufferTimer => _attackBufferTimer;

        public bool IsOnCooldown => _comboCooldown.IsActive;
        public bool IsDuringAttack => _attackTimer.IsActive || _attackBufferTimer.IsActive;

        public override void OnEnter() {
            _attackCount = 0;
            Attack();
        }

        public override void OnExit() {
            _attackCount = 0;
        }

        private void Attack() {
            _attackTimer.Start();
            _attackBufferTimer.Reset();
            Motor.SetRotation(Controller.LastCharacterInputs.CursorRotation);
            Controller.LastNonZeroMoveInput = Controller.LookInput;
            Controller.AddVelocity(_pushForce * Controller.LookInput);
            
            _attackCount++;

            if (_attackCount >= _maxComboChain)
                _comboCooldown.Start();
            
            this.CallWithDelay(() => CheckForHittables(), _hitDelay);
        }

        private void CheckForHittables() {
            Vector3 position = Motor.TransientPosition + Motor.CharacterTransformToCapsuleCenter +
                               Motor.CharacterForward * _zOffset;

            Collider[] _colliders = Physics.OverlapSphere(position, _radius, LayerManager.Masks.DEFAULT_AND_NPC);

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
            
            DebugExtension.DebugWireSphere(position, _radius, 1.0f);
            
            if(hitSomething && _hitFeedback)
                _hitFeedback.PlayFeedbacks();
        }

        public override void SetInputs(CharacterInputs inputs) {
            Controller.MoveInput = Vector3.zero;

            if (_comboCooldown.IsActive)
                return;

            if (inputs.AttackDown)
                _attackBufferTimer.Start();

            if (!_attackTimer.IsActive && _attackBufferTimer.IsActive)
                Attack();
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) { }
    }
}