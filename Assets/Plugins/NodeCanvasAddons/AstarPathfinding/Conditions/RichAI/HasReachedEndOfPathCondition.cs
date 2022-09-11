using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Has Reached End Of Path")]
    [Description("Checks to see if a rich AI agent is reporting reaching end of the path")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class HasReachedEndOfPathCondition : ConditionTask<RichAI>
    {
        [GetFromAgent]
        private RichAI _richAI = default;

        protected override string info
        {
            get { return "Has Rich AI Reached End Of Path"; }
        }

        protected override bool OnCheck()
        {
            return _richAI.reachedEndOfPath;
        }
    }
}