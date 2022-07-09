using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using System;
using UnityEngine.Serialization;

namespace KinematicCharacterController.Walkthrough.RootMotionExample
{
    public struct PlayerCharacterInputs
    {
        public float MoveAxisForward;
        public float MoveAxisRight;
    }

    public class MyCharacterController : MonoBehaviour, ICharacterController
    {
        [FormerlySerializedAs("Motor")] public KinematicCharacterMotor _motor;

        [FormerlySerializedAs("MaxStableMoveSpeed")] [Header("Stable Movement")]
        public float _maxStableMoveSpeed = 10f;
        [FormerlySerializedAs("StableMovementSharpness")] public float _stableMovementSharpness = 15;
        [FormerlySerializedAs("OrientationSharpness")] public float _orientationSharpness = 10;

        [FormerlySerializedAs("MaxAirMoveSpeed")] [Header("Air Movement")]
        public float _maxAirMoveSpeed = 10f;
        [FormerlySerializedAs("AirAccelerationSpeed")] public float _airAccelerationSpeed = 5f;
        [FormerlySerializedAs("Drag")] public float _drag = 0.1f;

        [FormerlySerializedAs("CharacterAnimator")] [Header("Animation Parameters")]
        public Animator _characterAnimator;
        [FormerlySerializedAs("ForwardAxisSharpness")] public float _forwardAxisSharpness = 10;
        [FormerlySerializedAs("TurnAxisSharpness")] public float _turnAxisSharpness = 5;

        [FormerlySerializedAs("Gravity")] [Header("Misc")]
        public Vector3 _gravity = new Vector3(0, -30f, 0);
        [FormerlySerializedAs("MeshRoot")] public Transform _meshRoot;

        private Vector3 _moveInputVector;
        private Vector3 _lookInputVector;
        private float _forwardAxis;
        private float _rightAxis;
        private float _targetForwardAxis;
        private float _targetRightAxis;
        private Vector3 _rootMotionPositionDelta;
        private Quaternion _rootMotionRotationDelta;

        /// <summary>
        /// This is called every frame by MyPlayer in order to tell the character what its inputs are
        /// </summary>
        public void SetInputs(ref PlayerCharacterInputs inputs)
        {
            // Axis inputs
            _targetForwardAxis = inputs.MoveAxisForward;
            _targetRightAxis = inputs.MoveAxisRight;
        }

        private void Start()
        {
            _rootMotionPositionDelta = Vector3.zero;
            _rootMotionRotationDelta = Quaternion.identity;

            // Assign to motor
            _motor.CharacterController = this;
        }

        private void Update()
        {
            // Handle animation
            _forwardAxis = Mathf.Lerp(_forwardAxis, _targetForwardAxis, 1f - Mathf.Exp(-_forwardAxisSharpness * Time.deltaTime));
            _rightAxis = Mathf.Lerp(_rightAxis, _targetRightAxis, 1f - Mathf.Exp(-_turnAxisSharpness * Time.deltaTime));
            _characterAnimator.SetFloat("Forward", _forwardAxis);
            _characterAnimator.SetFloat("Turn", _rightAxis);
            _characterAnimator.SetBool("OnGround", _motor.GroundingStatus.IsStableOnGround);
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is called before the character begins its movement update
        /// </summary>
        public void BeforeCharacterUpdate(float deltaTime)
        {
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its rotation should be right now. 
        /// This is the ONLY place where you should set the character's rotation
        /// </summary>
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            currentRotation = _rootMotionRotationDelta * currentRotation;
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its velocity should be right now. 
        /// This is the ONLY place where you can set the character's velocity
        /// </summary>
        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (_motor.GroundingStatus.IsStableOnGround)
            {
                if (deltaTime > 0)
                {
                    // The final velocity is the velocity from root motion reoriented on the ground plane
                    currentVelocity = _rootMotionPositionDelta / deltaTime;
                    currentVelocity = _motor.GetDirectionTangentToSurface(currentVelocity, _motor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;
                }
                else
                {
                    // Prevent division by zero
                    currentVelocity = Vector3.zero;
                }
            }
            else
            {
                if (_forwardAxis > 0f)
                {
                    // If we want to move, add an acceleration to the velocity
                    Vector3 targetMovementVelocity = _motor.CharacterForward * _forwardAxis * _maxAirMoveSpeed;
                    Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, _gravity);
                    currentVelocity += velocityDiff * _airAccelerationSpeed * deltaTime;
                }

                // Gravity
                currentVelocity += _gravity * deltaTime;

                // Drag
                currentVelocity *= (1f / (1f + (_drag * deltaTime)));
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is called after the character has finished its movement update
        /// </summary>
        public void AfterCharacterUpdate(float deltaTime)
        {
            // Reset root motion deltas
            _rootMotionPositionDelta = Vector3.zero;
            _rootMotionRotationDelta = Quaternion.identity;
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void PostGroundingUpdate(float deltaTime)
        {
        }

        public void AddVelocity(Vector3 velocity)
        {
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }

        private void OnAnimatorMove()
        {
            // Accumulate rootMotion deltas between character updates 
            _rootMotionPositionDelta += _characterAnimator.deltaPosition;
            _rootMotionRotationDelta = _characterAnimator.deltaRotation * _rootMotionRotationDelta;
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
        }
    }
}