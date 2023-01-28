using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

namespace VHS {
    public enum SpawnStartType {
        Aggro,
        Trigger,
        Immediate
    }
    
    [Serializable]
    public struct SpawnData {
        public Npc NpcPrefab;
        public AnimationCurve SpawnCurve;
    }
    
    public abstract class SpawnController : ChildBehaviour<GameController> {
        [SerializeField] private SpawnStartType _spawnStartType = SpawnStartType.Immediate;

        public event Action OnStarted = delegate { };
        public event Action OnFinished = delegate { };

        private void Awake() {
            
        }

        public abstract void StartSpawn();

        protected void StartCallback() => OnStarted();
        protected void FinishCallback() => OnFinished();
    }
}
