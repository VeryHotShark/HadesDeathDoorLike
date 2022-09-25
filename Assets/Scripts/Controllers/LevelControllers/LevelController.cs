using System;

namespace VHS {
    public class LevelController : BaseBehaviour {
        public event Action OnLevelStart = delegate { };
        public event Action OnLevelEnd = delegate { };

        private void Start() {
            StartLevel();
        }

        protected virtual void StartLevel() {
            OnLevelStart();
        }

        protected virtual void EndLevel() {
            OnLevelEnd();
        }
    }
}
