using System;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class ArenaController : SpawnHandler {
        [Space] 
        [SerializeField] private SpawnData[] _spawnsData;

        public override void OnTick(float dt) {
            base.OnTick(dt);

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
                        Vector3 SpawnPos = _spawnController.GetSpawnPosition(spawnData.EnemyID);  
                        _spawnController.SpawnEnemy(spawnData.EnemyID, SpawnPos);
                    }
                }
            }

            if (removedAllKeys && _spawnController.AliveNpcs.Count == 0) 
                Finish();
        }
    }
}