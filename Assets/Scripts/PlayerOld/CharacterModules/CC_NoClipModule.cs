using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VHS {
    public class CC_NoClipModule : OldCharacterControllerModule {
        [SerializeField] private float _noClipMoveSpeed = 10f;
        [SerializeField] private float _noClipSharpness = 15f;
        [SerializeField] private float _orientationSharpness = 15f;
        
        public override void SetInputs(OldCharacterInputs inputs) {
            Vector3 moveInput = new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward);
            Vector3 clampedMoveInput = Vector3.ClampMagnitude(moveInput, 1f);
            Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, Motor.CharacterUp).normalized;
            Controller.MoveInput = inputs.CameraRotation * clampedMoveInput;
            Controller.LookInput = cameraPlanarDirection;
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            float verticalInput = (Keyboard.current.spaceKey.isPressed ? 1f : 0f) + (Keyboard.current.leftCtrlKey.isPressed ? -1f : 0f);
            Vector3 targetMovementVelocity = (Controller.MoveInput + Motor.CharacterUp * verticalInput).normalized * _noClipMoveSpeed;
            currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1 - Mathf.Exp(-_noClipSharpness * deltaTime));
        }

        public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
            if (Controller.LookInput != Vector3.zero && _orientationSharpness > 0f) {
                Vector3 smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, Controller.LookInput, 1 - Mathf.Exp(-_orientationSharpness * deltaTime)).normalized;
                currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);
            }
        }

        public override void OnStateEnter() {
            Motor.SetCapsuleCollisionsActivation(false);
            Motor.SetMovementCollisionsSolvingActivation(false);
            Motor.SetGroundSolvingActivation(false);    
        }

        public override void OnStateExit() {
            Motor.SetCapsuleCollisionsActivation(true);
            Motor.SetMovementCollisionsSolvingActivation(true);
            Motor.SetGroundSolvingActivation(true);
        }
    }
}
