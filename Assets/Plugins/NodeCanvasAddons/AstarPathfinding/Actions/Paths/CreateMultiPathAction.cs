using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding Pro")]
    [Name("Create Multi Path")]
    [Description("Creates a path from the current agent to the destination point")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class CreateMultiPathAction : ActionTask
    {
        [RequiredField]
        public BBParameter<List<Vector3>> TargetDestinations;

        [BlackboardOnly]
        public BBParameter<MultiTargetPath> OutputPath;

        protected override string info
        {
            get { return string.Format("Creating multi path \nTo targets {0} \nAs {1}", TargetDestinations, OutputPath); }
        }

        protected override void OnExecute()
        {
            var currentPath = MultiTargetPath.Construct(agent.transform.position, TargetDestinations.value.ToArray(), null, PathFinishedDelegate);
            AstarPath.StartPath(currentPath);
        }

        private void PathFinishedDelegate(Path path)
        {
            OutputPath.value = (MultiTargetPath)path;
            EndAction(!path.error);
        }
    }
}