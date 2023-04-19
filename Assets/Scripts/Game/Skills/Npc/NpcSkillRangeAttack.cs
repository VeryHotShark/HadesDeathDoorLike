using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class NpcSkillRangeAttack : NpcSkill {
        public int _projectileCount = 1;
        public float _angle = 0.0f;
        public Projectile _projectile;
        public float _speed = 5.0f; // TODO maybe change Projetile to SerializeReferce do all properties can be set directly in editor

        public override void OnCastTick(float deltaTime) => Owner.transform.rotation = Quaternion.LookRotation(Owner.DirectionToTargetFlat);
        
        public override void OnSkillStart() {
            float startAngle = -(_projectileCount / 2.0f) * _angle;
            Vector3 direction =  Owner.Forward;

            for (int i = 0; i < _projectileCount; i++) {
                Vector3 rotatedDirection = Quaternion.Euler(0.0f, startAngle, 0.0f) * direction;
                ProjectileBullet bullet = SpawnProjectile<ProjectileBullet>(rotatedDirection);
                bullet.SetSpeed(_speed);
                startAngle += _angle;
            }
        }

        private T SpawnProjectile<T>(Vector3 direction) where T : Projectile {
            Vector3 spawnPos = Owner.CenterOfMass;
            Quaternion spawnRot = Quaternion.LookRotation(direction);
            Projectile projectile = PoolManager.Spawn(_projectile, spawnPos, spawnRot);
            projectile.Init(Owner);
            return projectile as T;
        }
    }
}
