using UnityEngine;

namespace VHS {
    public class CharacterUltimateCombat : CharacterModule {
        [SerializeField] private float _requiredDealDamage = 20.0f;
        [SerializeField] private float _duration = 3.0f;

        private float _currentDuration = 0.0f;
        private float _currentDealtDamage = 0.0f;
        
        public float CurrentDamagePercent => _currentDealtDamage / _requiredDealDamage;
        public float CurrentDurationPercent => _currentDuration / _duration;
        
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
                UpdateDealtDamage(hitData.damage);
        }

        private void OnRangeHit(HitData hitData) {
            if(Controller.CurrentState != this)
                UpdateDealtDamage(hitData.damage / 2.0f);
        }

        private void UpdateDealtDamage(float amount) {
            _currentDealtDamage += amount;
            _currentDealtDamage = Mathf.Clamp(_currentDealtDamage,0.0f, _requiredDealDamage);
            Parent.OnUltimatePercentChanged(CurrentDamagePercent);
        }

        public override void OnEnter() {
            _currentDuration = _duration;
            Parent.OnUltimateEnter();
        }

        public override void OnExit() {
            _currentDealtDamage = CurrentDurationPercent * _requiredDealDamage;
            Parent.OnUltimateExit();
        }

        public override void OnTick(float deltaTime) {
            _currentDuration -= deltaTime;
            _currentDuration = Mathf.Max(_currentDuration, 0.0f);
            Parent.OnUltimatePercentChanged(CurrentDurationPercent);
            
            if(CurrentDurationPercent <= 0.0f)
                Controller.TransitionToDefaultState();
        }

        public override void SetInputs(CharacterInputs inputs) {
            if(inputs.Ultimate.Performed)
                Controller.TransitionToDefaultState();
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) => currentVelocity = Vector3.zero;

        public override bool CanEnterState() => CurrentDamagePercent >= 1.0f;
    }
}
