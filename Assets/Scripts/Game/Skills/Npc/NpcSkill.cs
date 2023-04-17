using System;
using System.Collections;

namespace VHS {
    [Serializable]
    public class NpcSkill : Skill<Npc> {
        public override void OnCastStart() {
            Owner.AIAgent.ResetPath();
            Owner.AIAgent.Stop();
            Owner.SetState(NpcState.Attacking);
        }
        
        public override void OnAbort() => FinishSkill(false);
    }
}
