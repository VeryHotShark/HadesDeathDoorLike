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
            rvoController.enabled = false;
            rvoController.enabled = true;
            enabled = false;
            enabled = true;
        }
    }
}
