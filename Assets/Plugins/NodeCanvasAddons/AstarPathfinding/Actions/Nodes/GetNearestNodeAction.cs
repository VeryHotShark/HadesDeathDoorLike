using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Nodes")]
    [Name("Get Nearest Node")]
    [Description("Finds the nearest node to a position")]
    [ParadoxNotion.Design.Icon("PathfindingNode")]
    public class GetNearestNodeAction : ActionTask
    {
        [RequiredField]
        public BBParameter<Vector3> Position;

        [BlackboardOnly]
        public BBParameter<GraphNode> NearestNode;

        protected override void OnExecute()
        {
            var closestResult = AstarPath.active.GetNearest(Position.value);
            if (closestResult.node == null)
            {
                EndAction(false);
                return;
            }

            NearestNode.value = closestResult.node;
            EndAction(true);
        }
    }
}