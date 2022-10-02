using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class NpcSkill : Skill<Npc> {
        public override void StartTarget_Hook() {
            Owner.AIAgent.ResetPath();
            Owner.AIAgent.isStopped = true;
            Owner.AIAgent.RVO.locked = true;
        }

        public override void FinishSkill_Hook() {
            Owner.AIAgent.isStopped = false;
            Owner.AIAgent.RVO.locked = false;
        }

        public override void Abort() {
            FinishSkill();
        }
    }
}