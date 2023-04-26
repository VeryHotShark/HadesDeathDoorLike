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

        private bool _enteredFromPrimary;
        private SkillCasterComponent _skillCaster;

        private PlayerSkill _primaryInstance;
        private PlayerSkill _secondaryInstance;
        
        public PlayerSkill SkillPrimary => _primaryInstance;
        public PlayerSkill SkillSecondary => _secondaryInstance;
        
        private void Awake() {
            _skillCaster = GetComponent<SkillCasterComponent>();
            _primaryInstance = _skillPrimary.GetInstance();
            _secondaryInstance = _skillSecondary.GetInstance();
        }
        
        public override void OnEnter() {
            _enteredFromPrimary = Controller.CurrentCharacterInputs.SkillPrimary.Pressed;
            _skillCaster.CastSkill(_enteredFromPrimary ? _primaryInstance : _secondaryInstance);
        }
        
        public override void SetInputs(CharacterInputs inputs) {
            if (_enteredFromPrimary) {
                if(_primaryInstance.CastType == TimeType.Infinite)
                    if (inputs.SkillPrimary.Released)
                        _primaryInstance.FinishCast(); 
            }
            else {
                if(_secondaryInstance.CastType == TimeType.Infinite)
                    if (inputs.SkillSecondary.Released)
                        _secondaryInstance.FinishCast(); 
            }
        }

        public override void OnExit() => _skillCaster.CancelSkill();

        public override void OnTick(float deltaTime) {
            _skillCaster.TickSkill(deltaTime);
            
            if(_skillCaster.ActiveSkill == null)   
               Controller.TransitionToDefaultState();
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) => currentVelocity = Vector3.zero; // To będzie pewnie per Skill

        public override bool CanEnterState() => _skillCaster.ActiveSkill == null;

        public bool CanCastPrimary() => _primaryInstance.CanCastSkill();
        public bool CanCastSecondary() => _secondaryInstance.CanCastSkill();
    }
}