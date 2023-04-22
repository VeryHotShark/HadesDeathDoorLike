using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;


namespace VHS {
    [Serializable]
    public class AttackInfo {
        public int damage = 1;
        public Timer duration = new(0.3f);
        public float pushForce = 10.0f;
        public float zOffset = 1.0f;
        public float radius = 1.0f;
        [Range(0.0f,180.0f)] public float angle = 180.0f;

        public Feedback feedback;
        public ClipTransition animation;
            
        [HideInInspector] public PlayerAttackType attackType;

        public AttackInfo () { }

        public AttackInfo(AttackInfo source) {
            damage = source.damage;
            duration = source.duration;
            pushForce = source.pushForce;
            zOffset = source.zOffset;
            radius = source.radius;
            angle = source.angle;
            feedback = source.feedback;
            animation = source.animation;
            attackType = source.attackType;
        }

        public static AttackInfo Copy(AttackInfo source) => new AttackInfo(source);
    }
}
