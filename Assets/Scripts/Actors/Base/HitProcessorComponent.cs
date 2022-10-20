using UnityEngine;


namespace VHS {
    public class HitProcessorComponent : ChildBehaviour<Actor> {
        [SerializeField] protected HitPoints _hitPoints;

        public HitPoints HitPoints => _hitPoints;
        
        private void Awake() => _hitPoints.Reset();

        public virtual void Hit(HitData hitData) {
            if(!_hitPoints.AboveZero)
                return;
            
            _hitPoints.Subtract(hitData.damage);
            
            Parent.OnHit(hitData);

            if (!_hitPoints.AboveZero) 
                Parent.Die();
        }
    }
    
    public class HitProcessorComponent<T>: HitProcessorComponent where T : Actor{ // Change to Scriptable Object? So we can create assets out of it
        protected new T Parent => (T) base.Parent;
    }
}