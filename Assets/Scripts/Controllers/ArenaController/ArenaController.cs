using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [System.Serializable]
    public struct SpawnData {
        public Npc NpcPrefab;
        public AnimationCurve SpawnCurve;
    }
    
    public class ArenaController : ChildBehaviour<GameController>, ISlowUpdateListener {
        [SerializeField] private float _spawnInterval = 1.0f;
        [SerializeField] private float _arenaDuration = 30.0f;

        [Space] 
        [SerializeField] private SpawnData[] _spawnsData;

        private float _timer;

        protected override void Enable() {
            base.Enable();
            UpdateManager.AddSlowUpdateListener(this);
        }

        protected override void Disable() {
            base.Disable();
            UpdateManager.RemoveSlowUpdateListener(this);

        }

        public void OnSlowUpdate(float deltaTime) {
            _timer += deltaTime;
            ConsoleProDebug.Watch("Arena Timer", _timer.ToString());

            foreach (SpawnData spawnData in _spawnsData) {
                if(spawnData.SpawnCurve.length == 0)
                    continue;
                
                Keyframe keyframe = spawnData.SpawnCurve[0];

                if (_timer > keyframe.time) {
                    spawnData.SpawnCurve.RemoveKey(0);
                    Log(keyframe.time + " " + keyframe.value);
                }
            }
        }
    }
}