using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class ProjectileBullet : Projectile, IUpdateListener {
        [SerializeField] private float _speed = 10.0f;
        [SerializeField] private float _gravity;

        private Vector3 _lastPosition;
        private RaycastHit _hitInfo;

        public override void Init(IActor owner) {
            base.Init(owner);
            _lastPosition = transform.position;
        }

        private void OnEnable() => UpdateManager.AddUpdateListener(this);
        private void OnDisable() => UpdateManager.RemoveUpdateListener(this);

        public void OnUpdate(float deltaTime) {
            UpdateMovement(deltaTime);

            if(CheckForCollision())
                OnHit();
        }

        private void UpdateMovement(float deltaTime) {
            Vector3 downwardMovement = Vector3.down * (_gravity * deltaTime);
            Vector3 forwardMovement = transform.forward * (_speed * deltaTime);
            transform.position += forwardMovement + downwardMovement;
        }

        protected override bool CheckForCollision() =>
            Physics.Linecast(_lastPosition, transform.position, out _hitInfo,
                LayerManager.Masks.DEFAULT_AND_NPC);

        protected override void OnHit() {
            IHittable hittable = _hitInfo.transform.GetComponentInParent<IHittable>();

            if (hittable != null) {
                HitData hitData = new HitData {
                    dealer = _owner
                };
                
                hittable.Hit(hitData);
            }
            
            Destroy(gameObject);
        }
    }
}