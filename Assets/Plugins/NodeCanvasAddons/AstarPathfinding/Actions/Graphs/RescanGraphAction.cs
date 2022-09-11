using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Graphs")]
    [Name("Rescan Graph")]
    [Description("Rescans the graph to see if any changes have occurred")]
    [ParadoxNotion.Design.Icon("PathfindingRefresh")]
    public class RescanGraphAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<NavGraph> Graph;

        protected override string info
        {
            get { return string.Format("Rescan Graph {0}", Graph); }
        }

        protected override void OnExecute()
        {
            if (Graph.isNone || Graph.isNull)
            { EndAction(false); }

            Graph.value.Scan();
            EndAction(true);
        }
    }
}