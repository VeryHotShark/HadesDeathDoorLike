using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMeleeCombat : CharacterModule
{
    [SerializeField] private float _pushForce = 10.0f;
    [SerializeField] private Timer _attackTimer = new Timer(0.3f);

    [Header("Combo")]
    [SerializeField] private int _maxComboChain = 3;
    [SerializeField] private Timer _comboCooldown = new Timer(0.5f);
    [SerializeField] private Timer _attackBufferTimer = new Timer(0.2f);

    private int _attackCount = 0;
    
    public Timer AttackTimer => _attackTimer;
    public Timer ComboCooldown => _comboCooldown;
    public Timer AttackBufferTimer => _attackBufferTimer;
    
    public bool IsOnCooldown => _comboCooldown.IsActive;
    public bool IsDuringAttack => _attackTimer.IsActive || _attackBufferTimer.IsActive;

    public override void OnEnter() {
        _attackCount = 0;
        Attack();
    }

    public override void OnExit() {
        _attackCount = 0;
    }

    private void Attack() {
        _attackTimer.Start();
        _attackBufferTimer.Reset();
        Motor.SetRotation(Controller.LastCharacterInputs.CursorRotation);
        Controller.LastNonZeroMoveInput = Controller.LookInput;
        Controller.AddVelocity(_pushForce * Controller.LookInput);

        _attackCount++;
        
        if(_attackCount >= _maxComboChain)
            _comboCooldown.Start();
    }

    public override void SetInputs(CharacterInputs inputs) {
        Controller.MoveInput = Vector3.zero;

        if(_comboCooldown.IsActive)
            return;

        if (inputs.AttackDown) 
            _attackBufferTimer.Start();

        if (!_attackTimer.IsActive && _attackBufferTimer.IsActive)
            Attack();
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
        
    }

}
