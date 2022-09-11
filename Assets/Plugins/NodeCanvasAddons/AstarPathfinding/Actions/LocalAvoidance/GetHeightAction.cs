using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding.RVO;

namespace NodeCanvasAddons.AStarPathfinding.LocalAvoidance
{
    [Category("A* Pathfinding Pro/RVOController")]
    [Name("Get Height")]
    [Description("Gets the height for the RVOController")]
    [ParadoxNotion.Design.Icon("PathfindingNWaypoint")]
    public class GetHeightAction : ActionTask<RVOController>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<float> Height;

        [GetFromAgent]
        private RVOController _rvoController = default;

        protected override void OnExecute()
        {
            Height.value = _rvoController.height;
            EndAction(true);
        }
    }
}