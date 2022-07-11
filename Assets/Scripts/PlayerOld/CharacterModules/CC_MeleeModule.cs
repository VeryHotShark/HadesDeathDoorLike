using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class CC_MeleeModule : OldCharacterControllerModule {
        [SerializeField] private int _comboChainCount = 3;
        
        [SerializeField] private float _heavyAttackThreshold = 0.75f;
        [SerializeField] private float _lightAttackThreshold = 0.25f;

        private bool _lastAttackDown;
        private int _lightAttackIndex;
        private float _attackHeldDuration;
        private float _timeSinceAttackPressed;
        private float _timeSinceAttackReleased;

        
        public override void SetInputs(OldCharacterInputs inputs) {
            if (_lastAttackDown != inputs.AttackDown) {
                if(!_lastAttackDown && inputs.AttackDown)
                    OnAttackPressed();
                else if(_lastAttackDown && !inputs.AttackDown)
                    OnAttackReleased();

                _lastAttackDown = inputs.AttackDown;
            }
        }

        private void OnAttackPressed() {
            _timeSinceAttackPressed = Time.time;
            _attackHeldDuration = 0.0f;
        }

        private void OnAttackReleased() {
            if (_attackHeldDuration > _heavyAttackThreshold) {
                Controller.OnHeavyAttack();
            }
            else {
                float timeBetweenReleases = Time.time - _timeSinceAttackReleased;
                
                if (timeBetweenReleases < _lightAttackThreshold)
                    _lightAttackIndex = ++_lightAttackIndex % _comboChainCount;
                else
                    _lightAttackIndex = 0;

                Controller.OnLightAttack(_lightAttackIndex);
            }
            
            _timeSinceAttackReleased = Time.time;
        }

        public override void HandlePreCharacterUpdate(float deltaTime) {
            if (_lastAttackDown)
                _attackHeldDuration += deltaTime;
        }
    }
}
