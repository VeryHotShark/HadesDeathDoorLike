using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class CC_CrouchModule : OldCharacterControllerModule {
        [SerializeField, Range(0f, 1f)] private float _crouchRatio = 0.5f;
        
        private bool _isCrouching;
        private bool _shouldBeCrouching;
        
        private Collider[] _probedColliders = new Collider[8];
        
        public override void SetInputs(OldCharacterInputs inputs) {
            if (inputs.CrouchDown) {
                _shouldBeCrouching = true;

                if (!_isCrouching) {
                    _isCrouching = true;
                    Motor.SetCapsuleDimensions(0.5f, 2f * _crouchRatio,1f * _crouchRatio);
                    Controller.OnCrouchStart();
                }
            }
            else if (inputs.CrouchUp) 
                _shouldBeCrouching = false;
        }

        public override void OnStateExit() {
            _shouldBeCrouching = false;
            HandlePostCharacterUpdate(Time.deltaTime);
        }

        public override void HandlePostCharacterUpdate(float deltaTime) {
            if (_isCrouching && !_shouldBeCrouching) {
                Motor.SetCapsuleDimensions(0.5f, 2f, 1f);

                if (Motor.CharacterCollisionsOverlap(Motor.TransientPosition, Motor.TransientRotation, _probedColliders) == 0) {
                    _isCrouching = false;
                    Controller.OnCrouchEnd();   
                }
                else
                    Motor.SetCapsuleDimensions(0.5f, 2f * _crouchRatio, 1f * _crouchRatio);
            }
        }
    }
}
