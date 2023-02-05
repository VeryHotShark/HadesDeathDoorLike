using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class NpcSkillMeleeAttack : NpcSkill {
        public float _distance = 1.0f;
        public float _radius = 1.0f;
        public float _zOffset = 1.0f;

        private HashSet<IHittable> _hittables = new HashSet<IHittable>();
        private Collider[] _colliders = new Collider[32];

        public override void StartTarget_Hook() {
            base.StartTarget_Hook();         
            Owner.AIAgent.ResetPath();
            Owner.AIAgent.Stop();
            Owner.SetState(NpcState.Attacking);
        }

        public override void TickTarget(float deltaTime) => Owner.transform.rotation = Quaternion.LookRotation(Owner.DirectionToTarget);

        public override void StartSkill_Hook() {
            base.StartSkill_Hook();
            _hittables.Clear();
        }

        public override void TickSkill(float deltaTime) {
            MoveCharacter(deltaTime);
            CheckForHittables();    
        }

        public override void FinishSkill_Hook() {
            base.FinishSkill_Hook();
            
            Owner.AIAgent.Resume();
            
            // TODO rework this, temporary for stagger to work because this gets called after stagger
            if(Owner.State != NpcState.Recovery)
                Owner.SetState(NpcState.Default);
        }

        private void MoveCharacter(float deltaTime) {
            float speed = _distance / SkillDuration;
            Vector3 delta = Owner.Forward * (speed * deltaTime);
            Owner.AIAgent.Move(delta);
        }

        private void CheckForHittables() {
            Vector3 position = Owner.CenterOfMass + Owner.Forward * _zOffset;

            int hitCount = Physics.OverlapSphereNonAlloc(position, _radius, _colliders ,LayerManager.Masks.PLAYER);
            
            if (hitCount > 0) {
                for (int i = 0; i < hitCount; i++) {
                    Collider collider = _colliders[i];
                    IHittable hittable = collider.GetComponentInParent<IHittable>();

                    if (hittable != null) {
                        if (_hittables.Contains(hittable))
                            continue;

                        HitData hitData = new HitData {
                            damage = 1,
                            actor = Owner,
                            position = collider.ClosestPoint(Owner.CenterOfMass),
                            direction = Owner.FeetPosition.DirectionTo(collider.transform.position)
                        };

                        hittable.Hit(hitData);
                        _hittables.Add(hittable);
                    }
                }
                
                FinishSkill();
            } 
            
            DebugExtension.DebugWireSphere(position, Color.red, _radius);
        }
    }
}
