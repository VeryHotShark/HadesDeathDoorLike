using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding.RVO;

namespace NodeCanvasAddons.AStarPathfinding.LocalAvoidance
{
    [Category("A* Pathfinding Pro/RVOController")]
    [Name("Get Movement Plane")]
    [Description("Gets the movement plane for the RVOController")]
    [ParadoxNotion.Design.Icon("PathfindingNWaypoint")]
    public class GetMovementPlaneAction : ActionTask<RVOController>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<MovementPlane> MovementPlane;

        [GetFromAgent]
        private RVOController _rvoController = default;

        protected override void OnExecute()
        {
            MovementPlane.value = _rvoController.movementPlaneMode;
            EndAction(true);
        }
    }
}