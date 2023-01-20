using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VHS {
    /// <summary>
    /// Consider Wheter this is needed or we can Just Pass PlayerInput
    /// </summary>
    public struct CharacterInputs {
        public float MoveAxisForward;
        public float MoveAxisRight;
        
        public bool LockTarget;
        public bool RollDown;
        public bool ParryDown;

        public bool PrimaryAttackDown;
        public bool PrimaryAttackPressed;
        public bool PrimaryAttackPerformed;
        public bool PrimaryAttackReleased;
        
        public bool SecondaryAttackPressed;
        public bool SecondaryAttackPerformed;
        public bool SecondaryAttackReleased;

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

        private PlayerInput _input;
        private CharacterInputs _characterInputs;

        public CameraController Camera => _camera;
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
            
            _input.CharacterControls.PrimaryAttack.started += ctx => {
                // Log("Started " + ctx.ReadValueAsButton());
            };
            
            _input.CharacterControls.PrimaryAttack.performed += ctx => {
                // Log("Performed " + ctx.ReadValueAsButton());
            };
            
            _input.CharacterControls.PrimaryAttack.canceled += ctx => {
                // Log("Canceled " + ctx.ReadValueAsButton());
            };

            _input.CharacterControls.SecondaryAttack.started += ctx => {
                
            };
            
            _input.CharacterControls.SecondaryAttack.performed += ctx => {
                
            };
            
            _input.CharacterControls.SecondaryAttack.canceled += ctx => {
                
            };
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

            _characterInputs = new CharacterInputs();
            
            _characterInputs.LockTarget = Keyboard.current.shiftKey.wasPressedThisFrame;
            _characterInputs.RollDown = _input.CharacterControls.Roll.triggered;
            _characterInputs.ParryDown = _input.CharacterControls.Parry.triggered;

            _characterInputs.PrimaryAttackDown = _input.CharacterControls.PrimaryAttack.IsPressed();
            _characterInputs.PrimaryAttackPressed = _input.CharacterControls.PrimaryAttack.WasPressedThisFrame();
            _characterInputs.PrimaryAttackPerformed = _input.CharacterControls.PrimaryAttack.WasPerformedThisFrame();
            _characterInputs.PrimaryAttackReleased = _input.CharacterControls.PrimaryAttack.WasReleasedThisFrame();
            
            _characterInputs.SecondaryAttackPressed = _input.CharacterControls.SecondaryAttack.WasPressedThisFrame();
            _characterInputs.SecondaryAttackPerformed = _input.CharacterControls.SecondaryAttack.WasPerformedThisFrame();
            _characterInputs.SecondaryAttackReleased = _input.CharacterControls.SecondaryAttack.WasReleasedThisFrame();
            
            _characterInputs.MoveAxisRight = _moveInput.x;
            _characterInputs.MoveAxisForward = _moveInput.y;
            _characterInputs.CameraRotation = _camera.transform.rotation;
            _characterInputs.CursorPosition = _camera.CursorTransform.position;
            _characterInputs.CursorRotation = _camera.CursorTransform.rotation;

            _character.SetInputs(ref _characterInputs);
            
            if(Keyboard.current.fKey.wasReleasedThisFrame)
                Player.HandleInteract();

            // ResetInputs();
        }

        private void ResetInputs() {
            // _characterInputs.PrimaryAttackUp = false;
            // _characterInputs.PrimaryAttackHeld = false;
            _characterInputs.SecondaryAttackReleased = false;
            _characterInputs.SecondaryAttackPerformed = false;
        }
    }
}
