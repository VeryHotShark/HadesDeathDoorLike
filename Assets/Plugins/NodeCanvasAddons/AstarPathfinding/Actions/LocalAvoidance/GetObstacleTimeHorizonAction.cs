using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding.RVO;

namespace NodeCanvasAddons.AStarPathfinding.LocalAvoidance
{
    [Category("A* Pathfinding Pro/RVOController")]
    [Name("Get Obstacle Time Horizon")]
    [Description("Gets the obstacle time horizon for the RVOController")]
    [ParadoxNotion.Design.Icon("PathfindingNWaypoint")]
    public class GetObstacleTimeHorizonAction : ActionTask<RVOController>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<float> ObstacleTimeHorizon;

        [GetFromAgent]
        private RVOController _rvoController = default;

        protected override void OnExecute()
        {
            ObstacleTimeHorizon.value = _rvoController.obstacleTimeHorizon;
            EndAction(true);
        }
    }
}