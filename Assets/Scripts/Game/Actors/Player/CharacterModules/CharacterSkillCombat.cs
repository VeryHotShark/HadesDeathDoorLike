using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace VHS {
    public class CharacterSkillCombat : CharacterModule {
        [SerializeField] private SkillSO _skillPrimary;
        [SerializeField] private SkillSO _skillSecondary;

        private SkillCasterComponent _skillCaster;
        
        private void Awake() => _skillCaster = GetComponent<SkillCasterComponent>();

        public override void SetInputs(CharacterInputs inputs) {
            if(_skillPrimary.Instance.CastType == TimeType.Infinite)
                if (inputs.SkillPrimary.Released)
                    _skillPrimary.Instance.FinishCast();                    
        }

        public override void OnEnter() => _skillCaster.CastSkill(_skillPrimary.Instance);
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