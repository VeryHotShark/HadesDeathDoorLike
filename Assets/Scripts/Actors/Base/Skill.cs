using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class Skill : ISkill {
        public CastType _castType = CastType.SmartCast;
        public SkillType _skillType = SkillType.Instant;
        public float _castDuration = 1.0f;
        public float _skillDuration = 1.0f;

        public float CastDuration => _castDuration;
        public float SkillDuration => _skillDuration;
        
        public CastType CastType => _castType;
        public SkillType SkillType => _skillType;
        
        // Dodać property który liczy normalized ratio 0-1 trwania skilla i casta
        public Actor Owner { get; private set; }
        public SkillState SkillState { get; private set; } // Managed by SkillCaster
        
        public void SetOwner(Actor owner) => Owner = owner;
        public void SetState(SkillState state) => SkillState = state;

        public virtual bool CanCastSkill() => true;
        
        public virtual void StartTarget() { }
        public virtual void TickTarget(float deltaTime) { }
        public virtual void FinishTarget() { }
        
        
        public virtual void StartSkill() { }
        public virtual void TickSkill(float deltaTime) { }
        public virtual void FinishSkill() { }
        
        /// <summary>
        /// Called only if current skill State is during Targetting
        /// </summary>
        public virtual void CancelTarget() { }
        
        /// <summary>
        /// Called only if current skill State is in Progress
        /// </summary>
        public virtual void CancelSkill() { }
        
        /// <summary>
        /// Called when skill is canceled, independently of it current state
        /// </summary>
        public virtual void Abort() { }
    }
    
    [Serializable] // Jak się nie uda z generyczną CastSkillem w Node Canvas to zamień spowrotem by został tylko Skill klasa
    public class Skill<T>: Skill where T : Actor{ // Change to Scriptable Object? So we can create assets out of it
        protected new T Owner => base.Owner as T;
    }

}