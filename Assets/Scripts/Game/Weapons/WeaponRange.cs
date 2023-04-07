using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

namespace VHS {
    public class WeaponRange : Weapon {
        [SerializeField] private int _ammoCost = 1;
        [SerializeField] private int _maxAmmoCount = 4;
        [SerializeField] private Projectile _projectile;
        
        [SerializeField] private ClipTransition _shootClip;
        [SerializeField] private ClipTransition _shootWindupClip;
        
        private int _currentAmmo;

        public bool HasAmmo => _currentAmmo > 0;
        public int MaxAmmoCount => _maxAmmoCount;

        public override void Init(Player player) {
            base.Init(player);
            _currentAmmo = _maxAmmoCount;
        }

        protected override void Enable() => _shootWindupClip.Events.OnEnd += PauseGraph; 
        protected override void Disable() => _shootWindupClip.Events.OnEnd -= PauseGraph;

        private void PauseGraph() => AnimationController.PauseGraph();
        
        public virtual void OnRangeAttack() {
            ModifyCurrentAmmo(-_ammoCost);
            Projectile projectile = Character.RangeCombat.SpawnProjectile(_projectile);
            OnProjectileShot(projectile);
            
            AnimationController.UnpauseGraph();
            Animancer.Play(_shootClip);
        }

        public virtual void OnRangeAttackHeld() => Animancer.Play(_shootWindupClip);

        public virtual void OnRangeAttackReached() { }
        
        public virtual void OnPerfectRangeAttack() { }

        protected virtual void OnProjectileShot(Projectile projectile) { }
        
        public void ModifyCurrentAmmo(int amount) {
            _currentAmmo = Mathf.Clamp(_currentAmmo + amount, 0, _maxAmmoCount);
            _player.OnCurrentAmmoChanged(_currentAmmo);
        }
    }
}
