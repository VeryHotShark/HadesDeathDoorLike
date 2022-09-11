using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding.Examples
{
    [Category("Movement")]
    [Name("Move Towards Node")]
    [ParadoxNotion.Design.Icon("PathfindingNode")]
    public class MoveTowardsNodeAction : ActionTask
    {
        private const float RotationDampening = 2.0f; // To allow rotation speeds to appear same for similar values

        [RequiredField]
        [BlackboardOnly]
        public BBParameter<GraphNode> Node;

        [RequiredField]
        public BBParameter<float> MovementSpeed = new BBParameter<float> { value = 1.0f };
        public BBParameter<float> RotationSpeed = new BBParameter<float> { value = 1.0f };

        protected override void OnExecute()
        {
            var nodeAsWaypoint = (Vector3) Node.value.position;
            var waypointDirection = (nodeAsWaypoint - agent.transform.position).normalized;
            if (waypointDirection == Vector3.zero)
            {
                EndAction(true);
                return;
            }

            var targetRotation = Quaternion.LookRotation(new Vector3(waypointDirection.x, 0.0f, waypointDirection.z));
            var rotationThisFrame = (RotationSpeed.value / RotationDampening) * Time.deltaTime;
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, targetRotation, rotationThisFrame);

            var movementThisFrame = MovementSpeed.value * Time.deltaTime;

            if (Vector3.Distance(agent.transform.position, nodeAsWaypoint) <= movementThisFrame)
            { agent.transform.position = nodeAsWaypoint; }
            else
            {
                var movementChange = waypointDirection * movementThisFrame;
                agent.transform.position += movementChange;
            }

            EndAction(true);
        }
    }
}