using NodeCanvas.Framework;
using Pathfinding;
using UnityEngine;

namespace VHS {
    public enum NpcState {
        Default,
        Recovery,
        Attacking,
    }
    
    public class Npc : Actor, ITargetable {

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

        protected override void Initialize() {
        }

        private void Start() {
            _target = NpcBlackboard.PlayerInstance; // Dependency Injection?
            _blackboard.SetVariableValue("Target", NpcBlackboard.PlayerInstance.gameObject);
        }

        public Vector3 GetTargetPosition() => transform.position;
        public void SetState(NpcState newState) => _state = newState;
    }
}