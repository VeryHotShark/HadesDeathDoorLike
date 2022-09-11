using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Paths")]
    [Name("Get Distance Of Path")]
    [Description("Gets the distance of a given path")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetDistanceOfPathAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Path> Path;

        [BlackboardOnly]
        public BBParameter<float> TotalDistance = new BBParameter<float>();

        protected override string info
        {
            get { return string.Format("Get {0} distance", Path); }
        }

        protected override void OnExecute()
        {
            if (Path.isNone || Path.isNull)
            { EndAction(false); }

            TotalDistance.value = Path.value.vectorPath.Count > 1 ? GetTotalDistanceOfPath(Path.value) : 0;
            EndAction(true);
        }

        private float GetTotalDistanceOfPath(Path path)
        {
            var totalDistance = 0.0f;
            for (var i = 1; i < path.vectorPath.Count; i++)
            {
                var previousPosition = path.vectorPath[i - 1];
                var nextPosition = path.vectorPath[i];
                totalDistance = Vector3.Distance(previousPosition, nextPosition);
            }
            return totalDistance;
        }
    }
}