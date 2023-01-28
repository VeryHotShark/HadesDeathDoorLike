using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class SurvivalController : SpawnController, ISlowUpdateListener {
        [SerializeField] private float _survivalDuration = 60.0f;
        
        [Space] 
        [SerializeField] private SpawnData[] _spawnsData;

        private float _timer;
        private Dictionary<Npc, List<Npc>> _aliveNpcsDict = new();

        private void Awake() {
            foreach (SpawnData spawnData in _spawnsData) {
                List<Npc> aliveNpcs = new List<Npc>();
                _aliveNpcsDict.Add(spawnData.NpcPrefab, aliveNpcs);
            }
        }
        
        protected override void Disable() {
            base.Disable();
            UpdateManager.RemoveSlowUpdateListener(this);

            foreach (Npc npcKey in _aliveNpcsDict.Keys) {
                foreach (Npc npc in _aliveNpcsDict[npcKey]) 
                    npc.OnDeath -= OnNpcDeath;
            }
        }

        private void OnNpcDeath(IActor actor) {
            Npc npc = actor as Npc;
            npc.OnDeath -= OnNpcDeath;
            _aliveNpcsDict[npc].Remove(npc);
        }

        public void OnSlowUpdate(float deltaTime) {
            _timer += deltaTime;
            ConsoleProDebug.Watch("Arena Timer", _timer.ToString());

            if (_timer > _survivalDuration) {
                StopSurvival();
                return;
            }

            float normalizedTime = Mathf.Clamp01(_timer / _survivalDuration);
            
            foreach (SpawnData spawnData in _spawnsData) {
                int desiredNpcCount = Mathf.RoundToInt(spawnData.SpawnCurve.Evaluate(normalizedTime));
                int currentNpcCount = _aliveNpcsDict[spawnData.NpcPrefab].Count;
                
                if (currentNpcCount < desiredNpcCount) {
                    int enemiesCountToSpawn = desiredNpcCount - currentNpcCount;

                    for (int i = 0; i < enemiesCountToSpawn; i++)
                        SpawnEnemy(spawnData.NpcPrefab);
                }
            }
        }
        
        private void SpawnEnemy(Npc prefab) {
            Vector3 spawnPos = transform.position;
            Npc spawnedNpc = Instantiate(prefab, spawnPos, Quaternion.identity);
            spawnedNpc.OnDeath += OnNpcDeath;
            _aliveNpcsDict[prefab].Add(spawnedNpc);
            // PoolManager.Spawn(prefab, sampledInfo.position, Quaternion.identity); TODO make NPC Poolable
        }

        public override void StartSpawn() {
            UpdateManager.AddSlowUpdateListener(this);
            StartCallback();
        }

        private void StopSurvival() {
            UpdateManager.RemoveSlowUpdateListener(this);
            
            foreach (Npc npcKey in _aliveNpcsDict.Keys) {
                for (int i = _aliveNpcsDict[npcKey].Count - 1; i >= 0; i--) 
                    _aliveNpcsDict[npcKey][i].Kill(Parent.Player);
            }

            FinishCallback();
        }
    }
}
