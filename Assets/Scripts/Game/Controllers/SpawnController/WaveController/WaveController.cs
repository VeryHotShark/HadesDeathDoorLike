using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace VHS {
    public class WaveController : SpawnController {
        [SerializeField] private float _timeBetweenWaves = 2.0f;

        private List<Wave> _waves;
        
        public event Action<int> OnWaveChanged = delegate { };

        private Wave _currentWave;
        private int _currentWaveIndex;

        private void Awake() {
            base.Awake();
            _waves = new List<Wave>(GetComponentsInChildren<Wave>());
        }

        protected override void OnNpcDeath(IActor actor) {
            base.OnNpcDeath(actor);
            
            if(_aliveNpcs.Count == 0)
                OnWaveCleared();
        }

        private void OnWaveCleared() {
            _currentWaveIndex++;

            if (_currentWaveIndex >= _waves.Count) {
                FinishCallback();
                return;
            }
            
            StartWave(_currentWaveIndex);
        }
        
        public void OnSpawnRequest(EnemyID enemyID, Vector3 spawnPosition) => SpawnEnemy(enemyID, spawnPosition);

        public override void StartSpawn() {
            _currentWaveIndex = 0;
            StartWave(_currentWaveIndex);
            StartCallback();
        }

        private void StartWave(int index) {
            Timing.CallDelayed(_timeBetweenWaves, delegate {
                _currentWave = _waves[index];
                _currentWave.StartWave();
                OnWaveChanged(index);
            });
        }
    }
}