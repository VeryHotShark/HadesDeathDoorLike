using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class PlayerHealthUI : PlayerUIModule {
        [SerializeField] private UIHealthPoint _healthPointPrefab;

        private List<UIHealthPoint> _healthPoints = new();

        private void Start() => SpawnHealthPoints(Player.HitPoints.Max);

        protected override void Enable() => Player.OnHealthChanged += OnHitPointsChanged;
        protected override void Disable() => Player.OnHealthChanged -= OnHitPointsChanged;

        private void OnHitPointsChanged(HitPoints hitPoints) => UpdateHealthPoints(hitPoints.Current);

        private void SpawnHealthPoints(int count) {
            for (int i = 0; i < count; i++) {
                UIHealthPoint healthPoint = Instantiate(_healthPointPrefab, transform);
                healthPoint.Fill(true);
                _healthPoints.Add(healthPoint);
            }
        }

        private void UpdateHealthPoints(int current) {
            for (int i = 0; i < _healthPoints.Count; i++) {
                UIHealthPoint healthPoint = _healthPoints[i];
                healthPoint.Fill(i < current);
            }
        }
    }
}
