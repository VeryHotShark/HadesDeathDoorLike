using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding")]
    [Name("Set Heuristic")]
    [Description("Sets the heuristic to use when calculating paths")]
    [ParadoxNotion.Design.Icon("PathfindingGraph")]
    public class SetHeuristicAction : ActionTask
    {
        [RequiredField]
        public Heuristic Heuristic = Heuristic.Euclidean;

        protected override void OnExecute()
        {
            AstarPath.active.heuristic = Heuristic;
            EndAction(true);
        }
    }
}