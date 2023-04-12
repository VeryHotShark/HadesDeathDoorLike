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
        public Vector2 MoveAxis;

        public KeyInput Roll;
        public KeyInput Melee;
        public KeyInput Skill;
        public KeyInput Range;
        public KeyInput Ultimate;

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

        public Vector3 CharacterDirectionToCursor =>
            Character.Motor.TransientPosition.DirectionTo(Camera.CursorTransform.position).Flatten();
        
        public CameraController Camera => _camera;
        public CharacterController Character => _character;
        public Player Player => _character.Player;

        private void Awake() => SetInputs();
        private void OnDestroy() => _input.Dispose();

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
        }
        
        public void OnUpdate(float deltaTime) => HandleCharacterInput();

        public void OnLateUpdate(float deltaTime) {
            if(!_character || GameManager.IsPaused)
                return;
            
            _camera.SetCursorPos(_mousePos);
        }

        private KeyInput SetupKeyInput(InputAction inputAction) {
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
            
            _characterInputs.Roll = SetupKeyInput(_input.CharacterControls.Roll);
            _characterInputs.Melee = SetupKeyInput(_input.CharacterControls.Melee);
            _characterInputs.Skill = SetupKeyInput(_input.CharacterControls.Skill);
            _characterInputs.Range = SetupKeyInput(_input.CharacterControls.Range);
            _characterInputs.Ultimate = SetupKeyInput(_input.CharacterControls.Ultimate);
            
            _characterInputs.MoveAxis = _moveInput;
            
            _characterInputs.CameraRotation = _camera.transform.rotation;
            _characterInputs.CursorPosition = _camera.CursorTransform.position;
            _characterInputs.CursorRotation = _camera.CursorTransform.rotation;

            _character.SetInputs(ref _characterInputs);
            
            if(Keyboard.current.fKey.wasReleasedThisFrame)
                Player.HandleInteract();
        }
    }
}
