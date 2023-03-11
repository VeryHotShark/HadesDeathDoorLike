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
            if (inputs.Ultimate.Pressed) 
                _skillCaster.CastSkill(_activeSkill);
            
            Controller.TransitionToDefaultState();
        }
    }
}