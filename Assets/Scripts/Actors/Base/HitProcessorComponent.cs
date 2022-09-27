using UnityEngine;


namespace VHS {
    public class HitProcessorComponent : ChildBehaviour<Actor> {
        [SerializeField] private HitPoints _hitPoints;

        public HitPoints HitPoints => _hitPoints;
        
        private void Awake() => _hitPoints.Reset();

        public void Hit(HitData hitData) {
            if(!_hitPoints.AboveZero)
                return;
            
            _hitPoints.Subtract(hitData.damage);
            
            Parent.OnHit(hitData);

            if (!_hitPoints.AboveZero) 
                Parent.Die();
        }
    }
}