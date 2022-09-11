using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding")]
    [Name("Has Reached Waypoint")]
    [Description("Checks to see if the agent is close enough to waypoint")]
    [ParadoxNotion.Design.Icon("PathfindingWaypoint")]
    public class HasReachedWaypointCondition : ConditionTask<Transform>
    {
        [RequiredField]
        public BBParameter<Vector3> Waypoint;

        [RequiredField]
        public BBParameter<float> AcceptableDistance = new BBParameter<float>{value = 0.1f};

        protected override string info
        {
            get { return string.Format("reached {0}?", Waypoint); }
        }

        protected override bool OnCheck()
        {
            var distance = Vector3.Distance(Waypoint.value, agent.transform.position);
            return distance <= AcceptableDistance.value;
        }
    }
}