using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding.RVO;

namespace NodeCanvasAddons.AStarPathfinding.LocalAvoidance
{
    [Category("A* Pathfinding Pro/RVOController")]
    [Name("Set Radius")]
    [Description("Sets the radius for the RVOController")]
    [ParadoxNotion.Design.Icon("PathfindingNWaypoint")]
    public class SetRadiusAction : ActionTask<RVOController>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<float> Radius;

        [GetFromAgent]
        private RVOController _rvoController = default;

        protected override void OnExecute()
        {
            _rvoController.radius = Radius.value;
            EndAction(true);
        }
    }
}