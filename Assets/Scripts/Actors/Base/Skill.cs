using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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
        public SkillState SkillState { get; private set; }
        
        public void SetOwner(Actor owner) => Owner = owner;

        public virtual bool CanCastSkill() => true;

        public void StartTarget() {
            SkillState = SkillState.Targetting;
            StartTarget_Hook();
        }

        /// <summary>
        /// Called After Start Target
        /// </summary>
        public virtual void StartTarget_Hook() { }
        
        /// <summary>
        /// Called every tick when Targetting is active
        /// </summary>
        public virtual void TickTarget(float deltaTime) { }
        
        /// <summary>
        /// Called after the targetting is finish
        /// </summary>
        public virtual void FinishTarget() { }

        public  void StartSkill() {
            SkillState = SkillState.InProgress;
            StartSkill_Hook();
        }

        /// <summary>
        /// Called After skill is Started
        /// </summary>
        public virtual void StartSkill_Hook() {}
        
        /// <summary>
        /// Called every tick when Skill is active
        /// </summary>
        public virtual void TickSkill(float deltaTime) { }
        
        public void FinishSkill() {
            FinishSkill_Hook();
            SkillState = SkillState.Finished;
        }

        /// <summary>
        /// Called after skill is Finished
        /// </summary>
        public virtual void FinishSkill_Hook() {}

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

        /// <summary>
        /// Called before skill Activation and after Skill Finish
        /// </summary>
        public virtual void Reset() => SkillState = SkillState.None;
    }
    
    [Serializable] // Jak się nie uda z generyczną CastSkillem w Node Canvas to zamień spowrotem by został tylko Skill klasa
    public class Skill<T>: Skill where T : Actor{ // Change to Scriptable Object? So we can create assets out of it
        protected new T Owner => (T) base.Owner;
    }

}