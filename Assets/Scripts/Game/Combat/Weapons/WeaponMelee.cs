using System.Collections;
using System.Collections.Generic;
using Animancer;
using Animancer.Examples.Events;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VHS {
    public abstract  class WeaponMelee : Weapon
    {
        private const float HIT_DELAY = 0.15f;
        
        [Header("VFX")]
        [SerializeField] private SlashController _slashParticle;
        
        [TitleGroup("Input")]
        [SerializeField] protected Timer _recoveryTimer = new(0.5f);
        
        
        [Header("Attacks")]
        [SerializeField] protected List<AttackInfo> _lightAttacks;
        [SerializeField] protected AttackInfo _heavyAttack;
        [SerializeField] protected AttackInfo _perfectHeavyAttack;
        [SerializeField] protected AttackInfo _dashAttack;
        [SerializeField] protected ClipTransition _heavyAttackWindupClip;

        private bool _coroutineStarted; // TODO temporary because MEC Free doesnt have IsRunning Field
        private SlashController _slashInstance;

        protected AnimancerEvent.Sequence _events;
        protected AttackInfo _currentAttack;

        public Timer RecoveryTimer => _recoveryTimer;
        public bool IsDuringRecovery => _recoveryTimer.IsActive;
        public bool IsDuringAttack => _currentAttack != null && _currentAttack.animation.State.IsPlayingAndNotEnding();
        public int LastLightAttackIndex => _lightAttacks.Count;
        
        public override void Init(Player player) {
            base.Init(player);

            foreach (AttackInfo attack in _lightAttacks) 
                attack.attackType = PlayerAttackType.LIGHT;

            _heavyAttack.attackType = PlayerAttackType.HEAVY;
            _dashAttack.attackType = PlayerAttackType.DASH_ATTACK;
            _perfectHeavyAttack.attackType = PlayerAttackType.PERFECT_HEAVY;
            
            // We do this so the same animation wont share the events
            // we assign them on play
            _events = new AnimancerEvent.Sequence {
                OnEnd = OnAttackEnd
            };
        }
        
        protected void OnAttackEnd() {
            EventUtilities.PauseAtCurrentEvent();
            Character.MeleeCombat.OnAttackEnd();
        }

        public override void OnChargeStart() {
            if(!_coroutineStarted)
                base.OnChargeStart();

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

        public virtual void DashAttack() => SpawnAttack(_dashAttack);

        public virtual void LightAttack(int index) {
            SpawnAttack(_lightAttacks[index]);
        }

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
            _recoveryTimer.Reset();
            _currentAttack = attackInfo;
            Attack(_currentAttack);
            
            Vector3 slashSize = Vector3.one * attackInfo.radius;
            slashSize.x *= 0.75f;
            slashSize.y *= 0.75f;
            SpawnSlash(attackInfo.angle, slashSize, attackInfo.leftToRight, 0.5f, 0.25f);
        }

        private void Attack(AttackInfo attackInfo) {
            Motor.SetRotation(Quaternion.LookRotation(Character.LastNonZeroLookInput));
            Character.LastNonZeroMoveInput = Character.LastNonZeroLookInput;
            Character.AddVelocity(attackInfo.pushForce * Character.LastNonZeroLookInput);
            
            AnimancerState state = Animancer.Play(attackInfo.animation);
            _events.NormalizedEndTime = state.NormalizedEndTime;
            state.Events = _events;

            Timing.CallDelayed(HIT_DELAY, () => CheckForHittables(attackInfo), gameObject);
        }

        private void SpawnSlash(float angle, Vector3 size, bool leftToRight = true, float innerRadius = 0.5f, float lifetime = 0.25f) {
            if (_slashInstance)
                PoolManager.Return(_slashInstance);

            _slashInstance = PoolManager.Spawn(_slashParticle, Motor.CharacterTransformToCapsuleCenter, Quaternion.identity, _player.transform);
            _slashInstance.SetSlashSettings(angle, size, leftToRight, innerRadius, lifetime);
        }
        
        protected virtual void CheckForHittables(AttackInfo attackInfo) {
            Vector3 position = Motor.TransientPosition + Motor.CharacterTransformToCapsuleCenter;
                               // Motor.CharacterForward * attackInfo.zOffset;
            
            DebugExtension.DebugWireSphere(position, Color.red, attackInfo.radius,
                attackInfo.animation.Length);

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
