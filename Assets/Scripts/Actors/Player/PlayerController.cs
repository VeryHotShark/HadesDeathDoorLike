using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VHS {
    public struct CharacterInputs {
        public float MoveAxisForward;
        public float MoveAxisRight;
        public bool LockTarget;
        public bool RollDown;
        public bool PrimaryAttackDown;
        public bool PrimaryAttackUp;
        public bool SecondaryAttackDown;
        public bool SecondaryAttackUp;
        public Quaternion CameraRotation;
        public Vector3 CursorPosition;
        public Quaternion CursorRotation;
    }

    public class PlayerController : BaseBehaviour, IUpdateListener, ILateUpdateListener {
        [SerializeField] private CameraController _camera;
        [SerializeField] private CharacterController _character;

        private Vector3 _lookVector;
        private Vector2 _moveInput;
        private Vector2 _mousePos;

        private bool _primaryAttackDown;
        private bool _secondaryAttackDown;

        private PlayerInput _input;

        public CharacterController Character => _character;
        public Player Player => _character.ControlledCharacter;

        private void Awake() => SetInputs();

        protected override void Enable() {
            _input.Enable();
            UpdateManager.AddUpdateListener(this);
            UpdateManager.AddLateUpdateListener(this);
        }

        protected override void Disable() {
            _input.Disable();
            UpdateManager.RemoveUpdateListener(this);
            UpdateManager.RemoveLateUpdateListener(this);
        }

        private void SetInputs() {
            _input = new PlayerInput();

            _input.CharacterControls.MousePosition.performed += ctx => _mousePos = ctx.ReadValue<Vector2>();
            _input.CharacterControls.MousePosition.canceled += ctx => _mousePos = ctx.ReadValue<Vector2>();

            _input.CharacterControls.Movement.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
            _input.CharacterControls.Movement.canceled += ctx => _moveInput = ctx.ReadValue<Vector2>();

            _input.CharacterControls.PrimaryAttack.performed += ctx => _primaryAttackDown = ctx.ReadValueAsButton();
            _input.CharacterControls.PrimaryAttack.canceled += ctx => _primaryAttackDown = ctx.ReadValueAsButton();

            _input.CharacterControls.SecondaryAttack.performed += ctx => _secondaryAttackDown = ctx.ReadValueAsButton();
            _input.CharacterControls.SecondaryAttack.canceled += ctx => _secondaryAttackDown = ctx.ReadValueAsButton();
        }
        
        public void OnUpdate(float deltaTime) => HandleCharacterInput();

        public void OnLateUpdate(float deltaTime) {
            if(!_character)
                return;
            
            _camera.SetCursorPos(_mousePos);
        }

        private void HandleCharacterInput() {
            if(!_character)
                return;
                
            CharacterInputs characterInputs = new CharacterInputs();

            characterInputs.LockTarget = Keyboard.current.shiftKey.wasPressedThisFrame;
            characterInputs.RollDown = _input.CharacterControls.Roll.triggered;
            characterInputs.PrimaryAttackDown = _input.CharacterControls.PrimaryAttack.triggered;
            characterInputs.SecondaryAttackUp = _input.CharacterControls.SecondaryAttack.WasReleasedThisFrame();
            characterInputs.SecondaryAttackDown = _secondaryAttackDown;
            characterInputs.MoveAxisRight = _moveInput.x;
            characterInputs.MoveAxisForward = _moveInput.y;
            characterInputs.CameraRotation = _camera.transform.rotation;
            characterInputs.CursorPosition = _camera.CursorTransform.position;
            characterInputs.CursorRotation = _camera.CursorTransform.rotation;

            _character.SetInputs(ref characterInputs);
        }
    }
}