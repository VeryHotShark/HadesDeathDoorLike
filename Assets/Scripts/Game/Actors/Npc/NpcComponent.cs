using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace VHS {
    /// <summary>
    /// Wrapper for Inherited Components so they wont have to Call Parent.
    /// </summary>
    public abstract class NpcComponent : ActorComponent<Npc> {
        protected AIAgent AIAgent => Parent.AIAgent;
    }
}
