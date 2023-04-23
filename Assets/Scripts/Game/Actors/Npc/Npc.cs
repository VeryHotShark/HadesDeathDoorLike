using MEC;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using Pathfinding;
using UnityEngine;

namespace VHS {
    public enum NpcState {
        Default,
        Recovery,
        Attacking,
    }
    
    public class Npc : Actor<Npc>, IPoolable {
        [SerializeField] private EnemyID _enemyID;

        private NpcState _state;
        private IActor _target;
        private AIAgent _aiAgent;
        private Blackboard _blackboard;
        private NpcStatusComponent _statusComponent;
        private BehaviourTreeOwner _behaviourTreeOwner;

        public bool HasTarget => _target is {IsAlive: true}; // equivalent to != null && IsAlive
        public Vector3 TargetPosition => _target.FeetPosition;
        public Vector3 DirectionToTarget => HasTarget ? FeetPosition.DirectionTo(Target.FeetPosition) : transform.forward;
        public Vector3 DirectionToTargetFlat => DirectionToTarget.Flatten();
        
        public NpcState State => _state;
        public IActor Target => _target;
        public AIAgent AIAgent => _aiAgent;
        public EnemyID EnemyID => _enemyID;
        public Blackboard Blackboard => _blackboard;
        public BehaviourTreeOwner BehaviourTreeOwner => _behaviourTreeOwner;

        protected override void GetComponents() {
            base.GetComponents();
            _aiAgent = GetComponent<AIAgent>();
            _blackboard = GetComponent<Blackboard>();
            _statusComponent = GetComponent<NpcStatusComponent>();
            _behaviourTreeOwner = GetComponent<BehaviourTreeOwner>();
        }

        private void Start() {
            _target = NpcBlackboard.PlayerInstance; // Dependency Injection?
            
            if(_target != null)
                _blackboard.SetVariableValue("Target", _target.gameObject);
        }

        public override void Hit(HitData hitData) {
            base.Hit(hitData);
            
            if(hitData.statusToApply != null) 
                ApplyStatus(hitData.statusToApply);

            if (_target == null) {
                _target = hitData.instigator; // Dependency Injection?
                _blackboard.SetVariableValue("Target", _target.gameObject);
            }
        }

        public void SetState(NpcState newState) => _state = newState;

        public void Stagger(float duration) {
            SetState(NpcState.Recovery);
            Timing.CallDelayed(duration, () => SetState(NpcState.Default), ((Component)this).gameObject);
        }

        public void ApplyStatus(Status status) => _statusComponent.ApplyStatus(status);
        public void RemoveStatus(Status status) => _statusComponent.RemoveStatus(status);

        public void Kill(IActor dealer) {
            HitData hitData = new HitData {
                damage = HitPoints.Max,
                instigator = dealer,
                position = transform.position,
                direction = -transform.forward
            };

            Hit(hitData);
        }
    }
}