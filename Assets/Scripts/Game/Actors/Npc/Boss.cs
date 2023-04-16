using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [Serializable]
    public struct BossStage {
        public int HPThreshold;
    }
    
    public class Boss : Npc {
        [SerializeField] private BossStage[] _stages = Array.Empty<BossStage>();

        private int _currentStage = 0;
        public int CurrentStage => _currentStage;

        protected override void Enable() => OnHealthChanged += OnHealthChangedCallback;
        protected override void Disable() => OnHealthChanged -= OnHealthChangedCallback;

        private void OnHealthChangedCallback(HitPoints hitPoints) {
            if(_stages.Length == 0)
                return;

            for (var i = _stages.Length - 1; i >= 0; i--) {
                var bossStage = _stages[i];
                if (hitPoints.Current <= bossStage.HPThreshold) {
                    _currentStage = i + 1;
                    break;
                }
            }
        }
    }
}
