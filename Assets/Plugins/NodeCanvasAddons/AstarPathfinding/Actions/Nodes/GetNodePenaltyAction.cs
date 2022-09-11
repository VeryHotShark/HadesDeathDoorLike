using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Nodes")]
    [Name("Get Node Penalty")]
    [Description("Gets the penalty for a node")]
    [ParadoxNotion.Design.Icon("PathfindingNode")]
    public class GetNodePenaltyAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<GraphNode> NodeToCheck;

        [BlackboardOnly]
        public BBParameter<int> Penalty;

        protected override void OnExecute()
        {
            Penalty.value = (int)NodeToCheck.value.Penalty;
            EndAction(true);
        }
    }
}