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
        [SerializeField] private GameEvent _perfectHeavyHitEvent;
        [SerializeField] private GameEvent _lightDashHitEvent;
        [SerializeField] private GameEvent _heavyDashHitEvent;

        private int _attackIndex;
        private bool _heavyAttackHeld;
        private bool _enteredFromDash;

        public bool IsOnCooldown => CurrentWeapon.IsOnCooldown;
        public bool IsDuringAttack => CurrentWeapon.IsDuringAttack;
        public bool IsDuringLastAttack => _attackIndex >= CurrentWeapon.LastLightAttackIndex;
        public bool IsDuringInputBuffering => _preAttackBuffer.IsActive || _postAttackBuffer.IsActive;
        
        private WeaponMelee CurrentWeapon => Parent.WeaponController.MeleeWeapon;

        protected override void Enable() => _postAttackBuffer.OnEnd += OnPostInputBufferEnd;
        protected override void Disable() => _postAttackBuffer.OnEnd -= OnPostInputBufferEnd;

        public override void OnEnter() {
            ResetAttackVariables();
            CurrentWeapon.OnWeaponStart();
            _enteredFromDash = Controller.LastState == Controller.RollModule; 
        }

        public override void OnExit() {
            ResetAttackVariables();
            CurrentWeapon.OnWeaponStop();
        }

        private void ResetAttackVariables() {
            _attackIndex = 0;
            _heavyAttackHeld = false;
            _postAttackBuffer.Reset();
        }

        private void OnPostInputBufferEnd() {
            if (IsDuringLastAttack || (!IsDuringAttack && !_heavyAttackHeld))
                Controller.TransitionToDefaultState();
            else
                TryResetWeaponHoldTimer();
        }

        public void OnAttackEnd() {
            if (IsDuringLastAttack)
                CurrentWeapon.StartCooldown();
            else
                _postAttackBuffer.Start();

            if (!IsDuringInputBuffering && !_heavyAttackHeld)
                Controller.TransitionToDefaultState();
            else 
                TryResetWeaponHoldTimer();
        }

        private void TryResetWeaponHoldTimer() {
            if (_heavyAttackHeld && !IsDuringLastAttack)
                CurrentWeapon.OnWeaponStart();
        }
        
        public override void SetInputs(CharacterInputs inputs) {
            if(IsDuringLastAttack)
                return;
            
            _heavyAttackHeld = inputs.Melee.Held;

            if (_heavyAttackHeld) {
                Parent.OnHeavyAttackHeld();
                CurrentWeapon.AttackHeld();
            }

            if (inputs.Melee.Released) {
                if (!IsDuringAttack) {
                    if (CurrentWeapon.MinInputReached) {
                        if (_enteredFromDash)
                            DashHeavyAttack();
                        else 
                            HeavyAttack();
                    }
                    else {
                        if (_enteredFromDash)
                            DashLightAttack();
                        else
                            LightAttack();
                    }
                }
                else
                    _preAttackBuffer.Start();

                _enteredFromDash = false; // TODO fix to DuringRoll;
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
            Parent.AnimationController.UnpauseGraph();
            _heavyDashAttackEvent?.Raise(this);
            CurrentWeapon.DashHeavyAttack();
        }

        private void LightAttack() {
            _preAttackBuffer.Reset();
            Parent.OnLightAttack(_attackIndex);
            CurrentWeapon.LightAttack(_attackIndex);
            _lightAttackEvent?.Raise(this);
            _attackIndex++;
        }

        private void HeavyAttack() {
            ResetAttackVariables();
            CurrentWeapon.OnAttackReleased();
            _heavyAttackEvent?.Raise(this);
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            float t = 1 - Mathf.Exp(-_slowDownSharpness * deltaTime);
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, t);
        }

        public void OnAttackHit(HitData hitData) {
            Parent.OnMeleeHit(hitData);
            Parent.LastDealtData = hitData;
            
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

        public override bool CanEnterState() => !IsOnCooldown;
    }
}