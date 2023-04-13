using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

namespace VHS {
    public class Feedback : MonoBehaviour, IPoolable {
        [SerializeField] private bool _manualControl;
        
        private MMF_Player _mmfPlayer;
        public MMF_Player FeedbackPlayer => _mmfPlayer;
        
        public Action OnComplete = delegate { };

        private void Awake() {
            _mmfPlayer = GetComponent<MMF_Player>();
            _mmfPlayer.Initialization();
        }

        private void OnEnable() {
            if(!_manualControl)
                _mmfPlayer.Events.OnComplete.AddListener(OnFeedbackComplete);
        }

        private void OnDisable() {
            if(!_manualControl)
                _mmfPlayer.Events.OnComplete.RemoveListener(OnFeedbackComplete);
        }

        private void OnFeedbackComplete() {
            OnComplete();
            ReturnToPool();
        }

        public void ReturnToPool() => PoolManager.Return(this);

        public void OnSpawnFromPool() {
            if(!_manualControl)
                Play();
        }

        public void Play() => _mmfPlayer.PlayFeedbacks();
        public void Stop() => _mmfPlayer.StopFeedbacks();
        public void ResetFeedbacks() => _mmfPlayer.ResetFeedbacks();
    }
}
