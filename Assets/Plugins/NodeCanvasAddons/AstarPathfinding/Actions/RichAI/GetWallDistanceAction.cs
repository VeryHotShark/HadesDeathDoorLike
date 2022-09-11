using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Get Wall Distance")]
    [Description("Gets the wall distance from the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetWallDistanceAction : ActionTask<RichAI>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<float> WallDistance;

        [GetFromAgent]
        private RichAI _richAI = default;

        protected override void OnExecute()
        {
            WallDistance.value = _richAI.wallDist;
            EndAction(true);
        }
    }
}