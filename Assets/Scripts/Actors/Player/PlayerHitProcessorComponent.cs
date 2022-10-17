using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    /// <summary>
    /// Maybe we can make it generic and Move parry and other processosrs to Classes,
    /// and Handle them before subtracting health like in Postal
    /// </summary>
    public class PlayerHitProcessorComponent : HitProcessorComponent<Player> {
        
        public override void Hit(HitData hitData) {
            if(!_hitPoints.AboveZero)
                return;

            if (!Parent.DuringParry) {
                _hitPoints.Subtract(hitData.damage);
                Parent.OnHit(hitData);
            }
            else 
                Parent.OnParry(hitData);

            if (!_hitPoints.AboveZero) 
                Parent.Die();            
        }
    }
}
