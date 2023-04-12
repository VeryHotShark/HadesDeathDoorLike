using System;
using Animancer;
using KinematicCharacterController;
using UnityEngine;

namespace VHS {
    public class Player : Actor<Player> {
        public override Vector3 CenterOfMass => FeetPosition + _characterController.Motor.CharacterTransformToCapsuleCenter;

        private PlayerController _playerController;
        private CharacterController _characterController;
        private InteractionComponent _interactionComponent;
        private WeaponController _weaponController;
        private AnimationController _animationController;
        private PassivesHandlerComponent _passivesHandlerComponent;
        
        public PlayerController PlayerController => _playerController;
        public CameraController CameraController => _playerController.Camera;
        public CharacterController CharacterController => _characterController;
        public KinematicCharacterMotor CharacterMotor => CharacterController.Motor;

        public Camera Camera => CameraController.Camera;
        public AnimancerComponent Animancer => _animationController.Animancer;
        public WeaponController WeaponController => _weaponController;
        public AnimationController AnimationController => _animationController;
        
        public Action OnRoll = delegate { };
        
        public Action OnRangeAttack = delegate { };
        public Action OnPerfectRangeAttack = delegate { };
        public Action OnPerfectRangeAttackEnd = delegate { };
        public Action OnPerfectRangeAttackStart = delegate { };
        public Action OnRangeAttackHeld = delegate { };
        public Action<HitData> OnRangeHit = delegate {};
        
        public Action<int> OnLightAttack = delegate {};
        public Action OnHeavyAttack = delegate {};
        public Action OnHeavyAttackHeld = delegate {};
        public Action OnPerfectMeleeAttack = delegate { };
        public Action OnPerfectMeleeAttackEnd = delegate { };
        public Action OnPerfectMeleeAttackStart = delegate { };
        public Action<HitData> OnMeleeHit = delegate {};
        
        public Action OnUltimateEnter = delegate {  };
        public Action OnUltimateExit = delegate {  };
        public Action<float> OnUltimatePercentChanged = delegate {  };
        
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
