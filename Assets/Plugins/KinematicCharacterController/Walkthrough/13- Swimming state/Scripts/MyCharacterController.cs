using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using System;
using UnityEngine.Serialization;

namespace KinematicCharacterController.Walkthrough.SwimmingState
{
    public enum CharacterState
    {
        Default,
        Swimming,
    }

    public struct PlayerCharacterInputs
    {
        public float MoveAxisForward;
        public float MoveAxisRight;
        public Quaternion CameraRotation;
        public bool JumpDown;
        public bool JumpHeld;
        public bool CrouchDown;
        public bool CrouchUp;
        public bool CrouchHeld;
    }

    public class MyCharacterController : MonoBehaviour, ICharacterController
    {
        [FormerlySerializedAs("Motor")] public KinematicCharacterMotor _motor;

        [FormerlySerializedAs("MaxStableMoveSpeed")] [Header("Stable Movement")]
        public float _maxStableMoveSpeed = 10f;
        [FormerlySerializedAs("StableMovementSharpness")] public float _stableMovementSharpness = 15;
        [FormerlySerializedAs("OrientationSharpness")] public float _orientationSharpness = 10;
        [FormerlySerializedAs("MaxStableDistanceFromLedge")] public float _maxStableDistanceFromLedge = 5f;
        [FormerlySerializedAs("MaxStableDenivelationAngle")] [Range(0f, 180f)]
        public float _maxStableDenivelationAngle = 180f;

        [FormerlySerializedAs("MaxAirMoveSpeed")] [Header("Air Movement")]
        public float _maxAirMoveSpeed = 10f;
        [FormerlySerializedAs("AirAccelerationSpeed")] public float _airAccelerationSpeed = 5f;
        [FormerlySerializedAs("Drag")] public float _drag = 0.1f;

        [FormerlySerializedAs("AllowJumpingWhenSliding")] [Header("Jumping")]
        public bool _allowJumpingWhenSliding = false;
        [FormerlySerializedAs("AllowDoubleJump")] public bool _allowDoubleJump = false;
        [FormerlySerializedAs("AllowWallJump")] public bool _allowWallJump = false;
        [FormerlySerializedAs("JumpSpeed")] public float _jumpSpeed = 10f;
        [FormerlySerializedAs("JumpPreGroundingGraceTime")] public float _jumpPreGroundingGraceTime = 0f;
        [FormerlySerializedAs("JumpPostGroundingGraceTime")] public float _jumpPostGroundingGraceTime = 0f;

        [FormerlySerializedAs("SwimmingReferencePoint")] [Header("Swimming")]
        public Transform _swimmingReferencePoint;
        [FormerlySerializedAs("WaterLayer")] public LayerMask _waterLayer;
        [FormerlySerializedAs("SwimmingSpeed")] public float _swimmingSpeed = 4f;
        [FormerlySerializedAs("SwimmingMovementSharpness")] public float _swimmingMovementSharpness = 3;
        [FormerlySerializedAs("SwimmingOrientationSharpness")] public float _swimmingOrientationSharpness = 2f;

        [FormerlySerializedAs("IgnoredColliders")] [Header("Misc")]
        public List<Collider> _ignoredColliders = new List<Collider>();
        [FormerlySerializedAs("OrientTowardsGravity")] public bool _orientTowardsGravity = false;
        [FormerlySerializedAs("Gravity")] public Vector3 _gravity = new Vector3(0, -30f, 0);
        [FormerlySerializedAs("MeshRoot")] public Transform _meshRoot;

        public CharacterState CurrentCharacterState { get; private set; }

        private Collider[] _probedColliders = new Collider[8];
        private Vector3 _moveInputVector;
        private Vector3 _lookInputVector;
        private bool _jumpInputIsHeld = false;
        private bool _crouchInputIsHeld = false;
        private bool _jumpRequested = false;
        private bool _jumpConsumed = false;
        private bool _doubleJumpConsumed = false;
        private bool _jumpedThisFrame = false;
        private bool _canWallJump = false;
        private Vector3 _wallJumpNormal;
        private float _timeSinceJumpRequested = Mathf.Infinity;
        private float _timeSinceLastAbleToJump = 0f;
        private Vector3 _internalVelocityAdd = Vector3.zero;
        private bool _shouldBeCrouching = false;
        private bool _isCrouching = false;
        private Collider _waterZone;

        private void Start()
        {
            // Assign to motor
            _motor.CharacterController = this;

            // Handle initial state
            TransitionToState(CharacterState.Default);
        }

