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

        private void Awake() {
            _mmfPlayer = GetComponent<MMF_Player>();
            _mmfPlayer.Initialization();
            _mmfPlayer.Events.OnComplete.AddListener(ReturnToPool);
        }

        private void OnEnable() => MMFeedbacksEvent.Register(OnMMFeedbacksEvent);
        private void OnDisable() => MMFeedbacksEvent.Unregister(OnMMFeedbacksEvent);

        private void OnMMFeedbacksEvent(MMFeedbacks source, MMFeedbacksEvent.EventTypes type) {
            switch (type) {
                case MMFeedbacksEvent.EventTypes.Complete:
                    Debug.Log("The feedback " + source.name + " just completed.");
                    break;
            }
        }

        public void ReturnToPool() => PoolManager.Return(this);

        public void OnSpawnFromPool() => _mmfPlayer.PlayFeedbacks();
    }
}
