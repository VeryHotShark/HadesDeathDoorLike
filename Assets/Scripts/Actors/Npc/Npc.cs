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
        private GameObject _target;
        private RichAI _richAI;

        public RichAI RichAI => _richAI;
        public NpcState State => _state;
        public GameObject Target => _target; // zmień na Actor

        public Vector3 DirectionToTarget =>
            Target ? FeetPosition.DirectionTo(Target.transform.position).Flatten() : transform.forward;

        protected override void GetComponents() {
            base.GetComponents();
            _richAI = GetComponent<RichAI>();
            _hitProcessorComponent = GetComponent<HitProcessorComponent>();
        }

        protected override void Initialize() {
            Log("Test Log");
        }

        private void Start() {
            _target = NpcBlackboard.PlayerInstance.gameObject; // Dependency Injection?
        }

        public Vector3 GetTargetPosition() => transform.position;
        public void SetState(NpcState newState) => _state = newState;
    }
}