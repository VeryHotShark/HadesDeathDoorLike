using System;
using System.Collections;
using System.Collections.Generic;
using ParadoxNotion;
using UnityEngine;

namespace VHS {
    public class PlayerSpawnPoint : MonoBehaviour {
        [SerializeField] private float _gizmoSize = 1.5f;
        private void OnDrawGizmos() {
            Gizmos.color = Color.green.WithAlpha(0.3f);
            Gizmos.DrawCube(transform.position + Vector3.up * _gizmoSize / 2f, Vector3.one * _gizmoSize);
        }
    }
}
