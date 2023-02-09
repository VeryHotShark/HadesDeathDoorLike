using System;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class EQSPointProvider : ISpawnPointProvider
    {
        public Transform Transform { get; set; }
        
        public void OnDrawGizmos(Transform transform) {
            
        }

        public Vector3 ProvidePoint() {
            return Vector3.zero;
        }
    }
}
