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

        [Space, Header("SpawnPoints")]
        [SerializeField] private float _minDistance = 5.0f;

        [Space, Header("Random")]
        [SerializeField] private Vector2 _spawnRange = new Vector2(4f, 10f);
        
        public event Action OnArenaCleared = delegate { };

        private float _minDistanceSqr;
        private float _timer;
        private bool _arenaStarted;
        private List<Npc> _aliveNpcs = new List<Npc>();
        
        private SpawnPoint[] _spawnPoints = new SpawnPoint[0];
        private Dictionary<Npc, List<SpawnPoint>> _spawnPointsDict = new Dictionary<Npc, List<SpawnPoint>>();
        
        private void Awake() {
            _minDistanceSqr = _minDistance.Square();
            
            foreach (SpawnData spawnData in _spawnsData) {
                List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
                _spawnPointsDict.Add(spawnData.NpcPrefab, spawnPoints);
            }
            
            SpawnPoint[] allSpawnPoints = GetComponentsInChildren<SpawnPoint>();

            foreach (SpawnPoint spawnPoint in allSpawnPoints) {
                foreach (Npc npc in spawnPoint.Npcs) {
                    if (_spawnPointsDict.TryGetValue(npc, out List<SpawnPoint> spawnPoints))
                        spawnPoints.Add(spawnPoint);                        
                }
            }
        }

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
            Vector3 spawnPos = GetSpawnPosition(prefab);
            Npc spawnedNpc = Instantiate(prefab, spawnPos, Quaternion.identity);
            spawnedNpc.OnDeath += OnNpcDeath;
            _aliveNpcs.Add(spawnedNpc);
            // PoolManager.Spawn(prefab, sampledInfo.position, Quaternion.identity); TODO make NPC Poolable
        }

        private Vector3 GetSpawnPosition(Npc prefab) {
            SpawnPoint spawnPoint = GetValidSpawnPoint(prefab);
                
            if (spawnPoint != null)
                return spawnPoint.transform.position;
            
            return GetRandomPosition();
        }

        private SpawnPoint GetValidSpawnPoint(Npc prefab) {
            List<SpawnPoint> validSpawnPoints = new List<SpawnPoint>();
            
            foreach (SpawnPoint spawnPoint in _spawnPointsDict[prefab]) {
                if (spawnPoint.IsValid()) {
                    float distanceToTarget = Parent.Player.FeetPosition.DistanceSquaredTo(spawnPoint.transform.position);
                    
                    if(distanceToTarget > _minDistanceSqr)
                        validSpawnPoints.Add(spawnPoint);
                }
            }

            int validSpawnPointsCount = validSpawnPoints.Count; 

            if (validSpawnPointsCount > 0) {
                int randomIndex = Random.Range(0,validSpawnPointsCount);
                return validSpawnPoints[randomIndex];
            }

            return null;
        }

        private Vector3 GetRandomPosition() {
            float randomDistance = _spawnRange.Random();
            Vector3 randomOffset = Random.insideUnitSphere.Flatten() * randomDistance;
            Vector3 spawnPos = Parent.Player.FeetPosition + randomOffset;
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