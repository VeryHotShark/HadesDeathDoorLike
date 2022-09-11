using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Nodes")]
    [Name("Get Nodes Position")]
    [Description("Gets the underlying position of a Node")]
    [ParadoxNotion.Design.Icon("PathfindingNode")]
    public class GetNodesPositionAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<GraphNode> Node;

        [BlackboardOnly]
        public BBParameter<Vector3> Position = new BBParameter<Vector3>();

        protected override void OnExecute()
        {
            Position.value = (Vector3)Node.value.position;
            EndAction(true);
        }
    }
}