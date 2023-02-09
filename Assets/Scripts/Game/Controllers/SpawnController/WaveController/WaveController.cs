using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class WaveController : SpawnHandler {
        [SerializeField] private float _timeBetweenWaves = 2.0f;

        private List<Wave> _waves;
        
        public event Action<int> OnWaveChanged = delegate { };

        private Wave _currentWave;
        private int _currentWaveIndex;

        public override void Init(SpawnController spawnController) {
            base.Init(spawnController);
            _waves = new List<Wave>(_spawnController.GetComponentsInChildren<Wave>());
        }

        public override void OnNpcDeath(Npc npc) {
            if(_spawnController.AliveNpcs.Count == 0)
                OnWaveCleared();
        }

        private void OnWaveCleared() {
            _currentWaveIndex++;

            if (_currentWaveIndex >= _waves.Count) {
                Finish();
                return;
            }
            
            SpawnWave(_currentWaveIndex);
        }

        public override void Start() {
            _currentWaveIndex = 0;
            SpawnWave(_currentWaveIndex);
        }

        private void SpawnWave(int index) {
            Timing.CallDelayed(_timeBetweenWaves, delegate {
                _currentWave = _waves[index];
                _currentWave.Spawn(_spawnController);
                OnWaveChanged(index);
            });
        }
    }
}