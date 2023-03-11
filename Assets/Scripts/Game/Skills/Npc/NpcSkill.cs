using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class NpcSkill : Skill<Npc> {
        public override void OnAbort() => FinishSkill(false);
    }
}
