using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class NpcSkillSummon : NpcSkill {
        public Npc _npcToSummon;
        public float _spawnRadius = 3.0f;

        private List<Npc> _spawnedNpcs = new List<Npc>();

        public override void OnSkillFinish() {

            Vector3 randomOffset = Random.insideUnitSphere.Flatten() * _spawnRadius;
            Vector3 spawnPos = Owner.FeetPosition + randomOffset;

            Npc spawnedNpc = PoolManager.Spawn(_npcToSummon, spawnPos, Quaternion.Euler(0.0f, Random.value * 360.0f, 0.0f));
            
            _spawnedNpcs.Add(spawnedNpc);
            // /Debug.Log(_spawnedNpcs.Count);
                
            base.OnSkillFinish();
        }
    }
}
