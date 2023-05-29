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
            _mirroredHeavyAttack.leftToRight = false;
            _mirroredHeavyAttack.animation = _mirroredHeavy;
        }

        protected override void OnAttackHeld() => Animancer.Play(_flipHeavy ? _mirroredHeavyWindup : _heavyAttackWindupClip);

        protected override void OnPerfectHoldAttack() {
            base.OnPerfectHoldAttack();
            _flipHeavy = !_flipHeavy;
        }

        protected override void OnRegularHoldAttack() {
            SpawnAttack(_flipHeavy ? _mirroredHeavyAttack : _heavyAttack);
            _flipHeavy = !_flipHeavy;
            _player.OnHeavyAttack();
        }
    }
}
