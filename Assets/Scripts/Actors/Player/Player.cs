using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class Player : Actor {
        
        public override Vector3 CenterOfMass => FeetPosition + _characterController.Motor.CharacterTransformToCapsuleCenter;

        private CharacterController _characterController;
        
        protected override void GetComponents() {
            base.GetComponents();
            _characterController = GetComponent<CharacterController>();
        }

        public override void Die() {
            PlayerManager.RegisterPlayerDeath(this);
            base.Die();
        }
        
        
    }
}
