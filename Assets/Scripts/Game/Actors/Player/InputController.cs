using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Key = Animancer.Key;

namespace VHS {
    public struct KeyInput {
        public bool Held;
        public bool Pressed;
        public bool Performed;
        public bool Released;
    }
    
    /// <summary>
    /// Consider Wheter this is needed or we can Just Pass PlayerInput
    /// </summary>
    public struct CharacterInputs {
        public Vector3 MousePos;
        public Vector2 MoveAxis;
        public Vector2 LookAxis;

        public KeyInput Roll;
        public KeyInput Melee;
        public KeyInput SkillPrimary;
        public KeyInput SkillSecondary;
        public KeyInput Range;
        public KeyInput Ultimate;
        public KeyInput Interact; 

        public Quaternion CameraRotation;
    }

    public class InputController : BaseBehaviour, IUpdateListener, ILateUpdateListener {
        [SerializeField] private CameraController _camera;
        [SerializeField] private CharacterController _character;

        private bool _isGamepad;
        
        private Vector2 _lookInput;
        private Vector2 _moveInput;
        private Vector2 _mousePos;

        private PlayerInput _input;
        private CharacterInputs _characterInputs;

        public bool IsGamepad => _isGamepad;
        public CharacterInputs CharacterInputs => _characterInputs;
        public CameraController Camera => _camera;
        public CharacterController Character => _character;
        public Player Player => _character.Player;

        private void Awake() => SetInputs();
        private void OnDestroy() => _input.Dispose();
 

        protected override void Enable() {
            _input.Enable();
            UpdateManager.AddUpdateListener(this);
            UpdateManager.AddLateUpdateListener(this);
            InputSystem.onActionChange += InputActionChangeCallback;
        }

        protected override void Disable() {
            _input.Disable();
            UpdateManager.RemoveUpdateListener(this);
            UpdateManager.RemoveLateUpdateListener(this);
            InputSystem.onActionChange -= InputActionChangeCallback;
        }

        private void SetInputs() {
            _input = new PlayerInput();

            _input.CharacterControls.MousePosition.performed += ctx => _mousePos = ctx.ReadValue<Vector2>();
            _input.CharacterControls.MousePosition.canceled += ctx => _mousePos = ctx.ReadValue<Vector2>();
            
            _input.CharacterControls.Look.performed += ctx => _lookInput = ctx.ReadValue<Vector2>();
            _input.CharacterControls.Look.canceled += ctx => _lookInput = ctx.ReadValue<Vector2>();

            _input.CharacterControls.Movement.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
            _input.CharacterControls.Movement.canceled += ctx => _moveInput = ctx.ReadValue<Vector2>();
        }
        
        
        private void InputActionChangeCallback(object inputAction, InputActionChange change)
        {
            if(!_character)
                return;
            
            if (change == InputActionChange.ActionPerformed)
            {
                InputAction receivedInputAction = (InputAction) inputAction;
                InputDevice lastDevice = receivedInputAction.activeControl.device;
                _isGamepad = !(lastDevice.name.Equals("Keyboard") || lastDevice.name.Equals("Mouse"));
                _camera.PlayerCursor.SetVisible(!_isGamepad);
                _character.SetIndicatorVisible(_isGamepad);
                // Debug.Log("GAMEPAD:" + _isGamepad);
            }
        }
        
        public void OnUpdate(float deltaTime) => HandleCharacterInput();

        public void OnLateUpdate(float deltaTime) {
            if(!_character || GameManager.IsPaused)
                return;

            if (_isGamepad)
                _character.UpdateAimIndicator();
            else
                _camera.SetCursorPos(_mousePos);
        }

        private KeyInput UpdateKeyInput(InputAction inputAction) {
            // Consider Caching
            KeyInput keyInput = new KeyInput() { 
                Held = inputAction.IsPressed(),
                Pressed =  inputAction.WasPressedThisFrame(),
                Released = inputAction.WasReleasedThisFrame(),
                Performed = inputAction.WasPerformedThisFrame()
            };

            return keyInput;
        }

        private void HandleCharacterInput() {
            if(!_character || GameManager.IsPaused)
                return;

            _characterInputs = new CharacterInputs();

            _characterInputs.Roll = UpdateKeyInput(_input.CharacterControls.Roll);
            _characterInputs.Melee = UpdateKeyInput(_input.CharacterControls.Melee);
            _characterInputs.SkillPrimary = UpdateKeyInput(_input.CharacterControls.SkillPrimary);
            _characterInputs.SkillSecondary = UpdateKeyInput(_input.CharacterControls.SkillSecondary);
            _characterInputs.Range = UpdateKeyInput(_input.CharacterControls.Range);
            _characterInputs.Ultimate = UpdateKeyInput(_input.CharacterControls.Ultimate);
            _characterInputs.Interact = UpdateKeyInput(_input.CharacterControls.Interact);
            
            _characterInputs.MoveAxis = _moveInput;
            _characterInputs.LookAxis = _lookInput;
            
            _characterInputs.MousePos = _camera.CursorTransform.position;
            _characterInputs.CameraRotation = _camera.transform.rotation;

            _character.SetInputs(ref _characterInputs);
            
            if(_characterInputs.Interact.Released)
                Player.HandleInteract();
        }
    }
}
