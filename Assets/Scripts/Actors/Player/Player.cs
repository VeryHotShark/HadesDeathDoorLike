using System;
using UnityEngine;

namespace VHS {
    public class Player : Actor<Player> {
        public override Vector3 CenterOfMass => FeetPosition + _characterController.Motor.CharacterTransformToCapsuleCenter;
        
        private PlayerController _playerController;
        
        private CharacterController _characterController;
        public PlayerController PlayerController => _playerController;
        public CharacterController CharacterController => _characterController;

        public Camera Camera => PlayerController.Camera.Camera;

        protected override void GetComponents() {
            base.GetComponents();
            _characterController = GetComponent<CharacterController>();
            _playerController = GetComponentInParent<PlayerController>();
        }

        public override void Die() {
            PlayerManager.RegisterPlayerDeath(this);
            base.Die();
        }
        
        
    }
}
