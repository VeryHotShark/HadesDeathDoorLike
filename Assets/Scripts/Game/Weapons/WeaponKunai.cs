using Sirenix.OdinInspector;
using UnityEngine;

namespace VHS {
    public class WeaponKunai : WeaponRange {
        [SerializeField] private float _speed = 35.0f;
        
        [TitleGroup("Perfect Attack")]
        [SerializeField] private int _kunaiCount = 3;
        [SerializeField] private float _angle = 20.0f;

        protected override void OnProjectileShot(Projectile projectile) => (projectile as ProjectileBullet).SetSpeed(_speed);

        protected override void OnPerfectRangeAttack() {
            float startAngle = -(_kunaiCount / 2.0f) * _angle;
            Vector3 direction =  Character.LookInput;
            
            for (int i = 0; i < _kunaiCount; i++) {
                Vector3 rotatedDirection = Quaternion.Euler(0.0f, startAngle, 0.0f) * direction;
                Projectile projectile = Character.RangeCombat.SpawnProjectile(_projectile, rotatedDirection);
                OnProjectileShot(projectile);
                startAngle += _angle;
            }            
        }
    }
}
