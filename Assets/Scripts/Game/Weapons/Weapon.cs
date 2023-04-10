using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using KinematicCharacterController;
using MEC;
using MoreMountains.Feedbacks;
using ParadoxNotion;
using UnityEngine;

namespace VHS {
    public enum PlayerAttackType {
        LIGHT,
        HEAVY,
        PERFECT_HEAVY,
        DASH_LIGHT,
        DASH_HEAVY,
        RANGE,
        PERFECT_RANGE
    }
    
    
    public abstract class Weapon : BaseBehaviour { // TODO do the same to Range and maybe seperate by WeaponMelee, WeaponRange
        [Header("Common")]
        [SerializeField] protected Timer _cooldown = new(0.5f);
        
        public Timer Cooldown => _cooldown;
        
        private MeshRenderer[] _renderers;
                
        protected Player _player;
        protected KinematicCharacterMotor Motor => Character.Motor;
        protected CharacterController Character => _player.CharacterController;
        protected AnimancerComponent Animancer => _player.Animancer;
        protected AnimationController AnimationController => _player.AnimationController;
        
        public bool IsOnCooldown => _cooldown.IsActive;
        

        protected virtual void Awake() => _renderers = GetComponentsInChildren<MeshRenderer>();

        public virtual void Init(Player player) => _player = player;

        public void SetVisible(bool state) {
            foreach (MeshRenderer renderer in _renderers)
                renderer.enabled = state;
        }

        public void StartCooldown() {
            if(_cooldown.Duration > 0.0f)
                _cooldown.Start();
        }
    }
}
