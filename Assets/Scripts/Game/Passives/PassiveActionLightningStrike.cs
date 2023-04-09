using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class PassiveActionLightningStrike : PassiveAction<Actor> {
        public int damage = 1;
        public float radius = 5.0f;
        public ParticleController particle;
    
        public override void PerformAction() {
            // Physics.OverlapSphere(_actor.FeetPosition)
        }
    }
}
