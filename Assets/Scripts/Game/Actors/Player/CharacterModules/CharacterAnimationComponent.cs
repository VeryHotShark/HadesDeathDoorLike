using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

namespace VHS {
    public class CharacterAnimationComponent : ChildBehaviour<Player> {
        [SerializeField] private AnimationClip _meleeAttackClip;
        private AnimancerComponent _animancer;

        private void Awake() {
            _animancer = GetComponentInChildren<AnimancerComponent>();
        }

        protected override void Enable() {
            Parent.OnMeleeAttack += OnMeleeAttack;
        }

        protected override void Disable() {
            Parent.OnMeleeAttack -= OnMeleeAttack;
        }

        private void OnMeleeAttack() {
            Log("DUPA");
            _animancer.Play(_meleeAttackClip);
        }
    }
}
