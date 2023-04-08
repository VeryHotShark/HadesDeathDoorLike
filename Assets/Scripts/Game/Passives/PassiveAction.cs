using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [Serializable]
    public abstract class PassiveAction {
        protected Actor _actor;
        
        public virtual void Init(Actor actor) => _actor = actor;

        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
        
        public abstract void PerformAction();
    }

    public abstract class PassiveAction<T> : PassiveAction where T : Actor {
        protected new T _actor;

        public override void Init(Actor actor) => _actor = (T)actor;
    }
}
