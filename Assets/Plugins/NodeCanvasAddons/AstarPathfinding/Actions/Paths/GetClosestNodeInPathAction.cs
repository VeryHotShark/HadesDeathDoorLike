using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Paths")]
    [Name("Get Closest Node In Path")]
    [Description("Gets the closest node in an existing path")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetClosestNodeInPathAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Path> Path;

        [RequiredField]
        public BBParameter<Vector3> PositionToCheck;

        [BlackboardOnly]
        public BBParameter<GraphNode> OutputNode = new BBParameter<GraphNode>();

        [BlackboardOnly]
        public BBParameter<float> DistanceToClosest = new BBParameter<float>();

        protected override void OnExecute()
        {
            if(Path.isNone || Path.isNull)
            { EndAction(false); }

            if(Path.value.vectorPath.Count == 0)
            { EndAction(false); }

            var closestNode = Path.value.FindClosestNodeTo(PositionToCheck.value);
            OutputNode.value = closestNode;
            if (!DistanceToClosest.isNone)
            {
                var nodePosition = (Vector3) closestNode.position;
                DistanceToClosest.value = Vector3.Distance(nodePosition, PositionToCheck.value);
            }
            EndAction(true);
        }
    }
}