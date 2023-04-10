using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VHS {
    public static class PoolManager {
        static PoolManager() => SceneManager.sceneUnloaded += OnSceneUnloaded;
        
        private static void OnSceneUnloaded(Scene scene) {
            _objectPool.Clear();
            _poolPrefabs.Clear();
        }

        private static readonly Dictionary<IPoolable, GameObject> _poolPrefabs = new ();
        private static readonly Dictionary<GameObject, Queue<IPoolable>> _objectPool = new();

        public static T Spawn<T>(T poolablePrefab) where T : IPoolable {
            return Spawn(poolablePrefab, Vector3.zero, Quaternion.identity);
        }

        public static T Spawn<T>(T poolablePrefab, Vector3 position, Quaternion rotation, Transform parent = null) where T : IPoolable {

            if (_objectPool.TryGetValue(poolablePrefab.gameObject, out Queue<IPoolable> queue)) {
                if (queue.TryDequeue(out IPoolable poolableInstance)) {
                    if (parent) {
                        Transform poolableTransform = poolableInstance.gameObject.transform;
                        poolableTransform.SetParent(parent);
                        poolableTransform.localPosition = position;
                        poolableTransform.localRotation = rotation;
                    }
                    else
                        poolableInstance.gameObject.transform.SetPositionAndRotation(position,rotation);
                    
                    poolableInstance.gameObject.SetActive(true);
                    poolableInstance.OnSpawnFromPool();
                    return (T) poolableInstance;
                }
            }

            T spawnedInstance = Object.Instantiate(poolablePrefab.gameObject, position, rotation, parent).GetComponent<T>();
            _poolPrefabs.Add(spawnedInstance, poolablePrefab.gameObject);

            spawnedInstance.OnSpawnFromPool();
            return spawnedInstance;
        }

        public static void Return(IPoolable poolable) {
            if (poolable.gameObject == null) {
                Debug.LogWarning("returned NULL poolable, doing nothing");
                return;
            }

            if (_poolPrefabs.TryGetValue(poolable, out GameObject prefab)) {
                if (_objectPool.TryGetValue(prefab, out Queue<IPoolable> queue)) {
                    if(!queue.Contains(poolable))
                        queue.Enqueue(poolable);
                }
                else {
                    Queue<IPoolable> newQueue = new (new [] {poolable} );
                    _objectPool.Add(prefab, newQueue );
                }
            }
            else {
                Debug.LogWarning($"{poolable} has NULL prefab, destroying, probably was not spawned using PoolManager");   
                Object.Destroy(poolable.gameObject);
            }
            
            poolable.OnReturnToPool();
            poolable.gameObject.transform.SetParent(null);
            poolable.gameObject.SetActive(false);
        }
    }
}