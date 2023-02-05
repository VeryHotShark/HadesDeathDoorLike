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

        private float _rvoPriorityBeforeStop;
        
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
            _rvoPriorityBeforeStop = rvoController.priority;
            rvoController.priority = 1.0f;
            rvoController.locked = true;
        }

        public void Resume() {
            canSearch = true;
            isStopped = false;
            rvoController.priority = _rvoPriorityBeforeStop;
            rvoController.locked = false;
        }
    }
}
