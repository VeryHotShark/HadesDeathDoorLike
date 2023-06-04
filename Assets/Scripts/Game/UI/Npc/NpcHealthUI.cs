using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace VHS {
    public class NpcHealthUI : BaseBehaviour
    {
        [SerializeField] private UIFillPoint healthFillPointPrefab;
        
        private CanvasGroup _canvasGroup;
        private List<UIFillPoint> _healthPoints = new();

        private void Awake() {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Init(int maxPoints) {
            SpawnHealthPoints(maxPoints);
            _canvasGroup.alpha = 0.0f;
        }

        private void SpawnHealthPoints(int count) {
            for (int i = 0; i < count; i++) {
                UIFillPoint fillPoint = Instantiate(healthFillPointPrefab, transform);
                fillPoint.Fill(true);
                _healthPoints.Add(fillPoint);
            }
        }
        
        public void OnHitPointsChanged(HitPoints hitPoints) {
            if (_canvasGroup.alpha == 0.0f)
                _canvasGroup.alpha = 1.0f;
            
            for (int i = 0; i < _healthPoints.Count; i++) {
                UIFillPoint fillPoint = _healthPoints[i];
                fillPoint.Fill(i < hitPoints.Current);
            } 
        }
    }
}
