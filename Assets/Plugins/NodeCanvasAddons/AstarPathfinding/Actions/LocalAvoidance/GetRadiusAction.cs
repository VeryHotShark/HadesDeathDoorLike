using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using Pathfinding.RVO;

namespace NodeCanvasAddons.AStarPathfinding.LocalAvoidance
{
    [Category("A* Pathfinding Pro/RVOController")]
    [Name("Get Radius")]
    [Description("Gets the radius for the RVOController")]
    [ParadoxNotion.Design.Icon("PathfindingNWaypoint")]
    public class GetRadiusAction : ActionTask<RVOController>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<float> Radius;

        [GetFromAgent]
        private RVOController _rvoController = default;

        protected override void OnExecute()
        {
            Radius.value = _rvoController.radius;
            EndAction(true);
        }
    }
}