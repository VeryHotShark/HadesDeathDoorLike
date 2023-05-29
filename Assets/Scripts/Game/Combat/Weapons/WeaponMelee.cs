using System.Collections;
using System.Collections.Generic;
using Animancer;
using MEC;
using UnityEngine;
using UnityEngine.Serialization;

namespace VHS {
    public abstract  class WeaponMelee : Weapon
    {
        private const float HIT_DELAY = 0.15f;
        
        [Header("VFX")]
        [SerializeField] private SlashController _slashParticle;
        
        [Header("Attacks")]
        [SerializeField] protected List<AttackInfo> _lightAttacks;
        [SerializeField] protected AttackInfo _heavyAttack;
        [SerializeField] protected AttackInfo _perfectHeavyAttack;
        [FormerlySerializedAs("_dashLightAttack")] [SerializeField] protected AttackInfo _dashAttack;
        [SerializeField] protected ClipTransition _heavyAttackWindupClip;

        private bool _coroutineStarted; // TODO temporary because MEC Free doesnt have IsRunning Field
        private SlashController _slashInstance;
        protected AttackInfo _currentAttack;
        
        public Timer CurrentAttackTimer => _currentAttack?.duration;
        public bool IsDuringAttack => CurrentAttackTimer is {IsActive: true};
        public int LastLightAttackIndex => _lightAttacks.Count;

        public override void OnWeaponStart() {
            if(!_coroutineStarted)
                base.OnWeaponStart();

            _coroutineStarted = true;
        }

        protected override void OnPerfectStart() {
            base.OnPerfectStart();
            _player.OnPerfectMeleeAttackStart();
        }
        
        protected override void OnPerfectEnd() {
            base.OnPerfectEnd();
            _coroutineStarted = false;
            _player.OnPerfectMeleeAttackEnd();
        }
        
        public override void Init(Player player) {
            base.Init(player);

            foreach (AttackInfo attack in _lightAttacks)
                attack.attackType = PlayerAttackType.LIGHT;

            _heavyAttack.attackType = PlayerAttackType.HEAVY;
            _dashAttack.attackType = PlayerAttackType.DASH_ATTACK;
            _perfectHeavyAttack.attackType = PlayerAttackType.PERFECT_HEAVY;
        }

        protected override void Enable() {
            base.Enable();
            foreach (AttackInfo attack in _lightAttacks)
                attack.duration.OnEnd += OnAttackEnd;

            _perfectHeavyAttack.duration.OnEnd += OnAttackEnd;
            _dashAttack.duration.OnEnd += OnAttackEnd;
            _heavyAttack.duration.OnEnd += OnAttackEnd;
        }

        protected override void Disable() {
            base.Disable();
            foreach (AttackInfo attack in _lightAttacks)
                attack.duration.OnEnd -= OnAttackEnd;

            _perfectHeavyAttack.duration.OnEnd -= OnAttackEnd;
            _dashAttack.duration.OnEnd -= OnAttackEnd;
            _heavyAttack.duration.OnEnd -= OnAttackEnd;
        }
        
        private void OnAttackEnd() => Character.MeleeCombat.OnAttackEnd();

        public virtual void DashAttack() => SpawnAttack(_dashAttack);

        public virtual void LightAttack(int index) => SpawnAttack(_lightAttacks[index]);

        protected override void OnPerfectHoldAttack() {
            SpawnAttack(_perfectHeavyAttack);
            _player.OnPerfectMeleeAttack();
        }

        protected override void OnRegularHoldAttack() {
            SpawnAttack(_heavyAttack);
            _player.OnHeavyAttack();
        }

        protected override void OnAttackHeld() => Animancer.Play(_heavyAttackWindupClip);

        protected void SpawnAttack(AttackInfo attackInfo) {
            OnPerfectEnd();
            _currentAttack = attackInfo;
            Attack(_currentAttack);
            Vector3 slashSize = Vector3.one * attackInfo.radius;
            slashSize.x *= 0.75f;
            slashSize.y *= 0.75f;
            SpawnSlash(attackInfo.angle, slashSize, attackInfo.leftToRight, 0.5f, 0.25f);
        }

        private void Attack(AttackInfo attackInfo) {
            attackInfo.duration.Start();

            Motor.SetRotation(Quaternion.LookRotation(Character.LastNonZeroLookInput));
            Character.LastNonZeroMoveInput = Character.LastNonZeroLookInput;
            Character.AddVelocity(attackInfo.pushForce * Character.LastNonZeroLookInput);
            Animancer.Play(attackInfo.animation);

            Timing.CallDelayed(HIT_DELAY, () => CheckForHittables(attackInfo), gameObject);
        }

        private void SpawnSlash(float angle, Vector3 size, bool leftToRight = true, float innerRadius = 0.5f, float lifetime = 0.25f) {
            if (_slashInstance)
                PoolManager.Return(_slashInstance);

            Quaternion rot = Quaternion.LookRotation(Character.LastNonZeroLookInput);
            _slashInstance = Instantiate(_slashParticle, _player.CenterOfMass, rot, _player.transform);
            _slashInstance.SetSlashSettings(angle, size, leftToRight, innerRadius, lifetime);
        }
        
        protected virtual void CheckForHittables(AttackInfo attackInfo) {
            Vector3 position = Motor.TransientPosition + Motor.CharacterTransformToCapsuleCenter;
                               // Motor.CharacterForward * attackInfo.zOffset;
            
            DebugExtension.DebugWireSphere(position, Color.red, attackInfo.radius,
                attackInfo.duration.Duration);

            Collider[] _colliders =
                Physics.OverlapSphere(position, attackInfo.radius, LayerManager.Masks.DEFAULT_AND_NPC);

            bool hitSomething = false;
            
            if(_colliders.Length == 0)
                return;

            foreach (Collider col in _colliders) {
                IHittable hittable = col.GetComponentInParent<IHittable>();
                

                Vector3 hitDirection = _player.FeetPosition.DirectionTo(col.transform.position).Flatten();

                float dot = Vector3.Dot(_player.Forward, hitDirection);
                float hitAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;

                if (hittable == null || hitAngle > attackInfo.angle)
                    continue;
            
                HitData hitData = new HitData {
                    hittable = hittable,
                    damage = attackInfo.damage,
                    instigator = _player,
                    dealer = gameObject,
                    playerAttackType = attackInfo.attackType,
                    position = col.ClosestPoint(_player.CenterOfMass),
                    direction = hitDirection
                };
                
                hitSomething = true;
                hittable.Hit(hitData);
                Character.MeleeCombat.OnAttackHit(hitData);
            }

            if (hitSomething)
                PoolManager.Spawn(attackInfo.feedback, Vector3.zero, Quaternion.identity);
        }
    }
}
