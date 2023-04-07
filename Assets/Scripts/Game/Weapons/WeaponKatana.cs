using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

namespace VHS {
    public class WeaponKatana : WeaponMelee {
        [SerializeField] private ClipTransition _mirroredHeavy;
        [SerializeField] private ClipTransition _mirroredHeavyWindup;

        private bool _flipHeavy = false;
        private AttackInfo _mirroredHeavyAttack;

        public override void Init(Player player) {
            base.Init(player);
            _mirroredHeavyAttack = AttackInfo.Copy(_heavyAttack);
            _mirroredHeavyAttack.animation = _mirroredHeavy;
        }

        public override void OnHeavyAttackHeld() => Animancer.Play(_flipHeavy ? _mirroredHeavyWindup : _heavyAttackWindupClip);
        public override void HeavyAttack() {
            SpawnAttack(_flipHeavy ? _mirroredHeavyAttack : _heavyAttack, Vector3.one * 0.4f, _flipHeavy);
            _flipHeavy = !_flipHeavy;
        }
    }
}
