using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

namespace VHS {
    public class CC_ChargeModule : OldCharacterControllerModule {
        [SerializeField] private float _chargeSpeed = 15f;
        [SerializeField] private float _stoppedTime = 1f;
        [SerializeField] private float _maxChargeTime = 1.5f;

        private bool _isStopped;
        private bool _mustStopVelocity;
        private float _timeSinceStopped;
        private float _timeSinceStartedCharge;
        private Vector3 _currentChargeVelocity;
        
        public override void SetInputs(OldCharacterInputs inputs) { }

        public override void OnStateEnter() {
            _isStopped = false;
            _timeSinceStopped = 0f;
            _timeSinceStartedCharge = 0f;
            _currentChargeVelocity = Motor.CharacterForward * _chargeSpeed;
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            if (_mustStopVelocity) {
                currentVelocity = Vector3.zero;
                _mustStopVelocity = false;
            }

            if (!_isStopped) {
                float previousY = currentVelocity.y;
                currentVelocity = _currentChargeVelocity;
                currentVelocity.y = previousY;
            }
                    
            currentVelocity += Controller.Gravity * deltaTime;
        }

        public override void HandlePreCharacterUpdate(float deltaTime) {
            _timeSinceStartedCharge += deltaTime;
                    
            if (_isStopped)
                _timeSinceStopped += deltaTime;
        }

        public override void HandlePostCharacterUpdate(float deltaTime) {
            if (!_isStopped && _timeSinceStartedCharge > _maxChargeTime) {
                _mustStopVelocity = true;
                _isStopped = true;
            }

            if (_timeSinceStopped > _stoppedTime)
                StateMachine.SetState(StateMachine.LastState);            
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) {
            if (!_isStopped && !hitStabilityReport.IsStable && Vector3.Dot(-hitNormal, _currentChargeVelocity.normalized) > 0.5f) {
                _mustStopVelocity = true;
                _isStopped = true;
            }
        }
    }
}