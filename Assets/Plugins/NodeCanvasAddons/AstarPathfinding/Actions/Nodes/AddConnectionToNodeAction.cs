using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Nodes")]
    [Name("Add Connection To Node")]
    [Description("Adds a connection to the node with an associated cost (must be positive)")]
    [ParadoxNotion.Design.Icon("PathfindingNode")]
    public class AddConnectionToNodeAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<GraphNode> Node;

        [RequiredField]
        [BlackboardOnly]
        public BBParameter<GraphNode> ConnectingNode;

        [BlackboardOnly]
        public BBParameter<int> ConnectionCost;

        protected override void OnExecute()
        {
            Node.value.AddConnection(ConnectingNode.value, (uint)ConnectionCost.value);
            EndAction(true);
        }
    }
}