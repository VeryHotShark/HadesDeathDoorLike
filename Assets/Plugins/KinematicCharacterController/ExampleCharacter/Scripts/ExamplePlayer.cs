using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace KinematicCharacterController.Examples
{
    public class ExamplePlayer : MonoBehaviour
    {
        [FormerlySerializedAs("Character")] public ExampleCharacterController _character;
        [FormerlySerializedAs("CharacterCamera")] public ExampleCharacterCamera _characterCamera;

        private Vector2 _lookDelta;
        private Vector2 _moveInput;

        private PlayerInput _playerInput;

        private void Awake() {
            _playerInput = new PlayerInput();
            
            // _playerInput.CharacterControls.Movement.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
            // _playerInput.CharacterControls.Movement.canceled += ctx => _moveInput = ctx.ReadValue<Vector2>();
            
            // _playerInput.CharacterControls.Look.performed += ctx => _lookDelta = ctx.ReadValue<Vector2>();
            // _playerInput.CharacterControls.Look.canceled += ctx => _lookDelta = ctx.ReadValue<Vector2>();
        }

        private void OnEnable() {
            // _playerInput.Enable();
        }

        private void OnDisable() {
            // _playerInput.Disable();
        }

        private void Start() {
            Cursor.lockState = CursorLockMode.Locked;

            // Tell camera to follow transform
            _characterCamera.SetFollowTransform(_character.CameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            _characterCamera.IgnoredColliders.Clear();
            _characterCamera.IgnoredColliders.AddRange(_character.GetComponentsInChildren<Collider>());
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
            // Handle rotating the camera along with physics movers
            if (_characterCamera.RotateWithPhysicsMover && _character.Motor.AttachedRigidbody != null)
            {
                _characterCamera.PlanarDirection = _character.Motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation * _characterCamera.PlanarDirection;
                _characterCamera.PlanarDirection = Vector3.ProjectOnPlane(_characterCamera.PlanarDirection, _character.Motor.CharacterUp).normalized;
            }

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

            // Input for zooming the camera (disabled in WebGL because it can cause problems)
            // float scrollInput = -Input.GetAxis(MouseScrollInput);
#if UNITY_WEBGL
        // scrollInput = 0f;
#endif

            // Apply inputs to the camera
            _characterCamera.UpdateWithInput(Time.deltaTime, 0f, lookInputVector);

            // Handle toggling zoom level
            if (Mouse.current.middleButton.wasPressedThisFrame)
            {
                _characterCamera.TargetDistance = (_characterCamera.TargetDistance == 0f) ? _characterCamera.DefaultDistance : 0f;
            }
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct
            characterInputs.MoveAxisForward = _moveInput.y;
            characterInputs.MoveAxisRight = _moveInput.x;
            characterInputs.CameraRotation = _characterCamera.Transform.rotation;
            // characterInputs.JumpDown = _playerInput.CharacterControls.Roll.triggered;
            characterInputs.CrouchDown = Keyboard.current.cKey.wasPressedThisFrame;
            characterInputs.CrouchUp = Keyboard.current.cKey.wasReleasedThisFrame;

            // Apply inputs to character
            _character.SetInputs(ref characterInputs);
        }
    }
}