        /// <summary>
        /// Handles movement state transitions and enter/exit callbacks
        /// </summary>
        public void TransitionToState(CharacterState newState)
        {
            CharacterState tmpInitialState = CurrentCharacterState;
            OnStateExit(tmpInitialState, newState);
            CurrentCharacterState = newState;
            OnStateEnter(newState, tmpInitialState);
        }

        /// <summary>
        /// Event when entering a state
        /// </summary>
        public void OnStateEnter(CharacterState state, CharacterState fromState)
        {
            switch (state)
            {
                case CharacterState.Default:
                    {
                        _motor.SetGroundSolvingActivation(true);
                        break;
                    }
                case CharacterState.Swimming:
                    {
                        _motor.SetGroundSolvingActivation(false);
                        break;
                    }
            }
        }

        /// <summary>
        /// Event when exiting a state
        /// </summary>
        public void OnStateExit(CharacterState state, CharacterState toState)
        {
            switch (state)
            {
                case CharacterState.Default:
                    {
                        break;
                    }
            }
        }

        /// <summary>
        /// This is called every frame by MyPlayer in order to tell the character what its inputs are
        /// </summary>
        public void SetInputs(ref PlayerCharacterInputs inputs)
        {
            _jumpInputIsHeld = inputs.JumpHeld;
            _crouchInputIsHeld = inputs.CrouchHeld;

            // Clamp input
            Vector3 moveInputVector = Vector3.ClampMagnitude(new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward), 1f);

