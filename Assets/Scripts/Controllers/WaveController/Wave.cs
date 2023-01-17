using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VHS {
    public class Wave : ChildBehaviour<WaveController> {
        [SerializeField] private int _spawnEnemyCount;

        private int _spawnCount = 0;
        
        private WaveSpawner[] _waveSpawners;
        private List<Npc> _npcs = new List<Npc>();

        public Action<Wave> OnWaveCleared;

        private void Awake() {
            _waveSpawners = GetComponentsInChildren<WaveSpawner>();
        }

        public void StartWave() {
            foreach (WaveSpawner waveSpawner in _waveSpawners) 
                waveSpawner.StartWave();
        }

        public void StopWave() {
            foreach (WaveSpawner waveSpawner in _waveSpawners) 
                waveSpawner.StopWave();
        }

        protected override void Disable() {
            foreach (Npc npc in _npcs) 
                npc.OnDeath -= OnNpcDeath;
        }

        private void OnNpcDeath(IActor actor) {
            Npc npc = actor as Npc;
            npc.OnDeath -= OnNpcDeath;
            _npcs.Remove(npc);

            if (_npcs.Count <= 0)
                OnWaveCleared(this);
        }

        private bool CanSpawn() {
            return _spawnCount < _spawnEnemyCount;
        }

        public void RequestSpawn(WaveSpawner spawner) {
            if (CanSpawn()) {
                Npc npc = spawner.Spawn();
                npc.OnDeath += OnNpcDeath;
                _npcs.Add(npc);
                _spawnCount++;
            }
        }
    }
}
