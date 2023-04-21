using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class ProjectileBullet : Projectile, IUpdateListener {
        [SerializeField] private float _speed = 10.0f;
        [SerializeField] private float _gravity;

        private float _runtimeSpeed;
        private Vector3 _lastPosition;
        private RaycastHit _hitInfo;

        public override void Init(IActor owner) {
            base.Init(owner);
            _runtimeSpeed = _speed;
            _lastPosition = transform.position;
        }

        protected override void Enable() => UpdateManager.AddUpdateListener(this);
        protected override void Disable() => UpdateManager.RemoveUpdateListener(this);

        public void OnUpdate(float deltaTime) {
            UpdateMovement(deltaTime);
            
            Debug.DrawLine(_lastPosition,transform.position, Color.blue);

            if (CheckForCollision())    
                Hit();

            _lastPosition = transform.position;
        }

        private void UpdateMovement(float deltaTime) {
            Vector3 downwardMovement = Vector3.down * (_gravity * deltaTime);
            Vector3 forwardMovement = transform.forward * (_runtimeSpeed * deltaTime);
            transform.position += forwardMovement + downwardMovement;
        }

        protected override bool CheckForCollision() =>
            Physics.Linecast(_lastPosition, transform.position, out _hitInfo,
                LayerManager.Masks.DEFAULT_AND_ACTORS);

        protected override void Hit() {
            IHittable hittable = _hitInfo.transform.GetComponentInParent<IHittable>();
            
            if (hittable != null) {
                HitData hitData = new HitData {
                    position = _hitInfo.point,
                    hittable = hittable,
                    dealer = gameObject,
                    instigator = _owner,
                    damage = _damage,
                    playerAttackType = PlayerAttackType.RANGE,
                };
                
                hittable.Hit(hitData);
                RaiseEvents(hitData);
                PlayHitFX();
            }

            PoolManager.Return(this);
        }

        public void SetSpeed(float speed) => _runtimeSpeed = speed;
        public override void OnReturnToPool() => _runtimeSpeed = _speed;
    }
}