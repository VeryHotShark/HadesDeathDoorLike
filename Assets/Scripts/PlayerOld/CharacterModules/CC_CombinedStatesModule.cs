using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

namespace VHS {
    public class CC_CombinedStatesModule : CharacterControllerModule {
        private List<CharacterControllerModule> _modules = new List<CharacterControllerModule>();

        public void AddModules(params CharacterControllerModule[] modules) {
            foreach (CharacterControllerModule module in modules) {
                if (!_modules.Contains(module))
                    _modules.Add(module);
            }
        }

        public override void SetInputs(CharacterInputs inputs) {
            foreach (CharacterControllerModule module in _modules)
                module.SetInputs(inputs);
        }

        public override void OnStateEnter() {
            foreach (CharacterControllerModule module in _modules)
                module.OnStateEnter();
        }

        public override void OnStateExit() {
            foreach (CharacterControllerModule module in _modules)
                module.OnStateExit();
        }

        public override void ResetValues() {
            foreach (CharacterControllerModule module in _modules)
                module.ResetValues();
        }

        public override void HandlePreCharacterUpdate(float deltaTime) {
            foreach (CharacterControllerModule module in _modules)
                module.HandlePreCharacterUpdate(deltaTime);
        }

        public override void HandlePostCharacterUpdate(float deltaTime) {
            foreach (CharacterControllerModule module in _modules)
                module.HandlePostCharacterUpdate(deltaTime);
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            foreach (CharacterControllerModule module in _modules)
                module.UpdateVelocity(ref currentVelocity, deltaTime);
        }

        public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
            foreach (CharacterControllerModule module in _modules)
                module.UpdateRotation(ref currentRotation, deltaTime);
        }

        public override void OnAnimatorMoveCallback(Animator animator) {
            foreach (CharacterControllerModule module in _modules)
                module.OnAnimatorMoveCallback(animator);
        }
        
        public override void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport) {
            foreach (CharacterControllerModule module in _modules)
                module.OnMovementHit(hitCollider, hitNormal, hitPoint, ref hitStabilityReport);
        }

        public override void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport) {
            foreach (CharacterControllerModule module in _modules)
                module.ProcessHitStabilityReport(hitCollider, hitNormal, hitPoint, atCharacterPosition,
                    atCharacterRotation, ref hitStabilityReport);
        }
    }
}