using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class SpawnPoint : BaseBehaviour {
        [SerializeField] private float _cooldownDuration;
        [SerializeField] private Npc[] _npcsToSpawn;
        [SerializeReference] private ISpawnPointProvider _spawnPointProvider;

        private Timer _cooldown;

        private void Awake() {
            _cooldown = new Timer(_cooldownDuration);
            _spawnPointProvider.Transform = transform;
        }

        public Npc[] Npcs => _npcsToSpawn;
        
        public bool IsValid() => !_cooldown.IsActive;
        public Vector3 ProvidePoint() => _spawnPointProvider.ProvidePoint();

        public void OnSelected() {
            if(_cooldown.Duration > 0.0f)
                _cooldown.Start();
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            
            if(_spawnPointProvider != null)
                _spawnPointProvider.OnDrawGizmos(transform);
        }
    }
}
