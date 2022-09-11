using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons
{
    [Category("A* Pathfinding/Graphs")]
    [Name("Get Graph By Index")]
    [Description("Gets a graph in the scene by index")]
    [ParadoxNotion.Design.Icon("PathfindingGraph")]
    public class GetGraphByIndexAction : ActionTask
    {
        [RequiredField]
        public BBParameter<int> GraphIndex;

        [BlackboardOnly]
        public BBParameter<NavGraph> LocatedGraph = new BBParameter<NavGraph>();

        protected override void OnExecute()
        {
            if(AstarPath.active.graphs.Length <= GraphIndex.value)
            { EndAction(false); }

            LocatedGraph.value = AstarPath.active.graphs[GraphIndex.value];
            EndAction(true);
        }
    }
}