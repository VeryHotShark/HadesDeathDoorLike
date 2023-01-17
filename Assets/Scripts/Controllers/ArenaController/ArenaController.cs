using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VHS {
    [Serializable]
    public struct SpawnData {
        public Npc NpcPrefab;
        public AnimationCurve SpawnCurve;
    }
    
    public class ArenaController : ChildBehaviour<GameController>, ISlowUpdateListener {
        [SerializeField] private float _spawnInterval = 1.0f;
        [SerializeField] private float _arenaDuration = 30.0f;

        [Space] 
        [SerializeField] private SpawnData[] _spawnsData;

        public event Action OnArenaCleared = delegate { };
        
        private float _timer;
        private bool _arenaStarted;
        private List<Npc> _aliveNpcs = new List<Npc>();
        

        protected override void Enable() {
            base.Enable();
            UpdateManager.AddSlowUpdateListener(this);
        }

        protected override void Disable() {
            base.Disable();
            UpdateManager.RemoveSlowUpdateListener(this);

            foreach (Npc npc in _aliveNpcs) 
                npc.OnDeath -= OnNpcDeath;
        }

        public void OnSlowUpdate(float deltaTime) {
            if(!_arenaStarted)
                return;
            
            _timer += deltaTime;
            ConsoleProDebug.Watch("Arena Timer", _timer.ToString());

            bool removedAllKeys = true;

            foreach (SpawnData spawnData in _spawnsData) {
                if(spawnData.SpawnCurve.length == 0)
                    continue;

                removedAllKeys = false;
                Keyframe keyframe = spawnData.SpawnCurve[0];

                if (_timer > keyframe.time) {
                    spawnData.SpawnCurve.RemoveKey(0);
                    int spawnAmount = Mathf.RoundToInt(keyframe.value);

                    for (int i = 0; i < spawnAmount; i++)
                        SpawnEnemy(spawnData.NpcPrefab);
                }
            }

            if (removedAllKeys && _aliveNpcs.Count == 0) {
                Log("VICTORY");
                OnArenaCleared();
                StopArena();
            }
        }

        private void SpawnEnemy(Npc prefab) {
            Vector3 spawnPos = GetSpawnPosition();
            Npc spawnedNpc = Instantiate(prefab, spawnPos, Quaternion.identity);
            spawnedNpc.OnDeath += OnNpcDeath;
            _aliveNpcs.Add(spawnedNpc);
            // PoolManager.Spawn(prefab, sampledInfo.position, Quaternion.identity); TODO make NPC Poolable
        }

        private Vector3 GetSpawnPosition() {
            Vector3 randomOffset = Random.insideUnitSphere.Flatten() * 10.0f;
            Vector3 spawnPos = transform.position + randomOffset;
            NNInfo sampledInfo = AstarPath.active.GetNearest(spawnPos);
            return sampledInfo.position;
        }
        
        private void OnNpcDeath(IActor actor) {
            Npc npc = actor as Npc;
            npc.OnDeath -= OnNpcDeath;
            _aliveNpcs.Remove(npc);
        }

        public void StartArena() => _arenaStarted = true;
        public void StopArena() => _arenaStarted = false;
    }
}