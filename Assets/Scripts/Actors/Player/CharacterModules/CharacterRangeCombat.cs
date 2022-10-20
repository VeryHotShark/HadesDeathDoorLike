using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Pool;

namespace VHS {
    public class CharacterRangeCombat : CharacterModule {
        [SerializeField] private Projectile _projectile;

        public override void SetInputs(CharacterInputs inputs) {
            if(inputs.SecondaryAttackReleased)
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
            Vector3 spawnPos = Motor.TransientPosition + Vector3.up;
            Quaternion spawnRot = Quaternion.LookRotation(Controller.LookInput);
            PoolManager.Spawn(_projectile, spawnPos, spawnRot).Init(Parent);
            Controller.TransitionToDefaultState();
        }
    }
}