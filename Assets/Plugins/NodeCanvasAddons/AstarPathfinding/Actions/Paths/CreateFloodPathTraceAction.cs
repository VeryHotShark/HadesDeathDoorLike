using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding Pro")]
    [Name("Create Flood Path Trace")]
    [Description("Creates a path from the trace start to the flood path start")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class CreateFloodPathTraceAction : ActionTask<Transform>
    {
        [RequiredField]
        public BBParameter<Vector3> TraceStart;

        [RequiredField]
        [BlackboardOnly]
        public BBParameter<FloodPath> FloodPath;

        [BlackboardOnly]
        public BBParameter<Path> OutputPath = new BBParameter<Path>();

        protected override string info
        {
            get { return string.Format("Creating trace \non path {0} \nfrom {1} \nas {2}", FloodPath, TraceStart, OutputPath); }
        }

        protected override void OnExecute()
        {
            var path = FloodPathTracer.Construct(TraceStart.value, FloodPath.value, PathFinishedDelegate);
            AstarPath.StartPath(path);
        }

        private void PathFinishedDelegate(Path path)
        {
            OutputPath.value = path;
            EndAction(!path.error);
        }
    }
}