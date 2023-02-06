using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class PlayerStaminaUI : PlayerUIModule {
        [SerializeField] private UIFillPoint staminaFillPointPrefab;
        
        private List<UIFillPoint> _staminaPoints = new();

        private void Start() => SpawnStaminaPoints(Player.CharacterController.RangeCombat.MaxAmmoCount);

        protected override void Enable() => Player.OnCurrentAmmoChanged += OnStaminaPointsChanged;
        protected override void Disable() => Player.OnCurrentAmmoChanged -= OnStaminaPointsChanged;

        private void OnStaminaPointsChanged(int staminaPoints) => UpdateStaminaPoints(staminaPoints);

        private void SpawnStaminaPoints(int count) {
            for (int i = 0; i < count; i++) {
                UIFillPoint fillPoint = Instantiate(staminaFillPointPrefab, transform);
                fillPoint.Fill(true);
                _staminaPoints.Add(fillPoint);
            }
        }

        private void UpdateStaminaPoints(int current) {
            for (int i = 0; i < _staminaPoints.Count; i++) {
                UIFillPoint fillPoint = _staminaPoints[i];
                fillPoint.Fill(i < current);
            }
        }
    }
}