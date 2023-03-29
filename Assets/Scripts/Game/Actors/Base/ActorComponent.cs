using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class ActorComponent<T> : ChildBehaviour<T> where T : Actor {
        [SerializeField] private int priority = 0;
        
        protected new T Parent => (T) base.Parent;
        
        public virtual void OnActorInitialized(T actor) { }
    }
}
