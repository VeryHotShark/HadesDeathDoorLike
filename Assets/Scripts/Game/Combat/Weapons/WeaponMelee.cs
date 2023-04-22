using System.Collections;
using System.Collections.Generic;
using Animancer;
using MEC;
using UnityEngine;

namespace VHS {
    public abstract  class WeaponMelee : Weapon
    {
        private const float HIT_DELAY = 0.15f;
        
        [Header("VFX")]
        [SerializeField] private GameObject _slashParticle;
        
        [Header("Attacks")]
        [SerializeField] protected List<AttackInfo> _lightAttacks;
        [SerializeField] protected AttackInfo _heavyAttack;
        [SerializeField] protected AttackInfo _perfectHeavyAttack;
        [SerializeField] protected AttackInfo _dashLightAttack;
        [SerializeField] protected AttackInfo _dashHeavyAttack;
        [SerializeField] protected ClipTransition _heavyAttackWindupClip;

        private bool _coroutineStarted; // TODO temporary because MEC Free doesnt have IsRunning Field
        private GameObject _slashInstance;
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
            _dashLightAttack.attackType = PlayerAttackType.DASH_LIGHT;
            _dashHeavyAttack.attackType = PlayerAttackType.DASH_HEAVY;
            _perfectHeavyAttack.attackType = PlayerAttackType.PERFECT_HEAVY;
        }

        protected override void Enable() {
            base.Enable();
            foreach (AttackInfo attack in _lightAttacks)
                attack.duration.OnEnd += OnAttackEnd;

            _perfectHeavyAttack.duration.OnEnd += OnAttackEnd;
            _dashLightAttack.duration.OnEnd += OnAttackEnd;
            _dashHeavyAttack.duration.OnEnd += OnAttackEnd;
            _heavyAttack.duration.OnEnd += OnAttackEnd;
        }

        protected override void Disable() {
            base.Disable();
            foreach (AttackInfo attack in _lightAttacks)
                attack.duration.OnEnd -= OnAttackEnd;

            _perfectHeavyAttack.duration.OnEnd -= OnAttackEnd;
            _dashLightAttack.duration.OnEnd -= OnAttackEnd;
            _dashHeavyAttack.duration.OnEnd -= OnAttackEnd;
            _heavyAttack.duration.OnEnd -= OnAttackEnd;
        }
        
        private void OnAttackEnd() => Character.MeleeCombat.OnAttackEnd();

        public virtual void DashLightAttack() => SpawnAttack(_dashLightAttack, new Vector3(0.3f, 0.3f, 1f));

        public virtual void DashHeavyAttack() => SpawnAttack(_dashHeavyAttack, new Vector3(1, 1f, 0.4f));

        public virtual void LightAttack(int index) => SpawnAttack(_lightAttacks[index], Vector3.one * 0.25f, index % 2 == 0);

        protected override void OnPerfectHoldAttack() {
            SpawnAttack(_perfectHeavyAttack, Vector3.one);
            _player.OnPerfectMeleeAttack();
        }

        protected override void OnRegularHoldAttack() {
            SpawnAttack(_heavyAttack, Vector3.one * 0.4f);
            _player.OnHeavyAttack();
        }

        public virtual void OnHeavyAttackHeld() {
            Animancer.Play(_heavyAttackWindupClip);
            _heldInputDuration += Time.deltaTime;
        }

        protected void SpawnAttack(AttackInfo attackInfo, Vector3 slashSize, bool flipSlash = false) {
            OnPerfectEnd();
            _currentAttack = attackInfo;
            Attack(_currentAttack);
            SpawnSlash(slashSize, flipSlash);
        }

        private void Attack(AttackInfo attackInfo) {
            attackInfo.duration.Start();

            Motor.SetRotation(Character.LastCharacterInputs.CursorRotation);
            Character.LastNonZeroMoveInput = Character.LookInput;
            Character.AddVelocity(attackInfo.pushForce * Character.LookInput);
            Animancer.Play(attackInfo.animation);

            Timing.CallDelayed(HIT_DELAY, () => CheckForHittables(attackInfo), gameObject);
        }

        private void SpawnSlash(Vector3 size, bool flip = false) {
            if (_slashInstance)
                Destroy(_slashInstance);

            Quaternion rot = Quaternion.LookRotation(Character.LookInput);
            _slashInstance = Instantiate(_slashParticle, _player.CenterOfMass, rot, _player.transform);

            size.z *= 1.2f;

            if (flip)
                size.x *= -1.0f;

            _slashInstance.transform.localScale = size;
        }
        
        protected virtual void CheckForHittables(AttackInfo attackInfo) {
            Vector3 position = Motor.TransientPosition + Motor.CharacterTransformToCapsuleCenter +
                               Motor.CharacterForward * attackInfo.zOffset;
            
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
