using Animancer;
using KinematicCharacterController;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VHS {
    public enum PlayerAttackType {
        LIGHT,
        HEAVY,
        PERFECT_HEAVY,
        DASH_LIGHT,
        DASH_HEAVY,
        RANGE,
        PERFECT_RANGE
    }
    
    
    public abstract class Weapon : BaseBehaviour { // TODO do the same to Range and maybe seperate by WeaponMelee, WeaponRange
        [Header("Common")]
        [SerializeField] protected Timer _cooldown = new(0.5f);

        [TitleGroup("Input")]
        [SerializeField, MinMaxSlider(0.0f, 2.0f)] protected Vector2 _perfectWindow = new Vector2(0.7f, 1.0f);
        [SerializeField] protected Feedback _perfectWindowFeedback;
        [SerializeField] protected float _minHoldDuration = 0.0f;

        private MeshRenderer[] _renderers;
        protected Player _player;
        
        protected bool _lastAttackPerfect;
        protected float _heldInputDuration = 0.0f;
        
        protected Timer _perfectWindowTimer;
        protected CoroutineHandle _perfectWindowCoroutine;

        protected KinematicCharacterMotor Motor => Character.Motor;
        protected CharacterController Character => _player.CharacterController;
        protected AnimancerComponent Animancer => _player.Animancer;
        protected AnimationController AnimationController => _player.AnimationController;
        
        public Timer Cooldown => _cooldown;
        public bool IsOnCooldown => _cooldown.IsActive;
        public bool MinInputReached => _heldInputDuration > _minHoldDuration;

        protected virtual void Awake() {
            _renderers = GetComponentsInChildren<MeshRenderer>();
            _perfectWindowTimer = new Timer(_perfectWindow.Range());
        }

        protected override void Enable() => _perfectWindowTimer.OnEnd += OnPerfectEnd;
        protected override void Disable() => _perfectWindowTimer.OnEnd -= OnPerfectEnd;

        public virtual void Init(Player player) => _player = player;
        
        public virtual void OnWeaponStart() {
            _heldInputDuration = 0.0f;
            _perfectWindowCoroutine = Timing.CallDelayed(_perfectWindow.x, OnPerfectStart);
        }

        public virtual void OnWeaponStop() => OnPerfectEnd();

        public void AttackHeld() {
            _heldInputDuration += Time.deltaTime;
            OnAttackHeld();
        }
        
        protected virtual void OnAttackHeld() { }

        public void OnAttackReleased() {
            OnPerfectEnd();
            AnimationController.UnpauseGraph();
            _lastAttackPerfect = _heldInputDuration.IsWithinRange(_perfectWindow);
            
            if(_heldInputDuration < _minHoldDuration)
                return;
            
            OnSuccessfulAttackReleased();
        }

        protected virtual void OnSuccessfulAttackReleased() {
            if (_lastAttackPerfect) 
                OnPerfectHoldAttack();
            else 
                OnRegularHoldAttack();                
        }

        protected virtual void OnPerfectStart() {
            _perfectWindowTimer.Start();
            PoolManager.Spawn(_perfectWindowFeedback, _player.CenterOfMass, Quaternion.identity);
        }
        
        protected virtual void OnPerfectEnd() {
            _perfectWindowTimer.Reset();
            Timing.KillCoroutines(_perfectWindowCoroutine);
        }
        
        protected virtual void OnPerfectHoldAttack() => OnRegularHoldAttack();
        protected virtual void OnRegularHoldAttack() { }
        
        public void SetVisible(bool state) {
            foreach (MeshRenderer renderer in _renderers)
                renderer.enabled = state;
        }

        public void StartCooldown() {
            if(_cooldown.Duration > 0.0f)
                _cooldown.Start();
        }
    }
}
