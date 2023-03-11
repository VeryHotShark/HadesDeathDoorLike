using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class Skill : ISkill {
        public UseType _castType = UseType.Instant;
        public UseType _skillType = UseType.Instant;
        
        [HideIf("_castType", UseType.Instant)]public float _castDuration = 1.0f;
        [HideIf("_skillType", UseType.Instant)]public float _skillDuration = 1.0f;

        private bool _finishedSuccessful;

        public bool FinishSuccessful => _finishedSuccessful;
        
        public float SkillDuration => _skillDuration;
        public float CastDuration => _castDuration;
        
        public UseType CastType => _castType;
        public UseType SkillType => _skillType;
        
        // Dodać property który liczy normalized ratio 0-1 trwania skilla i casta
        public Actor Owner { get; private set; }
        public SkillState SkillState { get; private set; }
        
        public void SetOwner(Actor owner) => Owner = owner;

        public virtual bool CanCastSkill() => true;

        public void Start() {
            Reset();
            SkillState = SkillState.Casting;
            OnCastStart();
        }
        
        public void FinishCast() {
            SkillState = SkillState.InProgress;
            OnSkillStart();
        }
        
        public void FinishSkill(bool successful) {
            _finishedSuccessful = successful;
            OnSkillFinish();
            SkillState = SkillState.Finished;
        }
        
        public virtual void Reset() {
            SkillState = SkillState.None;
            OnReset();
        }
        
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
        
    }
    
    [Serializable] // Jak się nie uda z generyczną CastSkillem w Node Canvas to zamień spowrotem by został tylko Skill klasa
    public class Skill<T>: Skill where T : Actor{ // Change to Scriptable Object? So we can create assets out of it
        protected new T Owner => (T) base.Owner;
    }

}