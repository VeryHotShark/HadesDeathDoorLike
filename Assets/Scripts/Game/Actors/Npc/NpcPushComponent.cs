using MEC;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace VHS {
    public class NpcPushComponent : NpcComponent {
        [FoldoutGroup("Push Properties"),SerializeField] private float _pushDistance;
        [FoldoutGroup("Push Properties"),SerializeField] private float _pushDuration;
        [FoldoutGroup("Push Properties"),SerializeField] private float _recoveryDuration;

        private float _lastPushSpeed;
        private Vector3 _lastPushDirection;
        private CoroutineHandle _pushBackRoutine;
        
        protected override void Enable() => Parent.OnHit += OnHit;
        protected override void Disable() => Parent.OnHit -= OnHit;
        
        private void OnHit(HitData hitData) => Push(_pushDuration, _pushDistance, hitData.direction);

        public void Push(float duration, float distance, Vector3 direction) {
            Timing.KillCoroutines(_pushBackRoutine);
            
            AIAgent.SetPath(null);
            AIAgent.isStopped = true;
            Parent.SetState(NpcState.Recovery);

            _lastPushSpeed = distance / duration;
            _lastPushDirection = direction.Flatten();
            _pushBackRoutine = Timing.CallContinuously(duration, MoveAgent, PushEnd);
        }
        
        private void MoveAgent() => AIAgent.Move(_lastPushDirection * (_lastPushSpeed * Time.deltaTime));

        private void PushEnd() {
            AIAgent.isStopped = false;
            Timing.CallDelayed(_recoveryDuration, () => Parent.SetState(NpcState.Default));
        }

    }
}
