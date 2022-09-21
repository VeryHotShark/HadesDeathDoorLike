using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace VHS {
    /// <summary>
    /// Wrapper for RICH AI
    /// </summary>
    public class AIAgent : RichAI {
        public void ResetPath() {
            ClearPath();
        }
    }
}
