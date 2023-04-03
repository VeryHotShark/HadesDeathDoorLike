using System;
using Animancer;
using UnityEngine;

namespace VHS {
    public class Player : Actor<Player> {
        public override Vector3 CenterOfMass => FeetPosition + _characterController.Motor.CharacterTransformToCapsuleCenter;

        private PlayerController _playerController;
        private CharacterController _characterController;
        private InteractionComponent _interactionComponent;
        private WeaponController _weaponController;
        private AnimationController _animationController;
        
        
                
        public PlayerController PlayerController => _playerController;
        public CharacterController CharacterController => _characterController;

        public Camera Camera => PlayerController.Camera.Camera;
        public AnimancerComponent Animancer => _animationController.Animancer;
        public WeaponController WeaponController => _weaponController;
        public AnimationController AnimationController => _animationController;
        

        
        public Action OnRoll = delegate { };
        public Action OnRangeAttack = delegate { };
        public Action OnRangeAttackHeld = delegate { };
        public Action<int> OnLightAttack = delegate {};
        public Action OnHeavyAttack = delegate {};
        public Action OnHeavyAttackHeld = delegate {};
        public Action OnHeavyAttackReached = delegate {};
        public Action<HitData> OnMeleeHit = delegate {};
        
        public Action<int> OnCurrentAmmoChanged = delegate {};
        public Action<Weapon> OnWeaponChanged = delegate { };
        public Action<IInteractable> OnInteractableChanged = delegate {};
        public Action<CharacterModule> OnCharacterStateChanged = delegate {  };

        protected override void GetComponents() {
            base.GetComponents();
            _animationController = GetComponent<AnimationController>();
            _characterController = GetComponent<CharacterController>();
            _playerController = GetComponentInParent<PlayerController>();
            _weaponController = GetComponent<WeaponController>();
            _interactionComponent = GetComponent<InteractionComponent>();
        }

        public override void Die() {
            PlayerManager.RegisterPlayerDeath(this);
            base.Die();
        }

        public void HandleInteract() => _interactionComponent.TryInteract();
    }
}
