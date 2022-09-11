using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding.RVO;

namespace NodeCanvasAddons.AStarPathfinding.LocalAvoidance
{
    [Category("A* Pathfinding Pro/RVOController")]
    [Name("Get Center")]
    [Description("Gets the center for the RVOController")]
    [ParadoxNotion.Design.Icon("PathfindingNWaypoint")]
    public class GetCenterAction : ActionTask<RVOController>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<float> Center;

        [GetFromAgent]
        private RVOController _rvoController = default;

        protected override void OnExecute()
        {
            Center.value = _rvoController.center;
            EndAction(true);
        }
    }
}