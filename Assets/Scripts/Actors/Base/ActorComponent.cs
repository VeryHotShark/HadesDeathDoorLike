using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class ActorComponent<T> : ChildBehaviour<T> where T : Actor {
        public virtual void OnActorInitialized(T actor) { }
    }
}
