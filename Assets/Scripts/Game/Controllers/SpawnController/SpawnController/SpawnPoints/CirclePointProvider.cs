using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class CirclePointProvider : ISpawnPointProvider {
        [SerializeField, MinValue(0)] private float _radius = 5f;
        [SerializeField, MinValue(0)] private int _points = 2;
        public Transform Transform { get; set; }
        public void OnDrawGizmos(Transform transform) {
            DebugExtension.DrawCircle(transform.position, Color.cyan, _radius);

            float theta = Mathf.PI * 2.0f / _points;

            for (int i = 0; i < _points; i++) {
                float angle = i * theta;
                Vector3 offset = _radius * new Vector3(Mathf.Cos(angle), 0.0f, Mathf.Sin(angle));
                Gizmos.DrawSphere(transform.position + offset, 0.5f);
            }
        }

        public Vector3 ProvidePoint() {
            return Transform.position;
        }
    }
}
