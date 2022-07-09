using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace KinematicCharacterController.Walkthrough.PlayerCameraCharacterSetup
{
    public class MyPlayer : MonoBehaviour
    {
        [FormerlySerializedAs("OrbitCamera")] public ExampleCharacterCamera _orbitCamera;
        [FormerlySerializedAs("CameraFollowPoint")] public Transform _cameraFollowPoint;
        [FormerlySerializedAs("Character")] public MyCharacterController _character;

        private Vector3 _lookInputVector = Vector3.zero;
        
        private Vector2 _lookDelta;
        private PlayerInput _playerInput;

        private void Awake() {
            _playerInput = new PlayerInput();
            
            // _playerInput.CharacterControls.Look.performed += ctx => _lookDelta = ctx.ReadValue<Vector2>();
            // _playerInput.CharacterControls.Look.canceled += ctx => _lookDelta = ctx.ReadValue<Vector2>();
        }

        private void OnEnable() {
            // _playerInput.Enable();
        }

        private void OnDisable() {
            // _playerInput.Disable();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            // Tell camera to follow transform
            _orbitCamera.SetFollowTransform(_cameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            _orbitCamera.IgnoredColliders = _character.GetComponentsInChildren<Collider>().ToList();
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private void LateUpdate()
        {
            HandleCameraInput();
        }

        private void HandleCameraInput()
        {
            // Create the look input vector for the camera
            _lookInputVector = new Vector3(_lookDelta.x, _lookDelta.y, 0f);

            // Prevent moving the camera while the cursor isn't locked
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                _lookInputVector = Vector3.zero;
            }

            // Input for zooming the camera (disabled in WebGL because it can cause problems)
            // float scrollInput = -Input.GetAxis("Mouse ScrollWheel");
    #if UNITY_WEBGL
            // scrollInput = 0f;
    #endif

            // Apply inputs to the camera
            _orbitCamera.UpdateWithInput(Time.deltaTime,0f, _lookInputVector);

            // Handle toggling zoom level
            if (Mouse.current.middleButton.wasPressedThisFrame)
            {
                _orbitCamera.TargetDistance = (_orbitCamera.TargetDistance == 0f) ? _orbitCamera.DefaultDistance : 0f;
            }
        }
    }
}