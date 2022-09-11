using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Has Reached Destination")]
    [Description("Checks to see if a rich AI agent is reporting destination being reached")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class HasReachedDestinationCondition : ConditionTask<RichAI>
    {
        [GetFromAgent]
        private RichAI _richAI = default;

        protected override string info
        {
            get { return "Has Rich AI Reached Destination"; }
        }

        protected override bool OnCheck()
        {
            return _richAI.reachedDestination;
        }
    }
}