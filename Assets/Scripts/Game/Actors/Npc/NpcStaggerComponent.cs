using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace VHS {
    public class NpcStaggerComponent : NpcComponent {

        [SerializeField] private int _maxStaggerCount = 3;
        [SerializeField] private Timer _staggerCooldown = new Timer(1.0f);

        private int _staggerCount = 0;
        private bool _duringStagger = false;
        private CoroutineHandle _staggerCoroutine;
        
        public bool DuringStagger => _duringStagger;
        
        private void Awake() => _staggerCooldown.OnEnd = () => _staggerCount = 0;

        public void Stagger(float duration) {
            if(!Parent.Model.CanBeStaggered || _staggerCount >= _maxStaggerCount)
                return;
            
            _staggerCount++;
            _duringStagger = true;
            Parent.SetState(NpcState.Recovery);

            if (_duringStagger) 
                Timing.KillCoroutines(_staggerCoroutine);

            Parent.OnStaggerStart();
            _staggerCoroutine = Timing.CallDelayed(duration, OnStaggerEnd , gameObject);
        }

        private void OnStaggerEnd() {
            Parent.OnStaggerEnd();
            _staggerCooldown.Start();
            _duringStagger = false;
            Parent.SetState(NpcState.Default);
        }
    }
}
