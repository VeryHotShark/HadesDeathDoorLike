using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VHS;

namespace VHS {
    public class NpcHealthbarComponent : NpcComponent {
        [SerializeField] private NpcHealthUI _healthUIPrefab;

        private NpcHealthUI _healthUI;

        protected override void Enable() => Parent.OnHealthChanged += OnHealthChanged;
        protected override void Disable() => Parent.OnHealthChanged -= OnHealthChanged;

        private void OnHealthChanged(HitPoints hitPoints) {
            if (_healthUI == null) {
                _healthUI = Instantiate(_healthUIPrefab);
                Parent.UIComponent.Attach(_healthUI.transform);
                _healthUI.Init(hitPoints.Max);
            }
            
            _healthUI.OnHitPointsChanged(hitPoints);
            
            if(!hitPoints.AboveZero)
                Destroy(_healthUI.gameObject);
        }
    }
}