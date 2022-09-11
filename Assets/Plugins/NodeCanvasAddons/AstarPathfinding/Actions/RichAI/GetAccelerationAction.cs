using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Get Acceleration")]
    [Description("Gets the acceleration from the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetAccelerationAction : ActionTask<RichAI>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<float> Acceleration;

        [GetFromAgent]
        private RichAI _richAI = default;

        protected override void OnExecute()
        {
            Acceleration.value = _richAI.acceleration;
            EndAction(true);
        }
    }
}