using System.Collections;
using System.Collections.Generic;
using Animancer;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VHS {
    public class WeaponRange : Weapon {
        [TitleGroup("Common")]
        [SerializeField] private int _ammoCost = 1;
        [SerializeField] private int _maxAmmoCount = 4;
        [SerializeField] protected Projectile _projectile;

        [TitleGroup("Animations")]
        [SerializeField] private ClipTransition _shootClip;
        [SerializeField] private ClipTransition _shootWindupClip;

        private int _currentAmmo;

        public bool HasAmmo => _currentAmmo > 0;
        public int MaxAmmoCount => _maxAmmoCount;

        public override void OnWeaponStop() {
            base.OnWeaponStop();
            UnpauseGraph();
        }

        public override void Init(Player player) {
            base.Init(player);
            _currentAmmo = _maxAmmoCount;
        }

        protected override void Enable() {
            base.Enable();
            _shootWindupClip.Events.OnEnd += PauseGraph;
        }

        protected override void Disable() {
            base.Disable();
            _shootWindupClip.Events.OnEnd -= PauseGraph;
        }

        private void PauseGraph() => AnimationController.PauseGraph();
        private void UnpauseGraph() => AnimationController.UnpauseGraph();

        protected override void OnPerfectStart() {
            base.OnPerfectStart();
            _player.OnPerfectRangeAttackStart();
        }

        protected override void OnPerfectEnd() {
            base.OnPerfectEnd();
            _player.OnPerfectRangeAttackEnd();
        }
        
        protected override void OnSuccessfulAttackReleased() {
            Animancer.Play(_shootClip);
            ModifyCurrentAmmo(-_ammoCost);
            base.OnSuccessfulAttackReleased();
        }

        protected override void OnPerfectHoldAttack() => _player.OnPerfectRangeAttack();

        protected override void OnRegularHoldAttack() {
            Projectile projectile = Character.RangeCombat.SpawnProjectile(_projectile);
            OnProjectileShot(projectile);
            _player.OnRangeAttack();
        }

        protected override void OnAttackHeld() => Animancer.Play(_shootWindupClip);

        protected virtual void OnProjectileShot(Projectile projectile) { }
        
        public void ModifyCurrentAmmo(int amount) {
            _currentAmmo = Mathf.Clamp(_currentAmmo + amount, 0, _maxAmmoCount);
            _player.OnCurrentAmmoChanged(_currentAmmo);
        }
    }
}
