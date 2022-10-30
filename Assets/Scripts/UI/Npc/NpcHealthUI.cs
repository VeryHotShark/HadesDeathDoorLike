using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class NpcHealthUI : ActorComponent<Npc>
    {
        [SerializeField] private UIHealthPoint _healthPointPrefab;
        
        private List<UIHealthPoint> _healthPoints = new();
        private void Start() => SpawnHealthPoints(Parent.HitPoints.Max);

        protected override void Enable() => Parent.HitPoints.OnChanged += OnHitPointsChanged;
        protected override void Disable() => Parent.HitPoints.OnChanged -= OnHitPointsChanged;

        private void SpawnHealthPoints(int count) {
            for (int i = 0; i < count; i++) {
                UIHealthPoint healthPoint = Instantiate(_healthPointPrefab, transform);
                healthPoint.Fill(true);
                _healthPoints.Add(healthPoint);
            }
        }
        
        private void OnHitPointsChanged(HitPoints hitPoints) {
            for (int i = 0; i < _healthPoints.Count; i++) {
                UIHealthPoint healthPoint = _healthPoints[i];
                healthPoint.Fill(i < hitPoints.Current);
            } 
        }
    }
}
