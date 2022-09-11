using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Is Approaching Path Endpoint")]
    [Description("Checks to see if a rich AI agent is approaching path endpoint")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class IsApproachingPathEndpointCondition : ConditionTask<RichAI>
    {
        [GetFromAgent]
        private RichAI _richAI = default;

        protected override string info
        {
            get { return "Is Rich AI Approaching Path Endpoint"; }
        }

        protected override bool OnCheck()
        {
            return _richAI.approachingPathEndpoint;
        }
    }
}