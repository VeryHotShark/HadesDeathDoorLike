using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Get Slow When Not Facing Target")]
    [Description("Gets the slow when not facing target state from the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetSlowWhenNotFacingTargetAction : ActionTask<RichAI>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<bool> IsSlowWhenNotFacingTarget;

        [GetFromAgent]
        private RichAI _richAI = default;

        protected override void OnExecute()
        {
            IsSlowWhenNotFacingTarget.value = _richAI.slowWhenNotFacingTarget;
            EndAction(true);
        }
    }
}