using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Set Slowdown Time")]
    [Description("Sets the slowdown time from the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetSlowdownTimeAction : ActionTask<RichAI>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<float> SlowdownTime;

        [GetFromAgent]
        private RichAI _richAI = default;

        protected override void OnExecute()
        {
            _richAI.slowdownTime = SlowdownTime.value;
            EndAction(true);
        }
    }
}