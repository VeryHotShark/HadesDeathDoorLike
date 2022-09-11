using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Get End Reached Distance")]
    [Description("Gets the end reached distance from the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetEndReachedDistanceAction : ActionTask<RichAI>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<float> EndReachedDistance;

        [GetFromAgent]
        private RichAI _richAI = default;

        protected override void OnExecute()
        {
            EndReachedDistance.value = _richAI.endReachedDistance;
            EndAction(true);
        }
    }
}