using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;
using VHS;

public class CharacterRoll : CharacterModule {
    [SerializeField] private Timer _cooldown = new Timer(0.5f);
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _distance = 1f;
    [SerializeField] private float _maxRollAngle = 45.0f;
    [SerializeField] private AnimationCurve _curve = AnimationCurve.Linear(0.0f,0.0f,1.0f,1.0f);
    
    [SerializeField] private GameEvent _rollEvent;

    private bool _lostGround;
    private bool _rollStopped;
    private float _rollTimestamp = -100.0f;
    private float _rollDotThreshold;
    
    private Vector3 _endPos;
    private Vector3 _startPos;
    private Vector3 _lastDesiredPos;
    private Vector3 _rollDirection;
    
    public bool OnCoooldown => _cooldown.IsActive;

    private void Awake() => _rollDotThreshold = Mathf.Cos(_maxRollAngle * Mathf.Deg2Rad);

    public override void OnEnter() {
        Parent.OnRoll();    
        _rollEvent?.Raise(Parent);
        _rollTimestamp = Time.time;
        _rollDirection = Controller.LastNonZeroMoveInput;
        _lastDesiredPos = Motor.TransientPosition;
        _startPos = _lastDesiredPos;
        _endPos = _startPos + _rollDirection * _distance;
    }

    public override void OnExit() {
        _rollStopped = false;
        _cooldown.Start();
    }

    public override void HandlePostCharacterUpdate(float deltaTime) {
        if(_rollStopped)
            Controller.TransitionToDefaultState();
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
        if (_rollStopped) {
            if(!_lostGround)
                currentVelocity = Vector3.zero;
            
            return;
        }

        float rollProgress = (Time.time - _rollTimestamp) / _duration;
        float t = _curve.Evaluate(rollProgress);
        Vector3 desiredPos = Vector3.Lerp(_startPos, _endPos, t);
        float deltaDistance = Vector3.Distance(_lastDesiredPos, desiredPos);
        float speed = deltaDistance / deltaTime;
        _lastDesiredPos = desiredPos;
        
        currentVelocity = (_rollDirection * speed).With(y: currentVelocity.y);
        currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, 
            Motor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;

        if (rollProgress > 1.0f) 
            _rollStopped = true;
    }

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
        currentRotation = Quaternion.LookRotation(_rollDirection);
        Controller.LastNonZeroMoveInput = _rollDirection;
    }

    public override void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
        ref HitStabilityReport hitStabilityReport) {
        Debug.DrawLine(hitPoint, hitPoint + hitNormal * 5.0f, Color.red, 5.0f);
        
        if (Vector3.Dot(hitNormal,Vector3.up) > _rollDotThreshold) 
            _endPos.y = hitPoint.y;
        else 
            _rollStopped = true;
    }

    public override void OnStableGroundLost() {
        _lostGround = true;
        _rollStopped = true;
    }
    
    public override bool CanEnterState() => !OnCoooldown;
}
