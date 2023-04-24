using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VHS {
    public class CharacterController : ChildBehaviour<Player>, ICharacterController, IUpdateListener {
        // Remove this if Unity fixes Simultanoeus Release Button
        [SerializeField] private Timer _inputDirectionChangeTimer = new Timer(0.1f);

        private Vector3 _lastMoveInput;
        private Vector3 _internalVelocityAdd;

        private CharacterRoll _rollModule;
        private CharacterMovement _movementModule;
        private CharacterMeleeCombat _meleeCombatModule;
        private CharacterRangeCombat _rangeCombatModule;
        private CharacterSkillCombat _skillCombatModule;
        private CharacterUltimateCombat _ultimateCombatModule;
        private CharacterFallingMovement _fallingMovementModule;

        private KinematicCharacterMotor _motor;
        private StateMachine<CharacterModule> _stateMachine;


        public CharacterRoll RollModule => _rollModule;
        public CharacterMovement MovementModule => _movementModule;
        public CharacterRangeCombat RangeCombat => _rangeCombatModule;
        public CharacterMeleeCombat MeleeCombat => _meleeCombatModule;
        
        public CharacterModule LastState => _stateMachine.LastState;
        public CharacterModule CurrentState => _stateMachine.CurrentState;
        public StateMachine<CharacterModule> StateMachine => _stateMachine;
        public KinematicCharacterMotor Motor => _motor;
        public Player Player => Parent;

        public Vector3 LookInput { get; set; }
        public Vector3 MoveInput { get; set; }
        public Vector3 LastNonZeroMoveInput { get; set; }
        public CharacterInputs LastCharacterInputs { get; private set; }
        public CharacterInputs CurrentCharacterInputs { get; private set; }


        private void Awake() {
            _motor = GetComponent<KinematicCharacterMotor>();
            _motor.CharacterController = this;

            _rollModule = GetComponent<CharacterRoll>();
            _movementModule = GetComponent<CharacterMovement>();
            _meleeCombatModule = GetComponent<CharacterMeleeCombat>();
            _rangeCombatModule = GetComponent<CharacterRangeCombat>();
            _skillCombatModule = GetComponent<CharacterSkillCombat>();
            _ultimateCombatModule = GetComponent<CharacterUltimateCombat>();
            _fallingMovementModule = GetComponent<CharacterFallingMovement>();

            _stateMachine = new StateMachine<CharacterModule>(_movementModule);
            _stateMachine.SetState(_fallingMovementModule);
            _stateMachine.OnStateChanged += Parent.OnCharacterStateChanged;
        }

        private void OnEnable() => UpdateManager.AddUpdateListener(this);
        private void OnDisable() => UpdateManager.RemoveUpdateListener(this);
        public void OnUpdate(float deltaTime) => _stateMachine.Tick(deltaTime);

        public void SetInputs(ref CharacterInputs inputs) {
            UpdateInput(inputs);
            
            CurrentCharacterInputs = inputs;

            if (Motor.GroundingStatus.IsStableOnGround) {
                if (inputs.Ultimate.Pressed)
                    _stateMachine.SetState(_ultimateCombatModule);
                else if (inputs.Roll.Pressed)
                    _stateMachine.SetState(_rollModule);
                else if (inputs.Range.Pressed)
                    _stateMachine.SetState(_rangeCombatModule);
                else if (inputs.Melee.Pressed)
                    _stateMachine.SetState(_meleeCombatModule);
                else if (inputs.SkillPrimary.Pressed &&  _skillCombatModule.CanCastPrimary()) 
                    _stateMachine.SetState(_skillCombatModule);
                else if (inputs.SkillSecondary.Pressed && _skillCombatModule.CanCastSecondary()) 
                    _stateMachine.SetState(_skillCombatModule);
            }

            _stateMachine.CurrentState.SetInputs(inputs);

            LastCharacterInputs = inputs;
        }

        private void UpdateInput(CharacterInputs inputs) {
            LookInput = transform.position.DirectionTo(inputs.CursorPosition).Flatten();

            Vector3 rawInput = new Vector3(inputs.MoveAxis.x, 0.0f, inputs.MoveAxis.y);
            Vector3 clampedMoveInput = Vector3.ClampMagnitude(rawInput, 1.0f);

            if (_lastMoveInput != clampedMoveInput) {
                _inputDirectionChangeTimer.Start();
                _lastMoveInput = clampedMoveInput;
            }

            if (!_inputDirectionChangeTimer.IsActive) {
                Vector3 cameraPlanarDirection =
                    Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, Motor.CharacterUp).normalized;

                Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Motor.CharacterUp);
                MoveInput = cameraPlanarRotation * clampedMoveInput;

                if (clampedMoveInput != Vector3.zero)
                    LastNonZeroMoveInput = MoveInput;
            }
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            _stateMachine.CurrentState.UpdateVelocity(ref currentVelocity, deltaTime);

            if (_internalVelocityAdd.sqrMagnitude > 0f) {
                currentVelocity += _internalVelocityAdd;
                _internalVelocityAdd = Vector3.zero;
            }
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime) 
            => _stateMachine.CurrentState.UpdateRotation(ref currentRotation, deltaTime);

        public void BeforeCharacterUpdate(float deltaTime) =>
            _stateMachine.CurrentState.HandlePreCharacterUpdate(deltaTime);

        public void AfterCharacterUpdate(float deltaTime) =>
            _stateMachine.CurrentState.HandlePostCharacterUpdate(deltaTime);

        public bool IsColliderValidForCollisions(Collider coll) => true;

        public void OnDiscreteCollisionDetected(Collider hitCollider) { }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport) { }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport) =>
            _stateMachine.CurrentState.OnMovementHit(hitCollider, hitNormal, hitPoint, ref hitStabilityReport);

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            Vector3 atCharacterPosition,
            Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport) =>
            _stateMachine.CurrentState.ProcessHitStabilityReport(hitCollider, hitNormal, hitPoint, atCharacterPosition,
                atCharacterRotation, ref hitStabilityReport);

        public void PostGroundingUpdate(float deltaTime) {
            if (_motor.GroundingStatus.IsStableOnGround && !_motor.LastGroundingStatus.IsStableOnGround)
                OnStableGroundRegained();
            else if (!_motor.GroundingStatus.IsStableOnGround && _motor.LastGroundingStatus.IsStableOnGround)
                OnStableGroundLost();
        }

        private void OnStableGroundLost() {
            _stateMachine.SetState(_fallingMovementModule);
            _stateMachine.CurrentState.OnStableGroundLost();
        }

        private void OnStableGroundRegained() {
            _stateMachine.SetState(_movementModule);
            _stateMachine.CurrentState.OnStableGroundRegained();
        }

        public void AddVelocity(Vector3 velocity) => _internalVelocityAdd += velocity;

        public void TransitionToDefaultState(bool force = false) => _stateMachine.TransitionToDefaultState(force);
        public void TransitionToLastState(bool force = false) => _stateMachine.TransitionToLastState(force);

        public void SetLastDirectionToForward() => LastNonZeroMoveInput = Motor.CharacterForward;
    }
}