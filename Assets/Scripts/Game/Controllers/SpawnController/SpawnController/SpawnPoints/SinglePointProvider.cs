using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class SinglePointProvider : ISpawnPointProvider {
        public Transform Transform { get; set; }

        public Vector3 ProvidePoint() {
            return Transform.position;
        }

        public void OnDrawGizmos(Transform transform) {
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
    }
}