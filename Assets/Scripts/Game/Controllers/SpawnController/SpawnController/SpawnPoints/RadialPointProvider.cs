using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VHS {
    [Serializable]
    public class RadialPointProvider : ISpawnPointProvider {
        [SerializeField] private float _radius = 3f;
        
        public Transform Transform { get; set; }

        public void OnDrawGizmos(Transform transform) {
            DebugExtension.DrawCircle(transform.position, Color.red, _radius);
        }

        public Vector3 ProvidePoint() {
            Vector3 randomOffset = _radius > 0 ? Random.insideUnitSphere.Flatten() * _radius : Vector3.zero;
            Vector3 spawnPos = Transform.position + randomOffset;
            NNInfo info = AstarPath.active.GetNearest(spawnPos);
            return info.position;
        }
    }
}
