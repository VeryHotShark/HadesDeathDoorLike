using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class NpcSkill : Skill<Npc> {
        public override void StartTarget() {
            Owner.RichAI.SetPath(null);
            Owner.RichAI.isStopped = true;
        }

        public override void FinishSkill() {
            Owner.RichAI.isStopped = false;
        }
    }
}