using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Set Rotation Speed")]
    [Description("Sets the rotation speed from the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetRotationSpeedAction : ActionTask<RichAI>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<float> RotationSpeed;

        [GetFromAgent]
        private RichAI _richAI = default;

        protected override void OnExecute()
        {
            _richAI.rotationSpeed = RotationSpeed.value;
            EndAction(true);
        }
    }
}