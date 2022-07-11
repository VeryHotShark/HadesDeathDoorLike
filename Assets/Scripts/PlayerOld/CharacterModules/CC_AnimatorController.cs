using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class CC_AnimatorController : MonoBehaviour {
        [Header("Animations")]
        [SerializeField] private float _moveSharpness = 10f;
        [SerializeField] private float _climbSharpness = 10f;
        
        private readonly int _rollParam = Animator.StringToHash("Roll");
        private readonly int _moveParam = Animator.StringToHash("Move");
        private readonly int _stateParam = Animator.StringToHash("State");
        private readonly int _climbParam = Animator.StringToHash("Climb");
        private readonly int _attackTypeParam = Animator.StringToHash("AttackType");
        private readonly int _attackIndexParam = Animator.StringToHash("AttackIndex");

        private float _smoothMove;
        private float _targetMove;
        
        private float _smoothClimb;
        private float _targetClimb;

        private Animator _animator;
        private OldCharacterController _cc;

        public Animator Animator => _animator;
        public float GetMoveParam => _animator.GetFloat(_moveParam);

        private void Awake() {
            _animator = GetComponent<Animator>();
            _cc = GetComponent<OldCharacterController>();
        }

        private void OnEnable() {
            _cc.OnCrouchStart += OnCrouchStart;
            _cc.OnCrouchEnd += OnCrouchEnd;
            _cc.OnLadderStart += OnLadderStart;
            _cc.OnLadderEnd += OnLadderEnd;
            _cc.OnRollStart += OnRollStart;
            _cc.OnRollUpdate += OnRollUpdate;
            _cc.OnRollEnd += OnRollEnd;
            _cc.OnLightAttack += OnLightAttack;
            _cc.OnHeavyAttack += OnHeavyAttack;
        }

        private void OnDisable() {
            _cc.OnCrouchStart -= OnCrouchStart;
            _cc.OnCrouchEnd -= OnCrouchEnd;
            _cc.OnLadderStart -= OnLadderStart;
            _cc.OnLadderEnd -= OnLadderEnd;
            _cc.OnRollStart -= OnRollStart;
            _cc.OnRollUpdate -= OnRollUpdate;
            _cc.OnRollEnd -= OnRollEnd;
            _cc.OnLightAttack -= OnLightAttack;
            _cc.OnHeavyAttack -= OnHeavyAttack;
        }

        private void Update() {
            _smoothMove = Mathf.Lerp(_smoothMove, _targetMove, 1f - Mathf.Exp(-_moveSharpness * Time.deltaTime));
            _smoothClimb = Mathf.Lerp(_smoothClimb, _targetClimb, 1f - Mathf.Exp(-_climbSharpness * Time.deltaTime));
            _animator.SetFloat(_moveParam, _smoothMove);
            _animator.SetFloat(_climbParam, _smoothClimb);
        }

        private void OnCrouchStart() => SetState(1);
        private void OnCrouchEnd() => SetState(0);
        
        private void OnLadderStart() => SetState(2);
        private void OnLadderEnd() => SetState(0);
        
        private void OnRollStart() => SetState(3);
        private void OnRollUpdate(float normalizedTime) => _animator.Play(_rollParam,-1,normalizedTime);
        private void OnRollEnd() => SetState(0);
        
        private void OnLightAttack(int index) {
            _animator.SetInteger(_attackTypeParam, 1);
            _animator.SetInteger(_attackIndexParam, index);
        }

        private void OnHeavyAttack() => _animator.SetInteger(_attackTypeParam,2);

        public void SetInputs(OldCharacterInputs inputs) {
            _targetMove = _cc.MoveInput.magnitude;
            _targetClimb = inputs.MoveAxisForward;
        }

        public void SetState(int state) {
            if(_animator.GetInteger(_stateParam) != state)
                _animator.SetInteger(_stateParam,state);
        }

        private void OnAnimatorMove() {
            _cc.StateMachine.CurrentState.OnAnimatorMoveCallback(_animator);
        }
    }
}