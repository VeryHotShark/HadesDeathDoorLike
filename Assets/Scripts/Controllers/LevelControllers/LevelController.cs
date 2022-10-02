using System;

namespace VHS {
    public class LevelController : BaseBehaviour {
        public event Action OnLevelStart = delegate { };
        public event Action OnLevelEnd = delegate { };

        private void Awake() {
            GetComponents();
            Initialize();
            StartLevel();
        }
        
        protected virtual void GetComponents() {}
        protected virtual void Initialize() { }

        protected virtual void StartLevel() => OnLevelStart();

        protected virtual void EndLevel() => OnLevelEnd();
    }
}
