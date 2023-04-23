using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class StatusIce : Status {
        public override void OnApplied() {
            _npc.AIAgent.Stop();
            _npc.BehaviourTreeOwner.PauseBehaviour();
        }

        public override void OnRemoved() {
            _npc.AIAgent.Resume();
            _npc.BehaviourTreeOwner.StartBehaviour();
        }
    }
}
