using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VHS {
    public class CharacterAnimationComponent : ChildBehaviour<CharacterController>, IUpdateListener {
        [TitleGroup("Movement")]
        [SerializeField] private ClipTransition _idleClip;
        [SerializeField] private ClipTransition _moveClip;

        [Space]
        [SerializeField] private ClipTransition _rollClip;
        [SerializeField] private ClipTransition _shootClip;

        private AnimancerComponent _animancer;

        public AnimancerComponent Animancer => _animancer;

        private void Awake() {
            _animancer = GetComponentInChildren<AnimancerComponent>();
        }

        protected override void Enable() {
            Parent.Player.OnRoll += OnRoll;
            Parent.Player.OnRangeAttack += OnRangeAttack;
            Parent.Player.OnCharacterStateChanged += OnCharacterStateChanged;
            UpdateManager.AddUpdateListener(this);
        }

        protected override void Disable() {
            Parent.Player.OnRoll -= OnRoll;
            Parent.Player.OnRangeAttack -= OnRangeAttack;
            Parent.Player.OnCharacterStateChanged -= OnCharacterStateChanged;
            UpdateManager.RemoveUpdateListener(this);
        }

        private void OnRoll() {
            _animancer.Play(_rollClip);
        }

        private void OnRangeAttack() {
            _animancer.Playable.UnpauseGraph();
            _animancer.Play(_shootClip);
        }
        
        private void OnCharacterStateChanged(CharacterModule module) {
            switch (module) {
                case CharacterRangeCombat range:
                    _animancer.Play(_shootClip);
                    _animancer.Playable.PauseGraph();
                    _animancer.Evaluate();
                    break;
            }            
        }

        public void OnUpdate(float deltaTime) {
            switch (Parent.CurrentModule) {
                case CharacterMovement movement:
                    _animancer.Play(Parent.Motor.Velocity != Vector3.zero && Parent.MoveInput != Vector3.zero ? _moveClip : _idleClip);
                    break;
                case CharacterFallingMovement falling:
                    // _animancer.Play(_rollClip);
                    break;
            }
        }
    }
}
