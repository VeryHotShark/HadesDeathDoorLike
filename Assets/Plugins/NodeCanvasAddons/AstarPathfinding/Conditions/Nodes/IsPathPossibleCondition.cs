using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding")]
    [Name("Is Path Possible")]
    [Description("Checks to see if a path is possible between 2 positions")]
    [ParadoxNotion.Design.Icon("PathfindingNode")]
    public class IsPathPossibleCondition : ConditionTask
    {
        [RequiredField]
        public BBParameter<Vector3> StartPoint;

        [RequiredField]
        public BBParameter<Vector3> EndPoint;

        public BBParameter<float> MaximumDistanceFromPoint;

        protected override string info
        {
            get { return string.Format("Is Path Possible \nFrom {0}\nTo {1}", StartPoint, EndPoint); }
        }

        protected override bool OnCheck()
        {
            var startNode = AstarPath.active.GetNearest(StartPoint.value).node;
            if (MaximumDistanceFromPoint.value > 0)
            {
                var startDistance = Vector3.Distance((Vector3) startNode.position, StartPoint.value); 
                if(startDistance > MaximumDistanceFromPoint.value)
                { return false; }
            }

            var destinationNode = AstarPath.active.GetNearest(EndPoint.value).node;
            if (MaximumDistanceFromPoint.value > 0)
            {
                var endDistance = Vector3.Distance((Vector3)destinationNode.position, EndPoint.value);
                if (endDistance > MaximumDistanceFromPoint.value)
                { return false; }
            }

            return PathUtilities.IsPathPossible(startNode, destinationNode);
        }
    }
}