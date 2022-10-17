using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class CharacterParry : CharacterModule {
        [SerializeField] private Timer _parryWindow = new Timer(0.5f);

        public bool DuringParryWindow => _parryWindow.IsActive;
        
        private void OnEnable() {
            _parryWindow.OnEnd += OnParryEnd;
            Player.OnParry += OnParry;
        }

        private void OnDisable() {
            _parryWindow.OnEnd -= OnParryEnd;
            Player.OnParry -= OnParry;
        }

        private void OnParry(HitData hitData) {
            if (hitData.dealer != null) {
                hitData.dealer.OnMyAttackParried(hitData);
                //Log("DUPA"); // TODO zeby po nacisnieciu przeniosło do dobrej linijki w konsoli
            }
            
            _parryWindow.Reset();
            Controller.TransitionToDefaultState();
        }

        private void OnParryEnd() => Controller.TransitionToDefaultState();

        public override void OnEnter() {
            Motor.SetRotation(Controller.LastCharacterInputs.CursorRotation);
            _parryWindow.Start();
        }
        
        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            currentVelocity = Vector3.zero;
        }
        
        public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
            currentRotation = Quaternion.LookRotation(Controller.LookInput);
            Controller.LastNonZeroMoveInput = Controller.LookInput;
        }
    }
}
