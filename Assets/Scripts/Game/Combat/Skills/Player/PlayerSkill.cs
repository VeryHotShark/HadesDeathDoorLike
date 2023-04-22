using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class PlayerSkill : Skill<Player> {
        public Timer _cooldown = new Timer(1.0f);

        public bool DuringCooldown => _cooldown.IsActive;
    }
}
