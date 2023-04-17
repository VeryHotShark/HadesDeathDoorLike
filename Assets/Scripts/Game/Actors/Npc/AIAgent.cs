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
            Disable();
            Enable();
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

        public void Disable() {
            ClearPath();
            enabled = false;
            rvoController.enabled = false;
        }
        
        public void Enable() {
            enabled = true;
            rvoController.enabled = true;
        }
    }
}
