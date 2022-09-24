using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class PlayerSpawnPoint : MonoBehaviour {
        private void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position, Vector3.one * 3.0f);
        }
    }
}
