using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using MoreMountains.Feedbacks;

namespace VHS {
    public class Npc : BaseBehaviour, IHittable, ITargetable, IActor {
        public enum NpcState {
            Default,
            Recovery,
        }
    
        [SerializeField] private float _pushSpeed;
        [SerializeField] private float _pushDuration;
        [SerializeField] private float _recoveryDuration;

        private NpcState _state;
        private GameObject _target;
        private MMF_Player _hitFeedbacks;
        private RichAI _richAI;

        public GameObject Target => _target;
        public Vector3 FeetPosition => transform.position;
        public Vector3 CenterOfMass => FeetPosition + Vector3.up;
        
        public NpcState State => _state;
        public Action OnHitCallback = delegate {  };

        private CoroutineHandle _pushBackRoutine;

        private void Awake() {
            _richAI = GetComponent<RichAI>();
            _hitFeedbacks = GetComponent<MMF_Player>();

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

            _hitFeedbacks.AddFeedback(flickerFeedback);
            _hitFeedbacks.Initialization();
        }

        private void Start() {
            _target = NpcBlackboard.PlayerInstance.Character.Motor.gameObject;
        }

        public override void OnCustomUpdate(float deltaTime) {
            // _richAI.destination = NpcBlackboard.PlayerInstance.Character.Motor.TransientPosition;
        }

        public void OnHit(HitData hitData) {
            OnHitCallback();
            _hitFeedbacks.PlayFeedbacks(); // Move this to seperate component like Npc Hit Component or Npc Fx Component
            
            Timing.KillCoroutines(_pushBackRoutine);
            
            _richAI.SetPath(null);
            _richAI.isStopped = true;
            _state = NpcState.Recovery;


            Vector3 flattenedDirection = hitData.direction.Flatten();
            _pushBackRoutine = Timing.CallContinuously(flattenedDirection, _pushDuration, PushBack, PushBackEnd);
            
            // _pushBackRoutine = Timing.RunCoroutine(_PushBack());
        }

        private void PushBack(Vector3 direction) {
            _richAI.Move(direction * (_pushSpeed * Time.deltaTime));
        }

        private void PushBackEnd(Vector3 direction) {
            _richAI.isStopped = false;
            Timing.CallDelayed(_recoveryDuration, delegate { _state = NpcState.Default; });
        }

        IEnumerator<float> _PushBack() {
            _richAI.SetPath(null);
            _richAI.isStopped = true;

            float pushTimer = 0.0f;

            while (pushTimer < _pushDuration) {
                pushTimer += Time.deltaTime;
                _richAI.Move(-transform.forward * (_pushSpeed * Time.deltaTime)); // może użyj teleportu
                // Dodać recovery stan w którym AI nie ustawia nowy path, by nie było, że obrywa i cały czas idzie na gracza
                yield return Timing.WaitForOneFrame;  
            }
            
            _richAI.isStopped = false;
        }

        public Vector3 GetTargetPosition() => transform.position;
    }
}