using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding")]
    [Name("Set Heuristic Scale")]
    [Description("Sets the heuristic scale to use when calculating paths")]
    [ParadoxNotion.Design.Icon("PathfindingGraph")]
    public class SetHeuristicScaleAction : ActionTask
    {
        [RequiredField]
        public BBParameter<float> HeuristicScale;

        protected override void OnExecute()
        {
            AstarPath.active.heuristicScale = HeuristicScale.value;
            EndAction(true);
        }
    }
}