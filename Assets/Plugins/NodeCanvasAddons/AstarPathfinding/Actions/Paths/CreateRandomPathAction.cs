using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding Pro")]
    [Name("Create Random Path")]
    [Description("Creates a random from the agents position")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class CreateRandomPathAction : ActionTask<Transform>
    {
        [RequiredField]
        public BBParameter<int> SearchLength;

        [BlackboardOnly]
        public BBParameter<Path> OutputPath;

        protected override string info
        {
            get { return string.Format("Creating random path\n as {0}", OutputPath); }
        }

        protected override void OnExecute()
        {
            var currentPath = RandomPath.Construct(agent.transform.position, SearchLength.value, PathFinishedDeletate);
            AstarPath.StartPath(currentPath);
        }

        private void PathFinishedDeletate(Path path)
        {
            OutputPath.value = path;
            EndAction(!path.error);
        }
    }
}