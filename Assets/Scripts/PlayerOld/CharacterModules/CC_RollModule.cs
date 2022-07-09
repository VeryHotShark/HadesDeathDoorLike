using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class CC_RollModule : CharacterControllerModule {
        [SerializeField] private float _rollDuration = 1.5f;
            
        [Header("Root Motion")]
        [SerializeField] private bool _useRootMotion;
        [SerializeField] private Vector3 _rootAxisToUse = Vector3.one;

        private bool _stopRoll;
        private bool _rollRequested;
        private bool _rollConsumed;
        private float _timeSinceRollStart;
        
        private Vector3 _rootMotionPositionDelta;

        public override void SetInputs(CharacterInputs inputs) {
            _rollRequested = true;
        }

        public override void OnStateEnter() {
            _timeSinceRollStart = 0f;
            Motor.SetCapsuleDimensions(0.5f, 1,0.5f);
            Controller.OnRollStart();
        }

        public override void OnStateExit() {
            _stopRoll = false;
            _rollConsumed = false;
            _rollRequested = false;
            Motor.SetCapsuleDimensions(0.5f, 2f, 1f);
            Controller.OnRollEnd();
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            if (_stopRoll) {
                currentVelocity = Vector3.zero;
                return;
            }
            
            if (deltaTime > Mathf.Epsilon) {
                currentVelocity = _rootMotionPositionDelta / deltaTime;
                currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, 
                                      Motor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;
            }
            else
                currentVelocity = Vector3.zero;

            if (!Motor.GroundingStatus.IsStableOnGround)
                currentVelocity += Controller.Gravity * 5f * deltaTime;
        }

        public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
            if (_rollRequested && !_rollConsumed && Motor.GroundingStatus.FoundAnyGround)
                currentRotation = Quaternion.LookRotation(Controller.MoveInput != Vector3.zero ? Controller.MoveInput : Motor.CharacterForward);            
        }
        
        public override void OnAnimatorMoveCallback(Animator animator) {
            if (_useRootMotion) {
                Vector3 localDeltaPos = Motor.transform.InverseTransformVector(animator.deltaPosition);
                Vector3 clampedDeltaPos = Vector3.Scale(localDeltaPos, _rootAxisToUse);
                _rootMotionPositionDelta += Motor.transform.TransformVector(clampedDeltaPos);
            }
        }

        public override void HandlePreCharacterUpdate(float deltaTime) {
            _timeSinceRollStart += deltaTime;
            float normalizedRollTime = Mathf.Clamp01(_timeSinceRollStart / _rollDuration);
            Controller.OnRollUpdate(normalizedRollTime);
        }

        public override void HandlePostCharacterUpdate(float deltaTime) {
            if (!_stopRoll && _timeSinceRollStart > _rollDuration) {
                _stopRoll = true;
                StateMachine.SetState(Controller.DefaultMovementModule);
            }
                
            _rootMotionPositionDelta = Vector3.zero;
            _rollRequested = false;
        }
        
        
    }
}
