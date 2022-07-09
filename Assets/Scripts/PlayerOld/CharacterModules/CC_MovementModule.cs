using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class CC_MovementModule : CharacterControllerModule {
        [Header("Stable Movement")]
        [SerializeField] private float _maxStableMoveSpeed = 10f;
        [SerializeField] private float _stableMovementSharpness = 15;
        [SerializeField] private float _orientationSharpness = 10;

        [Header("Air Movement")] 
        [SerializeField] private float _maxAirMoveSpeed = 10f;
        [SerializeField] private float _airAccelerationSpeed = 5f;
        [SerializeField] private float _drag;

        [Header("Root Motion")]
        [SerializeField] private bool _useRootMotion;
        [SerializeField] private Vector3 _rootAxisToUse = Vector3.one;
        [SerializeField, Range(0f,1f)] private float _faceMoveInputThreshold = 0.9f;
        
        private Vector3 _rootMotionPositionDelta;

        public override void SetInputs(CharacterInputs inputs) {
            Vector3 moveInput = new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward);
            Vector3 clampedMoveInput = Vector3.ClampMagnitude(moveInput, 1f);

            Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, Motor.CharacterUp).normalized;

            if (cameraPlanarDirection.sqrMagnitude == 0f) {
                Vector3 cameraUp = inputs.CameraRotation * Vector3.up;
                bool lookingUp = Vector3.Dot(Vector3.up, inputs.CameraRotation * Vector3.forward) > 0f;
                cameraPlanarDirection = Vector3.ProjectOnPlane(lookingUp ? -cameraUp : cameraUp, Motor.CharacterUp).normalized;
            }

            Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Motor.CharacterUp);

            Controller.MoveInput = cameraPlanarRotation * clampedMoveInput;

            if (clampedMoveInput != Vector3.zero)
                Controller.LastNonZeroMoveInput = Controller.MoveInput;
            
            Controller.LookInput = cameraPlanarDirection;    
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            Vector3 targetMovementVelocity;

            if (Motor.GroundingStatus.IsStableOnGround) {
                if (_useRootMotion) {
                    bool facingMoveInput = Vector3.Dot(Motor.CharacterForward, Controller.MoveInput) > _faceMoveInputThreshold;
                    
                    if (deltaTime > Mathf.Epsilon && facingMoveInput) {
                        currentVelocity = _rootMotionPositionDelta / deltaTime;
                        currentVelocity =
                            Motor.GetDirectionTangentToSurface(currentVelocity, Motor.GroundingStatus.GroundNormal) *
                            currentVelocity.magnitude;
                    }
                    else
                        currentVelocity = Vector3.zero;
                }
                else {
                    currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, Motor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;

                    Vector3 inputRight = Vector3.Cross(Controller.MoveInput, Motor.CharacterUp);
                    Vector3 reorientedInput = Vector3.Cross(Motor.GroundingStatus.GroundNormal, inputRight).normalized *
                                              Controller.MoveInput.magnitude;
                    targetMovementVelocity = reorientedInput * _maxStableMoveSpeed;

                    float t = 1 - Mathf.Exp(-_stableMovementSharpness * deltaTime);
                    currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, t);
                }
            }
            else {
                float moveInput = _useRootMotion 
                    ? Controller.AC.GetMoveParam 
                    : Controller.MoveInput.sqrMagnitude;
                
                if (moveInput > 0f) {
                    Vector3 moveDir = _useRootMotion
                        ? Motor.CharacterForward * Controller.AC.GetMoveParam
                        : Controller.MoveInput;
                    
                    targetMovementVelocity = moveDir * _maxAirMoveSpeed;

                    if (Motor.GroundingStatus.FoundAnyGround) {
                        Vector3 right = Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal);
                        Vector3 perpendicularObstructionNormal = Vector3.Cross(right, Motor.CharacterUp).normalized;
                        targetMovementVelocity = Vector3.ProjectOnPlane(targetMovementVelocity, perpendicularObstructionNormal);
                    }

                    Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, Controller.Gravity);
                    currentVelocity += velocityDiff * _airAccelerationSpeed * deltaTime;
                }

                currentVelocity += Controller.Gravity * deltaTime;
                currentVelocity *= 1f / 1f + _drag * deltaTime;
            }
        }
        
        public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
            if (_orientationSharpness > 0f) {
                float t = 1 - Mathf.Exp(-_orientationSharpness * deltaTime);
                Vector3 smoothLookInputDirection = Vector3.Slerp(Motor.CharacterForward, Controller.LastNonZeroMoveInput, t).normalized;
                currentRotation = Quaternion.LookRotation(smoothLookInputDirection, Motor.CharacterUp);
            }
        }
        
        public override void OnAnimatorMoveCallback(Animator animator) {
            if (_useRootMotion) {
                Vector3 localDeltaPos = Motor.transform.InverseTransformVector(animator.deltaPosition);
                Vector3 clampedDeltaPos = Vector3.Scale(localDeltaPos, _rootAxisToUse);
                _rootMotionPositionDelta += Motor.transform.TransformVector(clampedDeltaPos);
            }
        }

        public override void HandlePostCharacterUpdate(float deltaTime) => _rootMotionPositionDelta = Vector3.zero;
    }
}
