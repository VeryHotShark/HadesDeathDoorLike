using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Get Distance To Waypoint")]
    [Description("Gets the distances to the next waypoint from the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetDistanceToNextWaypointAction : ActionTask<RichAI>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<float> DistanceToNextWaypoint;

        [GetFromAgent]
        private RichAI _richAI = default;
        
        protected override string info
        {
            get
            {
                return "Getting agent distance to next waypoint";
            }
        }

        protected override void OnExecute()
        {
            DistanceToNextWaypoint.value = Vector3.Distance(_richAI.transform.position, _richAI.steeringTarget);
            EndAction(true);
        }
    }
}