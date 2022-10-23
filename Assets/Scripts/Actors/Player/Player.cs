using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class Player : Actor<Player> {
        public Action<HitData> OnParry = delegate { };
        public Action<HitPoints> OnHealthChanged = delegate { };
        
        public override Vector3 CenterOfMass => FeetPosition + _characterController.Motor.CharacterTransformToCapsuleCenter;
        

        private CharacterController _characterController;
        public CharacterController CharacterController => _characterController;

        public HitPoints HitPoints => _hitProcessorComponent.HitPoints;
        public bool DuringParry => _characterController.ParryModule.DuringParryWindow;

        protected override void GetComponents() {
            base.GetComponents();
            _characterController = GetComponent<CharacterController>();
        }

        protected override void Initialize() {
            HitPoints.OnChanged = hitPoints => OnHealthChanged(hitPoints);            
            base.Initialize();
        }

        public override void Die() {
            PlayerManager.RegisterPlayerDeath(this);
            base.Die();
        }
        
        
    }
}
