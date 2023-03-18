using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class PassiveActionIncrementHealth : PassiveAction {
        public int _amount = 1;
        
        public override void PerformAction() => _actor.HitPoints.Add(_amount);
    }
}
