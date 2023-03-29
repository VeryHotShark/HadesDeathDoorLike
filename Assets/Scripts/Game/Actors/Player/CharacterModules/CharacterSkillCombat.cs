using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VHS {
    public class CharacterSkillCombat : CharacterModule {
        [SerializeField] private SkillSO _skill;

        private SkillCasterComponent _skillCaster;
        
        private void Awake() => _skillCaster = GetComponent<SkillCasterComponent>();

        public override void SetInputs(CharacterInputs inputs) {
            if(_skill.Instance.CastType == TimeType.Infinite)
                if (inputs.Ultimate.Released)
                    _skill.Instance.FinishCast();                    
        }

        public override void OnEnter() => _skillCaster.CastSkill(_skill.Instance);
        public override void OnExit() => _skillCaster.CancelSkill();

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