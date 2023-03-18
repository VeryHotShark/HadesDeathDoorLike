using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VHS {
    public enum PassiveTriggerType {
        Tick,
        Interval,
        Event,
    }

    public class PassiveTrigger : IUpdateListener {
        public event Action OnTriggered = delegate { };
        
        public PassiveTriggerType _triggerType;

        [ShowIf("_triggerType", PassiveTriggerType.Interval)]
        private float _interval;
        
        public Timer _triggerTimer;

        public void InitTrigger(Actor actor) {
            switch (_triggerType) {
                case PassiveTriggerType.Interval:
                    _triggerTimer = new Timer(_interval, true);
                    break;
                case PassiveTriggerType.Event:
                    break;
            }
        }

        public virtual void OnEnable() {
            if(_triggerType == PassiveTriggerType.Interval)
                UpdateManager.AddUpdateListener(this);
        }

        public virtual void OnDisable() {
            if(_triggerType == PassiveTriggerType.Interval)
                UpdateManager.RemoveUpdateListener(this);
        }

        public void OnUpdate(float deltaTime) => OnTriggered();
    }
}