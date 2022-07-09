using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KinematicCharacterController
{
    /// <summary>
    /// The system that manages the simulation of KinematicCharacterMotor and PhysicsMover
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class KinematicCharacterSystem : MonoBehaviour
    {
        private static KinematicCharacterSystem _instance;

        public static List<KinematicCharacterMotor> CharacterMotors = new List<KinematicCharacterMotor>();
        public static List<PhysicsMover> PhysicsMovers = new List<PhysicsMover>();

        private static float _lastCustomInterpolationStartTime = -1f;
        private static float _lastCustomInterpolationDeltaTime = -1f;

        public static KccSettings Settings;

        /// <summary>
        /// Creates a KinematicCharacterSystem instance if there isn't already one
        /// </summary>
        public static void EnsureCreation()
        {
            if (_instance == null)
            {
                GameObject systemGameObject = new GameObject("KinematicCharacterSystem");
                _instance = systemGameObject.AddComponent<KinematicCharacterSystem>();

                systemGameObject.hideFlags = HideFlags.NotEditable;
                _instance.hideFlags = HideFlags.NotEditable;

                Settings = ScriptableObject.CreateInstance<KccSettings>();

                GameObject.DontDestroyOnLoad(systemGameObject);
            }
        }

        /// <summary>
        /// Gets the KinematicCharacterSystem instance if any
        /// </summary>
        /// <returns></returns>
        public static KinematicCharacterSystem GetInstance()
        {
            return _instance;
        }

        /// <summary>
        /// Sets the maximum capacity of the character motors list, to prevent allocations when adding characters
        /// </summary>
        /// <param name="capacity"></param>
        public static void SetCharacterMotorsCapacity(int capacity)
        {
            if (capacity < CharacterMotors.Count)
            {
                capacity = CharacterMotors.Count;
            }
            CharacterMotors.Capacity = capacity;
        }

        /// <summary>
        /// Registers a KinematicCharacterMotor into the system
        /// </summary>
        public static void RegisterCharacterMotor(KinematicCharacterMotor motor)
        {
            CharacterMotors.Add(motor);
        }

        /// <summary>
        /// Unregisters a KinematicCharacterMotor from the system
        /// </summary>
        public static void UnregisterCharacterMotor(KinematicCharacterMotor motor)
        {
            CharacterMotors.Remove(motor);
        }

        /// <summary>
        /// Sets the maximum capacity of the physics movers list, to prevent allocations when adding movers
        /// </summary>
        /// <param name="capacity"></param>
        public static void SetPhysicsMoversCapacity(int capacity)
        {
            if (capacity < PhysicsMovers.Count)
            {
                capacity = PhysicsMovers.Count;
            }
            PhysicsMovers.Capacity = capacity;
        }

        /// <summary>
        /// Registers a PhysicsMover into the system
        /// </summary>
        public static void RegisterPhysicsMover(PhysicsMover mover)
        {
            PhysicsMovers.Add(mover);

            mover._rigidbody.interpolation = RigidbodyInterpolation.None;
        }

        /// <summary>
        /// Unregisters a PhysicsMover from the system
        /// </summary>
        public static void UnregisterPhysicsMover(PhysicsMover mover)
        {
            PhysicsMovers.Remove(mover);
        }

        // This is to prevent duplicating the singleton gameobject on script recompiles
        private void OnDisable()
        {
            Destroy(this.gameObject);
        }

        private void Awake()
        {
            _instance = this;
        }

        private void FixedUpdate()
        {
            if (Settings._autoSimulation)
            {
                float deltaTime = Time.deltaTime;

                if (Settings._interpolate)
                {
                    PreSimulationInterpolationUpdate(deltaTime);
                }

                Simulate(deltaTime, CharacterMotors, PhysicsMovers);

                if (Settings._interpolate)
                {
                    PostSimulationInterpolationUpdate(deltaTime);
                }
            }
        }

        private void LateUpdate()
        {
            if (Settings._interpolate)
            {
                CustomInterpolationUpdate();
            }
        }

        /// <summary>
        /// Remembers the point to interpolate from for KinematicCharacterMotors and PhysicsMovers
        /// </summary>
        public static void PreSimulationInterpolationUpdate(float deltaTime)
        {
            // Save pre-simulation poses and place transform at transient pose
            for (int i = 0; i < CharacterMotors.Count; i++)
            {
                KinematicCharacterMotor motor = CharacterMotors[i];

                motor.InitialTickPosition = motor.TransientPosition;
                motor.InitialTickRotation = motor.TransientRotation;

                motor.Transform.SetPositionAndRotation(motor.TransientPosition, motor.TransientRotation);
            }

            for (int i = 0; i < PhysicsMovers.Count; i++)
            {
                PhysicsMover mover = PhysicsMovers[i];

                mover.InitialTickPosition = mover.TransientPosition;
                mover.InitialTickRotation = mover.TransientRotation;

                mover.Transform.SetPositionAndRotation(mover.TransientPosition, mover.TransientRotation);
                mover._rigidbody.position = mover.TransientPosition;
                mover._rigidbody.rotation = mover.TransientRotation;
            }
        }

        /// <summary>
        /// Ticks characters and/or movers
        /// </summary>
        public static void Simulate(float deltaTime, List<KinematicCharacterMotor> motors, List<PhysicsMover> movers)
        {
            int characterMotorsCount = motors.Count;
            int physicsMoversCount = movers.Count;

#pragma warning disable 0162
            // Update PhysicsMover velocities
            for (int i = 0; i < physicsMoversCount; i++)
            {
                movers[i].VelocityUpdate(deltaTime);
            }

            // Character controller update phase 1
            for (int i = 0; i < characterMotorsCount; i++)
            {
                motors[i].UpdatePhase1(deltaTime);
            }

            // Simulate PhysicsMover displacement
            for (int i = 0; i < physicsMoversCount; i++)
            {
                PhysicsMover mover = movers[i];

                mover.Transform.SetPositionAndRotation(mover.TransientPosition, mover.TransientRotation);
                mover._rigidbody.position = mover.TransientPosition;
                mover._rigidbody.rotation = mover.TransientRotation;
            }

            // Character controller update phase 2 and move
            for (int i = 0; i < characterMotorsCount; i++)
            {
                KinematicCharacterMotor motor = motors[i];

                motor.UpdatePhase2(deltaTime);

                motor.Transform.SetPositionAndRotation(motor.TransientPosition, motor.TransientRotation);
            }
#pragma warning restore 0162
        }

        /// <summary>
        /// Initiates the interpolation for KinematicCharacterMotors and PhysicsMovers
        /// </summary>
        public static void PostSimulationInterpolationUpdate(float deltaTime)
        {
            _lastCustomInterpolationStartTime = Time.time;
            _lastCustomInterpolationDeltaTime = deltaTime;

            // Return interpolated roots to their initial poses
            for (int i = 0; i < CharacterMotors.Count; i++)
            {
                KinematicCharacterMotor motor = CharacterMotors[i];

                motor.Transform.SetPositionAndRotation(motor.InitialTickPosition, motor.InitialTickRotation);
            }

            for (int i = 0; i < PhysicsMovers.Count; i++)
            {
                PhysicsMover mover = PhysicsMovers[i];

                if (mover._moveWithPhysics)
                {
                    mover._rigidbody.position = mover.InitialTickPosition;
                    mover._rigidbody.rotation = mover.InitialTickRotation;

                    mover._rigidbody.MovePosition(mover.TransientPosition);
                    mover._rigidbody.MoveRotation(mover.TransientRotation);
                }
                else
                {
                    mover._rigidbody.position = (mover.TransientPosition);
                    mover._rigidbody.rotation = (mover.TransientRotation);
                }
            }
        }

        /// <summary>
        /// Handles per-frame interpolation
        /// </summary>
        private static void CustomInterpolationUpdate()
        {
            float interpolationFactor = Mathf.Clamp01((Time.time - _lastCustomInterpolationStartTime) / _lastCustomInterpolationDeltaTime);

            // Handle characters interpolation
            for (int i = 0; i < CharacterMotors.Count; i++)
            {
                KinematicCharacterMotor motor = CharacterMotors[i];

                motor.Transform.SetPositionAndRotation(
                    Vector3.Lerp(motor.InitialTickPosition, motor.TransientPosition, interpolationFactor),
                    Quaternion.Slerp(motor.InitialTickRotation, motor.TransientRotation, interpolationFactor));
            }

            // Handle PhysicsMovers interpolation
            for (int i = 0; i < PhysicsMovers.Count; i++)
            {
                PhysicsMover mover = PhysicsMovers[i];
                
                mover.Transform.SetPositionAndRotation(
                    Vector3.Lerp(mover.InitialTickPosition, mover.TransientPosition, interpolationFactor),
                    Quaternion.Slerp(mover.InitialTickRotation, mover.TransientRotation, interpolationFactor));

                Vector3 newPos = mover.Transform.position;
                Quaternion newRot = mover.Transform.rotation;
                mover.PositionDeltaFromInterpolation = newPos - mover.LatestInterpolationPosition;
                mover.RotationDeltaFromInterpolation = Quaternion.Inverse(mover.LatestInterpolationRotation) * newRot;
                mover.LatestInterpolationPosition = newPos;
                mover.LatestInterpolationRotation = newRot;
            }
        }
    }
}