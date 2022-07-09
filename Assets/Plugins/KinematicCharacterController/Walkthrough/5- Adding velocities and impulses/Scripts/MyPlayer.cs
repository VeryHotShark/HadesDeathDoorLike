using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace KinematicCharacterController.Walkthrough.AddingImpulses
{
    public class MyPlayer : MonoBehaviour
    {
        [FormerlySerializedAs("OrbitCamera")] public ExampleCharacterCamera _orbitCamera;
        [FormerlySerializedAs("CameraFollowPoint")] public Transform _cameraFollowPoint;
        [FormerlySerializedAs("Character")] public MyCharacterController _character;

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";
        
        private Vector3 _lookInputVector = Vector3.zero;
        
        private Vector2 _lookDelta;
        private Vector2 _moveInput;
        private PlayerInput _playerInput;

        private void Awake() {
            _playerInput = new PlayerInput();
            
            // _playerInput.CharacterControls.Look.performed += ctx => _lookDelta = ctx.ReadValue<Vector2>();
            // _playerInput.CharacterControls.Look.canceled += ctx => _lookDelta = ctx.ReadValue<Vector2>();
            
            // _playerInput.CharacterControls.Movement.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
            // _playerInput.CharacterControls.Movement.canceled += ctx => _moveInput = ctx.ReadValue<Vector2>();
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
            _orbitCamera.IgnoredColliders.Clear();
            _orbitCamera.IgnoredColliders.AddRange(_character.GetComponentsInChildren<Collider>());
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            HandleCharacterInput();
        }

        private void LateUpdate()
        {
            HandleCameraInput();
        }

        private void HandleCameraInput()
        {
            // Create the look input vector for the camera
            Vector3 lookInputVector = new Vector3(_lookDelta.x, _lookDelta.y, 0f);

            // Prevent moving the camera while the cursor isn't locked
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInputVector = Vector3.zero;
            }

            // Apply inputs to the camera
            _orbitCamera.UpdateWithInput(Time.deltaTime, 0f, lookInputVector);

            // Handle toggling zoom level
            if (Mouse.current.middleButton.wasPressedThisFrame)
            {
                _orbitCamera.TargetDistance = (_orbitCamera.TargetDistance == 0f) ? _orbitCamera.DefaultDistance : 0f;
            }
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct
            characterInputs.MoveAxisRight = _moveInput.x;
            characterInputs.MoveAxisForward = _moveInput.y;
            characterInputs.CameraRotation = _orbitCamera.Transform.rotation;
            characterInputs.JumpDown = Keyboard.current.spaceKey.wasPressedThisFrame;

            // Apply inputs to character
            _character.SetInputs(ref characterInputs);

            // Apply impulse
            if(Keyboard.current.qKey.wasPressedThisFrame)
            {
                _character._motor.ForceUnground(0.1f);
                _character.AddVelocity(Vector3.one * 10f);
            }
        }
    }
}