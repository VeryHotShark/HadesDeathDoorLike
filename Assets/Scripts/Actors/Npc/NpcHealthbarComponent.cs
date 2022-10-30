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
                _healthUI.transform.localScale = Vector3.one * 0.005f;
                _healthUI.transform.rotation = Quaternion.Euler(60.0f,45.0f,0.0f);
                _healthUI.Init(hitPoints.Max, Parent.transform, Vector3.up * 3.0f);
            }
            
            _healthUI.OnHitPointsChanged(hitPoints);
            
            if(!hitPoints.AboveZero)
                Destroy(_healthUI.gameObject);
        }
    }
}