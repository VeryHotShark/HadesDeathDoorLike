using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class CC_JumpModule : OldCharacterControllerModule {
        [Header("Jumping")]
        [SerializeField] private float _jumpSpeed = 10f;
        [SerializeField] private float _jumpPreGroundingGraceTime = 0.2f;
        [SerializeField] private float _jumpPostGroundingGraceTime = 0.2f;
        
        private bool _jumpConsumed;
        private bool _jumpThisFrame;
        private bool _jumpRequested;
        private float _timeSinceJumpRequested;
        private float _timeSinceLastAbleToJump;
        
        public override void SetInputs(OldCharacterInputs inputs) {
            if (inputs.RollPressed) {
                _jumpRequested = true;
                _timeSinceJumpRequested = 0f;
            }
        }
        
        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            _jumpThisFrame = false;
            _timeSinceJumpRequested += deltaTime;

            if (_jumpRequested && !_jumpConsumed && (Motor.GroundingStatus.FoundAnyGround || _timeSinceLastAbleToJump <= _jumpPostGroundingGraceTime)) {
                Vector3 jumpDirection = Motor.CharacterUp;

                if (Motor.GroundingStatus.FoundAnyGround && !Motor.GroundingStatus.IsStableOnGround)
                    jumpDirection = Motor.GroundingStatus.GroundNormal;
                
                Motor.ForceUnground();
                currentVelocity += jumpDirection * _jumpSpeed - Vector3.Project(currentVelocity, Motor.CharacterUp);
                _jumpRequested = false;
                _jumpConsumed = true;
                _jumpThisFrame = true;
            }
        }

        public override void HandlePostCharacterUpdate(float deltaTime) {
            if (_jumpRequested && _timeSinceJumpRequested > _jumpPreGroundingGraceTime)
                _jumpRequested = false;

            if (Motor.GroundingStatus.FoundAnyGround) {
                if (!_jumpThisFrame)
                    _jumpConsumed = false;

                _timeSinceLastAbleToJump = 0f;
            }
            else
                _timeSinceLastAbleToJump += deltaTime;
        }
    }
}
