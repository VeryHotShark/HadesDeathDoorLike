using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding.RVO;

namespace NodeCanvasAddons.AStarPathfinding.LocalAvoidance
{
    [Category("A* Pathfinding Pro/RVOController")]
    [Name("Get Mask")]
    [Description("Gets the max neighbour for the RVOController")]
    [ParadoxNotion.Design.Icon("PathfindingNWaypoint")]
    public class GetMaxNeighboursAction : ActionTask<RVOController>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<int> MaxNeighbours;

        [GetFromAgent]
        private RVOController _rvoController = default;

        protected override void OnExecute()
        {
            MaxNeighbours.value = _rvoController.maxNeighbours;
            EndAction(true);
        }
    }
}