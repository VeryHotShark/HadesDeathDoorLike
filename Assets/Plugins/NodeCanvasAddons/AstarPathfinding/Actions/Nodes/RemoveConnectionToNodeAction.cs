using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Nodes")]
    [Name("Add Connection To Node")]
    [Description("Removes a connected node")]
    [ParadoxNotion.Design.Icon("PathfindingNode")]
    public class RemoveConnectionToNodeAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<GraphNode> Node;

        [RequiredField]
        [BlackboardOnly]
        public BBParameter<GraphNode> ConnectedNode;

        protected override void OnExecute()
        {
            Node.value.RemoveConnection(ConnectedNode.value);
            EndAction(true);
        }
    }
}