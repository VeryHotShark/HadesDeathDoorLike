using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VHS {
    public class CharacterController : ChildBehaviour<Player>, ICharacterController, IUpdateListener {
        [ShowInInspector] public CharacterModule CurrentModule => _stateMachine?.CurrentState;
        
        // Remove this if Unity fixes Simultanoeus Release Button
        [SerializeField] private Timer _inputDirectionChangeTimer = new Timer(0.1f);

        private Vector3 _lastMoveInput;
        private Vector3 _internalVelocityAdd;

        private CharacterRoll _rollModule;
        private CharacterParry _parryModule;
        private CharacterMovement _movementModule;
        private CharacterMeleeCombat _meleeCombatModule;
        private CharacterRangeCombat _rangeCombatModule;
        private CharacterFallingMovement _fallingMovementModule;

        private KinematicCharacterMotor _motor;
        private StateMachine<CharacterModule> _stateMachine;

        public CharacterParry ParryModule => _parryModule;
        public KinematicCharacterMotor Motor => _motor;
        public Player ControlledCharacter => Parent;

        public Vector3 LookInput { get; set; }
        public Vector3 MoveInput { get; set; }
        public Vector3 LastNonZeroMoveInput { get; set; }
        public CharacterInputs LastCharacterInputs { get; private set; }

        private void Awake() {
            _motor = GetComponent<KinematicCharacterMotor>();
            _motor.CharacterController = this;

            _rollModule = GetComponent<CharacterRoll>();
            _parryModule = GetComponent<CharacterParry>();
            _movementModule = GetComponent<CharacterMovement>();
            _meleeCombatModule = GetComponent<CharacterMeleeCombat>();
            _rangeCombatModule = GetComponent<CharacterRangeCombat>();
            _fallingMovementModule = GetComponent<CharacterFallingMovement>();

            _stateMachine = new StateMachine<CharacterModule>(_movementModule);
        }

        private void OnEnable() => UpdateManager.AddUpdateListener(this);
        private void OnDisable() => UpdateManager.RemoveUpdateListener(this);
        public void OnUpdate(float deltaTime) => _stateMachine.Tick(deltaTime);

        public void SetInputs(ref CharacterInputs inputs) {
            LookInput = transform.position.DirectionTo(inputs.CursorPosition).Flatten();

            Vector3 rawInput = new Vector3(inputs.MoveAxisRight, 0.0f, inputs.MoveAxisForward);
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

            if (Motor.GroundingStatus.IsStableOnGround) {
                if (inputs.RollDown)
                    _stateMachine.SetState(_rollModule);
                else if(inputs.ParryDown)
                    _stateMachine.SetState(_parryModule);
                else if (inputs.SecondaryAttackPressed)
                    _stateMachine.SetState(_rangeCombatModule);
                else if(inputs.PrimaryAttackPressed && !_meleeCombatModule.IsOnCooldown)
                    _stateMachine.SetState(_meleeCombatModule);
            }

            _stateMachine.CurrentState.SetInputs(inputs);

            LastCharacterInputs = inputs;
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            _stateMachine.CurrentState.UpdateVelocity(ref currentVelocity, deltaTime);

            if (_internalVelocityAdd.sqrMagnitude > 0f) {
                currentVelocity += _internalVelocityAdd;
                _internalVelocityAdd = Vector3.zero;
            }
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
            _stateMachine.CurrentState.UpdateRotation(ref currentRotation, deltaTime);
        }

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

        public void TransitionToDefaultState() => _stateMachine.TransitionToDefaultState();
        public void TransitionToLastState() => _stateMachine.TransitionToLastState();
    }
}