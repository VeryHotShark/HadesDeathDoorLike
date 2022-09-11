using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Nodes")]
    [Name("Get Node Walkability")]
    [Description("Gets a boolean indicating if the path is walkable")]
    [ParadoxNotion.Design.Icon("PathfindingNode")]
    public class GetNodeWalkabilityAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<GraphNode> NodeToCheck;

        [BlackboardOnly]
        public BBParameter<bool> Walkability;

        protected override void OnExecute()
        {
            Walkability.value = NodeToCheck.value.Walkable;
            EndAction(true);
        }
    }
}