using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace VHS {
    public class NpcHealthUI : BaseBehaviour
    {
        [SerializeField] private UIHealthPoint _healthPointPrefab;
        
        private CanvasGroup _canvasGroup;
        private PositionConstraint _positionConstraint;
        private List<UIHealthPoint> _healthPoints = new();

        private void Awake() {
            _canvasGroup = GetComponent<CanvasGroup>();
            _positionConstraint = GetComponent<PositionConstraint>();
        }

        public void Init(int maxPoints, Transform attachTransform, Vector3 offset) {
            ConstraintSource constraintSource = new ConstraintSource {
                weight = 1.0f,
                sourceTransform = attachTransform
            };

            _positionConstraint.AddSource(constraintSource);
            _positionConstraint.translationOffset = offset;
            _positionConstraint.constraintActive = true;
            SpawnHealthPoints(maxPoints);
            _canvasGroup.alpha = 0.0f;
        }

        private void SpawnHealthPoints(int count) {
            for (int i = 0; i < count; i++) {
                UIHealthPoint healthPoint = Instantiate(_healthPointPrefab, transform);
                healthPoint.Fill(true);
                _healthPoints.Add(healthPoint);
            }
        }
        
        public void OnHitPointsChanged(HitPoints hitPoints) {
            if (_canvasGroup.alpha == 0.0f)
                _canvasGroup.alpha = 1.0f;
            
            for (int i = 0; i < _healthPoints.Count; i++) {
                UIHealthPoint healthPoint = _healthPoints[i];
                healthPoint.Fill(i < hitPoints.Current);
            } 
        }
    }
}
