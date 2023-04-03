using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VHS {
    public class AnimationController : ChildBehaviour<Player>, IUpdateListener {
        [TitleGroup("Movement")]
        [SerializeField] private ClipTransition _idleClip;
        [SerializeField] private ClipTransition _moveClip;

        [Space]
        [SerializeField] private ClipTransition _rollClip;
        [SerializeField] private ClipTransition _shootClip;
        [SerializeField] private ClipTransition _shootWindupClip;

        private AnimancerComponent _animancer;
        public AnimancerComponent Animancer => _animancer;
        private CharacterController Character => Parent.CharacterController;


        private void Awake() {
            _animancer = GetComponentInChildren<AnimancerComponent>();
        }

        protected override void Enable() {
            Parent.OnRoll += OnRoll;
            Parent.OnRangeAttack += OnRangeAttack;
            Parent.OnRangeAttackHeld += OnRangeAttackHeld;
            Parent.OnCharacterStateChanged += OnCharacterStateChanged;
            _shootWindupClip.Events.OnEnd += PauseGraph;
            UpdateManager.AddUpdateListener(this);
        }

        protected override void Disable() {
            Parent.OnRoll -= OnRoll;
            Parent.OnRangeAttack -= OnRangeAttack;
            Parent.OnRangeAttackHeld -= OnRangeAttackHeld;
            Parent.OnCharacterStateChanged -= OnCharacterStateChanged;
            _shootWindupClip.Events.OnEnd -= PauseGraph;
            UpdateManager.RemoveUpdateListener(this);
        }

        private void OnRoll() {
            _animancer.Play(_rollClip);
        }
        
        private void OnRangeAttackHeld() {
            _animancer.Play(_shootWindupClip);
        }

        private void OnRangeAttack() {
            _animancer.Playable.UnpauseGraph();
            _animancer.Play(_shootClip);
        }
        
        private void OnCharacterStateChanged(CharacterModule module) {
            return;
            
            switch (module) {
                case CharacterRangeCombat range:
                    _animancer.Play(_shootClip);
                    _animancer.Playable.PauseGraph();
                    _animancer.Evaluate();
                    break;
            }            
        }

        public void OnUpdate(float deltaTime) {
            switch (Parent.CharacterController.CurrentModule) {
                case CharacterMovement movement:
                    _animancer.Play(Character.Motor.Velocity != Vector3.zero && Character.MoveInput != Vector3.zero ? _moveClip : _idleClip);
                    break;
                case CharacterFallingMovement falling:
                    // _animancer.Play(_rollClip);
                    break;
            }
        }

        public void PauseGraph() {
            _animancer.Playable.PauseGraph();
        }
    }
}
