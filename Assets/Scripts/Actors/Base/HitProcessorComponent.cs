using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VHS;


namespace VHS {
    public class HitProcessorComponent : ChildBehaviour<Actor> {
        [SerializeField] private HitPoints _hitPoints;

        private void Awake() => _hitPoints.Reset();

        public void Hit(HitData hitData) {
            if(!_hitPoints.AboveZero)
                return;
            
            _hitPoints.Subtract(hitData.damage);
            
            Parent.OnHit(hitData);

            if (!_hitPoints.AboveZero) {
                Parent.OnDeath(Parent);
            }
        }

    }
}