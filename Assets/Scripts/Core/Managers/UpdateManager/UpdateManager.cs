using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : Singleton<UpdateManager> {
        private class CustomUpdateListener {
            public CustomUpdateListener() {
                UpdateTimer = 0.0f;
                Listeners = new List<ICustomUpdateListener>();
            }
            
            public float UpdateTimer;
            public readonly List<ICustomUpdateListener> Listeners;
        }

        private const float SLOW_UPDATE_INTERVAL = 0.2f;

        private readonly List<IFixedUpdateListener> _fixedUpdateListeners = new();
        private readonly List<ILateUpdateListener> _lateUpdateListeners = new();
        private readonly List<ISlowUpdateListener> _slowUpdateListeners = new();
        private readonly List<IUpdateListener> _updateListeners = new();

        private readonly Dictionary<float, CustomUpdateListener> _customUpdateDict = new();
        
        private float _slowUpdateTimeElapsed;

        private void Update() {
            for (int i = 0; i < _updateListeners.Count; i++)
                _updateListeners[i].OnUpdate(Time.deltaTime);

            SlowUpdate();
            CustomUpdate();
        }

        private void CustomUpdate() {
            foreach (KeyValuePair<float, CustomUpdateListener> kvp in _customUpdateDict) {
                if (_customUpdateDict[kvp.Key].UpdateTimer >= kvp.Key) {
                    for (int i = 0; i < _customUpdateDict[kvp.Key].Listeners.Count; i++)
                        _customUpdateDict[kvp.Key].Listeners[i].OnCustomUpdate(kvp.Key);
                    
                    
                    _customUpdateDict[kvp.Key].UpdateTimer = 0.0f;
                }
                else
                    _customUpdateDict[kvp.Key].UpdateTimer += Time.deltaTime;
            }
        }

        private void SlowUpdate() {
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

        public static void RemoveUpdateListener(IUpdateListener listener) => Instance._updateListeners.Remove(listener);

        public static void AddFixedUpdateListener(IFixedUpdateListener listener) {
            if (!Instance._fixedUpdateListeners.Contains(listener))
                Instance._fixedUpdateListeners.Add(listener);
        }

        public static void RemoveFixedUpdateListener(IFixedUpdateListener listener) => Instance._fixedUpdateListeners.Remove(listener);

        public static void AddSlowUpdateListener(ISlowUpdateListener listener) {
            if (!Instance._slowUpdateListeners.Contains(listener))
                Instance._slowUpdateListeners.Add(listener);
        }

        public static void RemoveSlowUpdateListener(ISlowUpdateListener listener) => Instance._slowUpdateListeners.Remove(listener);

        public static void AddLateUpdateListener(ILateUpdateListener listener) {
            if (!Instance._lateUpdateListeners.Contains(listener))
                Instance._lateUpdateListeners.Add(listener);
        }

        public static void RemoveLateUpdateListener(ILateUpdateListener listener) => Instance._lateUpdateListeners.Remove(listener);

        public static void AddCustomUpdateListener(float updateRate, ICustomUpdateListener listener) {
            if (!Instance._customUpdateDict.ContainsKey(updateRate)) {
                CustomUpdateListener customUpdateListener = new CustomUpdateListener();
                customUpdateListener.Listeners.Add(listener);
                Instance._customUpdateDict.Add(updateRate, customUpdateListener);
            }
            else if(!Instance._customUpdateDict[updateRate].Listeners.Contains(listener))
                Instance._customUpdateDict[updateRate].Listeners.Add(listener);
        }
        
        public static void RemoveCustomUpdateListener(float updateRate, ICustomUpdateListener listener) {
            if (Instance._customUpdateDict.ContainsKey(updateRate)) 
                Instance._customUpdateDict[updateRate].Listeners.Remove(listener);
        }
}
