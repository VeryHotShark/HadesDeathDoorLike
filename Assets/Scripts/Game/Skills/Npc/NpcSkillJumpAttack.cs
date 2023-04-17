using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blobcreate.ProjectileToolkit;
using ParadoxNotion.Design;

namespace VHS {
    [Serializable]
    public class NpcSkillJumpAttack : NpcSkill {
        public float _jumpHeight;
        public AnimationCurve _jumpCurve;
        
        [ParadoxNotion.Design.Header("Properties")]
        public  float _radius = 3.0f;
        public  int _damage = 1;
        public  ParticleController _particle;
        
        private float _jumpStartTimer;
        private Vector3 _startPos;
        private Vector3 _endPos;

        public override void OnCastTick(float deltaTime) => Owner.transform.rotation = Quaternion.LookRotation(Owner.DirectionToTargetFlat);

        public override void OnSkillStart() {
            _jumpStartTimer = 0.0f;
            _startPos = Owner.FeetPosition;
            _endPos = Owner.TargetPosition;
            Owner.AIAgent.Disable();
        }

        public override void OnSkillTick(float delta) {
            _jumpStartTimer += delta;
            float t = _jumpStartTimer / _skillDuration;
            float height = _jumpCurve.Evaluate(t) * _jumpHeight;
            Vector3 desiredPos = Vector3.Lerp(_startPos, _endPos, t);
            desiredPos += Vector3.up * height;
            Owner.transform.position = desiredPos;
        }

        public override void OnSkillFinish() {
            Extension_Skill.DoSphereDamage(Owner, _radius, _damage, null, null, _particle);
            Owner.AIAgent.Enable();
        }

        public override void OnSkillCancel() => Owner.AIAgent.Enable();
    }
}
