using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace VHS {
    public class CC_SwimModule : CharacterControllerModule {
        [SerializeField] private Transform _swimReferencePoint;
        [SerializeField] private LayerMask _waterLayer;
        [SerializeField] private float _jumpOutSpeed = 10f;
        [SerializeField] private float _swimSpeed = 4f;
        [SerializeField] private float _swimMovementSharpness = 3f;
        [SerializeField] private float _swimOrientationSharpness = 2f;
        [SerializeField, Range(0f,1f)] private float _gravityMultiplier = 0.5f;
        
        private bool _jumpRequested;
        private Collider _waterZone;
        private Collider[] _probedColliders = new Collider[8];
        
        public override void SetInputs(CharacterInputs inputs) {
            Vector3 moveInput = new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward);
            Vector3 clampedMoveInput = Vector3.ClampMagnitude(moveInput, 1f);

            Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, Motor.CharacterUp).normalized;

            if (cameraPlanarDirection.sqrMagnitude == 0f) {
                Vector3 cameraUp = inputs.CameraRotation * Vector3.up;
                bool lookingUp = Vector3.Dot(Vector3.up, inputs.CameraRotation * Vector3.forward) > 0f;
                cameraPlanarDirection = Vector3.ProjectOnPlane(lookingUp ? -cameraUp : cameraUp, Motor.CharacterUp).normalized;
            }

            Controller.MoveInput = inputs.CameraRotation * clampedMoveInput;
            Controller.LookInput = cameraPlanarDirection;
            
            _jumpRequested = Keyboard.current.spaceKey.isPressed;
        }

        public override void OnStateEnter() => Motor.SetGroundSolvingActivation(false);
        public override void OnStateExit() => Motor.SetGroundSolvingActivation(true);

        public override void HandlePreCharacterUpdate(float deltaTime) {
            int hitCount = Motor.CharacterOverlap(Motor.TransientPosition, Motor.TransientRotation, _probedColliders, _waterLayer,
                QueryTriggerInteraction.Collide);
            
            if (hitCount > 0) {
                Collider col = _probedColliders[0];
                if (col != null) {
                    Vector3 closestPoint = Physics.ClosestPoint(_swimReferencePoint.position, col, col.transform.position, col.transform.rotation);

                    if (closestPoint == _swimReferencePoint.position) {
                        StateMachine.SetState(this);
                        _waterZone = _probedColliders[0];
                    } else
                        StateMachine.SetState(Controller.DefaultMovementModule);  
                }
            }
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            float verticalInput = (Keyboard.current.spaceKey.isPressed ? 1f : 0f) + (Keyboard.current.leftCtrlKey.isPressed ? -1f : 0f);
            Vector3 targetMovementVelocity = (Controller.MoveInput + Motor.CharacterUp * verticalInput).normalized * _swimSpeed;
            Vector3 smoothVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-_swimMovementSharpness * deltaTime));

            Vector3 resultingSwimRefPos = Motor.TransientPosition + smoothVelocity * deltaTime +
                                          (_swimReferencePoint.position - Motor.TransientPosition);
            Vector3 closesPointWaterSurface = Physics.ClosestPoint(resultingSwimRefPos, _waterZone,
                                            _waterZone.transform.position, _waterZone.transform.rotation);

            if (closesPointWaterSurface != resultingSwimRefPos) {
                Vector3 waterSurfaceNormal = (resultingSwimRefPos - closesPointWaterSurface).normalized;
                smoothVelocity = Vector3.ProjectOnPlane(smoothVelocity, waterSurfaceNormal);

                if (_jumpRequested)
                    smoothVelocity += Motor.CharacterUp * _jumpOutSpeed -
                                      Vector3.Project(currentVelocity, Motor.CharacterUp);
            }

            if(verticalInput == 0f)
                smoothVelocity += Controller.Gravity * _gravityMultiplier * deltaTime;
            
            currentVelocity = smoothVelocity;
        }

        public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
            if (Controller.LookInput != Vector3.zero && _swimOrientationSharpness > 0f) {
                Vector3 smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, Controller.LookInput, 1 - Mathf.Exp(-_swimOrientationSharpness * deltaTime)).normalized;
                currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);
            }
        }
    }
}
