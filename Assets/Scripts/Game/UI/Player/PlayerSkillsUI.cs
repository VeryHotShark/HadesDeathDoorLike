using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class PlayerSkillsUI : PlayerUIModule {
        
        private UISkillDisplay[] _skillDisplays;

        protected override void Awake() {
            base.Awake();
            _skillDisplays = GetComponentsInChildren<UISkillDisplay>();

            foreach (UISkillDisplay skillDisplay in _skillDisplays) 
                skillDisplay.ResetSkillCooldown();
        }

        protected override void Enable() {
            Player.OnSkillCooldownStart += OnSkillCooldownStart;
            Player.OnSkillCooldownUpdate += OnSkillCooldownUpdate;
            Player.OnSkillCooldownEnd += OnSkillCooldownSEnd;
        }

        protected override void Disable() {
            Player.OnSkillCooldownStart -= OnSkillCooldownStart;
            Player.OnSkillCooldownUpdate -= OnSkillCooldownUpdate;
            Player.OnSkillCooldownEnd -= OnSkillCooldownSEnd;
        }


        private void OnSkillCooldownStart(PlayerSkill playerSkill) {
            if (playerSkill == Character.SkillCombat.SkillPrimary) 
                _skillDisplays[1].StartSkillCooldown();
            else 
                _skillDisplays[0].StartSkillCooldown();
        }
        
        private void OnSkillCooldownUpdate(PlayerSkill playerSkill) {
            if (playerSkill == Character.SkillCombat.SkillPrimary) 
                _skillDisplays[1].UpdateSkillCooldown(playerSkill.CooldownRatio);
            else 
                _skillDisplays[0].UpdateSkillCooldown(playerSkill.CooldownRatio);
        }

        private void OnSkillCooldownSEnd(PlayerSkill playerSkill) {
            if (playerSkill == Character.SkillCombat.SkillPrimary) 
                _skillDisplays[1].ResetSkillCooldown();
            else 
                _skillDisplays[0].ResetSkillCooldown();
        }
    }
}
