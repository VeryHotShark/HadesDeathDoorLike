using System;
using System.Collections;
using System.Collections.Generic;
using Animancer.Examples.Basics;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Pool;

namespace VHS {
    public class CharacterRangeCombat : CharacterModule {
        [SerializeField] private int _maxAmmoCount = 4;
        [SerializeField] private float _speed = 35.0f;
        [SerializeField] private Projectile _projectile;

        private int _currentAmmo;

        public bool HasAmmo => _currentAmmo > 0;
        public int MaxAmmoCount => _maxAmmoCount;
        
        private void Awake() => _currentAmmo = _maxAmmoCount;

        private void OnEnable() => Parent.OnMeleeHit += OnMeleeHit;
        private void OnDisable() => Parent.OnMeleeHit -= OnMeleeHit;

        private void OnMeleeHit(HitData hitData) => ModifyCurrentAmmo(1);

        public override void SetInputs(CharacterInputs inputs) {
            if(inputs.Secondary.Released)
                Shoot();
        }

        public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
            currentRotation = Quaternion.LookRotation(Controller.LookInput);
            Controller.LastNonZeroMoveInput = Controller.LookInput;
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            currentVelocity = Vector3.zero;
        }

        public void Shoot() {
            ModifyCurrentAmmo(-1);
            Vector3 spawnPos = Motor.TransientPosition + Vector3.up;
            Quaternion spawnRot = Quaternion.LookRotation(Controller.LookInput);
            PoolManager.Spawn(_projectile, spawnPos, spawnRot).Init(Parent);
            (_projectile as ProjectileBullet).SetSpeed(_speed); // TODO zmienić
            Controller.TransitionToDefaultState();
        }

        private void ModifyCurrentAmmo(int amount) {
            _currentAmmo = Mathf.Clamp(_currentAmmo + amount, 0, _maxAmmoCount);
            Parent.OnCurrentAmmoChanged(_currentAmmo);
        }

    }
}