using System;
using System.Collections;
using System.Collections.Generic;
using Animancer.Examples.Basics;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Pool;

namespace VHS {
    public class CharacterRangeCombat : CharacterModule {
        [SerializeField] private GameEvent _hitEvent;
        
        private WeaponRange CurrentWeapon => Parent.WeaponController.WeaponRange;

        public bool HasAmmo => Parent.WeaponController.WeaponRange.HasAmmo;
        public bool IsOnCooldown => Parent.WeaponController.WeaponRange.IsOnCooldown;
        public int MaxAmmoCount => Parent.WeaponController.WeaponRange.MaxAmmoCount;

        public override void OnExit() => Parent.AnimationController.UnpauseGraph();

        public override void SetInputs(CharacterInputs inputs) {
            if (inputs.Secondary.Performed)
                OnRangeAttackReached();
            
            if (inputs.Secondary.Held)
                OnRangeAttackHeld();
            
            if(inputs.Secondary.Released)
                OnRangeAttackReleased();
        }

        public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
            currentRotation = Quaternion.LookRotation(Controller.LookInput);
            Controller.LastNonZeroMoveInput = Controller.LookInput;
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) => currentVelocity = Vector3.zero;

        private void OnRangeAttackReached() {
            CurrentWeapon.OnRangeAttackReached();
        }

        private void OnRangeAttackHeld() {
            CurrentWeapon.OnRangeAttackHeld();
        }

        private void OnRangeAttackReleased() {
            Parent.OnRangeAttack();
            CurrentWeapon.OnRangeAttack();
            CurrentWeapon.StartCooldown();
            Controller.TransitionToDefaultState();
        }

        public Projectile SpawnProjectile(Projectile prefab) {
            Vector3 spawnPos = Motor.TransientPosition + Vector3.up;
            Quaternion spawnRot = Quaternion.LookRotation(Controller.LookInput);
            Projectile  projectile = PoolManager.Spawn(prefab, spawnPos, spawnRot);
            projectile.Init(Parent);
            projectile.OnHit = OnProjectileHit;
            return projectile;
        }

        private void OnProjectileHit(Projectile projectile, HitData hitData) => _hitEvent?.Raise(projectile);

        public override bool CanEnterState() => HasAmmo && !IsOnCooldown;
    }
}