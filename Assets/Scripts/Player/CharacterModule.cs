using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

namespace VHS {
    public abstract class CharacterModule : MonoBehaviour, IState {
        private CharacterController _controller;
        protected KinematicCharacterMotor Motor => Controller.Motor;
        protected CharacterController Controller => _controller ??= GetComponent<CharacterController>();

        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void OnReset() { }
        public virtual void OnTick(float deltaTime) { }

        public virtual void SetInputs(CharacterInputs inputs) { }
        public virtual void HandlePreCharacterUpdate(float deltaTime) { }
        public virtual void HandlePostCharacterUpdate(float deltaTime) { }
        public virtual void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) { }
        public virtual void UpdateRotation(ref Quaternion currentRotation, float deltaTime) { }

        public virtual void OnAnimatorMoveCallback(Animator animator) { }

        public virtual void OnStableGroundLost() { }
        public virtual void OnStableGroundRegained() { }

        public virtual void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport) { }

        public virtual void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport) { }
    }
}