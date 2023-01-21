using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.RVO;
using UnityEngine;

namespace VHS {
    /// <summary>
    /// Wrapper for RICH AI
    /// </summary>
    public class AIAgent : RichAI {
        public RVOController RVO => rvoController;  
        
        public void ResetPath() {
            enabled = false;
            enabled = true;
            rvoController.enabled = false;
            rvoController.enabled = true;
            desiredVelocityWithoutLocalAvoidance = Vector3.zero;
        }

        public void Stop() {
            isStopped = true;
            canSearch = false;
            rvoController.locked = true;
        }

        public void Resume() {
            canSearch = true;
            isStopped = false;
            rvoController.locked = false;
        }
    }
}
