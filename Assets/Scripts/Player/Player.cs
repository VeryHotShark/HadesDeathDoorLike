using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VHS {
    public struct CharacterInputs {
        public float MoveAxisForward;
        public float MoveAxisRight;
        public bool SwitchAim;
        public bool RollDown;
        public bool AimDown;
        public bool AimUp;
        public bool AttackDown;
        public bool AttackUp;
        public Quaternion CameraRotation;
        public Vector3 CursorPosition;
        public Quaternion CursorRotation;
    }

    public class Player : MonoBehaviour {
        [SerializeField] private CameraController _camera;
        [SerializeField] private CharacterController _character;

        private Vector3 _lookVector;
        private Vector2 _moveInput;
        private Vector2 _mousePos;

        private bool _attackDown;
        private bool _aimDown;

        private PlayerInput _input;

        private void Awake() => SetInputs();

        private void OnEnable() => _input.Enable();
        private void OnDisable() => _input.Disable();

        private void SetInputs() {
            _input = new PlayerInput();

            _input.CharacterControls.MousePosition.performed += ctx => _mousePos = ctx.ReadValue<Vector2>();
            _input.CharacterControls.MousePosition.canceled += ctx => _mousePos = ctx.ReadValue<Vector2>();

            _input.CharacterControls.Movement.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
            _input.CharacterControls.Movement.canceled += ctx => _moveInput = ctx.ReadValue<Vector2>();

            _input.CharacterControls.Attack.performed += ctx => _attackDown = ctx.ReadValueAsButton();
            _input.CharacterControls.Attack.canceled += ctx => _attackDown = ctx.ReadValueAsButton();

            _input.CharacterControls.Aim.performed += ctx => _aimDown = ctx.ReadValueAsButton();
            _input.CharacterControls.Aim.canceled += ctx => _aimDown = ctx.ReadValueAsButton();
        }

        private void Update() => HandleCharacterInput();
        private void LateUpdate() => _camera.SetCursorPos(_mousePos);


        private void HandleCharacterInput() {
            CharacterInputs characterInputs = new CharacterInputs();

            characterInputs.AimDown = _aimDown;
            characterInputs.SwitchAim = Keyboard.current.shiftKey.wasPressedThisFrame;
            characterInputs.RollDown = _input.CharacterControls.Roll.triggered;
            characterInputs.AttackDown = _input.CharacterControls.Attack.triggered;
            characterInputs.MoveAxisRight = _moveInput.x;
            characterInputs.MoveAxisForward = _moveInput.y;
            characterInputs.CameraRotation = _camera.transform.rotation;
            characterInputs.CursorPosition = _camera.CursorTransform.position;
            characterInputs.CursorRotation = _camera.CursorTransform.rotation;

            _character.SetInputs(ref characterInputs);
        }
    }
}