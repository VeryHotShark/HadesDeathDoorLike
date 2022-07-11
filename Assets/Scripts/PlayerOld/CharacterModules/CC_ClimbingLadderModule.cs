using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController.Walkthrough.ClimbingLadders;
using UnityEngine;

namespace VHS {
    public enum ClimbingState {
        Climbing,
        Anchoring,
        DeAnchoring,
    }
    
    public class CC_ClimbingLadderModule : OldCharacterControllerModule {
        [SerializeField] private float _climbingSpeed = 4f;
        [SerializeField] private float _anchoringDuration = 0.25f;
        [SerializeField] private LayerMask _interactionLayer;

        [Header("Root Motion")]
        [SerializeField] private bool _useRootMotion;
        [SerializeField] private Vector3 _rootAxisToUse = Vector3.one;
        
        private float _anchoringTimer;
        private float _ladderVerticalInput;
        private float _onLadderSegmentState;

        private Vector3 _ladderTargetPosition;
        private Vector3 _anchoringStartPosition;
        private Vector3 _rootMotionPositionDelta;
        private Quaternion _ladderTargetRotation;
        private Quaternion _anchoringStartRotation;
        private Quaternion _rotationBeforeClimbing;
        
        private MyLadder ActiveLadder { get; set; }
        private Collider[] _probedColliders = new Collider[8];
        private ClimbingState _internalClimbingState;

        private ClimbingState ClimbingState {
            get => _internalClimbingState;
            set {
                _internalClimbingState = value;
                _anchoringTimer = 0f;
                _anchoringStartPosition = Motor.TransientPosition;
                _anchoringStartRotation = Motor.TransientRotation;
            }
        }
        
        public override void SetInputs(OldCharacterInputs inputs) => _ladderVerticalInput = inputs.MoveAxisForward;

        public void HandlePotentialLadderAnchor() {
            bool overlapSomething = Motor.CharacterOverlap(Motor.TransientPosition,
                Motor.TransientRotation,
                _probedColliders,
                _interactionLayer,
                QueryTriggerInteraction.Collide) > 0;

            if (!overlapSomething || _probedColliders[0] == null) return;

            ActiveLadder = _probedColliders[0].gameObject.GetComponent<MyLadder>();
            
            if(ActiveLadder)
                StateMachine.SetState(this);
        }

        public void HandlePotentialLadderDeAnchor() {
            if (StateMachine.CurrentState != this) return;
            
            ClimbingState = ClimbingState.DeAnchoring;
            
            _ladderTargetPosition = Motor.TransientPosition;
            _ladderTargetRotation = _rotationBeforeClimbing;
        }

        public override void OnStateEnter() {
            ClimbingState = ClimbingState.Anchoring;
            
            Motor.SetGroundSolvingActivation(false);
            Motor.SetMovementCollisionsSolvingActivation(false);

            _rotationBeforeClimbing = Motor.TransientRotation;
            _ladderTargetRotation = ActiveLadder.transform.rotation;
            _ladderTargetPosition = ActiveLadder.ClosestPointOnLadderSegment(Motor.TransientPosition, out _onLadderSegmentState);
            Controller.OnLadderStart();
        }

        public override void OnStateExit() {
            ActiveLadder = null;
            Motor.SetGroundSolvingActivation(true);
            Motor.SetMovementCollisionsSolvingActivation(true);
            Controller.OnLadderEnd();
        }

        public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
            switch (ClimbingState) {
                case ClimbingState.Climbing:
                    currentRotation = ActiveLadder.transform.rotation;
                    break;
                case ClimbingState.Anchoring:
                case ClimbingState.DeAnchoring:
                    currentRotation = Quaternion.Slerp(_anchoringStartRotation, _ladderTargetRotation, _anchoringTimer / _anchoringDuration);
                    break;
            }
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            currentVelocity = Vector3.zero;

            switch (ClimbingState) {
                case ClimbingState.Climbing:
                    if (_useRootMotion)
                        currentVelocity = _rootMotionPositionDelta / deltaTime;
                    else
                        currentVelocity = (_ladderVerticalInput * ActiveLadder.transform.up).normalized * _climbingSpeed;
                    break;
                case ClimbingState.Anchoring:
                case ClimbingState.DeAnchoring:
                    Vector3 tmpPosition = Vector3.Lerp(_anchoringStartPosition, _ladderTargetPosition, _anchoringTimer / _anchoringDuration);
                    currentVelocity = Motor.GetVelocityForMovePosition(Motor.TransientPosition, tmpPosition, deltaTime);
                    break;
            }
        }

        public override void HandlePostCharacterUpdate(float deltaTime) {
            _rootMotionPositionDelta = Vector3.zero;
            
            switch (ClimbingState) {
                case ClimbingState.Climbing:
                    ActiveLadder.ClosestPointOnLadderSegment(Motor.TransientPosition, out _onLadderSegmentState);
                    
                    if (Mathf.Abs(_onLadderSegmentState) > 0.05f) {
                        ClimbingState = ClimbingState.DeAnchoring;

                        if (_onLadderSegmentState > 0) {
                            _ladderTargetPosition = ActiveLadder._topReleasePoint.position;
                            _ladderTargetRotation = ActiveLadder._topReleasePoint.rotation;
                        } else if (_onLadderSegmentState < 0) {
                            _ladderTargetPosition = ActiveLadder._bottomReleasePoint.position;
                            _ladderTargetRotation = ActiveLadder._bottomReleasePoint.rotation;
                        }
                    }
                    break;
                case ClimbingState.Anchoring:
                case ClimbingState.DeAnchoring:
                    if (_anchoringTimer >= _anchoringDuration) {
                        if (ClimbingState == ClimbingState.Anchoring)
                            ClimbingState = ClimbingState.Climbing;
                        else if(ClimbingState == ClimbingState.DeAnchoring)
                            StateMachine.SetState(Controller.DefaultMovementModule);
                    }

                    _anchoringTimer += deltaTime;
                    break;
            }
        }
        

        public override void OnAnimatorMoveCallback(Animator animator) {
            if (_useRootMotion) {
                Vector3 localDeltaPos = Motor.transform.InverseTransformVector(animator.deltaPosition);
                Vector3 clampedDeltaPos = Vector3.Scale(localDeltaPos, _rootAxisToUse);
                _rootMotionPositionDelta += Motor.transform.TransformVector(clampedDeltaPos);
            } 
        }
         
    }
}
