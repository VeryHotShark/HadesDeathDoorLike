using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class ActorComponent<T> : ChildBehaviour<T> where T : Actor {
        protected new T Parent => (T) base.Parent;
        
        public virtual void OnActorInitialized(T actor) { }
    }
}
