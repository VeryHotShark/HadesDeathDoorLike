using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public interface ISpawnPointProvider {
        void Initialize(Transform transform) {
            Transform = transform;
        }
        
        Transform Transform { get; set; }

        void OnDrawGizmos(Transform transform);
        Vector3 ProvidePoint();
    }
}
