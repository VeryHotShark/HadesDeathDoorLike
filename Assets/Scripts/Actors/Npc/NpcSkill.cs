using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class NpcSkill : Skill<Npc> {
        public override void StartTarget() {
            Owner.AIAgent.ResetPath();
            Owner.AIAgent.isStopped = true;
            Owner.AIAgent.RVO.locked = true;
        }

        public override void FinishSkill() {
            Owner.AIAgent.isStopped = false;
            Owner.AIAgent.RVO.locked = false;
        }
    }
}