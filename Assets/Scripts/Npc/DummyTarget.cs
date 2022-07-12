using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Pathfinding;
using UnityEngine;

namespace VHS {
    public class DummyTarget : MonoBehaviour, IHittable, ITargetable {

        private MMF_Player _hitFeedbacks;

        private void Awake() {
            _hitFeedbacks = gameObject.GetComponent<MMF_Player>();

            MMF_Flicker flickerFeedback = new MMF_Flicker {
                BoundRenderer = GetComponentInChildren<Renderer>(),
                FlickerDuration = 0.1f,
                FlickerOctave = 0.04f,
                FlickerColor = Color.white * 1.5f,
                Mode = MMF_Flicker.Modes.PropertyName,
                UseMaterialPropertyBlocks = true,
                MaterialIndexes = new [] {0},
                PropertyName = "_BaseColor",
            };

            flickerFeedback.Timing = new MMFeedbackTiming();
            _hitFeedbacks.AddFeedback(flickerFeedback);
            _hitFeedbacks.Initialization();
        }

        public void OnHit() {
            _hitFeedbacks.PlayFeedbacks();
        }

        public Vector3 GetTargetPosition() => transform.position;
    }
}