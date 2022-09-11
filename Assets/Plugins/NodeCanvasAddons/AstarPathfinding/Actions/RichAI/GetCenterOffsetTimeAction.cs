using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Get Center Offset")]
    [Description("Gets the center offset from the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetCenterOffsetTimeAction : ActionTask<RichAI>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<float> CenterOffset;

        [GetFromAgent]
        private RichAI _richAI = default;

        protected override void OnExecute()
        {
            CenterOffset.value = _richAI.height / 2;
            EndAction(true);
        }
    }
}