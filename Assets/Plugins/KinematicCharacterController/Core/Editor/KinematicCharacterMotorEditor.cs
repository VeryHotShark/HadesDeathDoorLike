using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KinematicCharacterController
{
    [CustomEditor(typeof(KinematicCharacterMotor))]
    public class KinematicCharacterMotorEditor : Editor
    {
        protected virtual void OnSceneGUI()
        {            
            KinematicCharacterMotor motor = (target as KinematicCharacterMotor);
            if (motor)
            {
                Vector3 characterBottom = motor.transform.position + (motor._capsule.center + (-Vector3.up * (motor._capsule.height * 0.5f)));

                Handles.color = Color.yellow;
                Handles.CircleHandleCap(
                    0, 
                    characterBottom + (motor.transform.up * motor._maxStepHeight), 
                    Quaternion.LookRotation(motor.transform.up, motor.transform.forward), 
                    motor._capsule.radius + 0.1f, 
                    EventType.Repaint);
            }
        }
    }
}