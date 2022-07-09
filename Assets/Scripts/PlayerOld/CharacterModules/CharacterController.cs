using System;
using UnityEngine;
using KinematicCharacterController;

namespace VHS {
    public class CharacterController : MonoBehaviour, ICharacterController {
        [SerializeField] private Vector3 _gravity = new(0f,-30f,0f);

        private KinematicCharacterMotor _motor;
        
        private CC_RollModule _rollModule;
        private CC_DashModule _dashModule;
        private CC_MeleeModule _meleeModule;
        private CC_ChargeModule _chargeModule;
        private CC_CrouchModule _crouchModule;
        private CC_MovementModule _movementModule;
        private CC_ClimbingLadderModule _climbingLadderModule;
        private CC_CombinedStatesModule _defaultMovementModule;
        private CC_CollisionFilteringModule _collisionFilteringModule;

        private CC_StateMachine _stateMachine;
        private CC_AnimatorController _animatorController;

        public KinematicCharacterMotor Motor => _motor;
        public Animator Animator => _animatorController.Animator;
        
        public CC_AnimatorController AC => _animatorController;
        public CC_StateMachine StateMachine => _stateMachine;
        public CC_CombinedStatesModule DefaultMovementModule => _defaultMovementModule;
        
        public CharacterInputs LastCharacterInputs { get; private set; }

        public Vector3 Gravity => _gravity;
        
        public Vector3 LookInput { get; set; }
        public Vector3 MoveInput { get; set; }
        public Vector3 LastNonZeroMoveInput { get; set; }

        public Action OnCrouchStart = delegate { };
        public Action OnCrouchEnd = delegate { };
        
        public Action OnLadderStart = delegate { };
        public Action OnLadderEnd = delegate { };
        
        public Action OnHeavyAttack = delegate { };
        public Action<int> OnLightAttack = delegate { };

        public Action OnRollStart = delegate { };
        public Action<float> OnRollUpdate = delegate { };
        public Action OnRollEnd = delegate { };
        
        private void Awake() {
            _motor = GetComponent<KinematicCharacterMotor>();
            _animatorController = GetComponent<CC_AnimatorController>();
            
            _rollModule = GetComponent<CC_RollModule>();
            _dashModule = GetComponent<CC_DashModule>();
            _meleeModule = GetComponent<CC_MeleeModule>();
            _crouchModule = GetComponent<CC_CrouchModule>();
            _chargeModule = GetComponent<CC_ChargeModule>();
            _movementModule = GetComponent<CC_MovementModule>();
            _climbingLadderModule = GetComponent<CC_ClimbingLadderModule>();
            _collisionFilteringModule = GetComponent<CC_CollisionFilteringModule>();

            _defaultMovementModule = gameObject.AddComponent<CC_CombinedStatesModule>();
            _defaultMovementModule.AddModules(_movementModule, _crouchModule, _dashModule, _meleeModule);

            _stateMachine = new CC_StateMachine(_defaultMovementModule);

            _motor.CharacterController = this;
        }

        public void SetInputs(ref CharacterInputs inputs) {
            LastCharacterInputs = inputs;
            _animatorController.SetInputs(inputs);
            
            if(inputs.RollPressed)
                StateMachine.SetState(_rollModule);
            
            if (inputs.ChargingDown && _stateMachine.CurrentState == _defaultMovementModule)
                StateMachine.SetState(_chargeModule);

            if (inputs.InteractPressed) {
                if(_stateMachine.CurrentState == _defaultMovementModule)    
                    _climbingLadderModule.HandlePotentialLadderAnchor();
                else 
                    _climbingLadderModule.HandlePotentialLadderDeAnchor();
            }
            
            _collisionFilteringModule.SetInputs(inputs);
            
            _stateMachine.CurrentState.SetInputs(inputs);
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime) =>
            _stateMachine.CurrentState.UpdateRotation(ref currentRotation, deltaTime);

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) =>
            _stateMachine.CurrentState.UpdateVelocity(ref currentVelocity, deltaTime);

        public void BeforeCharacterUpdate(float deltaTime) {
            _stateMachine.CurrentState.HandlePreCharacterUpdate(deltaTime);
        }

        public void AfterCharacterUpdate(float deltaTime) =>
            _stateMachine.CurrentState.HandlePostCharacterUpdate(deltaTime);

        public void PostGroundingUpdate(float deltaTime) {
            if (_motor.GroundingStatus.IsStableOnGround && !_motor.LastGroundingStatus.IsStableOnGround)
                OnStableGroundRegained();
            else if (!_motor.GroundingStatus.IsStableOnGround && _motor.LastGroundingStatus.IsStableOnGround)
                OnStableGroundLost();
        }

        private void OnStableGroundRegained() => Debug.Log("Regained Ground");
        private void OnStableGroundLost() => Debug.Log("Lost Ground");

        public bool IsColliderValidForCollisions(Collider coll) {
            bool ignoreFromCollisionFiltering = _collisionFilteringModule.IsColliderValidForCollisions(coll);

            if (!ignoreFromCollisionFiltering)
                return false;

            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport) { }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) => _stateMachine.CurrentState.OnMovementHit(hitCollider, hitNormal, hitPoint, ref hitStabilityReport);

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport) => _stateMachine.CurrentState.ProcessHitStabilityReport(hitCollider, hitNormal, hitPoint, atCharacterPosition, atCharacterRotation, ref hitStabilityReport);

        public void OnDiscreteCollisionDetected(Collider hitCollider) { }
    }
}