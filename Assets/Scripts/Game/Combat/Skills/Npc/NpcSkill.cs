using System;
using System.Collections;
using FlowCanvas.Nodes;

namespace VHS {
    [Serializable]
    public class NpcSkill : Skill<Npc> {
        public override void OnCastStart() {
            Owner.AIAgent.ResetPath();
            Owner.AIAgent.Stop();
            Owner.SetState(NpcState.Attacking);
        }

        public override void OnSkillFinish() {
            Owner.AIAgent.Resume();
            
            if(!Owner.IsDuringStagger)
                Owner.SetState(NpcState.Default);
        }

        public override void OnAbort() => FinishSkill(false);
    }
}
