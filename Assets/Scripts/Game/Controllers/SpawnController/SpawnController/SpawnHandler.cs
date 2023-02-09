using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [Serializable]
    public abstract class SpawnHandler {
        protected float _timer;
        protected SpawnController _spawnController;
        
        public virtual void Init(SpawnController spawnController) => _spawnController = spawnController;
        public virtual void Start() { }
        
        public virtual void OnTick(float dt) {
            _timer += dt;
            ConsoleProDebug.Watch("Spawn Timer", _timer.ToString());
        }
        
        public virtual void Finish() => _spawnController.FinishCallback();

        public virtual void OnNpcDeath(Npc npc) { }
    }
}
