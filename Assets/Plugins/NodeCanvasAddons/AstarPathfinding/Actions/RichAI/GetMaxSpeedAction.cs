using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Get Max Speed")]
    [Description("Gets the max speed from the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetMaxSpeedAction : ActionTask<RichAI>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<float> MaxSpeed;

        [GetFromAgent]
        private RichAI _richAI = default;

        protected override void OnExecute()
        {
            MaxSpeed.value = _richAI.maxSpeed;
            EndAction(true);
        }
    }
}