using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VHS {
    public class HitFXComponent : ChildBehaviour<Actor> {
        [FoldoutGroup("Hit Properties"),SerializeField] private float _flickerDuration = 0.01f;
        [FoldoutGroup("Hit Properties"),SerializeField] private float _flickerOctave = 0.04f;
        [FoldoutGroup("Hit Properties"),SerializeField, ColorUsage(false, true)] private Color _flickerColor = Color.white;
        
        private MMF_Player _hitFeedbacks;

        private void Awake() {
            _hitFeedbacks = GetComponent<MMF_Player>();
            
            MMF_Flicker flickerFeedback = new MMF_Flicker {
                BoundRenderer = GetComponentInChildren<Renderer>(),
                FlickerDuration = _flickerDuration,
                FlickerOctave = _flickerOctave,
                FlickerColor = _flickerColor,
                Mode = MMF_Flicker.Modes.PropertyName,
                UseMaterialPropertyBlocks = true,
                MaterialIndexes = new [] {0},
                PropertyName = "_BaseColor",
            };

            _hitFeedbacks.AddFeedback(flickerFeedback);
            _hitFeedbacks.Initialization();
        }

        protected override void Enable() {
            Parent.OnHit += OnHit;
        }
        
        protected override void Disable() {
            Parent.OnHit -= OnHit;
        }

        private void OnHit(HitData hitData) {
            _hitFeedbacks.PlayFeedbacks();
        }
    }
}