            // Calculate camera direction and rotation on the character plane
            Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, _motor.CharacterUp).normalized;
            if (cameraPlanarDirection.sqrMagnitude == 0f)
            {
                cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.up, _motor.CharacterUp).normalized;
            }
            Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, _motor.CharacterUp);

            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        // Move and look inputs
                        _moveInputVector = cameraPlanarRotation * moveInputVector;
                        _lookInputVector = cameraPlanarDirection;

                        // Jumping input
                        if (inputs.JumpDown)
                        {
                            _timeSinceJumpRequested = 0f;
                            _jumpRequested = true;
                        }

                        // Crouching input
                        if (inputs.CrouchDown)
                        {
                            _shouldBeCrouching = true;

                            if (!_isCrouching)
                            {
                                _isCrouching = true;
                                _motor.SetCapsuleDimensions(0.5f, 1f, 0.5f);
                                _meshRoot.localScale = new Vector3(1f, 0.5f, 1f);
                            }
                        }
                        else if (inputs.CrouchUp)
                        {
                            _shouldBeCrouching = false;
                        }
                        break;
                    }
                case CharacterState.Swimming:
                    {
                        _jumpRequested = inputs.JumpHeld;

                        _moveInputVector = inputs.CameraRotation * moveInputVector;
                        _lookInputVector = cameraPlanarDirection;
                        break;
                    }
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is called before the character begins its movement update
        /// </summary>
        public void BeforeCharacterUpdate(float deltaTime)
        {
            // Handle detecting water surfaces
            {
                // Do a character overlap test to detect water surfaces
                if (_motor.CharacterOverlap(_motor.TransientPosition, _motor.TransientRotation, _probedColliders, _waterLayer, QueryTriggerInteraction.Collide) > 0)
                {
                    // If a water surface was detected
                    if (_probedColliders[0] != null)
                    {
                        // If the swimming reference point is inside the box, make sure we are in swimming state
                        if (Physics.ClosestPoint(_swimmingReferencePoint.position, _probedColliders[0], _probedColliders[0].transform.position, _probedColliders[0].transform.rotation) == _swimmingReferencePoint.position)
                        {
                            if (CurrentCharacterState == CharacterState.Default)
                            {
                                TransitionToState(CharacterState.Swimming);
                                _waterZone = _probedColliders[0];
                            }
                        }
                        // otherwise; default state
                        else
                        {
                            if (CurrentCharacterState == CharacterState.Swimming)
                            {
                                TransitionToState(CharacterState.Default);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its rotation should be right now. 
        /// This is the ONLY place where you should set the character's rotation
        /// </summary>
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                case CharacterState.Swimming:
                    {
                        if (_lookInputVector != Vector3.zero && _orientationSharpness > 0f)
                        {
                            // Smoothly interpolate from current to target look direction
                            Vector3 smoothedLookInputDirection = Vector3.Slerp(_motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-_orientationSharpness * deltaTime)).normalized;

                            // Set the current rotation (which will be used by the KinematicCharacterMotor)
                            currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, _motor.CharacterUp);
                        }
                        if (_orientTowardsGravity)
                        {
                            // Rotate from current up to invert gravity
                            currentRotation = Quaternion.FromToRotation((currentRotation * Vector3.up), -_gravity) * currentRotation;
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its velocity should be right now. 
        /// This is the ONLY place where you can set the character's velocity
        /// </summary>
        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        Vector3 targetMovementVelocity = Vector3.zero;
                        if (_motor.GroundingStatus.IsStableOnGround)
                        {
                            // Reorient velocity on slope
                            currentVelocity = _motor.GetDirectionTangentToSurface(currentVelocity, _motor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;

                            // Calculate target velocity
                            Vector3 inputRight = Vector3.Cross(_moveInputVector, _motor.CharacterUp);
                            Vector3 reorientedInput = Vector3.Cross(_motor.GroundingStatus.GroundNormal, inputRight).normalized * _moveInputVector.magnitude;
                            targetMovementVelocity = reorientedInput * _maxStableMoveSpeed;

                            // Smooth movement Velocity
                            currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1 - Mathf.Exp(-_stableMovementSharpness * deltaTime));
                        }
                        else
                        {
                            // Add move input
                            if (_moveInputVector.sqrMagnitude > 0f)
                            {
                                targetMovementVelocity = _moveInputVector * _maxAirMoveSpeed;

                                // Prevent climbing on un-stable slopes with air movement
                                if (_motor.GroundingStatus.FoundAnyGround)
                                {
                                    Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(_motor.CharacterUp, _motor.GroundingStatus.GroundNormal), _motor.CharacterUp).normalized;
                                    targetMovementVelocity = Vector3.ProjectOnPlane(targetMovementVelocity, perpenticularObstructionNormal);
                                }

                                Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, _gravity);
                                currentVelocity += velocityDiff * _airAccelerationSpeed * deltaTime;
                            }

                            // Gravity
                            currentVelocity += _gravity * deltaTime;

                            // Drag
                            currentVelocity *= (1f / (1f + (_drag * deltaTime)));
                        }

                        // Handle jumping
                        {
                            _jumpedThisFrame = false;
                            _timeSinceJumpRequested += deltaTime;
                            if (_jumpRequested)
                            {
                                // Handle double jump
                                if (_allowDoubleJump)
                                {
                                    if (_jumpConsumed && !_doubleJumpConsumed && (_allowJumpingWhenSliding ? !_motor.GroundingStatus.FoundAnyGround : !_motor.GroundingStatus.IsStableOnGround))
                                    {
                                        _motor.ForceUnground(0.1f);

                                        // Add to the return velocity and reset jump state
                                        currentVelocity += (_motor.CharacterUp * _jumpSpeed) - Vector3.Project(currentVelocity, _motor.CharacterUp);
                                        _jumpRequested = false;
                                        _doubleJumpConsumed = true;
                                        _jumpedThisFrame = true;
                                    }
                                }

                                // See if we actually are allowed to jump
                                if (_canWallJump ||
                                    (!_jumpConsumed && ((_allowJumpingWhenSliding ? _motor.GroundingStatus.FoundAnyGround : _motor.GroundingStatus.IsStableOnGround) || _timeSinceLastAbleToJump <= _jumpPostGroundingGraceTime)))
                                {
                                    // Calculate jump direction before ungrounding
                                    Vector3 jumpDirection = _motor.CharacterUp;
                                    if (_canWallJump)
                                    {
                                        jumpDirection = _wallJumpNormal;
                                    }
                                    else if (_motor.GroundingStatus.FoundAnyGround && !_motor.GroundingStatus.IsStableOnGround)
                                    {
                                        jumpDirection = _motor.GroundingStatus.GroundNormal;
                                    }

                                    // Makes the character skip ground probing/snapping on its next update. 
                                    // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
                                    _motor.ForceUnground(0.1f);

                                    // Add to the return velocity and reset jump state
                                    currentVelocity += (jumpDirection * _jumpSpeed) - Vector3.Project(currentVelocity, _motor.CharacterUp);
                                    _jumpRequested = false;
                                    _jumpConsumed = true;
                                    _jumpedThisFrame = true;
                                }
                            }

                            // Reset wall jump
                            _canWallJump = false;
                        }

                        // Take into account additive velocity
                        if (_internalVelocityAdd.sqrMagnitude > 0f)
                        {
                            currentVelocity += _internalVelocityAdd;
                            _internalVelocityAdd = Vector3.zero;
                        }
                        break;
                    }
                case CharacterState.Swimming:
                    {
                        float verticalInput = 0f + (_jumpInputIsHeld ? 1f : 0f) + (_crouchInputIsHeld ? -1f : 0f);

                        // Smoothly interpolate to target swimming velocity
                        Vector3 targetMovementVelocity = (_moveInputVector + (_motor.CharacterUp * verticalInput)).normalized * _swimmingSpeed;
                        Vector3 smoothedVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1 - Mathf.Exp(-_swimmingMovementSharpness * deltaTime));

                        // See if our swimming reference point would be out of water after the movement from our velocity has been applied
                        {
                            Vector3 resultingSwimmingReferancePosition = _motor.TransientPosition + (smoothedVelocity * deltaTime) + (_swimmingReferencePoint.position - _motor.TransientPosition);
                            Vector3 closestPointWaterSurface = Physics.ClosestPoint(resultingSwimmingReferancePosition, _waterZone, _waterZone.transform.position, _waterZone.transform.rotation);

                            // if our position would be outside the water surface on next update, project the velocity on the surface normal so that it would not take us out of the water
                            if (closestPointWaterSurface != resultingSwimmingReferancePosition)
                            {
                                Vector3 waterSurfaceNormal = (resultingSwimmingReferancePosition - closestPointWaterSurface).normalized;
                                smoothedVelocity = Vector3.ProjectOnPlane(smoothedVelocity, waterSurfaceNormal);

                                // Jump out of water
                                if (_jumpRequested)
                                {
                                    smoothedVelocity += (_motor.CharacterUp * _jumpSpeed) - Vector3.Project(currentVelocity, _motor.CharacterUp);
                                }
                            }
                        }

                        currentVelocity = smoothedVelocity;
                        break;
                    }
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is called after the character has finished its movement update
        /// </summary>
        public void AfterCharacterUpdate(float deltaTime)
        {
            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        // Handle jump-related values
                        {
                            // Handle jumping pre-ground grace period
                            if (_jumpRequested && _timeSinceJumpRequested > _jumpPreGroundingGraceTime)
                            {
                                _jumpRequested = false;
                            }

                            if (_allowJumpingWhenSliding ? _motor.GroundingStatus.FoundAnyGround : _motor.GroundingStatus.IsStableOnGround)
                            {
                                // If we're on a ground surface, reset jumping values
                                if (!_jumpedThisFrame)
                                {
                                    _doubleJumpConsumed = false;
                                    _jumpConsumed = false;
                                }
                                _timeSinceLastAbleToJump = 0f;
                            }
                            else
                            {
                                // Keep track of time since we were last able to jump (for grace period)
                                _timeSinceLastAbleToJump += deltaTime;
                            }
                        }

                        // Handle uncrouching
                        if (_isCrouching && !_shouldBeCrouching)
                        {
                            // Do an overlap test with the character's standing height to see if there are any obstructions
                            _motor.SetCapsuleDimensions(0.5f, 2f, 1f);
                            if (_motor.CharacterOverlap(
                                _motor.TransientPosition,
                                _motor.TransientRotation,
                                _probedColliders,
                                _motor.CollidableLayers,
                                QueryTriggerInteraction.Ignore) > 0)
                            {
                                // If obstructions, just stick to crouching dimensions
                                _motor.SetCapsuleDimensions(0.5f, 1f, 0.5f);
                            }
                            else
                            {
                                // If no obstructions, uncrouch
                                _meshRoot.localScale = new Vector3(1f, 1f, 1f);
                                _isCrouching = false;
                            }
                        }
                        break;
                    }
            }
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            if (_ignoredColliders.Contains(coll))
            {
                return false;
            }
            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        // We can wall jump only if we are not stable on ground and are moving against an obstruction
                        if (_allowWallJump && !_motor.GroundingStatus.IsStableOnGround && !hitStabilityReport.IsStable)
                        {
                            _canWallJump = true;
                            _wallJumpNormal = hitNormal;
                        }
                        break;
                    }
            }
        }

        public void AddVelocity(Vector3 velocity)
        {
            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        _internalVelocityAdd += velocity;
                        break;
                    }
            }
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void PostGroundingUpdate(float deltaTime)
        {
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
        }
    }
}