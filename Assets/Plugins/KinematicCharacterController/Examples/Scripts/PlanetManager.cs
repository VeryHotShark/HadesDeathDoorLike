using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using System;
using UnityEngine.Serialization;

namespace KinematicCharacterController.Examples
{
    public class PlanetManager : MonoBehaviour, IMoverController
    {
        [FormerlySerializedAs("PlanetMover")] public PhysicsMover _planetMover;
        [FormerlySerializedAs("GravityField")] public SphereCollider _gravityField;
        [FormerlySerializedAs("GravityStrength")] public float _gravityStrength = 10;
        [FormerlySerializedAs("OrbitAxis")] public Vector3 _orbitAxis = Vector3.forward;
        [FormerlySerializedAs("OrbitSpeed")] public float _orbitSpeed = 10;

        [FormerlySerializedAs("OnPlaygroundTeleportingZone")] public Teleporter _onPlaygroundTeleportingZone;
        [FormerlySerializedAs("OnPlanetTeleportingZone")] public Teleporter _onPlanetTeleportingZone;

        private List<ExampleCharacterController> _characterControllersOnPlanet = new List<ExampleCharacterController>();
        private Vector3 _savedGravity;
        private Quaternion _lastRotation;

        private void Start()
        {
            _onPlaygroundTeleportingZone.OnCharacterTeleport -= ControlGravity;
            _onPlaygroundTeleportingZone.OnCharacterTeleport += ControlGravity;

            _onPlanetTeleportingZone.OnCharacterTeleport -= UnControlGravity;
            _onPlanetTeleportingZone.OnCharacterTeleport += UnControlGravity;

            _lastRotation = _planetMover.transform.rotation;

            _planetMover.MoverController = this;
        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
        {
            goalPosition = _planetMover._rigidbody.position;

            // Rotate
            Quaternion targetRotation = Quaternion.Euler(_orbitAxis * _orbitSpeed * deltaTime) * _lastRotation;
            goalRotation = targetRotation;
            _lastRotation = targetRotation;

            // Apply gravity to characters
            foreach (ExampleCharacterController cc in _characterControllersOnPlanet)
            {
                cc.Gravity = (_planetMover.transform.position - cc.transform.position).normalized * _gravityStrength;
            }
        }

        void ControlGravity(ExampleCharacterController cc)
        {
            _savedGravity = cc.Gravity;
            _characterControllersOnPlanet.Add(cc);
        }

        void UnControlGravity(ExampleCharacterController cc)
        {
            cc.Gravity = _savedGravity;
            _characterControllersOnPlanet.Remove(cc);
        }
    }
}