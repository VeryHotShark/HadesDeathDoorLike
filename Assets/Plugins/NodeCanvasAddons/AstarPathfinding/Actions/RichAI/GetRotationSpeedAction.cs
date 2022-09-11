using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Get Rotation Speed")]
    [Description("Gets the rotation speed from the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetRotationSpeedAction : ActionTask<RichAI>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<float> RotationSpeed;

        [GetFromAgent]
        private RichAI _richAI = default;

        protected override void OnExecute()
        {
            RotationSpeed.value = _richAI.rotationSpeed;
            EndAction(true);
        }
    }
}