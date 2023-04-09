using System;
using System.Collections.Generic;
using Pathfinding;
using Sirenix.OdinInspector;
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
        public EnemyID EnemyID;
        public AnimationCurve SpawnCurve;
    }
    
    public class SpawnController : ChildBehaviour<GameController>, ISlowUpdateListener {
        [TitleGroup("Spawn Settings")]
        [SerializeField] private GameObject _spawnVFX;
        [SerializeField] private SpawnStartType _spawnStartType = SpawnStartType.Immediate;
        [SerializeReference] private SpawnHandler _spawnHandler;
        
        [TitleGroup("EQS & Random Spawn")]
        [SerializeField] private EQSPointProvider _eqsPointProvider;
        [SerializeField] private float _minDistance = 5.0f;
        [SerializeField] private Vector2 _randomSpawnRange = new Vector2(4.0f,10.0f);

        public event Action OnStarted = delegate { };
        public event Action OnFinished = delegate { };

        private Collider[] _overlapNpcs = new Collider[5];
        
        protected float _minDistanceSqr;
        
        protected List<Npc> _aliveNpcs = new();
        protected Dictionary<EnemyID, List<SpawnPoint>> _spawnPointsDict = new();

        public List<Npc> AliveNpcs => _aliveNpcs;

        private void Awake() {
            _minDistanceSqr = _minDistance.Square();
            
            InitializeSpawnPoints();

            _spawnHandler.Init(this);
        }

        private void InitializeSpawnPoints() {
            SpawnPoint[] allSpawnPoints = GetComponentsInChildren<SpawnPoint>();

            foreach (SpawnPoint spawnPoint in allSpawnPoints) {
                foreach (EnemyID enemyID in spawnPoint.Npcs) {
                    if (_spawnPointsDict.TryGetValue(enemyID, out List<SpawnPoint> spawnPoints))
                        spawnPoints.Add(spawnPoint);
                    else
                        _spawnPointsDict.Add(enemyID, new List<SpawnPoint> { spawnPoint });
                }
            }
        }

        protected override void Disable() {
            foreach (Npc npc in _aliveNpcs) 
                npc.OnDeath -= OnNpcDeath;
            
            UpdateManager.RemoveSlowUpdateListener(this);
        }
        
        private void OnNpcDeath(IActor actor) {
            Npc npc = actor as Npc;
            npc.OnDeath -= OnNpcDeath;
            _aliveNpcs.Remove(npc);
            
            _spawnHandler.OnNpcDeath(npc);
        }

        public void KillAliveNpcs() {
            for (int i = _aliveNpcs.Count - 1; i >= 0; i--) {
                Npc aliveNpc = _aliveNpcs[i];
                aliveNpc.Kill(Parent.Player);
            }
        }

        public Npc SpawnEnemy(EnemyID enemyID, Vector3 position) {
            Npc spawnedNpc = Instantiate(enemyID.Prefab, position, Quaternion.identity);
            spawnedNpc.OnDeath += OnNpcDeath;
            _aliveNpcs.Add(spawnedNpc);
            return spawnedNpc;
        }
        
        public  Vector3 GetSpawnPosition(EnemyID enemyID) {
            Vector3? spawnPointPosition = GetValidSpawnPointPosition(enemyID);
                
            if (spawnPointPosition != null)
                return spawnPointPosition.Value;

            var eqsPoint = GetEQSPoint();

            if (eqsPoint != null)
                return eqsPoint.Value;
            
            return GetRandomPosition();
        }

        private Vector3? GetValidSpawnPointPosition(EnemyID enemyID) {
            List<SpawnPoint> validSpawnPoints = new List<SpawnPoint>();
            
            foreach (SpawnPoint spawnPoint in _spawnPointsDict[enemyID]) {
                if (spawnPoint.IsValid()) {
                    float distanceToTarget = Parent.Player.FeetPosition.DistanceSquaredTo(spawnPoint.transform.position);
                    
                    if(distanceToTarget > _minDistanceSqr)
                        validSpawnPoints.Add(spawnPoint);
                }
            }

            return FilterProvidedPoints(validSpawnPoints);
        }

        private Vector3? FilterProvidedPoints(List<SpawnPoint> validSpawnPoints) {
            if (validSpawnPoints.Count == 0)
                return null;
            
            do {
                int randomIndex = Random.Range(0, validSpawnPoints.Count);
                SpawnPoint potentialSpawnPoint = validSpawnPoints[randomIndex];
                Vector3 providedPoint = potentialSpawnPoint.ProvidePoint();

                int hitCount = Physics.OverlapSphereNonAlloc(providedPoint, 1.0f, _overlapNpcs, LayerManager.Masks.ACTORS);

                if (hitCount == 0) 
                    return providedPoint;

                validSpawnPoints.RemoveAt(randomIndex);
            } while (validSpawnPoints.Count > 0);

            return null;
        }

        private Vector3? GetEQSPoint() {
            Vector3 point = _eqsPointProvider.ProvidePoint();

            if (point != Vector3.zero)
                return point;
            
            return null;
        }

        private Vector3 GetRandomPosition() {
            float randomDistance = _randomSpawnRange.Random();
            Vector3 randomOffset = Random.insideUnitSphere.Flatten() * randomDistance;
            Vector3 spawnPos = Parent.Player.FeetPosition + randomOffset;
            NNInfo sampledInfo = AstarPath.active.GetNearest(spawnPos);
            return sampledInfo.position;
        }

        public void StartSpawn() {
            OnStarted();
            _spawnHandler.Start();
            UpdateManager.AddSlowUpdateListener(this);
        }

        public void OnSlowUpdate(float deltaTime) {
            if(Parent.Player != null)
                _spawnHandler.OnTick(deltaTime);
        }

        public void FinishCallback() {
            OnFinished();
            UpdateManager.RemoveSlowUpdateListener(this);
        }
    }
}
