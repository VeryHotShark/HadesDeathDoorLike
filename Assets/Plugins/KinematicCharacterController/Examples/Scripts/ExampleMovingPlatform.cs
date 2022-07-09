using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace KinematicCharacterController.Examples
{
    public class ExampleMovingPlatform : MonoBehaviour, IMoverController
    {
        [FormerlySerializedAs("Mover")] public PhysicsMover _mover;

        [FormerlySerializedAs("TranslationAxis")] public Vector3 _translationAxis = Vector3.right;
        [FormerlySerializedAs("TranslationPeriod")] public float _translationPeriod = 10;
        [FormerlySerializedAs("TranslationSpeed")] public float _translationSpeed = 1;
        [FormerlySerializedAs("RotationAxis")] public Vector3 _rotationAxis = Vector3.up;
        [FormerlySerializedAs("RotSpeed")] public float _rotSpeed = 10;
        [FormerlySerializedAs("OscillationAxis")] public Vector3 _oscillationAxis = Vector3.zero;
        [FormerlySerializedAs("OscillationPeriod")] public float _oscillationPeriod = 10;
        [FormerlySerializedAs("OscillationSpeed")] public float _oscillationSpeed = 10;

        private Vector3 _originalPosition;
        private Quaternion _originalRotation;
        
        private void Start()
        {
            _originalPosition = _mover._rigidbody.position;
            _originalRotation = _mover._rigidbody.rotation;

            _mover.MoverController = this;
        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
        {
            goalPosition = (_originalPosition + (_translationAxis.normalized * Mathf.Sin(Time.time * _translationSpeed) * _translationPeriod));

            Quaternion targetRotForOscillation = Quaternion.Euler(_oscillationAxis.normalized * (Mathf.Sin(Time.time * _oscillationSpeed) * _oscillationPeriod)) * _originalRotation;
            goalRotation = Quaternion.Euler(_rotationAxis * _rotSpeed * Time.time) * targetRotForOscillation;
        }
    }
}