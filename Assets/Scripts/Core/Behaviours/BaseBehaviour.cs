using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VHS {
    public class BaseBehaviour : MonoBehaviour, ICustomUpdateListener {
        [FoldoutGroup("Behaviour Properties"),SerializeField] private bool _customUpdate = false;
        [FoldoutGroup("Behaviour Properties"),SerializeField, EnableIf("_customUpdate")] private float _updateRate = 0.2f;
        
        private bool _transformCached;
        private bool _waitForEndOfFrameCached;
        private bool _waitForFixedUpdateCached;
        
        private Transform _transform;
        private WaitForEndOfFrame _frame;
        private WaitForFixedUpdate _fixedFrame;

        protected WaitForEndOfFrame Frame => _frame ??= new WaitForEndOfFrame();
        protected WaitForFixedUpdate FixedFrame => _fixedFrame ??= new WaitForFixedUpdate();
            
        public new Transform transform => _transform ??= GetComponent<Transform>();

        private void OnEnable() {
            if(_customUpdate)
                UpdateManager.AddCustomUpdateListener(_updateRate, this);

            Enable();
        }

        private void OnDisable() {
            if(_customUpdate)
                UpdateManager.RemoveCustomUpdateListener(_updateRate, this);

            Disable();
        }

        protected virtual void Enable() { }
        protected virtual void Disable() { }
        
        public virtual void OnCustomUpdate(float deltaTime) { }
    }
}