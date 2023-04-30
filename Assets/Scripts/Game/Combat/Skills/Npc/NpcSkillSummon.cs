using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using UnityEngine;

namespace VHS {
    public class NpcSkillSummon : NpcSkill {
        public BBParameter<Npc> _npc;
        public BBParameter<float> _radius;
        public BBParameter<int> _maxCount;

        private List<Actor> _spawnedNpcs = new List<Actor>();

        public override void OnSkillFinish() {
            Vector3 randomOffset = Random.insideUnitSphere.Flatten() * _radius.value;
            Vector3 spawnPos = Owner.FeetPosition + randomOffset;

            Npc spawnedNpc = PoolManager.Spawn(_npc.value, spawnPos, Quaternion.Euler(0.0f, Random.value * 360.0f, 0.0f));
            
            _spawnedNpcs.Add(spawnedNpc);
            spawnedNpc.OnDeath += OnSpawnedNpcDeath;
                
            base.OnSkillFinish();
        }

        private void OnSpawnedNpcDeath(Actor actor) {
            actor.OnDeath -= OnSpawnedNpcDeath;
            _spawnedNpcs.Remove(actor);
        }

        public override void OnDisable() {
            foreach (Actor spawnedNpc in _spawnedNpcs) 
                spawnedNpc.OnDeath -= OnSpawnedNpcDeath;
        }

        public override bool CanCastSkill() => _spawnedNpcs.Count < _maxCount.value;
    }
}
