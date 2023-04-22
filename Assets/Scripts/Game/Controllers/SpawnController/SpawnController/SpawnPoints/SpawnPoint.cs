using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace VHS {
    public class SpawnPoint : BaseBehaviour {
        [SerializeField, Tooltip("Applicable only if SpawnHandler is WaveController")] private float _spawnDelay;
        [SerializeField] private float _cooldownDuration;
        [SerializeField] private EnemyID[] _npcsToSpawn;
        [SerializeReference] private ISpawnPointProvider _spawnPointProvider;

        private Timer _cooldown;
        public float SpawnDelay => _spawnDelay;

        private void Awake() {
            _cooldown = new Timer(_cooldownDuration);
            _spawnPointProvider.Initialize(transform);
        }

        public EnemyID[] Npcs => _npcsToSpawn;
        
        public bool IsValid() => !_cooldown.IsActive;

        public Vector3 ProvidePoint() {
            OnPointProvided();
            Vector3 point = _spawnPointProvider.ProvidePoint();
            NNInfo nnInfo = AstarPath.active.GetNearest(point);
            return nnInfo.position;
        }

        private void OnPointProvided() {
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
