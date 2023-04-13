using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VHS {
    public class CharacterUltimateCombat : CharacterModule {
        [SerializeField] private float _requiredDealDamage = 20.0f;
        [SerializeField] private float _duration = 3.0f;
        [SerializeField] private float _slowDownTimescale = 0.5f;
        [SerializeField] private Feedback _ultimateFeedback;

        [Title("Ultimate")] 
        [SerializeField] private float _radius = 10.0f;
        [SerializeField] private float _lerpDuration = 0.5f;

        private float _currentDuration = 0.0f;
        private float _currentDealtDamage = 0.0f;
        
        private Feedback _feedbackInstance;
        private MMF_CameraZoom _cameraZoom;

        private Collider _lastCollider;
        private Collider[] _colliders = new Collider[10];

        private void Start() {
            _feedbackInstance = PoolManager.Spawn(_ultimateFeedback);
            _cameraZoom = _feedbackInstance.FeedbackPlayer.GetFeedbackOfType<MMF_CameraZoom>();
            _cameraZoom.ZoomMode = MMCameraZoomModes.Set;
            _cameraZoom.ZoomTransitionDuration = 0.5f;
        }

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
            _cameraZoom.ZoomFieldOfView = 10.0f;
            _feedbackInstance.Play();
            
            _currentDuration = _duration;
            Parent.OnUltimateEnter();
            GameManager.SetTimescale(_slowDownTimescale, true);
        }

        public override void OnExit() {
            _cameraZoom.ZoomFieldOfView = 0.0f;
            _feedbackInstance.Play();
            
            _currentDealtDamage = CurrentDurationPercent * _requiredDealDamage;
            Parent.OnUltimateExit();
            GameManager.ResetTimescale(1.0f, true);
        }

        public override void OnTick(float deltaTime) {
            _currentDuration -= Time.unscaledDeltaTime;
            _currentDuration = Mathf.Max(_currentDuration, 0.0f);
            Parent.OnUltimatePercentChanged(CurrentDurationPercent);

            if (CurrentDurationPercent <= 0.0f) 
                Controller.TransitionToDefaultState(true);
        }

        public override void SetInputs(CharacterInputs inputs) {
            if (inputs.Ultimate.Performed) 
                Controller.TransitionToDefaultState(true);

            if (inputs.Melee.Released)
                AttackClosestTarget();
        }

        private void AttackClosestTarget() {
            int hitCount =
                Physics.OverlapSphereNonAlloc(Parent.CenterOfMass, _radius, _colliders, LayerManager.Masks.NPC);
            
            if(hitCount == 0)
                return;

            _colliders.MMShuffle();

            for (int i = 0; i < hitCount - 1; i++) {
                Collider collider = _colliders[i];

                if (_lastCollider == collider)
                    continue;
                
                IActor enemyActor = collider.GetComponentInParent<IActor>();
                
                if(enemyActor == null)
                    continue;

                IHittable hittable = enemyActor.GameObject.GetComponent<IHittable>();

                if (hittable != null) {
                    var closest = Parent.ClosestPosRotToActor(enemyActor);
                    Motor.SetPositionAndRotation(closest.Pos, closest.Rot);
                    
                    HitData hitData = new HitData {
                        hittable = hittable,
                        damage = 1,
                        actor = Parent,
                        dealer = gameObject,
                        position = closest.Pos,
                        direction = closest.Rot * Vector3.forward
                    };
                    
                    hittable.Hit(hitData);
                    _lastCollider = collider;
                    break;
                }
            }
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) => currentVelocity = Vector3.zero;

        public override bool CanEnterState() => CurrentDamagePercent >= 1.0f;
        
        public override bool CanExitState() => false;
    }
}
