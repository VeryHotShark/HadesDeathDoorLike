using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class StatusFire : Status {
        [SerializeField] private int _damagePerInterval;

        protected override void OnInterval(float interval) {
            HitData hitData = new HitData() {
                damage = _damagePerInterval,
                position = _npc.CenterOfMass,
                instigator = PlayerManager.PlayerInstance,
            };
            
            _npc.Hit(hitData);
        }
    }
}
