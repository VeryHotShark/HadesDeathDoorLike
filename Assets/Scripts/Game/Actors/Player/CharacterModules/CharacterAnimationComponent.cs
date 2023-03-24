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
        
        [TitleGroup("Actions")]
        [SerializeField] private ClipTransition[] _lightAttacks;
        
        private AnimancerComponent _animancer;

        private void Awake() {
            _animancer = GetComponentInChildren<AnimancerComponent>();
        }

        protected override void Enable() {
            Parent.Player.OnLightAttack += OnLightAttack;
            UpdateManager.AddUpdateListener(this);
        }

        protected override void Disable() {
            Parent.Player.OnLightAttack -= OnLightAttack;
            UpdateManager.RemoveUpdateListener(this);
        }

        private void OnLightAttack(int index) {
            _animancer.Play(_lightAttacks[index]);
        }

        public void OnUpdate(float deltaTime) {
            if (Parent.CurrentModule == Parent.MovementModule) 
                _animancer.Play(Parent.MoveInput != Vector3.zero ? _moveClip : _idleClip);
        }
    }
}
