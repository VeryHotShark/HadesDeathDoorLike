using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace VHS {
    public class Npc : BaseBehaviour {
        
        private RichAI _richAI;

        private void Awake() {
            _richAI = GetComponent<RichAI>();
            _richAI.destination = Vector3.zero;
        }
    }
}