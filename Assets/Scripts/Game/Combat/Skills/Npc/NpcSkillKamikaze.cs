using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class NpcSkillKamikaze : NpcSkill {
        public float _explodeRadius = 3.0f;
        public int _explodeDamage = 1;
        public ParticleController _explosionParticle;

        public override void OnSkillFinish() {
            Extension_Skill.DoSphereDamage(Owner, _explodeRadius, _explodeDamage, null, null, _explosionParticle);          
            Owner.Kill(Owner);
        }
    }
}
