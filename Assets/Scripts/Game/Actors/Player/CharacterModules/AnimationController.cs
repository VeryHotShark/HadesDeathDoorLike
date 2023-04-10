using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VHS {
    public class AnimationController : ChildBehaviour<Player>, IUpdateListener {
        [TitleGroup("Movement")]
        [SerializeField] private ClipTransition _idleClip;
        [SerializeField] private ClipTransition _moveClip;

        [Space]
        [SerializeField] private ClipTransition _rollClip;

        private AnimancerComponent _animancer;
        public AnimancerComponent Animancer => _animancer;
        private CharacterController Character => Parent.CharacterController;

        public bool IsPaused => _animancer.Playable.IsGraphPlaying;

        private void Awake() => _animancer = GetComponentInChildren<AnimancerComponent>();

        protected override void Enable() {
            Parent.OnRoll += OnRoll;
            UpdateManager.AddUpdateListener(this);
        }

        protected override void Disable() {
            Parent.OnRoll -= OnRoll;
            UpdateManager.RemoveUpdateListener(this);
        }

        private void OnRoll() => _animancer.Play(_rollClip);

        public void OnUpdate(float deltaTime) {
            switch (Parent.CharacterController.CurrentState) {
                case CharacterMovement movement:
                    _animancer.Play(Character.Motor.Velocity != Vector3.zero && Character.MoveInput != Vector3.zero ? _moveClip : _idleClip);
                    break;
            }
        }

        public void PauseGraph() => _animancer.Playable.PauseGraph();
        public void UnpauseGraph() => _animancer.Playable.UnpauseGraph();
    }
}
