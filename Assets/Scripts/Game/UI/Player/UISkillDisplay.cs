using System;
using System.Collections;
using System.Collections.Generic;
using TheraBytes.BetterUi;
using TMPro;
using UnityEngine;

namespace VHS {
    public class UISkillDisplay : MonoBehaviour {
        [SerializeField] private BetterImage _frame;
        [SerializeField] private BetterImage _overlay;
        [SerializeField] private BetterImage _icon;

        private TextMeshProUGUI _keyText;
        
        private void Awake() => _keyText = GetComponentInChildren<TextMeshProUGUI>();

        public void StartSkillCooldown() {
            _frame.color = Color.black;
            _overlay.fillAmount = 1.0f;
        }

        public void UpdateSkillCooldown(float cooldownRatio) => _overlay.fillAmount = cooldownRatio;

        public void ResetSkillCooldown() {
            _frame.color = Color.white;
            _overlay.fillAmount = 0.0f;
        }
    }
}
