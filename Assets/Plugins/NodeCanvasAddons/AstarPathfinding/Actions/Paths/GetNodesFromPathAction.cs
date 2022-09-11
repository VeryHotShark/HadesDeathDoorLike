using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Paths")]
    [Name("Get Nodes From Path")]
    [Description("Get nodes from an existing path")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetNodesFromPathAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Path> Path;

        [BlackboardOnly]
        public BBParameter<List<GraphNode>> NodeList = new BBParameter<List<GraphNode>>();

        protected override void OnExecute()
        {
            if (Path.isNone || Path.isNull)
            { EndAction(false); }

            NodeList.value = Path.value.path;
            EndAction(true);
        }
    }
}