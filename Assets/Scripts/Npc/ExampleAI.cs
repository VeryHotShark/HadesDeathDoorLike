using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using VHS;
using CharacterController = UnityEngine.CharacterController;

public class ExampleAI : MonoBehaviour {
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed = 2;
    [SerializeField] private float _nextWaypointDistance = 3;
    [SerializeField] private Timer _repathTimer = new(0.5f);
    
    
    private Path _path;
    private Seeker _seeker;
    private CharacterController _controller;

    private int _currentWaypoint;
    private bool _reachedEndOfPath;
    
    private void Awake() {
        _seeker = GetComponent<Seeker>();
        _controller = GetComponent<CharacterController>();
    }

    private void OnEnable() => _seeker.pathCallback += OnPathComplete;
    private void OnDisable() => _seeker.pathCallback -= OnPathComplete;

    private void OnPathComplete(Path path) {
        path.Claim(this);

        if (!path.error) {
            if(_path != null)
                _path.Release(this);
            
            _path = path;
            _currentWaypoint = 0;
        }
        else
            path.Release(this);
    }

    private void Update() {
        if (!_repathTimer.IsActive && _seeker.IsDone())
            _seeker.StartPath(transform.position, _target.position);
        
        if(_path == null)
            return;

        _reachedEndOfPath = false;

        float distanceToWaypointSqr;

        while (true) {
            distanceToWaypointSqr = transform.position.DistanceSquaredTo(_path.vectorPath[_currentWaypoint]);

            if (distanceToWaypointSqr < _nextWaypointDistance) {
                if (_currentWaypoint + 1 < _path.vectorPath.Count)
                    _currentWaypoint++;
                else {
                    _reachedEndOfPath = true;
                    break;
                }
            }
            else
                break;
        }

        float speedFactor =
            _reachedEndOfPath ? Mathf.Sqrt(distanceToWaypointSqr / _nextWaypointDistance.Square()) : 1.0f;

        Vector3 dir = transform.position.DirectionTo(_path.vectorPath[_currentWaypoint]);
        Vector3 velocity = dir * _speed * speedFactor;

        _controller.SimpleMove(velocity);
    }
}
