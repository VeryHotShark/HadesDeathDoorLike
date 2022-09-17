using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VHS {
    public class BaseBehaviour : MonoBehaviour, ICustomUpdateListener {
        [FoldoutGroup("Debug Properties"), ShowInInspector] private static bool DISABLE_ALL_LOGS = false;
        [FoldoutGroup("Debug Properties"), SerializeField, DisableIf("DISABLE_ALL_LOGS")] private bool _instanceLogs = true;
        [FoldoutGroup("Debug Properties"), SerializeField, ColorUsage(false,false)] private Color _color = Color.white;
        
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

        protected void Log(params object[] msg) {
            if(!DISABLE_ALL_LOGS || !_instanceLogs)
                return;

            VHSLogger.DoLog(Debug.Log, GetType().Name , gameObject, _color, msg );
        }
    }
}