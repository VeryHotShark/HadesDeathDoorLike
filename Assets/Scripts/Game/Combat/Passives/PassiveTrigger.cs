using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VHS {
    public enum PassiveTriggerType {
        Interval,
        Event
    }

    [Serializable]
    public class PassiveTrigger : ICustomUpdateListener {
        public Action OnTriggered = delegate { };
        
        public PassiveTriggerType _triggerType;

        [ShowIf("_triggerType", PassiveTriggerType.Interval)]
        public float interval = 3.0f;

        [ShowIf("_triggerType", PassiveTriggerType.Event)]
        public GameEvent gameEvent;

        public virtual void OnEnable() {
            switch (_triggerType) {
                case PassiveTriggerType.Interval:
                    UpdateManager.AddCustomUpdateListener(interval, this);
                    break;
                case PassiveTriggerType.Event:
                    gameEvent.OnEventRaised += OnGameEventRaised;
                    break;
            }
        }

        public virtual void OnDisable() {
            switch (_triggerType) {
                case PassiveTriggerType.Interval:
                    UpdateManager.RemoveCustomUpdateListener(interval, this);
                    break;
                case PassiveTriggerType.Event:
                    gameEvent.OnEventRaised -= OnGameEventRaised;
                    break;
            }
        }

        private void OnTimerEnd() => OnTriggered();

        public void OnCustomUpdate(float deltaTime) => OnTriggered();

        private void OnGameEventRaised(UnityEngine.Object sender) => OnTriggered();
    }
}