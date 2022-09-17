using System;
using UnityEngine;

namespace VHS {
    [Serializable]
    public class HitPoints {
        [SerializeField] private int _max;

        public HitPoints() {}

        public HitPoints(int hitPoints) {
            _max = hitPoints;
            Current = _max;
        }

        public Action<HitPoints> OnChanged = delegate { };
        
        public int Max => _max;
        public int Current { get; private set; }
        public float Ratio => Mathf.Clamp01((float)Current / _max);
        public bool AboveZero => Current > 0;

        public void Reset() {
            Current = _max;
            OnChanged(this);
        }

        public void Add(int delta, bool clampAtMax = true) {
            Current += delta;

            if (clampAtMax && Current > _max)
                Current = _max;

            OnChanged(this);
        }

        public void Subtract(int delta) {
            Current -= delta;
            OnChanged(this);
        }

        public void SetCurrent(int amount) {
            Current = amount;
            OnChanged(this);
        }

        public void SetMax(int max) => _max = max;
        public void SetCurrentSilent(int currentHp) => Current = currentHp;
    }
}
