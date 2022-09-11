using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Set Slow When Not Facing Target")]
    [Description("Sets the slow when not facing target state from the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetSlowWhenNotFacingTargetAction : ActionTask<RichAI>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<bool> IsSlowWhenNotFacingTarget;

        [GetFromAgent]
        private RichAI _richAI = default;

        protected override void OnExecute()
        {
            _richAI.slowWhenNotFacingTarget = IsSlowWhenNotFacingTarget.value;
            EndAction(true);
        }
    }
}