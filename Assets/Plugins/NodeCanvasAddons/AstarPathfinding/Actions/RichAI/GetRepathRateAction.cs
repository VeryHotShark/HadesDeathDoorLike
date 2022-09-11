using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Get Repath Rate")]
    [Description("Gets the repath rate from the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetRepathRateAction : ActionTask<RichAI>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<float> RepathRate;

        [GetFromAgent]
        private RichAI _richAI = default;

        protected override void OnExecute()
        {
            RepathRate.value = _richAI.repathRate;
            EndAction(true);
        }
    }
}