using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class Passive {
        private Actor _owner;
        public Actor Owner => _owner;
    }
    
    [Serializable]
    public class Passive<T>:  Passive where T : Actor {
        public new T Owner => (T) base.Owner;
    }
}
