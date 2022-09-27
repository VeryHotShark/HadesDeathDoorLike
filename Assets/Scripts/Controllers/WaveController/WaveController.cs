using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace VHS {
    public class WaveController : ChildBehaviour<GameController> {
        [SerializeField] private float _timeBetweenWaves = 3.0f;
        [SerializeField] private Npc _npcPrefab;

        private List<Wave> _waves ;

        private int _currentWaveIndex;
        private Wave _currentWave;
        
        public Npc NpcPrefab => _npcPrefab;

        private void Awake() {
            _waves = new List<Wave>(GetComponentsInChildren<Wave>());
        }

        protected override void Enable() {
            foreach (Wave wave in _waves) 
                wave.OnWaveCleared += OnWaveCleared;
        }

        protected override void Disable() {
            foreach (Wave wave in _waves) 
                wave.OnWaveCleared -= OnWaveCleared;
        }

        private void OnWaveCleared(Wave wave) {
            _currentWaveIndex++;

            if (_currentWaveIndex >= _waves.Count) {
                OnWavesCleared();
                return;
            }
            
            StartWave(_currentWaveIndex);
        }

        private void StartWave(int index) {
            Timing.CallDelayed(_timeBetweenWaves, delegate {
                _currentWave = _waves[index];
                _currentWave.Spawn();
            });
        }

        private void OnWavesCleared() {
            Log("FINISHED ALL WAVES");
        }

        public void StartWaves() {
            _currentWaveIndex = 0;
            StartWave(_currentWaveIndex);
        }
    }
}