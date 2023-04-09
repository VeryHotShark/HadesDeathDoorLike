using UnityEngine;

namespace VHS {
    public class WeaponKunai : WeaponRange {
        [SerializeField] private float _speed = 35.0f;

        protected override void OnProjectileShot(Projectile projectile) => (projectile as ProjectileBullet).SetSpeed(_speed);
    }
}
