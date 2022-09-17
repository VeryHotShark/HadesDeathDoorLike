using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace VHS {
    public class CharacterRangeCombat : CharacterModule {
        [SerializeField] private Projectile _projectile;

        public override void OnEnter() {
            Shoot();
        }

        public void Shoot() {
            Vector3 spawnPos = Motor.TransientPosition + Vector3.up + Motor.CharacterForward;
            Quaternion spawnRot = Quaternion.LookRotation(Motor.CharacterForward);
            Projectile spawnedProjectile = Instantiate(_projectile, spawnPos, spawnRot);
            spawnedProjectile.Init(Controller);
            Controller.TransitionToDefaultState();
        }
    }
}