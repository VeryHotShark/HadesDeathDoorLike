using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public interface IActor {
        Vector3 FeetPosition { get; }
        Vector3 CenterOfMass { get; }
        Vector3 Forward { get; }
        
        bool IsAlive { get; }
    }
}
