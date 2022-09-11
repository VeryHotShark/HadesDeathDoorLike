using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Paths")]
    [Name("Get Closest Position In Path")]
    [Description("Gets the closest position in an existing path")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetClosestPositionInPathAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Path> Path;

        [RequiredField]
        public BBParameter<Vector3> PositionToCheck;

        [BlackboardOnly]
        public BBParameter<Vector3> OutputVector = new BBParameter<Vector3>();

        [BlackboardOnly]
        public BBParameter<float> DistanceToClosest = new BBParameter<float>();

        protected override void OnExecute()
        {
            if (Path.isNone || Path.isNull)
            { EndAction(false); }

            if (Path.value.vectorPath.Count == 0)
            { EndAction(false); }

            var closestNode = Path.value.FindClosestPositionTo(PositionToCheck.value);
            OutputVector.value = closestNode;
            if (!DistanceToClosest.isNone)
            { DistanceToClosest.value = Vector3.Distance(closestNode, PositionToCheck.value); }
            EndAction(true);
        }
    }
}