using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding")]
    [Name("Is Node Walkable")]
    [Description("Checks to see if a node is walkable")]
    [ParadoxNotion.Design.Icon("PathfindingNode")]
    public class CheckNodePenaltyPredicateCondition : ConditionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<GraphNode> NodeToCheck;

        [RequiredField]
        public PredicateType PredicateType = PredicateType.EqualTo;

        [RequiredField]
        public BBParameter<int> ComparisonValue;

        protected override bool OnCheck()
        {
            var penalty = NodeToCheck.value.Penalty;
            switch (PredicateType)
            {
                case PredicateType.EqualTo: { return penalty == ComparisonValue.value; }
                case PredicateType.GreaterThan: { return penalty > ComparisonValue.value; }
                case PredicateType.GreaterThanOrEqualTo: { return penalty >= ComparisonValue.value; }
                case PredicateType.LessThan: { return penalty < ComparisonValue.value; }
                case PredicateType.LessThanOrEqualTo: { return penalty <= ComparisonValue.value; }
            }
            return false;
        }
    }
}