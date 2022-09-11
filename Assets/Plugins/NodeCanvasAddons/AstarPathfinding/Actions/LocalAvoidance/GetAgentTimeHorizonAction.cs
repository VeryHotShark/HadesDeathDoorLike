using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding.RVO;

namespace NodeCanvasAddons.AStarPathfinding.LocalAvoidance
{
    [Category("A* Pathfinding Pro/RVOController")]
    [Name("Get Agent Time Horizon")]
    [Description("Gets the agent time horizon for the RVOController")]
    [ParadoxNotion.Design.Icon("PathfindingNWaypoint")]
    public class GetAgentTimeHorizonAction : ActionTask<RVOController>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<float> AgentTimeHorizon;

        [GetFromAgent]
        private RVOController _rvoController = default;

        protected override void OnExecute()
        {
            AgentTimeHorizon.value = _rvoController.agentTimeHorizon;
            EndAction(true);
        }
    }
}