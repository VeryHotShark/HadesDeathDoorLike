using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VHS;


namespace VHS {
    public class ChildBehaviour<T> : BaseBehaviour where T : BaseBehaviour {
        private T _parent = default;
        protected T Parent => _parent ??= GetComponentInParent<T>();
    }
}
        