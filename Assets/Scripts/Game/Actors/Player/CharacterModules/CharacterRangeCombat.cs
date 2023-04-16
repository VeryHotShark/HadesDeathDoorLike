using UnityEngine;

namespace VHS {
    public class CharacterRangeCombat : CharacterModule {
        [SerializeField] private GameEvent _hitEvent;
        private WeaponRange CurrentWeapon => Parent.WeaponController.WeaponRange;

        public bool HasAmmo => Parent.WeaponController.WeaponRange.HasAmmo;
        public bool IsOnCooldown => Parent.WeaponController.WeaponRange.IsOnCooldown;
        public int MaxAmmoCount => Parent.WeaponController.WeaponRange.MaxAmmoCount;

        public override void OnEnter() => CurrentWeapon.OnWeaponStart();
        public override void OnExit() => CurrentWeapon.OnWeaponStop();

        public override void SetInputs(CharacterInputs inputs) {
            if (inputs.Range.Held) {
                Parent.OnRangeAttackHeld();
                CurrentWeapon.OnRangeAttackHeld();
            }

            if (inputs.Range.Released) {
                CurrentWeapon.OnAttackReleased();   
                CurrentWeapon.StartCooldown();
                Controller.TransitionToDefaultState();
            }
        }

        public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
            currentRotation = Quaternion.LookRotation(Controller.LookInput);
            Controller.LastNonZeroMoveInput = Controller.LookInput;
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) => currentVelocity = Vector3.zero;

        public Projectile SpawnProjectile(Projectile prefab, Vector3? direction = null ) {
            Vector3 spawnPos = Motor.TransientPosition + Vector3.up;
            Quaternion spawnRot = Quaternion.LookRotation(direction ?? Controller.LookInput);
            Projectile  projectile = PoolManager.Spawn(prefab, spawnPos, spawnRot);
            projectile.Init(Parent);
            projectile.OnHit = OnProjectileHit;
            return projectile;
        }

        private void OnProjectileHit(Projectile projectile, HitData hitData) {
            Parent.OnRangeHit(hitData);
            _hitEvent?.Raise(projectile);
        }

        public override bool CanEnterState() => HasAmmo && !IsOnCooldown;
    }
}