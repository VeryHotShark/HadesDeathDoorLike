using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    /*
        AStar team recommend you do not use this method however it
        still exists but will throw warnings, if you still need this
        functionality feel free to uncomment the class.
     */
    
    /*
    [Category("A* Pathfinding/Graphs")]
    [Name("Flood Fill Graphs")]
    [Description("Flood fills graphs forcing updates to propogate")]
    [ParadoxNotion.Design.Icon("PathfindingGraph")]
    public class FloodFillGraphsAction : ActionTask
    {
        [BlackboardOnly]
        public BBParameter<GraphNode> StartingNode;

        public BBParameter<int> Area;
    
        protected override void OnExecute()
        {
            if (StartingNode.isNull || StartingNode.isNone)
            { AstarPath.active.FloodFill(); }
            else if (Area.isNone)
            { AstarPath.active.FloodFill(StartingNode.value); }
            else
            { AstarPath.active.FloodFill(StartingNode.value, (uint)Area.value); }

            EndAction(true);
        }
    }*/
}