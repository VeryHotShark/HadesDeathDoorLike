using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    // TODO Check if this is neccesary
    public interface IActor {
        Vector3 FeetPosition { get; }
        Vector3 CenterOfMass { get; }
        Vector3 Forward { get; }
        
        HitData LastHitData { get; set; }
        HitData LastDealtData { get; set; }

        GameObject GameObject { get; }
        
        bool IsAlive { get; }

        void OnMyAttackParried(HitData hitData);
    }
}
