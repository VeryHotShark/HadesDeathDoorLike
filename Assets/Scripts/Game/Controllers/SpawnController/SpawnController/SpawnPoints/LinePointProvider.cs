using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VHS {
    [Serializable]
    public class LinePointProvider : ISpawnPointProvider {
        [SerializeField, Range(2, 10)] private int _points = 2;
        [SerializeField] private float _length = 5f;
        /*[DraggablePoint]*/ private Vector3 _start;
        /*[DraggablePoint]*/ private Vector3 _end;
        
        public Transform Transform { get; set; }

        public void OnDrawGizmos(Transform transform) {
            float halfLength = _length / 2f;
            _start = transform.position - transform.right * halfLength;
            _end = transform.position + transform.right * halfLength;
            
            for (int i = 0; i < _points; i++) {
                float t = (float)i / (_points - 1);
                Vector3 point = Vector3.Lerp(_start, _end, t);
                Gizmos.DrawSphere(point, 0.5f);
            }
            
            Gizmos.DrawLine(_start,_end);
        }

        public Vector3 ProvidePoint() {
            return Transform.position;
        }
    }
}
