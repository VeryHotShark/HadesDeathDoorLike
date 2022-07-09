using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRangeCombat : CharacterModule {
    [SerializeField] private float _aimSharpness = 20.0f;

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
        currentVelocity = Vector3.zero;
    }

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
        float t = 1 - Mathf.Exp(-_aimSharpness * deltaTime);
        currentRotation = Quaternion.Slerp(Motor.TransientRotation, Controller.LastCharacterInputs.CursorRotation, t);
        Controller.LastNonZeroMoveInput = Controller.LookInput;
    }
}