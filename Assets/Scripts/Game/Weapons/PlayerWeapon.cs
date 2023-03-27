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
    [Serializable]
    public class AttackInfo {
        public int damage = 1;
        public Timer duration = new(0.3f);
        public float pushForce = 10.0f;
        public float zOffset = 1.0f;
        public float radius = 1.0f;
        [Range(0.0f,180.0f)] public float angle = 180.0f;

        public ClipTransition animation;
        public MMF_Player feedback;
    }
    public class PlayerWeapon : BaseBehaviour {
        private const float HIT_DELAY = 0.2f;
        
        [Header("VFX")]
        [SerializeField] private GameObject _slashParticle;
        
        [Header("Common")]
        [SerializeField] private Timer _comboCooldown = new(0.5f);
        
        
        [Header("Attacks")]
        [SerializeField] private List<AttackInfo> _lightAttacks;
        [SerializeField] private AttackInfo _heavyAttack;
        [SerializeField] private AttackInfo _dashLightAttack;
        [SerializeField] private AttackInfo _dashHeavyAttack;
        
        private int _lightAttackIndex;
        private GameObject _slashInstance;
        
        protected bool _heavyAttackHeld;
        protected bool _heavyAttackReached;
        
        protected Player _player;
        protected AttackInfo _currentAttack;
        protected CharacterMeleeCombat _meleeController;

        protected CharacterController Character => _player.CharacterController;
        protected KinematicCharacterMotor Motor => Character.Motor;
        protected CharacterAnimationComponent AnimationController => _player.AnimationComponent;
        
        public Timer CurrentAttackTimer => _currentAttack.duration;
        public int LightAttackIndex => _lightAttackIndex;
        public bool IsOnComboCooldown => _comboCooldown.IsActive;
        public bool IsDuringAttack => CurrentAttackTimer.IsActive;
        public bool IsDuringLastAttack => _lightAttackIndex >= _lightAttacks.Count;

        public void Init(CharacterMeleeCombat meleeCombat, Player player) {
            _player = player;
            _meleeController = meleeCombat;
        }
        
        public virtual void OnPrimaryPressed() {
            
        }
        
        public virtual void OnPrimaryHeld() {
            
        }
        
        public virtual void OnPrimaryReleased() {
            
        }
        
        public virtual void OnPrimaryPerformed() {
            
        }
        
        private void ResetLightAttackIndex() => _lightAttackIndex = 0;

        private void DashLightAttack() {
            ResetLightAttackIndex();
            _meleeController.OnDashLightAttack(this);
            SpawnAttack(_dashLightAttack, new Vector3(0.3f, 0.3f, 1f));
        }

        private void DashHeavyAttack() {
            ResetLightAttackIndex();
            _meleeController.OnDashHeavyAttack(this);
            SpawnAttack(_dashHeavyAttack, new Vector3(1, 1f, 0.4f));
        }

        private void LightAttack() {
            _meleeController.OnLightAttack(this);
            SpawnAttack(_lightAttacks[_lightAttackIndex], Vector3.one * 0.25f, _lightAttackIndex % 2 == 0);
            _lightAttackIndex++;
        }

        private void HeavyAttack() {
            _meleeController.OnHeavyAttack(this);
            ResetLightAttackIndex();
            SpawnAttack(_heavyAttack, Vector3.one * 0.4f);
        }
        
        private void SpawnAttack(AttackInfo attackInfo, Vector3 slashSize, bool flipSlash = false) {
            _currentAttack = attackInfo;
            Attack(_currentAttack);
            SpawnSlash(slashSize, flipSlash);
        }

        private void Attack(AttackInfo attackInfo) {
            attackInfo.duration.Start();

            Motor.SetRotation(Character.LastCharacterInputs.CursorRotation);
            Character.LastNonZeroMoveInput = Character.LookInput;
            Character.AddVelocity(attackInfo.pushForce * Character.LookInput);

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
        
        protected void CheckForHittables(AttackInfo attackInfo) {
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
                    position = collider.ClosestPoint(_player.CenterOfMass),
                    direction = hitDirection
                };
                
                hitSomething = true;
                hittable.Hit(hitData);
                _meleeController.OnAttackHit(this, hitData);
            }

            if (hitSomething) 
                attackInfo.feedback.PlayFeedbacks();
        }
    }
}
