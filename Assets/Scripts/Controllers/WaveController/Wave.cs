using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VHS {
    public class Wave : ChildBehaviour<WaveController> {
        [SerializeField] private float _spawnRadius;
        [SerializeField] private int _spawnEnemyCount;
        
        private List<Npc> _npcs = new List<Npc>();

        public Action<Wave> OnWaveCleared; 
        
        public void Spawn() {
            for (int i = 0; i < _spawnEnemyCount; i++) {
                Vector3 randomPos = Random.insideUnitSphere.Flatten() * _spawnRadius;
                NNInfo info = AstarPath.active.GetNearest(randomPos);
                Npc npc = Instantiate(Parent.NpcPrefab, info.position, Quaternion.identity);

                npc.OnDeath += OnNpcDeath;
                _npcs.Add(npc);
            }
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
    }
}
