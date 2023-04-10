using System.Collections;
using System.Collections.Generic;
using Animancer;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VHS {
    public class WeaponRange : Weapon {
        [TitleGroup("Common")]
        [SerializeField] private int _ammoCost = 1;
        [SerializeField] private int _maxAmmoCount = 4;
        [SerializeField] protected Projectile _projectile;
        [SerializeField] private Feedback _perfectWindowFeedback;

        [TitleGroup("Input")]
        [SerializeField, MinMaxSlider(0.0f, 2.0f)]
        private Vector2 _perfectWindow = new Vector2(0.7f, 1.0f);
        [SerializeField] private float _minHoldDuration = 0.0f; 
        
        [TitleGroup("Animations")]
        [SerializeField] private ClipTransition _shootClip;
        [SerializeField] private ClipTransition _shootWindupClip;

        private int _currentAmmo;
        private bool _lastShotPerfect;
        private float _heldInputDuration = 0.0f;

        private Timer _perfectWindowTimer;
        private CoroutineHandle _perfectWindowCoroutine;
        
        public bool HasAmmo => _currentAmmo > 0;
        public int MaxAmmoCount => _maxAmmoCount;

        public Vector2 PerfectWindow => _perfectWindow;
        public float MinHoldDuration => _minHoldDuration;

        public override void Init(Player player) {
            base.Init(player);
            _currentAmmo = _maxAmmoCount;
        }

        protected override void Awake() {
            base.Awake();
            _perfectWindowTimer = new Timer(_perfectWindow.Range());
        }

        protected override void Enable() {
            _shootWindupClip.Events.OnEnd += PauseGraph;
            _perfectWindowTimer.OnEnd += OnPerfectRangeEnd;
        }

        protected override void Disable() {
            _shootWindupClip.Events.OnEnd -= PauseGraph;
            _perfectWindowTimer.OnEnd -= OnPerfectRangeEnd;
        }

        private void PauseGraph() => AnimationController.PauseGraph();

        public virtual void OnRangeAttackStart() {
            _heldInputDuration = 0.0f;
            _perfectWindowCoroutine = Timing.CallDelayed(_perfectWindow.x, OnPerfectRangeStart);
        }

        private void OnPerfectRangeStart() {
            _perfectWindowTimer.Start();
            _player.OnPerfectRangeAttackStart();
            PoolManager.Spawn(_perfectWindowFeedback, _player.CenterOfMass, Quaternion.identity);
        }

        private void OnPerfectRangeEnd() {
            _perfectWindowTimer.Reset();
            Timing.KillCoroutines(_perfectWindowCoroutine);
            _player.OnPerfectRangeAttackEnd();
        }
        
        public virtual void OnRangeAttackReleased() {
            OnPerfectRangeEnd();
            AnimationController.UnpauseGraph();
            
            if(_heldInputDuration < _minHoldDuration)
                return;
            
            Animancer.Play(_shootClip);

            ModifyCurrentAmmo(-_ammoCost);
            _lastShotPerfect = _heldInputDuration.IsWithinRange(_perfectWindow);
            
            if (_lastShotPerfect) {
                OnPerfectRangeAttack();
                _player.OnPerfectRangeAttack();
            }
            else {
                OnRangeAttack();
                _player.OnRangeAttack();
            }
        }
        
        protected virtual void OnRangeAttack() {
            Projectile projectile = Character.RangeCombat.SpawnProjectile(_projectile);
            OnProjectileShot(projectile);
        }

        protected virtual void OnPerfectRangeAttack() => OnRangeAttack();

        public virtual void OnRangeAttackHeld() {
            Animancer.Play(_shootWindupClip);
            _heldInputDuration += Time.deltaTime;
        }

        public virtual void OnRangeAttackReached() { }

        protected virtual void OnProjectileShot(Projectile projectile) { }
        
        public void ModifyCurrentAmmo(int amount) {
            _currentAmmo = Mathf.Clamp(_currentAmmo + amount, 0, _maxAmmoCount);
            _player.OnCurrentAmmoChanged(_currentAmmo);
        }
    }
}
