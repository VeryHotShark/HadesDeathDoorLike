using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class PlayerSkill : Skill<Player> {
        public Timer _cooldown = new Timer(1.0f);

        public bool DuringCooldown => _cooldown.IsActive;
        public float CooldownRatio => _cooldown.NormalizedTime.OneMinus();

        public override void OnSkillStart() => _cooldown.Start();

        public override bool CanCastSkill() => !DuringCooldown;

        public override void OnReset() => _cooldown.Reset();

        public override void OnInitialize() {
            _cooldown.OnStart = () => Owner.OnSkillCooldownStart(this);
            _cooldown.OnUpdate = () => Owner.OnSkillCooldownUpdate(this);
            _cooldown.OnEnd = () => Owner.OnSkillCooldownEnd(this);   
        }
    }
}
