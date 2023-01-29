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
        
        private List<Npc> _npcs = new List<Npc>();

        public Action<Wave> OnWaveCleared;

        private void Awake() {
        }

        public void StartWave() {

        }

        public void StopWave() {

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
    }
}
