using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class GridPointProvider : ISpawnPointProvider {
        [SerializeField] private Vector2Int _points;
        [SerializeField] private float _spacing = 1.0f;

        private Vector3 _start;
        private Vector3 _end;
        
        public Transform Transform { get; set; }

        public List<Vector3> _gridPoints;
        
        public void Initialize(Transform transform) {
            Transform = transform;
            
            float halfLengthX = _points.x / 2f;
            float halfLengthY = _points.y / 2f;

            Vector3 startOffsetX = transform.right * halfLengthX * _spacing;
            Vector3 startOffsetZ = transform.forward * halfLengthY * _spacing;
            
            _start = transform.position - startOffsetX - startOffsetZ;
            _end = transform.position + startOffsetX + startOffsetZ;

            Vector3 difference = _end - _start;
            
            for (int x = 0; x < _points.x; x++) {
                float tX = (float)x / (_points.x - 1);
                float xOffset = Mathf.Lerp(0.0f,  Mathf.Abs(difference.x), tX);

                for (int y = 0; y < _points.y; y++) {
                    float tY = (float)y / (_points.y - 1);
                    float zOffset = Mathf.Lerp(0.0f,  Mathf.Abs(difference.z), tY);

                    Vector3 point = _start + new Vector3(xOffset, 0, zOffset);
                    _gridPoints.Add(point);
                }
            }
        }
        
        public void OnDrawGizmos(Transform transform) {
            Gizmos.color = Color.red;

            float halfLengthX = _points.x / 2f;
            float halfLengthY = _points.y / 2f;

            Vector3 startOffsetX = transform.right * halfLengthX * _spacing;
            Vector3 startOffsetZ = transform.forward * halfLengthY * _spacing;
            
            _start = transform.position - startOffsetX - startOffsetZ;
            _end = transform.position + startOffsetX + startOffsetZ;

            Vector3 difference = _end - _start;
            
            for (int x = 0; x < _points.x; x++) {
                float tX = (float)x / (_points.x - 1);
                float xOffset = Mathf.Lerp(0.0f,  Mathf.Abs(difference.x), tX);

                for (int y = 0; y < _points.y; y++) {
                    float tY = (float)y / (_points.y - 1);
                    float zOffset = Mathf.Lerp(0.0f,  Mathf.Abs(difference.z), tY);

                    Vector3 point = _start + new Vector3(xOffset, 0, zOffset);
                    Gizmos.DrawSphere(point, 0.5f);
                }
            }
        }

        public Vector3 ProvidePoint() => _gridPoints.GetRandomElement();
    }
}
