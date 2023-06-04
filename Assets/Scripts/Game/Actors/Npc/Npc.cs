using System;
using MEC;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

namespace VHS {
    public enum NpcState {
        Default,
        Recovery,
        Attacking,
    }
    
    public class Npc : Actor<Npc>, IPoolable {
        [FormerlySerializedAs("_enemyID")] [SerializeField] private EnemyID id;
        [FormerlySerializedAs("_enemyModel")] [SerializeField] private EnemyModel model;

        private NpcState _state;
        private IActor _target;
        private AIAgent _aiAgent;
        private Blackboard _blackboard;
        private NpcUIComponent _uiComponent;
        private NpcPushComponent _pushComponent;
        private NpcStatusComponent _statusComponent;
        private NpcStaggerComponent _staggerComponent;
        private BehaviourTreeOwner _behaviourTreeOwner;

        public Action OnStaggerStart = delegate { };
        public Action OnStaggerEnd = delegate { };

        public bool HasTarget => _target is {IsAlive: true}; // equivalent to != null && IsAlive
        public bool IsDuringStagger => _staggerComponent.DuringStagger;
        
        public Vector3 TargetPosition => _target.FeetPosition;
        public Vector3 DirectionToTarget => HasTarget ? FeetPosition.DirectionTo(Target.FeetPosition) : transform.forward;
        public Vector3 DirectionToTargetFlat => DirectionToTarget.Flatten();
        
        public IActor Target => _target;
        public AIAgent AIAgent => _aiAgent;
        public Blackboard Blackboard => _blackboard;
        public BehaviourTreeOwner BehaviourTreeOwner => _behaviourTreeOwner;
        
        public NpcState State => _state;
        public EnemyID ID => id;
        public EnemyModel Model => model;

        public NpcUIComponent UIComponent => _uiComponent;
        public NpcPushComponent PushComponent => _pushComponent;

        protected override void GetComponents() {
            base.GetComponents();
            _aiAgent = GetComponent<AIAgent>();
            _blackboard = GetComponent<Blackboard>();
            _uiComponent = GetComponent<NpcUIComponent>();
            _pushComponent = GetComponent<NpcPushComponent>();
            _statusComponent = GetComponent<NpcStatusComponent>();
            _staggerComponent = GetComponent<NpcStaggerComponent>();
            _behaviourTreeOwner = GetComponent<BehaviourTreeOwner>();
        }

        private void Start() {
            _target = NpcBlackboard.PlayerInstance;
            
            if(_target != null)
                _blackboard.SetVariableValue("Target", _target.gameObject);
        }

        public override void Hit(HitData hitData) {
            base.Hit(hitData);
            
            if(hitData.statusToApply != null) 
                ApplyStatus(hitData.statusToApply);

            if (_target == null) {
                _target = hitData.instigator;
                _blackboard.SetVariableValue("Target", _target.gameObject);
            }
        }

        public void SetState(NpcState newState) => _state = newState;

        public void Push(float duration, float distance, Vector3 direction) => _pushComponent.Push(duration, distance, direction);
        public void Stagger(float duration) => _staggerComponent.Stagger(duration);

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