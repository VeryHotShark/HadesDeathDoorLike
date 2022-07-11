using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VHS {
    public class CharacterController : MonoBehaviour, ICharacterController, IUpdateListener {
        [ShowInInspector] public CharacterModule CurrentModule => _stateMachine?.CurrentState;

        private StateMachine<CharacterModule> _stateMachine;
        private Vector3 _internalVelocityAdd;

        private CharacterRoll _rollModule;
        private CharacterMovement _movementModule;
        private CharacterMeleeCombat _meleeCombatModule;
        private CharacterRangeCombat _rangeCombatModule;

        private KinematicCharacterMotor _motor;
        public KinematicCharacterMotor Motor => _motor;

        public Vector3 LookInput { get; set; }
        public Vector3 MoveInput { get; set; }
        public Vector3 LastNonZeroMoveInput { get; set; }
        public CharacterInputs LastCharacterInputs { get; private set; }

        private void Awake() {
            _motor = GetComponent<KinematicCharacterMotor>();
            _motor.CharacterController = this;

            _rollModule = GetComponent<CharacterRoll>();
            _movementModule = GetComponent<CharacterMovement>();
            _meleeCombatModule = GetComponent<CharacterMeleeCombat>();
            _rangeCombatModule = GetComponent<CharacterRangeCombat>();


            _stateMachine = new StateMachine<CharacterModule>(_movementModule);

            bool CanTryAttack() => LastCharacterInputs.AttackDown && !_meleeCombatModule.IsOnCooldown;

            // Pomyśl czy nie rodzielić na Osobno SetState i osobno transition by
            // Set Inputs przedstawiać na dany state jak eventy,
            // a AddTransitionTo jako update na czas działania modułu
            _stateMachine.AddTransitionTo(_rollModule, () => LastCharacterInputs.RollDown || _rollModule.DuringRoll);
            _stateMachine.AddTransitionTo(_meleeCombatModule,
                () => CanTryAttack() || _meleeCombatModule.IsDuringAttack);
            _stateMachine.AddTransitionTo(_rangeCombatModule, () => LastCharacterInputs.AimDown);
            _stateMachine.AddTransitionTo(_movementModule, () => true);
        }

        private void OnEnable() => UpdateManager.AddUpdateListener(this);
        private void OnDisable() => UpdateManager.RemoveUpdateListener(this);
        public void OnUpdate(float deltaTime) => _stateMachine.Tick(deltaTime);

        public void SetInputs(ref CharacterInputs inputs) {
            LookInput = transform.position.DirectionTo(inputs.CursorPosition);

            _stateMachine.CurrentState.SetInputs(inputs);

            LastCharacterInputs = inputs;
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            _stateMachine.CurrentState.UpdateVelocity(ref currentVelocity, deltaTime);

            if (_meleeCombatModule.AttackTimer.IsActive)
                _movementModule.UpdateVelocity(ref currentVelocity, deltaTime);

            if (_internalVelocityAdd.sqrMagnitude > 0f) {
                currentVelocity += _internalVelocityAdd;
                _internalVelocityAdd = Vector3.zero;
            }
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime) =>
            _stateMachine.CurrentState.UpdateRotation(ref currentRotation, deltaTime);

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

        private void OnStableGroundLost() => _stateMachine.CurrentState.OnStableGroundLost();
        private void OnStableGroundRegained() => _stateMachine.CurrentState.OnStableGroundRegained();

        public void AddVelocity(Vector3 velocity) => _internalVelocityAdd += velocity;
    }
}