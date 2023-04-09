using System;
using System.Configuration;

namespace VHS {
    public class LevelController : BaseBehaviour {
        public event Action OnLevelStart = delegate { };
        public event Action OnLevelEnd = delegate { };
        
        private void Awake() {
            GetComponents();
            Initialize();
        }

        /// <summary>
        /// Check if Submodules can Subscrive to OnLevelStart Event
        /// </summary>
        private void Start() => StartLevel();

        protected virtual void GetComponents() { }
        protected virtual void Initialize() { }

        protected virtual void StartLevel() => OnLevelStart();
        protected virtual void EndLevel() => OnLevelEnd();
    }
}
