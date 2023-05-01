using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding.RVO;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding.Examples
{
    [Category("Movement")]
    [Name("Move Towards Waypoint With RVO")]
    [ParadoxNotion.Design.Icon("PathfindingNode")]
    public class MoveTowardsWaypointWithRVOAction : ActionTask<RVOController>
    {
        [RequiredField]
        public BBParameter<Vector3> Waypoint;

        [RequiredField]
        public BBParameter<float> MovementSpeed = new BBParameter<float> { value = 1.0f };

        protected override void OnExecute()
        {
            var waypointDirection = (Waypoint.value - agent.transform.position).normalized;
            if (waypointDirection == Vector3.zero)
            {
                EndAction(true);
                return;
            }

            agent.SetTarget(Waypoint.value, MovementSpeed.value, MovementSpeed.value, Waypoint.value);
            var movement = agent.CalculateMovementDelta(Time.deltaTime);
            agent.transform.position += movement;

            EndAction(true);
        }
    }
}