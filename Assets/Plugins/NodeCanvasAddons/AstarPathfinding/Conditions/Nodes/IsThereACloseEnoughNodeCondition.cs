using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding")]
    [Name("Is There A Close Enough Node")]
    [Description("Checks to see if a node is close enough to a position")]
    [ParadoxNotion.Design.Icon("PathfindingNode")]
    public class IsThereACloseEnoughNodeCondition : ConditionTask
    {
        [RequiredField]
        public BBParameter<Vector3> Position;

        [RequiredField]
        public BBParameter<float> MaximumDistance;

        protected override bool OnCheck()
        {
            var closestResult = AstarPath.active.GetNearest(Position.value);
            if (closestResult.node == null)
            { return false; }

            var nearestNode = closestResult.node;
            var distance = Vector3.Distance((Vector3)nearestNode.position, Position.value);
            return distance <= MaximumDistance.value;
        }
    }
}