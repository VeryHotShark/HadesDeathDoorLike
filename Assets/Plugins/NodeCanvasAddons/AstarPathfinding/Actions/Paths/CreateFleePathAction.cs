using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding Pro")]
    [Name("Create Flee Path")]
    [Description("Creates a flee path for from an agents position")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class CreateFleePathAction : ActionTask<Transform>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Vector3> AvoidPosition;

        [RequiredField]
        public BBParameter<int> GScoreToStopAt;

        [BlackboardOnly]
        public BBParameter<Path> OutputPath;

        protected override string info
        {
            get { return string.Format("Creating flee path \navoiding {0} \nas {1}", AvoidPosition, OutputPath); }
        }

        protected override void OnExecute()
        {
            var path = FleePath.Construct(agent.transform.position, AvoidPosition.value, GScoreToStopAt.value, PathFinishedDelegate);
            AstarPath.StartPath(path);
        }

        private void PathFinishedDelegate(Path path)
        {
            OutputPath.value = path;
            if(path.error) { Debug.Log(path.error); }

            EndAction(!path.error);
        }
    }
}