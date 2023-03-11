using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class CharacterSkillCombat : CharacterModule {
        [SerializeReference] private ISkill _activeSkill;

        private SkillCasterComponent _skillCaster;
        
        private void Awake() => _skillCaster = GetComponent<SkillCasterComponent>();

        public override void SetInputs(CharacterInputs inputs) {
            if(_activeSkill.CastType == TimeType.Infinite)
                if (inputs.Ultimate.Released)
                    _activeSkill.FinishCast();                    
        }

        public override void OnEnter() => _skillCaster.CastSkill(_activeSkill);

        public override void OnTick(float deltaTime) {
            _skillCaster.TickSkill(deltaTime);
            
            if(_skillCaster.ActiveSkill == null)   
               Controller.TransitionToDefaultState();
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            currentVelocity = Vector3.zero; // To będzie pewnie per Skill
        }
    }
}