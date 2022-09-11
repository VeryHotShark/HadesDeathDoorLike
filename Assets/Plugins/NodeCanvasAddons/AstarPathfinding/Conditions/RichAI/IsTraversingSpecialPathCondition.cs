using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Is Traversing Special Path")]
    [Description("Checks to see if a rich AI agent is traversing a special path")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class IsTraversingSpecialPathCondition : ConditionTask<RichAI>
    {
        [GetFromAgent]
        private RichAI _richAI = default;

        protected override string info
        {
            get { return "Is Rich AI Traversing Special Path"; }
        }

        protected override bool OnCheck()
        {
            return _richAI.traversingOffMeshLink;
        }
    }
}