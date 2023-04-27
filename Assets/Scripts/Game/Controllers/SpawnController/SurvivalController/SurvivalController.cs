using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class SurvivalController : SpawnHandler {
        [SerializeField] private float _survivalDuration = 60.0f;
        
        [Space] 
        [SerializeField] private SpawnData[] _spawnsData;

        private Dictionary<EnemyID, List<Npc>> _aliveNpcsDict = new();

        public override void Init(SpawnController spawnController) {
            base.Init(spawnController);
            
            foreach (SpawnData spawnData in _spawnsData) {
                List<Npc> aliveNpcs = new List<Npc>();
                _aliveNpcsDict.Add(spawnData.EnemyID, aliveNpcs);
            }
        }
        
        public override void OnTick(float dt) {
            base.OnTick(dt);

            if (_timer > _survivalDuration) {
                Finish();
                return;
            }

            float normalizedTime = Mathf.Clamp01(_timer / _survivalDuration);
            
            foreach (SpawnData spawnData in _spawnsData) {
                int desiredNpcCount = Mathf.RoundToInt(spawnData.SpawnCurve.Evaluate(normalizedTime));
                int currentNpcCount = _aliveNpcsDict[spawnData.EnemyID].Count;
                
                if (currentNpcCount < desiredNpcCount) {
                    int enemiesCountToSpawn = desiredNpcCount - currentNpcCount;

                    for (int i = 0; i < enemiesCountToSpawn; i++) {
                        Vector3 SpawnPos = _spawnController.GetSpawnPosition(spawnData.EnemyID);
                        Npc spawnedNpc = _spawnController.SpawnEnemy(spawnData.EnemyID, SpawnPos);
                        _aliveNpcsDict[spawnData.EnemyID].Add(spawnedNpc);
                    }
                }
            }
        }

        public override void OnNpcDeath(Npc npc) => _aliveNpcsDict[npc.ID].Remove(npc);

        public override void Finish() {
            _spawnController.KillAliveNpcs();
            base.Finish();
        }

    }
}
