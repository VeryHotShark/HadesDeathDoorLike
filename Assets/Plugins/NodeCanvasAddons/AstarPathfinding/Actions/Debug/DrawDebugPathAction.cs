using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Paths")]
    [Name("Draw Debug Path")]
    [Description("Draws a debug line for the path in the scene")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class DrawDebugPathAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Path> Path;

        public BBParameter<Color> PathColor = new BBParameter<Color> {value = Color.green};
        public BBParameter<float> TimeToShow = new BBParameter<float> { value = 0.1f };

        protected override string info
        {
            get { return string.Format("Drawing Path {0}", Path); }
        }

        protected override void OnExecute()
        {
            var vectorPath = Path.value.vectorPath;
            for (var i = 1; i < vectorPath.Count; i++)
            {
                Debug.DrawLine(vectorPath[i - 1], vectorPath[i], PathColor.value, TimeToShow.value);
            }
            EndAction(true);
        }
    }
}
