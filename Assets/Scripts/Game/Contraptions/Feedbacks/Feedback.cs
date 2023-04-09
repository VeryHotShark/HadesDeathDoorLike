using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

namespace VHS {
    public class Feedback : MonoBehaviour, IPoolable {
        private MMF_Player _mmfPlayer;
        public MMF_Player FeedbackPlayer => _mmfPlayer;
        
        public Action OnComplete = delegate { };

        private void Awake() {
            _mmfPlayer = GetComponent<MMF_Player>();
            _mmfPlayer.Initialization();
        }

        private void OnEnable() => _mmfPlayer.Events.OnComplete.AddListener(OnFeedbackComplete);
        private void OnDisable() => _mmfPlayer.Events.OnComplete.RemoveListener(OnFeedbackComplete);

        private void OnFeedbackComplete() {
            OnComplete();
            ReturnToPool();
        }

        public void ReturnToPool() => PoolManager.Return(this);

        public void OnSpawnFromPool() => _mmfPlayer.PlayFeedbacks();
    }
}
