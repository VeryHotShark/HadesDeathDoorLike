using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding Pro")]
    [Name("Create Constant Path")]
    [Description("Creates a constant path for use")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class CreateConstantPathAction : ActionTask<Transform>
    {
        [RequiredField]
        public BBParameter<int> MaxScore;

        [BlackboardOnly]
        public BBParameter<ConstantPath> OutputPath;
        
        protected override string info
        {
            get { return string.Format("Creating constant path \nas {0}", OutputPath); }
        }

        protected override void OnExecute()
        {
            var currentPath = ConstantPath.Construct(agent.transform.position, MaxScore.value, PathFinishedDelegate);
            AstarPath.StartPath(currentPath);
        }

        private void PathFinishedDelegate(Path path)
        {
            OutputPath.value = (ConstantPath)path;
            EndAction(!path.error);
        }
    }
}