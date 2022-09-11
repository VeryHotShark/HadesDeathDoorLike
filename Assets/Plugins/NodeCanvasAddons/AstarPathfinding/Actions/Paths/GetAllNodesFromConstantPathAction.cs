using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding Pro")]
    [Name("Get All Nodes From Constant Path")]
    [Description("Get all nodes from a constant path")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetAllNodesFromConstantPathAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<ConstantPath> Path;

        [BlackboardOnly]
        public BBParameter<List<GraphNode>> NodeList = new BBParameter<List<GraphNode>>();

        protected override void OnExecute()
        {
            if (Path.isNone || Path.isNull)
            { EndAction(false); }

            NodeList.value = Path.value.allNodes;
            EndAction(true);
        }
    }
}