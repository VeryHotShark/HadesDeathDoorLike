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
        [SerializeField] private ClipTransition _heavyAttack;
        [SerializeField] private ClipTransition _heavyAttackWindup;
        
        [SerializeField] private ClipTransition _rollClip;
        [SerializeField] private ClipTransition _shootClip;

        private bool _duringAction = false;
        private AnimancerComponent _animancer;

        public AnimancerComponent Animancer => _animancer;

        private void Awake() {
            _animancer = GetComponentInChildren<AnimancerComponent>();
            _shootClip.Events.OnEnd = OnActionEnd;
            _rollClip.Events.OnEnd = OnActionEnd;
        }

        private void OnActionEnd() {
            _duringAction = false;
        }
        
        protected override void Enable() {
            Parent.Player.OnRoll += OnRoll;
            Parent.Player.OnLightAttack += OnLightAttack;
            Parent.Player.OnRangeAttack += OnRangeAttack;
            Parent.Player.OnHeavyAttack += OnHeavyAttack;
            Parent.Player.OnHeavyAttackHeld += OnHeavyAttackHeld;
            Parent.Player.OnHeavyAttackReached += OnHeavyAttackReached;
            Parent.Player.OnCharacterStateChanged += OnCharacterStateChanged;
            UpdateManager.AddUpdateListener(this);
        }

        protected override void Disable() {
            Parent.Player.OnRoll -= OnRoll;
            Parent.Player.OnLightAttack -= OnLightAttack;
            Parent.Player.OnRangeAttack -= OnRangeAttack;
            Parent.Player.OnHeavyAttack -= OnHeavyAttack;
            Parent.Player.OnHeavyAttackHeld -= OnHeavyAttackHeld;
            Parent.Player.OnHeavyAttackReached -= OnHeavyAttackReached;
            Parent.Player.OnCharacterStateChanged -= OnCharacterStateChanged;
            UpdateManager.RemoveUpdateListener(this);
        }

        private void OnRoll() {
            _duringAction = true;
            _animancer.Play(_rollClip);
        }

        private void OnLightAttack(int index) {
            _duringAction = false;
            _animancer.Play(_lightAttacks[index]);
        }

        private void OnHeavyAttack() {
            _duringAction = false;
            _animancer.Playable.UnpauseGraph();
            _animancer.Play(_heavyAttack);
        }

        private void OnHeavyAttackHeld() {
            _animancer.Play(_heavyAttackWindup);
        }

        private void OnHeavyAttackReached() {
            _animancer.Playable.PauseGraph();
        }

        private void OnRangeAttack() {
            _animancer.Playable.UnpauseGraph();
            _duringAction = true;
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
            if(_duringAction)
                return;

            switch (Parent.CurrentModule) {
                case CharacterMovement movement:
                    _animancer.Play(Parent.Motor.Velocity != Vector3.zero && Parent.MoveInput != Vector3.zero ? _moveClip : _idleClip);
                    break;
                case CharacterFallingMovement falling:
                    // _animancer.Play(_rollClip);
                    break;
            }
        }

        public void PlayAction(ClipTransition clip) {
            _duringAction = true;
            clip.Events.OnEnd = OnActionEnd;
            _animancer.Play(clip);
        }
    }
}
