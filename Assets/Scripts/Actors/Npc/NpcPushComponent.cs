using MEC;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VHS {
    public class NpcPushComponent : NpcComponent {
        [FoldoutGroup("Push Properties"),SerializeField] private float _pushSpeed;
        [FoldoutGroup("Push Properties"),SerializeField] private float _pushDuration;
        [FoldoutGroup("Push Properties"),SerializeField] private float _recoveryDuration;

        private CoroutineHandle _pushBackRoutine;
        
        protected override void Enable() => Parent.OnHit += OnHit;
        protected override void Disable() => Parent.OnHit -= OnHit;

        private void OnHit(HitData hitData) {
            Timing.KillCoroutines(_pushBackRoutine);
            
            RichAI.SetPath(null);
            RichAI.isStopped = true;
            Parent.SetState(NpcState.Recovery);

            Vector3 flattenedDirection = hitData.direction.Flatten();
            _pushBackRoutine = Timing.CallContinuously(flattenedDirection, _pushDuration, PushBack, PushBackEnd);
        }
        
        private void PushBack(Vector3 direction) {
            RichAI.Move(direction * (_pushSpeed * Time.deltaTime));
        }
        
        private void PushBackEnd(Vector3 direction) {
            RichAI.isStopped = false;
            Timing.CallDelayed(_recoveryDuration, () => Parent.SetState(NpcState.Default));
        }

    }
}
