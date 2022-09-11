using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Get Slowdown Time")]
    [Description("Gets the slowdown time from the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetSlowdownTimeAction : ActionTask<RichAI>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<float> SlowdownTime;

        [GetFromAgent]
        private RichAI _richAI = default;

        protected override void OnExecute()
        {
            SlowdownTime.value = _richAI.slowdownTime;
            EndAction(true);
        }
    }
}