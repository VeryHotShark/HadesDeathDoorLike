using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class CharacterLockTarget : CharacterModule {
        
        [SerializeField] private float _lockTargetRadius = 50.0f;
        [SerializeField, Range(0.0f, 1.0f)] private float _targetSelectDotThreshold = 0.8f;

        private ITargetable _lockTarget;
        public ITargetable LockTarget => _lockTarget;
        
        public void ToggleLockTarget() => _lockTarget = _lockTarget != null ? null : CheckForTarget();

        private ITargetable CheckForTarget() {
            ITargetable target = null;
            List<ITargetable> targetables = new List<ITargetable>();
            Collider[] colliders = Physics.OverlapSphere(transform.position, _lockTargetRadius, LayerManager.Masks.NPC);

            if (colliders.Length > 0) {
                Vector3 player = transform.position + Motor.CharacterTransformToCapsuleCenter;
                foreach (Collider col in colliders) {
                    if (Physics.Linecast(col.bounds.center, player, LayerManager.Masks.DEFAULT_AND_NPC))
                        continue;

                    ITargetable targetable = col.GetComponentInParent<ITargetable>();

                    if (targetable != null)
                        targetables.Add(targetable);
                }
            }

            if (targetables.Count > 0) {
                ITargetable closestTargetDot = null;
                float closestDistance = Mathf.Infinity;
                float closestDot = Mathf.NegativeInfinity;

                foreach (ITargetable targetable in targetables) {
                    Vector3 playerPosition = Motor.TransientPosition;
                    Vector3 targetPosition = targetable.GetTargetPosition();
                    float distance = playerPosition.DistanceSquaredTo(targetPosition);
                    float dot = Vector3.Dot(playerPosition.DirectionTo(targetPosition).Flatten(), Controller.LookInput);

                    if (distance < closestDistance) {
                        target = targetable;
                        closestDistance = distance;
                    }

                    if (dot > closestDot) {
                        closestDot = dot;
                        closestTargetDot = targetable;
                    }
                }

                if (closestDot > 0.8f)
                    target = closestTargetDot;
            }

            return target;
        }
    }
}