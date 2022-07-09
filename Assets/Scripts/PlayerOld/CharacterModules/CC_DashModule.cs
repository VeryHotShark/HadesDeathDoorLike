using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class CC_DashModule : CharacterControllerModule {
        [SerializeField] private float _force = 10f;
        
        private Vector3 _internalVelocityAdd;
        
        public override void SetInputs(CharacterInputs inputs) {
            if (inputs.DashPressed) {
                Motor.ForceUnground();
                Vector3 dashDirection = Controller.MoveInput.sqrMagnitude > 0 ? Controller.MoveInput : Motor.CharacterForward; 
                AddVelocity(dashDirection * _force);
            }
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            if (_internalVelocityAdd.sqrMagnitude > 0f) {
                currentVelocity += _internalVelocityAdd;
                _internalVelocityAdd = Vector3.zero;
            }
        }

        private void AddVelocity(Vector3 velocity) => _internalVelocityAdd += velocity;
    }
}
