using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding")]
    [Name("Node Contains Connection")]
    [Description("Checks to see if a node contains a given connection")]
    [ParadoxNotion.Design.Icon("PathfindingNode")]
    public class CheckNodeContainsConnectionCondition : ConditionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<GraphNode> NodeToCheck;

        [RequiredField]
        [BlackboardOnly]
        public BBParameter<GraphNode> ConnectingNode;

        protected override string info
        {
            get { return string.Format("Is Node {0}\nConnected To {1}", NodeToCheck, ConnectingNode); }
        }

        protected override bool OnCheck()
        {
            return NodeToCheck.value.ContainsConnection(ConnectingNode.value);
        }
    }
}