using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding.RVO;

namespace NodeCanvasAddons.AStarPathfinding.LocalAvoidance
{
    [Category("A* Pathfinding Pro/RVOController")]
    [Name("Set Height")]
    [Description("Sets the height for the RVOController")]
    [ParadoxNotion.Design.Icon("PathfindingNWaypoint")]
    public class SetHeightAction : ActionTask<RVOController>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<float> Height;

        [GetFromAgent]
        private RVOController _rvoController = default;

        protected override void OnExecute()
        {
            _rvoController.height = Height.value;
            EndAction(true);
        }
    }
}