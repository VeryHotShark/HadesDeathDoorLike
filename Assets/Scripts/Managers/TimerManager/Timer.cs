using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Timer {
    [SerializeField] private float _duration;

    public Timer(float duration = 0.0f) => _duration = duration;

    private bool _onCooldownLastFrame;
    private bool _started;
    private float _timestamp;
    
    public event Action OnEnd = delegate {  };

    public float Duration => _duration;
    public bool IsActive => _started && Time.time < _timestamp + _duration;

    public void Start() {
        if (!IsActive) 
            TimerManager.AddTimer(this);
        
        if(!_started)
            _started = true;
        
        _timestamp = Time.time;
    }

    public void Reset() {
        if(IsActive)
            TimerManager.RemoveTimer(this);
        
        _timestamp = 0f;
        _started = false;
    }

    public bool CheckTimerEnd() {
        if (_onCooldownLastFrame && !IsActive) {
            OnEnd();
            _onCooldownLastFrame = false;
            return true;
        }
        
        _onCooldownLastFrame = IsActive;
        return false;
    }
}
