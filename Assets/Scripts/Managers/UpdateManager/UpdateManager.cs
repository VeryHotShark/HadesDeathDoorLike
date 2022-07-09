using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : Singleton<UpdateManager> {

        private const float SLOW_UPDATE_INTERVAL = 0.2f;

        private readonly List<IFixedUpdateListener> _fixedUpdateListeners = new List<IFixedUpdateListener>();
        private readonly List<ILateUpdateListener> _lateUpdateListeners = new List<ILateUpdateListener>();
        private readonly List<ISlowUpdateListener> _slowUpdateListeners = new List<ISlowUpdateListener>();
        private readonly List<IUpdateListener> _updateListeners = new List<IUpdateListener>();

        private float _slowUpdateTimeElapsed;

        private void Update() {
            for (int i = 0; i < _updateListeners.Count; i++)
                _updateListeners[i].OnUpdate(Time.deltaTime);

            if (_slowUpdateTimeElapsed >= SLOW_UPDATE_INTERVAL) {
                for (int i = 0; i < _slowUpdateListeners.Count; i++)
                    _slowUpdateListeners[i].OnSlowUpdate(SLOW_UPDATE_INTERVAL);

                _slowUpdateTimeElapsed = 0.0f;
            }
            else 
                _slowUpdateTimeElapsed += Time.deltaTime;
        }

        private void FixedUpdate() {
            for (int i = 0; i < _fixedUpdateListeners.Count; i++)
                _fixedUpdateListeners[i].OnFixedUpdate(Time.fixedDeltaTime);
        }

        private void LateUpdate() {
            for (int i = 0; i < _lateUpdateListeners.Count; i++)
                _lateUpdateListeners[i].OnLateUpdate(Time.deltaTime);
        }

        public static void AddUpdateListener(IUpdateListener listener) {
            if (!Instance._updateListeners.Contains(listener))
                Instance._updateListeners.Add(listener);
        }

        public static void RemoveUpdateListener(IUpdateListener listener) {
            if (Instance._updateListeners.Contains(listener))
                Instance._updateListeners.Remove(listener);
        }

        public static void AddFixedUpdateListener(IFixedUpdateListener listener) {
            if (!Instance._fixedUpdateListeners.Contains(listener))
                Instance._fixedUpdateListeners.Add(listener);
        }

        public static void RemoveFixedUpdateListener(IFixedUpdateListener listener) {
            if (Instance._fixedUpdateListeners.Contains(listener))
                Instance._fixedUpdateListeners.Remove(listener);
        }

        public static void AddSlowUpdateListener(ISlowUpdateListener listener) {
            if (!Instance._slowUpdateListeners.Contains(listener))
                Instance._slowUpdateListeners.Add(listener);
        }

        public static void RemoveSlowUpdateListener(ISlowUpdateListener listener) {
            if (Instance._slowUpdateListeners.Contains(listener))
                Instance._slowUpdateListeners.Remove(listener);
        }

        public static void AddLateUpdateListener(ILateUpdateListener listener) {
            if (!Instance._lateUpdateListeners.Contains(listener))
                Instance._lateUpdateListeners.Add(listener);
        }

        public static void RemoveLateUpdateListener(ILateUpdateListener listener) {
            if (Instance._lateUpdateListeners.Contains(listener))
                Instance._lateUpdateListeners.Remove(listener);
        }
}
