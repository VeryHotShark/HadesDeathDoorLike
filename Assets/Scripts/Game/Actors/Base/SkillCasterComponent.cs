using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace VHS {
    public class SkillCasterComponent : ChildBehaviour<Actor> {
        private float _castDuration;
        private float _totalDuration;
        private Skill _activeSkill;

        public Skill ActiveSkill => _activeSkill;

        /// <summary>
        /// Setup skill owner reference to work correctly
        /// </summary>
        /// <param name="skill"> skill reference to be set </param>
        public void InitSkill(Skill skill) => skill.SetOwner(Parent);

        /// <summary>
        ///  Entry point of Casting Skill, decides whether to start target or start skill
        /// </summary>
        public bool CastSkill(Skill skill) {
            skill.SetOwner(Parent);

            if (!skill.CanCastSkill())
                return false;
            
            _castDuration = 0.0f;
            _totalDuration = 0.0f;
            
            _activeSkill = skill;
            _activeSkill.Start();

            if (_activeSkill.CastType == TimeType.Instant) {
                _activeSkill.FinishCast();
                
                if(_activeSkill.SkillType == TimeType.Instant)
                    _activeSkill.FinishSkill(true);
            } 
            
            return true;
        }

        /// <summary>
        /// Evaluate skill and change it state based on elapsed time
        /// </summary>
        public void TickSkill(float dt) {
            _totalDuration += dt;
            
            switch (_activeSkill.SkillState) {
                case SkillState.Casting:
                    if (_activeSkill.CastType == TimeType.Duration && _totalDuration > _activeSkill.CastDuration) 
                        _activeSkill.FinishCast();
                    else
                        _activeSkill.OnCastTick(dt);
                    
                    _castDuration = _totalDuration;
                    
                    break;
                case SkillState.InProgress:
                    if (_activeSkill.SkillType == TimeType.Instant) {
                        _activeSkill.FinishSkill(true);
                        return;          
                    }
                    
                    if (_activeSkill.SkillType == TimeType.Duration &&
                        _totalDuration - _castDuration > _activeSkill.SkillDuration) 
                        _activeSkill.FinishSkill(true);
                    else
                        _activeSkill.OnSkillTick(dt);
                    
                    break;
                case SkillState.Finished :
                    _activeSkill = null;
                    break;
            }
        }

        /// <summary>
        /// Cancel skill respective to it current state
        /// </summary>
        public void CancelSkill() {
            if(_activeSkill == null)
                return;

            switch (_activeSkill.SkillState) {
                case SkillState.Casting:
                    _activeSkill.OnCastCancel();
                    break;
                case SkillState.InProgress:
                    _activeSkill.OnSkillCancel();
                    break;
            }
            
            _activeSkill.Abort();
            _activeSkill = null;
        }
    }
}
