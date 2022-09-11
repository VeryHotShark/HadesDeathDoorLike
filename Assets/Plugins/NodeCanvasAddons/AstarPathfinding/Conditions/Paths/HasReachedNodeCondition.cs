using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding")]
    [Name("Has Reached Node")]
    [Description("Checks to see if the agent is close enough to node")]
    [ParadoxNotion.Design.Icon("PathfindingWaypoint")]
    public class HasReachedNodeCondition : ConditionTask<Transform>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<GraphNode> Node;

        [RequiredField]
        public BBParameter<float> AcceptableDistance = new BBParameter<float> { value = 0.1f };

        protected override string info
        {
            get { return string.Format("reached {0}?", Node); }
        }

        protected override bool OnCheck()
        {
            var nodeAsWaypoint = (Vector3) Node.value.position;
            var distance = Vector3.Distance(nodeAsWaypoint, agent.transform.position);
            return distance <= AcceptableDistance.value;
        }
    }
}