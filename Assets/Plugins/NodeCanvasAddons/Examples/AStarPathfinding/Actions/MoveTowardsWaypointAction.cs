using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding.Examples
{
    [Category("Movement")]
    [Name("Move Towards Waypoint")]
    [ParadoxNotion.Design.Icon("PathfindingNode")]
    public class MoveTowardsWaypointAction : ActionTask
    {
        private const float RotationDampening = 2.0f; // To allow rotation speeds to appear same for similar values

        [RequiredField]
        public BBParameter<Vector3> Waypoint;

        [RequiredField]
        public BBParameter<float> MovementSpeed = new BBParameter<float> { value = 1.0f };
        public BBParameter<float> RotationSpeed = new BBParameter<float> { value = 1.0f };

        protected override void OnExecute()
        {
            var waypointDirection = (Waypoint.value - agent.transform.position).normalized;
            if (waypointDirection == Vector3.zero)
            {
                EndAction(true);
                return;
            }

            var targetRotation = Quaternion.LookRotation(new Vector3(waypointDirection.x, 0.0f, waypointDirection.z));
            var rotationThisFrame = (RotationSpeed.value/RotationDampening)*Time.deltaTime;
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, targetRotation, rotationThisFrame);

            var movementThisFrame = MovementSpeed.value*Time.deltaTime;

            if (Vector3.Distance(agent.transform.position, Waypoint.value) <= movementThisFrame)
            { agent.transform.position = Waypoint.value; }
            else
            {
                var movementChange = waypointDirection * movementThisFrame;
                agent.transform.position += movementChange;
            }
        
            EndAction(true);
        }
    }
}