using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Get Wall Force")]
    [Description("Gets the wall force from the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetWallForceAction : ActionTask<RichAI>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<float> WallForce;

        [GetFromAgent]
        private RichAI _richAI = default;

        protected override void OnExecute()
        {
            WallForce.value = _richAI.wallForce;
            EndAction(true);
        }
    }
}