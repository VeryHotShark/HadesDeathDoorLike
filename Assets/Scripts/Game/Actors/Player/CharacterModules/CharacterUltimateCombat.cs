using UnityEngine;

namespace VHS {
    public class CharacterUltimateCombat : CharacterModule {
        [SerializeField] private float _requiredDealDamage = 20.0f;
        [SerializeField] private float _duration = 3.0f;

        private float _currentDuration = 0.0f;
        private float _currentUltimateFill = 0.0f;
        public float CurrentFillAmount => _currentUltimateFill / _requiredDealDamage;
        
        protected override void Enable() {
            Parent.OnMeleeHit += OnMeleeHit;
            Parent.OnRangeHit += OnRangeHit;
        }
        
        protected override void Disable() {
            Parent.OnMeleeHit -= OnMeleeHit;
            Parent.OnRangeHit -= OnRangeHit;
        }

        private void OnMeleeHit(HitData hitData) {
            if(Controller.CurrentState != this)
                ChangeUltimateFill(hitData.damage);
        }

        private void OnRangeHit(HitData hitData) {
            if(Controller.CurrentState != this)
                ChangeUltimateFill(hitData.damage / 2.0f);
        }

        private void ChangeUltimateFill(float amount) {
            _currentUltimateFill += amount;
            _currentUltimateFill = Mathf.Clamp(_currentUltimateFill,0.0f, _requiredDealDamage);
            Parent.OnUltimatePercentChanged(CurrentFillAmount);
        }

        public override void OnEnter() => _currentDuration = _duration;
        public override void OnExit() => _currentUltimateFill = 0.0f;

        public override void OnTick(float deltaTime) {
            _currentDuration -= deltaTime;
            _currentDuration = Mathf.Max(_currentDuration, 0.0f);
            Parent.OnUltimatePercentChanged(_currentDuration / _duration);
            
            if(CanExitState())
                Controller.TransitionToDefaultState();
        }
        
        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) => currentVelocity = Vector3.zero;

        public override bool CanEnterState() => CurrentFillAmount >= 1.0f;
        public override bool CanExitState() => _currentDuration <= 0.0f;
    }
}
