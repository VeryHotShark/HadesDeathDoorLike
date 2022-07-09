using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace KinematicCharacterController.Walkthrough.MovingPlatform
{
    public struct MyMovingPlatformState
    {
        public PhysicsMoverState MoverState;
        public float DirectorTime;
    }

    public class MyMovingPlatform : MonoBehaviour, IMoverController
    {
        [FormerlySerializedAs("Mover")] public PhysicsMover _mover;

        [FormerlySerializedAs("Director")] public PlayableDirector _director;

        private Transform _transform;

        private void Start()
        {
            _transform = this.transform;

            _mover.MoverController = this;
        }

        // This is called every FixedUpdate by our PhysicsMover in order to tell it what pose it should go to
        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
        {
            // Remember pose before animation
            Vector3 _positionBeforeAnim = _transform.position;
            Quaternion _rotationBeforeAnim = _transform.rotation;

            // Update animation
            EvaluateAtTime(Time.time);

            // Set our platform's goal pose to the animation's
            goalPosition = _transform.position;
            goalRotation = _transform.rotation;

            // Reset the actual transform pose to where it was before evaluating. 
            // This is so that the real movement can be handled by the physics mover; not the animation
            _transform.position = _positionBeforeAnim;
            _transform.rotation = _rotationBeforeAnim;
        }

        public void EvaluateAtTime(double time)
        {
            _director.time = time % _director.duration;
            _director.Evaluate();
        }
    }
}