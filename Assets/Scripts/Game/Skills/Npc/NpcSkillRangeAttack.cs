using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class NpcSkillRangeAttack : NpcSkill {
        public Projectile _projectile;
        public float _speed = 5.0f; // TODO maybe change Projetile to SerializeReferce do all properties can be set directly in editor

        public override void OnCastTick(float deltaTime) => Owner.transform.rotation = Quaternion.LookRotation(Owner.DirectionToTargetFlat);
        
        public override void OnSkillStart() {
            ProjectileBullet bullet = SpawnProjectile<ProjectileBullet>();
            bullet.SetSpeed(_speed);
        }

        public override void OnSkillFinish() {
            Owner.AIAgent.Resume();
            
            // TODO rework this, temporary for stagger to work because this gets called after stagger
            if(Owner.State != NpcState.Recovery)
                Owner.SetState(NpcState.Default);
        }

        private T SpawnProjectile<T>() where T : Projectile {
            Vector3 spawnPos = Owner.CenterOfMass;
            Vector3 direction = Owner.HasTarget ? Owner.CenterOfMass.DirectionTo(Owner.Target.CenterOfMass) : Owner.Forward;
            Quaternion spawnRot = Quaternion.LookRotation(direction);
            Projectile projectile = PoolManager.Spawn(_projectile, spawnPos, spawnRot);
            projectile.Init(Owner);
            return projectile as T;
        }
    }
}
