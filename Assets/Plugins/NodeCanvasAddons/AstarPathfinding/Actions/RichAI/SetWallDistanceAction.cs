using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Set Wall Distance")]
    [Description("Sets the wall distance from the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetWallDistanceAction : ActionTask<RichAI>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<float> WallDistance;

        [GetFromAgent]
        private RichAI _richAI = default;

        protected override void OnExecute()
        {
            _richAI.wallDist = WallDistance.value;
            EndAction(true);
        }
    }
}