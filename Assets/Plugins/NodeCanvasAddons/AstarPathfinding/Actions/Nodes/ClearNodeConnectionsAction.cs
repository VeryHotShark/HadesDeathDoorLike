using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Nodes")]
    [Name("Clear Nodes Connections")]
    [Description("Clears all nodes connections")]
    [ParadoxNotion.Design.Icon("PathfindingNode")]
    public class ClearNodeConnectionsAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<GraphNode> Node;

        public BBParameter<bool> ClearReverse = new BBParameter<bool> {value = true};

        protected override void OnExecute()
        {
            Node.value.ClearConnections(ClearReverse.value);
            EndAction(true);
        }
    }
}