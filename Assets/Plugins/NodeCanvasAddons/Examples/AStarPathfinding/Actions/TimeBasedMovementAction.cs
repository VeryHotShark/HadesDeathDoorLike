using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding.Examples
{
    [Category("Movement")]
    [Name("Time Based Movement")]
    [Description("Move entity based upon time, movement speed and the changes factoring in delta time")]
    public class TimeBasedMovementAction : ActionTask<Transform>
    {
        private const int WeightingFactor = 25; // This makes it a bit faster like the pathfinding turning

        public BBParameter<float> StrafeChange, TurnChange, ForwardChange;
        public BBParameter<float> MovementSpeed = new BBParameter<float> { value = 1.0f };
        public BBParameter<float> RotationSpeed = new BBParameter<float> { value = 1.0f };

        protected override void OnExecute()
        {
            var targetRotation = agent.transform.rotation * Quaternion.Euler(Vector3.up * TurnChange.value * WeightingFactor);
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, targetRotation, RotationSpeed.value * Time.deltaTime);

            var forwardMovement = agent.transform.forward * ForwardChange.value * MovementSpeed.value * Time.deltaTime;
            var strafeMovement = agent.transform.right * StrafeChange.value * MovementSpeed.value * Time.deltaTime;
            agent.transform.position += strafeMovement + forwardMovement;

            EndAction(true);
        }
    }
}