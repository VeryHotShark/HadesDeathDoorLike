using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding")]
    [Name("Is Position Walkable")]
    [Description("Checks to see if a position is walkable")]
    [ParadoxNotion.Design.Icon("PathfindingNode")]
    public class CheckIsPositionWalkableCondition : ConditionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Vector3> PositionToCheck;

        protected override string info
        {
            get { return string.Format("{0} Is Walkable", PositionToCheck); }
        }

        protected override bool OnCheck()
        {
            return AstarPath.active.graphs.Any(isGraphWalkable);
        }

        private bool isGraphWalkable(NavGraph graph)
        {
            var nearestNode = graph.GetNearest(PositionToCheck.value);
            return nearestNode.node != null && nearestNode.node.Walkable;
        }
    }
}