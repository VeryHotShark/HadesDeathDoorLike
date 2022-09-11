using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Graphs")]
    [Name("Flush Graph Updates")]
    [Description("Flushes all graph updates")]
    [ParadoxNotion.Design.Icon("PathfindingUpdate")]
    public class FlushGraphUpdatesAction : ActionTask
    {
        protected override void OnExecute()
        {
            AstarPath.active.FlushGraphUpdates();
            EndAction(true);
        }
    }
}