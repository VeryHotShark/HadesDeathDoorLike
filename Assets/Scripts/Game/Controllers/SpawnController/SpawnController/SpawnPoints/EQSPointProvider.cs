using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VHS;

namespace VHS {
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
