using System;
using System.Collections;
using System.Collections.Generic;
using TheraBytes.BetterUi;
using UnityEngine;

namespace VHS {
    public class PlayerUltimateUI : PlayerUIModule {
        [SerializeField] private BetterImage _fillImage;
        [SerializeField] private BetterImage _fillFrame;

        private void Start() {
            float fillAmount = Player.GetComponentInChildren<CharacterUltimateCombat>().UltimatePercent;
            OnUltimatePercentChanged(fillAmount);
        }

        protected override void Enable() => Player.OnUltimatePercentChanged += OnUltimatePercentChanged;
        protected override void Disable() => Player.OnUltimatePercentChanged -= OnUltimatePercentChanged;

        private void OnUltimatePercentChanged(float percent) {
            _fillImage.fillAmount = percent;
            _fillFrame.color = percent >= 1.0f ? Color.yellow : Color.white;
        }
    }
}
