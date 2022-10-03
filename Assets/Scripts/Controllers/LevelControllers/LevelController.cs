using System;
using System.Configuration;

namespace VHS {
    public class LevelController : BaseBehaviour {
        public event Action OnLevelStart = delegate { };
        public event Action OnLevelEnd = delegate { };
        
        protected LevelControllerModule[] _modules;

        private void Awake() {
            _modules = GetComponentsInChildren<LevelControllerModule>();
            GetComponents();
            Initialize();
        }

        /// <summary>
        /// Called in Start so all sub modules can be subscribed to appropriate delegates;
        /// </summary>
        private void Start() {
            StartLevel();
        }

        protected sealed override void Enable() {
            foreach (LevelControllerModule module in _modules) 
                module.MyEnable();

            MyEnable();
        }
        
        protected sealed override void Disable() {
            foreach (LevelControllerModule module in _modules) 
                module.MyDisable();
            
            MyDisable();
        }

        protected virtual void MyEnable() { }
        protected virtual void MyDisable() { }
        protected virtual void GetComponents() { }
        protected virtual void Initialize() { }

        protected virtual void StartLevel() => OnLevelStart();

        protected virtual void EndLevel() => OnLevelEnd();
    }
}
