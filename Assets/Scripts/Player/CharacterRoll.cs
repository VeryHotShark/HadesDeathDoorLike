using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;
using VHS;

public class CharacterRoll : CharacterModule {
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _distance = 1f;
    [SerializeField] private float _maxRollAngle = 45.0f;
    [SerializeField] private AnimationCurve _curve = AnimationCurve.Linear(0.0f,0.0f,1.0f,1.0f);

    private bool _lostGround;
    private bool _rollStopped;
    private float _rollProgress;
    private float _rollTimestamp;
    private float _rollDotThreshold;
    
    private Vector3 _endPos;
    private Vector3 _startPos;
    private Vector3 _rollDirection;

    private void Awake() {
        _rollDotThreshold = Mathf.Cos(_maxRollAngle * Mathf.Deg2Rad);
    }

    public override void OnEnter() {
        Debug.Log("Dupa");
        _rollTimestamp = Time.time;
        _rollDirection = Controller.MoveInput.sqrMagnitude < Mathf.Epsilon ? Controller.LastNonZeroMoveInput : Controller.MoveInput;
        _startPos = Motor.TransientPosition;
        _endPos = Motor.TransientPosition + _rollDirection * _distance;
    }

    public override void OnExit() {
        _rollProgress = 0.0f;
        _rollStopped = false;
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
        if (_rollStopped) {
            if(!_lostGround)
                currentVelocity = Vector3.zero;
            
            return;
        }

        _rollProgress = (Time.time - _rollTimestamp) / _duration;
        float t = _curve.Evaluate(_rollProgress);
        Vector3 desiredPos = Vector3.Lerp(_startPos, _endPos, t);
        Vector3 direction = Motor.TransientPosition.DirectionTo(desiredPos);
        float deltaDistance = Vector3.Distance(Motor.TransientPosition, desiredPos);
        float speed = deltaDistance / deltaTime;
        
        currentVelocity = (direction * speed).With(y: currentVelocity.y);
        currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, 
            Motor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;

        if (_rollProgress > 1.0f) 
            _rollStopped = true;
    }

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
        currentRotation = Quaternion.LookRotation(_rollDirection);
        Controller.LastNonZeroMoveInput = _rollDirection;
    }

    public override void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
        ref HitStabilityReport hitStabilityReport) {
        Debug.DrawLine(hitPoint, hitPoint + hitNormal * 10.0f, Color.red, 5.0f);
        
        if (Vector3.Dot(hitNormal,Vector3.up) > _rollDotThreshold) 
            _endPos.y = hitPoint.y;
        else 
            _rollStopped = true;
    }

    public override void OnStableGroundLost() {
        _lostGround = true;
        _rollStopped = true;
    }

    public bool DuringRoll => _rollProgress > 0.0f && !_rollStopped;
}
