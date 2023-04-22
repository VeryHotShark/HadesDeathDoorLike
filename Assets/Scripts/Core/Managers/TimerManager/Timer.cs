using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Timer {
    [SerializeField] private float _duration = 0.0f;
    [SerializeField] private bool _looping = false;

    public Timer(float duration = 0.0f, bool looping = false) {
        _duration = duration;
        _looping = looping;
    }

    private bool _onCooldownLastFrame;
    private bool _started;
    private float _timestamp;
    
    public Action OnEnd = delegate {  };

    public float Duration => _duration;
    public bool Started => _started;
    public bool IsLooping => _looping;
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
