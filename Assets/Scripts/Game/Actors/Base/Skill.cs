using System;
using System.Collections;
using System.Collections.Generic;
using ParadoxNotion.Design;
using UnityEngine;

namespace VHS {
    public enum SkillState {
        None,
        Casting,
        InProgress,
        Finished,
    }

    public enum TimeType {
        Instant = 0,
        Duration = 1,
        Infinite = 2,
    }
    
    [Serializable]
    public class Skill {
        [ParadoxNotion.Design.Header("Common")]
        
        public TimeType _castType = TimeType.Instant;
        [ShowIf("_castType", 1), Sirenix.OdinInspector.MinValue(0.0f)] public float _castDuration = 1.0f;
        
        public TimeType _skillType = TimeType.Instant;
        [ShowIf("_skillType", 1), Sirenix.OdinInspector.MinValue(0.0f)] public float _skillDuration = 1.0f;

        private bool _initialized;
        private bool _finishedSuccessful;

        public bool Initialized => _initialized;
        public bool FinishSuccessful => _finishedSuccessful;
        
        public float SkillDuration => _skillDuration;
        public float CastDuration => _castDuration;
        
        public TimeType CastType => _castType;
        public TimeType SkillType => _skillType;
        
        // Dodać property który liczy normalized ratio 0-1 trwania skilla i casta

        public Actor Owner { get; private set; }
        public SkillState SkillState { get; private set; }

        public void Initialize(Actor owner) {
            Owner = owner;
            _initialized = true;
            OnInitialize();
        }

        public void Disable() {
            Owner = null;
            _initialized = false;
            OnDisable();
        }

        public void Start() {
            Reset();
            SkillState = SkillState.Casting;
            OnCastStart();
        }
        
        public void FinishCast() {
            if (SkillState != SkillState.Casting)
                return; // Prevents Double Invocation
                
            OnCastFinish();
            SkillState = SkillState.InProgress;
            OnSkillStart();
        }
        
        public void FinishSkill(bool successful) {
            _finishedSuccessful = successful;
            SkillState = SkillState.Finished;
            OnSkillFinish();
        }

        public void Abort() {
            SkillState = SkillState.None;
            OnAbort();
        }
        
        public void Reset() {
            SkillState = SkillState.None;
            OnReset();
        }

        public virtual bool CanCastSkill() => true;

        public virtual void OnCastStart() { }
        public virtual void OnCastTick(float deltaTime) { }
        public virtual void OnCastFinish() { }
        public virtual void OnCastCancel() { }

        public virtual void OnSkillStart() { }
        public virtual void OnSkillTick(float deltaTime) { }
        public virtual void OnSkillFinish() { }
        public virtual void OnSkillCancel() { }

        public virtual void OnAbort() { }
        public virtual void OnReset() { }
        public virtual void OnInitialize() { }
        public virtual void OnDisable() { }
    }
    
    
    
    [Serializable]
    public class Skill<T>: Skill where T : Actor{
        protected new T Owner => (T) base.Owner;
    }
}