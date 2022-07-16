using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class BaseBehaviour : MonoBehaviour {
        private bool _transformCached;
        private bool _waitForEndOfFrameCached;
        private bool _waitForFixedUpdateCached;
        
        private Transform _transform;
        private WaitForEndOfFrame _frame;
        private WaitForFixedUpdate _fixedFrame;

        protected WaitForEndOfFrame Frame => _frame ??= new WaitForEndOfFrame();
        protected WaitForFixedUpdate FixedFrame => _fixedFrame ??= new WaitForFixedUpdate();
            
        public new Transform transform => _transform ??= GetComponent<Transform>();

    }
}