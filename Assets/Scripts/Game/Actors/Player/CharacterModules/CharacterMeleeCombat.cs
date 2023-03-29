using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

namespace VHS {
    public class CharacterMeleeCombat : CharacterModule {

        [Header("General")]
        [SerializeField] private float _slowDownSharpness = 10.0f;
        
        [Header("Input Buffers")] 
        [SerializeField] private float _dashLightBuffer = 0.4f;
        [SerializeField] private float _dashHeavyBuffer = 0.6f;

        [SerializeField] private Timer _preAttackBuffer = new(0.3f);
        [SerializeField] private Timer _postAttackBuffer = new(0.3f);
        
        [SerializeField] private Timer _perfectHeavyBuffer = new(0.3f);

        [Header("Events")] 
        [SerializeField] private GameEvent _lightAttackEvent;
        [SerializeField] private GameEvent _heavyAttackEvent;
        [SerializeField] private GameEvent _lightDashAttackEvent;
        [SerializeField] private GameEvent _heavyDashAttackEvent;

        [Space]
        [SerializeField] private GameEvent _anyHitEvent;
        [SerializeField] private GameEvent _lightHitEvent;
        [SerializeField] private GameEvent _heavyHitEvent;
        [SerializeField] private GameEvent _perfectHeavyHitEvent;
        [SerializeField] private GameEvent _lightDashHitEvent;
        [SerializeField] private GameEvent _heavyDashHitEvent;

        private int _lightAttackIndex;
        
        private bool _heavyAttackHeld;
        private bool _heavyAttackReached;

        public bool IsOnCooldown => CurrentWeapon.IsOnComboCooldown;
        public bool IsDuringAttack => CurrentWeapon.IsDuringAttack;
        public bool IsDuringLastAttack => _lightAttackIndex >= CurrentWeapon.LastLightAttackIndex;
        public bool IsDuringInputBuffering => _preAttackBuffer.IsActive || _postAttackBuffer.IsActive;
        
        private Weapon CurrentWeapon => Parent.WeaponController.MeleeWeapon;

        protected override void Enable() => _postAttackBuffer.OnEnd += OnPostInputBufferEnd;

        protected override void Disable() => _postAttackBuffer.OnEnd -= OnPostInputBufferEnd;

        public override void OnEnter() => ResetAttackVariables();
        public override void OnExit() => ResetAttackVariables();

        private void ResetAttackVariables() {
            _lightAttackIndex = 0;
            _heavyAttackReached = false;
        }

        private void OnPostInputBufferEnd() {
            if (!IsDuringAttack && !_heavyAttackHeld)
                Controller.TransitionToDefaultState();
        }

        public void OnAttackEnd() {
            if (IsDuringLastAttack)
                CurrentWeapon.ComboCooldown.Start();
            else
                _postAttackBuffer.Start();

            if (!IsDuringInputBuffering && !_heavyAttackHeld)
                Controller.TransitionToDefaultState();
        }
        
        public override void SetInputs(CharacterInputs inputs) {
            if (IsDuringLastAttack) {
                _heavyAttackHeld = false;
                return;
            }

            _heavyAttackHeld = inputs.Primary.Held;

            if (_heavyAttackHeld)
                OnHeavyAttackHeld();

            if (inputs.Primary.Performed) {
                _heavyAttackReached = true;
                OnHeavyAttackReached();
            }

            if (inputs.Primary.Released) {
                if (!IsDuringAttack) {
                    float _attackSinceRoll = Time.time - Controller.RollModule.LastRollTimestamp;

                    //TODO Fix this -> Controller.RollModule.DuringRoll by ten check działał
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
            if (!IsDuringAttack && _preAttackBuffer.IsActive)
                LightAttack();
        }

        private void DashLightAttack() {
            ResetAttackVariables();
            _lightDashAttackEvent?.Raise(this);
            CurrentWeapon.DashLightAttack();
        }

        private void DashHeavyAttack() {
            ResetAttackVariables();
            Parent.Animancer.Playable.UnpauseGraph();
            _heavyDashAttackEvent?.Raise(this);
            CurrentWeapon.DashHeavyAttack();
        }

        private void LightAttack() {
            _preAttackBuffer.Reset();
            Parent.OnLightAttack(_lightAttackIndex);
            CurrentWeapon.LightAttack(_lightAttackIndex);
            _lightAttackEvent?.Raise(this);
            _lightAttackIndex++;
        }

        private void HeavyAttack() {
            ResetAttackVariables();
            Parent.Animancer.Playable.UnpauseGraph();
            Parent.OnHeavyAttack();
            CurrentWeapon.HeavyAttack();
            _heavyAttackEvent?.Raise(this);
        }

        private void OnHeavyAttackReached() {
            Parent.Animancer.Playable.PauseGraph();
            Parent.OnHeavyAttackReached();
            CurrentWeapon.OnHeavyAttackReached();
        }

        private void OnHeavyAttackHeld() {
            Parent.OnHeavyAttackHeld();
            CurrentWeapon.OnHeavyAttackHeld();
        }

        private void HeavyPerfectAttack() {
            
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            float t = 1 - Mathf.Exp(-_slowDownSharpness * deltaTime);
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, t);
        }
        

        public void OnAttackHit(HitData hitData) {
            Parent.OnMeleeHit(hitData);
            
            switch (hitData.playerAttackType) {
                case PlayerAttackType.LIGHT:
                    _lightHitEvent?.Raise(this);
                    break;
                case PlayerAttackType.HEAVY:
                    _heavyHitEvent?.Raise(this);
                    break;
                case PlayerAttackType.PERFECT_HEAVY:
                    _perfectHeavyHitEvent?.Raise(this);
                    break;
                case PlayerAttackType.DASH_LIGHT:
                    _lightDashHitEvent?.Raise(this);
                    break;
                case PlayerAttackType.DASH_HEAVY:
                    _heavyDashHitEvent?.Raise(this);
                    break;
            }
            
            _anyHitEvent?.Raise(this);
        }
    }
}