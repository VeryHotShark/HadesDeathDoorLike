using MEC;
using NodeCanvas.Framework;
using Pathfinding;
using UnityEngine;

namespace VHS {
    public enum NpcState {
        Default,
        Recovery,
        Attacking,
    }
    
    public class Npc : Actor<Npc>, ITargetable, IPoolable {

        private NpcState _state;
        private IActor _target;
        private AIAgent _aiAgent;
        private Blackboard _blackboard;

        public bool HasTarget => _target is {IsAlive: true}; // equivalent to != null && IsAlive
        public Vector3 TargetPosition => _target.FeetPosition;
        public Vector3 DirectionToTarget => HasTarget ? FeetPosition.DirectionTo(Target.FeetPosition).Flatten() : transform.forward;
        
        public NpcState State => _state;
        public IActor Target => _target;
        public AIAgent AIAgent => _aiAgent;

        protected override void GetComponents() {
            base.GetComponents();
            _aiAgent = GetComponent<AIAgent>();
            _blackboard = GetComponent<Blackboard>();
            _hitProcessorComponent = GetComponent<HitProcessorComponent>();
        }

        private void Start() {
            _target = NpcBlackboard.PlayerInstance; // Dependency Injection?
            
            if(_target != null)
                _blackboard.SetVariableValue("Target", _target.GameObject);
        }

        public override void Hit(HitData hitData) {
            base.Hit(hitData);

            if (_target == null) {
                _target = hitData.dealer; // Dependency Injection?
                _blackboard.SetVariableValue("Target", _target.GameObject);
            }
        }

        public Vector3 GetTargetPosition() => transform.position;
        public void SetState(NpcState newState) => _state = newState;

        public override void OnMyAttackParried(HitData hitData) {
            // TODO make seperate Stagger ?
            Stagger(1f);
        }

        private void Stagger(float duration) {
            SetState(NpcState.Recovery);
            Timing.CallDelayed(duration, () => SetState(NpcState.Default), gameObject);
        }

        public void Kill(IActor dealer) {
            HitData hitData = new HitData {
                damage = 1,
                dealer = dealer,
                position = transform.position,
                direction = -transform.forward
            };

            Hit(hitData);
        }
    }
}