using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class SurvivalController : SpawnController, ISlowUpdateListener {
        [SerializeField] private float _survivalDuration = 60.0f;
        
        [Space] 
        [SerializeField] private SpawnData[] _spawnsData;

        private float _timer;
        private Dictionary<EnemyID, List<Npc>> _aliveNpcsDict = new();

        protected override void Awake() {
            base.Awake();
            
            foreach (SpawnData spawnData in _spawnsData) {
                List<Npc> aliveNpcs = new List<Npc>();
                _aliveNpcsDict.Add(spawnData.EnemyID, aliveNpcs);
            }
        }
        
        protected override void Disable() {
            base.Disable();
            UpdateManager.RemoveSlowUpdateListener(this);
        }

        protected override void OnNpcDeath(IActor actor) {
            base.OnNpcDeath(actor);
            Npc npc = actor as Npc;
            _aliveNpcsDict[npc.EnemyID].Remove(npc);
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
                int currentNpcCount = _aliveNpcsDict[spawnData.EnemyID].Count;
                
                if (currentNpcCount < desiredNpcCount) {
                    int enemiesCountToSpawn = desiredNpcCount - currentNpcCount;

                    for (int i = 0; i < enemiesCountToSpawn; i++) {
                        Vector3 SpawnPos = GetSpawnPosition(spawnData.EnemyID);
                        Npc spawnedNpc = SpawnEnemy(spawnData.EnemyID, SpawnPos);
                        _aliveNpcsDict[spawnData.EnemyID].Add(spawnedNpc);
                    }
                }
            }
        }

        public override void StartSpawn() {
            UpdateManager.AddSlowUpdateListener(this);
            StartCallback();
        }

        private void StopSurvival() {
            UpdateManager.RemoveSlowUpdateListener(this);

            for (int i = _aliveNpcs.Count - 1; i >= 0; i--) {
                Npc aliveNpc = _aliveNpcs[i];
                aliveNpc.Kill(Parent.Player);
            }

            FinishCallback();
        }
    }
}
