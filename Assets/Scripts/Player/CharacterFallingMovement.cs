using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class CharacterFallingMovement : CharacterModule {
        [SerializeField] private Vector3 _gravity = new(0.0f, -9.8f, 0.0f);
        [SerializeField] private float _airDrag = 0.1f;
        [SerializeField] private float _maxAirMoveSpeed = 10.0f;
        [SerializeField] private float _airAccelerationSpeed = 10.0f;
        [SerializeField] private float _orientationSharpness = 10.0f;

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            if (Controller.MoveInput.sqrMagnitude > 0.0f) {
                Vector3 targetMovementVelocity = Controller.MoveInput * _maxAirMoveSpeed;
                Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, _gravity);
                currentVelocity += velocityDiff * (_airAccelerationSpeed * deltaTime);
            }

            currentVelocity += _gravity * deltaTime;
            currentVelocity *= 1f / 1f + _airDrag * deltaTime;
        }
        
        public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
            if (_orientationSharpness > 0f) {
                float t = 1 - Mathf.Exp(-_orientationSharpness * deltaTime);
                Vector3 desiredRotation = Controller.LastNonZeroMoveInput;
                Vector3 smoothDesiredRotation = Vector3.Slerp(Motor.CharacterForward, desiredRotation, t).normalized;
                currentRotation = Quaternion.LookRotation(smoothDesiredRotation, Motor.CharacterUp);
            }
        }
    }
}