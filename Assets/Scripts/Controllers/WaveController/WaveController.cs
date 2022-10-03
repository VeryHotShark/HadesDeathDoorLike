using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace VHS {
    public class WaveController : ChildBehaviour<GameController> {
        [SerializeField] private float _timeBetweenWaves = 3.0f;

        private List<Wave> _waves;
        
        public event Action OnWavesStart = delegate { };
        public event Action OnWavesCleared = delegate { };
        public event Action<int> OnWaveChanged = delegate { };

        private Wave _currentWave;
        private int _currentWaveIndex;

        private void Awake() => _waves = new List<Wave>(GetComponentsInChildren<Wave>());

        protected override void Enable() {
            foreach (Wave wave in _waves) 
                wave.OnWaveCleared += OnWaveCleared;
        }

        protected override void Disable() {
            foreach (Wave wave in _waves) 
                wave.OnWaveCleared -= OnWaveCleared;
        }

        private void OnWaveCleared(Wave wave) {
            wave.StopWave();
            
            _currentWaveIndex++;

            if (_currentWaveIndex >= _waves.Count) {
                WavesCleared();
                return;
            }
            
            StartWave(_currentWaveIndex);
        }

        private void StartWave(int index) {
            Timing.CallDelayed(_timeBetweenWaves, delegate {
                _currentWave = _waves[index];
                _currentWave.StartWave();
                OnWaveChanged(index);
            });
        }

        private void WavesCleared() {
            OnWavesCleared();
            Log("FINISHED ALL WAVES");
        }

        public void StartWaves() {
            _currentWaveIndex = 0;
            StartWave(_currentWaveIndex);
            OnWavesStart();
        }
    }
}