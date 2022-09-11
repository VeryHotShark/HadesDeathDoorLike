using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Graphs")]
    [Name("Rescan Graphs")]
    [Description("Rescans all graphs in the scene")]
    [ParadoxNotion.Design.Icon("PathfindingRefresh")]
    public class RescanGraphsAction : ActionTask
    {
        protected override void OnExecute()
        {
            AstarPath.active.Scan();
            EndAction(true);
        }
    }
}