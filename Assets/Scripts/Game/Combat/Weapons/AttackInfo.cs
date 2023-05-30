using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;


namespace VHS {
    [Serializable]
    public class AttackInfo {
        public int damage = 1;
        public float pushForce = 10.0f;
        public float radius = 1.0f;
        [Range(0.0f,180.0f)] public float angle = 180.0f;

        public Feedback feedback;
        public ClipTransition animation;
        public bool leftToRight = true;
            
        [HideInInspector] public PlayerAttackType attackType;

        public AttackInfo () { }

        public AttackInfo(AttackInfo source) {
            damage = source.damage;
            pushForce = source.pushForce;
            radius = source.radius;
            angle = source.angle;
            feedback = source.feedback;
            animation = source.animation;
            attackType = source.attackType;
        }

        public static AttackInfo Copy(AttackInfo source) => new AttackInfo(source);
    }
}
