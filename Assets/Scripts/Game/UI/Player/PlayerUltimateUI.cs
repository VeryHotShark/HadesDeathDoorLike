using System;
using System.Collections;
using System.Collections.Generic;
using TheraBytes.BetterUi;
using UnityEngine;

namespace VHS {
    public class PlayerUltimateUI : PlayerUIModule {
        [SerializeField] private BetterImage _fillImage;
        [SerializeField] private BetterImage _fillFrame;

        private Color _originalColor;
        
        private void Start() {
            _originalColor = _fillImage.color;
            OnUltimatePercentChanged(0.0f);
        }

        protected override void Enable() {
            Player.OnUltimatePercentChanged += OnUltimatePercentChanged;
            Player.OnUltimateEnter += OnUltimateEnter;
            Player.OnUltimateExit += OnUltimateExit;
        }

        protected override void Disable() {
            Player.OnUltimatePercentChanged -= OnUltimatePercentChanged;
            Player.OnUltimateEnter -= OnUltimateEnter;
            Player.OnUltimateExit -= OnUltimateExit;
        }

        private void OnUltimateEnter() => _fillImage.color = Color.red;
        private void OnUltimateExit() => _fillImage.color = _originalColor;
        
        private void OnUltimatePercentChanged(float percent) {
            _fillImage.fillAmount = percent;

            bool isFilled = percent >= 1.0f;
            _fillFrame.color = isFilled ? Color.yellow : Color.white;
            
            if(isFilled)
                _fillImage.color = Color.red;
        }
    }
}
