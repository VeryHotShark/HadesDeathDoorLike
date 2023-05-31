using Animancer;
using UnityEngine;

namespace VHS {
    public class WeaponKatana : WeaponMelee {
        [SerializeField] private ClipTransition _mirroredHeavy;
        [SerializeField] private ClipTransition _mirroredPerfectHeavy;
        [SerializeField] private ClipTransition _mirroredHeavyWindup;

        private bool _flipHeavy = false;
        private AttackInfo _mirroredHeavyAttack;
        private AttackInfo _mirroredPerfectHeavyAttack;
        
        public override void Init(Player player) {
            base.Init(player);
            _mirroredHeavyAttack = AttackInfo.Copy(_heavyAttack);
            _mirroredHeavyAttack.leftToRight = false;
            _mirroredHeavyAttack.animation = _mirroredHeavy;
            _mirroredHeavyAttack.animation.Events.OnEnd = OnAttackEnd;
            
            _mirroredPerfectHeavyAttack = AttackInfo.Copy(_perfectHeavyAttack);
            _mirroredPerfectHeavyAttack.leftToRight = false;
            _mirroredPerfectHeavyAttack.animation = _mirroredPerfectHeavy;
            _mirroredPerfectHeavyAttack.animation.Events.OnEnd = OnAttackEnd;
        }

        protected override void OnAttackHeld() => Animancer.Play(_flipHeavy ? _mirroredHeavyWindup : _heavyAttackWindupClip);

        protected override void OnPerfectHoldAttack() {
            SpawnAttack(_mirroredPerfectHeavyAttack);
            _player.OnPerfectMeleeAttack();
            _flipHeavy = !_flipHeavy;
        }

        protected override void OnRegularHoldAttack() {
            SpawnAttack(_flipHeavy ? _mirroredHeavyAttack : _heavyAttack);
            _flipHeavy = !_flipHeavy;
            _player.OnHeavyAttack();
        }
    }
}
