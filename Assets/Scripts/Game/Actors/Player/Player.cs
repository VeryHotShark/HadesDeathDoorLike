using System;
using Animancer;
using UnityEngine;

namespace VHS {
    public class Player : Actor<Player> {
        public override Vector3 CenterOfMass => FeetPosition + _characterController.Motor.CharacterTransformToCapsuleCenter;

        private PlayerController _playerController;
        private CharacterController _characterController;
        private PlayerInteractionComponent _playerInteractionComponent;
                
        public PlayerController PlayerController => _playerController;
        public CharacterController CharacterController => _characterController;

        public Camera Camera => PlayerController.Camera.Camera;
        public AnimancerComponent Animancer => AnimationComponent.Animancer;
        public CharacterAnimationComponent AnimationComponent => CharacterController.AnimationComponent;

        
        public Action OnRoll = delegate { };
        public Action OnRangeAttack = delegate { };
        public Action<int> OnLightAttack = delegate {};
        public Action OnHeavyAttack = delegate {};
        public Action OnHeavyAttackHeld = delegate {};
        public Action OnHeavyAttackReached = delegate {};
        public Action<HitData> OnMeleeHit = delegate {};
        
        public Action<int> OnCurrentAmmoChanged = delegate {};
        public Action<IInteractable> OnInteractableChanged = delegate {};
        public Action<CharacterModule> OnCharacterStateChanged = delegate {  };

        protected override void GetComponents() {
            base.GetComponents();
            _characterController = GetComponent<CharacterController>();
            _playerController = GetComponentInParent<PlayerController>();
            _playerInteractionComponent = GetComponent<PlayerInteractionComponent>();
        }

        public override void Die() {
            PlayerManager.RegisterPlayerDeath(this);
            base.Die();
        }

        public void HandleInteract() => _playerInteractionComponent.TryInteract();
    }
}
