using System.Collections;
using System.Collections.Generic;
using MoreMountains.FeedbacksForThirdParty;
using UnityEditor.MPE;
using UnityEngine;

namespace VHS {
    public class WeaponKunai : WeaponRange {
        [SerializeField] private float _speed = 35.0f;

        protected override void OnProjectileShot(Projectile projectile) => (projectile as ProjectileBullet).SetSpeed(_speed);
    }
}
