using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VHS {
    
    public class ArenaController : SpawnController, ISlowUpdateListener {
        [Space] 
        [SerializeField] private SpawnData[] _spawnsData;

        private float _timer;

        protected override void Disable() {
            base.Disable();
            UpdateManager.RemoveSlowUpdateListener(this);
        }

        public void OnSlowUpdate(float deltaTime) {
            _timer += deltaTime;
            ConsoleProDebug.Watch("Arena Timer", _timer.ToString());

            bool removedAllKeys = true;

            foreach (SpawnData spawnData in _spawnsData) {
                if(spawnData.SpawnCurve.length == 0)
                    continue;

                removedAllKeys = false;
                Keyframe keyframe = spawnData.SpawnCurve[0];

                if (_timer > keyframe.time) {
                    spawnData.SpawnCurve.RemoveKey(0);
                    int spawnAmount = Mathf.RoundToInt(keyframe.value);

                    for (int i = 0; i < spawnAmount; i++) {
                        Vector3 SpawnPos = GetSpawnPosition(spawnData.NpcPrefab);  
                        SpawnEnemy(spawnData.NpcPrefab, SpawnPos);
                    }
                }
            }

            if (removedAllKeys && _aliveNpcs.Count == 0) 
                StopArena();
        }

        
        public override void StartSpawn() {
            UpdateManager.AddSlowUpdateListener(this);
            StartCallback();
        }
        
        private void StopArena() {
            UpdateManager.RemoveSlowUpdateListener(this);
            FinishCallback();
        }
    }
}