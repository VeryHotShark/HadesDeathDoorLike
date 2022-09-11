using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding Pro")]
    [Name("Create Flood Path")]
    [Description("Creates a flood path for subsequent lookups")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class CreateFloodPathAction : ActionTask<Transform>
    {
        [BlackboardOnly] public BBParameter<FloodPath> OutputPath;

        protected override string info
        {
            get { return string.Format("Creating flood path \nas {0}", OutputPath); }
        }

        protected override void OnExecute()
        {
            var currentPath = FloodPath.Construct(agent.transform.position, PathFinishedDelegate);
            AstarPath.StartPath(currentPath);
        }

        private void PathFinishedDelegate(Path path)
        {
            OutputPath.value = (FloodPath)path;
            EndAction(!path.error);
        }
    }
}