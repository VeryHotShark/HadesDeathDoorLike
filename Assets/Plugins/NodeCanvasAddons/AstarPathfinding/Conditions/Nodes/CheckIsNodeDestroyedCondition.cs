using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding")]
    [Name("Is Node Destroyed")]
    [Description("Checks to see if a node is walkable")]
    [ParadoxNotion.Design.Icon("PathfindingNode")]
    public class CheckIsNodeDestroyedCondition : ConditionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<GraphNode> NodeToCheck;

        protected override string info
        {
            get { return string.Format("{0} Is Destroyed", NodeToCheck); }
        }

        protected override bool OnCheck()
        {
            return NodeToCheck.value.Destroyed;
        }
    }
}