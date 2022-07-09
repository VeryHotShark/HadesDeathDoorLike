using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace KinematicCharacterController.Examples
{
    public class ExampleAIController : MonoBehaviour
    {
        [FormerlySerializedAs("MovementPeriod")] public float _movementPeriod = 1f;
        [FormerlySerializedAs("Characters")] public List<ExampleCharacterController> _characters = new List<ExampleCharacterController>();

        private bool _stepHandling;
        private bool _ledgeHandling;
        private bool _intHandling;
        private bool _safeMove;

        private void Update()
        {
            AICharacterInputs inputs = new AICharacterInputs();

            // Simulate an input on all controlled characters
            inputs.MoveVector = Mathf.Sin(Time.time * _movementPeriod) * Vector3.forward;
            inputs.LookVector = Vector3.Slerp(-Vector3.forward, Vector3.forward, inputs.MoveVector.z).normalized;
            for (int i = 0; i < _characters.Count; i++)
            {
                _characters[i].SetInputs(ref inputs);
            }
        }
    }
}