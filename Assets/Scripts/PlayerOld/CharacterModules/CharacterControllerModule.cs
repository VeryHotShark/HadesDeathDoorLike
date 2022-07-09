using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

namespace VHS {
    public abstract class CharacterControllerModule : MonoBehaviour {
        private CharacterController _controller;
        protected KinematicCharacterMotor Motor => Controller.Motor;
        protected CC_StateMachine StateMachine => Controller.StateMachine;
        protected CharacterController Controller => _controller ??= GetComponent<CharacterController>();

        public virtual void OnStateEnter() { }
        public virtual void OnStateExit() { }
        public virtual void ResetValues() { }
        
        public abstract void SetInputs(CharacterInputs inputs);
        public virtual void HandlePreCharacterUpdate(float deltaTime) { }
        public virtual void HandlePostCharacterUpdate(float deltaTime) { }
        public virtual void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) { }
        public virtual void UpdateRotation(ref Quaternion currentRotation, float deltaTime) { }
        
        public virtual void OnAnimatorMoveCallback(Animator animator) { }
        public virtual void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) { }
        public virtual void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)  { }
    }
}