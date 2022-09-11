using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Nodes")]
    [Name("Get Node Connections")]
    [Description("Gets all connections for a node")]
    [ParadoxNotion.Design.Icon("PathfindingNode")]
    public class GetNodeConnectionsAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<GraphNode> Node;
    
        [BlackboardOnly]
        public BBParameter<List<GraphNode>> ConnectionList = new BBParameter<List<GraphNode>>();

        protected override void OnExecute()
        {
            Node.value.GetConnections(connectedNode => ConnectionList.value.Add(connectedNode));
            EndAction(true);
        }
    }
}