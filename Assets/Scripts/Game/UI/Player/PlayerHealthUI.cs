using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class PlayerHealthUI : PlayerUIModule {
        [SerializeField] private UIFillPoint 
            healthFillPointPrefab;

        private List<UIFillPoint> _healthPoints = new();

        private void Start() => SpawnHealthPoints(Player.HitPoints.Max);

        protected override void Enable() => Player.OnHealthChanged += OnHitPointsChanged;
        protected override void Disable() => Player.OnHealthChanged -= OnHitPointsChanged;

        private void OnHitPointsChanged(HitPoints hitPoints) => UpdateHealthPoints(hitPoints.Current);

        private void SpawnHealthPoints(int count) {
            for (int i = transform.childCount -1 ; i >= 0; i--) 
                Destroy(transform.GetChild(i).gameObject);
            
            for (int i = 0; i < count; i++) {
                UIFillPoint fillPoint = Instantiate(healthFillPointPrefab, transform);
                fillPoint.Fill(true);
                _healthPoints.Add(fillPoint);
            }
        }

        private void UpdateHealthPoints(int current) {
            for (int i = 0; i < _healthPoints.Count; i++) {
                UIFillPoint fillPoint = _healthPoints[i];
                fillPoint.Fill(i < current);
            }
        }
    }
}
