using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace VHS {
    public class SkillCasterComponent : ChildBehaviour<Actor> {
        [SerializeReference] public Skill _skill;

        private float _endCastTimestamp;
        private ISkill _activeSkill;

        /// <summary>
        /// Setup skill owner reference to work correctly
        /// </summary>
        /// <param name="skill"> skill reference to be set </param>
        public void InitSkill(ISkill skill) => skill.SetOwner(Parent);

        /// <summary>
        ///  Entry point of Casting Skill, decides whether to start target or start skill
        /// </summary>
        public bool CastSkill(ISkill skill) {
            skill.SetOwner(Parent);

            if (!skill.CanCastSkill())
                return false;
            
            _activeSkill = skill;
            _endCastTimestamp = 0.0f;   
            
            skill.SetState(SkillState.None);
            skill.StartTarget();

            if (skill.CastType == CastType.SmartCast)
                skill.FinishTarget();

            switch (skill.CastType, skill.SkillType) {
                case(CastType.SmartCast, SkillType.Instant) :
                    skill.StartSkill();
                    skill.FinishSkill();
                    skill.SetState(SkillState.Finished);
                    break;
                case(CastType.SmartCast, SkillType.ForDuration) :
                    skill.StartSkill();
                    skill.SetState(SkillState.InProgress);
                    break;
                case(CastType.CustomCast, SkillType.Instant) :
                case(CastType.CustomCast, SkillType.ForDuration) :
                    skill.SetState(SkillState.Targetting);
                    break;
                default:
                    Log("THIS SHOULD NEVER BE THE CASE");
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Evaluate skill and change it state based on elapsed time
        /// </summary>
        /// <param name="elapsedTime"></param>
        /// <returns></returns>
        public void TickSkill(float elapsedTime) {
            switch (_activeSkill.SkillState) {
                case SkillState.Targetting:
                    if (elapsedTime > _activeSkill.CastDuration) {
                        _endCastTimestamp = elapsedTime;
                        _activeSkill.FinishTarget();
                        _activeSkill.StartSkill();
                        _activeSkill.SetState(SkillState.InProgress);
                    }
                    else
                        _activeSkill.TickTarget(Time.deltaTime);
                    
                    break;
                case SkillState.InProgress:
                    if (elapsedTime - _endCastTimestamp > _activeSkill.SkillDuration) {
                        _activeSkill.FinishSkill();
                        _activeSkill.SetState(SkillState.Finished);
                    }
                    else
                        _activeSkill.TickSkill(Time.deltaTime);
                    
                    break;
                case SkillState.Finished :
                    _activeSkill = null;
                    break;
            }
        }
    }
}
