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

        [Header("Events")] 
        [SerializeField] private GameEvent _lightAttackEvent;
        [SerializeField] private GameEvent _heavyAttackEvent;
        [SerializeField] private GameEvent _lightDashAttackEvent;

        [Space]
        [SerializeField] private GameEvent _anyHitEvent;
        [SerializeField] private GameEvent _lightHitEvent;
        [SerializeField] private GameEvent _heavyHitEvent;
        [SerializeField] private GameEvent _perfectHeavyHitEvent;
        [SerializeField] private GameEvent _lightDashHitEvent;

        private int _attackIndex = 0;
        private bool _heavyStarted;
        private bool _hasQueuedInput;
        private bool _enteredFromDash;

        public bool IsOnCooldown => CurrentWeapon.IsOnCooldown;
        public bool IsDuringAttack => CurrentWeapon.IsDuringAttack;
        public bool IsDuringRecovery => CurrentWeapon.IsDuringRecovery;
        public bool IsDuringLastAttack => _attackIndex >= CurrentWeapon.LastLightAttackIndex;
        
        private WeaponMelee CurrentWeapon => Parent.WeaponController.MeleeWeapon;

        public override void OnEnter() {
            ResetAttackVariables();
            _enteredFromDash = Controller.LastState == Controller.RollModule; 
        }

        public override void OnExit() {
            ResetAttackVariables();
            CurrentWeapon.OnChargeStop();
            CurrentWeapon.RecoveryTimer.Reset();
        }

        private void ResetAttackVariables() {
            _attackIndex = 0;
            _heavyStarted = false;
            _hasQueuedInput = false;
        }

        public void OnAttackEnd() {
            if (IsDuringLastAttack) 
                CurrentWeapon.StartCooldown();
            
            CurrentWeapon.RecoveryTimer.Start();
            CurrentWeapon.RecoveryTimer.OnEnd = OnRecoveryTimerEnd;
        }

        private void OnRecoveryTimerEnd() => Controller.TransitionToDefaultState();
        
        public override void SetInputs(CharacterInputs inputs) {
            if(IsDuringLastAttack)
                return;

            if (inputs.Melee.Held && !IsDuringAttack) {
                if (!_heavyStarted) {
                    _heavyStarted = true;
                    CurrentWeapon.OnChargeStart();
                }
                    
                CurrentWeapon.AttackHeld();
                Parent.OnHeavyAttackHeld();
            }

            if (inputs.Melee.Released) {
                if (!IsDuringAttack) {
                    if (!CurrentWeapon.MinInputReached) {
                        if (_enteredFromDash)
                            DashAttack();
                        else
                            LightAttack();
                    }
                    else 
                        HeavyAttack();
                }
                else
                    _hasQueuedInput = true;

                _heavyStarted = false;
                _enteredFromDash = false; // TODO fix to DuringRoll;
                CurrentWeapon.OnChargeStop();
            }

            // Handle Input Buffering
            if (_hasQueuedInput && !IsDuringAttack)
                LightAttack();
        }
        
        private void DashAttack() {
            ResetAttackVariables();
            _lightDashAttackEvent?.Raise(this);
            CurrentWeapon.DashAttack();
        }

        private void LightAttack() {
            _hasQueuedInput = false;
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
                case PlayerAttackType.DASH_ATTACK:
                    _lightDashHitEvent?.Raise(this);
                    break;
            }
            
            _anyHitEvent?.Raise(this);
        }

        public override bool CanEnterState() => !IsOnCooldown;
    }
}