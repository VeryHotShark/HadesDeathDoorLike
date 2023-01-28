using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VHS {
    public class WaveSpawner : ChildBehaviour<Wave> {
        [SerializeField] private Npc[] _prefabs;
        [SerializeField] private float _spawnRate;
        [SerializeField] private float _spawnRadius;

        private Timer _spawnTimer;

        private void Awake() => _spawnTimer = new Timer(_spawnRate, true);
        protected override void Enable() => _spawnTimer.OnEnd += OnSpawnTimerEnd;
        protected override void Disable() => _spawnTimer.OnEnd -= OnSpawnTimerEnd;

        /// <summary>
        /// Send spawn request to Wave, if succeeds Spawn will be called, else nothing happens 
        /// </summary>
        private void OnSpawnTimerEnd() => Parent.RequestSpawn(this);

        public void StartWave() {
            if(_spawnRate > 0.0f)
                Timing.CallDelayed(Random.value * 2.0f, () => _spawnTimer.Start());
            else
                Parent.RequestSpawn(this);
        }

        public void StopWave() => _spawnTimer.Reset();

        public Npc Spawn() {
            int randomIndex = Random.Range(0, _prefabs.Length);
            Vector3 randomOffset = _spawnRadius > 0 ? Random.insideUnitSphere.Flatten() * _spawnRadius : Vector3.zero;
            Vector3 spawnPos = transform.position + randomOffset;
            NNInfo info = AstarPath.active.GetNearest(spawnPos);
            return Instantiate(_prefabs[randomIndex], info.position, Quaternion.identity);
        }

        private void OnDrawGizmosSelected() => DebugExtension.DrawCircle(gameObject.transform.position, Color.red, _spawnRadius);
    }
}
