using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class PassiveActionIncrementStamina : PassiveAction<Player> {
        public int _amount = 1;
        
        public override void PerformAction() => _owner.WeaponController.WeaponRange.ModifyCurrentAmmo(_amount);
    }
}
