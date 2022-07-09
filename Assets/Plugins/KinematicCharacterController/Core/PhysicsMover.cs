using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace KinematicCharacterController
{
    /// <summary>
    /// Represents the entire state of a PhysicsMover that is pertinent for simulation.
    /// Use this to save state or revert to past state
    /// </summary>
    [System.Serializable]
    public struct PhysicsMoverState
    {
        [FormerlySerializedAs("Position")] public Vector3 _position;
        [FormerlySerializedAs("Rotation")] public Quaternion _rotation;
        [FormerlySerializedAs("Velocity")] public Vector3 _velocity;
        [FormerlySerializedAs("AngularVelocity")] public Vector3 _angularVelocity;
    }

    /// <summary>
    /// Component that manages the movement of moving kinematic rigidbodies for
    /// proper interaction with characters
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicsMover : MonoBehaviour
    {
        /// <summary>
        /// The mover's Rigidbody
        /// </summary>
        [FormerlySerializedAs("Rigidbody")] [ReadOnly]
        public Rigidbody _rigidbody;

        /// <summary>
        /// Determines if the platform moves with rigidbody.MovePosition (when true), or with rigidbody.position (when false)
        /// </summary>
        [FormerlySerializedAs("MoveWithPhysics")] public bool _moveWithPhysics = true;

        /// <summary>
        /// Index of this motor in KinematicCharacterSystem arrays
        /// </summary>
        [NonSerialized]
        public IMoverController MoverController;
        /// <summary>
        /// Remembers latest position in interpolation
        /// </summary>
        [NonSerialized]
        public Vector3 LatestInterpolationPosition;
        /// <summary>
        /// Remembers latest rotation in interpolation
        /// </summary>
        [NonSerialized]
        public Quaternion LatestInterpolationRotation;
        /// <summary>
        /// The latest movement made by interpolation
        /// </summary>
        [NonSerialized]
        public Vector3 PositionDeltaFromInterpolation;
        /// <summary>
        /// The latest rotation made by interpolation
        /// </summary>
        [NonSerialized]
        public Quaternion RotationDeltaFromInterpolation;

        /// <summary>
        /// Index of this motor in KinematicCharacterSystem arrays
        /// </summary>
        public int IndexInCharacterSystem { get; set; }
        /// <summary>
        /// Remembers initial position before all simulation are done
        /// </summary>
        public Vector3 Velocity { get; protected set; }
        /// <summary>
        /// Remembers initial position before all simulation are done
        /// </summary>
        public Vector3 AngularVelocity { get; protected set; }
        /// <summary>
        /// Remembers initial position before all simulation are done
        /// </summary>
        public Vector3 InitialTickPosition { get; set; }
        /// <summary>
        /// Remembers initial rotation before all simulation are done
        /// </summary>
        public Quaternion InitialTickRotation { get; set; }

        /// <summary>
        /// The mover's Transform
        /// </summary>
        public Transform Transform { get; private set; }
        /// <summary>
        /// The character's position before the movement calculations began
        /// </summary>
        public Vector3 InitialSimulationPosition { get; private set; }
        /// <summary>
        /// The character's rotation before the movement calculations began
        /// </summary>
        public Quaternion InitialSimulationRotation { get; private set; }

        private Vector3 _internalTransientPosition;

        /// <summary>
        /// The mover's rotation (always up-to-date during the character update phase)
        /// </summary>
        public Vector3 TransientPosition
        {
            get
            {
                return _internalTransientPosition;
            }
            private set
            {
                _internalTransientPosition = value;
            }
        }

        private Quaternion _internalTransientRotation;
        /// <summary>
        /// The mover's rotation (always up-to-date during the character update phase)
        /// </summary>
        public Quaternion TransientRotation
        {
            get
            {
                return _internalTransientRotation;
            }
            private set
            {
                _internalTransientRotation = value;
            }
        }


        private void Reset()
        {
            ValidateData();
        }

        private void OnValidate()
        {
            ValidateData();
        }

        /// <summary>
        /// Handle validating all required values
        /// </summary>
        public void ValidateData()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();

            _rigidbody.centerOfMass = Vector3.zero;
            _rigidbody.maxAngularVelocity = Mathf.Infinity;
            _rigidbody.maxDepenetrationVelocity = Mathf.Infinity;
            _rigidbody.isKinematic = true;
            _rigidbody.interpolation = RigidbodyInterpolation.None;
        }

        private void OnEnable()
        {
            KinematicCharacterSystem.EnsureCreation();
            KinematicCharacterSystem.RegisterPhysicsMover(this);
        }

        private void OnDisable()
        {
            KinematicCharacterSystem.UnregisterPhysicsMover(this);
        }

        private void Awake()
        {
            Transform = this.transform;
            ValidateData();

            TransientPosition = _rigidbody.position;
            TransientRotation = _rigidbody.rotation;
            InitialSimulationPosition = _rigidbody.position;
            InitialSimulationRotation = _rigidbody.rotation;
            LatestInterpolationPosition = Transform.position;
            LatestInterpolationRotation = Transform.rotation;
        }

        /// <summary>
        /// Sets the mover's position directly
        /// </summary>
        public void SetPosition(Vector3 position)
        {
            Transform.position = position;
            _rigidbody.position = position;
            InitialSimulationPosition = position;
            TransientPosition = position;
        }

        /// <summary>
        /// Sets the mover's rotation directly
        /// </summary>
        public void SetRotation(Quaternion rotation)
        {
            Transform.rotation = rotation;
            _rigidbody.rotation = rotation;
            InitialSimulationRotation = rotation;
            TransientRotation = rotation;
        }

        /// <summary>
        /// Sets the mover's position and rotation directly
        /// </summary>
        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            Transform.SetPositionAndRotation(position, rotation);
            _rigidbody.position = position;
            _rigidbody.rotation = rotation;
            InitialSimulationPosition = position;
            InitialSimulationRotation = rotation;
            TransientPosition = position;
            TransientRotation = rotation;
        }

        /// <summary>
        /// Returns all the state information of the mover that is pertinent for simulation
        /// </summary>
        public PhysicsMoverState GetState()
        {
            PhysicsMoverState state = new PhysicsMoverState();

            state._position = TransientPosition;
            state._rotation = TransientRotation;
            state._velocity = Velocity;
            state._angularVelocity = AngularVelocity;

            return state;
        }

        /// <summary>
        /// Applies a mover state instantly
        /// </summary>
        public void ApplyState(PhysicsMoverState state)
        {
            SetPositionAndRotation(state._position, state._rotation);
            Velocity = state._velocity;
            AngularVelocity = state._angularVelocity;
        }

        /// <summary>
        /// Caches velocity values based on deltatime and target position/rotations
        /// </summary>
        public void VelocityUpdate(float deltaTime)
        {
            InitialSimulationPosition = TransientPosition;
            InitialSimulationRotation = TransientRotation;

            MoverController.UpdateMovement(out _internalTransientPosition, out _internalTransientRotation, deltaTime);

            if (deltaTime > 0f)
            {
                Velocity = (TransientPosition - InitialSimulationPosition) / deltaTime;
                                
                Quaternion rotationFromCurrentToGoal = TransientRotation * (Quaternion.Inverse(InitialSimulationRotation));
                AngularVelocity = (Mathf.Deg2Rad * rotationFromCurrentToGoal.eulerAngles) / deltaTime;
            }
        }
    }
}