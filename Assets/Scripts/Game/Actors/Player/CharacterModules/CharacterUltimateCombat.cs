using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class CharacterUltimateCombat : CharacterModule {
        [SerializeField] private float _requiredDealDamage;
        [SerializeField] private float _duration;

        private float _currentUltimateFill = 0.0f;
        public float UltimatePercent => _currentUltimateFill / _requiredDealDamage;
        
        protected override void Enable() {
            Parent.OnMeleeHit += OnMeleeHit;
            Parent.OnRangeHit += OnRangeHit;
        }
        
        protected override void Disable() {
            Parent.OnMeleeHit -= OnMeleeHit;
            Parent.OnRangeHit -= OnRangeHit;
        }

        private void OnMeleeHit(HitData hitData) => ChangeUltimateFill(hitData.damage);

        private void OnRangeHit(HitData hitData) => ChangeUltimateFill(hitData.damage / 2.0f);

        private void ChangeUltimateFill(float amount) {
            _currentUltimateFill += amount;
            _currentUltimateFill = Mathf.Clamp(_currentUltimateFill,0.0f, _requiredDealDamage);
            Parent.OnUltimatePercentChanged(UltimatePercent);
        }
    }
}
