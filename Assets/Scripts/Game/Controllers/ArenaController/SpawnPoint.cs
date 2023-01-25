using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class SpawnPoint : BaseBehaviour {
        [SerializeField] private Timer _cooldown;
        [SerializeField] private Npc[] _npcsToSpawn;

        public Npc[] Npcs => _npcsToSpawn;
        
        public bool IsValid() => !_cooldown.IsActive;

        public void OnSelected() {
            if(_cooldown.Duration > 0.0f)
                _cooldown.Start();
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
    }
}
