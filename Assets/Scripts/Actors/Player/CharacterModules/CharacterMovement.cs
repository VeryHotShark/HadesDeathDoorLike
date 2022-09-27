using UnityEngine;

namespace VHS {
    public class CharacterMovement : CharacterModule {
        [SerializeField] private float _maxStableMoveSpeed = 10.0f;
        [SerializeField] private float _stableMovementSharpness = 10.0f;
        [SerializeField] private float _orientationSharpness = 10.0f;
        
        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity,
                Motor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;

            // handle slope compensation
            Vector3 crossInput = Vector3.Cross(Controller.MoveInput, Motor.CharacterUp);
            Vector3 reorientedInput = Vector3.Cross(Motor.GroundingStatus.GroundNormal, crossInput).normalized *
                                      Controller.MoveInput.magnitude;

            Vector3 targetMovementVelocity = reorientedInput * _maxStableMoveSpeed;

            float t = 1 - Mathf.Exp(-_stableMovementSharpness * deltaTime);
            currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, t);
        }

        public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
            if (_orientationSharpness > 0f) {
                float t = 1 - Mathf.Exp(-_orientationSharpness * deltaTime);
                
                /*
                Vector3 desiredRotation = Controller.LockTarget != null ? 
                    Motor.TransientPosition.DirectionTo(Controller.LockTarget.GetTargetPosition()).Flatten() 
                    : Controller.LastNonZeroMoveInput;
                */

                Vector3 desiredRotation = Controller.LastNonZeroMoveInput;
                Vector3 smoothDesiredRotation = Vector3.Slerp(Motor.CharacterForward, desiredRotation, t).normalized;
                currentRotation = Quaternion.LookRotation(smoothDesiredRotation, Motor.CharacterUp);
            }
        }
    }
}