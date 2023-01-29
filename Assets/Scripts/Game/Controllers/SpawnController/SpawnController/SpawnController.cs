using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Sirenix.OdinInspector;
using UnityEditor.Experimental;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VHS {
    public enum SpawnStartType {
        Aggro,
        Trigger,
        Immediate
    }
    
    [Serializable]
    public struct SpawnData {
        public Npc NpcPrefab;
        public AnimationCurve SpawnCurve;
    }
    
    public abstract class SpawnController : ChildBehaviour<GameController> {
        [SerializeField] private SpawnStartType _spawnStartType = SpawnStartType.Immediate;

        [TitleGroup("Spawn Requirements")]
        [SerializeField] private EQSPointProvider _eqsPointProvider;
        [SerializeField] private float _minDistance = 5.0f;

        
        public event Action OnStarted = delegate { };
        public event Action OnFinished = delegate { };
        
        protected float _minDistanceSqr;
        
        protected List<Npc> _aliveNpcs = new();
        protected Dictionary<Npc, List<SpawnPoint>> _spawnPointsDict = new();

        private void Awake() {
            _minDistanceSqr = _minDistance.Square();
            
            SpawnPoint[] allSpawnPoints = GetComponentsInChildren<SpawnPoint>();
            
            foreach (SpawnPoint spawnPoint in allSpawnPoints) {
                foreach (Npc npc in spawnPoint.Npcs) {
                    if (_spawnPointsDict.TryGetValue(npc, out List<SpawnPoint> spawnPoints))
                        spawnPoints.Add(spawnPoint); 
                    else 
                        _spawnPointsDict.Add(npc, new List<SpawnPoint>{spawnPoint});
                }
            }
        }

        protected override void Disable() {
            foreach (Npc npc in _aliveNpcs) 
                npc.OnDeath -= OnNpcDeath;
        }

        protected Npc SpawnEnemy(Npc prefab, Vector3 position) {
            Npc spawnedNpc = Instantiate(prefab, position, Quaternion.identity);
            spawnedNpc.OnDeath += OnNpcDeath;
            _aliveNpcs.Add(spawnedNpc);
            return spawnedNpc;
        }
        
        private void OnNpcDeath(IActor actor) {
            Npc npc = actor as Npc;
            npc.OnDeath -= OnNpcDeath;
            _aliveNpcs.Remove(npc);
        }
        
        protected Vector3 GetSpawnPosition(Npc prefab) {
            SpawnPoint spawnPoint = GetValidSpawnPoint(prefab);
                
            if (spawnPoint != null)
                return spawnPoint.ProvidePoint();

            var eqsPoint = GetEQSPoint();

            if (eqsPoint != null)
                return eqsPoint.Value;
            
            return GetRandomPosition();
        }
        
        protected SpawnPoint GetValidSpawnPoint(Npc prefab) {
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

        protected Vector3? GetEQSPoint() {
            Vector3 point = _eqsPointProvider.ProvidePoint();

            if (point != Vector3.zero)
                return point;
            
            return null;
        }
        
        protected Vector3 GetRandomPosition() {
            float randomDistance = new Vector2(4,10).Random();
            Vector3 randomOffset = Random.insideUnitSphere.Flatten() * randomDistance;
            Vector3 spawnPos = Parent.Player.FeetPosition + randomOffset;
            NNInfo sampledInfo = AstarPath.active.GetNearest(spawnPos);
            return sampledInfo.position;
        }

        public abstract void StartSpawn();

        protected void StartCallback() => OnStarted();
        protected void FinishCallback() => OnFinished();
    }
}
