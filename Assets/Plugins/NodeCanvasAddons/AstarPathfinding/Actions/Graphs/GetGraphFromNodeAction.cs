using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Graphs")]
    [Name("Get Graph From Node")]
    [Description("Gets a graph from a node")]
    [ParadoxNotion.Design.Icon("PathfindingGraph")]
    public class GetGraphFromNodeAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<GraphNode> Node;

        [BlackboardOnly]
        public BBParameter<NavGraph> LocatedGraph = new BBParameter<NavGraph>();

        protected override void OnExecute()
        {
            LocatedGraph.value = AstarPath.active.graphs[Node.value.GraphIndex];
            EndAction(true);
        }
    }
}