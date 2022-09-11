using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Paths")]
    [Name("Get Node From Path By Index")]
    [Description("Gets a node from a path by index")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetNodeFromPathByIndexAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Path> Path;

        [RequiredField]
        public BBParameter<int> Index;

        [BlackboardOnly]
        public BBParameter<GraphNode> Node = new BBParameter<GraphNode>();

        protected override void OnExecute()
        {
            if (Path.isNone || Path.isNull)
            { EndAction(false); }

            if(Path.value.path.Count >= Index.value)
            { EndAction(false); }

            Node.value = Path.value.path[Index.value];
            EndAction(true);
        }
    }
}