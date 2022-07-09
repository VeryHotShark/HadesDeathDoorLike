using System;
using System.Linq;
using KinematicCharacterController.Examples;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VHS {
    public struct CharacterInputs {
        public bool CrouchUp;
        public bool CrouchDown;
        public bool ChargingDown;
        public bool AttackDown;
        public bool DashPressed;
        public bool RollPressed;
        public bool InteractPressed;
        public bool IgnoreCollisionsPressed;
        public float MoveAxisForward;
        public float MoveAxisRight;
        public Quaternion CameraRotation;
        public Quaternion CursorRotation;
    }
    
    public class Player : MonoBehaviour {
        [SerializeField] private bool _lockCursor;
        [SerializeField] private CameraController _camera;
        [SerializeField] private CharacterController _character;

        private Vector3 _lookVector;
        private Vector2 _moveInput;
        private Vector2 _mousePos;
        private bool _attackDown;

        private PlayerInput _input;

        private void Awake() {
            SetInputs();
            
            if(_lockCursor)
                Cursor.lockState = CursorLockMode.Locked;
        }

        private void SetInputs() {
            _input = new PlayerInput();

            _input.CharacterControls.MousePosition.performed += ctx => _mousePos = ctx.ReadValue<Vector2>();
            _input.CharacterControls.MousePosition.canceled += ctx => _mousePos = ctx.ReadValue<Vector2>();
            
            _input.CharacterControls.Movement.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
            _input.CharacterControls.Movement.canceled += ctx => _moveInput = ctx.ReadValue<Vector2>();

            _input.CharacterControls.Attack.performed += ctx => _attackDown = ctx.ReadValueAsButton();
            _input.CharacterControls.Attack.canceled += ctx => _attackDown = ctx.ReadValueAsButton();
        }

        private void OnEnable() => _input.Enable();
        private void OnDisable() => _input.Disable();

        private void Update() {
            if (_lockCursor) {
                if (Cursor.lockState != CursorLockMode.Locked) {
                    if(Mouse.current.leftButton.wasPressedThisFrame)
                        Cursor.lockState = CursorLockMode.Locked;
                }
            }

            HandleCharacterInput();
        }

        private void LateUpdate() {
            _camera.SetCursorPos(_mousePos);
        }

        private void HandleCharacterInput() {
            CharacterInputs characterInputs = new CharacterInputs();

            characterInputs.AttackDown = _attackDown;
            characterInputs.MoveAxisRight = _moveInput.x;
            characterInputs.MoveAxisForward = _moveInput.y;
            characterInputs.CursorRotation = _camera.RotationToCursor;
            characterInputs.CameraRotation = _camera.transform.rotation;
            characterInputs.RollPressed = _input.CharacterControls.Roll.triggered;
            characterInputs.DashPressed = Keyboard.current.vKey.wasPressedThisFrame;
            characterInputs.ChargingDown = Keyboard.current.cKey.wasPressedThisFrame;
            characterInputs.InteractPressed = Keyboard.current.eKey.wasPressedThisFrame;
            characterInputs.IgnoreCollisionsPressed = Keyboard.current.qKey.wasPressedThisFrame;
            characterInputs.CrouchDown = _input.CharacterControls.Crouch.WasPressedThisFrame();
            characterInputs.CrouchUp = _input.CharacterControls.Crouch.WasReleasedThisFrame();
            
            _character.SetInputs(ref characterInputs);
        }
    }
}
