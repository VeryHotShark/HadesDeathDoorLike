using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class CharacterMovement : CharacterModule {
        [Header("Movement")] [SerializeField] private Vector3 _gravity = new(0.0f, -9.8f, 0.0f);
        [SerializeField] private float _airDrag = 0.1f;
        [SerializeField] private float _maxAirMoveSpeed = 10.0f;
        [SerializeField] private float _airAccelerationSpeed = 10.0f;

        [Space] [SerializeField] private float _maxStableMoveSpeed = 10.0f;
        [SerializeField] private float _stableMovementSharpness = 10.0f;
        [SerializeField] private float _orientationSharpness = 10.0f;

        public override void SetInputs(CharacterInputs inputs) {
            Vector3 rawInput = new Vector3(inputs.MoveAxisRight, 0.0f, inputs.MoveAxisForward);
            Vector3 clampedMoveInput = Vector3.ClampMagnitude(rawInput, 1.0f);

            Vector3 cameraPlanarDirection =
                Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, Motor.CharacterUp).normalized;

            Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Motor.CharacterUp);
            Controller.MoveInput = cameraPlanarRotation * clampedMoveInput;

            if (clampedMoveInput != Vector3.zero)
                Controller.LastNonZeroMoveInput = Controller.MoveInput;
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            if (Motor.GroundingStatus.IsStableOnGround) {
                currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity,
                    Motor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;

                Vector3 crossInput = Vector3.Cross(Controller.MoveInput, Motor.CharacterUp);
                Vector3 reorientedInput = Vector3.Cross(Motor.GroundingStatus.GroundNormal, crossInput).normalized *
                                          Controller.MoveInput.magnitude;

                Vector3 targetMovementVelocity = reorientedInput * _maxStableMoveSpeed;

                float t = 1 - Mathf.Exp(-_stableMovementSharpness * deltaTime);
                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, t);
            }
            else {
                if (Controller.MoveInput.sqrMagnitude > 0.0f) {
                    Vector3 targetMovementVelocity = Controller.MoveInput * _maxAirMoveSpeed;
                    Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, _gravity);
                    currentVelocity += velocityDiff * (_airAccelerationSpeed * deltaTime);
                }

                currentVelocity += _gravity * deltaTime;
                currentVelocity *= 1f / 1f + _airDrag * deltaTime;
            }
        }

        public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
            if (_orientationSharpness > 0f) {
                float t = 1 - Mathf.Exp(-_orientationSharpness * deltaTime);
                Vector3 smoothLookInputDirection =
                    Vector3.Slerp(Motor.CharacterForward, Controller.LastNonZeroMoveInput, t).normalized;
                currentRotation = Quaternion.LookRotation(smoothLookInputDirection, Motor.CharacterUp);
            }
        }
    }
}