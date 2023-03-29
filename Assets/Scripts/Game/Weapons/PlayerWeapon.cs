using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using KinematicCharacterController;
using MEC;
using MoreMountains.Feedbacks;
using ParadoxNotion;
using UnityEngine;

namespace VHS {
    public enum PlayerAttackType {
        LIGHT,
        HEAVY,
        PERFECT_HEAVY,
        DASH_LIGHT,
        DASH_HEAVY,
        RANGE,
        PERFECT_RANGE
    }
    
    
    public abstract class PlayerWeapon : BaseBehaviour { // TODO do the same to Range and maybe seperate by WeaponMelee, WeaponRange
        private const float HIT_DELAY = 0.15f;
        
        [Header("VFX")]
        [SerializeField] private GameObject _slashParticle;
        
        [Header("Common")]
        [SerializeField] private Timer _comboCooldown = new(0.5f);
        
        
        [Header("Attacks")]
        [SerializeField] protected List<AttackInfo> _lightAttacks;
        [SerializeField] protected AttackInfo _heavyAttack;
        [SerializeField] protected AttackInfo _dashLightAttack;
        [SerializeField] protected AttackInfo _dashHeavyAttack;
        [SerializeField] protected ClipTransition _heavyAttackWindupClip;
      
        private GameObject _slashInstance;
        
        protected bool _heavyAttackHeld;
        protected bool _heavyAttackReached;
        
        protected Player _player;
        protected AttackInfo _currentAttack;
        protected CharacterMeleeCombat _meleeController;
        
        protected KinematicCharacterMotor Motor => Character.Motor;
        protected CharacterController Character => _player.CharacterController;
        protected AnimancerComponent Animancer => _player.Animancer;
        protected CharacterAnimationComponent AnimationController => _player.AnimationComponent;

        public Timer ComboCooldown => _comboCooldown;
        public Timer CurrentAttackTimer => _currentAttack?.duration;
        
        public bool IsOnComboCooldown => _comboCooldown.IsActive;
        public bool IsDuringAttack => CurrentAttackTimer is {IsActive: true};
        public int LastLightAttackIndex => _lightAttacks.Count;

        public virtual void Init(CharacterMeleeCombat meleeCombat, Player player) {
            _player = player;
            _meleeController = meleeCombat;

            foreach (AttackInfo attack in _lightAttacks)
                attack.attackType = PlayerAttackType.LIGHT;

            _heavyAttack.attackType = PlayerAttackType.HEAVY;
            _dashLightAttack.attackType = PlayerAttackType.DASH_LIGHT;
            _dashHeavyAttack.attackType = PlayerAttackType.DASH_HEAVY;
        }

        protected override void Enable() {
            foreach (AttackInfo attack in _lightAttacks)
                attack.duration.OnEnd += OnAttackEnd;

            _dashLightAttack.duration.OnEnd += OnAttackEnd;
            _dashHeavyAttack.duration.OnEnd += OnAttackEnd;
            _heavyAttack.duration.OnEnd += OnAttackEnd;
        }

        protected override void Disable() {
            foreach (AttackInfo attack in _lightAttacks)
                attack.duration.OnEnd -= OnAttackEnd;

            _dashLightAttack.duration.OnEnd -= OnAttackEnd;
            _dashHeavyAttack.duration.OnEnd -= OnAttackEnd;
            _heavyAttack.duration.OnEnd -= OnAttackEnd;
        }

        private void OnAttackEnd() => _meleeController.OnAttackEnd();

        public virtual void DashLightAttack() => SpawnAttack(_dashLightAttack, new Vector3(0.3f, 0.3f, 1f));

        public virtual void DashHeavyAttack() => SpawnAttack(_dashHeavyAttack, new Vector3(1, 1f, 0.4f));

        public virtual void LightAttack(int index) => SpawnAttack(_lightAttacks[index], Vector3.one * 0.25f, index % 2 == 0);

        public virtual void HeavyAttack() => SpawnAttack(_heavyAttack, Vector3.one * 0.4f);
        
        public virtual void PerfectHeavyAttack() { }
        
        public virtual void OnHeavyAttackReached() { }
        public virtual void OnHeavyAttackHeld() => Animancer.Play(_heavyAttackWindupClip);

        protected void SpawnAttack(AttackInfo attackInfo, Vector3 slashSize, bool flipSlash = false) {
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

            foreach (Collider collider in _colliders) {
                IHittable hittable = collider.GetComponentInParent<IHittable>();
                

                Vector3 hitDirection = _player.FeetPosition.DirectionTo(collider.transform.position).Flatten();

                float dot = Vector3.Dot(_player.Forward, hitDirection);
                float hitAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;

                if (hittable == null || hitAngle > attackInfo.angle)
                    continue;
                
                HitData hitData = new HitData {
                    damage = attackInfo.damage,
                    actor = _player,
                    dealer = gameObject,
                    playerAttackType = attackInfo.attackType,
                    position = collider.ClosestPoint(_player.CenterOfMass),
                    direction = hitDirection
                };
                
                hitSomething = true;
                hittable.Hit(hitData);
                _meleeController.OnAttackHit(hitData);
            }

            if (hitSomething)
                PoolManager.Spawn(attackInfo.feedback, Vector3.zero, Quaternion.identity);
        }
    }
}
